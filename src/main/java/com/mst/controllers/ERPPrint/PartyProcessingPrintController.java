package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.PartyProcessingPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** PartyProcessing print actions. Generated from the verified seeder contracts. */
@Controller
public class PartyProcessingPrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 120_01_JobOrderPartyProcessingRegister.rpt
     * Procedure: [dbo].[USP_ProductionJobOrderPartyProcessing_SlipandRegister]
     * Desktop: InvProductionJobOrderPartyProcessing.ProductionJobOrderPartyProcessing_SlipandRegister
     */
    @RequestMapping(value = "/reports/print/120-01-job-order-party-processing-register", method = RequestMethod.POST)
    public void print12001JobOrderPartyProcessingRegister(HttpServletResponse response, @RequestBody(required = false) Rpt12001JobOrderPartyProcessingRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt12001JobOrderPartyProcessingRegisterRequest();
        printReport(response, "120_01_JobOrderPartyProcessingRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/120-01-job-order-party-processing-register", method = RequestMethod.GET)
    public void print12001JobOrderPartyProcessingRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print12001JobOrderPartyProcessingRegister(response, objectMapper.convertValue(query, Rpt12001JobOrderPartyProcessingRegisterRequest.class));
    }

    /**
     * Template: 123_01_PartyProcessingBillRegister.rpt
     * Procedure: [dbo].[USP_ProductionProcessingBill_Register]
     * Desktop: InvProductionProcessingBill.ProductionProcessingBill_Register
     */
    @RequestMapping(value = "/reports/print/123-01-party-processing-bill-register", method = RequestMethod.POST)
    public void print12301PartyProcessingBillRegister(HttpServletResponse response, @RequestBody(required = false) Rpt12301PartyProcessingBillRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt12301PartyProcessingBillRegisterRequest();
        printReport(response, "123_01_PartyProcessingBillRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/123-01-party-processing-bill-register", method = RequestMethod.GET)
    public void print12301PartyProcessingBillRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print12301PartyProcessingBillRegister(response, objectMapper.convertValue(query, Rpt12301PartyProcessingBillRegisterRequest.class));
    }

    /**
     * Template: 158-InvStockAdjustmentPartyProcessingSlip.rpt
     * Procedure: Sp_StockAdjustmentPartyProcessingSlipAndRegister
     * Desktop: InvStockAdjustmentPartyProcessing.StockAdjustmentSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/158-inv-stock-adjustment-party-processing-slip", method = RequestMethod.POST)
    public void print158InvStockAdjustmentPartyProcessingSlip(HttpServletResponse response, @RequestBody(required = false) Rpt158InvStockAdjustmentPartyProcessingSlipRequest request) throws Exception {
        if (request == null) request = new Rpt158InvStockAdjustmentPartyProcessingSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "158-InvStockAdjustmentPartyProcessingSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/158-inv-stock-adjustment-party-processing-slip", method = RequestMethod.GET)
    public void print158InvStockAdjustmentPartyProcessingSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print158InvStockAdjustmentPartyProcessingSlip(response, objectMapper.convertValue(query, Rpt158InvStockAdjustmentPartyProcessingSlipRequest.class));
    }

    /**
     * Template: 158_01_PartyProcessingStockAdjustmentRegister.rpt
     * Procedure: Sp_StockAdjustmentPartyProcessingSlipAndRegister
     * Desktop: InvStockAdjustmentPartyProcessing.StockAdjustmentSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/158-01-party-processing-stock-adjustment-register", method = RequestMethod.POST)
    public void print15801PartyProcessingStockAdjustmentRegister(HttpServletResponse response, @RequestBody(required = false) Rpt15801PartyProcessingStockAdjustmentRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt15801PartyProcessingStockAdjustmentRegisterRequest();
        printReport(response, "158_01_PartyProcessingStockAdjustmentRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/158-01-party-processing-stock-adjustment-register", method = RequestMethod.GET)
    public void print15801PartyProcessingStockAdjustmentRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print15801PartyProcessingStockAdjustmentRegister(response, objectMapper.convertValue(query, Rpt15801PartyProcessingStockAdjustmentRegisterRequest.class));
    }

    /**
     * Template: 169-InvPartyProcessingGrnInfo.rpt
     * Procedure: Sp_InvPartyProcessingGrn_Info
     * Desktop: GrnGdnReports.PartyProcessingGrnInfo
     */
    @RequestMapping(value = "/reports/print/169-inv-party-processing-grn-info", method = RequestMethod.POST)
    public void print169InvPartyProcessingGrnInfo(HttpServletResponse response, @RequestBody(required = false) Rpt169InvPartyProcessingGrnInfoRequest request) throws Exception {
        if (request == null) request = new Rpt169InvPartyProcessingGrnInfoRequest();
        printReport(response, "169-InvPartyProcessingGrnInfo.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/169-inv-party-processing-grn-info", method = RequestMethod.GET)
    public void print169InvPartyProcessingGrnInfoGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print169InvPartyProcessingGrnInfo(response, objectMapper.convertValue(query, Rpt169InvPartyProcessingGrnInfoRequest.class));
    }

    /**
     * Template: 176_01_StockConversionRegisterWithActivityPartyProcessing.rpt
     * Procedure: [dbo].[USP_StockConversionPartyProcessingRegisterWithActivity]
     * Desktop: StockConversionPartyProcessing.ProductionRegisterPartyProcessing
     */
    @RequestMapping(value = "/reports/print/176-01-stock-conversion-register-with-activity-party-processing", method = RequestMethod.POST)
    public void print17601StockConversionRegisterWithActivityPartyProcessing(HttpServletResponse response, @RequestBody(required = false) Rpt17601StockConversionRegisterWithActivityPartyProcessingRequest request) throws Exception {
        if (request == null) request = new Rpt17601StockConversionRegisterWithActivityPartyProcessingRequest();
        printReport(response, "176_01_StockConversionRegisterWithActivityPartyProcessing.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/176-01-stock-conversion-register-with-activity-party-processing", method = RequestMethod.GET)
    public void print17601StockConversionRegisterWithActivityPartyProcessingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print17601StockConversionRegisterWithActivityPartyProcessing(response, objectMapper.convertValue(query, Rpt17601StockConversionRegisterWithActivityPartyProcessingRequest.class));
    }

    /**
     * Template: 176_02_StockConversionRegisterWithActivityPartyProcessing.rpt
     * Procedure: [dbo].[USP_StockConversionPartyProcessingRegisterWithActivity]
     * Desktop: StockConversionPartyProcessing.ProductionRegisterPartyProcessing
     */
    @RequestMapping(value = "/reports/print/176-02-stock-conversion-register-with-activity-party-processing", method = RequestMethod.POST)
    public void print17602StockConversionRegisterWithActivityPartyProcessing(HttpServletResponse response, @RequestBody(required = false) Rpt17602StockConversionRegisterWithActivityPartyProcessingRequest request) throws Exception {
        if (request == null) request = new Rpt17602StockConversionRegisterWithActivityPartyProcessingRequest();
        printReport(response, "176_02_StockConversionRegisterWithActivityPartyProcessing.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/176-02-stock-conversion-register-with-activity-party-processing", method = RequestMethod.GET)
    public void print17602StockConversionRegisterWithActivityPartyProcessingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print17602StockConversionRegisterWithActivityPartyProcessing(response, objectMapper.convertValue(query, Rpt17602StockConversionRegisterWithActivityPartyProcessingRequest.class));
    }

    /**
     * Template: 217_01_GrnPurchaseFromPPRegister.rpt
     * Procedure: USP_GrnPurchaseFromPartyProcessing_Register
     * Desktop: InvGrnPartyProcessing.GrnPurchaseFromPartyProcessing_Register
     */
    @RequestMapping(value = "/reports/print/217-01-grn-purchase-from-pp-register", method = RequestMethod.POST)
    public void print21701GrnPurchaseFromPPRegister(HttpServletResponse response, @RequestBody(required = false) Rpt21701GrnPurchaseFromPPRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt21701GrnPurchaseFromPPRegisterRequest();
        printReport(response, "217_01_GrnPurchaseFromPPRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/217-01-grn-purchase-from-pp-register", method = RequestMethod.GET)
    public void print21701GrnPurchaseFromPPRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print21701GrnPurchaseFromPPRegister(response, objectMapper.convertValue(query, Rpt21701GrnPurchaseFromPPRegisterRequest.class));
    }

    /**
     * Template: 217_GrnPurchaseFromPPRegister.rpt
     * Procedure: USP_GrnPurchaseFromPartyProcessing_Register
     * Desktop: InvGrnPartyProcessing.GrnPurchaseFromPartyProcessing_Register
     */
    @RequestMapping(value = "/reports/print/217-grn-purchase-from-pp-register", method = RequestMethod.POST)
    public void print217GrnPurchaseFromPPRegister(HttpServletResponse response, @RequestBody(required = false) Rpt217GrnPurchaseFromPPRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt217GrnPurchaseFromPPRegisterRequest();
        printReport(response, "217_GrnPurchaseFromPPRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/217-grn-purchase-from-pp-register", method = RequestMethod.GET)
    public void print217GrnPurchaseFromPPRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print217GrnPurchaseFromPPRegister(response, objectMapper.convertValue(query, Rpt217GrnPurchaseFromPPRegisterRequest.class));
    }

    /**
     * Template: 220_StockTransferPartyProcessingSlip.rpt
     * Procedure: Sp_InvStockTransferPartyProcessing_SlipandRegister
     * Desktop: InvStockTransferPartyProcessing.StockTransferPartyProcessingSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/220-stock-transfer-party-processing-slip", method = RequestMethod.POST)
    public void print220StockTransferPartyProcessingSlip(HttpServletResponse response, @RequestBody(required = false) Rpt220StockTransferPartyProcessingSlipRequest request) throws Exception {
        if (request == null) request = new Rpt220StockTransferPartyProcessingSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "220_StockTransferPartyProcessingSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/220-stock-transfer-party-processing-slip", method = RequestMethod.GET)
    public void print220StockTransferPartyProcessingSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print220StockTransferPartyProcessingSlip(response, objectMapper.convertValue(query, Rpt220StockTransferPartyProcessingSlipRequest.class));
    }

    /**
     * Template: 221_01_GdnSaleToPartyProcessingRegister.rpt
     * Procedure: [dbo].[USP_GdnSaleToPartyProcessing_Register]
     * Desktop: InvGdn.GdnSaleToPartyProcessingRegister
     */
    @RequestMapping(value = "/reports/print/221-01-gdn-sale-to-party-processing-register", method = RequestMethod.POST)
    public void print22101GdnSaleToPartyProcessingRegister(HttpServletResponse response, @RequestBody(required = false) Rpt22101GdnSaleToPartyProcessingRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt22101GdnSaleToPartyProcessingRegisterRequest();
        printReport(response, "221_01_GdnSaleToPartyProcessingRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/221-01-gdn-sale-to-party-processing-register", method = RequestMethod.GET)
    public void print22101GdnSaleToPartyProcessingRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print22101GdnSaleToPartyProcessingRegister(response, objectMapper.convertValue(query, Rpt22101GdnSaleToPartyProcessingRegisterRequest.class));
    }

    /**
     * Template: 299-InvRptPartyProcessingInwardGatePassSlip.rpt
     * Procedure: [dbo].[Sp_GatePassPartyProcessing_SlipAndRegister_Rpt]
     * Desktop: PartyProcessingGatePassReports.PartyProcessingGatePassSlipandRegister
     */
    @RequestMapping(value = "/reports/print/299-party-processing-inward-gate-pass-slip", method = RequestMethod.POST)
    public void print299PartyProcessingInwardGatePassSlip(HttpServletResponse response, @RequestBody(required = false) Rpt299PartyProcessingInwardGatePassSlipRequest request) throws Exception {
        if (request == null) request = new Rpt299PartyProcessingInwardGatePassSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "299-InvRptPartyProcessingInwardGatePassSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/299-party-processing-inward-gate-pass-slip", method = RequestMethod.GET)
    public void print299PartyProcessingInwardGatePassSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print299PartyProcessingInwardGatePassSlip(response, objectMapper.convertValue(query, Rpt299PartyProcessingInwardGatePassSlipRequest.class));
    }

    /**
     * Template: 299-InvRptPartyProcessingOutwardGatePassSlip.rpt
     * Procedure: [dbo].[Sp_GatePassPartyProcessing_SlipAndRegister_Rpt]
     * Desktop: PartyProcessingGatePassReports.PartyProcessingGatePassSlipandRegister
     */
    @RequestMapping(value = "/reports/print/299-party-processing-outward-gate-pass-slip", method = RequestMethod.POST)
    public void print299PartyProcessingOutwardGatePassSlip(HttpServletResponse response, @RequestBody(required = false) Rpt299PartyProcessingOutwardGatePassSlipRequest request) throws Exception {
        if (request == null) request = new Rpt299PartyProcessingOutwardGatePassSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "299-InvRptPartyProcessingOutwardGatePassSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/299-party-processing-outward-gate-pass-slip", method = RequestMethod.GET)
    public void print299PartyProcessingOutwardGatePassSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print299PartyProcessingOutwardGatePassSlip(response, objectMapper.convertValue(query, Rpt299PartyProcessingOutwardGatePassSlipRequest.class));
    }

    /**
     * Template: 300-RptPartyProcessingGatePassRegister.rpt
     * Procedure: [dbo].[Sp_GatePassPartyProcessing_SlipAndRegister_Rpt]
     * Desktop: PartyProcessingGatePassReports.PartyProcessingGatePassSlipandRegister
     */
    @RequestMapping(value = "/reports/print/300-party-processing-gate-pass-register", method = RequestMethod.POST)
    public void print300PartyProcessingGatePassRegister(HttpServletResponse response, @RequestBody(required = false) Rpt300PartyProcessingGatePassRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt300PartyProcessingGatePassRegisterRequest();
        printReport(response, "300-RptPartyProcessingGatePassRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/300-party-processing-gate-pass-register", method = RequestMethod.GET)
    public void print300PartyProcessingGatePassRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print300PartyProcessingGatePassRegister(response, objectMapper.convertValue(query, Rpt300PartyProcessingGatePassRegisterRequest.class));
    }

    /**
     * Template: 332-PartyProcessingGRNSlip.rpt
     * Procedure: Sp_InvGrnGdnStorePartyProcessing_SlipandRegister
     * Desktop: PartyProcessingGatePassReports.PartyProcessingGrnGdnStoreSlipandRegister
     */
    @RequestMapping(value = "/reports/print/332-party-processing-grn-slip", method = RequestMethod.POST)
    public void print332PartyProcessingGRNSlip(HttpServletResponse response, @RequestBody(required = false) Rpt332PartyProcessingGRNSlipRequest request) throws Exception {
        if (request == null) request = new Rpt332PartyProcessingGRNSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "332-PartyProcessingGRNSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/332-party-processing-grn-slip", method = RequestMethod.GET)
    public void print332PartyProcessingGRNSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print332PartyProcessingGRNSlip(response, objectMapper.convertValue(query, Rpt332PartyProcessingGRNSlipRequest.class));
    }

    /**
     * Template: 332_01-PartyProcessingGDNSlip.rpt
     * Procedure: Sp_InvGrnGdnStorePartyProcessing_SlipandRegister
     * Desktop: PartyProcessingGatePassReports.PartyProcessingGrnGdnStoreSlipandRegister
     */
    @RequestMapping(value = "/reports/print/332-01-party-processing-gdn-slip", method = RequestMethod.POST)
    public void print33201PartyProcessingGDNSlip(HttpServletResponse response, @RequestBody(required = false) Rpt33201PartyProcessingGDNSlipRequest request) throws Exception {
        if (request == null) request = new Rpt33201PartyProcessingGDNSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "332_01-PartyProcessingGDNSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/332-01-party-processing-gdn-slip", method = RequestMethod.GET)
    public void print33201PartyProcessingGDNSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print33201PartyProcessingGDNSlip(response, objectMapper.convertValue(query, Rpt33201PartyProcessingGDNSlipRequest.class));
    }

    /**
     * Template: 333-InvRptGoodsReceiptsNotesRiceSlip.rpt
     * Procedure: [dbo].[Sp_InvGrnPartyProcessing_RiceSlip_Rpt]
     * Desktop: GrnGdnReports.GrnAndGdnSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/333-goods-receipts-notes-rice-slip", method = RequestMethod.POST)
    public void print333GoodsReceiptsNotesRiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt333GoodsReceiptsNotesRiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt333GoodsReceiptsNotesRiceSlipRequest();
        printReport(response, "333-InvRptGoodsReceiptsNotesRiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/333-goods-receipts-notes-rice-slip", method = RequestMethod.GET)
    public void print333GoodsReceiptsNotesRiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print333GoodsReceiptsNotesRiceSlip(response, objectMapper.convertValue(query, Rpt333GoodsReceiptsNotesRiceSlipRequest.class));
    }

    /**
     * Template: 339-InvRptGoodsReceiptsNotesRiceSlipGDN.rpt
     * Procedure: [dbo].[Sp_InvGrnPartyProcessing_RiceSlip_Rpt]
     * Desktop: GrnGdnReports.GrnAndGdnSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/339-goods-receipts-notes-rice-slip-gdn", method = RequestMethod.POST)
    public void print339GoodsReceiptsNotesRiceSlipGDN(HttpServletResponse response, @RequestBody(required = false) Rpt339GoodsReceiptsNotesRiceSlipGDNRequest request) throws Exception {
        if (request == null) request = new Rpt339GoodsReceiptsNotesRiceSlipGDNRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "339-InvRptGoodsReceiptsNotesRiceSlipGDN.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/339-goods-receipts-notes-rice-slip-gdn", method = RequestMethod.GET)
    public void print339GoodsReceiptsNotesRiceSlipGDNGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print339GoodsReceiptsNotesRiceSlipGDN(response, objectMapper.convertValue(query, Rpt339GoodsReceiptsNotesRiceSlipGDNRequest.class));
    }

    /**
     * Template: 414-InvStockRptInventoryTransactions.rpt
     * Procedure: [dbo].[USP_InventoryTransactions_PartyProcessing]
     * Desktop: StocksReport.InventoryTransactions_PartyProcessingRegister
     */
    @RequestMapping(value = "/reports/print/414-inv-stock-inventory-transactions", method = RequestMethod.POST)
    public void print414InvStockInventoryTransactions(HttpServletResponse response, @RequestBody(required = false) Rpt414InvStockInventoryTransactionsRequest request) throws Exception {
        if (request == null) request = new Rpt414InvStockInventoryTransactionsRequest();
        printReport(response, "414-InvStockRptInventoryTransactions.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/414-inv-stock-inventory-transactions", method = RequestMethod.GET)
    public void print414InvStockInventoryTransactionsGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print414InvStockInventoryTransactions(response, objectMapper.convertValue(query, Rpt414InvStockInventoryTransactionsRequest.class));
    }

    /**
     * Template: 416_01-InvStockOpeningBalancePartyProcessing_Slip.rpt
     * Procedure: [dbo].[Sp_InvStockOpeningBalancePartyProcessing_SlipandRegister]
     * Desktop: InvStockOpeningBalancePartyProcessing.StockOpeningBalancePartyProcessing_SlipandRegister
     */
    @RequestMapping(value = "/reports/print/416-01-inv-stock-opening-balance-party-processing-slip", method = RequestMethod.POST)
    public void print41601InvStockOpeningBalancePartyProcessingSlip(HttpServletResponse response, @RequestBody(required = false) Rpt41601InvStockOpeningBalancePartyProcessingSlipRequest request) throws Exception {
        if (request == null) request = new Rpt41601InvStockOpeningBalancePartyProcessingSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "416_01-InvStockOpeningBalancePartyProcessing_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/416-01-inv-stock-opening-balance-party-processing-slip", method = RequestMethod.GET)
    public void print41601InvStockOpeningBalancePartyProcessingSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print41601InvStockOpeningBalancePartyProcessingSlip(response, objectMapper.convertValue(query, Rpt41601InvStockOpeningBalancePartyProcessingSlipRequest.class));
    }

    /**
     * Template: 466_01-ItemStockSummaryPartyProcessing.rpt
     * Procedure: Sp_InventoryTransactionsPartyProcessing_StockSummaryGeneralByWeight_Rpt
     * Desktop: StocksReport.PartyProcessingstockGeneralSummaryByWeight
     */
    @RequestMapping(value = "/reports/print/466-01-item-stock-summary-party-processing", method = RequestMethod.POST)
    public void print46601ItemStockSummaryPartyProcessing(HttpServletResponse response, @RequestBody(required = false) Rpt46601ItemStockSummaryPartyProcessingRequest request) throws Exception {
        if (request == null) request = new Rpt46601ItemStockSummaryPartyProcessingRequest();
        printReport(response, "466_01-ItemStockSummaryPartyProcessing.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/466-01-item-stock-summary-party-processing", method = RequestMethod.GET)
    public void print46601ItemStockSummaryPartyProcessingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print46601ItemStockSummaryPartyProcessing(response, objectMapper.convertValue(query, Rpt46601ItemStockSummaryPartyProcessingRequest.class));
    }

    /**
     * Template: 466_02-ItemandWarehouseStockSummaryPartyProcessing.rpt
     * Procedure: Sp_InventoryTransactionsPartyProcessing_StockSummaryGeneralByWeight_Rpt
     * Desktop: StocksReport.PartyProcessingstockGeneralSummaryByWeight
     */
    @RequestMapping(value = "/reports/print/466-02-itemand-warehouse-stock-summary-party-processing", method = RequestMethod.POST)
    public void print46602ItemandWarehouseStockSummaryPartyProcessing(HttpServletResponse response, @RequestBody(required = false) Rpt46602ItemandWarehouseStockSummaryPartyProcessingRequest request) throws Exception {
        if (request == null) request = new Rpt46602ItemandWarehouseStockSummaryPartyProcessingRequest();
        printReport(response, "466_02-ItemandWarehouseStockSummaryPartyProcessing.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/466-02-itemand-warehouse-stock-summary-party-processing", method = RequestMethod.GET)
    public void print46602ItemandWarehouseStockSummaryPartyProcessingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print46602ItemandWarehouseStockSummaryPartyProcessing(response, objectMapper.convertValue(query, Rpt46602ItemandWarehouseStockSummaryPartyProcessingRequest.class));
    }

    /**
     * Template: 466_03-ItemandCropYearStockSummaryPartyProcessing.rpt
     * Procedure: Sp_InventoryTransactionsPartyProcessing_StockSummaryGeneralByWeight_Rpt
     * Desktop: StocksReport.PartyProcessingstockGeneralSummaryByWeight
     */
    @RequestMapping(value = "/reports/print/466-03-itemand-crop-year-stock-summary-party-processing", method = RequestMethod.POST)
    public void print46603ItemandCropYearStockSummaryPartyProcessing(HttpServletResponse response, @RequestBody(required = false) Rpt46603ItemandCropYearStockSummaryPartyProcessingRequest request) throws Exception {
        if (request == null) request = new Rpt46603ItemandCropYearStockSummaryPartyProcessingRequest();
        printReport(response, "466_03-ItemandCropYearStockSummaryPartyProcessing.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/466-03-itemand-crop-year-stock-summary-party-processing", method = RequestMethod.GET)
    public void print46603ItemandCropYearStockSummaryPartyProcessingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print46603ItemandCropYearStockSummaryPartyProcessing(response, objectMapper.convertValue(query, Rpt46603ItemandCropYearStockSummaryPartyProcessingRequest.class));
    }

    /**
     * Template: 466_04-JobLotandItemStockSummaryPartyProcessing.rpt
     * Procedure: Sp_InventoryTransactionsPartyProcessing_StockSummaryGeneralByWeight_Rpt
     * Desktop: StocksReport.PartyProcessingstockGeneralSummaryByWeight
     */
    @RequestMapping(value = "/reports/print/466-04-job-lotand-item-stock-summary-party-processing", method = RequestMethod.POST)
    public void print46604JobLotandItemStockSummaryPartyProcessing(HttpServletResponse response, @RequestBody(required = false) Rpt46604JobLotandItemStockSummaryPartyProcessingRequest request) throws Exception {
        if (request == null) request = new Rpt46604JobLotandItemStockSummaryPartyProcessingRequest();
        printReport(response, "466_04-JobLotandItemStockSummaryPartyProcessing.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/466-04-job-lotand-item-stock-summary-party-processing", method = RequestMethod.GET)
    public void print46604JobLotandItemStockSummaryPartyProcessingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print46604JobLotandItemStockSummaryPartyProcessing(response, objectMapper.convertValue(query, Rpt46604JobLotandItemStockSummaryPartyProcessingRequest.class));
    }

    /**
     * Template: 466_05-ItemandCropYearandWarehouseStockSummaryPartyProcessing.rpt
     * Procedure: Sp_InventoryTransactionsPartyProcessing_StockSummaryGeneralByWeight_Rpt
     * Desktop: StocksReport.PartyProcessingstockGeneralSummaryByWeight
     */
    @RequestMapping(value = "/reports/print/466-05-itemand-crop-yearand-warehouse-stock-summary-party-processing", method = RequestMethod.POST)
    public void print46605ItemandCropYearandWarehouseStockSummaryPartyProcessing(HttpServletResponse response, @RequestBody(required = false) Rpt46605ItemandCropYearandWarehouseStockSummaryPartyProcessingRequest request) throws Exception {
        if (request == null) request = new Rpt46605ItemandCropYearandWarehouseStockSummaryPartyProcessingRequest();
        printReport(response, "466_05-ItemandCropYearandWarehouseStockSummaryPartyProcessing.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/466-05-itemand-crop-yearand-warehouse-stock-summary-party-processing", method = RequestMethod.GET)
    public void print46605ItemandCropYearandWarehouseStockSummaryPartyProcessingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print46605ItemandCropYearandWarehouseStockSummaryPartyProcessing(response, objectMapper.convertValue(query, Rpt46605ItemandCropYearandWarehouseStockSummaryPartyProcessingRequest.class));
    }

    /**
     * Template: 466_06-WarehouseandJoblotandItemStockSummaryStockSummaryPartyProcessing.rpt
     * Procedure: Sp_InventoryTransactionsPartyProcessing_StockSummaryGeneralByWeight_Rpt
     * Desktop: StocksReport.PartyProcessingstockGeneralSummaryByWeight
     */
    @RequestMapping(value = "/reports/print/466-06-warehouseand-joblotand-item-stock-summary-stock-summary-party-processing", method = RequestMethod.POST)
    public void print46606WarehouseandJoblotandItemStockSummaryStockSummaryPartyProcessing(HttpServletResponse response, @RequestBody(required = false) Rpt46606WarehouseandJoblotandItemStockSummaryStockSummaryPartyProcessingRequest request) throws Exception {
        if (request == null) request = new Rpt46606WarehouseandJoblotandItemStockSummaryStockSummaryPartyProcessingRequest();
        printReport(response, "466_06-WarehouseandJoblotandItemStockSummaryStockSummaryPartyProcessing.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/466-06-warehouseand-joblotand-item-stock-summary-stock-summary-party-processing", method = RequestMethod.GET)
    public void print46606WarehouseandJoblotandItemStockSummaryStockSummaryPartyProcessingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print46606WarehouseandJoblotandItemStockSummaryStockSummaryPartyProcessing(response, objectMapper.convertValue(query, Rpt46606WarehouseandJoblotandItemStockSummaryStockSummaryPartyProcessingRequest.class));
    }

    /**
     * Template: 600-StockConversionPartyProcessing_Summery_Rpt.rpt
     * Procedure: SpInvStockConversionPartyProcessing_Summery_Rpt
     * Desktop: StockConversionPartyProcessing.StockConversionPartyProcessingSummeryRpt
     */
    @RequestMapping(value = "/reports/print/600-stock-conversion-party-processing-summery", method = RequestMethod.POST)
    public void print600StockConversionPartyProcessingSummery(HttpServletResponse response, @RequestBody(required = false) Rpt600StockConversionPartyProcessingSummeryRequest request) throws Exception {
        if (request == null) request = new Rpt600StockConversionPartyProcessingSummeryRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "600-StockConversionPartyProcessing_Summery_Rpt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/600-stock-conversion-party-processing-summery", method = RequestMethod.GET)
    public void print600StockConversionPartyProcessingSummeryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print600StockConversionPartyProcessingSummery(response, objectMapper.convertValue(query, Rpt600StockConversionPartyProcessingSummeryRequest.class));
    }

    /**
     * Template: 601_01-FoodProductionTransactionSlip.rpt
     * Procedure: [dbo].[Sp_InvFoodProductionPartyProcessing_Rpt]
     * Desktop: InvFoodProductionPartyProcessing.FoodProductionSlip
     */
    @RequestMapping(value = "/reports/print/601-01-food-production-transaction-slip", method = RequestMethod.POST)
    public void print60101FoodProductionTransactionSlip(HttpServletResponse response, @RequestBody(required = false) Rpt60101FoodProductionTransactionSlipRequest request) throws Exception {
        if (request == null) request = new Rpt60101FoodProductionTransactionSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "601_01-FoodProductionTransactionSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/601-01-food-production-transaction-slip", method = RequestMethod.GET)
    public void print60101FoodProductionTransactionSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print60101FoodProductionTransactionSlip(response, objectMapper.convertValue(query, Rpt60101FoodProductionTransactionSlipRequest.class));
    }

    /**
     * Template: 601_02-InvFoodPackingMaterialSlip.rpt
     * Procedure: [dbo].[USP_FoodProductionPartyProcessingPackingMaterial_SlipAndRegister]
     * Desktop: InvFoodProductionPartyProcessingPackingMaterial.FoodProductionPartyProcessingPackingMaterial_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/601-02-inv-food-packing-material-slip", method = RequestMethod.POST)
    public void print60102InvFoodPackingMaterialSlip(HttpServletResponse response, @RequestBody(required = false) Rpt60102InvFoodPackingMaterialSlipRequest request) throws Exception {
        if (request == null) request = new Rpt60102InvFoodPackingMaterialSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "601_02-InvFoodPackingMaterialSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/601-02-inv-food-packing-material-slip", method = RequestMethod.GET)
    public void print60102InvFoodPackingMaterialSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print60102InvFoodPackingMaterialSlip(response, objectMapper.convertValue(query, Rpt60102InvFoodPackingMaterialSlipRequest.class));
    }

    /**
     * Template: 608-InvRptProductionSummeryPartyProcessing.rpt
     * Procedure: Sp_InvFoodProductionPartyProcessing_Summery_Rpt
     * Desktop: ProductionReports.InvFoodProductionPartyProcessingRecoverySummeryReport
     */
    @RequestMapping(value = "/reports/print/608-production-summery-party-processing", method = RequestMethod.POST)
    public void print608ProductionSummeryPartyProcessing(HttpServletResponse response, @RequestBody(required = false) Rpt608ProductionSummeryPartyProcessingRequest request) throws Exception {
        if (request == null) request = new Rpt608ProductionSummeryPartyProcessingRequest();
        printReport(response, "608-InvRptProductionSummeryPartyProcessing.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/608-production-summery-party-processing", method = RequestMethod.GET)
    public void print608ProductionSummeryPartyProcessingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print608ProductionSummeryPartyProcessing(response, objectMapper.convertValue(query, Rpt608ProductionSummeryPartyProcessingRequest.class));
    }

    /**
     * Template: 609-FoodProductionIssuancePartyProcessingGrnWiseByJobOrderId.rpt
     * Procedure: Sp_InvFoodProductionPartyProcessingIssuanceGrnWiseByJobOrderId_rpt
     * Desktop: ProductionReports.FoodProductionPartyProcessingIssuanceGrnWiseByJobOrderId_609
     */
    @RequestMapping(value = "/reports/print/609-food-production-issuance-party-processing-grn-wise-by-job-order-id", method = RequestMethod.POST)
    public void print609FoodProductionIssuancePartyProcessingGrnWiseByJobOrderId(HttpServletResponse response, @RequestBody(required = false) Rpt609FoodProductionIssuancePartyProcessingGrnWiseByJobOrderIdRequest request) throws Exception {
        if (request == null) request = new Rpt609FoodProductionIssuancePartyProcessingGrnWiseByJobOrderIdRequest();
        printReport(response, "609-FoodProductionIssuancePartyProcessingGrnWiseByJobOrderId.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/609-food-production-issuance-party-processing-grn-wise-by-job-order-id", method = RequestMethod.GET)
    public void print609FoodProductionIssuancePartyProcessingGrnWiseByJobOrderIdGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print609FoodProductionIssuancePartyProcessingGrnWiseByJobOrderId(response, objectMapper.convertValue(query, Rpt609FoodProductionIssuancePartyProcessingGrnWiseByJobOrderIdRequest.class));
    }

    /**
     * Template: 611-ProductionPartyProcessingBill.rpt
     * Procedure: spInvProductionProcessingBill_PartyProcessingModule_Report
     * Desktop: InvProductionProcessingBill.InvProductionProcessingBill_PartyProcessingModule_Report_611
     */
    @RequestMapping(value = "/reports/print/611-production-party-processing-bill", method = RequestMethod.POST)
    public void print611ProductionPartyProcessingBill(HttpServletResponse response, @RequestBody(required = false) Rpt611ProductionPartyProcessingBillRequest request) throws Exception {
        if (request == null) request = new Rpt611ProductionPartyProcessingBillRequest();
        printReport(response, "611-ProductionPartyProcessingBill.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/611-production-party-processing-bill", method = RequestMethod.GET)
    public void print611ProductionPartyProcessingBillGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print611ProductionPartyProcessingBill(response, objectMapper.convertValue(query, Rpt611ProductionPartyProcessingBillRequest.class));
    }

    /**
     * Template: 626-ProductionJobOrderPartyProcessing.rpt
     * Procedure: [dbo].[USP_ProductionJobOrderPartyProcessing_SlipandRegister]
     * Desktop: InvProductionJobOrderPartyProcessing.ProductionJobOrderPartyProcessing_SlipandRegister
     */
    @RequestMapping(value = "/reports/print/626-production-job-order-party-processing", method = RequestMethod.POST)
    public void print626ProductionJobOrderPartyProcessing(HttpServletResponse response, @RequestBody(required = false) Rpt626ProductionJobOrderPartyProcessingRequest request) throws Exception {
        if (request == null) request = new Rpt626ProductionJobOrderPartyProcessingRequest();
        printReport(response, "626-ProductionJobOrderPartyProcessing.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/626-production-job-order-party-processing", method = RequestMethod.GET)
    public void print626ProductionJobOrderPartyProcessingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print626ProductionJobOrderPartyProcessing(response, objectMapper.convertValue(query, Rpt626ProductionJobOrderPartyProcessingRequest.class));
    }

    /**
     * Template: 670_01_ProductionRegisterWithActivityPartyProcessing.rpt
     * Procedure: [dbo].[USP_ProductionPartyProcessingRegisterWithActivity]
     * Desktop: InvFoodProductionPartyProcessing.ProductionRegisterPartyProcessing
     */
    @RequestMapping(value = "/reports/print/670-01-production-register-with-activity-party-processing", method = RequestMethod.POST)
    public void print67001ProductionRegisterWithActivityPartyProcessing(HttpServletResponse response, @RequestBody(required = false) Rpt67001ProductionRegisterWithActivityPartyProcessingRequest request) throws Exception {
        if (request == null) request = new Rpt67001ProductionRegisterWithActivityPartyProcessingRequest();
        printReport(response, "670_01_ProductionRegisterWithActivityPartyProcessing.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/670-01-production-register-with-activity-party-processing", method = RequestMethod.GET)
    public void print67001ProductionRegisterWithActivityPartyProcessingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print67001ProductionRegisterWithActivityPartyProcessing(response, objectMapper.convertValue(query, Rpt67001ProductionRegisterWithActivityPartyProcessingRequest.class));
    }

    /**
     * Template: 670_02_ProductionRegisterWithActivityPartyProcessing.rpt
     * Procedure: [dbo].[USP_ProductionPartyProcessingRegisterWithActivity]
     * Desktop: InvFoodProductionPartyProcessing.ProductionRegisterPartyProcessing
     */
    @RequestMapping(value = "/reports/print/670-02-production-register-with-activity-party-processing", method = RequestMethod.POST)
    public void print67002ProductionRegisterWithActivityPartyProcessing(HttpServletResponse response, @RequestBody(required = false) Rpt67002ProductionRegisterWithActivityPartyProcessingRequest request) throws Exception {
        if (request == null) request = new Rpt67002ProductionRegisterWithActivityPartyProcessingRequest();
        printReport(response, "670_02_ProductionRegisterWithActivityPartyProcessing.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/670-02-production-register-with-activity-party-processing", method = RequestMethod.GET)
    public void print67002ProductionRegisterWithActivityPartyProcessingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print67002ProductionRegisterWithActivityPartyProcessing(response, objectMapper.convertValue(query, Rpt67002ProductionRegisterWithActivityPartyProcessingRequest.class));
    }

    /**
     * Template: 670_ProductionRegisterWithActivity_OutPutByPackingMaterialPartyProcessing.rpt
     * Procedure: [dbo].[USP_ProductionPartyProcessingRegisterWithActivity]
     * Desktop: InvFoodProductionPartyProcessing.ProductionRegisterPartyProcessing
     */
    @RequestMapping(value = "/reports/print/670-production-register-with-activity-out-put-by-packing-material-party-processing", method = RequestMethod.POST)
    public void print670ProductionRegisterWithActivityOutPutByPackingMaterialPartyProcessing(HttpServletResponse response, @RequestBody(required = false) Rpt670ProductionRegisterWithActivityOutPutByPackingMaterialPartyProcessingRequest request) throws Exception {
        if (request == null) request = new Rpt670ProductionRegisterWithActivityOutPutByPackingMaterialPartyProcessingRequest();
        printReport(response, "670_ProductionRegisterWithActivity_OutPutByPackingMaterialPartyProcessing.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/670-production-register-with-activity-out-put-by-packing-material-party-processing", method = RequestMethod.GET)
    public void print670ProductionRegisterWithActivityOutPutByPackingMaterialPartyProcessingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print670ProductionRegisterWithActivityOutPutByPackingMaterialPartyProcessing(response, objectMapper.convertValue(query, Rpt670ProductionRegisterWithActivityOutPutByPackingMaterialPartyProcessingRequest.class));
    }

    /**
     * Template: PartyProcessingBillByProductSubReport.rpt
     * Procedure: Usp_ProductionProcessingBillOutPut_SubReport
     * Desktop: InvProductionProcessingBill.InvProductionProcessingBillByProduct_SubReport_611
     */
    @RequestMapping(value = "/reports/print/party-processing-bill-by-product-sub-report", method = RequestMethod.POST)
    public void printPartyProcessingBillByProductSubReport(HttpServletResponse response, @RequestBody(required = false) RptPartyProcessingBillByProductSubReportRequest request) throws Exception {
        if (request == null) request = new RptPartyProcessingBillByProductSubReportRequest();
        printReport(response, "PartyProcessingBillByProductSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/party-processing-bill-by-product-sub-report", method = RequestMethod.GET)
    public void printPartyProcessingBillByProductSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printPartyProcessingBillByProductSubReport(response, objectMapper.convertValue(query, RptPartyProcessingBillByProductSubReportRequest.class));
    }

    /**
     * Template: PartyProcessingBillSubReport.rpt
     * Procedure: spInvProductionProcessingBill_PartyProcessingModule_SubReport
     * Desktop: InvProductionProcessingBill.InvProductionProcessingBill_SubReport_611
     */
    @RequestMapping(value = "/reports/print/party-processing-bill-sub-report", method = RequestMethod.POST)
    public void printPartyProcessingBillSubReport(HttpServletResponse response, @RequestBody(required = false) RptPartyProcessingBillSubReportRequest request) throws Exception {
        if (request == null) request = new RptPartyProcessingBillSubReportRequest();
        printReport(response, "PartyProcessingBillSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/party-processing-bill-sub-report", method = RequestMethod.GET)
    public void printPartyProcessingBillSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printPartyProcessingBillSubReport(response, objectMapper.convertValue(query, RptPartyProcessingBillSubReportRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
