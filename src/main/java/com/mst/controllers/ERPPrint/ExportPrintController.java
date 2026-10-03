package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.ExportPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** Export print actions. Generated from the verified seeder contracts. */
@Controller
public class ExportPrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 529A_ExportInvoiceSlipPackingList.rpt
     * Procedure: usp_ExImInvoicePackingList_Rpt
     * Desktop: ExportPdfReport.InvoicePackingList529A
     */
    @RequestMapping(value = "/reports/print/529a-export-invoice-slip-packing-list", method = RequestMethod.POST)
    public void print529AExportInvoiceSlipPackingList(HttpServletResponse response, @RequestBody(required = false) Rpt529AExportInvoiceSlipPackingListRequest request) throws Exception {
        if (request == null) request = new Rpt529AExportInvoiceSlipPackingListRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "529A_ExportInvoiceSlipPackingList.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/529a-export-invoice-slip-packing-list", method = RequestMethod.GET)
    public void print529AExportInvoiceSlipPackingListGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print529AExportInvoiceSlipPackingList(response, objectMapper.convertValue(query, Rpt529AExportInvoiceSlipPackingListRequest.class));
    }

    /**
     * Template: 529B_ExportInvoiceSlipPackingList.rpt
     * Procedure: usp_ExImInvoicePackingList_Rpt
     * Desktop: ExportPdfReport.InvoicePackingList529A
     */
    @RequestMapping(value = "/reports/print/529b-export-invoice-slip-packing-list", method = RequestMethod.POST)
    public void print529BExportInvoiceSlipPackingList(HttpServletResponse response, @RequestBody(required = false) Rpt529BExportInvoiceSlipPackingListRequest request) throws Exception {
        if (request == null) request = new Rpt529BExportInvoiceSlipPackingListRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "529B_ExportInvoiceSlipPackingList.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/529b-export-invoice-slip-packing-list", method = RequestMethod.GET)
    public void print529BExportInvoiceSlipPackingListGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print529BExportInvoiceSlipPackingList(response, objectMapper.convertValue(query, Rpt529BExportInvoiceSlipPackingListRequest.class));
    }

    /**
     * Template: 529C_ExportInvoiceSlipPackingList.rpt
     * Procedure: usp_ExImInvoicePackingList_Rpt
     * Desktop: ExportPdfReport.InvoicePackingList529A
     */
    @RequestMapping(value = "/reports/print/529c-export-invoice-slip-packing-list", method = RequestMethod.POST)
    public void print529CExportInvoiceSlipPackingList(HttpServletResponse response, @RequestBody(required = false) Rpt529CExportInvoiceSlipPackingListRequest request) throws Exception {
        if (request == null) request = new Rpt529CExportInvoiceSlipPackingListRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "529C_ExportInvoiceSlipPackingList.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/529c-export-invoice-slip-packing-list", method = RequestMethod.GET)
    public void print529CExportInvoiceSlipPackingListGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print529CExportInvoiceSlipPackingList(response, objectMapper.convertValue(query, Rpt529CExportInvoiceSlipPackingListRequest.class));
    }

    /**
     * Template: 02_GDBreakUpHeader_Slip.rpt
     * Procedure: [dbo].[USP_GDBreakUpHeader_Slip]
     * Desktop: GDBreakUpHeader.GDBreakUpHeader_Slip
     */
    @RequestMapping(value = "/reports/print/02-gd-break-up-header-slip", method = RequestMethod.POST)
    public void print02GDBreakUpHeaderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt02GDBreakUpHeaderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt02GDBreakUpHeaderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "02_GDBreakUpHeader_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/02-gd-break-up-header-slip", method = RequestMethod.GET)
    public void print02GDBreakUpHeaderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print02GDBreakUpHeaderSlip(response, objectMapper.convertValue(query, Rpt02GDBreakUpHeaderSlipRequest.class));
    }

    /**
     * Template: 102-ANewAcRptExportInvoiceVoucherSlip.rpt
     * Procedure: SpVouchers_ExportInvoiceVoucherSlipNew_Rpt
     * Desktop: VoucherReports.ExportInvoiceVoucher
     */
    @RequestMapping(value = "/reports/print/102-a-new-ac-export-invoice-voucher-slip", method = RequestMethod.POST)
    public void print102ANewAcExportInvoiceVoucherSlip(HttpServletResponse response, @RequestBody(required = false) Rpt102ANewAcExportInvoiceVoucherSlipRequest request) throws Exception {
        if (request == null) request = new Rpt102ANewAcExportInvoiceVoucherSlipRequest();
        printReport(response, "102-ANewAcRptExportInvoiceVoucherSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/102-a-new-ac-export-invoice-voucher-slip", method = RequestMethod.GET)
    public void print102ANewAcExportInvoiceVoucherSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print102ANewAcExportInvoiceVoucherSlip(response, objectMapper.convertValue(query, Rpt102ANewAcExportInvoiceVoucherSlipRequest.class));
    }

    /**
     * Template: 102-ExportReturnInvoiceVoucherSlip.rpt
     * Procedure: SpVouchers_ExportReturnInvoiceVoucherSlipNew_Rpt
     * Desktop: VoucherReports.ExportReturnInvoice
     */
    @RequestMapping(value = "/reports/print/102-export-return-invoice-voucher-slip", method = RequestMethod.POST)
    public void print102ExportReturnInvoiceVoucherSlip(HttpServletResponse response, @RequestBody(required = false) Rpt102ExportReturnInvoiceVoucherSlipRequest request) throws Exception {
        if (request == null) request = new Rpt102ExportReturnInvoiceVoucherSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "102-ExportReturnInvoiceVoucherSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/102-export-return-invoice-voucher-slip", method = RequestMethod.GET)
    public void print102ExportReturnInvoiceVoucherSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print102ExportReturnInvoiceVoucherSlip(response, objectMapper.convertValue(query, Rpt102ExportReturnInvoiceVoucherSlipRequest.class));
    }

    /**
     * Template: 104-AcRptGeneralJournalAcAndInventoryDetailSlip.rpt
     * Procedure: Sp_Vouchers_GeneralJournalAcAndInventoryDetailSlip_Rpt
     * Desktop: VoucherReports.VoucherSlipForInventoryReport
     */
    @RequestMapping(value = "/reports/print/104-general-journal-ac-and-inventory-detail-slip", method = RequestMethod.POST)
    public void print104GeneralJournalAcAndInventoryDetailSlip(HttpServletResponse response, @RequestBody(required = false) Rpt104GeneralJournalAcAndInventoryDetailSlipRequest request) throws Exception {
        if (request == null) request = new Rpt104GeneralJournalAcAndInventoryDetailSlipRequest();
        printReport(response, "104-AcRptGeneralJournalAcAndInventoryDetailSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/104-general-journal-ac-and-inventory-detail-slip", method = RequestMethod.GET)
    public void print104GeneralJournalAcAndInventoryDetailSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print104GeneralJournalAcAndInventoryDetailSlip(response, objectMapper.convertValue(query, Rpt104GeneralJournalAcAndInventoryDetailSlipRequest.class));
    }

    /**
     * Template: 174-ExImForwardingDirect_SlipAndRegister.rpt
     * Procedure: Sp_ExImForwardingDirect_SlipAndregisterRpt
     * Desktop: ExportPdfReport.ExImForwardingSlipandRegisterDirect
     */
    @RequestMapping(value = "/reports/print/174-ex-im-forwarding-direct-slip-and-register", method = RequestMethod.POST)
    public void print174ExImForwardingDirectSlipAndRegister(HttpServletResponse response, @RequestBody(required = false) Rpt174ExImForwardingDirectSlipAndRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt174ExImForwardingDirectSlipAndRegisterRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "174-ExImForwardingDirect_SlipAndRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/174-ex-im-forwarding-direct-slip-and-register", method = RequestMethod.GET)
    public void print174ExImForwardingDirectSlipAndRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print174ExImForwardingDirectSlipAndRegister(response, objectMapper.convertValue(query, Rpt174ExImForwardingDirectSlipAndRegisterRequest.class));
    }

    /**
     * Template: 175-ForwardingByReferenceNoRegister.rpt
     * Procedure: USP_FarwardingByReferenceNoRegister
     * Desktop: ExportPdfReport.FarwardingByReferenceNoRegister
     */
    @RequestMapping(value = "/reports/print/175-forwarding-by-reference-no-register", method = RequestMethod.POST)
    public void print175ForwardingByReferenceNoRegister(HttpServletResponse response, @RequestBody(required = false) Rpt175ForwardingByReferenceNoRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt175ForwardingByReferenceNoRegisterRequest();
        printReport(response, "175-ForwardingByReferenceNoRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/175-forwarding-by-reference-no-register", method = RequestMethod.GET)
    public void print175ForwardingByReferenceNoRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print175ForwardingByReferenceNoRegister(response, objectMapper.convertValue(query, Rpt175ForwardingByReferenceNoRegisterRequest.class));
    }

    /**
     * Template: 216-ExportReturnInvoiceSlip.rpt
     * Procedure: [dbo].[USp_ExportReturnInvoice_SlipAndRegister]
     * Desktop: ExportReturnInvoice.ExportReturnInvoiceSlipandRegister
     */
    @RequestMapping(value = "/reports/print/216-export-return-invoice-slip", method = RequestMethod.POST)
    public void print216ExportReturnInvoiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt216ExportReturnInvoiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt216ExportReturnInvoiceSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "216-ExportReturnInvoiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/216-export-return-invoice-slip", method = RequestMethod.GET)
    public void print216ExportReturnInvoiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print216ExportReturnInvoiceSlip(response, objectMapper.convertValue(query, Rpt216ExportReturnInvoiceSlipRequest.class));
    }

    /**
     * Template: 244-GatePassInspectionSlip.rpt
     * Procedure: USP_ExImVCITransaction_SlipAndRegister
     * Desktop: ExImVCITransaction.ExImVCITransaction_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/244-gate-pass-inspection-slip", method = RequestMethod.POST)
    public void print244GatePassInspectionSlip(HttpServletResponse response, @RequestBody(required = false) Rpt244GatePassInspectionSlipRequest request) throws Exception {
        if (request == null) request = new Rpt244GatePassInspectionSlipRequest();
        printReport(response, "244-GatePassInspectionSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/244-gate-pass-inspection-slip", method = RequestMethod.GET)
    public void print244GatePassInspectionSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print244GatePassInspectionSlip(response, objectMapper.convertValue(query, Rpt244GatePassInspectionSlipRequest.class));
    }

    /**
     * Template: 263-InvDeliveryOrderForApproval.rpt
     * Procedure: Proc_ExImExportShipingLineBooking_SlipAndRegister_Rpt
     * Desktop: ExImExportShipingLineBooking.PrintSlipandRegister
     */
    @RequestMapping(value = "/reports/print/263-inv-delivery-order-for-approval", method = RequestMethod.POST)
    public void print263InvDeliveryOrderForApproval(HttpServletResponse response, @RequestBody(required = false) Rpt263InvDeliveryOrderForApprovalRequest request) throws Exception {
        if (request == null) request = new Rpt263InvDeliveryOrderForApprovalRequest();
        printReport(response, "263-InvDeliveryOrderForApproval.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/263-inv-delivery-order-for-approval", method = RequestMethod.GET)
    public void print263InvDeliveryOrderForApprovalGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print263InvDeliveryOrderForApproval(response, objectMapper.convertValue(query, Rpt263InvDeliveryOrderForApprovalRequest.class));
    }

    /**
     * Template: 283-FcyBankChargesRegister.rpt
     * Procedure: [dbo].[usp_FcyBankCharges_Register]
     * Desktop: ExImFCBankReceipts.FcyBankCharges_Register
     */
    @RequestMapping(value = "/reports/print/283-fcy-bank-charges-register", method = RequestMethod.POST)
    public void print283FcyBankChargesRegister(HttpServletResponse response, @RequestBody(required = false) Rpt283FcyBankChargesRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt283FcyBankChargesRegisterRequest();
        printReport(response, "283-FcyBankChargesRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/283-fcy-bank-charges-register", method = RequestMethod.GET)
    public void print283FcyBankChargesRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print283FcyBankChargesRegister(response, objectMapper.convertValue(query, Rpt283FcyBankChargesRegisterRequest.class));
    }

    /**
     * Template: 288-FcyReceiptsSummaryRegister.rpt
     * Procedure: USP_FcyReceiptsSummaryRegister
     * Desktop: ExImFCBankReceipts.FcyReceiptsSummaryRegister
     */
    @RequestMapping(value = "/reports/print/288-fcy-receipts-summary-register", method = RequestMethod.POST)
    public void print288FcyReceiptsSummaryRegister(HttpServletResponse response, @RequestBody(required = false) Rpt288FcyReceiptsSummaryRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt288FcyReceiptsSummaryRegisterRequest();
        printReport(response, "288-FcyReceiptsSummaryRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/288-fcy-receipts-summary-register", method = RequestMethod.GET)
    public void print288FcyReceiptsSummaryRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print288FcyReceiptsSummaryRegister(response, objectMapper.convertValue(query, Rpt288FcyReceiptsSummaryRegisterRequest.class));
    }

    /**
     * Template: 289-BankGdsSummaryRegister.rpt
     * Procedure: USP_BankGdsSummary
     * Desktop: ExportReports.BankGdSummary
     */
    @RequestMapping(value = "/reports/print/289-bank-gds-summary-register", method = RequestMethod.POST)
    public void print289BankGdsSummaryRegister(HttpServletResponse response, @RequestBody(required = false) Rpt289BankGdsSummaryRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt289BankGdsSummaryRegisterRequest();
        printReport(response, "289-BankGdsSummaryRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/289-bank-gds-summary-register", method = RequestMethod.GET)
    public void print289BankGdsSummaryRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print289BankGdsSummaryRegister(response, objectMapper.convertValue(query, Rpt289BankGdsSummaryRegisterRequest.class));
    }

    /**
     * Template: 300_01_FISummaryRegister.rpt
     * Procedure: usp_getFIBalanceSummary
     * Desktop: ExportReports.GetFIBalanceSummary
     */
    @RequestMapping(value = "/reports/print/300-01-fi-summary-register", method = RequestMethod.POST)
    public void print30001FISummaryRegister(HttpServletResponse response, @RequestBody(required = false) Rpt30001FISummaryRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt30001FISummaryRegisterRequest();
        printReport(response, "300_01_FISummaryRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/300-01-fi-summary-register", method = RequestMethod.GET)
    public void print30001FISummaryRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print30001FISummaryRegister(response, objectMapper.convertValue(query, Rpt30001FISummaryRegisterRequest.class));
    }

    /**
     * Template: 300_02-FIAdvanceBalanceSummary.rpt
     * Procedure: usp_FinancialInstrumentAdvanceBalanceSummary
     * Desktop: ExImEFormRegistration.FIAdvanceBalanceSummary
     */
    @RequestMapping(value = "/reports/print/300-02-fi-advance-balance-summary", method = RequestMethod.POST)
    public void print30002FIAdvanceBalanceSummary(HttpServletResponse response, @RequestBody(required = false) Rpt30002FIAdvanceBalanceSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt30002FIAdvanceBalanceSummaryRequest();
        printReport(response, "300_02-FIAdvanceBalanceSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/300-02-fi-advance-balance-summary", method = RequestMethod.GET)
    public void print30002FIAdvanceBalanceSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print30002FIAdvanceBalanceSummary(response, objectMapper.convertValue(query, Rpt30002FIAdvanceBalanceSummaryRequest.class));
    }

    /**
     * Template: 319-ExportSaleInvoiceSlip.rpt
     * Procedure: Sp_Vouchers_GetMethods
     * Desktop: VoucherHead.GetInvoiceNoByPaymentByInvoice
     */
    @RequestMapping(value = "/reports/print/319-export-sale-invoice-slip", method = RequestMethod.POST)
    public void print319ExportSaleInvoiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt319ExportSaleInvoiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt319ExportSaleInvoiceSlipRequest();
        printReport(response, "319-ExportSaleInvoiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/319-export-sale-invoice-slip", method = RequestMethod.GET)
    public void print319ExportSaleInvoiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print319ExportSaleInvoiceSlip(response, objectMapper.convertValue(query, Rpt319ExportSaleInvoiceSlipRequest.class));
    }

    /**
     * Template: 319A-ExportSaleInvoiceSlip.rpt
     * Procedure: Sp_InvSaleInvoice_GetAllMethod
     * Desktop: InvSaleInvoice.GetByGdnIdForSaleInvoiceTrading
     */
    @RequestMapping(value = "/reports/print/319a-export-sale-invoice-slip", method = RequestMethod.POST)
    public void print319AExportSaleInvoiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt319AExportSaleInvoiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt319AExportSaleInvoiceSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "319A-ExportSaleInvoiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/319a-export-sale-invoice-slip", method = RequestMethod.GET)
    public void print319AExportSaleInvoiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print319AExportSaleInvoiceSlip(response, objectMapper.convertValue(query, Rpt319AExportSaleInvoiceSlipRequest.class));
    }

    /**
     * Template: 359-GetExportSales.rpt
     * Procedure: USP_GetExportSales
     * Desktop: ExportReports.GetExportSalesReport
     */
    @RequestMapping(value = "/reports/print/359-get-export-sales", method = RequestMethod.POST)
    public void print359GetExportSales(HttpServletResponse response, @RequestBody(required = false) Rpt359GetExportSalesRequest request) throws Exception {
        if (request == null) request = new Rpt359GetExportSalesRequest();
        printReport(response, "359-GetExportSales.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/359-get-export-sales", method = RequestMethod.GET)
    public void print359GetExportSalesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print359GetExportSales(response, objectMapper.convertValue(query, Rpt359GetExportSalesRequest.class));
    }

    /**
     * Template: 395-LcOrderShipmentScheduleLcOrderWise_Slip.rpt
     * Procedure: SpExImInvoice_ExportInvoiceRegisterByLcContract_Rpt
     * Desktop: ExportPdfReport.ExpRptSalesContractExportRegister_523
     */
    @RequestMapping(value = "/reports/print/395-lc-order-shipment-schedule-lc-order-wise-slip", method = RequestMethod.POST)
    public void print395LcOrderShipmentScheduleLcOrderWiseSlip(HttpServletResponse response, @RequestBody(required = false) Rpt395LcOrderShipmentScheduleLcOrderWiseSlipRequest request) throws Exception {
        if (request == null) request = new Rpt395LcOrderShipmentScheduleLcOrderWiseSlipRequest();
        printReport(response, "395-LcOrderShipmentScheduleLcOrderWise_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/395-lc-order-shipment-schedule-lc-order-wise-slip", method = RequestMethod.GET)
    public void print395LcOrderShipmentScheduleLcOrderWiseSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print395LcOrderShipmentScheduleLcOrderWiseSlip(response, objectMapper.convertValue(query, Rpt395LcOrderShipmentScheduleLcOrderWiseSlipRequest.class));
    }

    /**
     * Template: 396-ExportDeliveryOrderSlip.rpt
     * Procedure: Sp_InvDeliveryOrder_Slip
     * Desktop: InvGrnandGdnReports.InvDeliveryOrderSlip
     */
    @RequestMapping(value = "/reports/print/396-export-delivery-order-slip", method = RequestMethod.POST)
    public void print396ExportDeliveryOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt396ExportDeliveryOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt396ExportDeliveryOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "396-ExportDeliveryOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/396-export-delivery-order-slip", method = RequestMethod.GET)
    public void print396ExportDeliveryOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print396ExportDeliveryOrderSlip(response, objectMapper.convertValue(query, Rpt396ExportDeliveryOrderSlipRequest.class));
    }

    /**
     * Template: 466-ExportGoodsReceiptsAtPortRegister.rpt
     * Procedure: Sp_ExImGoodsReceiptsAtPort_GetAllMetohd
     * Desktop: ExImGoodsReceiptsAtPort.Getall
     */
    @RequestMapping(value = "/reports/print/466-export-goods-receipts-at-port-register", method = RequestMethod.POST)
    public void print466ExportGoodsReceiptsAtPortRegister(HttpServletResponse response, @RequestBody(required = false) Rpt466ExportGoodsReceiptsAtPortRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt466ExportGoodsReceiptsAtPortRegisterRequest();
        printReport(response, "466-ExportGoodsReceiptsAtPortRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/466-export-goods-receipts-at-port-register", method = RequestMethod.GET)
    public void print466ExportGoodsReceiptsAtPortRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print466ExportGoodsReceiptsAtPortRegister(response, objectMapper.convertValue(query, Rpt466ExportGoodsReceiptsAtPortRegisterRequest.class));
    }

    /**
     * Template: 501-ExportSalesContractExportNew.rpt
     * Procedure: Sp_ExImLcOrderNo_ExportSlip_Rpt
     * Desktop: SaleContractReports.SaleContractReports501
     */
    @RequestMapping(value = "/reports/print/501-export-sales-contract-export-new", method = RequestMethod.POST)
    public void print501ExportSalesContractExportNew(HttpServletResponse response, @RequestBody(required = false) Rpt501ExportSalesContractExportNewRequest request) throws Exception {
        if (request == null) request = new Rpt501ExportSalesContractExportNewRequest();
        printReport(response, "501-ExportSalesContractExportNew.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/501-export-sales-contract-export-new", method = RequestMethod.GET)
    public void print501ExportSalesContractExportNewGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print501ExportSalesContractExportNew(response, objectMapper.convertValue(query, Rpt501ExportSalesContractExportNewRequest.class));
    }

    /**
     * Template: 501-ExpRptSalesContractExport.rpt
     * Procedure: Sp_ExImLcOrderNo_ExportSlip_Rpt
     * Desktop: ExportPdfReport.ExpRptSalesContractExport_501
     */
    @RequestMapping(value = "/reports/print/501-sales-contract-export", method = RequestMethod.POST)
    public void print501SalesContractExport(HttpServletResponse response, @RequestBody(required = false) Rpt501SalesContractExportRequest request) throws Exception {
        if (request == null) request = new Rpt501SalesContractExportRequest();
        printReport(response, "501-ExpRptSalesContractExport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/501-sales-contract-export", method = RequestMethod.GET)
    public void print501SalesContractExportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print501SalesContractExport(response, objectMapper.convertValue(query, Rpt501SalesContractExportRequest.class));
    }

    /**
     * Template: 501A_ExportSalesContractExportNew.rpt
     * Procedure: Sp_ExImLcOrderNo_ExportSlip_Rpt
     * Desktop: SaleContractReports.SaleContractReports501
     */
    @RequestMapping(value = "/reports/print/501a-export-sales-contract-export-new", method = RequestMethod.POST)
    public void print501AExportSalesContractExportNew(HttpServletResponse response, @RequestBody(required = false) Rpt501AExportSalesContractExportNewRequest request) throws Exception {
        if (request == null) request = new Rpt501AExportSalesContractExportNewRequest();
        printReport(response, "501A_ExportSalesContractExportNew.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/501a-export-sales-contract-export-new", method = RequestMethod.GET)
    public void print501AExportSalesContractExportNewGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print501AExportSalesContractExportNew(response, objectMapper.convertValue(query, Rpt501AExportSalesContractExportNewRequest.class));
    }

    /**
     * Template: 501B-ExpRptSalesContractExport.rpt
     * Procedure: Sp_ExportContractByInvoice_Rpt
     * Desktop: SaleContractReports.SaleContractByInvoiceIdReports501B
     */
    @RequestMapping(value = "/reports/print/501b-sales-contract-export", method = RequestMethod.POST)
    public void print501BSalesContractExport(HttpServletResponse response, @RequestBody(required = false) Rpt501BSalesContractExportRequest request) throws Exception {
        if (request == null) request = new Rpt501BSalesContractExportRequest();
        printReport(response, "501B-ExpRptSalesContractExport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/501b-sales-contract-export", method = RequestMethod.GET)
    public void print501BSalesContractExportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print501BSalesContractExport(response, objectMapper.convertValue(query, Rpt501BSalesContractExportRequest.class));
    }

    /**
     * Template: 501B_ExportSalesContractExportNew.rpt
     * Procedure: Sp_ExImLcOrderNo_ExportSlip_Rpt
     * Desktop: SaleContractReports.SaleContractReports501
     */
    @RequestMapping(value = "/reports/print/501b-export-sales-contract-export-new", method = RequestMethod.POST)
    public void print501BExportSalesContractExportNew(HttpServletResponse response, @RequestBody(required = false) Rpt501BExportSalesContractExportNewRequest request) throws Exception {
        if (request == null) request = new Rpt501BExportSalesContractExportNewRequest();
        printReport(response, "501B_ExportSalesContractExportNew.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/501b-export-sales-contract-export-new", method = RequestMethod.GET)
    public void print501BExportSalesContractExportNewGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print501BExportSalesContractExportNew(response, objectMapper.convertValue(query, Rpt501BExportSalesContractExportNewRequest.class));
    }

    /**
     * Template: 505-ExImBillOfLading_Slip.rpt
     * Procedure: Sp_ExImBillOfLading_SlipAndRegister_Rpt
     * Desktop: ExImBillOfLading.ExImBillOfLading_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/505-ex-im-bill-of-lading-slip", method = RequestMethod.POST)
    public void print505ExImBillOfLadingSlip(HttpServletResponse response, @RequestBody(required = false) Rpt505ExImBillOfLadingSlipRequest request) throws Exception {
        if (request == null) request = new Rpt505ExImBillOfLadingSlipRequest();
        printReport(response, "505-ExImBillOfLading_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/505-ex-im-bill-of-lading-slip", method = RequestMethod.GET)
    public void print505ExImBillOfLadingSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print505ExImBillOfLadingSlip(response, objectMapper.convertValue(query, Rpt505ExImBillOfLadingSlipRequest.class));
    }

    /**
     * Template: 506-ExImBillOfLading_register.rpt
     * Procedure: Sp_ExImBillOfLading_SlipAndRegister_Rpt
     * Desktop: ExImBillOfLading.ExImBillOfLading_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/506-ex-im-bill-of-lading-register", method = RequestMethod.POST)
    public void print506ExImBillOfLadingRegister(HttpServletResponse response, @RequestBody(required = false) Rpt506ExImBillOfLadingRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt506ExImBillOfLadingRegisterRequest();
        printReport(response, "506-ExImBillOfLading_register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/506-ex-im-bill-of-lading-register", method = RequestMethod.GET)
    public void print506ExImBillOfLadingRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print506ExImBillOfLadingRegister(response, objectMapper.convertValue(query, Rpt506ExImBillOfLadingRegisterRequest.class));
    }

    /**
     * Template: 507-ExImForwarding_Slip.rpt
     * Procedure: Sp_ExImForwarding_Rpt
     * Desktop: ExportPdfReport.ExImForwarding_Slip_507
     */
    @RequestMapping(value = "/reports/print/507-ex-im-forwarding-slip", method = RequestMethod.POST)
    public void print507ExImForwardingSlip(HttpServletResponse response, @RequestBody(required = false) Rpt507ExImForwardingSlipRequest request) throws Exception {
        if (request == null) request = new Rpt507ExImForwardingSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "507-ExImForwarding_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/507-ex-im-forwarding-slip", method = RequestMethod.GET)
    public void print507ExImForwardingSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print507ExImForwardingSlip(response, objectMapper.convertValue(query, Rpt507ExImForwardingSlipRequest.class));
    }

    /**
     * Template: 507_01_ExImForwardingDirect_Slip.rpt
     * Procedure: Sp_ExImForwarding_Rpt
     * Desktop: ExportPdfReport.ExImForwarding_Slip_507
     */
    @RequestMapping(value = "/reports/print/507-01-ex-im-forwarding-direct-slip", method = RequestMethod.POST)
    public void print50701ExImForwardingDirectSlip(HttpServletResponse response, @RequestBody(required = false) Rpt50701ExImForwardingDirectSlipRequest request) throws Exception {
        if (request == null) request = new Rpt50701ExImForwardingDirectSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "507_01_ExImForwardingDirect_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/507-01-ex-im-forwarding-direct-slip", method = RequestMethod.GET)
    public void print50701ExImForwardingDirectSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print50701ExImForwardingDirectSlip(response, objectMapper.convertValue(query, Rpt50701ExImForwardingDirectSlipRequest.class));
    }

    /**
     * Template: 508-ExImForwarding_Register.rpt
     * Procedure: Proc_ExImExportShipingLineBooking_SlipAndRegister_Rpt
     * Desktop: ExImExportShipingLineBooking.PrintSlipandRegister
     */
    @RequestMapping(value = "/reports/print/508-ex-im-forwarding-register", method = RequestMethod.POST)
    public void print508ExImForwardingRegister(HttpServletResponse response, @RequestBody(required = false) Rpt508ExImForwardingRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt508ExImForwardingRegisterRequest();
        printReport(response, "508-ExImForwarding_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/508-ex-im-forwarding-register", method = RequestMethod.GET)
    public void print508ExImForwardingRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print508ExImForwardingRegister(response, objectMapper.convertValue(query, Rpt508ExImForwardingRegisterRequest.class));
    }

    /**
     * Template: 508_01_Forwarding_Customised_Register.rpt
     * Procedure: Proc_ExImExportShipingLineBooking_SlipAndRegister_Rpt
     * Desktop: ExImExportShipingLineBooking.PrintSlipandRegister
     */
    @RequestMapping(value = "/reports/print/508-01-forwarding-customised-register", method = RequestMethod.POST)
    public void print50801ForwardingCustomisedRegister(HttpServletResponse response, @RequestBody(required = false) Rpt50801ForwardingCustomisedRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt50801ForwardingCustomisedRegisterRequest();
        printReport(response, "508_01_Forwarding_Customised_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/508-01-forwarding-customised-register", method = RequestMethod.GET)
    public void print50801ForwardingCustomisedRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print50801ForwardingCustomisedRegister(response, objectMapper.convertValue(query, Rpt50801ForwardingCustomisedRegisterRequest.class));
    }

    /**
     * Template: 511-ExImShippedConsignmentFollowUps_Slip.rpt
     * Procedure: Sp_ExImBillOfLading_SlipAndRegister_Rpt
     * Desktop: ExImBillOfLading.ExImBillOfLading_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/511-ex-im-shipped-consignment-follow-ups-slip", method = RequestMethod.POST)
    public void print511ExImShippedConsignmentFollowUpsSlip(HttpServletResponse response, @RequestBody(required = false) Rpt511ExImShippedConsignmentFollowUpsSlipRequest request) throws Exception {
        if (request == null) request = new Rpt511ExImShippedConsignmentFollowUpsSlipRequest();
        printReport(response, "511-ExImShippedConsignmentFollowUps_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/511-ex-im-shipped-consignment-follow-ups-slip", method = RequestMethod.GET)
    public void print511ExImShippedConsignmentFollowUpsSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print511ExImShippedConsignmentFollowUpsSlip(response, objectMapper.convertValue(query, Rpt511ExImShippedConsignmentFollowUpsSlipRequest.class));
    }

    /**
     * Template: 512-ExImShippedConsignmentFollowUps_Register.rpt
     * Procedure: Sp_ExImBillOfLading_SlipAndRegister_Rpt
     * Desktop: ExImBillOfLading.ExImBillOfLading_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/512-ex-im-shipped-consignment-follow-ups-register", method = RequestMethod.POST)
    public void print512ExImShippedConsignmentFollowUpsRegister(HttpServletResponse response, @RequestBody(required = false) Rpt512ExImShippedConsignmentFollowUpsRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt512ExImShippedConsignmentFollowUpsRegisterRequest();
        printReport(response, "512-ExImShippedConsignmentFollowUps_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/512-ex-im-shipped-consignment-follow-ups-register", method = RequestMethod.GET)
    public void print512ExImShippedConsignmentFollowUpsRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print512ExImShippedConsignmentFollowUpsRegister(response, objectMapper.convertValue(query, Rpt512ExImShippedConsignmentFollowUpsRegisterRequest.class));
    }

    /**
     * Template: 513-invLabPreProductionSlip.rpt
     * Procedure: Sp_InvLabPreProductionExportLotInspectionHeader_SlipandRegister
     * Desktop: InvLabPreProductionExportLotInspectionHeader.InvLabPreProductionExportLotInspectionHeader_SlipandRegister
     */
    @RequestMapping(value = "/reports/print/513-inv-lab-pre-production-slip", method = RequestMethod.POST)
    public void print513InvLabPreProductionSlip(HttpServletResponse response, @RequestBody(required = false) Rpt513InvLabPreProductionSlipRequest request) throws Exception {
        if (request == null) request = new Rpt513InvLabPreProductionSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "513-invLabPreProductionSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/513-inv-lab-pre-production-slip", method = RequestMethod.GET)
    public void print513InvLabPreProductionSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print513InvLabPreProductionSlip(response, objectMapper.convertValue(query, Rpt513InvLabPreProductionSlipRequest.class));
    }

    /**
     * Template: 514_01_InventoryStockReservedSlip.rpt
     * Procedure: [dbo].[USP_InventoryStockReserved_SlipAndRegister]
     * Desktop: InventoryStockReserved.InventoryStockReserved_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/514-01-inventory-stock-reserved-slip", method = RequestMethod.POST)
    public void print51401InventoryStockReservedSlip(HttpServletResponse response, @RequestBody(required = false) Rpt51401InventoryStockReservedSlipRequest request) throws Exception {
        if (request == null) request = new Rpt51401InventoryStockReservedSlipRequest();
        printReport(response, "514_01_InventoryStockReservedSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/514-01-inventory-stock-reserved-slip", method = RequestMethod.GET)
    public void print51401InventoryStockReservedSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print51401InventoryStockReservedSlip(response, objectMapper.convertValue(query, Rpt51401InventoryStockReservedSlipRequest.class));
    }

    /**
     * Template: 514_01_ThirdPartyInspectionLotTrackingReport.rpt
     * Procedure: [dbo].[usp_ThirdPartyInspectionData_ForApproval]
     * Desktop: InvLabPreProductionExportLotInspectionHeader.ThirdPartyInspectionData_ForApproval
     */
    @RequestMapping(value = "/reports/print/514-01-third-party-inspection-lot-tracking-report", method = RequestMethod.POST)
    public void print51401ThirdPartyInspectionLotTrackingReport(HttpServletResponse response, @RequestBody(required = false) Rpt51401ThirdPartyInspectionLotTrackingReportRequest request) throws Exception {
        if (request == null) request = new Rpt51401ThirdPartyInspectionLotTrackingReportRequest();
        printReport(response, "514_01_ThirdPartyInspectionLotTrackingReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/514-01-third-party-inspection-lot-tracking-report", method = RequestMethod.GET)
    public void print51401ThirdPartyInspectionLotTrackingReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print51401ThirdPartyInspectionLotTrackingReport(response, objectMapper.convertValue(query, Rpt51401ThirdPartyInspectionLotTrackingReportRequest.class));
    }

    /**
     * Template: 514_ThirdPartyInspectionSlip.rpt
     * Procedure: [dbo].[usp_InvLabPreThirdPartyInspection_Slip]
     * Desktop: InvLabPreProductionExportLotInspectionHeader.InvLabPreThirdPartyInspection_Slip
     */
    @RequestMapping(value = "/reports/print/514-third-party-inspection-slip", method = RequestMethod.POST)
    public void print514ThirdPartyInspectionSlip(HttpServletResponse response, @RequestBody(required = false) Rpt514ThirdPartyInspectionSlipRequest request) throws Exception {
        if (request == null) request = new Rpt514ThirdPartyInspectionSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "514_ThirdPartyInspectionSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/514-third-party-inspection-slip", method = RequestMethod.GET)
    public void print514ThirdPartyInspectionSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print514ThirdPartyInspectionSlip(response, objectMapper.convertValue(query, Rpt514ThirdPartyInspectionSlipRequest.class));
    }

    /**
     * Template: 516-ExImRptFCBankReceipts.rpt
     * Procedure: Sp_Vouchers_PaymentReceiptVoucherSlip_Rpt
     * Desktop: VoucherReports.VoucherReport
     */
    @RequestMapping(value = "/reports/print/516-fc-bank-receipts", method = RequestMethod.POST)
    public void print516FCBankReceipts(HttpServletResponse response, @RequestBody(required = false) Rpt516FCBankReceiptsRequest request) throws Exception {
        if (request == null) request = new Rpt516FCBankReceiptsRequest();
        printReport(response, "516-ExImRptFCBankReceipts.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/516-fc-bank-receipts", method = RequestMethod.GET)
    public void print516FCBankReceiptsGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print516FCBankReceipts(response, objectMapper.convertValue(query, Rpt516FCBankReceiptsRequest.class));
    }

    /**
     * Template: 518-ExpRptSalesContractExportRegister.rpt
     * Procedure: Sp_ExImLcOrderNo_ExportSlip_Rpt
     * Desktop: ExportPdfReport.ExpRptSalesContractExportRegister_518
     */
    @RequestMapping(value = "/reports/print/518-sales-contract-export-register", method = RequestMethod.POST)
    public void print518SalesContractExportRegister(HttpServletResponse response, @RequestBody(required = false) Rpt518SalesContractExportRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt518SalesContractExportRegisterRequest();
        printReport(response, "518-ExpRptSalesContractExportRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/518-sales-contract-export-register", method = RequestMethod.GET)
    public void print518SalesContractExportRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print518SalesContractExportRegister(response, objectMapper.convertValue(query, Rpt518SalesContractExportRegisterRequest.class));
    }

    /**
     * Template: 521-ExportInvoiceSlip.rpt
     * Procedure: Sp_ExImInvoice_SlipAndRegister_Rpt
     * Desktop: ExportPdfReport.InvoiceSlip521
     */
    @RequestMapping(value = "/reports/print/521-export-invoice-slip", method = RequestMethod.POST)
    public void print521ExportInvoiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt521ExportInvoiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt521ExportInvoiceSlipRequest();
        printReport(response, "521-ExportInvoiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/521-export-invoice-slip", method = RequestMethod.GET)
    public void print521ExportInvoiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print521ExportInvoiceSlip(response, objectMapper.convertValue(query, Rpt521ExportInvoiceSlipRequest.class));
    }

    /**
     * Template: 522-ExImInvoiceRegister.rpt
     * Procedure: Sp_ExImInvoice_ReceivedAndOutstandingHistory_Rpt
     * Desktop: ExImLcOrder.EximInvoiceRegister
     */
    @RequestMapping(value = "/reports/print/522-ex-im-invoice-register", method = RequestMethod.POST)
    public void print522ExImInvoiceRegister(HttpServletResponse response, @RequestBody(required = false) Rpt522ExImInvoiceRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt522ExImInvoiceRegisterRequest();
        printReport(response, "522-ExImInvoiceRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/522-ex-im-invoice-register", method = RequestMethod.GET)
    public void print522ExImInvoiceRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print522ExImInvoiceRegister(response, objectMapper.convertValue(query, Rpt522ExImInvoiceRegisterRequest.class));
    }

    /**
     * Template: 523-ExpRptSalesContractWiseInvoiceRegister.rpt
     * Procedure: SpExImInvoice_ExportInvoiceRegisterByLcContract_Rpt
     * Desktop: ExportPdfReport.ExpRptSalesContractExportRegister_523
     */
    @RequestMapping(value = "/reports/print/523-sales-contract-wise-invoice-register", method = RequestMethod.POST)
    public void print523SalesContractWiseInvoiceRegister(HttpServletResponse response, @RequestBody(required = false) Rpt523SalesContractWiseInvoiceRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt523SalesContractWiseInvoiceRegisterRequest();
        printReport(response, "523-ExpRptSalesContractWiseInvoiceRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/523-sales-contract-wise-invoice-register", method = RequestMethod.GET)
    public void print523SalesContractWiseInvoiceRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print523SalesContractWiseInvoiceRegister(response, objectMapper.convertValue(query, Rpt523SalesContractWiseInvoiceRegisterRequest.class));
    }

    /**
     * Template: 526-SaleContractRegister.rpt
     * Procedure: Sp_ExImLcOrder_ExportRegister_Rpt
     * Desktop: ExImLcOrder.EximLcOrder_Register
     */
    @RequestMapping(value = "/reports/print/526-sale-contract-register", method = RequestMethod.POST)
    public void print526SaleContractRegister(HttpServletResponse response, @RequestBody(required = false) Rpt526SaleContractRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt526SaleContractRegisterRequest();
        printReport(response, "526-SaleContractRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/526-sale-contract-register", method = RequestMethod.GET)
    public void print526SaleContractRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print526SaleContractRegister(response, objectMapper.convertValue(query, Rpt526SaleContractRegisterRequest.class));
    }

    /**
     * Template: 527-ExImLcOrderShipmentScheduleDetailRegister.rpt
     * Procedure: Sp_ExImLcOrderNo_ExportSlip_Rpt
     * Desktop: ExportPdfReport.ExpRptSalesContractExportRegister_518
     */
    @RequestMapping(value = "/reports/print/527-ex-im-lc-order-shipment-schedule-detail-register", method = RequestMethod.POST)
    public void print527ExImLcOrderShipmentScheduleDetailRegister(HttpServletResponse response, @RequestBody(required = false) Rpt527ExImLcOrderShipmentScheduleDetailRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt527ExImLcOrderShipmentScheduleDetailRegisterRequest();
        printReport(response, "527-ExImLcOrderShipmentScheduleDetailRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/527-ex-im-lc-order-shipment-schedule-detail-register", method = RequestMethod.GET)
    public void print527ExImLcOrderShipmentScheduleDetailRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print527ExImLcOrderShipmentScheduleDetailRegister(response, objectMapper.convertValue(query, Rpt527ExImLcOrderShipmentScheduleDetailRegisterRequest.class));
    }

    /**
     * Template: 527-ExImLcOrderShipmentScheduleDetailRegisterCustomerWise.rpt
     * Procedure: SpExImInvoice_ExportInvoiceRegisterByLcContract_Rpt
     * Desktop: ExportPdfReport.ExpRptSalesContractExportRegister_523
     */
    @RequestMapping(value = "/reports/print/527-ex-im-lc-order-shipment-schedule-detail-register-customer-wise", method = RequestMethod.POST)
    public void print527ExImLcOrderShipmentScheduleDetailRegisterCustomerWise(HttpServletResponse response, @RequestBody(required = false) Rpt527ExImLcOrderShipmentScheduleDetailRegisterCustomerWiseRequest request) throws Exception {
        if (request == null) request = new Rpt527ExImLcOrderShipmentScheduleDetailRegisterCustomerWiseRequest();
        printReport(response, "527-ExImLcOrderShipmentScheduleDetailRegisterCustomerWise.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/527-ex-im-lc-order-shipment-schedule-detail-register-customer-wise", method = RequestMethod.GET)
    public void print527ExImLcOrderShipmentScheduleDetailRegisterCustomerWiseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print527ExImLcOrderShipmentScheduleDetailRegisterCustomerWise(response, objectMapper.convertValue(query, Rpt527ExImLcOrderShipmentScheduleDetailRegisterCustomerWiseRequest.class));
    }

    /**
     * Template: 527-ExportDeliveryOrderByInvoiceIdSlip.rpt
     * Procedure: Sp_InvDeliveryOrder_ExportSlipByInvoice
     * Desktop: InvDeliveryOrder.DeliveryOrderExortSlip_ByInvoiceId
     */
    @RequestMapping(value = "/reports/print/527-export-delivery-order-by-invoice-id-slip", method = RequestMethod.POST)
    public void print527ExportDeliveryOrderByInvoiceIdSlip(HttpServletResponse response, @RequestBody(required = false) Rpt527ExportDeliveryOrderByInvoiceIdSlipRequest request) throws Exception {
        if (request == null) request = new Rpt527ExportDeliveryOrderByInvoiceIdSlipRequest();
        printReport(response, "527-ExportDeliveryOrderByInvoiceIdSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/527-export-delivery-order-by-invoice-id-slip", method = RequestMethod.GET)
    public void print527ExportDeliveryOrderByInvoiceIdSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print527ExportDeliveryOrderByInvoiceIdSlip(response, objectMapper.convertValue(query, Rpt527ExportDeliveryOrderByInvoiceIdSlipRequest.class));
    }

    /**
     * Template: 529-ExportInvoiceSlipPackingList.rpt
     * Procedure: Sp_ExImInvoice_SlipAndRegister_Rpt
     * Desktop: ExportPdfReport.InvoiceSlip521
     */
    @RequestMapping(value = "/reports/print/529-export-invoice-slip-packing-list", method = RequestMethod.POST)
    public void print529ExportInvoiceSlipPackingList(HttpServletResponse response, @RequestBody(required = false) Rpt529ExportInvoiceSlipPackingListRequest request) throws Exception {
        if (request == null) request = new Rpt529ExportInvoiceSlipPackingListRequest();
        printReport(response, "529-ExportInvoiceSlipPackingList.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/529-export-invoice-slip-packing-list", method = RequestMethod.GET)
    public void print529ExportInvoiceSlipPackingListGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print529ExportInvoiceSlipPackingList(response, objectMapper.convertValue(query, Rpt529ExportInvoiceSlipPackingListRequest.class));
    }

    /**
     * Template: 529-SubRpt-ContainerListByInvoice.rpt
     * Procedure: SpExImInvoiceGetContainersList
     * Desktop: ExportPdfReport.ExportInvoiceSubReportContainerForPackingliSt529
     */
    @RequestMapping(value = "/reports/print/529-sub-container-list-by-invoice", method = RequestMethod.POST)
    public void print529SubContainerListByInvoice(HttpServletResponse response, @RequestBody(required = false) Rpt529SubContainerListByInvoiceRequest request) throws Exception {
        if (request == null) request = new Rpt529SubContainerListByInvoiceRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "529-SubRpt-ContainerListByInvoice.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/529-sub-container-list-by-invoice", method = RequestMethod.GET)
    public void print529SubContainerListByInvoiceGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print529SubContainerListByInvoice(response, objectMapper.convertValue(query, Rpt529SubContainerListByInvoiceRequest.class));
    }

    /**
     * Template: 530-EximBillOfLadingSlip.rpt
     * Procedure: Sp_ExImInvoice_SlipAndRegister_Rpt
     * Desktop: ExportPdfReport.EximInvoiceSlip_520
     */
    @RequestMapping(value = "/reports/print/530-exim-bill-of-lading-slip", method = RequestMethod.POST)
    public void print530EximBillOfLadingSlip(HttpServletResponse response, @RequestBody(required = false) Rpt530EximBillOfLadingSlipRequest request) throws Exception {
        if (request == null) request = new Rpt530EximBillOfLadingSlipRequest();
        printReport(response, "530-EximBillOfLadingSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/530-exim-bill-of-lading-slip", method = RequestMethod.GET)
    public void print530EximBillOfLadingSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print530EximBillOfLadingSlip(response, objectMapper.convertValue(query, Rpt530EximBillOfLadingSlipRequest.class));
    }

    /**
     * Template: 531-ExImInvoiceSummeryByMonth.rpt
     * Procedure: SpExImInvoiceSummaryByMonthly_Report
     * Desktop: ExportReports.SpExImInvoiceSummaryByMonth
     */
    @RequestMapping(value = "/reports/print/531-ex-im-invoice-summery-by-month", method = RequestMethod.POST)
    public void print531ExImInvoiceSummeryByMonth(HttpServletResponse response, @RequestBody(required = false) Rpt531ExImInvoiceSummeryByMonthRequest request) throws Exception {
        if (request == null) request = new Rpt531ExImInvoiceSummeryByMonthRequest();
        printReport(response, "531-ExImInvoiceSummeryByMonth.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/531-ex-im-invoice-summery-by-month", method = RequestMethod.GET)
    public void print531ExImInvoiceSummeryByMonthGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print531ExImInvoiceSummeryByMonth(response, objectMapper.convertValue(query, Rpt531ExImInvoiceSummeryByMonthRequest.class));
    }

    /**
     * Template: 537-ExportDetailHistoryReport.rpt
     * Procedure: SpExImInvoice_ExportHistoryDetail_Report
     * Desktop: ExportReports.ExportInvoiceHistoryDetailReport
     */
    @RequestMapping(value = "/reports/print/537-export-detail-history-report", method = RequestMethod.POST)
    public void print537ExportDetailHistoryReport(HttpServletResponse response, @RequestBody(required = false) Rpt537ExportDetailHistoryReportRequest request) throws Exception {
        if (request == null) request = new Rpt537ExportDetailHistoryReportRequest();
        printReport(response, "537-ExportDetailHistoryReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/537-export-detail-history-report", method = RequestMethod.GET)
    public void print537ExportDetailHistoryReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print537ExportDetailHistoryReport(response, objectMapper.convertValue(query, Rpt537ExportDetailHistoryReportRequest.class));
    }

    /**
     * Template: 538-ExportSummariesReport.rpt
     * Procedure: SpExImInvoice_ExportsSummery_Reports
     * Desktop: ExportReports.ExportSummariesReport
     */
    @RequestMapping(value = "/reports/print/538-export-summaries-report", method = RequestMethod.POST)
    public void print538ExportSummariesReport(HttpServletResponse response, @RequestBody(required = false) Rpt538ExportSummariesReportRequest request) throws Exception {
        if (request == null) request = new Rpt538ExportSummariesReportRequest();
        printReport(response, "538-ExportSummariesReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/538-export-summaries-report", method = RequestMethod.GET)
    public void print538ExportSummariesReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print538ExportSummariesReport(response, objectMapper.convertValue(query, Rpt538ExportSummariesReportRequest.class));
    }

    /**
     * Template: 538-ExportSummaryByCustomer.rpt
     * Procedure: SpExImInvoice_ExportsSummery_Reports
     * Desktop: ExportReports.ExportSummariesReport
     */
    @RequestMapping(value = "/reports/print/538-export-summary-by-customer", method = RequestMethod.POST)
    public void print538ExportSummaryByCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt538ExportSummaryByCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt538ExportSummaryByCustomerRequest();
        printReport(response, "538-ExportSummaryByCustomer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/538-export-summary-by-customer", method = RequestMethod.GET)
    public void print538ExportSummaryByCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print538ExportSummaryByCustomer(response, objectMapper.convertValue(query, Rpt538ExportSummaryByCustomerRequest.class));
    }

    /**
     * Template: 538_01-ExportSummaryByItem.rpt
     * Procedure: SpExImInvoice_ExportsSummery_Reports
     * Desktop: ExportReports.ExportSummariesReport
     */
    @RequestMapping(value = "/reports/print/538-01-export-summary-by-item", method = RequestMethod.POST)
    public void print53801ExportSummaryByItem(HttpServletResponse response, @RequestBody(required = false) Rpt53801ExportSummaryByItemRequest request) throws Exception {
        if (request == null) request = new Rpt53801ExportSummaryByItemRequest();
        printReport(response, "538_01-ExportSummaryByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/538-01-export-summary-by-item", method = RequestMethod.GET)
    public void print53801ExportSummaryByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print53801ExportSummaryByItem(response, objectMapper.convertValue(query, Rpt53801ExportSummaryByItemRequest.class));
    }

    /**
     * Template: 538_02-ExportSummaryByPort.rpt
     * Procedure: SpExImInvoice_ExportsSummery_Reports
     * Desktop: ExportReports.ExportSummariesReport
     */
    @RequestMapping(value = "/reports/print/538-02-export-summary-by-port", method = RequestMethod.POST)
    public void print53802ExportSummaryByPort(HttpServletResponse response, @RequestBody(required = false) Rpt53802ExportSummaryByPortRequest request) throws Exception {
        if (request == null) request = new Rpt53802ExportSummaryByPortRequest();
        printReport(response, "538_02-ExportSummaryByPort.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/538-02-export-summary-by-port", method = RequestMethod.GET)
    public void print53802ExportSummaryByPortGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print53802ExportSummaryByPort(response, objectMapper.convertValue(query, Rpt53802ExportSummaryByPortRequest.class));
    }

    /**
     * Template: 540-RptServiceBillSlip.rpt
     * Procedure: Sp_ExImClearingAgentBill_Rpt
     * Desktop: ServicesReports.ServicesBill_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/540-service-bill-slip", method = RequestMethod.POST)
    public void print540ServiceBillSlip(HttpServletResponse response, @RequestBody(required = false) Rpt540ServiceBillSlipRequest request) throws Exception {
        if (request == null) request = new Rpt540ServiceBillSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "540-RptServiceBillSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/540-service-bill-slip", method = RequestMethod.GET)
    public void print540ServiceBillSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print540ServiceBillSlip(response, objectMapper.convertValue(query, Rpt540ServiceBillSlipRequest.class));
    }

    /**
     * Template: 541-ServiceBillRegister.rpt
     * Procedure: Sp_ExImClearingAgentBill_Rpt
     * Desktop: ServicesReports.ServicesBill_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/541-service-bill-register", method = RequestMethod.POST)
    public void print541ServiceBillRegister(HttpServletResponse response, @RequestBody(required = false) Rpt541ServiceBillRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt541ServiceBillRegisterRequest();
        printReport(response, "541-ServiceBillRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/541-service-bill-register", method = RequestMethod.GET)
    public void print541ServiceBillRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print541ServiceBillRegister(response, objectMapper.convertValue(query, Rpt541ServiceBillRegisterRequest.class));
    }

    /**
     * Template: 542-ShipmentCostingRegister.rpt
     * Procedure: SpExport_ShipmentCosting_Report
     * Desktop: ExportReports.EximShipmentCostingReport
     */
    @RequestMapping(value = "/reports/print/542-shipment-costing-register", method = RequestMethod.POST)
    public void print542ShipmentCostingRegister(HttpServletResponse response, @RequestBody(required = false) Rpt542ShipmentCostingRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt542ShipmentCostingRegisterRequest();
        printReport(response, "542-ShipmentCostingRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/542-shipment-costing-register", method = RequestMethod.GET)
    public void print542ShipmentCostingRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print542ShipmentCostingRegister(response, objectMapper.convertValue(query, Rpt542ShipmentCostingRegisterRequest.class));
    }

    /**
     * Template: 542_01-ShipmentCostingSummary.rpt
     * Procedure: USP_ExportShipmentCosting_SummaryReport
     * Desktop: ExportReports.ExportShipmentCosting_SummaryReport
     */
    @RequestMapping(value = "/reports/print/542-01-shipment-costing-summary", method = RequestMethod.POST)
    public void print54201ShipmentCostingSummary(HttpServletResponse response, @RequestBody(required = false) Rpt54201ShipmentCostingSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt54201ShipmentCostingSummaryRequest();
        printReport(response, "542_01-ShipmentCostingSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/542-01-shipment-costing-summary", method = RequestMethod.GET)
    public void print54201ShipmentCostingSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print54201ShipmentCostingSummary(response, objectMapper.convertValue(query, Rpt54201ShipmentCostingSummaryRequest.class));
    }

    /**
     * Template: 543-USP_ExportDetailByContract_ReportWithContainers.rpt
     * Procedure: USP_ExportDetailByContract_Report
     * Desktop: ExImLcOrder.ExportShipmentDetailReportByContract
     */
    @RequestMapping(value = "/reports/print/543-usp-export-detail-by-contract-report-with-containers", method = RequestMethod.POST)
    public void print543USPExportDetailByContractReportWithContainers(HttpServletResponse response, @RequestBody(required = false) Rpt543USPExportDetailByContractReportWithContainersRequest request) throws Exception {
        if (request == null) request = new Rpt543USPExportDetailByContractReportWithContainersRequest();
        printReport(response, "543-USP_ExportDetailByContract_ReportWithContainers.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/543-usp-export-detail-by-contract-report-with-containers", method = RequestMethod.GET)
    public void print543USPExportDetailByContractReportWithContainersGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print543USPExportDetailByContractReportWithContainers(response, objectMapper.convertValue(query, Rpt543USPExportDetailByContractReportWithContainersRequest.class));
    }

    /**
     * Template: 544-USP_ExportDetailByContract_ReportWithoutContainers.rpt
     * Procedure: USP_ExportDetailByContract_Report
     * Desktop: ExImLcOrder.ExportShipmentDetailReportByContract
     */
    @RequestMapping(value = "/reports/print/544-usp-export-detail-by-contract-report-without-containers", method = RequestMethod.POST)
    public void print544USPExportDetailByContractReportWithoutContainers(HttpServletResponse response, @RequestBody(required = false) Rpt544USPExportDetailByContractReportWithoutContainersRequest request) throws Exception {
        if (request == null) request = new Rpt544USPExportDetailByContractReportWithoutContainersRequest();
        printReport(response, "544-USP_ExportDetailByContract_ReportWithoutContainers.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/544-usp-export-detail-by-contract-report-without-containers", method = RequestMethod.GET)
    public void print544USPExportDetailByContractReportWithoutContainersGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print544USPExportDetailByContractReportWithoutContainers(response, objectMapper.convertValue(query, Rpt544USPExportDetailByContractReportWithoutContainersRequest.class));
    }

    /**
     * Template: 546-ExportPreInvoiceSlip.rpt
     * Procedure: [dbo].[USP_PreInvoice_SlipAndRegister_Rpt]
     * Desktop: ExportPdfReport.ExportPreInvoiceSlip
     */
    @RequestMapping(value = "/reports/print/546-export-pre-invoice-slip", method = RequestMethod.POST)
    public void print546ExportPreInvoiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt546ExportPreInvoiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt546ExportPreInvoiceSlipRequest();
        printReport(response, "546-ExportPreInvoiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/546-export-pre-invoice-slip", method = RequestMethod.GET)
    public void print546ExportPreInvoiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print546ExportPreInvoiceSlip(response, objectMapper.convertValue(query, Rpt546ExportPreInvoiceSlipRequest.class));
    }

    /**
     * Template: 546-ExportPreInvoiceSlipForBank.rpt
     * Procedure: [dbo].[USP_PreInvoice_SlipAndRegister_Rpt]
     * Desktop: ExportPdfReport.ExportPreInvoiceSlip
     */
    @RequestMapping(value = "/reports/print/546-export-pre-invoice-slip-for-bank", method = RequestMethod.POST)
    public void print546ExportPreInvoiceSlipForBank(HttpServletResponse response, @RequestBody(required = false) Rpt546ExportPreInvoiceSlipForBankRequest request) throws Exception {
        if (request == null) request = new Rpt546ExportPreInvoiceSlipForBankRequest();
        printReport(response, "546-ExportPreInvoiceSlipForBank.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/546-export-pre-invoice-slip-for-bank", method = RequestMethod.GET)
    public void print546ExportPreInvoiceSlipForBankGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print546ExportPreInvoiceSlipForBank(response, objectMapper.convertValue(query, Rpt546ExportPreInvoiceSlipForBankRequest.class));
    }

    /**
     * Template: 548-ExportCommercialInvoiceSlipForBank.rpt
     * Procedure: [dbo].[USP_PreInvoice_SlipAndRegister_Rpt]
     * Desktop: ExportPdfReport.ExportPreInvoiceSlip
     */
    @RequestMapping(value = "/reports/print/548-export-commercial-invoice-slip-for-bank", method = RequestMethod.POST)
    public void print548ExportCommercialInvoiceSlipForBank(HttpServletResponse response, @RequestBody(required = false) Rpt548ExportCommercialInvoiceSlipForBankRequest request) throws Exception {
        if (request == null) request = new Rpt548ExportCommercialInvoiceSlipForBankRequest();
        printReport(response, "548-ExportCommercialInvoiceSlipForBank.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/548-export-commercial-invoice-slip-for-bank", method = RequestMethod.GET)
    public void print548ExportCommercialInvoiceSlipForBankGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print548ExportCommercialInvoiceSlipForBank(response, objectMapper.convertValue(query, Rpt548ExportCommercialInvoiceSlipForBankRequest.class));
    }

    /**
     * Template: 548A-ExportCommercialInvoiceSlipForBank.rpt
     * Procedure: [dbo].[USP_PreInvoice_SlipAndRegister_Rpt]
     * Desktop: ExportPdfReport.ExportPreInvoiceSlip
     */
    @RequestMapping(value = "/reports/print/548a-export-commercial-invoice-slip-for-bank", method = RequestMethod.POST)
    public void print548AExportCommercialInvoiceSlipForBank(HttpServletResponse response, @RequestBody(required = false) Rpt548AExportCommercialInvoiceSlipForBankRequest request) throws Exception {
        if (request == null) request = new Rpt548AExportCommercialInvoiceSlipForBankRequest();
        printReport(response, "548A-ExportCommercialInvoiceSlipForBank.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/548a-export-commercial-invoice-slip-for-bank", method = RequestMethod.GET)
    public void print548AExportCommercialInvoiceSlipForBankGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print548AExportCommercialInvoiceSlipForBank(response, objectMapper.convertValue(query, Rpt548AExportCommercialInvoiceSlipForBankRequest.class));
    }

    /**
     * Template: 549-ExportCommercialInvoiceSlip.rpt
     * Procedure: [dbo].[USP_PreInvoice_SlipAndRegister_Rpt]
     * Desktop: ExportPdfReport.ExportPreInvoiceSlip
     */
    @RequestMapping(value = "/reports/print/549-export-commercial-invoice-slip", method = RequestMethod.POST)
    public void print549ExportCommercialInvoiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt549ExportCommercialInvoiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt549ExportCommercialInvoiceSlipRequest();
        printReport(response, "549-ExportCommercialInvoiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/549-export-commercial-invoice-slip", method = RequestMethod.GET)
    public void print549ExportCommercialInvoiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print549ExportCommercialInvoiceSlip(response, objectMapper.convertValue(query, Rpt549ExportCommercialInvoiceSlipRequest.class));
    }

    /**
     * Template: 549A-ExportCommercialInvoiceSlip.rpt
     * Procedure: [dbo].[USP_PreInvoice_SlipAndRegister_Rpt]
     * Desktop: ExportPdfReport.ExportPreInvoiceSlip
     */
    @RequestMapping(value = "/reports/print/549a-export-commercial-invoice-slip", method = RequestMethod.POST)
    public void print549AExportCommercialInvoiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt549AExportCommercialInvoiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt549AExportCommercialInvoiceSlipRequest();
        printReport(response, "549A-ExportCommercialInvoiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/549a-export-commercial-invoice-slip", method = RequestMethod.GET)
    public void print549AExportCommercialInvoiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print549AExportCommercialInvoiceSlip(response, objectMapper.convertValue(query, Rpt549AExportCommercialInvoiceSlipRequest.class));
    }

    /**
     * Template: 550-CommercialInvoicePackingDetailList_Slip.rpt
     * Procedure: [dbo].[USP_CommercialInvoicePackingDetailList_Slip]
     * Desktop: ExportPdfReport.CommercialInvoicePackingDetailListSlip
     */
    @RequestMapping(value = "/reports/print/550-commercial-invoice-packing-detail-list-slip", method = RequestMethod.POST)
    public void print550CommercialInvoicePackingDetailListSlip(HttpServletResponse response, @RequestBody(required = false) Rpt550CommercialInvoicePackingDetailListSlipRequest request) throws Exception {
        if (request == null) request = new Rpt550CommercialInvoicePackingDetailListSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "550-CommercialInvoicePackingDetailList_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/550-commercial-invoice-packing-detail-list-slip", method = RequestMethod.GET)
    public void print550CommercialInvoicePackingDetailListSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print550CommercialInvoicePackingDetailListSlip(response, objectMapper.convertValue(query, Rpt550CommercialInvoicePackingDetailListSlipRequest.class));
    }

    /**
     * Template: 551-ContractSchedule_FormHistory.rpt
     * Procedure: USP_ContractSchedule_FormHistory
     * Desktop: ExImLcOrderShipmentSchedule.ContractSchedule_FormHistory
     */
    @RequestMapping(value = "/reports/print/551-contract-schedule-form-history", method = RequestMethod.POST)
    public void print551ContractScheduleFormHistory(HttpServletResponse response, @RequestBody(required = false) Rpt551ContractScheduleFormHistoryRequest request) throws Exception {
        if (request == null) request = new Rpt551ContractScheduleFormHistoryRequest();
        printReport(response, "551-ContractSchedule_FormHistory.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/551-contract-schedule-form-history", method = RequestMethod.GET)
    public void print551ContractScheduleFormHistoryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print551ContractScheduleFormHistory(response, objectMapper.convertValue(query, Rpt551ContractScheduleFormHistoryRequest.class));
    }

    /**
     * Template: 551_01_ContractSchedule_StatusReport.rpt
     * Procedure: USP_ContractSchedule_StatusReport
     * Desktop: ExImLcOrderShipmentSchedule.ContractSchedule_StatusReport
     */
    @RequestMapping(value = "/reports/print/551-01-contract-schedule-status-report", method = RequestMethod.POST)
    public void print55101ContractScheduleStatusReport(HttpServletResponse response, @RequestBody(required = false) Rpt55101ContractScheduleStatusReportRequest request) throws Exception {
        if (request == null) request = new Rpt55101ContractScheduleStatusReportRequest();
        printReport(response, "551_01_ContractSchedule_StatusReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/551-01-contract-schedule-status-report", method = RequestMethod.GET)
    public void print55101ContractScheduleStatusReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print55101ContractScheduleStatusReport(response, objectMapper.convertValue(query, Rpt55101ContractScheduleStatusReportRequest.class));
    }

    /**
     * Template: 551_02_ExportContractScheduleLoadingDateItemWise_Slip.rpt
     * Procedure: Usp_ExportContractSchedulePeriodicB
     * Desktop: ExImLcOrderShipmentSchedule.ExportContractSchedulePeriodicB
     */
    @RequestMapping(value = "/reports/print/551-02-export-contract-schedule-loading-date-item-wise-slip", method = RequestMethod.POST)
    public void print55102ExportContractScheduleLoadingDateItemWiseSlip(HttpServletResponse response, @RequestBody(required = false) Rpt55102ExportContractScheduleLoadingDateItemWiseSlipRequest request) throws Exception {
        if (request == null) request = new Rpt55102ExportContractScheduleLoadingDateItemWiseSlipRequest();
        printReport(response, "551_02_ExportContractScheduleLoadingDateItemWise_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/551-02-export-contract-schedule-loading-date-item-wise-slip", method = RequestMethod.GET)
    public void print55102ExportContractScheduleLoadingDateItemWiseSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print55102ExportContractScheduleLoadingDateItemWiseSlip(response, objectMapper.convertValue(query, Rpt55102ExportContractScheduleLoadingDateItemWiseSlipRequest.class));
    }

    /**
     * Template: 551_03_ExportContractScheduleLoadingDateCustomerItemWise_Slip.rpt
     * Procedure: Usp_ExportContractSchedulePeriodicB
     * Desktop: ExImLcOrderShipmentSchedule.ExportContractSchedulePeriodicB
     */
    @RequestMapping(value = "/reports/print/551-03-export-contract-schedule-loading-date-customer-item-wise-slip", method = RequestMethod.POST)
    public void print55103ExportContractScheduleLoadingDateCustomerItemWiseSlip(HttpServletResponse response, @RequestBody(required = false) Rpt55103ExportContractScheduleLoadingDateCustomerItemWiseSlipRequest request) throws Exception {
        if (request == null) request = new Rpt55103ExportContractScheduleLoadingDateCustomerItemWiseSlipRequest();
        printReport(response, "551_03_ExportContractScheduleLoadingDateCustomerItemWise_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/551-03-export-contract-schedule-loading-date-customer-item-wise-slip", method = RequestMethod.GET)
    public void print55103ExportContractScheduleLoadingDateCustomerItemWiseSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print55103ExportContractScheduleLoadingDateCustomerItemWiseSlip(response, objectMapper.convertValue(query, Rpt55103ExportContractScheduleLoadingDateCustomerItemWiseSlipRequest.class));
    }

    /**
     * Template: 555-PendingWorkExportRegister.rpt
     * Procedure: [dbo].[USP_PendingWorkExportRegister]
     * Desktop: ExImLcOrder.PendingWorkExportRegister
     */
    @RequestMapping(value = "/reports/print/555-pending-work-export-register", method = RequestMethod.POST)
    public void print555PendingWorkExportRegister(HttpServletResponse response, @RequestBody(required = false) Rpt555PendingWorkExportRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt555PendingWorkExportRegisterRequest();
        printReport(response, "555-PendingWorkExportRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/555-pending-work-export-register", method = RequestMethod.GET)
    public void print555PendingWorkExportRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print555PendingWorkExportRegister(response, objectMapper.convertValue(query, Rpt555PendingWorkExportRegisterRequest.class));
    }

    /**
     * Template: 556-ExportLoadSheet.rpt
     * Procedure: [dbo].[USP_ExportLoadSheet]
     * Desktop: ExportReports.ExportLoadingSheet
     */
    @RequestMapping(value = "/reports/print/556-export-load-sheet", method = RequestMethod.POST)
    public void print556ExportLoadSheet(HttpServletResponse response, @RequestBody(required = false) Rpt556ExportLoadSheetRequest request) throws Exception {
        if (request == null) request = new Rpt556ExportLoadSheetRequest();
        printReport(response, "556-ExportLoadSheet.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/556-export-load-sheet", method = RequestMethod.GET)
    public void print556ExportLoadSheetGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print556ExportLoadSheet(response, objectMapper.convertValue(query, Rpt556ExportLoadSheetRequest.class));
    }

    /**
     * Template: 557-ExImLcOrderNo_ExportSlip.rpt
     * Procedure: Sp_ExImLcOrderNo_ExportSlip_Rpt
     * Desktop: SaleContractReports.SaleContractReports501
     */
    @RequestMapping(value = "/reports/print/557-ex-im-lc-order-no-export-slip", method = RequestMethod.POST)
    public void print557ExImLcOrderNoExportSlip(HttpServletResponse response, @RequestBody(required = false) Rpt557ExImLcOrderNoExportSlipRequest request) throws Exception {
        if (request == null) request = new Rpt557ExImLcOrderNoExportSlipRequest();
        printReport(response, "557-ExImLcOrderNo_ExportSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/557-ex-im-lc-order-no-export-slip", method = RequestMethod.GET)
    public void print557ExImLcOrderNoExportSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print557ExImLcOrderNoExportSlip(response, objectMapper.convertValue(query, Rpt557ExImLcOrderNoExportSlipRequest.class));
    }

    /**
     * Template: 558-ExportInvoiceAgainstForwarding_Register.rpt
     * Procedure: USP_ExportInvoiceAgainstForwarding_Register
     * Desktop: ExImLcOrder.ExportInvoiceAgainstForwarding_Register
     */
    @RequestMapping(value = "/reports/print/558-export-invoice-against-forwarding-register", method = RequestMethod.POST)
    public void print558ExportInvoiceAgainstForwardingRegister(HttpServletResponse response, @RequestBody(required = false) Rpt558ExportInvoiceAgainstForwardingRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt558ExportInvoiceAgainstForwardingRegisterRequest();
        printReport(response, "558-ExportInvoiceAgainstForwarding_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/558-export-invoice-against-forwarding-register", method = RequestMethod.GET)
    public void print558ExportInvoiceAgainstForwardingRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print558ExportInvoiceAgainstForwardingRegister(response, objectMapper.convertValue(query, Rpt558ExportInvoiceAgainstForwardingRegisterRequest.class));
    }

    /**
     * Template: 559-GDBreakUpandRealized_Register.rpt
     * Procedure: USP_GDBreakUpandRealized_Register
     * Desktop: ExportReports.GDBreakUpandRealized_Register
     */
    @RequestMapping(value = "/reports/print/559-gd-break-upand-realized-register", method = RequestMethod.POST)
    public void print559GDBreakUpandRealizedRegister(HttpServletResponse response, @RequestBody(required = false) Rpt559GDBreakUpandRealizedRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt559GDBreakUpandRealizedRegisterRequest();
        printReport(response, "559-GDBreakUpandRealized_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/559-gd-break-upand-realized-register", method = RequestMethod.GET)
    public void print559GDBreakUpandRealizedRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print559GDBreakUpandRealizedRegister(response, objectMapper.convertValue(query, Rpt559GDBreakUpandRealizedRegisterRequest.class));
    }

    /**
     * Template: 560-ExBooking Info(CRO).rpt
     * Procedure: Proc_ExImExportShipingLineBooking_SlipAndRegister_Rpt
     * Desktop: ExImExportShipingLineBooking.PrintSlipandRegister
     */
    @RequestMapping(value = "/reports/print/560-ex-booking-info-cro", method = RequestMethod.POST)
    public void print560ExBookingInfoCRO(HttpServletResponse response, @RequestBody(required = false) Rpt560ExBookingInfoCRORequest request) throws Exception {
        if (request == null) request = new Rpt560ExBookingInfoCRORequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "560-ExBooking Info(CRO).rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/560-ex-booking-info-cro", method = RequestMethod.GET)
    public void print560ExBookingInfoCROGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print560ExBookingInfoCRO(response, objectMapper.convertValue(query, Rpt560ExBookingInfoCRORequest.class));
    }

    /**
     * Template: 560-GDBreakUpBankRequest_Slip.rpt
     * Procedure: USP_GDBreakUpBankRequest_SlipAndRegister
     * Desktop: GDBreakUpBankRequestHeader.GDBreakUpBankRequest_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/560-gd-break-up-bank-request-slip", method = RequestMethod.POST)
    public void print560GDBreakUpBankRequestSlip(HttpServletResponse response, @RequestBody(required = false) Rpt560GDBreakUpBankRequestSlipRequest request) throws Exception {
        if (request == null) request = new Rpt560GDBreakUpBankRequestSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "560-GDBreakUpBankRequest_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/560-gd-break-up-bank-request-slip", method = RequestMethod.GET)
    public void print560GDBreakUpBankRequestSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print560GDBreakUpBankRequestSlip(response, objectMapper.convertValue(query, Rpt560GDBreakUpBankRequestSlipRequest.class));
    }

    /**
     * Template: 561-SaleContractRegisterItemWise.rpt
     * Procedure: Sp_ExImLcOrder_ExportRegisterItemWise_Rpt
     * Desktop: ExImLcOrder.ExImLcOrder_ExportRegisterItemWise
     */
    @RequestMapping(value = "/reports/print/561-sale-contract-register-item-wise", method = RequestMethod.POST)
    public void print561SaleContractRegisterItemWise(HttpServletResponse response, @RequestBody(required = false) Rpt561SaleContractRegisterItemWiseRequest request) throws Exception {
        if (request == null) request = new Rpt561SaleContractRegisterItemWiseRequest();
        printReport(response, "561-SaleContractRegisterItemWise.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/561-sale-contract-register-item-wise", method = RequestMethod.GET)
    public void print561SaleContractRegisterItemWiseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print561SaleContractRegisterItemWise(response, objectMapper.convertValue(query, Rpt561SaleContractRegisterItemWiseRequest.class));
    }

    /**
     * Template: 563-PackingListRegister_Export.rpt
     * Procedure: USP_PackingListRegister_Export
     * Desktop: ExportReports.ExportPackingListRegister
     */
    @RequestMapping(value = "/reports/print/563-packing-list-register-export", method = RequestMethod.POST)
    public void print563PackingListRegisterExport(HttpServletResponse response, @RequestBody(required = false) Rpt563PackingListRegisterExportRequest request) throws Exception {
        if (request == null) request = new Rpt563PackingListRegisterExportRequest();
        printReport(response, "563-PackingListRegister_Export.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/563-packing-list-register-export", method = RequestMethod.GET)
    public void print563PackingListRegisterExportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print563PackingListRegisterExport(response, objectMapper.convertValue(query, Rpt563PackingListRegisterExportRequest.class));
    }

    /**
     * Template: 567-EEReport_ExportGD.rpt
     * Procedure: USP_EEReport_ExportGD
     * Desktop: ExportReports.EEReport_ExportGD
     */
    @RequestMapping(value = "/reports/print/567-ee-report-export-gd", method = RequestMethod.POST)
    public void print567EEReportExportGD(HttpServletResponse response, @RequestBody(required = false) Rpt567EEReportExportGDRequest request) throws Exception {
        if (request == null) request = new Rpt567EEReportExportGDRequest();
        printReport(response, "567-EEReport_ExportGD.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/567-ee-report-export-gd", method = RequestMethod.GET)
    public void print567EEReportExportGDGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print567EEReportExportGD(response, objectMapper.convertValue(query, Rpt567EEReportExportGDRequest.class));
    }

    /**
     * Template: 567_01-EEReport_ExportGD.rpt
     * Procedure: USP_EEReport_ExportGD
     * Desktop: ExportReports.EEReport_ExportGD
     */
    @RequestMapping(value = "/reports/print/567-01-ee-report-export-gd", method = RequestMethod.POST)
    public void print56701EEReportExportGD(HttpServletResponse response, @RequestBody(required = false) Rpt56701EEReportExportGDRequest request) throws Exception {
        if (request == null) request = new Rpt56701EEReportExportGDRequest();
        printReport(response, "567_01-EEReport_ExportGD.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/567-01-ee-report-export-gd", method = RequestMethod.GET)
    public void print56701EEReportExportGDGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print56701EEReportExportGD(response, objectMapper.convertValue(query, Rpt56701EEReportExportGDRequest.class));
    }

    /**
     * Template: 567_02-EEReport_ExportGD.rpt
     * Procedure: USP_EEReport_ExportGD
     * Desktop: ExportReports.EEReport_ExportGD
     */
    @RequestMapping(value = "/reports/print/567-02-ee-report-export-gd", method = RequestMethod.POST)
    public void print56702EEReportExportGD(HttpServletResponse response, @RequestBody(required = false) Rpt56702EEReportExportGDRequest request) throws Exception {
        if (request == null) request = new Rpt56702EEReportExportGDRequest();
        printReport(response, "567_02-EEReport_ExportGD.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/567-02-ee-report-export-gd", method = RequestMethod.GET)
    public void print56702EEReportExportGDGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print56702EEReportExportGD(response, objectMapper.convertValue(query, Rpt56702EEReportExportGDRequest.class));
    }

    /**
     * Template: 612-InvRptProductionDetailWithExpValues.rpt
     * Procedure: [dbo].[usp_ProductionOutputAllocationWithExportInvoice_SlipAndRegister]
     * Desktop: ProductionOutputAllocationWithExportInvoice.ProductionOutputAllocationWithExportInvoice_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/612-production-detail-with-exp-values", method = RequestMethod.POST)
    public void print612ProductionDetailWithExpValues(HttpServletResponse response, @RequestBody(required = false) Rpt612ProductionDetailWithExpValuesRequest request) throws Exception {
        if (request == null) request = new Rpt612ProductionDetailWithExpValuesRequest();
        printReport(response, "612-InvRptProductionDetailWithExpValues.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/612-production-detail-with-exp-values", method = RequestMethod.GET)
    public void print612ProductionDetailWithExpValuesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print612ProductionDetailWithExpValues(response, objectMapper.convertValue(query, Rpt612ProductionDetailWithExpValuesRequest.class));
    }

    /**
     * Template: 618-DoWeightWbWeightDiff.rpt
     * Procedure: SpEximInvoice_DoWeightWbWeightDiff_Rpt
     * Desktop: ExImInvoice.ExportSalesAuditByWeight
     */
    @RequestMapping(value = "/reports/print/618-do-weight-wb-weight-diff", method = RequestMethod.POST)
    public void print618DoWeightWbWeightDiff(HttpServletResponse response, @RequestBody(required = false) Rpt618DoWeightWbWeightDiffRequest request) throws Exception {
        if (request == null) request = new Rpt618DoWeightWbWeightDiffRequest();
        printReport(response, "618-DoWeightWbWeightDiff.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/618-do-weight-wb-weight-diff", method = RequestMethod.GET)
    public void print618DoWeightWbWeightDiffGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print618DoWeightWbWeightDiff(response, objectMapper.convertValue(query, Rpt618DoWeightWbWeightDiffRequest.class));
    }

    /**
     * Template: 901_02_VoucherInvoicesAdjustment_Slip.rpt
     * Procedure: [dbo].[USP_VoucherInvoicesAdjustment_ReadByVoucherHeadId]
     * Desktop: VoucherInvoicesAdjustment.GetByVoucherHeadId
     */
    @RequestMapping(value = "/reports/print/901-02-voucher-invoices-adjustment-slip", method = RequestMethod.POST)
    public void print90102VoucherInvoicesAdjustmentSlip(HttpServletResponse response, @RequestBody(required = false) Rpt90102VoucherInvoicesAdjustmentSlipRequest request) throws Exception {
        if (request == null) request = new Rpt90102VoucherInvoicesAdjustmentSlipRequest();
        printReport(response, "901_02_VoucherInvoicesAdjustment_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/901-02-voucher-invoices-adjustment-slip", method = RequestMethod.GET)
    public void print90102VoucherInvoicesAdjustmentSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print90102VoucherInvoicesAdjustmentSlip(response, objectMapper.convertValue(query, Rpt90102VoucherInvoicesAdjustmentSlipRequest.class));
    }

    /**
     * Template: 903-ImExShipmentBookingInfo.rpt
     * Procedure: [Imex].[USP_Get_ShipmentBookingReport]
     * Desktop: ShipmentBooking.ShipmentBookingReport
     */
    @RequestMapping(value = "/reports/print/903-im-ex-shipment-booking-info", method = RequestMethod.POST)
    public void print903ImExShipmentBookingInfo(HttpServletResponse response, @RequestBody(required = false) Rpt903ImExShipmentBookingInfoRequest request) throws Exception {
        if (request == null) request = new Rpt903ImExShipmentBookingInfoRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "903-ImExShipmentBookingInfo.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/903-im-ex-shipment-booking-info", method = RequestMethod.GET)
    public void print903ImExShipmentBookingInfoGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print903ImExShipmentBookingInfo(response, objectMapper.convertValue(query, Rpt903ImExShipmentBookingInfoRequest.class));
    }

    /**
     * Template: ExImLcContractPackingMaterialDetail_SubRpt.rpt
     * Procedure: USP_ExImLcContractPackingMaterialDetail_SubRpt
     * Desktop: SaleContractReports.ExImLcContractPackingMaterialDetail_SubRpt501
     */
    @RequestMapping(value = "/reports/print/ex-im-lc-contract-packing-material-detail-sub", method = RequestMethod.POST)
    public void printExImLcContractPackingMaterialDetailSub(HttpServletResponse response, @RequestBody(required = false) RptExImLcContractPackingMaterialDetailSubRequest request) throws Exception {
        if (request == null) request = new RptExImLcContractPackingMaterialDetailSubRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "ExImLcContractPackingMaterialDetail_SubRpt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/ex-im-lc-contract-packing-material-detail-sub", method = RequestMethod.GET)
    public void printExImLcContractPackingMaterialDetailSubGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printExImLcContractPackingMaterialDetailSub(response, objectMapper.convertValue(query, RptExImLcContractPackingMaterialDetailSubRequest.class));
    }

    /**
     * Template: ExImLcOrderPaymentTermsDetail_SubRpt.rpt
     * Procedure: USP_ExImLcOrderPaymentTermsDetail_SubRpt
     * Desktop: SaleContractReports.ExImLcOrderPaymentTermsDetail_SubRpt501
     */
    @RequestMapping(value = "/reports/print/ex-im-lc-order-payment-terms-detail-sub", method = RequestMethod.POST)
    public void printExImLcOrderPaymentTermsDetailSub(HttpServletResponse response, @RequestBody(required = false) RptExImLcOrderPaymentTermsDetailSubRequest request) throws Exception {
        if (request == null) request = new RptExImLcOrderPaymentTermsDetailSubRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "ExImLcOrderPaymentTermsDetail_SubRpt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/ex-im-lc-order-payment-terms-detail-sub", method = RequestMethod.GET)
    public void printExImLcOrderPaymentTermsDetailSubGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printExImLcOrderPaymentTermsDetailSub(response, objectMapper.convertValue(query, RptExImLcOrderPaymentTermsDetailSubRequest.class));
    }

    /**
     * Template: ExportInvoiceCustomExpense_SubRpt.rpt
     * Procedure: [ExportInvoiceCustomExpense_SubRpt]
     * Desktop: ExportPdfReport.CustomeInvoiceSlip521SupReprt
     */
    @RequestMapping(value = "/reports/print/export-invoice-custom-expense-sub", method = RequestMethod.POST)
    public void printExportInvoiceCustomExpenseSub(HttpServletResponse response, @RequestBody(required = false) RptExportInvoiceCustomExpenseSubRequest request) throws Exception {
        if (request == null) request = new RptExportInvoiceCustomExpenseSubRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "ExportInvoiceCustomExpense_SubRpt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/export-invoice-custom-expense-sub", method = RequestMethod.GET)
    public void printExportInvoiceCustomExpenseSubGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printExportInvoiceCustomExpenseSub(response, objectMapper.convertValue(query, RptExportInvoiceCustomExpenseSubRequest.class));
    }

    /**
     * Template: ExportInvoiceOtherExpense_SubRpt.rpt
     * Procedure: [ExportInvoiceOtherExpense_SubRpt]
     * Desktop: ExportPdfReport.InvoiceSlip521SupReprt
     */
    @RequestMapping(value = "/reports/print/export-invoice-other-expense-sub", method = RequestMethod.POST)
    public void printExportInvoiceOtherExpenseSub(HttpServletResponse response, @RequestBody(required = false) RptExportInvoiceOtherExpenseSubRequest request) throws Exception {
        if (request == null) request = new RptExportInvoiceOtherExpenseSubRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "ExportInvoiceOtherExpense_SubRpt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/export-invoice-other-expense-sub", method = RequestMethod.GET)
    public void printExportInvoiceOtherExpenseSubGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printExportInvoiceOtherExpenseSub(response, objectMapper.convertValue(query, RptExportInvoiceOtherExpenseSubRequest.class));
    }

    /**
     * Template: LcorderOtherItem_SubRpt.rpt
     * Procedure: USP_LcorderOtherItem_SubRpt
     * Desktop: SaleContractReports.LcorderOtherItem_SubRpt501
     */
    @RequestMapping(value = "/reports/print/lcorder-other-item-sub", method = RequestMethod.POST)
    public void printLcorderOtherItemSub(HttpServletResponse response, @RequestBody(required = false) RptLcorderOtherItemSubRequest request) throws Exception {
        if (request == null) request = new RptLcorderOtherItemSubRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "LcorderOtherItem_SubRpt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/lcorder-other-item-sub", method = RequestMethod.GET)
    public void printLcorderOtherItemSubGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printLcorderOtherItemSub(response, objectMapper.convertValue(query, RptLcorderOtherItemSubRequest.class));
    }

    /**
     * Template: LcOrderShipmentScheduleLcOrderWise_SubReport1.rpt
     * Procedure: [dbo].[USP_LcOrderShipmentScheduleLcOrderWise_Slip]
     * Desktop: ExportPdfReport.LcOrderShipmentScheduleLcOrderWise_Slip
     */
    @RequestMapping(value = "/reports/print/lc-order-shipment-schedule-lc-order-wise-sub-report-1", method = RequestMethod.POST)
    public void printLcOrderShipmentScheduleLcOrderWiseSubReport1(HttpServletResponse response, @RequestBody(required = false) RptLcOrderShipmentScheduleLcOrderWiseSubReport1Request request) throws Exception {
        if (request == null) request = new RptLcOrderShipmentScheduleLcOrderWiseSubReport1Request();
        printReport(response, "LcOrderShipmentScheduleLcOrderWise_SubReport1.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/lc-order-shipment-schedule-lc-order-wise-sub-report-1", method = RequestMethod.GET)
    public void printLcOrderShipmentScheduleLcOrderWiseSubReport1Get(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printLcOrderShipmentScheduleLcOrderWiseSubReport1(response, objectMapper.convertValue(query, RptLcOrderShipmentScheduleLcOrderWiseSubReport1Request.class));
    }

    /**
     * Template: LcOrderShipmentScheduleLcOrderWiseDetail_SubReport.rpt
     * Procedure: [dbo].[USP_LcOrderShipmentScheduleLcOrderWiseDetail_SubReport]
     * Desktop: ExportPdfReport.LcOrderShipmentScheduleLcOrderWiseDetail_SubReport
     */
    @RequestMapping(value = "/reports/print/lc-order-shipment-schedule-lc-order-wise-detail-sub-report", method = RequestMethod.POST)
    public void printLcOrderShipmentScheduleLcOrderWiseDetailSubReport(HttpServletResponse response, @RequestBody(required = false) RptLcOrderShipmentScheduleLcOrderWiseDetailSubReportRequest request) throws Exception {
        if (request == null) request = new RptLcOrderShipmentScheduleLcOrderWiseDetailSubReportRequest();
        printReport(response, "LcOrderShipmentScheduleLcOrderWiseDetail_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/lc-order-shipment-schedule-lc-order-wise-detail-sub-report", method = RequestMethod.GET)
    public void printLcOrderShipmentScheduleLcOrderWiseDetailSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printLcOrderShipmentScheduleLcOrderWiseDetailSubReport(response, objectMapper.convertValue(query, RptLcOrderShipmentScheduleLcOrderWiseDetailSubReportRequest.class));
    }

    /**
     * Template: PreCommercialInvoiceExpenseSubReport.rpt
     * Procedure: [ExportInvoiceOtherExpense_SubRpt]
     * Desktop: ExportPdfReport.InvoiceSlip521SupReprt
     */
    @RequestMapping(value = "/reports/print/pre-commercial-invoice-expense-sub-report", method = RequestMethod.POST)
    public void printPreCommercialInvoiceExpenseSubReport(HttpServletResponse response, @RequestBody(required = false) RptPreCommercialInvoiceExpenseSubReportRequest request) throws Exception {
        if (request == null) request = new RptPreCommercialInvoiceExpenseSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "PreCommercialInvoiceExpenseSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/pre-commercial-invoice-expense-sub-report", method = RequestMethod.GET)
    public void printPreCommercialInvoiceExpenseSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printPreCommercialInvoiceExpenseSubReport(response, objectMapper.convertValue(query, RptPreCommercialInvoiceExpenseSubReportRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
