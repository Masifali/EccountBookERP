package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.DashboardPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** Dashboard print actions. Generated from the verified seeder contracts. */
@Controller
public class DashboardPrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 121_02_ReceivablesAndReceiptSchedule.rpt
     * Procedure: usp_ReceivablesAndReceiptsSchedule
     * Desktop: VoucherReports.ReceivablesAndReceiptsSchedule
     */
    @RequestMapping(value = "/reports/print/121-02-receivables-and-receipt-schedule", method = RequestMethod.POST)
    public void print12102ReceivablesAndReceiptSchedule(HttpServletResponse response, @RequestBody(required = false) Rpt12102ReceivablesAndReceiptScheduleRequest request) throws Exception {
        if (request == null) request = new Rpt12102ReceivablesAndReceiptScheduleRequest();
        printReport(response, "121_02_ReceivablesAndReceiptSchedule.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/121-02-receivables-and-receipt-schedule", method = RequestMethod.GET)
    public void print12102ReceivablesAndReceiptScheduleGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print12102ReceivablesAndReceiptSchedule(response, objectMapper.convertValue(query, Rpt12102ReceivablesAndReceiptScheduleRequest.class));
    }

    /**
     * Template: 842_ItemAndPMItemMapSlip.rpt
     * Procedure: [MRP].[USP_ItemAndPMItemMap_SlipAndRegister]
     * Desktop: ItemAndPMItemMap.ItemAndPMItemMapSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/842-item-and-pm-item-map-slip", method = RequestMethod.POST)
    public void print842ItemAndPMItemMapSlip(HttpServletResponse response, @RequestBody(required = false) Rpt842ItemAndPMItemMapSlipRequest request) throws Exception {
        if (request == null) request = new Rpt842ItemAndPMItemMapSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "842_ItemAndPMItemMapSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/842-item-and-pm-item-map-slip", method = RequestMethod.GET)
    public void print842ItemAndPMItemMapSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print842ItemAndPMItemMapSlip(response, objectMapper.convertValue(query, Rpt842ItemAndPMItemMapSlipRequest.class));
    }

    /**
     * Template: 121_03_PurchaseAnalyticsItemWise.rpt
     * Procedure: [dbo].[USP-PurchaseAnalyticsDashBoard_Report]
     * Desktop: Dashboard.PurchaseAnalyticsDashBoardReport
     */
    @RequestMapping(value = "/reports/print/121-03-purchase-analytics-item-wise", method = RequestMethod.POST)
    public void print12103PurchaseAnalyticsItemWise(HttpServletResponse response, @RequestBody(required = false) Rpt12103PurchaseAnalyticsItemWiseRequest request) throws Exception {
        if (request == null) request = new Rpt12103PurchaseAnalyticsItemWiseRequest();
        printReport(response, "121_03_PurchaseAnalyticsItemWise.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/121-03-purchase-analytics-item-wise", method = RequestMethod.GET)
    public void print12103PurchaseAnalyticsItemWiseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print12103PurchaseAnalyticsItemWise(response, objectMapper.convertValue(query, Rpt12103PurchaseAnalyticsItemWiseRequest.class));
    }

    /**
     * Template: 121_04_PurchaseAnalyticsSupplierWise.rpt
     * Procedure: [dbo].[USP-PurchaseAnalyticsDashBoard_Report]
     * Desktop: Dashboard.PurchaseAnalyticsDashBoardReport
     */
    @RequestMapping(value = "/reports/print/121-04-purchase-analytics-supplier-wise", method = RequestMethod.POST)
    public void print12104PurchaseAnalyticsSupplierWise(HttpServletResponse response, @RequestBody(required = false) Rpt12104PurchaseAnalyticsSupplierWiseRequest request) throws Exception {
        if (request == null) request = new Rpt12104PurchaseAnalyticsSupplierWiseRequest();
        printReport(response, "121_04_PurchaseAnalyticsSupplierWise.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/121-04-purchase-analytics-supplier-wise", method = RequestMethod.GET)
    public void print12104PurchaseAnalyticsSupplierWiseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print12104PurchaseAnalyticsSupplierWise(response, objectMapper.convertValue(query, Rpt12104PurchaseAnalyticsSupplierWiseRequest.class));
    }

    /**
     * Template: AuditActiviyReport.rpt
     * Procedure: [dbo].[USP_ReportsMethod_GetAllMethod]
     * Desktop: ReportsMethod.FormHistory
     */
    @RequestMapping(value = "/reports/print/audit-activiy-report", method = RequestMethod.POST)
    public void printAuditActiviyReport(HttpServletResponse response, @RequestBody(required = false) RptAuditActiviyReportRequest request) throws Exception {
        if (request == null) request = new RptAuditActiviyReportRequest();
        printReport(response, "AuditActiviyReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/audit-activiy-report", method = RequestMethod.GET)
    public void printAuditActiviyReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printAuditActiviyReport(response, objectMapper.convertValue(query, RptAuditActiviyReportRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
