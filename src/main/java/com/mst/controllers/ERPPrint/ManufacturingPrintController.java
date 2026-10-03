package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.ManufacturingPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** Manufacturing print actions. Generated from the verified seeder contracts. */
@Controller
public class ManufacturingPrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 1614_WorkOrder_Slip.rpt
     * Procedure: [Mfg].[USP_WorkOrder_SlipAndRegister]
     * Desktop: WorkOrder.WorkOrder_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1614-work-order-slip", method = RequestMethod.POST)
    public void print1614WorkOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1614WorkOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1614WorkOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1614_WorkOrder_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1614-work-order-slip", method = RequestMethod.GET)
    public void print1614WorkOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1614WorkOrderSlip(response, objectMapper.convertValue(query, Rpt1614WorkOrderSlipRequest.class));
    }

    /**
     * Template: 1619-Production_Slip.rpt
     * Procedure: [Mfg].[USP_ProductionSlipAndRegister_Engr]
     * Desktop: ProductionHeader.ProductionMfgSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1619-production-slip", method = RequestMethod.POST)
    public void print1619ProductionSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1619ProductionSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1619ProductionSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1619-Production_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1619-production-slip", method = RequestMethod.GET)
    public void print1619ProductionSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1619ProductionSlip(response, objectMapper.convertValue(query, Rpt1619ProductionSlipRequest.class));
    }

    /**
     * Template: 1620_WorkOrderInProgress_Slip.rpt
     * Procedure: [Mfg].[USP_WorkOrderInProgress_SlipAndRegister]
     * Desktop: WorkOrderInProgress.WorkOrderInProgress_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1620-work-order-in-progress-slip", method = RequestMethod.POST)
    public void print1620WorkOrderInProgressSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1620WorkOrderInProgressSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1620WorkOrderInProgressSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1620_WorkOrderInProgress_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1620-work-order-in-progress-slip", method = RequestMethod.GET)
    public void print1620WorkOrderInProgressSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1620WorkOrderInProgressSlip(response, objectMapper.convertValue(query, Rpt1620WorkOrderInProgressSlipRequest.class));
    }

    /**
     * Template: 1621_WorkOrderSemiFinish_Slip.rpt
     * Procedure: [Mfg].[USP_WorkOrderForSemiFinish_SlipAndRegister]
     * Desktop: WorkOrder.WorkOrderForSemiFinish_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1621-work-order-semi-finish-slip", method = RequestMethod.POST)
    public void print1621WorkOrderSemiFinishSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1621WorkOrderSemiFinishSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1621WorkOrderSemiFinishSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1621_WorkOrderSemiFinish_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1621-work-order-semi-finish-slip", method = RequestMethod.GET)
    public void print1621WorkOrderSemiFinishSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1621WorkOrderSemiFinishSlip(response, objectMapper.convertValue(query, Rpt1621WorkOrderSemiFinishSlipRequest.class));
    }

    /**
     * Template: 1656_SaleOrderDetailRegister.rpt
     * Procedure: [Mfg].[USP_SaleOrderDetailRegister_Eng]
     * Desktop: SaleOrder.SaleOrderDetailRegister_Mfg
     */
    @RequestMapping(value = "/reports/print/1656-sale-order-detail-register", method = RequestMethod.POST)
    public void print1656SaleOrderDetailRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1656SaleOrderDetailRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1656SaleOrderDetailRegisterRequest();
        printReport(response, "1656_SaleOrderDetailRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1656-sale-order-detail-register", method = RequestMethod.GET)
    public void print1656SaleOrderDetailRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1656SaleOrderDetailRegister(response, objectMapper.convertValue(query, Rpt1656SaleOrderDetailRegisterRequest.class));
    }

    /**
     * Template: 1657_01_DeliveryOrderRegister_Eng.rpt
     * Procedure: [Mfg].[USP_DeliveryOrderRegister_Eng]
     * Desktop: InvDeliveryOrder.DeliveryOrderRegister_MfgEng
     */
    @RequestMapping(value = "/reports/print/1657-01-delivery-order-register-eng", method = RequestMethod.POST)
    public void print165701DeliveryOrderRegisterEng(HttpServletResponse response, @RequestBody(required = false) Rpt165701DeliveryOrderRegisterEngRequest request) throws Exception {
        if (request == null) request = new Rpt165701DeliveryOrderRegisterEngRequest();
        printReport(response, "1657_01_DeliveryOrderRegister_Eng.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1657-01-delivery-order-register-eng", method = RequestMethod.GET)
    public void print165701DeliveryOrderRegisterEngGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print165701DeliveryOrderRegisterEng(response, objectMapper.convertValue(query, Rpt165701DeliveryOrderRegisterEngRequest.class));
    }

    /**
     * Template: 1659_01_GdnRegister_Eng.rpt
     * Procedure: [Mfg].[USP_GdnRegister_Eng]
     * Desktop: InvGdn.GdnRegister_MfgEng
     */
    @RequestMapping(value = "/reports/print/1659-01-gdn-register-eng", method = RequestMethod.POST)
    public void print165901GdnRegisterEng(HttpServletResponse response, @RequestBody(required = false) Rpt165901GdnRegisterEngRequest request) throws Exception {
        if (request == null) request = new Rpt165901GdnRegisterEngRequest();
        printReport(response, "1659_01_GdnRegister_Eng.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1659-01-gdn-register-eng", method = RequestMethod.GET)
    public void print165901GdnRegisterEngGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print165901GdnRegisterEng(response, objectMapper.convertValue(query, Rpt165901GdnRegisterEngRequest.class));
    }

    /**
     * Template: 1659_GdnSlip_Engr.rpt
     * Procedure: [dbo].[USP_GdnRegister_Eng]
     * Desktop: InvGdn.GdnRegister_Eng
     */
    @RequestMapping(value = "/reports/print/1659-gdn-slip-engr", method = RequestMethod.POST)
    public void print1659GdnSlipEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1659GdnSlipEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1659GdnSlipEngrRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1659_GdnSlip_Engr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1659-gdn-slip-engr", method = RequestMethod.GET)
    public void print1659GdnSlipEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1659GdnSlipEngr(response, objectMapper.convertValue(query, Rpt1659GdnSlipEngrRequest.class));
    }

    /**
     * Template: 1663_StockTransfer_Slip.rpt
     * Procedure: [Mfg].[USP_InvStockTransferSlipRegister]
     * Desktop: InvStockTransferHeader.StockTransferRegister
     */
    @RequestMapping(value = "/reports/print/1663-stock-transfer-slip", method = RequestMethod.POST)
    public void print1663StockTransferSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1663StockTransferSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1663StockTransferSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1663_StockTransfer_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1663-stock-transfer-slip", method = RequestMethod.GET)
    public void print1663StockTransferSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1663StockTransferSlip(response, objectMapper.convertValue(query, Rpt1663StockTransferSlipRequest.class));
    }

    /**
     * Template: 1665_StockTransferForProductionRejection_Slip.rpt
     * Procedure: [Mfg].[USP_InvStockTransferSlipRegister]
     * Desktop: InvStockTransferHeader.StockTransferRegister
     */
    @RequestMapping(value = "/reports/print/1665-stock-transfer-for-production-rejection-slip", method = RequestMethod.POST)
    public void print1665StockTransferForProductionRejectionSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1665StockTransferForProductionRejectionSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1665StockTransferForProductionRejectionSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1665_StockTransferForProductionRejection_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1665-stock-transfer-for-production-rejection-slip", method = RequestMethod.GET)
    public void print1665StockTransferForProductionRejectionSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1665StockTransferForProductionRejectionSlip(response, objectMapper.convertValue(query, Rpt1665StockTransferForProductionRejectionSlipRequest.class));
    }

    /**
     * Template: 1670_ProductionEngr_Slip.rpt
     * Procedure: [Mfg].[USP_ProductionHeader_Slip]
     * Desktop: ProductionHeader.ProductionHeader_Slip
     */
    @RequestMapping(value = "/reports/print/1670-production-engr-slip", method = RequestMethod.POST)
    public void print1670ProductionEngrSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1670ProductionEngrSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1670ProductionEngrSlipRequest();
        printReport(response, "1670_ProductionEngr_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1670-production-engr-slip", method = RequestMethod.GET)
    public void print1670ProductionEngrSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1670ProductionEngrSlip(response, objectMapper.convertValue(query, Rpt1670ProductionEngrSlipRequest.class));
    }

    /**
     * Template: 1671_ItemStockSummary_Engr.rpt
     * Procedure: [mfg].[usp_ItemStockReportWithValues]
     * Desktop: StocksReport.StockReportWithValues_Engr
     */
    @RequestMapping(value = "/reports/print/1671-item-stock-summary-engr", method = RequestMethod.POST)
    public void print1671ItemStockSummaryEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1671ItemStockSummaryEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1671ItemStockSummaryEngrRequest();
        printReport(response, "1671_ItemStockSummary_Engr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1671-item-stock-summary-engr", method = RequestMethod.GET)
    public void print1671ItemStockSummaryEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1671ItemStockSummaryEngr(response, objectMapper.convertValue(query, Rpt1671ItemStockSummaryEngrRequest.class));
    }

    /**
     * Template: 1672_01_ItemEvaluationLedger.rpt
     * Procedure: [Mfg].[usp_ItemLedgerFromStockEvaluations]
     * Desktop: InventoryStockEvalautionDetail.ItemLedgerFromStockEvaluations_Engr
     */
    @RequestMapping(value = "/reports/print/1672-01-item-evaluation-ledger", method = RequestMethod.POST)
    public void print167201ItemEvaluationLedger(HttpServletResponse response, @RequestBody(required = false) Rpt167201ItemEvaluationLedgerRequest request) throws Exception {
        if (request == null) request = new Rpt167201ItemEvaluationLedgerRequest();
        printReport(response, "1672_01_ItemEvaluationLedger.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1672-01-item-evaluation-ledger", method = RequestMethod.GET)
    public void print167201ItemEvaluationLedgerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print167201ItemEvaluationLedger(response, objectMapper.convertValue(query, Rpt167201ItemEvaluationLedgerRequest.class));
    }

    /**
     * Template: 1672_EvaluationTransactionReportWithValues_Engr.rpt
     * Procedure: [Mfg].[usp_EvaluationTransactionReportWithValues]
     * Desktop: StocksReport.EvaluationTransactionReportWithValues_Engr
     */
    @RequestMapping(value = "/reports/print/1672-evaluation-transaction-report-with-values-engr", method = RequestMethod.POST)
    public void print1672EvaluationTransactionReportWithValuesEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1672EvaluationTransactionReportWithValuesEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1672EvaluationTransactionReportWithValuesEngrRequest();
        printReport(response, "1672_EvaluationTransactionReportWithValues_Engr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1672-evaluation-transaction-report-with-values-engr", method = RequestMethod.GET)
    public void print1672EvaluationTransactionReportWithValuesEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1672EvaluationTransactionReportWithValuesEngr(response, objectMapper.convertValue(query, Rpt1672EvaluationTransactionReportWithValuesEngrRequest.class));
    }

    /**
     * Template: 207-RecipeSlip.rpt
     * Procedure: USp_Recipe_SlipandRegister
     * Desktop: InvManufacturingBOMHeader.ReceipePrint
     */
    @RequestMapping(value = "/reports/print/207-recipe-slip", method = RequestMethod.POST)
    public void print207RecipeSlip(HttpServletResponse response, @RequestBody(required = false) Rpt207RecipeSlipRequest request) throws Exception {
        if (request == null) request = new Rpt207RecipeSlipRequest();
        printReport(response, "207-RecipeSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/207-recipe-slip", method = RequestMethod.GET)
    public void print207RecipeSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print207RecipeSlip(response, objectMapper.convertValue(query, Rpt207RecipeSlipRequest.class));
    }

    /**
     * Template: 271A-InvRptSalesOrderRegister.rpt
     * Procedure: [Mfg].[USP_SaleOrderDetailRegister_Eng]
     * Desktop: SaleOrder.SaleOrderDetailRegister_Mfg
     */
    @RequestMapping(value = "/reports/print/271a-sales-order-register", method = RequestMethod.POST)
    public void print271ASalesOrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt271ASalesOrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt271ASalesOrderRegisterRequest();
        printReport(response, "271A-InvRptSalesOrderRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/271a-sales-order-register", method = RequestMethod.GET)
    public void print271ASalesOrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print271ASalesOrderRegister(response, objectMapper.convertValue(query, Rpt271ASalesOrderRegisterRequest.class));
    }

    /**
     * Template: RecipeExpenseSubReport.rpt
     * Procedure: USP_ReceipeExpense_SubRpt
     * Desktop: InvManufacturingBOMHeader.RecipeSubReport
     */
    @RequestMapping(value = "/reports/print/recipe-expense-sub-report", method = RequestMethod.POST)
    public void printRecipeExpenseSubReport(HttpServletResponse response, @RequestBody(required = false) RptRecipeExpenseSubReportRequest request) throws Exception {
        if (request == null) request = new RptRecipeExpenseSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "RecipeExpenseSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/recipe-expense-sub-report", method = RequestMethod.GET)
    public void printRecipeExpenseSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printRecipeExpenseSubReport(response, objectMapper.convertValue(query, RptRecipeExpenseSubReportRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
