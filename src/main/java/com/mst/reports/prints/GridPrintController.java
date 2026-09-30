package com.mst.reports.prints;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

/**
 * Prints whose desktop source is the SCREEN'S GRID, not a procedure: the desktop builds a DataTable from
 * what the form shows and hands it to the .rpt. Here the page sends those rows and gets the PDF back.
 *
 *   POST /reports/print/grid
 *   { "rpt": "128_PayablesReportInvoiceWise.rpt", "title": "Payables Report Invoice Wise",
 *     "rows": [ { "Party": "...", "Invoice No": "...", "Amount": "1,250.00" }, ... ] }
 *
 * Used by print-rpt.js for buttons marked data-rpt-grid (840, 128, 128-01, 135, 143, 06, 1003_01, 421, 219).
 */
@RestController
public class GridPrintController {

    private final ReportPdfService reportPdfService;

    public GridPrintController(ReportPdfService reportPdfService) {
        this.reportPdfService = reportPdfService;
    }

    @RequestMapping(value = "/reports/print/grid", method = RequestMethod.POST)
    public void printGrid(HttpServletResponse response, @RequestBody GridPrintRequest request) throws IOException {
        reportPdfService.writeGridPdf(response, request.rpt, request.title, request.rows);
    }

    /** Body of POST /reports/print/grid */
    @JsonIgnoreProperties(ignoreUnknown = true)
    public static class GridPrintRequest {
        /** the .rpt the desktop prints this grid with (names the PDF, gives orientation and logo) */
        public String rpt;
        /** printed title; defaults to the title in the .rpt name */
        public String title;
        /** the grid's rows as the page shows them: column header -> cell text */
        public List<Map<String, Object>> rows = new ArrayList<>();
    }
}
