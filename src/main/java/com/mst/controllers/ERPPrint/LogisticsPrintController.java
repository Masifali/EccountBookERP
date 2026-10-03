package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.LogisticsPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** Logistics print actions. Generated from the verified seeder contracts. */
@Controller
public class LogisticsPrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 1300_LogisticRateNegotiationSlip_Slip.rpt
     * Procedure: [lgstcm].[USP_logisticRateNegotiationHeader_Slip]
     * Desktop: logisticRateNegotiationHeader.logisticRateNegotiationHeader_Slip
     */
    @RequestMapping(value = "/reports/print/1300-logistic-rate-negotiation-slip-slip", method = RequestMethod.POST)
    public void print1300LogisticRateNegotiationSlipSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1300LogisticRateNegotiationSlipSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1300LogisticRateNegotiationSlipSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1300_LogisticRateNegotiationSlip_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1300-logistic-rate-negotiation-slip-slip", method = RequestMethod.GET)
    public void print1300LogisticRateNegotiationSlipSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1300LogisticRateNegotiationSlipSlip(response, objectMapper.convertValue(query, Rpt1300LogisticRateNegotiationSlipSlipRequest.class));
    }

    /**
     * Template: 1301_AgreementHeader_Slip.rpt
     * Procedure: [lgstcm].[USP_AgreementHeader_Slip]
     * Desktop: AgreementHeader.AgreementHeader_Slip
     */
    @RequestMapping(value = "/reports/print/1301-agreement-header-slip", method = RequestMethod.POST)
    public void print1301AgreementHeaderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1301AgreementHeaderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1301AgreementHeaderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1301_AgreementHeader_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1301-agreement-header-slip", method = RequestMethod.GET)
    public void print1301AgreementHeaderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1301AgreementHeaderSlip(response, objectMapper.convertValue(query, Rpt1301AgreementHeaderSlipRequest.class));
    }

    /**
     * Template: 1302_LogisticRateNegotiationTransporter_Slip.rpt
     * Procedure: [lgstcm].[USP_logisticRateNegotiationHeader_Slip]
     * Desktop: logisticRateNegotiationHeader.logisticRateNegotiationHeader_Slip
     */
    @RequestMapping(value = "/reports/print/1302-logistic-rate-negotiation-transporter-slip", method = RequestMethod.POST)
    public void print1302LogisticRateNegotiationTransporterSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1302LogisticRateNegotiationTransporterSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1302LogisticRateNegotiationTransporterSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1302_LogisticRateNegotiationTransporter_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1302-logistic-rate-negotiation-transporter-slip", method = RequestMethod.GET)
    public void print1302LogisticRateNegotiationTransporterSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1302LogisticRateNegotiationTransporterSlip(response, objectMapper.convertValue(query, Rpt1302LogisticRateNegotiationTransporterSlipRequest.class));
    }

    /**
     * Template: 1303_PurchaseOrderHeader_Slip.rpt
     * Procedure: [lgstcm].[USP_PurchaseOrderHeader_Slip]
     * Desktop: PurchaseOrderHeader.PurchaseOrderHeader_Slip
     */
    @RequestMapping(value = "/reports/print/1303-purchase-order-header-slip", method = RequestMethod.POST)
    public void print1303PurchaseOrderHeaderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1303PurchaseOrderHeaderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1303PurchaseOrderHeaderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1303_PurchaseOrderHeader_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1303-purchase-order-header-slip", method = RequestMethod.GET)
    public void print1303PurchaseOrderHeaderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1303PurchaseOrderHeaderSlip(response, objectMapper.convertValue(query, Rpt1303PurchaseOrderHeaderSlipRequest.class));
    }

    /**
     * Template: 1304_ServicesBillHeader_Slip.rpt
     * Procedure: [lgstcm].[USP_ServicesBillHeader_Slip]
     * Desktop: ServicesBillHeader.ServicesBillHeader_Slip
     */
    @RequestMapping(value = "/reports/print/1304-services-bill-header-slip", method = RequestMethod.POST)
    public void print1304ServicesBillHeaderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1304ServicesBillHeaderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1304ServicesBillHeaderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1304_ServicesBillHeader_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1304-services-bill-header-slip", method = RequestMethod.GET)
    public void print1304ServicesBillHeaderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1304ServicesBillHeaderSlip(response, objectMapper.convertValue(query, Rpt1304ServicesBillHeaderSlipRequest.class));
    }

    /**
     * Template: 1305_FreightVoucherOutward_Slip.rpt
     * Procedure: [lgstcm].[USP_FreightVoucherOutward_Slip]
     * Desktop: FreightVoucherOutward.FreightVoucherOutward_Slip
     */
    @RequestMapping(value = "/reports/print/1305-freight-voucher-outward-slip", method = RequestMethod.POST)
    public void print1305FreightVoucherOutwardSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1305FreightVoucherOutwardSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1305FreightVoucherOutwardSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1305_FreightVoucherOutward_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1305-freight-voucher-outward-slip", method = RequestMethod.GET)
    public void print1305FreightVoucherOutwardSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1305FreightVoucherOutwardSlip(response, objectMapper.convertValue(query, Rpt1305FreightVoucherOutwardSlipRequest.class));
    }

    /**
     * Template: logisticRateNegotiation_SourceDocumentSubReport.rpt
     * Procedure: [lgstcm].[USP_logisticRateNegotiation_SourceDocumentSubReport]
     * Desktop: logisticRateNegotiationHeader.logisticRateNegotiation_SourceDocumentSubReport
     */
    @RequestMapping(value = "/reports/print/logistic-rate-negotiation-source-document-sub-report", method = RequestMethod.POST)
    public void printLogisticRateNegotiationSourceDocumentSubReport(HttpServletResponse response, @RequestBody(required = false) RptLogisticRateNegotiationSourceDocumentSubReportRequest request) throws Exception {
        if (request == null) request = new RptLogisticRateNegotiationSourceDocumentSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "logisticRateNegotiation_SourceDocumentSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/logistic-rate-negotiation-source-document-sub-report", method = RequestMethod.GET)
    public void printLogisticRateNegotiationSourceDocumentSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printLogisticRateNegotiationSourceDocumentSubReport(response, objectMapper.convertValue(query, RptLogisticRateNegotiationSourceDocumentSubReportRequest.class));
    }

    /**
     * Template: logisticRateNegotiationTransporter_SourceDocumentSubReport.rpt
     * Procedure: [lgstcm].[USP_logisticRateNegotiation_SourceDocumentSubReport]
     * Desktop: logisticRateNegotiationHeader.logisticRateNegotiation_SourceDocumentSubReport
     */
    @RequestMapping(value = "/reports/print/logistic-rate-negotiation-transporter-source-document-sub-report", method = RequestMethod.POST)
    public void printLogisticRateNegotiationTransporterSourceDocumentSubReport(HttpServletResponse response, @RequestBody(required = false) RptLogisticRateNegotiationTransporterSourceDocumentSubReportRequest request) throws Exception {
        if (request == null) request = new RptLogisticRateNegotiationTransporterSourceDocumentSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "logisticRateNegotiationTransporter_SourceDocumentSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/logistic-rate-negotiation-transporter-source-document-sub-report", method = RequestMethod.GET)
    public void printLogisticRateNegotiationTransporterSourceDocumentSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printLogisticRateNegotiationTransporterSourceDocumentSubReport(response, objectMapper.convertValue(query, RptLogisticRateNegotiationTransporterSourceDocumentSubReportRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
