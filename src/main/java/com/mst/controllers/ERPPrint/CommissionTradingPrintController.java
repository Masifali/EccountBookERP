package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.CommissionTradingPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** CommissionTrading print actions. Generated from the verified seeder contracts. */
@Controller
public class CommissionTradingPrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 1056_01_CommissionAgentTradeBillPurchaseSlip.rpt
     * Procedure: Sp_InvCommAgentTradeBill_SlipandRegister
     * Desktop: InvCommAgentTradeBill.TradeBillSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1056-01-commission-agent-trade-bill-purchase-slip", method = RequestMethod.POST)
    public void print105601CommissionAgentTradeBillPurchaseSlip(HttpServletResponse response, @RequestBody(required = false) Rpt105601CommissionAgentTradeBillPurchaseSlipRequest request) throws Exception {
        if (request == null) request = new Rpt105601CommissionAgentTradeBillPurchaseSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1056_01_CommissionAgentTradeBillPurchaseSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1056-01-commission-agent-trade-bill-purchase-slip", method = RequestMethod.GET)
    public void print105601CommissionAgentTradeBillPurchaseSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print105601CommissionAgentTradeBillPurchaseSlip(response, objectMapper.convertValue(query, Rpt105601CommissionAgentTradeBillPurchaseSlipRequest.class));
    }

    /**
     * Template: 1056_02_CommissionAgentTradeBillSaleSlip.rpt
     * Procedure: Sp_InvCommAgentTradeBill_SlipandRegister
     * Desktop: InvCommAgentTradeBill.TradeBillSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1056-02-commission-agent-trade-bill-sale-slip", method = RequestMethod.POST)
    public void print105602CommissionAgentTradeBillSaleSlip(HttpServletResponse response, @RequestBody(required = false) Rpt105602CommissionAgentTradeBillSaleSlipRequest request) throws Exception {
        if (request == null) request = new Rpt105602CommissionAgentTradeBillSaleSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1056_02_CommissionAgentTradeBillSaleSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1056-02-commission-agent-trade-bill-sale-slip", method = RequestMethod.GET)
    public void print105602CommissionAgentTradeBillSaleSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print105602CommissionAgentTradeBillSaleSlip(response, objectMapper.convertValue(query, Rpt105602CommissionAgentTradeBillSaleSlipRequest.class));
    }

    /**
     * Template: 1056_03_CommissionAgentTradeBillSaleSlipII.rpt
     * Procedure: Sp_InvCommAgentTradeBill_SlipandRegister
     * Desktop: InvCommAgentTradeBill.TradeBillSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1056-03-commission-agent-trade-bill-sale-slip-ii", method = RequestMethod.POST)
    public void print105603CommissionAgentTradeBillSaleSlipII(HttpServletResponse response, @RequestBody(required = false) Rpt105603CommissionAgentTradeBillSaleSlipIIRequest request) throws Exception {
        if (request == null) request = new Rpt105603CommissionAgentTradeBillSaleSlipIIRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1056_03_CommissionAgentTradeBillSaleSlipII.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1056-03-commission-agent-trade-bill-sale-slip-ii", method = RequestMethod.GET)
    public void print105603CommissionAgentTradeBillSaleSlipIIGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print105603CommissionAgentTradeBillSaleSlipII(response, objectMapper.convertValue(query, Rpt105603CommissionAgentTradeBillSaleSlipIIRequest.class));
    }

    /**
     * Template: 1056_CommissionAgentTradeBillSlip.rpt
     * Procedure: Sp_InvCommAgentTradeBill_SlipandRegister
     * Desktop: InvCommAgentTradeBill.TradeBillSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1056-commission-agent-trade-bill-slip", method = RequestMethod.POST)
    public void print1056CommissionAgentTradeBillSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1056CommissionAgentTradeBillSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1056CommissionAgentTradeBillSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1056_CommissionAgentTradeBillSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1056-commission-agent-trade-bill-slip", method = RequestMethod.GET)
    public void print1056CommissionAgentTradeBillSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1056CommissionAgentTradeBillSlip(response, objectMapper.convertValue(query, Rpt1056CommissionAgentTradeBillSlipRequest.class));
    }

    /**
     * Template: 1056A_CommissionBillIncomeSlip.rpt
     * Procedure: USP_CommissionBillSupplierCustomerAndIncomeTransactions
     * Desktop: InvCommAgentTradeBill.CommissionBillSupplierCustomerAndIncomeTransactionsSlip
     */
    @RequestMapping(value = "/reports/print/1056a-commission-bill-income-slip", method = RequestMethod.POST)
    public void print1056ACommissionBillIncomeSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1056ACommissionBillIncomeSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1056ACommissionBillIncomeSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1056A_CommissionBillIncomeSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1056a-commission-bill-income-slip", method = RequestMethod.GET)
    public void print1056ACommissionBillIncomeSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1056ACommissionBillIncomeSlip(response, objectMapper.convertValue(query, Rpt1056ACommissionBillIncomeSlipRequest.class));
    }

    /**
     * Template: 1702-CommissionTradeLoadingDelivery_Register.rpt
     * Procedure: [CmTr].[USP_CommTradeLoadingDelivery_Register]
     * Desktop: CommTradeLoadingDelivery.CommissionTradeLoadingDeliverySlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1702-commission-trade-loading-delivery-register", method = RequestMethod.POST)
    public void print1702CommissionTradeLoadingDeliveryRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1702CommissionTradeLoadingDeliveryRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1702CommissionTradeLoadingDeliveryRegisterRequest();
        printReport(response, "1702-CommissionTradeLoadingDelivery_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1702-commission-trade-loading-delivery-register", method = RequestMethod.GET)
    public void print1702CommissionTradeLoadingDeliveryRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1702CommissionTradeLoadingDeliveryRegister(response, objectMapper.convertValue(query, Rpt1702CommissionTradeLoadingDeliveryRegisterRequest.class));
    }

    /**
     * Template: 1702-CommissionTradeLoadingDelivery_Slip.rpt
     * Procedure: [CmTr].[USP_CommTradeLoadingDelivery_Register]
     * Desktop: CommTradeLoadingDelivery.CommissionTradeLoadingDeliverySlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1702-commission-trade-loading-delivery-slip", method = RequestMethod.POST)
    public void print1702CommissionTradeLoadingDeliverySlip(HttpServletResponse response, @RequestBody(required = false) Rpt1702CommissionTradeLoadingDeliverySlipRequest request) throws Exception {
        if (request == null) request = new Rpt1702CommissionTradeLoadingDeliverySlipRequest();
        printReport(response, "1702-CommissionTradeLoadingDelivery_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1702-commission-trade-loading-delivery-slip", method = RequestMethod.GET)
    public void print1702CommissionTradeLoadingDeliverySlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1702CommissionTradeLoadingDeliverySlip(response, objectMapper.convertValue(query, Rpt1702CommissionTradeLoadingDeliverySlipRequest.class));
    }

    /**
     * Template: 1702A-CommissionTradeLoadingDelivery_Slip.rpt
     * Procedure: [CmTr].[USP_CommTradeLoadingDelivery_Register]
     * Desktop: CommTradeLoadingDelivery.CommissionTradeLoadingDeliverySlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1702a-commission-trade-loading-delivery-slip", method = RequestMethod.POST)
    public void print1702ACommissionTradeLoadingDeliverySlip(HttpServletResponse response, @RequestBody(required = false) Rpt1702ACommissionTradeLoadingDeliverySlipRequest request) throws Exception {
        if (request == null) request = new Rpt1702ACommissionTradeLoadingDeliverySlipRequest();
        printReport(response, "1702A-CommissionTradeLoadingDelivery_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1702a-commission-trade-loading-delivery-slip", method = RequestMethod.GET)
    public void print1702ACommissionTradeLoadingDeliverySlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1702ACommissionTradeLoadingDeliverySlip(response, objectMapper.convertValue(query, Rpt1702ACommissionTradeLoadingDeliverySlipRequest.class));
    }

    /**
     * Template: 1704-CommissionTransaction_Register.rpt
     * Procedure: [CmTr].[USP_CommTradeTransactionHeader_Register]
     * Desktop: CommTradeTransactionHeader.CommissionTransactionSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1704-commission-transaction-register", method = RequestMethod.POST)
    public void print1704CommissionTransactionRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1704CommissionTransactionRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1704CommissionTransactionRegisterRequest();
        printReport(response, "1704-CommissionTransaction_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1704-commission-transaction-register", method = RequestMethod.GET)
    public void print1704CommissionTransactionRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1704CommissionTransactionRegister(response, objectMapper.convertValue(query, Rpt1704CommissionTransactionRegisterRequest.class));
    }

    /**
     * Template: 1704-CommTradeTransactionCustomerExpense_SubReport.rpt
     * Procedure: [CmTr].[USP_CommTradeTransactionCustomerExpense_SubReport]
     * Desktop: CommTradeTransactionHeader.CommTradeTransactionCustomerExpense_SubReport
     */
    @RequestMapping(value = "/reports/print/1704-comm-trade-transaction-customer-expense-sub-report", method = RequestMethod.POST)
    public void print1704CommTradeTransactionCustomerExpenseSubReport(HttpServletResponse response, @RequestBody(required = false) Rpt1704CommTradeTransactionCustomerExpenseSubReportRequest request) throws Exception {
        if (request == null) request = new Rpt1704CommTradeTransactionCustomerExpenseSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1704-CommTradeTransactionCustomerExpense_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1704-comm-trade-transaction-customer-expense-sub-report", method = RequestMethod.GET)
    public void print1704CommTradeTransactionCustomerExpenseSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1704CommTradeTransactionCustomerExpenseSubReport(response, objectMapper.convertValue(query, Rpt1704CommTradeTransactionCustomerExpenseSubReportRequest.class));
    }

    /**
     * Template: 1704-CommTradeTransactionHeader_Slip.rpt
     * Procedure: [CmTr].[USP_CommTradeTransactionHeader_Register]
     * Desktop: CommTradeTransactionHeader.CommissionTransactionSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1704-comm-trade-transaction-header-slip", method = RequestMethod.POST)
    public void print1704CommTradeTransactionHeaderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1704CommTradeTransactionHeaderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1704CommTradeTransactionHeaderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1704-CommTradeTransactionHeader_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1704-comm-trade-transaction-header-slip", method = RequestMethod.GET)
    public void print1704CommTradeTransactionHeaderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1704CommTradeTransactionHeaderSlip(response, objectMapper.convertValue(query, Rpt1704CommTradeTransactionHeaderSlipRequest.class));
    }

    /**
     * Template: 1704_01_CommTradeTransaction_SlipWithSupplier.rpt
     * Procedure: [CmTr].[USP_CommTradeTransactionHeader_Register]
     * Desktop: CommTradeTransactionHeader.CommissionTransactionSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1704-01-comm-trade-transaction-slip-with-supplier", method = RequestMethod.POST)
    public void print170401CommTradeTransactionSlipWithSupplier(HttpServletResponse response, @RequestBody(required = false) Rpt170401CommTradeTransactionSlipWithSupplierRequest request) throws Exception {
        if (request == null) request = new Rpt170401CommTradeTransactionSlipWithSupplierRequest();
        printReport(response, "1704_01_CommTradeTransaction_SlipWithSupplier.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1704-01-comm-trade-transaction-slip-with-supplier", method = RequestMethod.GET)
    public void print170401CommTradeTransactionSlipWithSupplierGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print170401CommTradeTransactionSlipWithSupplier(response, objectMapper.convertValue(query, Rpt170401CommTradeTransactionSlipWithSupplierRequest.class));
    }

    /**
     * Template: 1704_01_CommTradeTransactionSupplierSummary_SubReport.rpt
     * Procedure: [CmTr].[USP_CommTradeTransactionSupplierSummary_SubReport]
     * Desktop: CommTradeTransactionHeader.CommTradeTransactionSupplierSummary_SubReport
     */
    @RequestMapping(value = "/reports/print/1704-01-comm-trade-transaction-supplier-summary-sub-report", method = RequestMethod.POST)
    public void print170401CommTradeTransactionSupplierSummarySubReport(HttpServletResponse response, @RequestBody(required = false) Rpt170401CommTradeTransactionSupplierSummarySubReportRequest request) throws Exception {
        if (request == null) request = new Rpt170401CommTradeTransactionSupplierSummarySubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1704_01_CommTradeTransactionSupplierSummary_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1704-01-comm-trade-transaction-supplier-summary-sub-report", method = RequestMethod.GET)
    public void print170401CommTradeTransactionSupplierSummarySubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print170401CommTradeTransactionSupplierSummarySubReport(response, objectMapper.convertValue(query, Rpt170401CommTradeTransactionSupplierSummarySubReportRequest.class));
    }

    /**
     * Template: 1705-CommissionOrder_Register.rpt
     * Procedure: [CmTr].[USP_CommTradeOrder_Register]
     * Desktop: CommTradeOrder.CommissionOrderSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1705-commission-order-register", method = RequestMethod.POST)
    public void print1705CommissionOrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1705CommissionOrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1705CommissionOrderRegisterRequest();
        printReport(response, "1705-CommissionOrder_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1705-commission-order-register", method = RequestMethod.GET)
    public void print1705CommissionOrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1705CommissionOrderRegister(response, objectMapper.convertValue(query, Rpt1705CommissionOrderRegisterRequest.class));
    }

    /**
     * Template: 1705-CommissionTradeOrder_Slip.rpt
     * Procedure: [CmTr].[USP_CommTradeOrder_Register]
     * Desktop: CommTradeOrder.CommissionOrderSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1705-commission-trade-order-slip", method = RequestMethod.POST)
    public void print1705CommissionTradeOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1705CommissionTradeOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1705CommissionTradeOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1705-CommissionTradeOrder_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1705-commission-trade-order-slip", method = RequestMethod.GET)
    public void print1705CommissionTradeOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1705CommissionTradeOrderSlip(response, objectMapper.convertValue(query, Rpt1705CommissionTradeOrderSlipRequest.class));
    }

    /**
     * Template: 1706-CommissionBillSupplierCustomerAndIncomeTransactions.rpt
     * Procedure: USP_CommissionBillSupplierCustomerAndIncomeTransactions
     * Desktop: InvCommAgentTradeBill.CommissionBillSupplierCustomerAndIncomeTransactionsSlip
     */
    @RequestMapping(value = "/reports/print/1706-commission-bill-supplier-customer-and-income-transactions", method = RequestMethod.POST)
    public void print1706CommissionBillSupplierCustomerAndIncomeTransactions(HttpServletResponse response, @RequestBody(required = false) Rpt1706CommissionBillSupplierCustomerAndIncomeTransactionsRequest request) throws Exception {
        if (request == null) request = new Rpt1706CommissionBillSupplierCustomerAndIncomeTransactionsRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1706-CommissionBillSupplierCustomerAndIncomeTransactions.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1706-commission-bill-supplier-customer-and-income-transactions", method = RequestMethod.GET)
    public void print1706CommissionBillSupplierCustomerAndIncomeTransactionsGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1706CommissionBillSupplierCustomerAndIncomeTransactions(response, objectMapper.convertValue(query, Rpt1706CommissionBillSupplierCustomerAndIncomeTransactionsRequest.class));
    }

    /**
     * Template: 238-InvPurchaseInvoice_StoreBillWithTax.rpt
     * Procedure: SP_CommisionAgentBillOtherExpense_SubRpt
     * Desktop: InvCommAgentTradeBill.TradeBillSlipAndRegisterSupReprt
     */
    @RequestMapping(value = "/reports/print/238-inv-purchase-invoice-store-bill-with-tax", method = RequestMethod.POST)
    public void print238InvPurchaseInvoiceStoreBillWithTax(HttpServletResponse response, @RequestBody(required = false) Rpt238InvPurchaseInvoiceStoreBillWithTaxRequest request) throws Exception {
        if (request == null) request = new Rpt238InvPurchaseInvoiceStoreBillWithTaxRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "238-InvPurchaseInvoice_StoreBillWithTax.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/238-inv-purchase-invoice-store-bill-with-tax", method = RequestMethod.GET)
    public void print238InvPurchaseInvoiceStoreBillWithTaxGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print238InvPurchaseInvoiceStoreBillWithTax(response, objectMapper.convertValue(query, Rpt238InvPurchaseInvoiceStoreBillWithTaxRequest.class));
    }

    /**
     * Template: 477-InventoryTransaction.rpt
     * Procedure: [CmTr].[USP_InventoryTransactionReport]
     * Desktop: InventoryStockReport.InventoryTransactionReport
     */
    @RequestMapping(value = "/reports/print/477-inventory-transaction", method = RequestMethod.POST)
    public void print477InventoryTransaction(HttpServletResponse response, @RequestBody(required = false) Rpt477InventoryTransactionRequest request) throws Exception {
        if (request == null) request = new Rpt477InventoryTransactionRequest();
        printReport(response, "477-InventoryTransaction.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/477-inventory-transaction", method = RequestMethod.GET)
    public void print477InventoryTransactionGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print477InventoryTransaction(response, objectMapper.convertValue(query, Rpt477InventoryTransactionRequest.class));
    }

    /**
     * Template: 478-EvaluationTransaction.rpt
     * Procedure: [CmTr].[USP_EvaluationTransactionReport]
     * Desktop: InventoryStockReport.EvaluationTransactionReport
     */
    @RequestMapping(value = "/reports/print/478-evaluation-transaction", method = RequestMethod.POST)
    public void print478EvaluationTransaction(HttpServletResponse response, @RequestBody(required = false) Rpt478EvaluationTransactionRequest request) throws Exception {
        if (request == null) request = new Rpt478EvaluationTransactionRequest();
        printReport(response, "478-EvaluationTransaction.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/478-evaluation-transaction", method = RequestMethod.GET)
    public void print478EvaluationTransactionGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print478EvaluationTransaction(response, objectMapper.convertValue(query, Rpt478EvaluationTransactionRequest.class));
    }

    /**
     * Template: 479_01_ItemStockSummary.rpt
     * Procedure: [CmTr].[USP_ItemStockReportWithValues]
     * Desktop: InventoryStockReport.ItemStockReportWithValues
     */
    @RequestMapping(value = "/reports/print/479-01-item-stock-summary", method = RequestMethod.POST)
    public void print47901ItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt47901ItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt47901ItemStockSummaryRequest();
        printReport(response, "479_01_ItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/479-01-item-stock-summary", method = RequestMethod.GET)
    public void print47901ItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print47901ItemStockSummary(response, objectMapper.convertValue(query, Rpt47901ItemStockSummaryRequest.class));
    }

    /**
     * Template: 479_02_ItemandWarehouseStockSummary.rpt
     * Procedure: [CmTr].[USP_ItemStockReportWithValues]
     * Desktop: InventoryStockReport.ItemStockReportWithValues
     */
    @RequestMapping(value = "/reports/print/479-02-itemand-warehouse-stock-summary", method = RequestMethod.POST)
    public void print47902ItemandWarehouseStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt47902ItemandWarehouseStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt47902ItemandWarehouseStockSummaryRequest();
        printReport(response, "479_02_ItemandWarehouseStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/479-02-itemand-warehouse-stock-summary", method = RequestMethod.GET)
    public void print47902ItemandWarehouseStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print47902ItemandWarehouseStockSummary(response, objectMapper.convertValue(query, Rpt47902ItemandWarehouseStockSummaryRequest.class));
    }

    /**
     * Template: 479_03_WarehouseAndItemStockSummary.rpt
     * Procedure: [CmTr].[USP_ItemStockReportWithValues]
     * Desktop: InventoryStockReport.ItemStockReportWithValues
     */
    @RequestMapping(value = "/reports/print/479-03-warehouse-and-item-stock-summary", method = RequestMethod.POST)
    public void print47903WarehouseAndItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt47903WarehouseAndItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt47903WarehouseAndItemStockSummaryRequest();
        printReport(response, "479_03_WarehouseAndItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/479-03-warehouse-and-item-stock-summary", method = RequestMethod.GET)
    public void print47903WarehouseAndItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print47903WarehouseAndItemStockSummary(response, objectMapper.convertValue(query, Rpt47903WarehouseAndItemStockSummaryRequest.class));
    }

    /**
     * Template: 479_04_JobLotandItemStockSummary.rpt
     * Procedure: [CmTr].[USP_ItemStockReportWithValues]
     * Desktop: InventoryStockReport.ItemStockReportWithValues
     */
    @RequestMapping(value = "/reports/print/479-04-job-lotand-item-stock-summary", method = RequestMethod.POST)
    public void print47904JobLotandItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt47904JobLotandItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt47904JobLotandItemStockSummaryRequest();
        printReport(response, "479_04_JobLotandItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/479-04-job-lotand-item-stock-summary", method = RequestMethod.GET)
    public void print47904JobLotandItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print47904JobLotandItemStockSummary(response, objectMapper.convertValue(query, Rpt47904JobLotandItemStockSummaryRequest.class));
    }

    /**
     * Template: 479_05_WarehouseandJoblotandItemStockSummary.rpt
     * Procedure: [CmTr].[USP_ItemStockReportWithValues]
     * Desktop: InventoryStockReport.ItemStockReportWithValues
     */
    @RequestMapping(value = "/reports/print/479-05-warehouseand-joblotand-item-stock-summary", method = RequestMethod.POST)
    public void print47905WarehouseandJoblotandItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt47905WarehouseandJoblotandItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt47905WarehouseandJoblotandItemStockSummaryRequest();
        printReport(response, "479_05_WarehouseandJoblotandItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/479-05-warehouseand-joblotand-item-stock-summary", method = RequestMethod.GET)
    public void print47905WarehouseandJoblotandItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print47905WarehouseandJoblotandItemStockSummary(response, objectMapper.convertValue(query, Rpt47905WarehouseandJoblotandItemStockSummaryRequest.class));
    }

    /**
     * Template: 479_06_WarehouseandItemandJoblotStockSummary.rpt
     * Procedure: [CmTr].[USP_ItemStockReportWithValues]
     * Desktop: InventoryStockReport.ItemStockReportWithValues
     */
    @RequestMapping(value = "/reports/print/479-06-warehouseand-itemand-joblot-stock-summary", method = RequestMethod.POST)
    public void print47906WarehouseandItemandJoblotStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt47906WarehouseandItemandJoblotStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt47906WarehouseandItemandJoblotStockSummaryRequest();
        printReport(response, "479_06_WarehouseandItemandJoblotStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/479-06-warehouseand-itemand-joblot-stock-summary", method = RequestMethod.GET)
    public void print47906WarehouseandItemandJoblotStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print47906WarehouseandItemandJoblotStockSummary(response, objectMapper.convertValue(query, Rpt47906WarehouseandItemandJoblotStockSummaryRequest.class));
    }

    /**
     * Template: 479_07_ItemandPackSizeStockSummary.rpt
     * Procedure: [CmTr].[USP_ItemStockReportWithValues]
     * Desktop: InventoryStockReport.ItemStockReportWithValues
     */
    @RequestMapping(value = "/reports/print/479-07-itemand-pack-size-stock-summary", method = RequestMethod.POST)
    public void print47907ItemandPackSizeStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt47907ItemandPackSizeStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt47907ItemandPackSizeStockSummaryRequest();
        printReport(response, "479_07_ItemandPackSizeStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/479-07-itemand-pack-size-stock-summary", method = RequestMethod.GET)
    public void print47907ItemandPackSizeStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print47907ItemandPackSizeStockSummary(response, objectMapper.convertValue(query, Rpt47907ItemandPackSizeStockSummaryRequest.class));
    }

    /**
     * Template: 479_08_ItemandItemAttributeVarientStockSummary.rpt
     * Procedure: [CmTr].[USP_ItemStockReportWithValues]
     * Desktop: InventoryStockReport.ItemStockReportWithValues
     */
    @RequestMapping(value = "/reports/print/479-08-itemand-item-attribute-varient-stock-summary", method = RequestMethod.POST)
    public void print47908ItemandItemAttributeVarientStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt47908ItemandItemAttributeVarientStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt47908ItemandItemAttributeVarientStockSummaryRequest();
        printReport(response, "479_08_ItemandItemAttributeVarientStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/479-08-itemand-item-attribute-varient-stock-summary", method = RequestMethod.GET)
    public void print47908ItemandItemAttributeVarientStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print47908ItemandItemAttributeVarientStockSummary(response, objectMapper.convertValue(query, Rpt47908ItemandItemAttributeVarientStockSummaryRequest.class));
    }

    /**
     * Template: 479_09_ItemandPackSizeandItemAttributeVarientStockSummary.rpt
     * Procedure: [CmTr].[USP_ItemStockReportWithValues]
     * Desktop: InventoryStockReport.ItemStockReportWithValues
     */
    @RequestMapping(value = "/reports/print/479-09-itemand-pack-sizeand-item-attribute-varient-stock-summary", method = RequestMethod.POST)
    public void print47909ItemandPackSizeandItemAttributeVarientStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt47909ItemandPackSizeandItemAttributeVarientStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt47909ItemandPackSizeandItemAttributeVarientStockSummaryRequest();
        printReport(response, "479_09_ItemandPackSizeandItemAttributeVarientStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/479-09-itemand-pack-sizeand-item-attribute-varient-stock-summary", method = RequestMethod.GET)
    public void print47909ItemandPackSizeandItemAttributeVarientStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print47909ItemandPackSizeandItemAttributeVarientStockSummary(response, objectMapper.convertValue(query, Rpt47909ItemandPackSizeandItemAttributeVarientStockSummaryRequest.class));
    }

    /**
     * Template: 480_01_ItemStockSummary.rpt
     * Procedure: [CmTr].[USP_StockReportWithOutValues]
     * Desktop: InventoryStockReport.StockReportWithOutValues
     */
    @RequestMapping(value = "/reports/print/480-01-item-stock-summary", method = RequestMethod.POST)
    public void print48001ItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt48001ItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt48001ItemStockSummaryRequest();
        printReport(response, "480_01_ItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/480-01-item-stock-summary", method = RequestMethod.GET)
    public void print48001ItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print48001ItemStockSummary(response, objectMapper.convertValue(query, Rpt48001ItemStockSummaryRequest.class));
    }

    /**
     * Template: 480_02_ItemandWarehouseStockSummary.rpt
     * Procedure: [CmTr].[USP_StockReportWithOutValues]
     * Desktop: InventoryStockReport.StockReportWithOutValues
     */
    @RequestMapping(value = "/reports/print/480-02-itemand-warehouse-stock-summary", method = RequestMethod.POST)
    public void print48002ItemandWarehouseStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt48002ItemandWarehouseStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt48002ItemandWarehouseStockSummaryRequest();
        printReport(response, "480_02_ItemandWarehouseStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/480-02-itemand-warehouse-stock-summary", method = RequestMethod.GET)
    public void print48002ItemandWarehouseStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print48002ItemandWarehouseStockSummary(response, objectMapper.convertValue(query, Rpt48002ItemandWarehouseStockSummaryRequest.class));
    }

    /**
     * Template: 480_03_WarehouseAndItemStockSummary.rpt
     * Procedure: [CmTr].[USP_StockReportWithOutValues]
     * Desktop: InventoryStockReport.StockReportWithOutValues
     */
    @RequestMapping(value = "/reports/print/480-03-warehouse-and-item-stock-summary", method = RequestMethod.POST)
    public void print48003WarehouseAndItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt48003WarehouseAndItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt48003WarehouseAndItemStockSummaryRequest();
        printReport(response, "480_03_WarehouseAndItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/480-03-warehouse-and-item-stock-summary", method = RequestMethod.GET)
    public void print48003WarehouseAndItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print48003WarehouseAndItemStockSummary(response, objectMapper.convertValue(query, Rpt48003WarehouseAndItemStockSummaryRequest.class));
    }

    /**
     * Template: 480_06_JobLotandItemStockSummary.rpt
     * Procedure: [CmTr].[USP_StockReportWithOutValues]
     * Desktop: InventoryStockReport.StockReportWithOutValues
     */
    @RequestMapping(value = "/reports/print/480-06-job-lotand-item-stock-summary", method = RequestMethod.POST)
    public void print48006JobLotandItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt48006JobLotandItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt48006JobLotandItemStockSummaryRequest();
        printReport(response, "480_06_JobLotandItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/480-06-job-lotand-item-stock-summary", method = RequestMethod.GET)
    public void print48006JobLotandItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print48006JobLotandItemStockSummary(response, objectMapper.convertValue(query, Rpt48006JobLotandItemStockSummaryRequest.class));
    }

    /**
     * Template: 480_07_ItemWarehouseAndJoblotStockSummary.rpt
     * Procedure: [CmTr].[USP_StockReportWithOutValues]
     * Desktop: InventoryStockReport.StockReportWithOutValues
     */
    @RequestMapping(value = "/reports/print/480-07-item-warehouse-and-joblot-stock-summary", method = RequestMethod.POST)
    public void print48007ItemWarehouseAndJoblotStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt48007ItemWarehouseAndJoblotStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt48007ItemWarehouseAndJoblotStockSummaryRequest();
        printReport(response, "480_07_ItemWarehouseAndJoblotStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/480-07-item-warehouse-and-joblot-stock-summary", method = RequestMethod.GET)
    public void print48007ItemWarehouseAndJoblotStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print48007ItemWarehouseAndJoblotStockSummary(response, objectMapper.convertValue(query, Rpt48007ItemWarehouseAndJoblotStockSummaryRequest.class));
    }

    /**
     * Template: 480_08_ItemandPackSizeStockSummary.rpt
     * Procedure: [CmTr].[USP_StockReportWithOutValues]
     * Desktop: InventoryStockReport.StockReportWithOutValues
     */
    @RequestMapping(value = "/reports/print/480-08-itemand-pack-size-stock-summary", method = RequestMethod.POST)
    public void print48008ItemandPackSizeStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt48008ItemandPackSizeStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt48008ItemandPackSizeStockSummaryRequest();
        printReport(response, "480_08_ItemandPackSizeStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/480-08-itemand-pack-size-stock-summary", method = RequestMethod.GET)
    public void print48008ItemandPackSizeStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print48008ItemandPackSizeStockSummary(response, objectMapper.convertValue(query, Rpt48008ItemandPackSizeStockSummaryRequest.class));
    }

    /**
     * Template: 480_09_ItemandItemAttributeVarientStockSummary.rpt
     * Procedure: [CmTr].[USP_StockReportWithOutValues]
     * Desktop: InventoryStockReport.StockReportWithOutValues
     */
    @RequestMapping(value = "/reports/print/480-09-itemand-item-attribute-varient-stock-summary", method = RequestMethod.POST)
    public void print48009ItemandItemAttributeVarientStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt48009ItemandItemAttributeVarientStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt48009ItemandItemAttributeVarientStockSummaryRequest();
        printReport(response, "480_09_ItemandItemAttributeVarientStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/480-09-itemand-item-attribute-varient-stock-summary", method = RequestMethod.GET)
    public void print48009ItemandItemAttributeVarientStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print48009ItemandItemAttributeVarientStockSummary(response, objectMapper.convertValue(query, Rpt48009ItemandItemAttributeVarientStockSummaryRequest.class));
    }

    /**
     * Template: 480_10_ItemandPackSizeandItemAttributeVarientStockSummary.rpt
     * Procedure: [CmTr].[USP_StockReportWithOutValues]
     * Desktop: InventoryStockReport.StockReportWithOutValues
     */
    @RequestMapping(value = "/reports/print/480-10-itemand-pack-sizeand-item-attribute-varient-stock-summary", method = RequestMethod.POST)
    public void print48010ItemandPackSizeandItemAttributeVarientStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt48010ItemandPackSizeandItemAttributeVarientStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt48010ItemandPackSizeandItemAttributeVarientStockSummaryRequest();
        printReport(response, "480_10_ItemandPackSizeandItemAttributeVarientStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/480-10-itemand-pack-sizeand-item-attribute-varient-stock-summary", method = RequestMethod.GET)
    public void print48010ItemandPackSizeandItemAttributeVarientStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print48010ItemandPackSizeandItemAttributeVarientStockSummary(response, objectMapper.convertValue(query, Rpt48010ItemandPackSizeandItemAttributeVarientStockSummaryRequest.class));
    }

    /**
     * Template: 556-CommissionTradeLoadingDelivery_Slip.rpt
     * Procedure: [CmTr].[USP_CommTradeLoadingDelivery_Register]
     * Desktop: CommTradeLoadingDelivery.CommissionTradeLoadingDeliverySlipAndRegister
     */
    @RequestMapping(value = "/reports/print/556-commission-trade-loading-delivery-slip", method = RequestMethod.POST)
    public void print556CommissionTradeLoadingDeliverySlip(HttpServletResponse response, @RequestBody(required = false) Rpt556CommissionTradeLoadingDeliverySlipRequest request) throws Exception {
        if (request == null) request = new Rpt556CommissionTradeLoadingDeliverySlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "556-CommissionTradeLoadingDelivery_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/556-commission-trade-loading-delivery-slip", method = RequestMethod.GET)
    public void print556CommissionTradeLoadingDeliverySlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print556CommissionTradeLoadingDeliverySlip(response, objectMapper.convertValue(query, Rpt556CommissionTradeLoadingDeliverySlipRequest.class));
    }

    /**
     * Template: CommTradeLoadingDeliveryExpense_SubReport.rpt
     * Procedure: [CmTr].[USP_CommTradeLoadingDeliveryExpense_SubReport]
     * Desktop: CommTradeLoadingDelivery.CommTradeLoadingDeliveryExpense_SubReport
     */
    @RequestMapping(value = "/reports/print/comm-trade-loading-delivery-expense-sub-report", method = RequestMethod.POST)
    public void printCommTradeLoadingDeliveryExpenseSubReport(HttpServletResponse response, @RequestBody(required = false) RptCommTradeLoadingDeliveryExpenseSubReportRequest request) throws Exception {
        if (request == null) request = new RptCommTradeLoadingDeliveryExpenseSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "CommTradeLoadingDeliveryExpense_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/comm-trade-loading-delivery-expense-sub-report", method = RequestMethod.GET)
    public void printCommTradeLoadingDeliveryExpenseSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printCommTradeLoadingDeliveryExpenseSubReport(response, objectMapper.convertValue(query, RptCommTradeLoadingDeliveryExpenseSubReportRequest.class));
    }

    /**
     * Template: CommTradeLoadingDeliveryPMExpense_SubReport.rpt
     * Procedure: [CmTr].[USP_CommTradeLoadingDeliveryPMExpense_SubReport]
     * Desktop: CommTradeLoadingDelivery.CommTradeLoadingDeliveryPMExpense_SubReport
     */
    @RequestMapping(value = "/reports/print/comm-trade-loading-delivery-pm-expense-sub-report", method = RequestMethod.POST)
    public void printCommTradeLoadingDeliveryPMExpenseSubReport(HttpServletResponse response, @RequestBody(required = false) RptCommTradeLoadingDeliveryPMExpenseSubReportRequest request) throws Exception {
        if (request == null) request = new RptCommTradeLoadingDeliveryPMExpenseSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "CommTradeLoadingDeliveryPMExpense_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/comm-trade-loading-delivery-pm-expense-sub-report", method = RequestMethod.GET)
    public void printCommTradeLoadingDeliveryPMExpenseSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printCommTradeLoadingDeliveryPMExpenseSubReport(response, objectMapper.convertValue(query, RptCommTradeLoadingDeliveryPMExpenseSubReportRequest.class));
    }

    /**
     * Template: CommTradeTransactionPM_SubReport.rpt
     * Procedure: [CmTr].[USP_CommTradeTransactionPM_SubReport]
     * Desktop: CommTradeTransactionHeader.CommTradeTransactionPM_SubReport
     */
    @RequestMapping(value = "/reports/print/comm-trade-transaction-pm-sub-report", method = RequestMethod.POST)
    public void printCommTradeTransactionPMSubReport(HttpServletResponse response, @RequestBody(required = false) RptCommTradeTransactionPMSubReportRequest request) throws Exception {
        if (request == null) request = new RptCommTradeTransactionPMSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "CommTradeTransactionPM_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/comm-trade-transaction-pm-sub-report", method = RequestMethod.GET)
    public void printCommTradeTransactionPMSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printCommTradeTransactionPMSubReport(response, objectMapper.convertValue(query, RptCommTradeTransactionPMSubReportRequest.class));
    }

    /**
     * Template: InvCommissionAgentBillOtherExp.rpt
     * Procedure: SP_CommisionAgentBillOtherExpense_SubRpt
     * Desktop: InvCommAgentTradeBill.TradeBillSlipAndRegisterSupReprt
     */
    @RequestMapping(value = "/reports/print/inv-commission-agent-bill-other-exp", method = RequestMethod.POST)
    public void printInvCommissionAgentBillOtherExp(HttpServletResponse response, @RequestBody(required = false) RptInvCommissionAgentBillOtherExpRequest request) throws Exception {
        if (request == null) request = new RptInvCommissionAgentBillOtherExpRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "InvCommissionAgentBillOtherExp.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/inv-commission-agent-bill-other-exp", method = RequestMethod.GET)
    public void printInvCommissionAgentBillOtherExpGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printInvCommissionAgentBillOtherExp(response, objectMapper.convertValue(query, RptInvCommissionAgentBillOtherExpRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
