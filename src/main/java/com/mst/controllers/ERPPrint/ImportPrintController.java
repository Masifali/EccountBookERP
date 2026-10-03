package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.ImportPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** Import print actions. Generated from the verified seeder contracts. */
@Controller
public class ImportPrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 231-ImLcOrder-Slip.rpt
     * Procedure: [dbo].[USP_ImLcOrder_Register]
     * Desktop: ImLcOrder.ImLcOrder_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/231-im-lc-order-slip", method = RequestMethod.POST)
    public void print231ImLcOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt231ImLcOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt231ImLcOrderSlipRequest();
        printReport(response, "231-ImLcOrder-Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/231-im-lc-order-slip", method = RequestMethod.GET)
    public void print231ImLcOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print231ImLcOrderSlip(response, objectMapper.convertValue(query, Rpt231ImLcOrderSlipRequest.class));
    }

    /**
     * Template: 803-ImGRnSlip.rpt
     * Procedure: Sp_ImGRN_SlipAndRegister
     * Desktop: ImportReports.ImportGrnSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/803-im-g-rn-slip", method = RequestMethod.POST)
    public void print803ImGRnSlip(HttpServletResponse response, @RequestBody(required = false) Rpt803ImGRnSlipRequest request) throws Exception {
        if (request == null) request = new Rpt803ImGRnSlipRequest();
        printReport(response, "803-ImGRnSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/803-im-g-rn-slip", method = RequestMethod.GET)
    public void print803ImGRnSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print803ImGRnSlip(response, objectMapper.convertValue(query, Rpt803ImGRnSlipRequest.class));
    }

    /**
     * Template: 804-PurchaseOrderSlip.rpt
     * Procedure: Sp_ExImLcOrderPurchaseOrder_Slip
     * Desktop: ImportReports.ExImLcOrderPurchaseOrderSlip
     */
    @RequestMapping(value = "/reports/print/804-purchase-order-slip", method = RequestMethod.POST)
    public void print804PurchaseOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt804PurchaseOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt804PurchaseOrderSlipRequest();
        printReport(response, "804-PurchaseOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/804-purchase-order-slip", method = RequestMethod.GET)
    public void print804PurchaseOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print804PurchaseOrderSlip(response, objectMapper.convertValue(query, Rpt804PurchaseOrderSlipRequest.class));
    }

    /**
     * Template: 806-ImInvoiceSlip.rpt
     * Procedure: Sp_ImInvoice_SlipandRegister
     * Desktop: ImportReports.ImInvoiceSlip_806
     */
    @RequestMapping(value = "/reports/print/806-im-invoice-slip", method = RequestMethod.POST)
    public void print806ImInvoiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt806ImInvoiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt806ImInvoiceSlipRequest();
        printReport(response, "806-ImInvoiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/806-im-invoice-slip", method = RequestMethod.GET)
    public void print806ImInvoiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print806ImInvoiceSlip(response, objectMapper.convertValue(query, Rpt806ImInvoiceSlipRequest.class));
    }

    /**
     * Template: 807-ImInvoiceRegister_WithAvgRates.rpt
     * Procedure: SP_ImInvoiceRegister_WithAvgRates
     * Desktop: ImportReports.ImInvoiceRegister_WithAvgRates
     */
    @RequestMapping(value = "/reports/print/807-im-invoice-register-with-avg-rates", method = RequestMethod.POST)
    public void print807ImInvoiceRegisterWithAvgRates(HttpServletResponse response, @RequestBody(required = false) Rpt807ImInvoiceRegisterWithAvgRatesRequest request) throws Exception {
        if (request == null) request = new Rpt807ImInvoiceRegisterWithAvgRatesRequest();
        printReport(response, "807-ImInvoiceRegister_WithAvgRates.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/807-im-invoice-register-with-avg-rates", method = RequestMethod.GET)
    public void print807ImInvoiceRegisterWithAvgRatesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print807ImInvoiceRegisterWithAvgRates(response, objectMapper.convertValue(query, Rpt807ImInvoiceRegisterWithAvgRatesRequest.class));
    }

    /**
     * Template: 808-ImportContractRegister.rpt
     * Procedure: Sp_ImLcOrderNo_ImportSlip_Rpt
     * Desktop: ImportReports.ImportContractSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/808-import-contract-register", method = RequestMethod.POST)
    public void print808ImportContractRegister(HttpServletResponse response, @RequestBody(required = false) Rpt808ImportContractRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt808ImportContractRegisterRequest();
        printReport(response, "808-ImportContractRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/808-import-contract-register", method = RequestMethod.GET)
    public void print808ImportContractRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print808ImportContractRegister(response, objectMapper.convertValue(query, Rpt808ImportContractRegisterRequest.class));
    }

    /**
     * Template: 809-ImportPackingListRegister.rpt
     * Procedure: Sp_ExImLcOrderPurchaseOrder_Slip
     * Desktop: ImportReports.ExImLcOrderPurchaseOrderSlip
     */
    @RequestMapping(value = "/reports/print/809-import-packing-list-register", method = RequestMethod.POST)
    public void print809ImportPackingListRegister(HttpServletResponse response, @RequestBody(required = false) Rpt809ImportPackingListRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt809ImportPackingListRegisterRequest();
        printReport(response, "809-ImportPackingListRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/809-import-packing-list-register", method = RequestMethod.GET)
    public void print809ImportPackingListRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print809ImportPackingListRegister(response, objectMapper.convertValue(query, Rpt809ImportPackingListRegisterRequest.class));
    }

    /**
     * Template: 810-ImportPurchaseOrder.rpt
     * Procedure: Sp_ImportPurchaseOrder_Slip_rpt
     * Desktop: ImportReports.ImportPurchaseOrderSlip
     */
    @RequestMapping(value = "/reports/print/810-import-purchase-order", method = RequestMethod.POST)
    public void print810ImportPurchaseOrder(HttpServletResponse response, @RequestBody(required = false) Rpt810ImportPurchaseOrderRequest request) throws Exception {
        if (request == null) request = new Rpt810ImportPurchaseOrderRequest();
        printReport(response, "810-ImportPurchaseOrder.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/810-import-purchase-order", method = RequestMethod.GET)
    public void print810ImportPurchaseOrderGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print810ImportPurchaseOrder(response, objectMapper.convertValue(query, Rpt810ImportPurchaseOrderRequest.class));
    }

    /**
     * Template: 811-ImportGrnRegister.rpt
     * Procedure: Sp_ImGRN_SlipAndRegister
     * Desktop: ImportReports.ImportGrnSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/811-import-grn-register", method = RequestMethod.POST)
    public void print811ImportGrnRegister(HttpServletResponse response, @RequestBody(required = false) Rpt811ImportGrnRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt811ImportGrnRegisterRequest();
        printReport(response, "811-ImportGrnRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/811-import-grn-register", method = RequestMethod.GET)
    public void print811ImportGrnRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print811ImportGrnRegister(response, objectMapper.convertValue(query, Rpt811ImportGrnRegisterRequest.class));
    }

    /**
     * Template: 900_ProformaInvoiceSlip.rpt
     * Procedure: [ImEx].[usp_Get_ProformaMaster_Report]
     * Desktop: ProformaMaster.ProformaMaster_Report
     */
    @RequestMapping(value = "/reports/print/900-proforma-invoice-slip", method = RequestMethod.POST)
    public void print900ProformaInvoiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt900ProformaInvoiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt900ProformaInvoiceSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "900_ProformaInvoiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/900-proforma-invoice-slip", method = RequestMethod.GET)
    public void print900ProformaInvoiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print900ProformaInvoiceSlip(response, objectMapper.convertValue(query, Rpt900ProformaInvoiceSlipRequest.class));
    }

    /**
     * Template: 902_ImportInvoiceSlip.rpt
     * Procedure: [ImEx].[usp_Get_invoiceMasterReport]
     * Desktop: invoiceMaster.InvoiceMaster_Report
     */
    @RequestMapping(value = "/reports/print/902-import-invoice-slip", method = RequestMethod.POST)
    public void print902ImportInvoiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt902ImportInvoiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt902ImportInvoiceSlipRequest();
        printReport(response, "902_ImportInvoiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/902-import-invoice-slip", method = RequestMethod.GET)
    public void print902ImportInvoiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print902ImportInvoiceSlip(response, objectMapper.convertValue(query, Rpt902ImportInvoiceSlipRequest.class));
    }

    /**
     * Template: ImLcOrderPaymentDetail_SubReport.rpt
     * Procedure: [dbo].[USP_ImLcOrderPaymentDetail_SubReport]
     * Desktop: ImLcOrder.ImLcOrderPaymentDetail_SubReport
     */
    @RequestMapping(value = "/reports/print/im-lc-order-payment-detail-sub-report", method = RequestMethod.POST)
    public void printImLcOrderPaymentDetailSubReport(HttpServletResponse response, @RequestBody(required = false) RptImLcOrderPaymentDetailSubReportRequest request) throws Exception {
        if (request == null) request = new RptImLcOrderPaymentDetailSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "ImLcOrderPaymentDetail_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/im-lc-order-payment-detail-sub-report", method = RequestMethod.GET)
    public void printImLcOrderPaymentDetailSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printImLcOrderPaymentDetailSubReport(response, objectMapper.convertValue(query, RptImLcOrderPaymentDetailSubReportRequest.class));
    }

    /**
     * Template: ImportInvoice_SubReport.rpt
     * Procedure: [ImEx].[usp_Get_Invoice_SubReport]
     * Desktop: invoiceMaster.ImportInvoiceSlipSupReprt
     */
    @RequestMapping(value = "/reports/print/import-invoice-sub-report", method = RequestMethod.POST)
    public void printImportInvoiceSubReport(HttpServletResponse response, @RequestBody(required = false) RptImportInvoiceSubReportRequest request) throws Exception {
        if (request == null) request = new RptImportInvoiceSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "ImportInvoice_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/import-invoice-sub-report", method = RequestMethod.GET)
    public void printImportInvoiceSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printImportInvoiceSubReport(response, objectMapper.convertValue(query, RptImportInvoiceSubReportRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
