package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.PurchasePrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** Purchase print actions. Generated from the verified seeder contracts. */
@Controller
public class PurchasePrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 211-InvRptGoodsReceiptsNotesRiceSlip.rpt
     * Procedure: Sp_InvGrn_RiceSlip_Rpt
     * Desktop: InvGrnandGdnReports.InvGrnSlip211
     */
    @RequestMapping(value = "/reports/print/211-goods-receipts-notes-rice-slip", method = RequestMethod.POST)
    public void print211GoodsReceiptsNotesRiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt211GoodsReceiptsNotesRiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt211GoodsReceiptsNotesRiceSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "211-InvRptGoodsReceiptsNotesRiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/211-goods-receipts-notes-rice-slip", method = RequestMethod.GET)
    public void print211GoodsReceiptsNotesRiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print211GoodsReceiptsNotesRiceSlip(response, objectMapper.convertValue(query, Rpt211GoodsReceiptsNotesRiceSlipRequest.class));
    }

    /**
     * Template: 251-InvRptInwardGatePassSlip.rpt
     * Procedure: Sp_GatePassInward_SlipAndRegister_Rpt
     * Desktop: GatePassInwardReports.GatePassInwardSlipandRegister
     */
    @RequestMapping(value = "/reports/print/251-inward-gate-pass-slip", method = RequestMethod.POST)
    public void print251InwardGatePassSlip(HttpServletResponse response, @RequestBody(required = false) Rpt251InwardGatePassSlipRequest request) throws Exception {
        if (request == null) request = new Rpt251InwardGatePassSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "251-InvRptInwardGatePassSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/251-inward-gate-pass-slip", method = RequestMethod.GET)
    public void print251InwardGatePassSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print251InwardGatePassSlip(response, objectMapper.convertValue(query, Rpt251InwardGatePassSlipRequest.class));
    }

    /**
     * Template: 203-InvRptPurchaseOrderRiceSlip.rpt
     * Procedure: Sp_PurchaseOrderSlip_Rpt
     * Desktop: PurchaseOrderReports.PurchaseOrderSlipReport203
     */
    @RequestMapping(value = "/reports/print/203-purchase-order-rice-slip", method = RequestMethod.POST)
    public void print203PurchaseOrderRiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt203PurchaseOrderRiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt203PurchaseOrderRiceSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "203-InvRptPurchaseOrderRiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/203-purchase-order-rice-slip", method = RequestMethod.GET)
    public void print203PurchaseOrderRiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print203PurchaseOrderRiceSlip(response, objectMapper.convertValue(query, Rpt203PurchaseOrderRiceSlipRequest.class));
    }

    /**
     * Template: 203A_PurchaseOrderSlip.rpt
     * Procedure: Sp_PurchaseOrderSlip_Rpt
     * Desktop: PurchaseOrderReports.PurchaseOrderSlipReport203
     */
    @RequestMapping(value = "/reports/print/203a-purchase-order-slip", method = RequestMethod.POST)
    public void print203APurchaseOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt203APurchaseOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt203APurchaseOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "203A_PurchaseOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/203a-purchase-order-slip", method = RequestMethod.GET)
    public void print203APurchaseOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print203APurchaseOrderSlip(response, objectMapper.convertValue(query, Rpt203APurchaseOrderSlipRequest.class));
    }

    /**
     * Template: 203_01_PurchaseOrderRiceSlip.rpt
     * Procedure: Sp_PurchaseOrderSlip_Rpt
     * Desktop: PurchaseOrderReports.PurchaseOrderSlipReport203
     */
    @RequestMapping(value = "/reports/print/203-01-purchase-order-rice-slip", method = RequestMethod.POST)
    public void print20301PurchaseOrderRiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt20301PurchaseOrderRiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt20301PurchaseOrderRiceSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "203_01_PurchaseOrderRiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/203-01-purchase-order-rice-slip", method = RequestMethod.GET)
    public void print20301PurchaseOrderRiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print20301PurchaseOrderRiceSlip(response, objectMapper.convertValue(query, Rpt20301PurchaseOrderRiceSlipRequest.class));
    }

    /**
     * Template: 203_02_PurchaseOrderRiceSlip.rpt
     * Procedure: Sp_PurchaseOrderSlip_Rpt
     * Desktop: PurchaseOrderReports.PurchaseOrderSlipReport203
     */
    @RequestMapping(value = "/reports/print/203-02-purchase-order-rice-slip", method = RequestMethod.POST)
    public void print20302PurchaseOrderRiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt20302PurchaseOrderRiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt20302PurchaseOrderRiceSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "203_02_PurchaseOrderRiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/203-02-purchase-order-rice-slip", method = RequestMethod.GET)
    public void print20302PurchaseOrderRiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print20302PurchaseOrderRiceSlip(response, objectMapper.convertValue(query, Rpt20302PurchaseOrderRiceSlipRequest.class));
    }

    /**
     * Template: 257-InwardGatePassWithWbAndLabSlip.rpt
     * Procedure: Sp_GatePassInward_SlipAndRegister_Rpt
     * Desktop: GatePassInwardReports.GatePassInwardSlipandRegister
     */
    @RequestMapping(value = "/reports/print/257-inward-gate-pass-with-wb-and-lab-slip", method = RequestMethod.POST)
    public void print257InwardGatePassWithWbAndLabSlip(HttpServletResponse response, @RequestBody(required = false) Rpt257InwardGatePassWithWbAndLabSlipRequest request) throws Exception {
        if (request == null) request = new Rpt257InwardGatePassWithWbAndLabSlipRequest();
        printReport(response, "257-InwardGatePassWithWbAndLabSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/257-inward-gate-pass-with-wb-and-lab-slip", method = RequestMethod.GET)
    public void print257InwardGatePassWithWbAndLabSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print257InwardGatePassWithWbAndLabSlip(response, objectMapper.convertValue(query, Rpt257InwardGatePassWithWbAndLabSlipRequest.class));
    }

    /**
     * Template: 653-RptInvLabPurchaseAnalysisSlip.rpt
     * Procedure: Sp_InvLabAnalysisPurchaseSlip_Rpt
     * Desktop: InvLabAnalysisPurchaseHeader.RptInvLabPurchaseAnalysisSlip653
     */
    @RequestMapping(value = "/reports/print/653-inv-lab-purchase-analysis-slip", method = RequestMethod.POST)
    public void print653InvLabPurchaseAnalysisSlip(HttpServletResponse response, @RequestBody(required = false) Rpt653InvLabPurchaseAnalysisSlipRequest request) throws Exception {
        if (request == null) request = new Rpt653InvLabPurchaseAnalysisSlipRequest();
        printReport(response, "653-RptInvLabPurchaseAnalysisSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/653-inv-lab-purchase-analysis-slip", method = RequestMethod.GET)
    public void print653InvLabPurchaseAnalysisSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print653InvLabPurchaseAnalysisSlip(response, objectMapper.convertValue(query, Rpt653InvLabPurchaseAnalysisSlipRequest.class));
    }

    /**
     * Template: 225-InvRepPurchaseBillDirectWithoutPo.rpt
     * Procedure: sp_InvpurchaseInvoiceDirectSlip
     * Desktop: InvPurchaseInvoiceReports.PurchaseInvoiceDirectSlip_225
     */
    @RequestMapping(value = "/reports/print/225-inv-rep-purchase-bill-direct-without-po", method = RequestMethod.POST)
    public void print225InvRepPurchaseBillDirectWithoutPo(HttpServletResponse response, @RequestBody(required = false) Rpt225InvRepPurchaseBillDirectWithoutPoRequest request) throws Exception {
        if (request == null) request = new Rpt225InvRepPurchaseBillDirectWithoutPoRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "225-InvRepPurchaseBillDirectWithoutPo.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/225-inv-rep-purchase-bill-direct-without-po", method = RequestMethod.GET)
    public void print225InvRepPurchaseBillDirectWithoutPoGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print225InvRepPurchaseBillDirectWithoutPo(response, objectMapper.convertValue(query, Rpt225InvRepPurchaseBillDirectWithoutPoRequest.class));
    }

    /**
     * Template: 220R-InvRptPurchaseBillReturnSupplierRiceSlip.rpt
     * Procedure: Sp_InvPurchaseInvoiceReturn_SupplierBill_Rpt
     * Desktop: InvPurchaseInvoiceReports.InvPurchaseInvoiceReturnSlipReport220R
     */
    @RequestMapping(value = "/reports/print/220r-purchase-bill-return-supplier-rice-slip", method = RequestMethod.POST)
    public void print220RPurchaseBillReturnSupplierRiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt220RPurchaseBillReturnSupplierRiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt220RPurchaseBillReturnSupplierRiceSlipRequest();
        printReport(response, "220R-InvRptPurchaseBillReturnSupplierRiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/220r-purchase-bill-return-supplier-rice-slip", method = RequestMethod.GET)
    public void print220RPurchaseBillReturnSupplierRiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print220RPurchaseBillReturnSupplierRiceSlip(response, objectMapper.convertValue(query, Rpt220RPurchaseBillReturnSupplierRiceSlipRequest.class));
    }

    /**
     * Template: 103-AcRptPurchaseSalesVoucherSlip.rpt
     * Procedure: Sp_Vouchers_GeneralJournalAcAndInventoryDetailSlip_Rpt
     * Desktop: VoucherReports.VoucherSlipForInventoryReport
     */
    @RequestMapping(value = "/reports/print/103-purchase-sales-voucher-slip", method = RequestMethod.POST)
    public void print103PurchaseSalesVoucherSlip(HttpServletResponse response, @RequestBody(required = false) Rpt103PurchaseSalesVoucherSlipRequest request) throws Exception {
        if (request == null) request = new Rpt103PurchaseSalesVoucherSlipRequest();
        printReport(response, "103-AcRptPurchaseSalesVoucherSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/103-purchase-sales-voucher-slip", method = RequestMethod.GET)
    public void print103PurchaseSalesVoucherSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print103PurchaseSalesVoucherSlip(response, objectMapper.convertValue(query, Rpt103PurchaseSalesVoucherSlipRequest.class));
    }

    /**
     * Template: 230-InvRptPurchaseBillStoreSlip.rpt
     * Procedure: Sp_InvPurchaseInvoice_StoreBill_Rpt
     * Desktop: InvPurchaseInvoiceReports.InvPurchaseInvoice_StoreBillSlip
     */
    @RequestMapping(value = "/reports/print/230-purchase-bill-store-slip", method = RequestMethod.POST)
    public void print230PurchaseBillStoreSlip(HttpServletResponse response, @RequestBody(required = false) Rpt230PurchaseBillStoreSlipRequest request) throws Exception {
        if (request == null) request = new Rpt230PurchaseBillStoreSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "230-InvRptPurchaseBillStoreSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/230-purchase-bill-store-slip", method = RequestMethod.GET)
    public void print230PurchaseBillStoreSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print230PurchaseBillStoreSlip(response, objectMapper.convertValue(query, Rpt230PurchaseBillStoreSlipRequest.class));
    }

    /**
     * Template: 1858-GrnRegister.rpt
     * Procedure: [pcc].[USP_InvGrn_SlipAndRegister]
     * Desktop: InvGrn.GrnSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1858-grn-register", method = RequestMethod.POST)
    public void print1858GrnRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1858GrnRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1858GrnRegisterRequest();
        printReport(response, "1858-GrnRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1858-grn-register", method = RequestMethod.GET)
    public void print1858GrnRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1858GrnRegister(response, objectMapper.convertValue(query, Rpt1858GrnRegisterRequest.class));
    }

    /**
     * Template: 253-InvRptGatePassInwardRegisterA.rpt
     * Procedure: Sp_GatePassInward_History
     * Desktop: GatePassInward.GatepassInwardHistory
     */
    @RequestMapping(value = "/reports/print/253-gate-pass-inward-register-a", method = RequestMethod.POST)
    public void print253GatePassInwardRegisterA(HttpServletResponse response, @RequestBody(required = false) Rpt253GatePassInwardRegisterARequest request) throws Exception {
        if (request == null) request = new Rpt253GatePassInwardRegisterARequest();
        printReport(response, "253-InvRptGatePassInwardRegisterA.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/253-gate-pass-inward-register-a", method = RequestMethod.GET)
    public void print253GatePassInwardRegisterAGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print253GatePassInwardRegisterA(response, objectMapper.convertValue(query, Rpt253GatePassInwardRegisterARequest.class));
    }

    /**
     * Template: 1863-PurchaseOrderRegister.rpt
     * Procedure: [pcc].[USP_PurchaseOrder_SlipAndRegister]
     * Desktop: PurchaseOrder.PurchaseOrder_Slip
     */
    @RequestMapping(value = "/reports/print/1863-purchase-order-register", method = RequestMethod.POST)
    public void print1863PurchaseOrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1863PurchaseOrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1863PurchaseOrderRegisterRequest();
        printReport(response, "1863-PurchaseOrderRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1863-purchase-order-register", method = RequestMethod.GET)
    public void print1863PurchaseOrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1863PurchaseOrderRegister(response, objectMapper.convertValue(query, Rpt1863PurchaseOrderRegisterRequest.class));
    }

    /**
     * Template: 0228-InvPurchaseInvoice_SaleReturnRegister.rpt
     * Procedure: Sp_InvPurchaseInvoice_SaleReturnRegister
     * Desktop: InvSaleInvoiceReports.InvSaleInvoiceReturnSlipandRegister
     */
    @RequestMapping(value = "/reports/print/0228-inv-purchase-invoice-sale-return-register", method = RequestMethod.POST)
    public void print0228InvPurchaseInvoiceSaleReturnRegister(HttpServletResponse response, @RequestBody(required = false) Rpt0228InvPurchaseInvoiceSaleReturnRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt0228InvPurchaseInvoiceSaleReturnRegisterRequest();
        printReport(response, "0228-InvPurchaseInvoice_SaleReturnRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/0228-inv-purchase-invoice-sale-return-register", method = RequestMethod.GET)
    public void print0228InvPurchaseInvoiceSaleReturnRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print0228InvPurchaseInvoiceSaleReturnRegister(response, objectMapper.convertValue(query, Rpt0228InvPurchaseInvoiceSaleReturnRegisterRequest.class));
    }

    /**
     * Template: 1203_GdnReturnable_Slip.rpt
     * Procedure: Sp_InvGdn_SlipAndRegisterRice_Rpt
     * Desktop: InvGrnandGdnReports.InvGdnSlip260
     */
    @RequestMapping(value = "/reports/print/1203-gdn-returnable-slip", method = RequestMethod.POST)
    public void print1203GdnReturnableSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1203GdnReturnableSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1203GdnReturnableSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1203_GdnReturnable_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1203-gdn-returnable-slip", method = RequestMethod.GET)
    public void print1203GdnReturnableSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1203GdnReturnableSlip(response, objectMapper.convertValue(query, Rpt1203GdnReturnableSlipRequest.class));
    }

    /**
     * Template: 1600-PurchaseOrder_Slip_Engr.rpt
     * Procedure: USP_PurchaseOrder_Slip_Engr
     * Desktop: PurchaseOrder.PurchaseOrder_Slip_Engr
     */
    @RequestMapping(value = "/reports/print/1600-purchase-order-slip-engr", method = RequestMethod.POST)
    public void print1600PurchaseOrderSlipEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1600PurchaseOrderSlipEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1600PurchaseOrderSlipEngrRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1600-PurchaseOrder_Slip_Engr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1600-purchase-order-slip-engr", method = RequestMethod.GET)
    public void print1600PurchaseOrderSlipEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1600PurchaseOrderSlipEngr(response, objectMapper.convertValue(query, Rpt1600PurchaseOrderSlipEngrRequest.class));
    }

    /**
     * Template: 1601-InwardGatePassSlipEngr.rpt
     * Procedure: Sp_GatePassInward_SlipAndRegister_Rpt
     * Desktop: GatePassInwardReports.GatePassInwardSlipandRegister
     */
    @RequestMapping(value = "/reports/print/1601-inward-gate-pass-slip-engr", method = RequestMethod.POST)
    public void print1601InwardGatePassSlipEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1601InwardGatePassSlipEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1601InwardGatePassSlipEngrRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1601-InwardGatePassSlipEngr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1601-inward-gate-pass-slip-engr", method = RequestMethod.GET)
    public void print1601InwardGatePassSlipEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1601InwardGatePassSlipEngr(response, objectMapper.convertValue(query, Rpt1601InwardGatePassSlipEngrRequest.class));
    }

    /**
     * Template: 1602-GoodsReceiptsNotesSlip.rpt
     * Procedure: [dbo].[USp_InvGrnRegisterAndSlip_Engr]
     * Desktop: InvGrn.InvGrnRegisterAndSlipLoader_Engr
     */
    @RequestMapping(value = "/reports/print/1602-goods-receipts-notes-slip", method = RequestMethod.POST)
    public void print1602GoodsReceiptsNotesSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1602GoodsReceiptsNotesSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1602GoodsReceiptsNotesSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1602-GoodsReceiptsNotesSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1602-goods-receipts-notes-slip", method = RequestMethod.GET)
    public void print1602GoodsReceiptsNotesSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1602GoodsReceiptsNotesSlip(response, objectMapper.convertValue(query, Rpt1602GoodsReceiptsNotesSlipRequest.class));
    }

    /**
     * Template: 1602_01-GoodsReceiptsNotesSlip.rpt
     * Procedure: [dbo].[USp_InvGrnRegisterAndSlip_Engr]
     * Desktop: InvGrn.InvGrnRegisterAndSlipLoader_Engr
     */
    @RequestMapping(value = "/reports/print/1602-01-goods-receipts-notes-slip", method = RequestMethod.POST)
    public void print160201GoodsReceiptsNotesSlip(HttpServletResponse response, @RequestBody(required = false) Rpt160201GoodsReceiptsNotesSlipRequest request) throws Exception {
        if (request == null) request = new Rpt160201GoodsReceiptsNotesSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1602_01-GoodsReceiptsNotesSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1602-01-goods-receipts-notes-slip", method = RequestMethod.GET)
    public void print160201GoodsReceiptsNotesSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print160201GoodsReceiptsNotesSlip(response, objectMapper.convertValue(query, Rpt160201GoodsReceiptsNotesSlipRequest.class));
    }

    /**
     * Template: 1603-PurchaseBillSupplier_Engr_ItemSlip.rpt
     * Procedure: USP_InvPurchaseInvoice_PartySlip_Engr
     * Desktop: InvPurchaseInvoiceReports.InvPurchaseInvoice_PartySlip_Engr
     */
    @RequestMapping(value = "/reports/print/1603-purchase-bill-supplier-engr-item-slip", method = RequestMethod.POST)
    public void print1603PurchaseBillSupplierEngrItemSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1603PurchaseBillSupplierEngrItemSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1603PurchaseBillSupplierEngrItemSlipRequest();
        printReport(response, "1603-PurchaseBillSupplier_Engr_ItemSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1603-purchase-bill-supplier-engr-item-slip", method = RequestMethod.GET)
    public void print1603PurchaseBillSupplierEngrItemSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1603PurchaseBillSupplierEngrItemSlip(response, objectMapper.convertValue(query, Rpt1603PurchaseBillSupplierEngrItemSlipRequest.class));
    }

    /**
     * Template: 1603A-PurchaseInvoice_Engr_PartySlip.rpt
     * Procedure: USP_InvPurchaseInvoice_PartySlip_Engr
     * Desktop: InvPurchaseInvoiceReports.InvPurchaseInvoice_PartySlip_Engr
     */
    @RequestMapping(value = "/reports/print/1603a-purchase-invoice-engr-party-slip", method = RequestMethod.POST)
    public void print1603APurchaseInvoiceEngrPartySlip(HttpServletResponse response, @RequestBody(required = false) Rpt1603APurchaseInvoiceEngrPartySlipRequest request) throws Exception {
        if (request == null) request = new Rpt1603APurchaseInvoiceEngrPartySlipRequest();
        printReport(response, "1603A-PurchaseInvoice_Engr_PartySlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1603a-purchase-invoice-engr-party-slip", method = RequestMethod.GET)
    public void print1603APurchaseInvoiceEngrPartySlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1603APurchaseInvoiceEngrPartySlip(response, objectMapper.convertValue(query, Rpt1603APurchaseInvoiceEngrPartySlipRequest.class));
    }

    /**
     * Template: 1604-PurchaseInvoiceDirect_Engr_ItemSlip.rpt
     * Procedure: USP_InvPurchaseInvoiceDirect_PartySlip_Engr
     * Desktop: InvPurchaseInvoiceReports.InvPurchaseInvoiceDirect_PartySlip_Engr
     */
    @RequestMapping(value = "/reports/print/1604-purchase-invoice-direct-engr-item-slip", method = RequestMethod.POST)
    public void print1604PurchaseInvoiceDirectEngrItemSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1604PurchaseInvoiceDirectEngrItemSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1604PurchaseInvoiceDirectEngrItemSlipRequest();
        printReport(response, "1604-PurchaseInvoiceDirect_Engr_ItemSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1604-purchase-invoice-direct-engr-item-slip", method = RequestMethod.GET)
    public void print1604PurchaseInvoiceDirectEngrItemSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1604PurchaseInvoiceDirectEngrItemSlip(response, objectMapper.convertValue(query, Rpt1604PurchaseInvoiceDirectEngrItemSlipRequest.class));
    }

    /**
     * Template: 1604A-PurchaseInvoiceDirect_Engr_PartySlip.rpt
     * Procedure: USP_InvPurchaseInvoiceDirect_PartySlip_Engr
     * Desktop: InvPurchaseInvoiceReports.InvPurchaseInvoiceDirect_PartySlip_Engr
     */
    @RequestMapping(value = "/reports/print/1604a-purchase-invoice-direct-engr-party-slip", method = RequestMethod.POST)
    public void print1604APurchaseInvoiceDirectEngrPartySlip(HttpServletResponse response, @RequestBody(required = false) Rpt1604APurchaseInvoiceDirectEngrPartySlipRequest request) throws Exception {
        if (request == null) request = new Rpt1604APurchaseInvoiceDirectEngrPartySlipRequest();
        printReport(response, "1604A-PurchaseInvoiceDirect_Engr_PartySlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1604a-purchase-invoice-direct-engr-party-slip", method = RequestMethod.GET)
    public void print1604APurchaseInvoiceDirectEngrPartySlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1604APurchaseInvoiceDirectEngrPartySlip(response, objectMapper.convertValue(query, Rpt1604APurchaseInvoiceDirectEngrPartySlipRequest.class));
    }

    /**
     * Template: 1612-InvGdn_Slip.rpt
     * Procedure: Sp_InvGdn_SlipAndRegisterRice_Rpt
     * Desktop: InvGrnandGdnReports.InvGdnSlip260
     */
    @RequestMapping(value = "/reports/print/1612-inv-gdn-slip", method = RequestMethod.POST)
    public void print1612InvGdnSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1612InvGdnSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1612InvGdnSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1612-InvGdn_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1612-inv-gdn-slip", method = RequestMethod.GET)
    public void print1612InvGdnSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1612InvGdnSlip(response, objectMapper.convertValue(query, Rpt1612InvGdnSlipRequest.class));
    }

    /**
     * Template: 1618-GoodsReceiptsNotesSlip.rpt
     * Procedure: [dbo].[USp_InvGrnRegisterAndSlip_Engr]
     * Desktop: InvGrn.InvGrnRegisterAndSlipLoader_Engr
     */
    @RequestMapping(value = "/reports/print/1618-goods-receipts-notes-slip", method = RequestMethod.POST)
    public void print1618GoodsReceiptsNotesSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1618GoodsReceiptsNotesSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1618GoodsReceiptsNotesSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1618-GoodsReceiptsNotesSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1618-goods-receipts-notes-slip", method = RequestMethod.GET)
    public void print1618GoodsReceiptsNotesSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1618GoodsReceiptsNotesSlip(response, objectMapper.convertValue(query, Rpt1618GoodsReceiptsNotesSlipRequest.class));
    }

    /**
     * Template: 1618-RptGrnRegister.rpt
     * Procedure: [dbo].[USp_InvGrnRegisterAndSlip_Engr]
     * Desktop: InvGrn.InvGrnRegisterAndSlipLoader_Engr
     */
    @RequestMapping(value = "/reports/print/1618-grn-register", method = RequestMethod.POST)
    public void print1618GrnRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1618GrnRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1618GrnRegisterRequest();
        printReport(response, "1618-RptGrnRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1618-grn-register", method = RequestMethod.GET)
    public void print1618GrnRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1618GrnRegister(response, objectMapper.convertValue(query, Rpt1618GrnRegisterRequest.class));
    }

    /**
     * Template: 1618A-RptGrnRegister.rpt
     * Procedure: [dbo].[USp_InvGrnRegisterAndSlip_Engr]
     * Desktop: InvGrn.InvGrnRegisterAndSlipLoader_Engr
     */
    @RequestMapping(value = "/reports/print/1618a-grn-register", method = RequestMethod.POST)
    public void print1618AGrnRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1618AGrnRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1618AGrnRegisterRequest();
        printReport(response, "1618A-RptGrnRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1618a-grn-register", method = RequestMethod.GET)
    public void print1618AGrnRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1618AGrnRegister(response, objectMapper.convertValue(query, Rpt1618AGrnRegisterRequest.class));
    }

    /**
     * Template: 1626-PurchaseBySupplier.rpt
     * Procedure: [dbo].[USP_PurchaseInvoice_WithActivitiesRegister_Engr]
     * Desktop: InvPurchaseInvoice.PurchaseInvoice_WithActivitiesRegister_Engr
     */
    @RequestMapping(value = "/reports/print/1626-purchase-by-supplier", method = RequestMethod.POST)
    public void print1626PurchaseBySupplier(HttpServletResponse response, @RequestBody(required = false) Rpt1626PurchaseBySupplierRequest request) throws Exception {
        if (request == null) request = new Rpt1626PurchaseBySupplierRequest();
        printReport(response, "1626-PurchaseBySupplier.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1626-purchase-by-supplier", method = RequestMethod.GET)
    public void print1626PurchaseBySupplierGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1626PurchaseBySupplier(response, objectMapper.convertValue(query, Rpt1626PurchaseBySupplierRequest.class));
    }

    /**
     * Template: 1627-PurchaseBySupplier&Item.rpt
     * Procedure: [dbo].[USP_PurchaseInvoice_WithActivitiesRegister_Engr]
     * Desktop: InvPurchaseInvoice.PurchaseInvoice_WithActivitiesRegister_Engr
     */
    @RequestMapping(value = "/reports/print/1627-purchase-by-supplier-item", method = RequestMethod.POST)
    public void print1627PurchaseBySupplierItem(HttpServletResponse response, @RequestBody(required = false) Rpt1627PurchaseBySupplierItemRequest request) throws Exception {
        if (request == null) request = new Rpt1627PurchaseBySupplierItemRequest();
        printReport(response, "1627-PurchaseBySupplier&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1627-purchase-by-supplier-item", method = RequestMethod.GET)
    public void print1627PurchaseBySupplierItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1627PurchaseBySupplierItem(response, objectMapper.convertValue(query, Rpt1627PurchaseBySupplierItemRequest.class));
    }

    /**
     * Template: 1628-PurchaseByItem,Category&ItemType.rpt
     * Procedure: [dbo].[USP_PurchaseInvoice_WithActivitiesRegister_Engr]
     * Desktop: InvPurchaseInvoice.PurchaseInvoice_WithActivitiesRegister_Engr
     */
    @RequestMapping(value = "/reports/print/1628-purchase-by-item-category-item-type", method = RequestMethod.POST)
    public void print1628PurchaseByItemCategoryItemType(HttpServletResponse response, @RequestBody(required = false) Rpt1628PurchaseByItemCategoryItemTypeRequest request) throws Exception {
        if (request == null) request = new Rpt1628PurchaseByItemCategoryItemTypeRequest();
        printReport(response, "1628-PurchaseByItem,Category&ItemType.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1628-purchase-by-item-category-item-type", method = RequestMethod.GET)
    public void print1628PurchaseByItemCategoryItemTypeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1628PurchaseByItemCategoryItemType(response, objectMapper.convertValue(query, Rpt1628PurchaseByItemCategoryItemTypeRequest.class));
    }

    /**
     * Template: 1629-PurchaseByCategory.rpt
     * Procedure: [dbo].[USP_PurchaseInvoice_WithActivitiesRegister_Engr]
     * Desktop: InvPurchaseInvoice.PurchaseInvoice_WithActivitiesRegister_Engr
     */
    @RequestMapping(value = "/reports/print/1629-purchase-by-category", method = RequestMethod.POST)
    public void print1629PurchaseByCategory(HttpServletResponse response, @RequestBody(required = false) Rpt1629PurchaseByCategoryRequest request) throws Exception {
        if (request == null) request = new Rpt1629PurchaseByCategoryRequest();
        printReport(response, "1629-PurchaseByCategory.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1629-purchase-by-category", method = RequestMethod.GET)
    public void print1629PurchaseByCategoryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1629PurchaseByCategory(response, objectMapper.convertValue(query, Rpt1629PurchaseByCategoryRequest.class));
    }

    /**
     * Template: 1630-PurchaseByItemType.rpt
     * Procedure: [dbo].[USP_PurchaseInvoice_WithActivitiesRegister_Engr]
     * Desktop: InvPurchaseInvoice.PurchaseInvoice_WithActivitiesRegister_Engr
     */
    @RequestMapping(value = "/reports/print/1630-purchase-by-item-type", method = RequestMethod.POST)
    public void print1630PurchaseByItemType(HttpServletResponse response, @RequestBody(required = false) Rpt1630PurchaseByItemTypeRequest request) throws Exception {
        if (request == null) request = new Rpt1630PurchaseByItemTypeRequest();
        printReport(response, "1630-PurchaseByItemType.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1630-purchase-by-item-type", method = RequestMethod.GET)
    public void print1630PurchaseByItemTypeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1630PurchaseByItemType(response, objectMapper.convertValue(query, Rpt1630PurchaseByItemTypeRequest.class));
    }

    /**
     * Template: 1631-PurchaseByPaymentTerms.rpt
     * Procedure: [dbo].[USP_PurchaseInvoice_WithActivitiesRegister_Engr]
     * Desktop: InvPurchaseInvoice.PurchaseInvoice_WithActivitiesRegister_Engr
     */
    @RequestMapping(value = "/reports/print/1631-purchase-by-payment-terms", method = RequestMethod.POST)
    public void print1631PurchaseByPaymentTerms(HttpServletResponse response, @RequestBody(required = false) Rpt1631PurchaseByPaymentTermsRequest request) throws Exception {
        if (request == null) request = new Rpt1631PurchaseByPaymentTermsRequest();
        printReport(response, "1631-PurchaseByPaymentTerms.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1631-purchase-by-payment-terms", method = RequestMethod.GET)
    public void print1631PurchaseByPaymentTermsGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1631PurchaseByPaymentTerms(response, objectMapper.convertValue(query, Rpt1631PurchaseByPaymentTermsRequest.class));
    }

    /**
     * Template: 1632-PurchaseByCategory&PackSize.rpt
     * Procedure: [dbo].[USP_PurchaseInvoice_WithActivitiesRegister_Engr]
     * Desktop: InvPurchaseInvoice.PurchaseInvoice_WithActivitiesRegister_Engr
     */
    @RequestMapping(value = "/reports/print/1632-purchase-by-category-pack-size", method = RequestMethod.POST)
    public void print1632PurchaseByCategoryPackSize(HttpServletResponse response, @RequestBody(required = false) Rpt1632PurchaseByCategoryPackSizeRequest request) throws Exception {
        if (request == null) request = new Rpt1632PurchaseByCategoryPackSizeRequest();
        printReport(response, "1632-PurchaseByCategory&PackSize.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1632-purchase-by-category-pack-size", method = RequestMethod.GET)
    public void print1632PurchaseByCategoryPackSizeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1632PurchaseByCategoryPackSize(response, objectMapper.convertValue(query, Rpt1632PurchaseByCategoryPackSizeRequest.class));
    }

    /**
     * Template: 1654_PurchaseInvoice_CustomerBillDirectSlip_Engr.rpt
     * Procedure: USP_InvPurchaseInvoiceDirect_PartySlip_Engr
     * Desktop: InvPurchaseInvoiceReports.InvPurchaseInvoiceDirect_PartySlip_Engr
     */
    @RequestMapping(value = "/reports/print/1654-purchase-invoice-customer-bill-direct-slip-engr", method = RequestMethod.POST)
    public void print1654PurchaseInvoiceCustomerBillDirectSlipEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1654PurchaseInvoiceCustomerBillDirectSlipEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1654PurchaseInvoiceCustomerBillDirectSlipEngrRequest();
        printReport(response, "1654_PurchaseInvoice_CustomerBillDirectSlip_Engr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1654-purchase-invoice-customer-bill-direct-slip-engr", method = RequestMethod.GET)
    public void print1654PurchaseInvoiceCustomerBillDirectSlipEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1654PurchaseInvoiceCustomerBillDirectSlipEngr(response, objectMapper.convertValue(query, Rpt1654PurchaseInvoiceCustomerBillDirectSlipEngrRequest.class));
    }

    /**
     * Template: 1654A_PurchaseInvoice_CustomerBillDirectSlip_Engr.rpt
     * Procedure: USP_InvPurchaseInvoiceDirect_PartySlip_Engr
     * Desktop: InvPurchaseInvoiceReports.InvPurchaseInvoiceDirect_PartySlip_Engr
     */
    @RequestMapping(value = "/reports/print/1654a-purchase-invoice-customer-bill-direct-slip-engr", method = RequestMethod.POST)
    public void print1654APurchaseInvoiceCustomerBillDirectSlipEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1654APurchaseInvoiceCustomerBillDirectSlipEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1654APurchaseInvoiceCustomerBillDirectSlipEngrRequest();
        printReport(response, "1654A_PurchaseInvoice_CustomerBillDirectSlip_Engr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1654a-purchase-invoice-customer-bill-direct-slip-engr", method = RequestMethod.GET)
    public void print1654APurchaseInvoiceCustomerBillDirectSlipEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1654APurchaseInvoiceCustomerBillDirectSlipEngr(response, objectMapper.convertValue(query, Rpt1654APurchaseInvoiceCustomerBillDirectSlipEngrRequest.class));
    }

    /**
     * Template: 1804-InvRepPurchaseBillDirectWithoutPo.rpt
     * Procedure: [dbo].[USP_PurchaseInvoiceDirectForSaltSlip]
     * Desktop: InvPurchaseInvoiceReports.PurchaseInvoiceDirectForSaltSlip
     */
    @RequestMapping(value = "/reports/print/1804-inv-rep-purchase-bill-direct-without-po", method = RequestMethod.POST)
    public void print1804InvRepPurchaseBillDirectWithoutPo(HttpServletResponse response, @RequestBody(required = false) Rpt1804InvRepPurchaseBillDirectWithoutPoRequest request) throws Exception {
        if (request == null) request = new Rpt1804InvRepPurchaseBillDirectWithoutPoRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1804-InvRepPurchaseBillDirectWithoutPo.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1804-inv-rep-purchase-bill-direct-without-po", method = RequestMethod.GET)
    public void print1804InvRepPurchaseBillDirectWithoutPoGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1804InvRepPurchaseBillDirectWithoutPo(response, objectMapper.convertValue(query, Rpt1804InvRepPurchaseBillDirectWithoutPoRequest.class));
    }

    /**
     * Template: 1804_01_PurchaseRegister.rpt
     * Procedure: [dbo].[USP_PurchaseFromEvaulationsWithActivitiesReportSalt]
     * Desktop: InvPurchaseInvoice.PurchaseFromEvaulationsWithActivitiesReportSalt
     */
    @RequestMapping(value = "/reports/print/1804-01-purchase-register", method = RequestMethod.POST)
    public void print180401PurchaseRegister(HttpServletResponse response, @RequestBody(required = false) Rpt180401PurchaseRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt180401PurchaseRegisterRequest();
        printReport(response, "1804_01_PurchaseRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1804-01-purchase-register", method = RequestMethod.GET)
    public void print180401PurchaseRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180401PurchaseRegister(response, objectMapper.convertValue(query, Rpt180401PurchaseRegisterRequest.class));
    }

    /**
     * Template: 1804_02_PurchaseSummaryByItem.rpt
     * Procedure: [dbo].[USP_PurchaseFromEvaulationsWithActivitiesReportSalt]
     * Desktop: InvPurchaseInvoice.PurchaseFromEvaulationsWithActivitiesReportSalt
     */
    @RequestMapping(value = "/reports/print/1804-02-purchase-summary-by-item", method = RequestMethod.POST)
    public void print180402PurchaseSummaryByItem(HttpServletResponse response, @RequestBody(required = false) Rpt180402PurchaseSummaryByItemRequest request) throws Exception {
        if (request == null) request = new Rpt180402PurchaseSummaryByItemRequest();
        printReport(response, "1804_02_PurchaseSummaryByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1804-02-purchase-summary-by-item", method = RequestMethod.GET)
    public void print180402PurchaseSummaryByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180402PurchaseSummaryByItem(response, objectMapper.convertValue(query, Rpt180402PurchaseSummaryByItemRequest.class));
    }

    /**
     * Template: 1804_03_PurchaseSummaryByItemAndWarehouse.rpt
     * Procedure: [dbo].[USP_PurchaseFromEvaulationsWithActivitiesReportSalt]
     * Desktop: InvPurchaseInvoice.PurchaseFromEvaulationsWithActivitiesReportSalt
     */
    @RequestMapping(value = "/reports/print/1804-03-purchase-summary-by-item-and-warehouse", method = RequestMethod.POST)
    public void print180403PurchaseSummaryByItemAndWarehouse(HttpServletResponse response, @RequestBody(required = false) Rpt180403PurchaseSummaryByItemAndWarehouseRequest request) throws Exception {
        if (request == null) request = new Rpt180403PurchaseSummaryByItemAndWarehouseRequest();
        printReport(response, "1804_03_PurchaseSummaryByItemAndWarehouse.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1804-03-purchase-summary-by-item-and-warehouse", method = RequestMethod.GET)
    public void print180403PurchaseSummaryByItemAndWarehouseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180403PurchaseSummaryByItemAndWarehouse(response, objectMapper.convertValue(query, Rpt180403PurchaseSummaryByItemAndWarehouseRequest.class));
    }

    /**
     * Template: 1804_04_PurchaseSummaryBySupplier.rpt
     * Procedure: [dbo].[USP_PurchaseFromEvaulationsWithActivitiesReportSalt]
     * Desktop: InvPurchaseInvoice.PurchaseFromEvaulationsWithActivitiesReportSalt
     */
    @RequestMapping(value = "/reports/print/1804-04-purchase-summary-by-supplier", method = RequestMethod.POST)
    public void print180404PurchaseSummaryBySupplier(HttpServletResponse response, @RequestBody(required = false) Rpt180404PurchaseSummaryBySupplierRequest request) throws Exception {
        if (request == null) request = new Rpt180404PurchaseSummaryBySupplierRequest();
        printReport(response, "1804_04_PurchaseSummaryBySupplier.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1804-04-purchase-summary-by-supplier", method = RequestMethod.GET)
    public void print180404PurchaseSummaryBySupplierGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180404PurchaseSummaryBySupplier(response, objectMapper.convertValue(query, Rpt180404PurchaseSummaryBySupplierRequest.class));
    }

    /**
     * Template: 1804_05_PurchaseSummaryByItemAndSupplier.rpt
     * Procedure: [dbo].[USP_PurchaseFromEvaulationsWithActivitiesReportSalt]
     * Desktop: InvPurchaseInvoice.PurchaseFromEvaulationsWithActivitiesReportSalt
     */
    @RequestMapping(value = "/reports/print/1804-05-purchase-summary-by-item-and-supplier", method = RequestMethod.POST)
    public void print180405PurchaseSummaryByItemAndSupplier(HttpServletResponse response, @RequestBody(required = false) Rpt180405PurchaseSummaryByItemAndSupplierRequest request) throws Exception {
        if (request == null) request = new Rpt180405PurchaseSummaryByItemAndSupplierRequest();
        printReport(response, "1804_05_PurchaseSummaryByItemAndSupplier.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1804-05-purchase-summary-by-item-and-supplier", method = RequestMethod.GET)
    public void print180405PurchaseSummaryByItemAndSupplierGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180405PurchaseSummaryByItemAndSupplier(response, objectMapper.convertValue(query, Rpt180405PurchaseSummaryByItemAndSupplierRequest.class));
    }

    /**
     * Template: 1804_06_PurchaseSummaryByParentCategory.rpt
     * Procedure: [dbo].[USP_PurchaseFromEvaulationsWithActivitiesReportSalt]
     * Desktop: InvPurchaseInvoice.PurchaseFromEvaulationsWithActivitiesReportSalt
     */
    @RequestMapping(value = "/reports/print/1804-06-purchase-summary-by-parent-category", method = RequestMethod.POST)
    public void print180406PurchaseSummaryByParentCategory(HttpServletResponse response, @RequestBody(required = false) Rpt180406PurchaseSummaryByParentCategoryRequest request) throws Exception {
        if (request == null) request = new Rpt180406PurchaseSummaryByParentCategoryRequest();
        printReport(response, "1804_06_PurchaseSummaryByParentCategory.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1804-06-purchase-summary-by-parent-category", method = RequestMethod.GET)
    public void print180406PurchaseSummaryByParentCategoryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180406PurchaseSummaryByParentCategory(response, objectMapper.convertValue(query, Rpt180406PurchaseSummaryByParentCategoryRequest.class));
    }

    /**
     * Template: 1804_07_PurchaseSummaryByParentCategoryAndSupplier.rpt
     * Procedure: [dbo].[USP_PurchaseFromEvaulationsWithActivitiesReportSalt]
     * Desktop: InvPurchaseInvoice.PurchaseFromEvaulationsWithActivitiesReportSalt
     */
    @RequestMapping(value = "/reports/print/1804-07-purchase-summary-by-parent-category-and-supplier", method = RequestMethod.POST)
    public void print180407PurchaseSummaryByParentCategoryAndSupplier(HttpServletResponse response, @RequestBody(required = false) Rpt180407PurchaseSummaryByParentCategoryAndSupplierRequest request) throws Exception {
        if (request == null) request = new Rpt180407PurchaseSummaryByParentCategoryAndSupplierRequest();
        printReport(response, "1804_07_PurchaseSummaryByParentCategoryAndSupplier.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1804-07-purchase-summary-by-parent-category-and-supplier", method = RequestMethod.GET)
    public void print180407PurchaseSummaryByParentCategoryAndSupplierGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180407PurchaseSummaryByParentCategoryAndSupplier(response, objectMapper.convertValue(query, Rpt180407PurchaseSummaryByParentCategoryAndSupplierRequest.class));
    }

    /**
     * Template: 1804A-InvRepPurchaseBillDirectWithoutPo.rpt
     * Procedure: [dbo].[USP_PurchaseInvoiceDirectForSaltSlip]
     * Desktop: InvPurchaseInvoiceReports.PurchaseInvoiceDirectForSaltSlip
     */
    @RequestMapping(value = "/reports/print/1804a-inv-rep-purchase-bill-direct-without-po", method = RequestMethod.POST)
    public void print1804AInvRepPurchaseBillDirectWithoutPo(HttpServletResponse response, @RequestBody(required = false) Rpt1804AInvRepPurchaseBillDirectWithoutPoRequest request) throws Exception {
        if (request == null) request = new Rpt1804AInvRepPurchaseBillDirectWithoutPoRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1804A-InvRepPurchaseBillDirectWithoutPo.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1804a-inv-rep-purchase-bill-direct-without-po", method = RequestMethod.GET)
    public void print1804AInvRepPurchaseBillDirectWithoutPoGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1804AInvRepPurchaseBillDirectWithoutPo(response, objectMapper.convertValue(query, Rpt1804AInvRepPurchaseBillDirectWithoutPoRequest.class));
    }

    /**
     * Template: 1858-GoodsReceiptsNotes_Slip.rpt
     * Procedure: Sp_InvGrn_RiceSlip_Rpt
     * Desktop: InvGrnandGdnReports.InvGrnSlip211
     */
    @RequestMapping(value = "/reports/print/1858-goods-receipts-notes-slip", method = RequestMethod.POST)
    public void print1858GoodsReceiptsNotesSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1858GoodsReceiptsNotesSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1858GoodsReceiptsNotesSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1858-GoodsReceiptsNotes_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1858-goods-receipts-notes-slip", method = RequestMethod.GET)
    public void print1858GoodsReceiptsNotesSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1858GoodsReceiptsNotesSlip(response, objectMapper.convertValue(query, Rpt1858GoodsReceiptsNotesSlipRequest.class));
    }

    /**
     * Template: 203-InvRptPurchaseOrderRiceSlip(A).rpt
     * Procedure: Sp_PurchaseOrderSlip_Rpt
     * Desktop: PurchaseOrderReports.PurchaseOrderSlipReport203
     */
    @RequestMapping(value = "/reports/print/203-purchase-order-rice-slip-a", method = RequestMethod.POST)
    public void print203PurchaseOrderRiceSlipA(HttpServletResponse response, @RequestBody(required = false) Rpt203PurchaseOrderRiceSlipARequest request) throws Exception {
        if (request == null) request = new Rpt203PurchaseOrderRiceSlipARequest();
        printReport(response, "203-InvRptPurchaseOrderRiceSlip(A).rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/203-purchase-order-rice-slip-a", method = RequestMethod.GET)
    public void print203PurchaseOrderRiceSlipAGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print203PurchaseOrderRiceSlipA(response, objectMapper.convertValue(query, Rpt203PurchaseOrderRiceSlipARequest.class));
    }

    /**
     * Template: 205-InvRptPurchaseOrderDetailRegisterRice.rpt
     * Procedure: Sp_PurchaseOrder_RegisterDetail_Rpt
     * Desktop: PurchaseOrderReports.PurchaseOrderDetailRegisterReport206
     */
    @RequestMapping(value = "/reports/print/205-purchase-order-detail-register-rice", method = RequestMethod.POST)
    public void print205PurchaseOrderDetailRegisterRice(HttpServletResponse response, @RequestBody(required = false) Rpt205PurchaseOrderDetailRegisterRiceRequest request) throws Exception {
        if (request == null) request = new Rpt205PurchaseOrderDetailRegisterRiceRequest();
        printReport(response, "205-InvRptPurchaseOrderDetailRegisterRice.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/205-purchase-order-detail-register-rice", method = RequestMethod.GET)
    public void print205PurchaseOrderDetailRegisterRiceGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print205PurchaseOrderDetailRegisterRice(response, objectMapper.convertValue(query, Rpt205PurchaseOrderDetailRegisterRiceRequest.class));
    }

    /**
     * Template: 206-RptPurchaseOrderRegisterRice.rpt
     * Procedure: Sp_PurchaseOrder_RegisterDetail_Rpt
     * Desktop: PurchaseOrderReports.PurchaseOrderDetailRegisterReport206
     */
    @RequestMapping(value = "/reports/print/206-purchase-order-register-rice", method = RequestMethod.POST)
    public void print206PurchaseOrderRegisterRice(HttpServletResponse response, @RequestBody(required = false) Rpt206PurchaseOrderRegisterRiceRequest request) throws Exception {
        if (request == null) request = new Rpt206PurchaseOrderRegisterRiceRequest();
        printReport(response, "206-RptPurchaseOrderRegisterRice.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/206-purchase-order-register-rice", method = RequestMethod.GET)
    public void print206PurchaseOrderRegisterRiceGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print206PurchaseOrderRegisterRice(response, objectMapper.convertValue(query, Rpt206PurchaseOrderRegisterRiceRequest.class));
    }

    /**
     * Template: 213-GoodsReceiptsNotesAgainstOrderSlip.rpt
     * Procedure: Sp_InvGrn_RiceSlip_Rpt
     * Desktop: InvGrnandGdnReports.InvGrnSlip211
     */
    @RequestMapping(value = "/reports/print/213-goods-receipts-notes-against-order-slip", method = RequestMethod.POST)
    public void print213GoodsReceiptsNotesAgainstOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt213GoodsReceiptsNotesAgainstOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt213GoodsReceiptsNotesAgainstOrderSlipRequest();
        printReport(response, "213-GoodsReceiptsNotesAgainstOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/213-goods-receipts-notes-against-order-slip", method = RequestMethod.GET)
    public void print213GoodsReceiptsNotesAgainstOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print213GoodsReceiptsNotesAgainstOrderSlip(response, objectMapper.convertValue(query, Rpt213GoodsReceiptsNotesAgainstOrderSlipRequest.class));
    }

    /**
     * Template: 220-InvRptPurchaseBillSupplierRiceSlip.rpt
     * Procedure: SP_InvPurchaseInvoice_ItemOthersAddLess_SubRpt
     * Desktop: InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprtForItem
     */
    @RequestMapping(value = "/reports/print/220-purchase-bill-supplier-rice-slip", method = RequestMethod.POST)
    public void print220PurchaseBillSupplierRiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt220PurchaseBillSupplierRiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt220PurchaseBillSupplierRiceSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "220-InvRptPurchaseBillSupplierRiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/220-purchase-bill-supplier-rice-slip", method = RequestMethod.GET)
    public void print220PurchaseBillSupplierRiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print220PurchaseBillSupplierRiceSlip(response, objectMapper.convertValue(query, Rpt220PurchaseBillSupplierRiceSlipRequest.class));
    }

    /**
     * Template: 220A-InvRptPurchaseBillSupplierRiceSlip.rpt
     * Procedure: Sp_InvPurchaseInvoice_SupplierBill_Rpt
     * Desktop: InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220
     */
    @RequestMapping(value = "/reports/print/220a-purchase-bill-supplier-rice-slip", method = RequestMethod.POST)
    public void print220APurchaseBillSupplierRiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt220APurchaseBillSupplierRiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt220APurchaseBillSupplierRiceSlipRequest();
        printReport(response, "220A-InvRptPurchaseBillSupplierRiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/220a-purchase-bill-supplier-rice-slip", method = RequestMethod.GET)
    public void print220APurchaseBillSupplierRiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print220APurchaseBillSupplierRiceSlip(response, objectMapper.convertValue(query, Rpt220APurchaseBillSupplierRiceSlipRequest.class));
    }

    /**
     * Template: 220B-InvRptPurchaseBillSupplierRiceSummary.rpt
     * Procedure: SP_InvPurchaseInvoice_ItemOthersAddLess_SubRpt
     * Desktop: InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprtForItem
     */
    @RequestMapping(value = "/reports/print/220b-purchase-bill-supplier-rice-summary", method = RequestMethod.POST)
    public void print220BPurchaseBillSupplierRiceSummary(HttpServletResponse response, @RequestBody(required = false) Rpt220BPurchaseBillSupplierRiceSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt220BPurchaseBillSupplierRiceSummaryRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "220B-InvRptPurchaseBillSupplierRiceSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/220b-purchase-bill-supplier-rice-summary", method = RequestMethod.GET)
    public void print220BPurchaseBillSupplierRiceSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print220BPurchaseBillSupplierRiceSummary(response, objectMapper.convertValue(query, Rpt220BPurchaseBillSupplierRiceSummaryRequest.class));
    }

    /**
     * Template: 221-InvRptPurchaseInvoicePurchaseAvgRateByItem.rpt
     * Procedure: Sp_InvPurchaseInvoice_AvgRatesComparisonsByItemSupplier_Rpt
     * Desktop: InvPurchaseInvoiceReports.InvPurchaseInvoiceAvgRateBySupplier222
     */
    @RequestMapping(value = "/reports/print/221-purchase-invoice-purchase-avg-rate-by-item", method = RequestMethod.POST)
    public void print221PurchaseInvoicePurchaseAvgRateByItem(HttpServletResponse response, @RequestBody(required = false) Rpt221PurchaseInvoicePurchaseAvgRateByItemRequest request) throws Exception {
        if (request == null) request = new Rpt221PurchaseInvoicePurchaseAvgRateByItemRequest();
        printReport(response, "221-InvRptPurchaseInvoicePurchaseAvgRateByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/221-purchase-invoice-purchase-avg-rate-by-item", method = RequestMethod.GET)
    public void print221PurchaseInvoicePurchaseAvgRateByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print221PurchaseInvoicePurchaseAvgRateByItem(response, objectMapper.convertValue(query, Rpt221PurchaseInvoicePurchaseAvgRateByItemRequest.class));
    }

    /**
     * Template: 221_GdnForSaleToPartyProcssingSlip.rpt
     * Procedure: Sp_InvGdn_SlipAndRegisterRice_Rpt
     * Desktop: InvGrnandGdnReports.InvGdnSlip260
     */
    @RequestMapping(value = "/reports/print/221-gdn-for-sale-to-party-procssing-slip", method = RequestMethod.POST)
    public void print221GdnForSaleToPartyProcssingSlip(HttpServletResponse response, @RequestBody(required = false) Rpt221GdnForSaleToPartyProcssingSlipRequest request) throws Exception {
        if (request == null) request = new Rpt221GdnForSaleToPartyProcssingSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "221_GdnForSaleToPartyProcssingSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/221-gdn-for-sale-to-party-procssing-slip", method = RequestMethod.GET)
    public void print221GdnForSaleToPartyProcssingSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print221GdnForSaleToPartyProcssingSlip(response, objectMapper.convertValue(query, Rpt221GdnForSaleToPartyProcssingSlipRequest.class));
    }

    /**
     * Template: 222-InvRptPurchaseInvoicePurchaseAvgRateByItemSupplier.rpt
     * Procedure: Sp_InvPurchaseInvoice_AvgRatesComparisonsByItemSupplier_Rpt
     * Desktop: InvPurchaseInvoiceReports.InvPurchaseInvoiceAvgRateBySupplier222
     */
    @RequestMapping(value = "/reports/print/222-purchase-invoice-purchase-avg-rate-by-item-supplier", method = RequestMethod.POST)
    public void print222PurchaseInvoicePurchaseAvgRateByItemSupplier(HttpServletResponse response, @RequestBody(required = false) Rpt222PurchaseInvoicePurchaseAvgRateByItemSupplierRequest request) throws Exception {
        if (request == null) request = new Rpt222PurchaseInvoicePurchaseAvgRateByItemSupplierRequest();
        printReport(response, "222-InvRptPurchaseInvoicePurchaseAvgRateByItemSupplier.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/222-purchase-invoice-purchase-avg-rate-by-item-supplier", method = RequestMethod.GET)
    public void print222PurchaseInvoicePurchaseAvgRateByItemSupplierGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print222PurchaseInvoicePurchaseAvgRateByItemSupplier(response, objectMapper.convertValue(query, Rpt222PurchaseInvoicePurchaseAvgRateByItemSupplierRequest.class));
    }

    /**
     * Template: 224-InvRptPurchaseBillRegister.rpt
     * Procedure: Sp_InvPurchaseInvoice_Rpt
     * Desktop: InvPurchaseInvoiceReports.InvPurchaseInvoiceDetailRegister
     */
    @RequestMapping(value = "/reports/print/224-purchase-bill-register", method = RequestMethod.POST)
    public void print224PurchaseBillRegister(HttpServletResponse response, @RequestBody(required = false) Rpt224PurchaseBillRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt224PurchaseBillRegisterRequest();
        printReport(response, "224-InvRptPurchaseBillRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/224-purchase-bill-register", method = RequestMethod.GET)
    public void print224PurchaseBillRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print224PurchaseBillRegister(response, objectMapper.convertValue(query, Rpt224PurchaseBillRegisterRequest.class));
    }

    /**
     * Template: 225A-InvRepPurchaseBillDirectWithoutPo.rpt
     * Procedure: sp_InvpurchaseInvoiceDirectSlip
     * Desktop: InvPurchaseInvoiceReports.PurchaseInvoiceDirectSlip_225
     */
    @RequestMapping(value = "/reports/print/225a-inv-rep-purchase-bill-direct-without-po", method = RequestMethod.POST)
    public void print225AInvRepPurchaseBillDirectWithoutPo(HttpServletResponse response, @RequestBody(required = false) Rpt225AInvRepPurchaseBillDirectWithoutPoRequest request) throws Exception {
        if (request == null) request = new Rpt225AInvRepPurchaseBillDirectWithoutPoRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "225A-InvRepPurchaseBillDirectWithoutPo.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/225a-inv-rep-purchase-bill-direct-without-po", method = RequestMethod.GET)
    public void print225AInvRepPurchaseBillDirectWithoutPoGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print225AInvRepPurchaseBillDirectWithoutPo(response, objectMapper.convertValue(query, Rpt225AInvRepPurchaseBillDirectWithoutPoRequest.class));
    }

    /**
     * Template: 227-RptInvPurchaseInvoiceRegister.rpt
     * Procedure: Sp_InvPurchaseInvoiceTrading_SupplierBill_Register
     * Desktop: PurchaseTrading.GetInvoiceRegister
     */
    @RequestMapping(value = "/reports/print/227-inv-purchase-invoice-register", method = RequestMethod.POST)
    public void print227InvPurchaseInvoiceRegister(HttpServletResponse response, @RequestBody(required = false) Rpt227InvPurchaseInvoiceRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt227InvPurchaseInvoiceRegisterRequest();
        printReport(response, "227-RptInvPurchaseInvoiceRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/227-inv-purchase-invoice-register", method = RequestMethod.GET)
    public void print227InvPurchaseInvoiceRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print227InvPurchaseInvoiceRegister(response, objectMapper.convertValue(query, Rpt227InvPurchaseInvoiceRegisterRequest.class));
    }

    /**
     * Template: 232-SpInventoryTransactions_PurchasesingRicePaddy_NewRpt_Format-I.rpt
     * Procedure: SpInventoryTransactions_PurchasesingRicePaddy_Rpt
     * Desktop: InvPurchaseInvoiceReports.RicePaddyPuchaseReport
     */
    @RequestMapping(value = "/reports/print/232-sp-inventory-transactions-purchasesing-rice-paddy-new-format-i", method = RequestMethod.POST)
    public void print232SpInventoryTransactionsPurchasesingRicePaddyNewFormatI(HttpServletResponse response, @RequestBody(required = false) Rpt232SpInventoryTransactionsPurchasesingRicePaddyNewFormatIRequest request) throws Exception {
        if (request == null) request = new Rpt232SpInventoryTransactionsPurchasesingRicePaddyNewFormatIRequest();
        printReport(response, "232-SpInventoryTransactions_PurchasesingRicePaddy_NewRpt_Format-I.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/232-sp-inventory-transactions-purchasesing-rice-paddy-new-format-i", method = RequestMethod.GET)
    public void print232SpInventoryTransactionsPurchasesingRicePaddyNewFormatIGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print232SpInventoryTransactionsPurchasesingRicePaddyNewFormatI(response, objectMapper.convertValue(query, Rpt232SpInventoryTransactionsPurchasesingRicePaddyNewFormatIRequest.class));
    }

    /**
     * Template: 232-SpInventoryTransactions_PurchasesingRicePaddy_NewRpt_Format-II.rpt
     * Procedure: SpInventoryTransactions_PurchasesingRicePaddy_Rpt
     * Desktop: InvPurchaseInvoiceReports.RicePaddyPuchaseReport
     */
    @RequestMapping(value = "/reports/print/232-sp-inventory-transactions-purchasesing-rice-paddy-new-format-ii", method = RequestMethod.POST)
    public void print232SpInventoryTransactionsPurchasesingRicePaddyNewFormatII(HttpServletResponse response, @RequestBody(required = false) Rpt232SpInventoryTransactionsPurchasesingRicePaddyNewFormatIIRequest request) throws Exception {
        if (request == null) request = new Rpt232SpInventoryTransactionsPurchasesingRicePaddyNewFormatIIRequest();
        printReport(response, "232-SpInventoryTransactions_PurchasesingRicePaddy_NewRpt_Format-II.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/232-sp-inventory-transactions-purchasesing-rice-paddy-new-format-ii", method = RequestMethod.GET)
    public void print232SpInventoryTransactionsPurchasesingRicePaddyNewFormatIIGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print232SpInventoryTransactionsPurchasesingRicePaddyNewFormatII(response, objectMapper.convertValue(query, Rpt232SpInventoryTransactionsPurchasesingRicePaddyNewFormatIIRequest.class));
    }

    /**
     * Template: 234-PurchaseRegisterSummary.rpt
     * Procedure: Usp_InvPurchaseInvoice_PurchasingReports
     * Desktop: InventoryStockEvalautionDetail.PurchasesingRegister
     */
    @RequestMapping(value = "/reports/print/234-purchase-register-summary", method = RequestMethod.POST)
    public void print234PurchaseRegisterSummary(HttpServletResponse response, @RequestBody(required = false) Rpt234PurchaseRegisterSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt234PurchaseRegisterSummaryRequest();
        printReport(response, "234-PurchaseRegisterSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/234-purchase-register-summary", method = RequestMethod.GET)
    public void print234PurchaseRegisterSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print234PurchaseRegisterSummary(response, objectMapper.convertValue(query, Rpt234PurchaseRegisterSummaryRequest.class));
    }

    /**
     * Template: 235-PurchaseRegisterSummaryByItem&Customer.rpt
     * Procedure: Usp_InvPurchaseInvoice_PurchasingReports
     * Desktop: InventoryStockEvalautionDetail.PurchasesingRegister
     */
    @RequestMapping(value = "/reports/print/235-purchase-register-summary-by-item-customer", method = RequestMethod.POST)
    public void print235PurchaseRegisterSummaryByItemCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt235PurchaseRegisterSummaryByItemCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt235PurchaseRegisterSummaryByItemCustomerRequest();
        printReport(response, "235-PurchaseRegisterSummaryByItem&Customer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/235-purchase-register-summary-by-item-customer", method = RequestMethod.GET)
    public void print235PurchaseRegisterSummaryByItemCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print235PurchaseRegisterSummaryByItemCustomer(response, objectMapper.convertValue(query, Rpt235PurchaseRegisterSummaryByItemCustomerRequest.class));
    }

    /**
     * Template: 236-PurchaseRegisterSummaryByItem.rpt
     * Procedure: Usp_InvPurchaseInvoice_PurchasingReports
     * Desktop: InventoryStockEvalautionDetail.PurchasesingRegister
     */
    @RequestMapping(value = "/reports/print/236-purchase-register-summary-by-item", method = RequestMethod.POST)
    public void print236PurchaseRegisterSummaryByItem(HttpServletResponse response, @RequestBody(required = false) Rpt236PurchaseRegisterSummaryByItemRequest request) throws Exception {
        if (request == null) request = new Rpt236PurchaseRegisterSummaryByItemRequest();
        printReport(response, "236-PurchaseRegisterSummaryByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/236-purchase-register-summary-by-item", method = RequestMethod.GET)
    public void print236PurchaseRegisterSummaryByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print236PurchaseRegisterSummaryByItem(response, objectMapper.convertValue(query, Rpt236PurchaseRegisterSummaryByItemRequest.class));
    }

    /**
     * Template: 245_PurchaseInvoiceDirectPM_PartySlip.rpt
     * Procedure: USP_InvPurchaseInvoiceDirect_PartySlip_Engr
     * Desktop: InvPurchaseInvoiceReports.InvPurchaseInvoiceDirect_PartySlip_Engr
     */
    @RequestMapping(value = "/reports/print/245-purchase-invoice-direct-pm-party-slip", method = RequestMethod.POST)
    public void print245PurchaseInvoiceDirectPMPartySlip(HttpServletResponse response, @RequestBody(required = false) Rpt245PurchaseInvoiceDirectPMPartySlipRequest request) throws Exception {
        if (request == null) request = new Rpt245PurchaseInvoiceDirectPMPartySlipRequest();
        printReport(response, "245_PurchaseInvoiceDirectPM_PartySlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/245-purchase-invoice-direct-pm-party-slip", method = RequestMethod.GET)
    public void print245PurchaseInvoiceDirectPMPartySlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print245PurchaseInvoiceDirectPMPartySlip(response, objectMapper.convertValue(query, Rpt245PurchaseInvoiceDirectPMPartySlipRequest.class));
    }

    /**
     * Template: 246_SupplierDispatchSlip.rpt
     * Procedure: [dbo].[USP_SupplierDispatch_SlipAndRegister]
     * Desktop: SupplierDispatch.SupplierDispatch_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/246-supplier-dispatch-slip", method = RequestMethod.POST)
    public void print246SupplierDispatchSlip(HttpServletResponse response, @RequestBody(required = false) Rpt246SupplierDispatchSlipRequest request) throws Exception {
        if (request == null) request = new Rpt246SupplierDispatchSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "246_SupplierDispatchSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/246-supplier-dispatch-slip", method = RequestMethod.GET)
    public void print246SupplierDispatchSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print246SupplierDispatchSlip(response, objectMapper.convertValue(query, Rpt246SupplierDispatchSlipRequest.class));
    }

    /**
     * Template: 251_SupplierDispatchPreBillSlip.rpt
     * Procedure: [dbo].[USP_SupplierDispatch_SlipAndRegister]
     * Desktop: SupplierDispatch.SupplierDispatch_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/251-supplier-dispatch-pre-bill-slip", method = RequestMethod.POST)
    public void print251SupplierDispatchPreBillSlip(HttpServletResponse response, @RequestBody(required = false) Rpt251SupplierDispatchPreBillSlipRequest request) throws Exception {
        if (request == null) request = new Rpt251SupplierDispatchPreBillSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "251_SupplierDispatchPreBillSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/251-supplier-dispatch-pre-bill-slip", method = RequestMethod.GET)
    public void print251SupplierDispatchPreBillSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print251SupplierDispatchPreBillSlip(response, objectMapper.convertValue(query, Rpt251SupplierDispatchPreBillSlipRequest.class));
    }

    /**
     * Template: 253_01-InvRptGatePassInwardRegisterA.rpt
     * Procedure: Sp_GatePassInward_StockReservedAsAmanat
     * Desktop: GatePassInward.GatePassInward_StockReservedAsAmanat
     */
    @RequestMapping(value = "/reports/print/253-01-gate-pass-inward-register-a", method = RequestMethod.POST)
    public void print25301GatePassInwardRegisterA(HttpServletResponse response, @RequestBody(required = false) Rpt25301GatePassInwardRegisterARequest request) throws Exception {
        if (request == null) request = new Rpt25301GatePassInwardRegisterARequest();
        printReport(response, "253_01-InvRptGatePassInwardRegisterA.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/253-01-gate-pass-inward-register-a", method = RequestMethod.GET)
    public void print25301GatePassInwardRegisterAGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print25301GatePassInwardRegisterA(response, objectMapper.convertValue(query, Rpt25301GatePassInwardRegisterARequest.class));
    }

    /**
     * Template: 254-RptInwardGatePassSlipGeneral.rpt
     * Procedure: Sp_GatePassGeneralInward_rpt
     * Desktop: GatePassInwardReports.GatePassGeneralInward254
     */
    @RequestMapping(value = "/reports/print/254-inward-gate-pass-slip-general", method = RequestMethod.POST)
    public void print254InwardGatePassSlipGeneral(HttpServletResponse response, @RequestBody(required = false) Rpt254InwardGatePassSlipGeneralRequest request) throws Exception {
        if (request == null) request = new Rpt254InwardGatePassSlipGeneralRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "254-RptInwardGatePassSlipGeneral.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/254-inward-gate-pass-slip-general", method = RequestMethod.GET)
    public void print254InwardGatePassSlipGeneralGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print254InwardGatePassSlipGeneral(response, objectMapper.convertValue(query, Rpt254InwardGatePassSlipGeneralRequest.class));
    }

    /**
     * Template: 260A-DeliveryChallansByGDN.rpt
     * Procedure: Sp_InvGdn_SlipAndRegisterRice_Rpt
     * Desktop: InvGrnandGdnReports.InvGdnSlip260
     */
    @RequestMapping(value = "/reports/print/260a-delivery-challans-by-gdn", method = RequestMethod.POST)
    public void print260ADeliveryChallansByGDN(HttpServletResponse response, @RequestBody(required = false) Rpt260ADeliveryChallansByGDNRequest request) throws Exception {
        if (request == null) request = new Rpt260ADeliveryChallansByGDNRequest();
        printReport(response, "260A-DeliveryChallansByGDN.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/260a-delivery-challans-by-gdn", method = RequestMethod.GET)
    public void print260ADeliveryChallansByGDNGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print260ADeliveryChallansByGDN(response, objectMapper.convertValue(query, Rpt260ADeliveryChallansByGDNRequest.class));
    }

    /**
     * Template: 261-InvRptGdnRegister.rpt
     * Procedure: Sp_InvGdn_Register_Rpt
     * Desktop: InvGrnandGdnReports.InvGdnRegister261
     */
    @RequestMapping(value = "/reports/print/261-gdn-register", method = RequestMethod.POST)
    public void print261GdnRegister(HttpServletResponse response, @RequestBody(required = false) Rpt261GdnRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt261GdnRegisterRequest();
        printReport(response, "261-InvRptGdnRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/261-gdn-register", method = RequestMethod.GET)
    public void print261GdnRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print261GdnRegister(response, objectMapper.convertValue(query, Rpt261GdnRegisterRequest.class));
    }

    /**
     * Template: 264-DeliveryChallanByDeliveryOrder.rpt
     * Procedure: Sp_InvDeliveryOrder_Slip
     * Desktop: InvGrnandGdnReports.InvDeliveryOrderSlip
     */
    @RequestMapping(value = "/reports/print/264-delivery-challan-by-delivery-order", method = RequestMethod.POST)
    public void print264DeliveryChallanByDeliveryOrder(HttpServletResponse response, @RequestBody(required = false) Rpt264DeliveryChallanByDeliveryOrderRequest request) throws Exception {
        if (request == null) request = new Rpt264DeliveryChallanByDeliveryOrderRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "264-DeliveryChallanByDeliveryOrder.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/264-delivery-challan-by-delivery-order", method = RequestMethod.GET)
    public void print264DeliveryChallanByDeliveryOrderGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print264DeliveryChallanByDeliveryOrder(response, objectMapper.convertValue(query, Rpt264DeliveryChallanByDeliveryOrderRequest.class));
    }

    /**
     * Template: 264-DeliveryChallanByDeliveryOrderTransfer.rpt
     * Procedure: Sp_InvDeliveryOrder_Slip
     * Desktop: InvGrnandGdnReports.InvDeliveryOrderSlip
     */
    @RequestMapping(value = "/reports/print/264-delivery-challan-by-delivery-order-transfer", method = RequestMethod.POST)
    public void print264DeliveryChallanByDeliveryOrderTransfer(HttpServletResponse response, @RequestBody(required = false) Rpt264DeliveryChallanByDeliveryOrderTransferRequest request) throws Exception {
        if (request == null) request = new Rpt264DeliveryChallanByDeliveryOrderTransferRequest();
        printReport(response, "264-DeliveryChallanByDeliveryOrderTransfer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/264-delivery-challan-by-delivery-order-transfer", method = RequestMethod.GET)
    public void print264DeliveryChallanByDeliveryOrderTransferGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print264DeliveryChallanByDeliveryOrderTransfer(response, objectMapper.convertValue(query, Rpt264DeliveryChallanByDeliveryOrderTransferRequest.class));
    }

    /**
     * Template: 271-InvRptSalesOrderRegister.rpt
     * Procedure: Sp_PurchaseOrder_History_Rpt
     * Desktop: Dashboard.InventoryDashboardOutstandingPurchaseOrders
     */
    @RequestMapping(value = "/reports/print/271-sales-order-register", method = RequestMethod.POST)
    public void print271SalesOrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt271SalesOrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt271SalesOrderRegisterRequest();
        printReport(response, "271-InvRptSalesOrderRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/271-sales-order-register", method = RequestMethod.GET)
    public void print271SalesOrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print271SalesOrderRegister(response, objectMapper.convertValue(query, Rpt271SalesOrderRegisterRequest.class));
    }

    /**
     * Template: 297-InvRptPurchase BillActivity.rpt
     * Procedure: Sp_InvPurchaseInvoice_Rpt
     * Desktop: InvPurchaseInvoiceReports.InvPurchaseInvoiceDetailRegister
     */
    @RequestMapping(value = "/reports/print/297-purchase-bill-activity", method = RequestMethod.POST)
    public void print297PurchaseBillActivity(HttpServletResponse response, @RequestBody(required = false) Rpt297PurchaseBillActivityRequest request) throws Exception {
        if (request == null) request = new Rpt297PurchaseBillActivityRequest();
        printReport(response, "297-InvRptPurchase BillActivity.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/297-purchase-bill-activity", method = RequestMethod.GET)
    public void print297PurchaseBillActivityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print297PurchaseBillActivity(response, objectMapper.convertValue(query, Rpt297PurchaseBillActivityRequest.class));
    }

    /**
     * Template: 330-InvSupplyOrderSlip.rpt
     * Procedure: Sp_InvSupplyOrder_SlipandRegister
     * Desktop: InvSupplyOrder.SupplyOrderSlipandRegister
     */
    @RequestMapping(value = "/reports/print/330-inv-supply-order-slip", method = RequestMethod.POST)
    public void print330InvSupplyOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt330InvSupplyOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt330InvSupplyOrderSlipRequest();
        printReport(response, "330-InvSupplyOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/330-inv-supply-order-slip", method = RequestMethod.GET)
    public void print330InvSupplyOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print330InvSupplyOrderSlip(response, objectMapper.convertValue(query, Rpt330InvSupplyOrderSlipRequest.class));
    }

    /**
     * Template: 334_01-GrnAuditReport.rpt
     * Procedure: usp_getDataForGrnAudit
     * Desktop: InvGrn.GetGrnDataForAudit
     */
    @RequestMapping(value = "/reports/print/334-01-grn-audit-report", method = RequestMethod.POST)
    public void print33401GrnAuditReport(HttpServletResponse response, @RequestBody(required = false) Rpt33401GrnAuditReportRequest request) throws Exception {
        if (request == null) request = new Rpt33401GrnAuditReportRequest();
        printReport(response, "334_01-GrnAuditReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/334-01-grn-audit-report", method = RequestMethod.GET)
    public void print33401GrnAuditReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print33401GrnAuditReport(response, objectMapper.convertValue(query, Rpt33401GrnAuditReportRequest.class));
    }

    /**
     * Template: 335_01-GrnRegisterSummaryItemWise.rpt
     * Procedure: usp_GrnRegisterSummaryItemWise
     * Desktop: InvGrn.GrnRegisterSummaryItemWise
     */
    @RequestMapping(value = "/reports/print/335-01-grn-register-summary-item-wise", method = RequestMethod.POST)
    public void print33501GrnRegisterSummaryItemWise(HttpServletResponse response, @RequestBody(required = false) Rpt33501GrnRegisterSummaryItemWiseRequest request) throws Exception {
        if (request == null) request = new Rpt33501GrnRegisterSummaryItemWiseRequest();
        printReport(response, "335_01-GrnRegisterSummaryItemWise.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/335-01-grn-register-summary-item-wise", method = RequestMethod.GET)
    public void print33501GrnRegisterSummaryItemWiseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print33501GrnRegisterSummaryItemWise(response, objectMapper.convertValue(query, Rpt33501GrnRegisterSummaryItemWiseRequest.class));
    }

    /**
     * Template: 358_01_PendingGatePassForGRN.rpt
     * Procedure: USP_GatePassInward_PendingGpForUnloading
     * Desktop: GatePassInward.PendingGpForUnloading
     */
    @RequestMapping(value = "/reports/print/358-01-pending-gate-pass-for-grn", method = RequestMethod.POST)
    public void print35801PendingGatePassForGRN(HttpServletResponse response, @RequestBody(required = false) Rpt35801PendingGatePassForGRNRequest request) throws Exception {
        if (request == null) request = new Rpt35801PendingGatePassForGRNRequest();
        printReport(response, "358_01_PendingGatePassForGRN.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/358-01-pending-gate-pass-for-grn", method = RequestMethod.GET)
    public void print35801PendingGatePassForGRNGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print35801PendingGatePassForGRN(response, objectMapper.convertValue(query, Rpt35801PendingGatePassForGRNRequest.class));
    }

    /**
     * Template: 387-GRNWeightAuditReport.rpt
     * Procedure: USP_GRNWeightAuditReport
     * Desktop: InvGrn.GRNWeightAuditReport
     */
    @RequestMapping(value = "/reports/print/387-grn-weight-audit-report", method = RequestMethod.POST)
    public void print387GRNWeightAuditReport(HttpServletResponse response, @RequestBody(required = false) Rpt387GRNWeightAuditReportRequest request) throws Exception {
        if (request == null) request = new Rpt387GRNWeightAuditReportRequest();
        printReport(response, "387-GRNWeightAuditReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/387-grn-weight-audit-report", method = RequestMethod.GET)
    public void print387GRNWeightAuditReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print387GRNWeightAuditReport(response, objectMapper.convertValue(query, Rpt387GRNWeightAuditReportRequest.class));
    }

    /**
     * Template: 391-OrderRegister.rpt
     * Procedure: [dbo].[USP_PurchaseOrderSummaryRegister]
     * Desktop: PurchaseOrder.PurchaseOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/391-order-register", method = RequestMethod.POST)
    public void print391OrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt391OrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt391OrderRegisterRequest();
        printReport(response, "391-OrderRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/391-order-register", method = RequestMethod.GET)
    public void print391OrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print391OrderRegister(response, objectMapper.convertValue(query, Rpt391OrderRegisterRequest.class));
    }

    /**
     * Template: 392-OrderSummaryByItem.rpt
     * Procedure: [dbo].[USP_PurchaseOrderSummaryRegister]
     * Desktop: PurchaseOrder.PurchaseOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/392-order-summary-by-item", method = RequestMethod.POST)
    public void print392OrderSummaryByItem(HttpServletResponse response, @RequestBody(required = false) Rpt392OrderSummaryByItemRequest request) throws Exception {
        if (request == null) request = new Rpt392OrderSummaryByItemRequest();
        printReport(response, "392-OrderSummaryByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/392-order-summary-by-item", method = RequestMethod.GET)
    public void print392OrderSummaryByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print392OrderSummaryByItem(response, objectMapper.convertValue(query, Rpt392OrderSummaryByItemRequest.class));
    }

    /**
     * Template: 393-OrderSummaryBySupplier.rpt
     * Procedure: [dbo].[USP_PurchaseOrderSummaryRegister]
     * Desktop: PurchaseOrder.PurchaseOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/393-order-summary-by-supplier", method = RequestMethod.POST)
    public void print393OrderSummaryBySupplier(HttpServletResponse response, @RequestBody(required = false) Rpt393OrderSummaryBySupplierRequest request) throws Exception {
        if (request == null) request = new Rpt393OrderSummaryBySupplierRequest();
        printReport(response, "393-OrderSummaryBySupplier.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/393-order-summary-by-supplier", method = RequestMethod.GET)
    public void print393OrderSummaryBySupplierGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print393OrderSummaryBySupplier(response, objectMapper.convertValue(query, Rpt393OrderSummaryBySupplierRequest.class));
    }

    /**
     * Template: 394-OrderSummaryByCustomerandItem.rpt
     * Procedure: [dbo].[USP_PurchaseOrderSummaryRegister]
     * Desktop: PurchaseOrder.PurchaseOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/394-order-summary-by-customerand-item", method = RequestMethod.POST)
    public void print394OrderSummaryByCustomerandItem(HttpServletResponse response, @RequestBody(required = false) Rpt394OrderSummaryByCustomerandItemRequest request) throws Exception {
        if (request == null) request = new Rpt394OrderSummaryByCustomerandItemRequest();
        printReport(response, "394-OrderSummaryByCustomerandItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/394-order-summary-by-customerand-item", method = RequestMethod.GET)
    public void print394OrderSummaryByCustomerandItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print394OrderSummaryByCustomerandItem(response, objectMapper.convertValue(query, Rpt394OrderSummaryByCustomerandItemRequest.class));
    }

    /**
     * Template: 413-PaddyPurchaseRegister.rpt
     * Procedure: USP_PaddyGatePurchase_Register
     * Desktop: InvPurchaseInvoiceReports.PaddyGatePurchaseRegister
     */
    @RequestMapping(value = "/reports/print/413-paddy-purchase-register", method = RequestMethod.POST)
    public void print413PaddyPurchaseRegister(HttpServletResponse response, @RequestBody(required = false) Rpt413PaddyPurchaseRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt413PaddyPurchaseRegisterRequest();
        printReport(response, "413-PaddyPurchaseRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/413-paddy-purchase-register", method = RequestMethod.GET)
    public void print413PaddyPurchaseRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print413PaddyPurchaseRegister(response, objectMapper.convertValue(query, Rpt413PaddyPurchaseRegisterRequest.class));
    }

    /**
     * Template: 56_01_PurchaseRegister.rpt
     * Procedure: [dbo].[USP_PurchaseReportWithActivitiesFromEvaulations]
     * Desktop: InvPurchaseInvoice.PurchaseReportWithActivitiesFromEvaulations
     */
    @RequestMapping(value = "/reports/print/56-01-purchase-register", method = RequestMethod.POST)
    public void print5601PurchaseRegister(HttpServletResponse response, @RequestBody(required = false) Rpt5601PurchaseRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt5601PurchaseRegisterRequest();
        printReport(response, "56_01_PurchaseRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/56-01-purchase-register", method = RequestMethod.GET)
    public void print5601PurchaseRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print5601PurchaseRegister(response, objectMapper.convertValue(query, Rpt5601PurchaseRegisterRequest.class));
    }

    /**
     * Template: 56_02_PurchaseSummaryByItem.rpt
     * Procedure: [dbo].[USP_PurchaseReportWithActivitiesFromEvaulations]
     * Desktop: InvPurchaseInvoice.PurchaseReportWithActivitiesFromEvaulations
     */
    @RequestMapping(value = "/reports/print/56-02-purchase-summary-by-item", method = RequestMethod.POST)
    public void print5602PurchaseSummaryByItem(HttpServletResponse response, @RequestBody(required = false) Rpt5602PurchaseSummaryByItemRequest request) throws Exception {
        if (request == null) request = new Rpt5602PurchaseSummaryByItemRequest();
        printReport(response, "56_02_PurchaseSummaryByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/56-02-purchase-summary-by-item", method = RequestMethod.GET)
    public void print5602PurchaseSummaryByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print5602PurchaseSummaryByItem(response, objectMapper.convertValue(query, Rpt5602PurchaseSummaryByItemRequest.class));
    }

    /**
     * Template: 56_03_PurchaseSummaryByItemAndWarehouse.rpt
     * Procedure: [dbo].[USP_PurchaseReportWithActivitiesFromEvaulations]
     * Desktop: InvPurchaseInvoice.PurchaseReportWithActivitiesFromEvaulations
     */
    @RequestMapping(value = "/reports/print/56-03-purchase-summary-by-item-and-warehouse", method = RequestMethod.POST)
    public void print5603PurchaseSummaryByItemAndWarehouse(HttpServletResponse response, @RequestBody(required = false) Rpt5603PurchaseSummaryByItemAndWarehouseRequest request) throws Exception {
        if (request == null) request = new Rpt5603PurchaseSummaryByItemAndWarehouseRequest();
        printReport(response, "56_03_PurchaseSummaryByItemAndWarehouse.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/56-03-purchase-summary-by-item-and-warehouse", method = RequestMethod.GET)
    public void print5603PurchaseSummaryByItemAndWarehouseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print5603PurchaseSummaryByItemAndWarehouse(response, objectMapper.convertValue(query, Rpt5603PurchaseSummaryByItemAndWarehouseRequest.class));
    }

    /**
     * Template: 56_04_PurchaseSummaryBySupplier.rpt
     * Procedure: [dbo].[USP_PurchaseReportWithActivitiesFromEvaulations]
     * Desktop: InvPurchaseInvoice.PurchaseReportWithActivitiesFromEvaulations
     */
    @RequestMapping(value = "/reports/print/56-04-purchase-summary-by-supplier", method = RequestMethod.POST)
    public void print5604PurchaseSummaryBySupplier(HttpServletResponse response, @RequestBody(required = false) Rpt5604PurchaseSummaryBySupplierRequest request) throws Exception {
        if (request == null) request = new Rpt5604PurchaseSummaryBySupplierRequest();
        printReport(response, "56_04_PurchaseSummaryBySupplier.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/56-04-purchase-summary-by-supplier", method = RequestMethod.GET)
    public void print5604PurchaseSummaryBySupplierGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print5604PurchaseSummaryBySupplier(response, objectMapper.convertValue(query, Rpt5604PurchaseSummaryBySupplierRequest.class));
    }

    /**
     * Template: 56_05_PurchaseSummaryByItemAndSupplier.rpt
     * Procedure: [dbo].[USP_PurchaseReportWithActivitiesFromEvaulations]
     * Desktop: InvPurchaseInvoice.PurchaseReportWithActivitiesFromEvaulations
     */
    @RequestMapping(value = "/reports/print/56-05-purchase-summary-by-item-and-supplier", method = RequestMethod.POST)
    public void print5605PurchaseSummaryByItemAndSupplier(HttpServletResponse response, @RequestBody(required = false) Rpt5605PurchaseSummaryByItemAndSupplierRequest request) throws Exception {
        if (request == null) request = new Rpt5605PurchaseSummaryByItemAndSupplierRequest();
        printReport(response, "56_05_PurchaseSummaryByItemAndSupplier.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/56-05-purchase-summary-by-item-and-supplier", method = RequestMethod.GET)
    public void print5605PurchaseSummaryByItemAndSupplierGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print5605PurchaseSummaryByItemAndSupplier(response, objectMapper.convertValue(query, Rpt5605PurchaseSummaryByItemAndSupplierRequest.class));
    }

    /**
     * Template: 56_06_PurchaseSummaryByParentCategory.rpt
     * Procedure: [dbo].[USP_PurchaseReportWithActivitiesFromEvaulations]
     * Desktop: InvPurchaseInvoice.PurchaseReportWithActivitiesFromEvaulations
     */
    @RequestMapping(value = "/reports/print/56-06-purchase-summary-by-parent-category", method = RequestMethod.POST)
    public void print5606PurchaseSummaryByParentCategory(HttpServletResponse response, @RequestBody(required = false) Rpt5606PurchaseSummaryByParentCategoryRequest request) throws Exception {
        if (request == null) request = new Rpt5606PurchaseSummaryByParentCategoryRequest();
        printReport(response, "56_06_PurchaseSummaryByParentCategory.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/56-06-purchase-summary-by-parent-category", method = RequestMethod.GET)
    public void print5606PurchaseSummaryByParentCategoryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print5606PurchaseSummaryByParentCategory(response, objectMapper.convertValue(query, Rpt5606PurchaseSummaryByParentCategoryRequest.class));
    }

    /**
     * Template: 56_07_PurchaseSummaryByParentCategoryAndSupplier.rpt
     * Procedure: [dbo].[USP_PurchaseReportWithActivitiesFromEvaulations]
     * Desktop: InvPurchaseInvoice.PurchaseReportWithActivitiesFromEvaulations
     */
    @RequestMapping(value = "/reports/print/56-07-purchase-summary-by-parent-category-and-supplier", method = RequestMethod.POST)
    public void print5607PurchaseSummaryByParentCategoryAndSupplier(HttpServletResponse response, @RequestBody(required = false) Rpt5607PurchaseSummaryByParentCategoryAndSupplierRequest request) throws Exception {
        if (request == null) request = new Rpt5607PurchaseSummaryByParentCategoryAndSupplierRequest();
        printReport(response, "56_07_PurchaseSummaryByParentCategoryAndSupplier.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/56-07-purchase-summary-by-parent-category-and-supplier", method = RequestMethod.GET)
    public void print5607PurchaseSummaryByParentCategoryAndSupplierGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print5607PurchaseSummaryByParentCategoryAndSupplier(response, objectMapper.convertValue(query, Rpt5607PurchaseSummaryByParentCategoryAndSupplierRequest.class));
    }

    /**
     * Template: 56_08_PurchaseAndSaleDetailByJobLot.rpt
     * Procedure: [dbo].[usp_PurchaseAndSaleDetailByJobLot]
     * Desktop: StocksReport.PurchaseAndSaleDetailByJobLot
     */
    @RequestMapping(value = "/reports/print/56-08-purchase-and-sale-detail-by-job-lot", method = RequestMethod.POST)
    public void print5608PurchaseAndSaleDetailByJobLot(HttpServletResponse response, @RequestBody(required = false) Rpt5608PurchaseAndSaleDetailByJobLotRequest request) throws Exception {
        if (request == null) request = new Rpt5608PurchaseAndSaleDetailByJobLotRequest();
        printReport(response, "56_08_PurchaseAndSaleDetailByJobLot.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/56-08-purchase-and-sale-detail-by-job-lot", method = RequestMethod.GET)
    public void print5608PurchaseAndSaleDetailByJobLotGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print5608PurchaseAndSaleDetailByJobLot(response, objectMapper.convertValue(query, Rpt5608PurchaseAndSaleDetailByJobLotRequest.class));
    }

    /**
     * Template: 56_09_PurchaseSummaryByHsCode.rpt
     * Procedure: [dbo].[USP_PurchaseReportWithActivitiesFromEvaulations]
     * Desktop: InvPurchaseInvoice.PurchaseReportWithActivitiesFromEvaulations
     */
    @RequestMapping(value = "/reports/print/56-09-purchase-summary-by-hs-code", method = RequestMethod.POST)
    public void print5609PurchaseSummaryByHsCode(HttpServletResponse response, @RequestBody(required = false) Rpt5609PurchaseSummaryByHsCodeRequest request) throws Exception {
        if (request == null) request = new Rpt5609PurchaseSummaryByHsCodeRequest();
        printReport(response, "56_09_PurchaseSummaryByHsCode.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/56-09-purchase-summary-by-hs-code", method = RequestMethod.GET)
    public void print5609PurchaseSummaryByHsCodeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print5609PurchaseSummaryByHsCode(response, objectMapper.convertValue(query, Rpt5609PurchaseSummaryByHsCodeRequest.class));
    }

    /**
     * Template: 910_01_AdvanceDeliveryOrderSlip.rpt
     * Procedure: Sp_InvDeliveryOrder_Slip
     * Desktop: InvGrnandGdnReports.InvDeliveryOrderSlip
     */
    @RequestMapping(value = "/reports/print/910-01-advance-delivery-order-slip", method = RequestMethod.POST)
    public void print91001AdvanceDeliveryOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt91001AdvanceDeliveryOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt91001AdvanceDeliveryOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "910_01_AdvanceDeliveryOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/910-01-advance-delivery-order-slip", method = RequestMethod.GET)
    public void print91001AdvanceDeliveryOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print91001AdvanceDeliveryOrderSlip(response, objectMapper.convertValue(query, Rpt91001AdvanceDeliveryOrderSlipRequest.class));
    }

    /**
     * Template: 910_AdvanceDeliveryOrderSlip.rpt
     * Procedure: Sp_InvDeliveryOrder_Slip
     * Desktop: InvGrnandGdnReports.InvDeliveryOrderSlip
     */
    @RequestMapping(value = "/reports/print/910-advance-delivery-order-slip", method = RequestMethod.POST)
    public void print910AdvanceDeliveryOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt910AdvanceDeliveryOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt910AdvanceDeliveryOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "910_AdvanceDeliveryOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/910-advance-delivery-order-slip", method = RequestMethod.GET)
    public void print910AdvanceDeliveryOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print910AdvanceDeliveryOrderSlip(response, objectMapper.convertValue(query, Rpt910AdvanceDeliveryOrderSlipRequest.class));
    }

    /**
     * Template: 910A_AdvanceDeliveryChallanSlip.rpt
     * Procedure: Sp_InvDeliveryOrder_Slip
     * Desktop: InvGrnandGdnReports.InvDeliveryOrderSlip
     */
    @RequestMapping(value = "/reports/print/910a-advance-delivery-challan-slip", method = RequestMethod.POST)
    public void print910AAdvanceDeliveryChallanSlip(HttpServletResponse response, @RequestBody(required = false) Rpt910AAdvanceDeliveryChallanSlipRequest request) throws Exception {
        if (request == null) request = new Rpt910AAdvanceDeliveryChallanSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "910A_AdvanceDeliveryChallanSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/910a-advance-delivery-challan-slip", method = RequestMethod.GET)
    public void print910AAdvanceDeliveryChallanSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print910AAdvanceDeliveryChallanSlip(response, objectMapper.convertValue(query, Rpt910AAdvanceDeliveryChallanSlipRequest.class));
    }

    /**
     * Template: 98_SaleInvoiceReturnItemSlip.rpt
     * Procedure: sp_InvpurchaseInvoiceDirectSlip
     * Desktop: InvPurchaseInvoiceReports.PurchaseInvoiceDirectSlip_225
     */
    @RequestMapping(value = "/reports/print/98-sale-invoice-return-item-slip", method = RequestMethod.POST)
    public void print98SaleInvoiceReturnItemSlip(HttpServletResponse response, @RequestBody(required = false) Rpt98SaleInvoiceReturnItemSlipRequest request) throws Exception {
        if (request == null) request = new Rpt98SaleInvoiceReturnItemSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "98_SaleInvoiceReturnItemSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/98-sale-invoice-return-item-slip", method = RequestMethod.GET)
    public void print98SaleInvoiceReturnItemSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print98SaleInvoiceReturnItemSlip(response, objectMapper.convertValue(query, Rpt98SaleInvoiceReturnItemSlipRequest.class));
    }

    /**
     * Template: 98A_SaleInvoiceReturnPartySlip.rpt
     * Procedure: sp_InvpurchaseInvoiceDirectSlip
     * Desktop: InvPurchaseInvoiceReports.PurchaseInvoiceDirectSlip_225
     */
    @RequestMapping(value = "/reports/print/98a-sale-invoice-return-party-slip", method = RequestMethod.POST)
    public void print98ASaleInvoiceReturnPartySlip(HttpServletResponse response, @RequestBody(required = false) Rpt98ASaleInvoiceReturnPartySlipRequest request) throws Exception {
        if (request == null) request = new Rpt98ASaleInvoiceReturnPartySlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "98A_SaleInvoiceReturnPartySlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/98a-sale-invoice-return-party-slip", method = RequestMethod.GET)
    public void print98ASaleInvoiceReturnPartySlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print98ASaleInvoiceReturnPartySlip(response, objectMapper.convertValue(query, Rpt98ASaleInvoiceReturnPartySlipRequest.class));
    }

    /**
     * Template: getLedgerforPrint.rpt
     * Procedure: usp_getLedgerforPrint
     * Desktop: InvPurchaseInvoiceReports.GetDataForLedgerSubReport
     */
    @RequestMapping(value = "/reports/print/get-ledgerfor-print", method = RequestMethod.POST)
    public void printGetLedgerforPrint(HttpServletResponse response, @RequestBody(required = false) RptGetLedgerforPrintRequest request) throws Exception {
        if (request == null) request = new RptGetLedgerforPrintRequest();
        printReport(response, "getLedgerforPrint.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/get-ledgerfor-print", method = RequestMethod.GET)
    public void printGetLedgerforPrintGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printGetLedgerforPrint(response, objectMapper.convertValue(query, RptGetLedgerforPrintRequest.class));
    }

    /**
     * Template: GrnDetailEmptyBagsSubReport.rpt
     * Procedure: Sp_InvGrnDetailEmptyBagsSubReport
     * Desktop: InvGrnandGdnReports.GrnDetailEmptyBagsSubReport
     */
    @RequestMapping(value = "/reports/print/grn-detail-empty-bags-sub-report", method = RequestMethod.POST)
    public void printGrnDetailEmptyBagsSubReport(HttpServletResponse response, @RequestBody(required = false) RptGrnDetailEmptyBagsSubReportRequest request) throws Exception {
        if (request == null) request = new RptGrnDetailEmptyBagsSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "GrnDetailEmptyBagsSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/grn-detail-empty-bags-sub-report", method = RequestMethod.GET)
    public void printGrnDetailEmptyBagsSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printGrnDetailEmptyBagsSubReport(response, objectMapper.convertValue(query, RptGrnDetailEmptyBagsSubReportRequest.class));
    }

    /**
     * Template: InvRptPurchaseBillSupplierOthers.rpt
     * Procedure: SP_InvSaleInvoice_CustomerBillRice OthersExp_SubRep
     * Desktop: InvSaleInvoiceReports.SalesCustomerBillSubReport
     */
    @RequestMapping(value = "/reports/print/purchase-bill-supplier-others", method = RequestMethod.POST)
    public void printPurchaseBillSupplierOthers(HttpServletResponse response, @RequestBody(required = false) RptPurchaseBillSupplierOthersRequest request) throws Exception {
        if (request == null) request = new RptPurchaseBillSupplierOthersRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "InvRptPurchaseBillSupplierOthers.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/purchase-bill-supplier-others", method = RequestMethod.GET)
    public void printPurchaseBillSupplierOthersGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printPurchaseBillSupplierOthers(response, objectMapper.convertValue(query, RptPurchaseBillSupplierOthersRequest.class));
    }

    /**
     * Template: InvRptPurchaseItemBillOthers.rpt
     * Procedure: SP_InvPurchaseInvoice_ItemOthersAddLess_SubRpt
     * Desktop: InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprtForItem
     */
    @RequestMapping(value = "/reports/print/purchase-item-bill-others", method = RequestMethod.POST)
    public void printPurchaseItemBillOthers(HttpServletResponse response, @RequestBody(required = false) RptPurchaseItemBillOthersRequest request) throws Exception {
        if (request == null) request = new RptPurchaseItemBillOthersRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "InvRptPurchaseItemBillOthers.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/purchase-item-bill-others", method = RequestMethod.GET)
    public void printPurchaseItemBillOthersGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printPurchaseItemBillOthers(response, objectMapper.convertValue(query, RptPurchaseItemBillOthersRequest.class));
    }

    /**
     * Template: PurchaseOrder_SupplierDispatchSubReport.rpt
     * Procedure: USP_PurchaseOrder_SupplierDispatchSubReport
     * Desktop: PurchaseOrderReports.SupplierDispatchSubReport_203A
     */
    @RequestMapping(value = "/reports/print/purchase-order-supplier-dispatch-sub-report", method = RequestMethod.POST)
    public void printPurchaseOrderSupplierDispatchSubReport(HttpServletResponse response, @RequestBody(required = false) RptPurchaseOrderSupplierDispatchSubReportRequest request) throws Exception {
        if (request == null) request = new RptPurchaseOrderSupplierDispatchSubReportRequest();
        printReport(response, "PurchaseOrder_SupplierDispatchSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/purchase-order-supplier-dispatch-sub-report", method = RequestMethod.GET)
    public void printPurchaseOrderSupplierDispatchSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printPurchaseOrderSupplierDispatchSubReport(response, objectMapper.convertValue(query, RptPurchaseOrderSupplierDispatchSubReportRequest.class));
    }

    /**
     * Template: PurchaseOrder_WithGrnDetail_SubReport.rpt
     * Procedure: [dbo].[USP_PurchaseOrder_WithGrnDetail_Report]
     * Desktop: InvGrnandGdnReports.PurchaseOrder_WithGrnDetail_Report
     */
    @RequestMapping(value = "/reports/print/purchase-order-with-grn-detail-sub-report", method = RequestMethod.POST)
    public void printPurchaseOrderWithGrnDetailSubReport(HttpServletResponse response, @RequestBody(required = false) RptPurchaseOrderWithGrnDetailSubReportRequest request) throws Exception {
        if (request == null) request = new RptPurchaseOrderWithGrnDetailSubReportRequest();
        printReport(response, "PurchaseOrder_WithGrnDetail_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/purchase-order-with-grn-detail-sub-report", method = RequestMethod.GET)
    public void printPurchaseOrderWithGrnDetailSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printPurchaseOrderWithGrnDetailSubReport(response, objectMapper.convertValue(query, RptPurchaseOrderWithGrnDetailSubReportRequest.class));
    }

    /**
     * Template: PurchaseOrderLabSampleSubReport.rpt
     * Procedure: [dbo].[USP_PurchaseOrderLabSample_SubReport]
     * Desktop: PurchaseOrderReports.PurchaseOrderLabSample_SubReport
     */
    @RequestMapping(value = "/reports/print/purchase-order-lab-sample-sub-report", method = RequestMethod.POST)
    public void printPurchaseOrderLabSampleSubReport(HttpServletResponse response, @RequestBody(required = false) RptPurchaseOrderLabSampleSubReportRequest request) throws Exception {
        if (request == null) request = new RptPurchaseOrderLabSampleSubReportRequest();
        printReport(response, "PurchaseOrderLabSampleSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/purchase-order-lab-sample-sub-report", method = RequestMethod.GET)
    public void printPurchaseOrderLabSampleSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printPurchaseOrderLabSampleSubReport(response, objectMapper.convertValue(query, RptPurchaseOrderLabSampleSubReportRequest.class));
    }

    /**
     * Template: PurchaseOrderSubReport.rpt
     * Procedure: USP-PurchaseOrderSubReport
     * Desktop: PurchaseOrderReports.PurchaseOrderSlipReport203SubReport
     */
    @RequestMapping(value = "/reports/print/purchase-order-sub-report", method = RequestMethod.POST)
    public void printPurchaseOrderSubReport(HttpServletResponse response, @RequestBody(required = false) RptPurchaseOrderSubReportRequest request) throws Exception {
        if (request == null) request = new RptPurchaseOrderSubReportRequest();
        printReport(response, "PurchaseOrderSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/purchase-order-sub-report", method = RequestMethod.GET)
    public void printPurchaseOrderSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printPurchaseOrderSubReport(response, objectMapper.convertValue(query, RptPurchaseOrderSubReportRequest.class));
    }

    /**
     * Template: PurchaseOrderSupplierExpenseSubReport.rpt
     * Procedure: [dbo].[USP_PurchaseOrderSupplierExpense_SubReport]
     * Desktop: PurchaseOrderReports.PurchaseOrderSupplierExpense_SubReport
     */
    @RequestMapping(value = "/reports/print/purchase-order-supplier-expense-sub-report", method = RequestMethod.POST)
    public void printPurchaseOrderSupplierExpenseSubReport(HttpServletResponse response, @RequestBody(required = false) RptPurchaseOrderSupplierExpenseSubReportRequest request) throws Exception {
        if (request == null) request = new RptPurchaseOrderSupplierExpenseSubReportRequest();
        printReport(response, "PurchaseOrderSupplierExpenseSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/purchase-order-supplier-expense-sub-report", method = RequestMethod.GET)
    public void printPurchaseOrderSupplierExpenseSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printPurchaseOrderSupplierExpenseSubReport(response, objectMapper.convertValue(query, RptPurchaseOrderSupplierExpenseSubReportRequest.class));
    }

    /**
     * Template: SupplierBillReport.rpt
     * Procedure: SP_InvPurchaseInvoice_SupplierBillOthersAddLess_SubRpt
     * Desktop: InvPurchaseInvoiceReports.InvPurchaseInvoiceSlipReport220SupReprt
     */
    @RequestMapping(value = "/reports/print/supplier-bill-report", method = RequestMethod.POST)
    public void printSupplierBillReport(HttpServletResponse response, @RequestBody(required = false) RptSupplierBillReportRequest request) throws Exception {
        if (request == null) request = new RptSupplierBillReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "SupplierBillReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/supplier-bill-report", method = RequestMethod.GET)
    public void printSupplierBillReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printSupplierBillReport(response, objectMapper.convertValue(query, RptSupplierBillReportRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
