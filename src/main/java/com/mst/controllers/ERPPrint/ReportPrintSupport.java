package com.mst.controllers.ERPPrint;

import com.mst.reports.jasper.CR;
import com.mst.reports.jasper.CrystalJasperPrinter;
import com.mst.reports.ReportDataService;
import com.mst.reports.ReportDefinition;
import com.mst.reports.ReportRegistry;
import com.mst.reports.jasper.ReportTemplateService;
import com.mst.reports.prints.ReportPdfService;
import com.mst.security.CurrentUserContext;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.web.bind.annotation.*;
import net.sf.jasperreports.engine.JRParameter;
import net.sf.jasperreports.engine.JasperCompileManager;
import net.sf.jasperreports.engine.JasperExportManager;
import net.sf.jasperreports.engine.JasperFillManager;
import net.sf.jasperreports.engine.JasperPrint;
import net.sf.jasperreports.engine.JasperReport;
import net.sf.jasperreports.engine.data.JRMapCollectionDataSource;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.LinkedHashMap;
import org.springframework.http.*;

/** Shared load, compile, fill and PDF export steps used by module print controllers. */
public abstract class ReportPrintSupport {
    @Autowired protected ReportDataService reportDataService;
    @Autowired protected ReportRegistry reportRegistry;
    @Autowired protected ReportPdfService reportPdfService;
    @Autowired protected ReportTemplateService reportTemplates;
    @Autowired protected CurrentUserContext currentUserContext;
    @Autowired protected ObjectMapper objectMapper;

    protected void printReport(HttpServletResponse response, String template, Map<String, Object> request) throws Exception {
        if (!template.toLowerCase(Locale.ROOT).endsWith(".rpt")) template += ".rpt";
        String key = reportPdfService.keyOf(template);
        if (key == null) {
            printError(response, 404, "No report data source is registered for " + template);
            return;
        }
        printReportKey(response, key, request);
    }

    /** Put breakpoints on result, rows, parameters and jasperPrint to debug any ERP report. */
    @SuppressWarnings("unchecked")
    protected void printReportKey(HttpServletResponse response, String key, Map<String, Object> request) throws Exception {
        // 1. Authenticate and load this report's rows using its existing procedure and filters.
        currentUserContext.currentUserId();
        ReportDefinition definition = reportRegistry.get(key);
        if (definition == null) { printError(response, 404, "Unknown report: " + key); return; }
        Map<String, Object> arguments = new LinkedHashMap<>();
        if (request != null) request.forEach((name, value) -> {
            if (!name.equalsIgnoreCase("organizationId") && !name.equalsIgnoreCase("companyId")) arguments.put(name, value);
        });
        arguments.put("organizationId", currentUserContext.currentOrganizationId());
        arguments.put("companyId", currentUserContext.currentCompanyId());
        arguments.put("clsGlobalVariables", currentUserContext.currentUserId());
        Map<String, Object> result = reportDataService.run(key, arguments);
        printReportData(response, definition.template, result);
    }

    /** Render data loaded by a screen's own report service using the normal Jasper pipeline. */
    @SuppressWarnings("unchecked")
    protected void printReportData(HttpServletResponse response, String template, Map<String, Object> result) throws Exception {
        currentUserContext.currentUserId();
        List<Map<String, Object>> rows = (List<Map<String, Object>>) result.get("rows");
        if (rows == null || rows.isEmpty()) { printError(response, 404, "No Record Found For Display"); return; }

        // 2. Open and compile the report's JRXML (including configured, edited templates).
        ReportTemplateService.Source source = reportTemplates.source(template, rows);
        JasperReport jasperReport;
        try (InputStream input = source.openMain()) {
            jasperReport = JasperCompileManager.compileReport(input);
        }

        // 3. Set company/report parameters and compile any subreports with their own data.
        Map<String, Object> parameters = new HashMap<>();
        parameters.put("CR_VARS", new CR.Vars());
        parameters.put(JRParameter.REPORT_LOCALE, Locale.US);
        Map<String, Object> reportParameters = (Map<String, Object>) result.getOrDefault("reportParameters", Map.of());
        reportParameters.forEach((name, value) -> {
            String parameter = CrystalJasperPrinter.sanitize(name);
            parameters.put(parameter, CrystalJasperPrinter.coerceParam(jasperReport, parameter, value));
        });
        if (source.isGenerated()) {
            try { parameters.put("PrintedBy", currentUserContext.requireAccountingUser().getUserName()); }
            catch (RuntimeException ignored) { /* The report still prints when the display name is unavailable. */ }
        }
        for (ReportTemplateService.Subreport sub : source.subreports()) {
            JasperReport subreport;
            try (InputStream input = source.openSubreport(sub)) {
                subreport = JasperCompileManager.compileReport(input);
            }
            List<Map<String, Object>> subRows = List.of();
            for (Map<String, Object> data : (List<Map<String, Object>>) result.getOrDefault("subReports", List.of())) {
                if (CrystalJasperPrinter.baseName(String.valueOf(data.get("template")))
                        .equalsIgnoreCase(CrystalJasperPrinter.baseName(sub.name()))) {
                    subRows = (List<Map<String, Object>>) data.getOrDefault("rows", List.of());
                    break;
                }
            }
            parameters.put("SUBREPORT_" + sub.key(), subreport);
            parameters.put("SUBDATA_" + sub.key(), CrystalJasperPrinter.normalize(subreport, subRows));
        }

        // 4. Fill the report with the procedure's rows, preserving their order and numeric values.
        JRMapCollectionDataSource dataSource = new JRMapCollectionDataSource(
                new ArrayList<Map<String, ?>>(CrystalJasperPrinter.normalize(jasperReport, rows)));
        JasperPrint jasperPrint = JasperFillManager.fillReport(jasperReport, parameters, dataSource);

        // 5. Export directly to this response. Concurrent print requests never share an output file.
        byte[] pdf = JasperExportManager.exportReportToPdf(jasperPrint);
        String fileName = CrystalJasperPrinter.stem(template) + ".pdf";
        response.setContentType("application/pdf");
        response.setHeader("Content-Disposition", "inline; filename=\"" + fileName + "\"");
        response.setContentLength(pdf.length);
        response.getOutputStream().write(pdf);
    }

    @ExceptionHandler(IllegalArgumentException.class)
    @ResponseBody
    public ResponseEntity<String> invalidPrint(IllegalArgumentException exception) {
        return ResponseEntity.badRequest().contentType(org.springframework.http.MediaType.TEXT_PLAIN).body(exception.getMessage());
    }

    private void printError(HttpServletResponse response, int status, String message) throws IOException {
        response.setStatus(status);
        response.setContentType("text/plain;charset=UTF-8");
        response.getWriter().write(message);
    }

    @ExceptionHandler(AccessDeniedException.class)
    @ResponseBody
    public ResponseEntity<String> deniedPrint(AccessDeniedException exception) {
        return ResponseEntity.status(403).contentType(org.springframework.http.MediaType.TEXT_PLAIN).body(exception.getMessage());
    }


}
