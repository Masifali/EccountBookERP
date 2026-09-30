package com.mst.controllers;

import com.mst.reports.ReportDataService;
import com.mst.reports.ReportDefinition;
import com.mst.reports.ReportRegistry;
import com.mst.reports.jasper.CrystalJasperPrinter;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;

import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.util.LinkedHashMap;
import java.util.Map;

/**
 * Desktop Crystal prints rendered in Java by their converted Jasper templates.
 *
 * Same shape as the existing Jasper endpoints (/reports/credit_list): load the .jrxml, compile,
 * fill, export PDF, write it to the response. The difference is where the rows come from - the
 * procedure the desktop's BLL runs, through ReportDataService, with OrganizationId/CompanyId taken
 * from the session and never from the request.
 *
 *   POST /reports/sale_order_slip_273       {"id": 123}     273-InvRptSaleOrderSlip.rpt
 *   GET  /reports/sale_order_slip_273?id=123
 *   POST /reports/jasper/{key}              {"id": 123}     any registered report with a converted template
 */
@Controller
public class CrystalJasperPrintController {

    private static final Logger LOG = LoggerFactory.getLogger(CrystalJasperPrintController.class);

    private final ReportDataService reportDataService;
    private final ReportRegistry registry;
    private final CrystalJasperPrinter printer;
    private final CurrentUserContext context;

    public CrystalJasperPrintController(ReportDataService reportDataService, ReportRegistry registry,
                                        CrystalJasperPrinter printer, CurrentUserContext context) {
        this.reportDataService = reportDataService;
        this.registry = registry;
        this.printer = printer;
        this.context = context;
    }

    /** Folder the PDF is written to before it is sent, as the other Jasper prints do. */
    private static final String REPORT_PDF_PATH =
            java.nio.file.Paths.get(System.getProperty("java.io.tmpdir"), "eccount-reports").toString();

    /**
     * Sale Order "273-Print" - CommonServices.SaleOrderReports273(PrintId), written in the same
     * style as /reports/credit_list: load the .jrxml, compile, fill, export to a file, copy it to
     * the response. Body: {"id": <SaleOrder Id>}.
     */
    @RequestMapping(value = "/reports/sale_order_slip_273", method = RequestMethod.POST)
    public String generateSaleOrderSlip273(@RequestBody Map<String, Object> request, HttpServletResponse response)
            throws net.sf.jasperreports.engine.JRException, IOException {

        if (java.nio.file.Files.notExists(java.nio.file.Paths.get(REPORT_PDF_PATH))) {
            java.nio.file.Files.createDirectories(java.nio.file.Paths.get(REPORT_PDF_PATH));
        }
        boolean signedIn;
        try { signedIn = context.currentUserId() > 0; } catch (RuntimeException e) { signedIn = false; }
        if (!signedIn) { fail(response, 403, "Sign in to print reports."); return null; }
        Object id = request == null ? null : request.get("id");
        if (id == null || "0".equals(String.valueOf(id))) { fail(response, 400, "PrintId not found..."); return null; }

        String fileName = "273-SaleOrderSlip-" + id + ".pdf";

        // main template + its sub-report, both converted from 273-InvRptSaleOrderSlip.rpt
        final java.io.InputStream reportInputStream =
                getClass().getResourceAsStream("/jasper/converted/273_InvRptSaleOrderSlip.jrxml");
        final java.io.InputStream subInputStream =
                getClass().getResourceAsStream("/jasper/converted/273_InvRptSaleOrderSlip__SaleOrderCustomerExpense_SubReport.jrxml");
        if (reportInputStream == null || subInputStream == null) {
            fail(response, 503, "273 has not been converted yet - run migration/rpt-to-jasper.");
            return null;
        }
        final net.sf.jasperreports.engine.design.JasperDesign jasperDesign =
                net.sf.jasperreports.engine.xml.JRXmlLoader.load(reportInputStream);
        net.sf.jasperreports.engine.JasperReport jasperReport =
                net.sf.jasperreports.engine.JasperCompileManager.compileReport(jasperDesign);
        net.sf.jasperreports.engine.JasperReport subReport =
                net.sf.jasperreports.engine.JasperCompileManager.compileReport(
                        net.sf.jasperreports.engine.xml.JRXmlLoader.load(subInputStream));

        // desktop: SaleOrderReports.SaleOrderReports273(obj)  -> Sp_SaleOrder_RiceSlip_Rpt
        //          SaleOrderCustomerExpense_SubReport(obj.Id) -> USP_SaleOrderCustomerExpense_SubReport
        Map<String, Object> result = reportDataService.run("so-273", request);
        @SuppressWarnings("unchecked")
        java.util.List<Map<String, Object>> rows = (java.util.List<Map<String, Object>>) result.get("rows");
        if (rows == null || rows.isEmpty()) { fail(response, 404, "No Record Found For Display"); return null; }
        java.util.List<Map<String, Object>> expenseRows = new java.util.ArrayList<>();
        Object subs = result.get("subReports");
        if (subs instanceof java.util.List && !((java.util.List<?>) subs).isEmpty()) {
            @SuppressWarnings("unchecked")
            java.util.List<Map<String, Object>> r = (java.util.List<Map<String, Object>>)
                    ((Map<String, Object>) ((java.util.List<?>) subs).get(0)).get("rows");
            if (r != null) expenseRows = r;
        }

        net.sf.jasperreports.engine.data.JRMapCollectionDataSource dataSource =
                new net.sf.jasperreports.engine.data.JRMapCollectionDataSource(
                        new java.util.ArrayList<Map<String, ?>>(CrystalJasperPrinter.normalize(jasperReport, rows)));

        Map<String, Object> parameters = new java.util.HashMap<>();
        // desktop: val.RptPerameter("@CompanyName", ...) / ("@CompanyAddress", ...)
        @SuppressWarnings("unchecked")
        Map<String, Object> rp = (Map<String, Object>) result.get("reportParameters");
        parameters.put("CompanyName", rp == null ? "" : rp.get("@CompanyName"));
        parameters.put("CompanyAddress", rp == null ? "" : rp.get("@CompanyAddress"));
        parameters.put("CR_VARS", new com.mst.reports.jasper.CR.Vars());
        parameters.put(net.sf.jasperreports.engine.JRParameter.REPORT_LOCALE, java.util.Locale.US);
        // desktop: new SubReportObject(dt, "SaleOrderCustomerExpense_SubReport.rpt")
        parameters.put("SUBREPORT_SaleOrderCustomerExpense_SubReport", subReport);
        parameters.put("SUBDATA_SaleOrderCustomerExpense_SubReport", CrystalJasperPrinter.normalize(subReport, expenseRows));

        net.sf.jasperreports.engine.JasperPrint jasperPrint =
                net.sf.jasperreports.engine.JasperFillManager.fillReport(jasperReport, parameters, dataSource);
        String filePath = REPORT_PDF_PATH;

        net.sf.jasperreports.engine.JasperExportManager.exportReportToPdfFile(
                jasperPrint, java.nio.file.Paths.get(filePath, fileName).toString());
        response.setContentType("application/pdf");
        response.setHeader("Content-Disposition", "inline; filename=\"" + fileName + "\"");

        java.nio.file.Path sourceLocation = java.nio.file.Paths.get(filePath).resolve(fileName).toAbsolutePath().normalize();
        org.springframework.util.FileCopyUtils.copy(java.nio.file.Files.readAllBytes(sourceLocation), response.getOutputStream());
        java.nio.file.Files.deleteIfExists(sourceLocation);
        return null;   // response already written
    }

