package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.WeighbridgePrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** Weighbridge print actions. Generated from the verified seeder contracts. */
@Controller
public class WeighbridgePrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 280-InvRptWeighBridgeSlip.rpt
     * Procedure: Sp_WbTransactionsSlip_rpt
     * Desktop: WbTransactionsReports.WbTransactionSlip280
     */
    @RequestMapping(value = "/reports/print/280-weigh-bridge-slip", method = RequestMethod.POST)
    public void print280WeighBridgeSlip(HttpServletResponse response, @RequestBody(required = false) Rpt280WeighBridgeSlipRequest request) throws Exception {
        if (request == null) request = new Rpt280WeighBridgeSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "280-InvRptWeighBridgeSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/280-weigh-bridge-slip", method = RequestMethod.GET)
    public void print280WeighBridgeSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print280WeighBridgeSlip(response, objectMapper.convertValue(query, Rpt280WeighBridgeSlipRequest.class));
    }

    /**
     * Template: 281-InvRptWeighBridgeRegister.rpt
     * Procedure: [dbo].[USP_WbTransactions_NewReport]
     * Desktop: WbTransactionsReports.WbTransactionHistory_Updating
     */
    @RequestMapping(value = "/reports/print/281-weigh-bridge-register", method = RequestMethod.POST)
    public void print281WeighBridgeRegister(HttpServletResponse response, @RequestBody(required = false) Rpt281WeighBridgeRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt281WeighBridgeRegisterRequest();
        printReport(response, "281-InvRptWeighBridgeRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/281-weigh-bridge-register", method = RequestMethod.GET)
    public void print281WeighBridgeRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print281WeighBridgeRegister(response, objectMapper.convertValue(query, Rpt281WeighBridgeRegisterRequest.class));
    }

    /**
     * Template: 281-InvRptWeighBridgeSlipWithPics.rpt
     * Procedure: Sp_WbTransactionsSlip_rpt
     * Desktop: WbTransactionsReports.WbTransactionSlip280
     */
    @RequestMapping(value = "/reports/print/281-weigh-bridge-slip-with-pics", method = RequestMethod.POST)
    public void print281WeighBridgeSlipWithPics(HttpServletResponse response, @RequestBody(required = false) Rpt281WeighBridgeSlipWithPicsRequest request) throws Exception {
        if (request == null) request = new Rpt281WeighBridgeSlipWithPicsRequest();
        printReport(response, "281-InvRptWeighBridgeSlipWithPics.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/281-weigh-bridge-slip-with-pics", method = RequestMethod.GET)
    public void print281WeighBridgeSlipWithPicsGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print281WeighBridgeSlipWithPics(response, objectMapper.convertValue(query, Rpt281WeighBridgeSlipWithPicsRequest.class));
    }

    /**
     * Template: WbTransationByGPIDSubReport.rpt
     * Procedure: [dbo].[USP_WbTransationByGPID_SubReport]
     * Desktop: WbTransactions.WbTransationByGPID_SubReport
     */
    @RequestMapping(value = "/reports/print/wb-transation-by-gpid-sub-report", method = RequestMethod.POST)
    public void printWbTransationByGPIDSubReport(HttpServletResponse response, @RequestBody(required = false) RptWbTransationByGPIDSubReportRequest request) throws Exception {
        if (request == null) request = new RptWbTransationByGPIDSubReportRequest();
        printReport(response, "WbTransationByGPIDSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/wb-transation-by-gpid-sub-report", method = RequestMethod.GET)
    public void printWbTransationByGPIDSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printWbTransationByGPIDSubReport(response, objectMapper.convertValue(query, RptWbTransationByGPIDSubReportRequest.class));
    }

    /**
     * Template: WbTransationByOutwardGPID_SubReport.rpt
     * Procedure: [dbo].[USP_WbTransationByOutwardGPID_SubReport]
     * Desktop: WbTransactions.WbTransationByOutwardGPID_SubReport
     */
    @RequestMapping(value = "/reports/print/wb-transation-by-outward-gpid-sub-report", method = RequestMethod.POST)
    public void printWbTransationByOutwardGPIDSubReport(HttpServletResponse response, @RequestBody(required = false) RptWbTransationByOutwardGPIDSubReportRequest request) throws Exception {
        if (request == null) request = new RptWbTransationByOutwardGPIDSubReportRequest();
        printReport(response, "WbTransationByOutwardGPID_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/wb-transation-by-outward-gpid-sub-report", method = RequestMethod.GET)
    public void printWbTransationByOutwardGPIDSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printWbTransationByOutwardGPIDSubReport(response, objectMapper.convertValue(query, RptWbTransationByOutwardGPIDSubReportRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
