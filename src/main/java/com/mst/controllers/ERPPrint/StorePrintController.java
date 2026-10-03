package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.StorePrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** Store print actions. Generated from the verified seeder contracts. */
@Controller
public class StorePrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 148_DeliveryChallanSlip.rpt
     * Procedure: [dbo].[USP_DeliveryChallanHeader_Slip]
     * Desktop: DeliveryChallanHeader.DeliveryChallanHeader_Slip
     */
    @RequestMapping(value = "/reports/print/148-delivery-challan-slip", method = RequestMethod.POST)
    public void print148DeliveryChallanSlip(HttpServletResponse response, @RequestBody(required = false) Rpt148DeliveryChallanSlipRequest request) throws Exception {
        if (request == null) request = new Rpt148DeliveryChallanSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "148_DeliveryChallanSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/148-delivery-challan-slip", method = RequestMethod.GET)
    public void print148DeliveryChallanSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print148DeliveryChallanSlip(response, objectMapper.convertValue(query, Rpt148DeliveryChallanSlipRequest.class));
    }

    /**
     * Template: 262-DeliveryOrderSlip.rpt
     * Procedure: Sp_InvDeliveryOrder_Slip
     * Desktop: InvGrnandGdnReports.InvDeliveryOrderSlip
     */
    @RequestMapping(value = "/reports/print/262-delivery-order-slip", method = RequestMethod.POST)
    public void print262DeliveryOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt262DeliveryOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt262DeliveryOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "262-DeliveryOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/262-delivery-order-slip", method = RequestMethod.GET)
    public void print262DeliveryOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print262DeliveryOrderSlip(response, objectMapper.convertValue(query, Rpt262DeliveryOrderSlipRequest.class));
    }

    /**
     * Template: 451-RptDepartmentRequestSlip.rpt
     * Procedure: Sp_DepartmentRequest_History
     * Desktop: DepartmentRequest.DepartmentRequestHistory
     */
    @RequestMapping(value = "/reports/print/451-department-request-slip", method = RequestMethod.POST)
    public void print451DepartmentRequestSlip(HttpServletResponse response, @RequestBody(required = false) Rpt451DepartmentRequestSlipRequest request) throws Exception {
        if (request == null) request = new Rpt451DepartmentRequestSlipRequest();
        printReport(response, "451-RptDepartmentRequestSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/451-department-request-slip", method = RequestMethod.GET)
    public void print451DepartmentRequestSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print451DepartmentRequestSlip(response, objectMapper.convertValue(query, Rpt451DepartmentRequestSlipRequest.class));
    }

    /**
     * Template: 1615-DepartmentRequestToConsumableStoreSlip.rpt
     * Procedure: Sp_DepartmentRequest_History
     * Desktop: DepartmentRequest.DepartmentRequestHistory
     */
    @RequestMapping(value = "/reports/print/1615-department-request-to-consumable-store-slip", method = RequestMethod.POST)
    public void print1615DepartmentRequestToConsumableStoreSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1615DepartmentRequestToConsumableStoreSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1615DepartmentRequestToConsumableStoreSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1615-DepartmentRequestToConsumableStoreSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1615-department-request-to-consumable-store-slip", method = RequestMethod.GET)
    public void print1615DepartmentRequestToConsumableStoreSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1615DepartmentRequestToConsumableStoreSlip(response, objectMapper.convertValue(query, Rpt1615DepartmentRequestToConsumableStoreSlipRequest.class));
    }

    /**
     * Template: 212-InvRptGoodsReceiptsNotesStoreSlip.rpt
     * Procedure: Sp_InvGrn_StoreSlip_Rpt
     * Desktop: InvGrnandGdnReports.GrnSlipStore
     */
    @RequestMapping(value = "/reports/print/212-goods-receipts-notes-store-slip", method = RequestMethod.POST)
    public void print212GoodsReceiptsNotesStoreSlip(HttpServletResponse response, @RequestBody(required = false) Rpt212GoodsReceiptsNotesStoreSlipRequest request) throws Exception {
        if (request == null) request = new Rpt212GoodsReceiptsNotesStoreSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "212-InvRptGoodsReceiptsNotesStoreSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/212-goods-receipts-notes-store-slip", method = RequestMethod.GET)
    public void print212GoodsReceiptsNotesStoreSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print212GoodsReceiptsNotesStoreSlip(response, objectMapper.convertValue(query, Rpt212GoodsReceiptsNotesStoreSlipRequest.class));
    }

    /**
     * Template: 336-GrnStoreRegister.rpt
     * Procedure: USp_InvGrnStore_Register
     * Desktop: InvGrn.GrnRegisterStore
     */
    @RequestMapping(value = "/reports/print/336-grn-store-register", method = RequestMethod.POST)
    public void print336GrnStoreRegister(HttpServletResponse response, @RequestBody(required = false) Rpt336GrnStoreRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt336GrnStoreRegisterRequest();
        printReport(response, "336-GrnStoreRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/336-grn-store-register", method = RequestMethod.GET)
    public void print336GrnStoreRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print336GrnStoreRegister(response, objectMapper.convertValue(query, Rpt336GrnStoreRegisterRequest.class));
    }

    /**
     * Template: 454-InvPurchaseDemanSlip.rpt
     * Procedure: Sp_InvPurchaseDemand_Rpt
     * Desktop: InvPurchaseDemandHeader.InvPurchaseDemondSlip
     */
    @RequestMapping(value = "/reports/print/454-inv-purchase-deman-slip", method = RequestMethod.POST)
    public void print454InvPurchaseDemanSlip(HttpServletResponse response, @RequestBody(required = false) Rpt454InvPurchaseDemanSlipRequest request) throws Exception {
        if (request == null) request = new Rpt454InvPurchaseDemanSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "454-InvPurchaseDemanSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/454-inv-purchase-deman-slip", method = RequestMethod.GET)
    public void print454InvPurchaseDemanSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print454InvPurchaseDemanSlip(response, objectMapper.convertValue(query, Rpt454InvPurchaseDemanSlipRequest.class));
    }

    /**
     * Template: 145A-PurchaseInvoiceReturn_StorePartySlip.rpt
     * Procedure: [dbo].[USP_PurchaseInvoiceReturn_StoreSip]
     * Desktop: InvSaleInvoiceReports.PurchaseInvoiceReturn_StoreSip
     */
    @RequestMapping(value = "/reports/print/145a-purchase-invoice-return-store-party-slip", method = RequestMethod.POST)
    public void print145APurchaseInvoiceReturnStorePartySlip(HttpServletResponse response, @RequestBody(required = false) Rpt145APurchaseInvoiceReturnStorePartySlipRequest request) throws Exception {
        if (request == null) request = new Rpt145APurchaseInvoiceReturnStorePartySlipRequest();
        printReport(response, "145A-PurchaseInvoiceReturn_StorePartySlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/145a-purchase-invoice-return-store-party-slip", method = RequestMethod.GET)
    public void print145APurchaseInvoiceReturnStorePartySlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print145APurchaseInvoiceReturnStorePartySlip(response, objectMapper.convertValue(query, Rpt145APurchaseInvoiceReturnStorePartySlipRequest.class));
    }

    /**
     * Template: 145-PurchaseInvoiceReturn_StoreItemSlip.rpt
     * Procedure: [dbo].[USP_PurchaseInvoiceReturn_StoreSip]
     * Desktop: InvSaleInvoiceReports.PurchaseInvoiceReturn_StoreSip
     */
    @RequestMapping(value = "/reports/print/145-purchase-invoice-return-store-item-slip", method = RequestMethod.POST)
    public void print145PurchaseInvoiceReturnStoreItemSlip(HttpServletResponse response, @RequestBody(required = false) Rpt145PurchaseInvoiceReturnStoreItemSlipRequest request) throws Exception {
        if (request == null) request = new Rpt145PurchaseInvoiceReturnStoreItemSlipRequest();
        printReport(response, "145-PurchaseInvoiceReturn_StoreItemSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/145-purchase-invoice-return-store-item-slip", method = RequestMethod.GET)
    public void print145PurchaseInvoiceReturnStoreItemSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print145PurchaseInvoiceReturnStoreItemSlip(response, objectMapper.convertValue(query, Rpt145PurchaseInvoiceReturnStoreItemSlipRequest.class));
    }

    /**
     * Template: 147_PurchasePreBillSlip.rpt
     * Procedure: [dbo].[USP_PurchasePreBillHeader_Slip]
     * Desktop: PurchasePreBillHeader.PurchasePreBillHeader_Slip
     */
    @RequestMapping(value = "/reports/print/147-purchase-pre-bill-slip", method = RequestMethod.POST)
    public void print147PurchasePreBillSlip(HttpServletResponse response, @RequestBody(required = false) Rpt147PurchasePreBillSlipRequest request) throws Exception {
        if (request == null) request = new Rpt147PurchasePreBillSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "147_PurchasePreBillSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/147-purchase-pre-bill-slip", method = RequestMethod.GET)
    public void print147PurchasePreBillSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print147PurchasePreBillSlip(response, objectMapper.convertValue(query, Rpt147PurchasePreBillSlipRequest.class));
    }

    /**
     * Template: 409-InvStockAdjustmentSlip.rpt
     * Procedure: Sp_StockAdjustmentSlipAndRegister
     * Desktop: InvStockAdjustment.StockAdjustmentSlipAndRegister409
     */
    @RequestMapping(value = "/reports/print/409-inv-stock-adjustment-slip", method = RequestMethod.POST)
    public void print409InvStockAdjustmentSlip(HttpServletResponse response, @RequestBody(required = false) Rpt409InvStockAdjustmentSlipRequest request) throws Exception {
        if (request == null) request = new Rpt409InvStockAdjustmentSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "409-InvStockAdjustmentSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/409-inv-stock-adjustment-slip", method = RequestMethod.GET)
    public void print409InvStockAdjustmentSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print409InvStockAdjustmentSlip(response, objectMapper.convertValue(query, Rpt409InvStockAdjustmentSlipRequest.class));
    }

    /**
     * Template: 406-InvStockTransferSlip.rpt
     * Procedure: Sp_InvStockTransfer_SlipandRegister
     * Desktop: InvStockTransferHeader.StockTransferSlipandRegister
     */
    @RequestMapping(value = "/reports/print/406-inv-stock-transfer-slip", method = RequestMethod.POST)
    public void print406InvStockTransferSlip(HttpServletResponse response, @RequestBody(required = false) Rpt406InvStockTransferSlipRequest request) throws Exception {
        if (request == null) request = new Rpt406InvStockTransferSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "406-InvStockTransferSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/406-inv-stock-transfer-slip", method = RequestMethod.GET)
    public void print406InvStockTransferSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print406InvStockTransferSlip(response, objectMapper.convertValue(query, Rpt406InvStockTransferSlipRequest.class));
    }

    /**
     * Template: 407-InvStockTransferSlip.rpt
     * Procedure: Sp_InvStockTransfer_SlipandRegister
     * Desktop: InvStockTransferHeader.StockTransferSlipandRegister
     */
    @RequestMapping(value = "/reports/print/407-inv-stock-transfer-slip", method = RequestMethod.POST)
    public void print407InvStockTransferSlip(HttpServletResponse response, @RequestBody(required = false) Rpt407InvStockTransferSlipRequest request) throws Exception {
        if (request == null) request = new Rpt407InvStockTransferSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "407-InvStockTransferSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/407-inv-stock-transfer-slip", method = RequestMethod.GET)
    public void print407InvStockTransferSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print407InvStockTransferSlip(response, objectMapper.convertValue(query, Rpt407InvStockTransferSlipRequest.class));
    }

    /**
     * Template: 452-RptInvGsStoreIssuanceHeader_Slip.rpt
     * Procedure: Sp_InvGsStoreIssuanceHeader_SlipandRegister
     * Desktop: InvGsStoreIssuanceHeader.StoreIssuanceHistory
     */
    @RequestMapping(value = "/reports/print/452-inv-gs-store-issuance-header-slip", method = RequestMethod.POST)
    public void print452InvGsStoreIssuanceHeaderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt452InvGsStoreIssuanceHeaderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt452InvGsStoreIssuanceHeaderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "452-RptInvGsStoreIssuanceHeader_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/452-inv-gs-store-issuance-header-slip", method = RequestMethod.GET)
    public void print452InvGsStoreIssuanceHeaderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print452InvGsStoreIssuanceHeaderSlip(response, objectMapper.convertValue(query, Rpt452InvGsStoreIssuanceHeaderSlipRequest.class));
    }

    /**
     * Template: 1616-StoreIssuanceToConsumableStore_Slip.rpt
     * Procedure: Sp_InvGsStoreIssuanceHeader_SlipandRegister
     * Desktop: InvGsStoreIssuanceHeader.StoreIssuanceHistory
     */
    @RequestMapping(value = "/reports/print/1616-store-issuance-to-consumable-store-slip", method = RequestMethod.POST)
    public void print1616StoreIssuanceToConsumableStoreSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1616StoreIssuanceToConsumableStoreSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1616StoreIssuanceToConsumableStoreSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1616-StoreIssuanceToConsumableStore_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1616-store-issuance-to-consumable-store-slip", method = RequestMethod.GET)
    public void print1616StoreIssuanceToConsumableStoreSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1616StoreIssuanceToConsumableStoreSlip(response, objectMapper.convertValue(query, Rpt1616StoreIssuanceToConsumableStoreSlipRequest.class));
    }

    /**
     * Template: 457-StoreIssuanceReturnSlip.rpt
     * Procedure: Sp_InvStoreRetrun_SlipandRegister
     * Desktop: InvGsStoreIssuanceHeader.StoreIssuanceReturnRegister
     */
    @RequestMapping(value = "/reports/print/457-store-issuance-return-slip", method = RequestMethod.POST)
    public void print457StoreIssuanceReturnSlip(HttpServletResponse response, @RequestBody(required = false) Rpt457StoreIssuanceReturnSlipRequest request) throws Exception {
        if (request == null) request = new Rpt457StoreIssuanceReturnSlipRequest();
        printReport(response, "457-StoreIssuanceReturnSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/457-store-issuance-return-slip", method = RequestMethod.GET)
    public void print457StoreIssuanceReturnSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print457StoreIssuanceReturnSlip(response, objectMapper.convertValue(query, Rpt457StoreIssuanceReturnSlipRequest.class));
    }

    /**
     * Template: 458-StorePurchaseRegister.rpt
     * Procedure: USP_StorePurchaseRegister
     * Desktop: InvGsStoreIssuanceHeader.StorePurchaseRegister
     */
    @RequestMapping(value = "/reports/print/458-store-purchase-register", method = RequestMethod.POST)
    public void print458StorePurchaseRegister(HttpServletResponse response, @RequestBody(required = false) Rpt458StorePurchaseRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt458StorePurchaseRegisterRequest();
        printReport(response, "458-StorePurchaseRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/458-store-purchase-register", method = RequestMethod.GET)
    public void print458StorePurchaseRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print458StorePurchaseRegister(response, objectMapper.convertValue(query, Rpt458StorePurchaseRegisterRequest.class));
    }

    /**
     * Template: 144-InvGdn_Slip.rpt
     * Procedure: Sp_InvGdn_SlipAndRegisterRice_Rpt
     * Desktop: InvGrnandGdnReports.InvGdnSlip260
     */
    @RequestMapping(value = "/reports/print/144-inv-gdn-slip", method = RequestMethod.POST)
    public void print144InvGdnSlip(HttpServletResponse response, @RequestBody(required = false) Rpt144InvGdnSlipRequest request) throws Exception {
        if (request == null) request = new Rpt144InvGdnSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "144-InvGdn_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/144-inv-gdn-slip", method = RequestMethod.GET)
    public void print144InvGdnSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print144InvGdnSlip(response, objectMapper.convertValue(query, Rpt144InvGdnSlipRequest.class));
    }

    /**
     * Template: 149-InvGdnStorePmSlip.rpt
     * Procedure: Sp_InvGdn_SlipAndRegisterRice_Rpt
     * Desktop: InvGrnandGdnReports.InvGdnSlip260
     */
    @RequestMapping(value = "/reports/print/149-inv-gdn-store-pm-slip", method = RequestMethod.POST)
    public void print149InvGdnStorePmSlip(HttpServletResponse response, @RequestBody(required = false) Rpt149InvGdnStorePmSlipRequest request) throws Exception {
        if (request == null) request = new Rpt149InvGdnStorePmSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "149-InvGdnStorePmSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/149-inv-gdn-store-pm-slip", method = RequestMethod.GET)
    public void print149InvGdnStorePmSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print149InvGdnStorePmSlip(response, objectMapper.convertValue(query, Rpt149InvGdnStorePmSlipRequest.class));
    }

    /**
     * Template: 201-InvRptPurchaseOrderGeneralSlip.rpt
     * Procedure: Sp_PurchaseOrder_GeneralOrderSlip_Rpt
     * Desktop: PurchaseOrderReports.PurchaseOrderSlipReport201
     */
    @RequestMapping(value = "/reports/print/201-purchase-order-general-slip", method = RequestMethod.POST)
    public void print201PurchaseOrderGeneralSlip(HttpServletResponse response, @RequestBody(required = false) Rpt201PurchaseOrderGeneralSlipRequest request) throws Exception {
        if (request == null) request = new Rpt201PurchaseOrderGeneralSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "201-InvRptPurchaseOrderGeneralSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/201-purchase-order-general-slip", method = RequestMethod.GET)
    public void print201PurchaseOrderGeneralSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print201PurchaseOrderGeneralSlip(response, objectMapper.convertValue(query, Rpt201PurchaseOrderGeneralSlipRequest.class));
    }

    /**
     * Template: 202_01_ItemandWarehouseStockSummary.rpt
     * Procedure: Sp_ItemStockReportWithValues_Store
     * Desktop: StocksReport.ItemStockReportWithValues_Store
     */
    @RequestMapping(value = "/reports/print/202-01-itemand-warehouse-stock-summary", method = RequestMethod.POST)
    public void print20201ItemandWarehouseStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt20201ItemandWarehouseStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt20201ItemandWarehouseStockSummaryRequest();
        printReport(response, "202_01_ItemandWarehouseStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/202-01-itemand-warehouse-stock-summary", method = RequestMethod.GET)
    public void print20201ItemandWarehouseStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print20201ItemandWarehouseStockSummary(response, objectMapper.convertValue(query, Rpt20201ItemandWarehouseStockSummaryRequest.class));
    }

    /**
     * Template: 212_01_GoodsReceiptsNotesEmptyBagsSlip.rpt
     * Procedure: Sp_InvGrn_StoreSlip_Rpt
     * Desktop: InvGrnandGdnReports.GrnSlipStore
     */
    @RequestMapping(value = "/reports/print/212-01-goods-receipts-notes-empty-bags-slip", method = RequestMethod.POST)
    public void print21201GoodsReceiptsNotesEmptyBagsSlip(HttpServletResponse response, @RequestBody(required = false) Rpt21201GoodsReceiptsNotesEmptyBagsSlipRequest request) throws Exception {
        if (request == null) request = new Rpt21201GoodsReceiptsNotesEmptyBagsSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "212_01_GoodsReceiptsNotesEmptyBagsSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/212-01-goods-receipts-notes-empty-bags-slip", method = RequestMethod.GET)
    public void print21201GoodsReceiptsNotesEmptyBagsSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print21201GoodsReceiptsNotesEmptyBagsSlip(response, objectMapper.convertValue(query, Rpt21201GoodsReceiptsNotesEmptyBagsSlipRequest.class));
    }

    /**
     * Template: 239-SaleInvoice_StoreBillWithTax.rpt
     * Procedure: SP_InvSaleInvoice_CustomerBillRice OthersExp_SubRep
     * Desktop: InvSaleInvoiceReports.SalesCustomerBillSubReport
     */
    @RequestMapping(value = "/reports/print/239-sale-invoice-store-bill-with-tax", method = RequestMethod.POST)
    public void print239SaleInvoiceStoreBillWithTax(HttpServletResponse response, @RequestBody(required = false) Rpt239SaleInvoiceStoreBillWithTaxRequest request) throws Exception {
        if (request == null) request = new Rpt239SaleInvoiceStoreBillWithTaxRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "239-SaleInvoice_StoreBillWithTax.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/239-sale-invoice-store-bill-with-tax", method = RequestMethod.GET)
    public void print239SaleInvoiceStoreBillWithTaxGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print239SaleInvoiceStoreBillWithTax(response, objectMapper.convertValue(query, Rpt239SaleInvoiceStoreBillWithTaxRequest.class));
    }

    /**
     * Template: 401-StockEvalautionDetail_GenerateStocks_Register -Store.rpt
     * Procedure: Sp_InventoryStockEvalautionDetail_GenerateStocksStore
     * Desktop: InventoryStockEvalautionDetail.InventoryStockStoreGenrate
     */
    @RequestMapping(value = "/reports/print/401-stock-evalaution-detail-generate-stocks-register-store", method = RequestMethod.POST)
    public void print401StockEvalautionDetailGenerateStocksRegisterStore(HttpServletResponse response, @RequestBody(required = false) Rpt401StockEvalautionDetailGenerateStocksRegisterStoreRequest request) throws Exception {
        if (request == null) request = new Rpt401StockEvalautionDetailGenerateStocksRegisterStoreRequest();
        printReport(response, "401-StockEvalautionDetail_GenerateStocks_Register -Store.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/401-stock-evalaution-detail-generate-stocks-register-store", method = RequestMethod.GET)
    public void print401StockEvalautionDetailGenerateStocksRegisterStoreGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print401StockEvalautionDetailGenerateStocksRegisterStore(response, objectMapper.convertValue(query, Rpt401StockEvalautionDetailGenerateStocksRegisterStoreRequest.class));
    }

    /**
     * Template: 402-InvStockRptInventoryTransactionsA-Store.rpt
     * Procedure: Sp_InventoryTransactions_GenerateStocksReport_Store_Rpt
     * Desktop: InventoryStockEvalautionDetail.TransactionStockGenrateStore
     */
    @RequestMapping(value = "/reports/print/402-inv-stock-inventory-transactions-a-store", method = RequestMethod.POST)
    public void print402InvStockInventoryTransactionsAStore(HttpServletResponse response, @RequestBody(required = false) Rpt402InvStockInventoryTransactionsAStoreRequest request) throws Exception {
        if (request == null) request = new Rpt402InvStockInventoryTransactionsAStoreRequest();
        printReport(response, "402-InvStockRptInventoryTransactionsA-Store.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/402-inv-stock-inventory-transactions-a-store", method = RequestMethod.GET)
    public void print402InvStockInventoryTransactionsAStoreGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print402InvStockInventoryTransactionsAStore(response, objectMapper.convertValue(query, Rpt402InvStockInventoryTransactionsAStoreRequest.class));
    }

    /**
     * Template: 403-InvStockRptInventoryTransactionsStocks -Store.rpt
     * Procedure: Sp_InventoryTransactions_GenerateStocksReport_Store_Rpt
     * Desktop: InventoryStockEvalautionDetail.TransactionStockGenrateStore
     */
    @RequestMapping(value = "/reports/print/403-inv-stock-inventory-transactions-stocks-store", method = RequestMethod.POST)
    public void print403InvStockInventoryTransactionsStocksStore(HttpServletResponse response, @RequestBody(required = false) Rpt403InvStockInventoryTransactionsStocksStoreRequest request) throws Exception {
        if (request == null) request = new Rpt403InvStockInventoryTransactionsStocksStoreRequest();
        printReport(response, "403-InvStockRptInventoryTransactionsStocks -Store.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/403-inv-stock-inventory-transactions-stocks-store", method = RequestMethod.GET)
    public void print403InvStockInventoryTransactionsStocksStoreGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print403InvStockInventoryTransactionsStocksStore(response, objectMapper.convertValue(query, Rpt403InvStockInventoryTransactionsStocksStoreRequest.class));
    }

    /**
     * Template: 422_01_StockReportWithoutValueDocumentWiseStore.rpt
     * Procedure: USP_StockReport_WithoutValue_DocumentWiseStore
     * Desktop: InventoryStockEvalautionDetail.StockReportWithoutValueDocumentWiseStore
     */
    @RequestMapping(value = "/reports/print/422-01-stock-report-without-value-document-wise-store", method = RequestMethod.POST)
    public void print42201StockReportWithoutValueDocumentWiseStore(HttpServletResponse response, @RequestBody(required = false) Rpt42201StockReportWithoutValueDocumentWiseStoreRequest request) throws Exception {
        if (request == null) request = new Rpt42201StockReportWithoutValueDocumentWiseStoreRequest();
        printReport(response, "422_01_StockReportWithoutValueDocumentWiseStore.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/422-01-stock-report-without-value-document-wise-store", method = RequestMethod.GET)
    public void print42201StockReportWithoutValueDocumentWiseStoreGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print42201StockReportWithoutValueDocumentWiseStore(response, objectMapper.convertValue(query, Rpt42201StockReportWithoutValueDocumentWiseStoreRequest.class));
    }

    /**
     * Template: 422_InventoryEvaluationLedgerStore.rpt
     * Procedure: USP_InventoryEvaluationLedgerStore
     * Desktop: InventoryStockEvalautionDetail.InventoryEvaluationLedgerStore
     */
    @RequestMapping(value = "/reports/print/422-inventory-evaluation-ledger-store", method = RequestMethod.POST)
    public void print422InventoryEvaluationLedgerStore(HttpServletResponse response, @RequestBody(required = false) Rpt422InventoryEvaluationLedgerStoreRequest request) throws Exception {
        if (request == null) request = new Rpt422InventoryEvaluationLedgerStoreRequest();
        printReport(response, "422_InventoryEvaluationLedgerStore.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/422-inventory-evaluation-ledger-store", method = RequestMethod.GET)
    public void print422InventoryEvaluationLedgerStoreGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print422InventoryEvaluationLedgerStore(response, objectMapper.convertValue(query, Rpt422InventoryEvaluationLedgerStoreRequest.class));
    }

    /**
     * Template: 450-RptDepartmentRequestRegister.rpt
     * Procedure: Sp_DepartmentRequest_History
     * Desktop: DepartmentRequest.DepartmentRequestHistory
     */
    @RequestMapping(value = "/reports/print/450-department-request-register", method = RequestMethod.POST)
    public void print450DepartmentRequestRegister(HttpServletResponse response, @RequestBody(required = false) Rpt450DepartmentRequestRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt450DepartmentRequestRegisterRequest();
        printReport(response, "450-RptDepartmentRequestRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/450-department-request-register", method = RequestMethod.GET)
    public void print450DepartmentRequestRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print450DepartmentRequestRegister(response, objectMapper.convertValue(query, Rpt450DepartmentRequestRegisterRequest.class));
    }

    /**
     * Template: 456-PurchaseDemandRegister.rpt
     * Procedure: USP_StorePurchaseDemandRegister
     * Desktop: InvPurchaseDemandHeader.InvStorePurchaseDemandRegister
     */
    @RequestMapping(value = "/reports/print/456-purchase-demand-register", method = RequestMethod.POST)
    public void print456PurchaseDemandRegister(HttpServletResponse response, @RequestBody(required = false) Rpt456PurchaseDemandRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt456PurchaseDemandRegisterRequest();
        printReport(response, "456-PurchaseDemandRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/456-purchase-demand-register", method = RequestMethod.GET)
    public void print456PurchaseDemandRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print456PurchaseDemandRegister(response, objectMapper.convertValue(query, Rpt456PurchaseDemandRegisterRequest.class));
    }

    /**
     * Template: 465-StoreSendReceipt_Slip.rpt
     * Procedure: USP_StoreSendReceipt_Slip
     * Desktop: InvStoreSendReceipt.StoreSendReceipt_Slip
     */
    @RequestMapping(value = "/reports/print/465-store-send-receipt-slip", method = RequestMethod.POST)
    public void print465StoreSendReceiptSlip(HttpServletResponse response, @RequestBody(required = false) Rpt465StoreSendReceiptSlipRequest request) throws Exception {
        if (request == null) request = new Rpt465StoreSendReceiptSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "465-StoreSendReceipt_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/465-store-send-receipt-slip", method = RequestMethod.GET)
    public void print465StoreSendReceiptSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print465StoreSendReceiptSlip(response, objectMapper.convertValue(query, Rpt465StoreSendReceiptSlipRequest.class));
    }

    /**
     * Template: 475-RptInvGsStoreIssuanceHeader_Slip.rpt
     * Procedure: Sp_InvGsStoreIssuanceHeader_SlipandRegister
     * Desktop: InvGsStoreIssuanceHeader.StoreIssuanceHistory
     */
    @RequestMapping(value = "/reports/print/475-inv-gs-store-issuance-header-slip", method = RequestMethod.POST)
    public void print475InvGsStoreIssuanceHeaderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt475InvGsStoreIssuanceHeaderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt475InvGsStoreIssuanceHeaderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "475-RptInvGsStoreIssuanceHeader_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/475-inv-gs-store-issuance-header-slip", method = RequestMethod.GET)
    public void print475InvGsStoreIssuanceHeaderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print475InvGsStoreIssuanceHeaderSlip(response, objectMapper.convertValue(query, Rpt475InvGsStoreIssuanceHeaderSlipRequest.class));
    }

    /**
     * Template: 90-SaleOrderStoreAndPmSlip.rpt
     * Procedure: Sp_SaleOrder_RiceSlip_Rpt
     * Desktop: SaleOrderReports.SaleOrderReports273
     */
    @RequestMapping(value = "/reports/print/90-sale-order-store-and-pm-slip", method = RequestMethod.POST)
    public void print90SaleOrderStoreAndPmSlip(HttpServletResponse response, @RequestBody(required = false) Rpt90SaleOrderStoreAndPmSlipRequest request) throws Exception {
        if (request == null) request = new Rpt90SaleOrderStoreAndPmSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "90-SaleOrderStoreAndPmSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/90-sale-order-store-and-pm-slip", method = RequestMethod.GET)
    public void print90SaleOrderStoreAndPmSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print90SaleOrderStoreAndPmSlip(response, objectMapper.convertValue(query, Rpt90SaleOrderStoreAndPmSlipRequest.class));
    }

    /**
     * Template: DeliveryChallanHeader_ExpenseSubReport.rpt
     * Procedure: [dbo].[USP_DeliveryChallan_ExpenseSubReport]
     * Desktop: DeliveryChallanHeader.DeliveryChallanHeader_ExpenseSubReport
     */
    @RequestMapping(value = "/reports/print/delivery-challan-header-expense-sub-report", method = RequestMethod.POST)
    public void printDeliveryChallanHeaderExpenseSubReport(HttpServletResponse response, @RequestBody(required = false) RptDeliveryChallanHeaderExpenseSubReportRequest request) throws Exception {
        if (request == null) request = new RptDeliveryChallanHeaderExpenseSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "DeliveryChallanHeader_ExpenseSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/delivery-challan-header-expense-sub-report", method = RequestMethod.GET)
    public void printDeliveryChallanHeaderExpenseSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printDeliveryChallanHeaderExpenseSubReport(response, objectMapper.convertValue(query, RptDeliveryChallanHeaderExpenseSubReportRequest.class));
    }

    /**
     * Template: PurchasePreBillHeader_ExpenseSubReport.rpt
     * Procedure: [dbo].[USP_PurchasePreBillHeader_ExpenseSubReport]
     * Desktop: PurchasePreBillHeader.PurchasePreBillHeader_ExpenseSubReport
     */
    @RequestMapping(value = "/reports/print/purchase-pre-bill-header-expense-sub-report", method = RequestMethod.POST)
    public void printPurchasePreBillHeaderExpenseSubReport(HttpServletResponse response, @RequestBody(required = false) RptPurchasePreBillHeaderExpenseSubReportRequest request) throws Exception {
        if (request == null) request = new RptPurchasePreBillHeaderExpenseSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "PurchasePreBillHeader_ExpenseSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/purchase-pre-bill-header-expense-sub-report", method = RequestMethod.GET)
    public void printPurchasePreBillHeaderExpenseSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printPurchasePreBillHeaderExpenseSubReport(response, objectMapper.convertValue(query, RptPurchasePreBillHeaderExpenseSubReportRequest.class));
    }

    /**
     * Template: StoreIssuanceSubReport.rpt
     * Procedure: Sp_InvGsStoreIssuanceHeader_SlipandRegister
     * Desktop: InvGsStoreIssuanceHeader.StoreIssuanceHistory
     */
    @RequestMapping(value = "/reports/print/store-issuance-sub-report", method = RequestMethod.POST)
    public void printStoreIssuanceSubReport(HttpServletResponse response, @RequestBody(required = false) RptStoreIssuanceSubReportRequest request) throws Exception {
        if (request == null) request = new RptStoreIssuanceSubReportRequest();
        printReport(response, "StoreIssuanceSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/store-issuance-sub-report", method = RequestMethod.GET)
    public void printStoreIssuanceSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printStoreIssuanceSubReport(response, objectMapper.convertValue(query, RptStoreIssuanceSubReportRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