    @RequestMapping(value = "/reports/sale_order_slip_273", method = RequestMethod.GET)
    public void saleOrderSlip273Get(@RequestParam("id") Integer id, HttpServletResponse response) throws IOException {
        Map<String, Object> body = new LinkedHashMap<>();
        body.put("id", id);
        print("so-273", body, response);
    }

    /** Any registered report whose .rpt has been converted (see migration/rpt-to-jasper). */
    @RequestMapping(value = "/reports/jasper/{key}", method = RequestMethod.POST)
    public void printByKey(@PathVariable("key") String key, @RequestBody(required = false) Map<String, Object> body,
                           HttpServletResponse response) throws IOException {
        print(key, body == null ? new LinkedHashMap<>() : body, response);
    }

    private void print(String key, Map<String, Object> args, HttpServletResponse response) throws IOException {
        try {
            boolean signedIn;
            try { signedIn = context.currentUserId() > 0; } catch (RuntimeException e) { signedIn = false; }
            if (!signedIn) { fail(response, 403, "Sign in to print reports."); return; }

            ReportDefinition def = registry.get(key);
            if (def == null) { fail(response, 404, "Unknown report '" + key + "'."); return; }
            if (!printer.hasTemplate(def.template)) {
                fail(response, 503, "No converted Jasper template for " + def.template
                        + " yet - run migration/rpt-to-jasper for it.");
                return;
            }
            Object id = args.get("id");
            if (id == null || "0".equals(String.valueOf(id))) {          // desktop: "PrintId not found..."
                fail(response, 400, "PrintId not found...");
                return;
            }

            Map<String, Object> result = reportDataService.run(key, args);
            Object rows = result.get("rows");
            if (!(rows instanceof java.util.List) || ((java.util.List<?>) rows).isEmpty()) {
                fail(response, 404, "No Record Found For Display");      // desktop's own message
                return;
            }

            byte[] pdf = printer.renderPdf(result);

            String fileName = CrystalJasperPrinter.stem(def.template) + "-" + id + ".pdf";
            response.setContentType("application/pdf");
            /* inline: the desktop opens a print preview, it does not download a file */
            response.setHeader("Content-Disposition", "inline; filename=\"" + fileName + "\"");
            response.setContentLength(pdf.length);
            response.getOutputStream().write(pdf);
            response.getOutputStream().flush();
        } catch (IllegalArgumentException e) {
            fail(response, 400, e.getMessage());
        } catch (Exception e) {
            LOG.warn("Jasper print of '{}' failed", key, e);
            fail(response, 500, e.getMessage() == null ? "Printing failed." : e.getMessage());
        }
    }

    /** A failure on a PDF endpoint is readable text, never a corrupt PDF. */
    private static void fail(HttpServletResponse response, int status, String message) throws IOException {
        if (response.isCommitted()) return;
        response.reset();
        response.setStatus(status);
        response.setContentType("text/plain;charset=UTF-8");
        response.getOutputStream().write(message.getBytes(StandardCharsets.UTF_8));
    }
}
