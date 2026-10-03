package com.mst.controllers.ERPPrint;

import com.mst.reports.ReportDefinition;
import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.LinkedHashMap;
import org.springframework.http.*;

/** Common print actions. Existing print URLs are preserved. */
@Controller
public class CommonPrintController extends ReportPrintSupport {


    /** Template-based callers and older saved links enter the same controller as named print actions. */
    @RequestMapping(value = {"/reports/print/by-template/{template}/pdf", "/api/print/by-template/{template}/pdf"}, method = RequestMethod.GET)
    public void printTemplateGet(HttpServletResponse response, @PathVariable String template,
            @RequestParam Map<String, String> query) throws Exception {
        printReport(response, template, new LinkedHashMap<>(query));
    }

    @RequestMapping(value = {"/reports/print/by-template/{template}/pdf", "/api/print/by-template/{template}/pdf"}, method = RequestMethod.POST)
    public void printTemplatePost(HttpServletResponse response, @PathVariable String template,
            @RequestBody(required = false) Map<String, Object> request) throws Exception {
        printReport(response, template, request);
    }

    @RequestMapping(value = {"/reports/print/by-key/{key}", "/api/print/{key}/pdf", "/api/reports/{key}/print.pdf", "/reports/jasper/{key}"}, method = RequestMethod.POST)
    public void printKeyPost(HttpServletResponse response, @PathVariable String key,
            @RequestBody(required = false) Map<String, Object> request) throws Exception {
        printReportKey(response, key, request);
    }

    @RequestMapping(value = {"/reports/print/by-key/{key}", "/api/print/{key}/pdf"}, method = RequestMethod.GET)
    public void printKeyGet(HttpServletResponse response, @PathVariable String key,
            @RequestParam Map<String, String> query) throws Exception {
        printReportKey(response, key, new LinkedHashMap<>(query));
    }

    /** Grid reports use the rows already displayed on the screen, including checked-row selections. */
    @PostMapping("/reports/print/grid")
    public void printGrid(HttpServletResponse response, @RequestBody GridPrintRequest request) throws IOException {
        reportPdfService.writeGridPdf(response, request.rpt, request.title, request.rows);
    }

    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class GridPrintRequest {
        public String rpt;
        public String title;
        public List<Map<String, Object>> rows = new ArrayList<>();
    }

    /** Browser grid-print buttons preserve all visible grids in one PDF. */
    @PostMapping("/reports/print/screen")
    public void printScreen(HttpServletResponse response, @RequestBody List<GridPrintRequest> grids) throws Exception {
        currentUserContext.currentUserId();
        if (grids == null || grids.isEmpty() || grids.stream().allMatch(grid -> grid.rows == null || grid.rows.isEmpty())) {
            response.sendError(404, "No Record Found For Display");
            return;
        }
        java.io.ByteArrayOutputStream output = new java.io.ByteArrayOutputStream();
        com.itextpdf.text.Document document = new com.itextpdf.text.Document();
        com.itextpdf.text.pdf.PdfCopy copy = new com.itextpdf.text.pdf.PdfCopy(document, output);
        document.open();
        try {
            for (GridPrintRequest grid : grids) {
                if (grid.rows == null || grid.rows.isEmpty()) continue;
                com.itextpdf.text.pdf.PdfReader reader = new com.itextpdf.text.pdf.PdfReader(
                        reportPdfService.gridPdf(grid.rpt, grid.title, grid.rows));
                try { copy.addDocument(reader); }
                finally { reader.close(); }
            }
        } finally {
            document.close();
        }
        byte[] pdf = output.toByteArray();
        response.setContentType("application/pdf");
        response.setHeader("Content-Disposition", "inline; filename=\"Screen-Report.pdf\"");
        response.setContentLength(pdf.length);
        response.getOutputStream().write(pdf);
    }

    private volatile Map<String, Object> controlMap;
    private volatile Map<String, String> namedRoutes;

    @GetMapping("/reports/print/controls")
    @ResponseBody
    @SuppressWarnings("unchecked")
    public Map<String, Object> printControls(@RequestParam String rpt) throws IOException {
        currentUserContext.currentUserId();
        if (controlMap == null) {
            try (InputStream in = getClass().getResourceAsStream("/reports/print-controls.json")) {
                controlMap = objectMapper.readValue(in, Map.class);
            }
        }
        if (namedRoutes == null) {
            try (InputStream in = getClass().getResourceAsStream("/reports/print-endpoints.json")) {
                namedRoutes = objectMapper.readValue(in, Map.class);
            }
        }
        String template = rpt.toLowerCase(Locale.ROOT);
        if (!template.endsWith(".rpt")) template += ".rpt";
        Map<String, Object> result = new LinkedHashMap<>();
        Object controls = controlMap.get(template);
        if (controls instanceof Map) result.putAll((Map<String, Object>) controls);
        String key = reportPdfService.keyOf(template);
        ReportDefinition definition = key == null ? null : reportRegistry.get(key);
        List<String> arguments = new ArrayList<>();
        List<String> required = new ArrayList<>();
        if (definition != null) for (ReportDefinition.Param parameter : definition.params) {
            if (parameter.source.startsWith("arg:")) {
                String name = parameter.source.substring(4);
                if (name.equalsIgnoreCase("organizationId") || name.equalsIgnoreCase("companyId")) continue;
                arguments.add(name);
                if (parameter.mode == ReportDefinition.Mode.ALWAYS) required.add(name);
            }
        }
        result.putIfAbsent("args", arguments);
        result.putIfAbsent("controls", Map.of());
        result.put("required", required);
        result.put("key", key);
        result.put("pdf", namedRoutes.getOrDefault(template, key == null ? "" : "/reports/print/by-key/" + key));
        return result;
    }


    // BEGIN GENERATED PRINT ACTIONS
    // END GENERATED PRINT ACTIONS
}
