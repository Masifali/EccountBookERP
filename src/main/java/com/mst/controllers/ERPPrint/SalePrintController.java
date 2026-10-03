package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.SalePrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import javax.servlet.http.HttpServletResponse;
import java.util.Map;
import java.util.LinkedHashMap;
import org.springframework.http.*;

/** Sale print actions. Existing print URLs are preserved. */
@Controller
public class SalePrintController extends ReportPrintSupport {


    @RequestMapping(value = "/reports/sale_order_slip_273", method = {RequestMethod.GET, RequestMethod.POST})
    public void saleOrder273(HttpServletResponse response, @RequestParam Map<String, String> query,
            @RequestBody(required = false) Map<String, Object> body) throws Exception {
        Map<String, Object> request = new LinkedHashMap<>(query);
        if (body != null) request.putAll(body);
        ReportPdfService.require(request.get("id"), "id");
        printReportKey(response, "so-273", request);
    }


    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 260-InvRptGdnRiceSlip.rpt
     * Procedure: Sp_InvGdn_SlipAndRegisterRice_Rpt
     * Desktop: InvGrnandGdnReports.InvGdnSlip260
     */
    @RequestMapping(value = "/reports/print/260-gdn-rice-slip", method = RequestMethod.POST)
    public void print260GdnRiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt260GdnRiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt260GdnRiceSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "260-InvRptGdnRiceSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/260-gdn-rice-slip", method = RequestMethod.GET)
    public void print260GdnRiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print260GdnRiceSlip(response, objectMapper.convertValue(query, Rpt260GdnRiceSlipRequest.class));
    }

    /**
     * Template: 703_Gdn_PurchaseReturnSlip.rpt
     * Procedure: [dbo].[USP_InvGdn_PurchaseReturnReport]
     * Desktop: InvGrnandGdnReports.InvGdn_PurchaseReturnReport
     */
    @RequestMapping(value = "/reports/print/703-gdn-purchase-return-slip", method = RequestMethod.POST)
    public void print703GdnPurchaseReturnSlip(HttpServletResponse response, @RequestBody(required = false) Rpt703GdnPurchaseReturnSlipRequest request) throws Exception {
        if (request == null) request = new Rpt703GdnPurchaseReturnSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "703_Gdn_PurchaseReturnSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/703-gdn-purchase-return-slip", method = RequestMethod.GET)
    public void print703GdnPurchaseReturnSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print703GdnPurchaseReturnSlip(response, objectMapper.convertValue(query, Rpt703GdnPurchaseReturnSlipRequest.class));
    }

    /**
     * Template: 703A_Gdn_PurchaseReturnSlip.rpt
     * Procedure: [dbo].[USP_InvGdn_PurchaseReturnReport]
     * Desktop: InvGrnandGdnReports.InvGdn_PurchaseReturnReport
     */
    @RequestMapping(value = "/reports/print/703a-gdn-purchase-return-slip", method = RequestMethod.POST)
    public void print703AGdnPurchaseReturnSlip(HttpServletResponse response, @RequestBody(required = false) Rpt703AGdnPurchaseReturnSlipRequest request) throws Exception {
        if (request == null) request = new Rpt703AGdnPurchaseReturnSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "703A_Gdn_PurchaseReturnSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/703a-gdn-purchase-return-slip", method = RequestMethod.GET)
    public void print703AGdnPurchaseReturnSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print703AGdnPurchaseReturnSlip(response, objectMapper.convertValue(query, Rpt703AGdnPurchaseReturnSlipRequest.class));
    }

    /**
     * Template: 273-InvRptSaleOrderSlip.rpt
     * Procedure: Sp_SaleOrder_RiceSlip_Rpt
     * Desktop: SaleOrderReports.SaleOrderReports273
     */
    @RequestMapping(value = "/reports/print/273-sale-order-slip", method = RequestMethod.POST)
    public void print273SaleOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt273SaleOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt273SaleOrderSlipRequest();
        printReport(response, "273-InvRptSaleOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/273-sale-order-slip", method = RequestMethod.GET)
    public void print273SaleOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print273SaleOrderSlip(response, objectMapper.convertValue(query, Rpt273SaleOrderSlipRequest.class));
    }

    /**
     * Template: 273_01_SaleOrderSlip.rpt
     * Procedure: Sp_SaleOrder_RiceSlip_Rpt
     * Desktop: SaleOrderReports.SaleOrderReports273
     */
    @RequestMapping(value = "/reports/print/273-01-sale-order-slip", method = RequestMethod.POST)
    public void print27301SaleOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt27301SaleOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt27301SaleOrderSlipRequest();
        printReport(response, "273_01_SaleOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/273-01-sale-order-slip", method = RequestMethod.GET)
    public void print27301SaleOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print27301SaleOrderSlip(response, objectMapper.convertValue(query, Rpt27301SaleOrderSlipRequest.class));
    }

    /**
     * Template: 07_RptGatepassOutwardDriverInfoSlipA.rpt
     * Procedure: Sp_GatePassOutwardDriverInfo_rpt
     * Desktop: GatePassOutwardDriverInfo.GatePassOutwardDriverInfoRpt
     */
    @RequestMapping(value = "/reports/print/07-gatepass-outward-driver-info-slip-a", method = RequestMethod.POST)
    public void print07GatepassOutwardDriverInfoSlipA(HttpServletResponse response, @RequestBody(required = false) Rpt07GatepassOutwardDriverInfoSlipARequest request) throws Exception {
        if (request == null) request = new Rpt07GatepassOutwardDriverInfoSlipARequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "07_RptGatepassOutwardDriverInfoSlipA.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/07-gatepass-outward-driver-info-slip-a", method = RequestMethod.GET)
    public void print07GatepassOutwardDriverInfoSlipAGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print07GatepassOutwardDriverInfoSlipA(response, objectMapper.convertValue(query, Rpt07GatepassOutwardDriverInfoSlipARequest.class));
    }

    /**
     * Template: 258-OutwardGatePassWithWbAndLabSlip.rpt
     * Procedure: Sp_GatePassOutward_SlipAndRegister_Rpt
     * Desktop: GatePassOutwardReports.GatePassOutwardSlipandRegister
     */
    @RequestMapping(value = "/reports/print/258-outward-gate-pass-with-wb-and-lab-slip", method = RequestMethod.POST)
    public void print258OutwardGatePassWithWbAndLabSlip(HttpServletResponse response, @RequestBody(required = false) Rpt258OutwardGatePassWithWbAndLabSlipRequest request) throws Exception {
        if (request == null) request = new Rpt258OutwardGatePassWithWbAndLabSlipRequest();
        printReport(response, "258-OutwardGatePassWithWbAndLabSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/258-outward-gate-pass-with-wb-and-lab-slip", method = RequestMethod.GET)
    public void print258OutwardGatePassWithWbAndLabSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print258OutwardGatePassWithWbAndLabSlip(response, objectMapper.convertValue(query, Rpt258OutwardGatePassWithWbAndLabSlipRequest.class));
    }

    /**
     * Template: 1856-SaleInvoiceDirect_Slip.rpt
     * Procedure: pcc.USP_InvSaleInvoice_DirectSlip
     * Desktop: InvSaleInvoice.SaleInvoiceDirectSlip
     */
    @RequestMapping(value = "/reports/print/1856-sale-invoice-direct-slip", method = RequestMethod.POST)
    public void print1856SaleInvoiceDirectSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1856SaleInvoiceDirectSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1856SaleInvoiceDirectSlipRequest();
        printReport(response, "1856-SaleInvoiceDirect_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1856-sale-invoice-direct-slip", method = RequestMethod.GET)
    public void print1856SaleInvoiceDirectSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1856SaleInvoiceDirectSlip(response, objectMapper.convertValue(query, Rpt1856SaleInvoiceDirectSlipRequest.class));
    }

    /**
     * Template: 1862-SaleInvoiceRetrurn_Slip.rpt
     * Procedure: pcc.USP_InvSaleInvoice_DirectSlip
     * Desktop: InvSaleInvoice.SaleInvoiceDirectSlip
     */
    @RequestMapping(value = "/reports/print/1862-sale-invoice-retrurn-slip", method = RequestMethod.POST)
    public void print1862SaleInvoiceRetrurnSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1862SaleInvoiceRetrurnSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1862SaleInvoiceRetrurnSlipRequest();
        printReport(response, "1862-SaleInvoiceRetrurn_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1862-sale-invoice-retrurn-slip", method = RequestMethod.GET)
    public void print1862SaleInvoiceRetrurnSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1862SaleInvoiceRetrurnSlip(response, objectMapper.convertValue(query, Rpt1862SaleInvoiceRetrurnSlipRequest.class));
    }

    /**
     * Template: 1200-DeliveryOrderSlipReturnable.rpt
     * Procedure: USP_DeliveryOrder_Slip_Engr
     * Desktop: InvDeliveryOrder.InvDeliveryOrderSlip_Engr
     */
    @RequestMapping(value = "/reports/print/1200-delivery-order-slip-returnable", method = RequestMethod.POST)
    public void print1200DeliveryOrderSlipReturnable(HttpServletResponse response, @RequestBody(required = false) Rpt1200DeliveryOrderSlipReturnableRequest request) throws Exception {
        if (request == null) request = new Rpt1200DeliveryOrderSlipReturnableRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1200-DeliveryOrderSlipReturnable.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1200-delivery-order-slip-returnable", method = RequestMethod.GET)
    public void print1200DeliveryOrderSlipReturnableGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1200DeliveryOrderSlipReturnable(response, objectMapper.convertValue(query, Rpt1200DeliveryOrderSlipReturnableRequest.class));
    }

    /**
     * Template: 1201_OutwardGatePassReturnable_Slip.rpt
     * Procedure: Sp_GatePassOutward_SlipAndRegister_Rpt
     * Desktop: GatePassOutwardReports.GatePassOutwardSlipandRegister
     */
    @RequestMapping(value = "/reports/print/1201-outward-gate-pass-returnable-slip", method = RequestMethod.POST)
    public void print1201OutwardGatePassReturnableSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1201OutwardGatePassReturnableSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1201OutwardGatePassReturnableSlipRequest();
        printReport(response, "1201_OutwardGatePassReturnable_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1201-outward-gate-pass-returnable-slip", method = RequestMethod.GET)
    public void print1201OutwardGatePassReturnableSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1201OutwardGatePassReturnableSlip(response, objectMapper.convertValue(query, Rpt1201OutwardGatePassReturnableSlipRequest.class));
    }

    /**
     * Template: 122_03_SaleAnalyticsItemWise.rpt
     * Procedure: spInventoryStockEvaluation_LocalSalesComaprisons_Report
     * Desktop: InvSaleInvoiceReports.InventoryStockEvaluation_LocalSalesComaprisons_Report
     */
    @RequestMapping(value = "/reports/print/122-03-sale-analytics-item-wise", method = RequestMethod.POST)
    public void print12203SaleAnalyticsItemWise(HttpServletResponse response, @RequestBody(required = false) Rpt12203SaleAnalyticsItemWiseRequest request) throws Exception {
        if (request == null) request = new Rpt12203SaleAnalyticsItemWiseRequest();
        printReport(response, "122_03_SaleAnalyticsItemWise.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/122-03-sale-analytics-item-wise", method = RequestMethod.GET)
    public void print12203SaleAnalyticsItemWiseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print12203SaleAnalyticsItemWise(response, objectMapper.convertValue(query, Rpt12203SaleAnalyticsItemWiseRequest.class));
    }

    /**
     * Template: 122_04_SaleAnalyticsCustomerWise.rpt
     * Procedure: spInventoryStockEvaluation_LocalSalesComaprisons_Report
     * Desktop: InvSaleInvoiceReports.InventoryStockEvaluation_LocalSalesComaprisons_Report
     */
    @RequestMapping(value = "/reports/print/122-04-sale-analytics-customer-wise", method = RequestMethod.POST)
    public void print12204SaleAnalyticsCustomerWise(HttpServletResponse response, @RequestBody(required = false) Rpt12204SaleAnalyticsCustomerWiseRequest request) throws Exception {
        if (request == null) request = new Rpt12204SaleAnalyticsCustomerWiseRequest();
        printReport(response, "122_04_SaleAnalyticsCustomerWise.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/122-04-sale-analytics-customer-wise", method = RequestMethod.GET)
    public void print12204SaleAnalyticsCustomerWiseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print12204SaleAnalyticsCustomerWise(response, objectMapper.convertValue(query, Rpt12204SaleAnalyticsCustomerWiseRequest.class));
    }

    /**
     * Template: 146-OutstandingOrderWithLedgerBalance.rpt
     * Procedure: USP_GetOutstandingOrdersWithLedgerBalance
     * Desktop: SaleOrder.GetOutstandingOrdersWithLedgerBalance
     */
    @RequestMapping(value = "/reports/print/146-outstanding-order-with-ledger-balance", method = RequestMethod.POST)
    public void print146OutstandingOrderWithLedgerBalance(HttpServletResponse response, @RequestBody(required = false) Rpt146OutstandingOrderWithLedgerBalanceRequest request) throws Exception {
        if (request == null) request = new Rpt146OutstandingOrderWithLedgerBalanceRequest();
        printReport(response, "146-OutstandingOrderWithLedgerBalance.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/146-outstanding-order-with-ledger-balance", method = RequestMethod.GET)
    public void print146OutstandingOrderWithLedgerBalanceGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print146OutstandingOrderWithLedgerBalance(response, objectMapper.convertValue(query, Rpt146OutstandingOrderWithLedgerBalanceRequest.class));
    }

    /**
     * Template: 1605-SaleOrderSlipAndRegister_Engr.rpt
     * Procedure: USP_SaleOrderSlipAndRegister_Engr
     * Desktop: SaleOrder.SaleOrderSlipAndRegister_Engr
     */
    @RequestMapping(value = "/reports/print/1605-sale-order-slip-and-register-engr", method = RequestMethod.POST)
    public void print1605SaleOrderSlipAndRegisterEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1605SaleOrderSlipAndRegisterEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1605SaleOrderSlipAndRegisterEngrRequest();
        printReport(response, "1605-SaleOrderSlipAndRegister_Engr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1605-sale-order-slip-and-register-engr", method = RequestMethod.GET)
    public void print1605SaleOrderSlipAndRegisterEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1605SaleOrderSlipAndRegisterEngr(response, objectMapper.convertValue(query, Rpt1605SaleOrderSlipAndRegisterEngrRequest.class));
    }

    /**
     * Template: 1605A-InvRptSalesOrderRegister.rpt
     * Procedure: USP_SaleOrderSlipAndRegister_Engr
     * Desktop: SaleOrder.SaleOrderSlipAndRegister_Engr
     */
    @RequestMapping(value = "/reports/print/1605a-sales-order-register", method = RequestMethod.POST)
    public void print1605ASalesOrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1605ASalesOrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1605ASalesOrderRegisterRequest();
        printReport(response, "1605A-InvRptSalesOrderRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1605a-sales-order-register", method = RequestMethod.GET)
    public void print1605ASalesOrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1605ASalesOrderRegister(response, objectMapper.convertValue(query, Rpt1605ASalesOrderRegisterRequest.class));
    }

    /**
     * Template: 1605B-InvRptSaleOderRegister.rpt
     * Procedure: USP_SaleOrderSlipAndRegister_Engr
     * Desktop: SaleOrder.SaleOrderSlipAndRegister_Engr
     */
    @RequestMapping(value = "/reports/print/1605b-sale-oder-register", method = RequestMethod.POST)
    public void print1605BSaleOderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1605BSaleOderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1605BSaleOderRegisterRequest();
        printReport(response, "1605B-InvRptSaleOderRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1605b-sale-oder-register", method = RequestMethod.GET)
    public void print1605BSaleOderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1605BSaleOderRegister(response, objectMapper.convertValue(query, Rpt1605BSaleOderRegisterRequest.class));
    }

    /**
     * Template: 1606-DeliveryOrderSlip_Engr.rpt
     * Procedure: USP_DeliveryOrder_Slip_Engr
     * Desktop: InvDeliveryOrder.InvDeliveryOrderSlip_Engr
     */
    @RequestMapping(value = "/reports/print/1606-delivery-order-slip-engr", method = RequestMethod.POST)
    public void print1606DeliveryOrderSlipEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1606DeliveryOrderSlipEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1606DeliveryOrderSlipEngrRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1606-DeliveryOrderSlip_Engr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1606-delivery-order-slip-engr", method = RequestMethod.GET)
    public void print1606DeliveryOrderSlipEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1606DeliveryOrderSlipEngr(response, objectMapper.convertValue(query, Rpt1606DeliveryOrderSlipEngrRequest.class));
    }

    /**
     * Template: 1606A-InvDeliveryOrderForApproval_Register.rpt
     * Procedure: USP_InvDeliveryOrderForApproval_Engr
     * Desktop: InvDeliveryOrder.DeliveryOrderForApproval_Engr
     */
    @RequestMapping(value = "/reports/print/1606a-inv-delivery-order-for-approval-register", method = RequestMethod.POST)
    public void print1606AInvDeliveryOrderForApprovalRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1606AInvDeliveryOrderForApprovalRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1606AInvDeliveryOrderForApprovalRegisterRequest();
        printReport(response, "1606A-InvDeliveryOrderForApproval_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1606a-inv-delivery-order-for-approval-register", method = RequestMethod.GET)
    public void print1606AInvDeliveryOrderForApprovalRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1606AInvDeliveryOrderForApprovalRegister(response, objectMapper.convertValue(query, Rpt1606AInvDeliveryOrderForApprovalRegisterRequest.class));
    }

    /**
     * Template: 1608-SaleInvoice_CustomerBillDirect_Engr.rpt
     * Procedure: USP_SaleInvoice_CustomerBillDirect_Engr
     * Desktop: InvSaleInvoiceReports.InvSaleInvoiceDirectSlip
     */
    @RequestMapping(value = "/reports/print/1608-sale-invoice-customer-bill-direct-engr", method = RequestMethod.POST)
    public void print1608SaleInvoiceCustomerBillDirectEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1608SaleInvoiceCustomerBillDirectEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1608SaleInvoiceCustomerBillDirectEngrRequest();
        printReport(response, "1608-SaleInvoice_CustomerBillDirect_Engr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1608-sale-invoice-customer-bill-direct-engr", method = RequestMethod.GET)
    public void print1608SaleInvoiceCustomerBillDirectEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1608SaleInvoiceCustomerBillDirectEngr(response, objectMapper.convertValue(query, Rpt1608SaleInvoiceCustomerBillDirectEngrRequest.class));
    }

    /**
     * Template: 1608A-SaleInvoice_CustomerBillDirect_Engr.rpt
     * Procedure: USP_SaleInvoice_CustomerBillDirect_Engr
     * Desktop: InvSaleInvoiceReports.InvSaleInvoiceDirectSlip
     */
    @RequestMapping(value = "/reports/print/1608a-sale-invoice-customer-bill-direct-engr", method = RequestMethod.POST)
    public void print1608ASaleInvoiceCustomerBillDirectEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1608ASaleInvoiceCustomerBillDirectEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1608ASaleInvoiceCustomerBillDirectEngrRequest();
        printReport(response, "1608A-SaleInvoice_CustomerBillDirect_Engr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1608a-sale-invoice-customer-bill-direct-engr", method = RequestMethod.GET)
    public void print1608ASaleInvoiceCustomerBillDirectEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1608ASaleInvoiceCustomerBillDirectEngr(response, objectMapper.convertValue(query, Rpt1608ASaleInvoiceCustomerBillDirectEngrRequest.class));
    }

    /**
     * Template: 1609-InvRepSaleBillCustomer.rpt
     * Procedure: [dbo].[Sp_InvSaleInvoice_Slip_Eng]
     * Desktop: InvSaleInvoice.SaleInvoiceSlip_Engr
     */
    @RequestMapping(value = "/reports/print/1609-inv-rep-sale-bill-customer", method = RequestMethod.POST)
    public void print1609InvRepSaleBillCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt1609InvRepSaleBillCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt1609InvRepSaleBillCustomerRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1609-InvRepSaleBillCustomer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1609-inv-rep-sale-bill-customer", method = RequestMethod.GET)
    public void print1609InvRepSaleBillCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1609InvRepSaleBillCustomer(response, objectMapper.convertValue(query, Rpt1609InvRepSaleBillCustomerRequest.class));
    }

    /**
     * Template: 1609A-InvRepSaleBillCustomer-Format-II.rpt
     * Procedure: [dbo].[Sp_InvSaleInvoice_Slip_Eng]
     * Desktop: InvSaleInvoice.SaleInvoiceSlip_Engr
     */
    @RequestMapping(value = "/reports/print/1609a-inv-rep-sale-bill-customer-format-ii", method = RequestMethod.POST)
    public void print1609AInvRepSaleBillCustomerFormatII(HttpServletResponse response, @RequestBody(required = false) Rpt1609AInvRepSaleBillCustomerFormatIIRequest request) throws Exception {
        if (request == null) request = new Rpt1609AInvRepSaleBillCustomerFormatIIRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1609A-InvRepSaleBillCustomer-Format-II.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1609a-inv-rep-sale-bill-customer-format-ii", method = RequestMethod.GET)
    public void print1609AInvRepSaleBillCustomerFormatIIGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1609AInvRepSaleBillCustomerFormatII(response, objectMapper.convertValue(query, Rpt1609AInvRepSaleBillCustomerFormatIIRequest.class));
    }

    /**
     * Template: 1611-SaleInvoice_CustomerBillReturn_Engr.rpt
     * Procedure: USP_SaleInvoice_CustomerBillDirect_Engr
     * Desktop: InvSaleInvoiceReports.InvSaleInvoiceDirectSlip
     */
    @RequestMapping(value = "/reports/print/1611-sale-invoice-customer-bill-return-engr", method = RequestMethod.POST)
    public void print1611SaleInvoiceCustomerBillReturnEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1611SaleInvoiceCustomerBillReturnEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1611SaleInvoiceCustomerBillReturnEngrRequest();
        printReport(response, "1611-SaleInvoice_CustomerBillReturn_Engr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1611-sale-invoice-customer-bill-return-engr", method = RequestMethod.GET)
    public void print1611SaleInvoiceCustomerBillReturnEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1611SaleInvoiceCustomerBillReturnEngr(response, objectMapper.convertValue(query, Rpt1611SaleInvoiceCustomerBillReturnEngrRequest.class));
    }

    /**
     * Template: 1611A-SaleInvoice_CustomerBillReturn_Engr.rpt
     * Procedure: USP_SaleInvoice_CustomerBillDirect_Engr
     * Desktop: InvSaleInvoiceReports.InvSaleInvoiceDirectSlip
     */
    @RequestMapping(value = "/reports/print/1611a-sale-invoice-customer-bill-return-engr", method = RequestMethod.POST)
    public void print1611ASaleInvoiceCustomerBillReturnEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1611ASaleInvoiceCustomerBillReturnEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1611ASaleInvoiceCustomerBillReturnEngrRequest();
        printReport(response, "1611A-SaleInvoice_CustomerBillReturn_Engr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1611a-sale-invoice-customer-bill-return-engr", method = RequestMethod.GET)
    public void print1611ASaleInvoiceCustomerBillReturnEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1611ASaleInvoiceCustomerBillReturnEngr(response, objectMapper.convertValue(query, Rpt1611ASaleInvoiceCustomerBillReturnEngrRequest.class));
    }

    /**
     * Template: 1612-GdnRegister_Eng.rpt
     * Procedure: [dbo].[USP_GdnRegister_Eng]
     * Desktop: InvGdn.GdnRegister_Eng
     */
    @RequestMapping(value = "/reports/print/1612-gdn-register-eng", method = RequestMethod.POST)
    public void print1612GdnRegisterEng(HttpServletResponse response, @RequestBody(required = false) Rpt1612GdnRegisterEngRequest request) throws Exception {
        if (request == null) request = new Rpt1612GdnRegisterEngRequest();
        printReport(response, "1612-GdnRegister_Eng.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1612-gdn-register-eng", method = RequestMethod.GET)
    public void print1612GdnRegisterEngGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1612GdnRegisterEng(response, objectMapper.convertValue(query, Rpt1612GdnRegisterEngRequest.class));
    }

    /**
     * Template: 1656_SaleOrderSlip_Engr.rpt
     * Procedure: USP_SaleOrderSlipAndRegister_Engr
     * Desktop: SaleOrder.SaleOrderSlipAndRegister_Engr
     */
    @RequestMapping(value = "/reports/print/1656-sale-order-slip-engr", method = RequestMethod.POST)
    public void print1656SaleOrderSlipEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1656SaleOrderSlipEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1656SaleOrderSlipEngrRequest();
        printReport(response, "1656_SaleOrderSlip_Engr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1656-sale-order-slip-engr", method = RequestMethod.GET)
    public void print1656SaleOrderSlipEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1656SaleOrderSlipEngr(response, objectMapper.convertValue(query, Rpt1656SaleOrderSlipEngrRequest.class));
    }

    /**
     * Template: 1656A-InvRptSalesOrderRegister.rpt
     * Procedure: USP_SaleOrderSlipAndRegister_Engr
     * Desktop: SaleOrder.SaleOrderSlipAndRegister_Engr
     */
    @RequestMapping(value = "/reports/print/1656a-sales-order-register", method = RequestMethod.POST)
    public void print1656ASalesOrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1656ASalesOrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1656ASalesOrderRegisterRequest();
        printReport(response, "1656A-InvRptSalesOrderRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1656a-sales-order-register", method = RequestMethod.GET)
    public void print1656ASalesOrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1656ASalesOrderRegister(response, objectMapper.convertValue(query, Rpt1656ASalesOrderRegisterRequest.class));
    }

    /**
     * Template: 1656B-InvRptSaleOderRegister.rpt
     * Procedure: USP_SaleOrderSlipAndRegister_Engr
     * Desktop: SaleOrder.SaleOrderSlipAndRegister_Engr
     */
    @RequestMapping(value = "/reports/print/1656b-sale-oder-register", method = RequestMethod.POST)
    public void print1656BSaleOderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1656BSaleOderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1656BSaleOderRegisterRequest();
        printReport(response, "1656B-InvRptSaleOderRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1656b-sale-oder-register", method = RequestMethod.GET)
    public void print1656BSaleOderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1656BSaleOderRegister(response, objectMapper.convertValue(query, Rpt1656BSaleOderRegisterRequest.class));
    }

    /**
     * Template: 1657_DeliveryOrderSlip_Engr.rpt
     * Procedure: USP_DeliveryOrder_Slip_Engr
     * Desktop: InvDeliveryOrder.InvDeliveryOrderSlip_Engr
     */
    @RequestMapping(value = "/reports/print/1657-delivery-order-slip-engr", method = RequestMethod.POST)
    public void print1657DeliveryOrderSlipEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1657DeliveryOrderSlipEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1657DeliveryOrderSlipEngrRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1657_DeliveryOrderSlip_Engr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1657-delivery-order-slip-engr", method = RequestMethod.GET)
    public void print1657DeliveryOrderSlipEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1657DeliveryOrderSlipEngr(response, objectMapper.convertValue(query, Rpt1657DeliveryOrderSlipEngrRequest.class));
    }

    /**
     * Template: 1657A-InvDeliveryOrderForApproval_Register.rpt
     * Procedure: USP_InvDeliveryOrderForApproval_Engr
     * Desktop: InvDeliveryOrder.DeliveryOrderForApproval_Engr
     */
    @RequestMapping(value = "/reports/print/1657a-inv-delivery-order-for-approval-register", method = RequestMethod.POST)
    public void print1657AInvDeliveryOrderForApprovalRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1657AInvDeliveryOrderForApprovalRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1657AInvDeliveryOrderForApprovalRegisterRequest();
        printReport(response, "1657A-InvDeliveryOrderForApproval_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1657a-inv-delivery-order-for-approval-register", method = RequestMethod.GET)
    public void print1657AInvDeliveryOrderForApprovalRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1657AInvDeliveryOrderForApprovalRegister(response, objectMapper.convertValue(query, Rpt1657AInvDeliveryOrderForApprovalRegisterRequest.class));
    }

    /**
     * Template: 1660_SaleInvoiceSlipEngr.rpt
     * Procedure: [dbo].[Sp_InvSaleInvoice_Slip_Eng]
     * Desktop: InvSaleInvoice.SaleInvoiceSlip_Engr
     */
    @RequestMapping(value = "/reports/print/1660-sale-invoice-slip-engr", method = RequestMethod.POST)
    public void print1660SaleInvoiceSlipEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1660SaleInvoiceSlipEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1660SaleInvoiceSlipEngrRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1660_SaleInvoiceSlipEngr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1660-sale-invoice-slip-engr", method = RequestMethod.GET)
    public void print1660SaleInvoiceSlipEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1660SaleInvoiceSlipEngr(response, objectMapper.convertValue(query, Rpt1660SaleInvoiceSlipEngrRequest.class));
    }

    /**
     * Template: 1660A_SaleInvoiceSlipEngr.rpt
     * Procedure: [dbo].[Sp_InvSaleInvoice_Slip_Eng]
     * Desktop: InvSaleInvoice.SaleInvoiceSlip_Engr
     */
    @RequestMapping(value = "/reports/print/1660a-sale-invoice-slip-engr", method = RequestMethod.POST)
    public void print1660ASaleInvoiceSlipEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1660ASaleInvoiceSlipEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1660ASaleInvoiceSlipEngrRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1660A_SaleInvoiceSlipEngr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1660a-sale-invoice-slip-engr", method = RequestMethod.GET)
    public void print1660ASaleInvoiceSlipEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1660ASaleInvoiceSlipEngr(response, objectMapper.convertValue(query, Rpt1660ASaleInvoiceSlipEngrRequest.class));
    }

    /**
     * Template: 1661_SaleInvoice_CustomerBillDirectSlip_Engr.rpt
     * Procedure: USP_SaleInvoice_CustomerBillDirect_Engr
     * Desktop: InvSaleInvoiceReports.InvSaleInvoiceDirectSlip
     */
    @RequestMapping(value = "/reports/print/1661-sale-invoice-customer-bill-direct-slip-engr", method = RequestMethod.POST)
    public void print1661SaleInvoiceCustomerBillDirectSlipEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1661SaleInvoiceCustomerBillDirectSlipEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1661SaleInvoiceCustomerBillDirectSlipEngrRequest();
        printReport(response, "1661_SaleInvoice_CustomerBillDirectSlip_Engr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1661-sale-invoice-customer-bill-direct-slip-engr", method = RequestMethod.GET)
    public void print1661SaleInvoiceCustomerBillDirectSlipEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1661SaleInvoiceCustomerBillDirectSlipEngr(response, objectMapper.convertValue(query, Rpt1661SaleInvoiceCustomerBillDirectSlipEngrRequest.class));
    }

    /**
     * Template: 1661A_SaleInvoice_CustomerBillDirectSlip_Engr.rpt
     * Procedure: USP_SaleInvoice_CustomerBillDirect_Engr
     * Desktop: InvSaleInvoiceReports.InvSaleInvoiceDirectSlip
     */
    @RequestMapping(value = "/reports/print/1661a-sale-invoice-customer-bill-direct-slip-engr", method = RequestMethod.POST)
    public void print1661ASaleInvoiceCustomerBillDirectSlipEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1661ASaleInvoiceCustomerBillDirectSlipEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1661ASaleInvoiceCustomerBillDirectSlipEngrRequest();
        printReport(response, "1661A_SaleInvoice_CustomerBillDirectSlip_Engr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1661a-sale-invoice-customer-bill-direct-slip-engr", method = RequestMethod.GET)
    public void print1661ASaleInvoiceCustomerBillDirectSlipEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1661ASaleInvoiceCustomerBillDirectSlipEngr(response, objectMapper.convertValue(query, Rpt1661ASaleInvoiceCustomerBillDirectSlipEngrRequest.class));
    }

    /**
     * Template: 1662_SaleInvoiceReturn_Slip_Engr.rpt
     * Procedure: USP_SaleInvoice_CustomerBillDirect_Engr
     * Desktop: InvSaleInvoiceReports.InvSaleInvoiceDirectSlip
     */
    @RequestMapping(value = "/reports/print/1662-sale-invoice-return-slip-engr", method = RequestMethod.POST)
    public void print1662SaleInvoiceReturnSlipEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1662SaleInvoiceReturnSlipEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1662SaleInvoiceReturnSlipEngrRequest();
        printReport(response, "1662_SaleInvoiceReturn_Slip_Engr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1662-sale-invoice-return-slip-engr", method = RequestMethod.GET)
    public void print1662SaleInvoiceReturnSlipEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1662SaleInvoiceReturnSlipEngr(response, objectMapper.convertValue(query, Rpt1662SaleInvoiceReturnSlipEngrRequest.class));
    }

    /**
     * Template: 1662A_SaleInvoiceReturn_Slip_Engr.rpt
     * Procedure: USP_SaleInvoice_CustomerBillDirect_Engr
     * Desktop: InvSaleInvoiceReports.InvSaleInvoiceDirectSlip
     */
    @RequestMapping(value = "/reports/print/1662a-sale-invoice-return-slip-engr", method = RequestMethod.POST)
    public void print1662ASaleInvoiceReturnSlipEngr(HttpServletResponse response, @RequestBody(required = false) Rpt1662ASaleInvoiceReturnSlipEngrRequest request) throws Exception {
        if (request == null) request = new Rpt1662ASaleInvoiceReturnSlipEngrRequest();
        printReport(response, "1662A_SaleInvoiceReturn_Slip_Engr.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1662a-sale-invoice-return-slip-engr", method = RequestMethod.GET)
    public void print1662ASaleInvoiceReturnSlipEngrGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1662ASaleInvoiceReturnSlipEngr(response, objectMapper.convertValue(query, Rpt1662ASaleInvoiceReturnSlipEngrRequest.class));
    }

    /**
     * Template: 1809-SaleInvoiceDirectSlip_ForSalt.rpt
     * Procedure: [dbo].[USP_SaleInvoiceDirectSlip_ForSalt]
     * Desktop: InvSaleInvoiceReports.SaleInvoiceDirectSlip_ForSalt
     */
    @RequestMapping(value = "/reports/print/1809-sale-invoice-direct-slip-for-salt", method = RequestMethod.POST)
    public void print1809SaleInvoiceDirectSlipForSalt(HttpServletResponse response, @RequestBody(required = false) Rpt1809SaleInvoiceDirectSlipForSaltRequest request) throws Exception {
        if (request == null) request = new Rpt1809SaleInvoiceDirectSlipForSaltRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1809-SaleInvoiceDirectSlip_ForSalt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1809-sale-invoice-direct-slip-for-salt", method = RequestMethod.GET)
    public void print1809SaleInvoiceDirectSlipForSaltGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1809SaleInvoiceDirectSlipForSalt(response, objectMapper.convertValue(query, Rpt1809SaleInvoiceDirectSlipForSaltRequest.class));
    }

    /**
     * Template: 1809_01_SalesRegisterSummary.rpt
     * Procedure: [dbo].[USP_SalesFromEvaulation_RegisterSalt]
     * Desktop: InventoryStockEvalautionDetail.SalesFromEvaulation_RegisterSalt
     */
    @RequestMapping(value = "/reports/print/1809-01-sales-register-summary", method = RequestMethod.POST)
    public void print180901SalesRegisterSummary(HttpServletResponse response, @RequestBody(required = false) Rpt180901SalesRegisterSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt180901SalesRegisterSummaryRequest();
        printReport(response, "1809_01_SalesRegisterSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1809-01-sales-register-summary", method = RequestMethod.GET)
    public void print180901SalesRegisterSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180901SalesRegisterSummary(response, objectMapper.convertValue(query, Rpt180901SalesRegisterSummaryRequest.class));
    }

    /**
     * Template: 1809_02-SalesRegisterSummaryByCustomer.rpt
     * Procedure: [dbo].[USP_SalesFromEvaulation_RegisterSalt]
     * Desktop: InventoryStockEvalautionDetail.SalesFromEvaulation_RegisterSalt
     */
    @RequestMapping(value = "/reports/print/1809-02-sales-register-summary-by-customer", method = RequestMethod.POST)
    public void print180902SalesRegisterSummaryByCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt180902SalesRegisterSummaryByCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt180902SalesRegisterSummaryByCustomerRequest();
        printReport(response, "1809_02-SalesRegisterSummaryByCustomer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1809-02-sales-register-summary-by-customer", method = RequestMethod.GET)
    public void print180902SalesRegisterSummaryByCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180902SalesRegisterSummaryByCustomer(response, objectMapper.convertValue(query, Rpt180902SalesRegisterSummaryByCustomerRequest.class));
    }

    /**
     * Template: 1809_03-SalesRegisterSummaryByItem.rpt
     * Procedure: [dbo].[USP_SalesFromEvaulation_RegisterSalt]
     * Desktop: InventoryStockEvalautionDetail.SalesFromEvaulation_RegisterSalt
     */
    @RequestMapping(value = "/reports/print/1809-03-sales-register-summary-by-item", method = RequestMethod.POST)
    public void print180903SalesRegisterSummaryByItem(HttpServletResponse response, @RequestBody(required = false) Rpt180903SalesRegisterSummaryByItemRequest request) throws Exception {
        if (request == null) request = new Rpt180903SalesRegisterSummaryByItemRequest();
        printReport(response, "1809_03-SalesRegisterSummaryByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1809-03-sales-register-summary-by-item", method = RequestMethod.GET)
    public void print180903SalesRegisterSummaryByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180903SalesRegisterSummaryByItem(response, objectMapper.convertValue(query, Rpt180903SalesRegisterSummaryByItemRequest.class));
    }

    /**
     * Template: 1809_04-SalesRegisterSummaryByCustomer&Item.rpt
     * Procedure: [dbo].[USP_SalesFromEvaulation_RegisterSalt]
     * Desktop: InventoryStockEvalautionDetail.SalesFromEvaulation_RegisterSalt
     */
    @RequestMapping(value = "/reports/print/1809-04-sales-register-summary-by-customer-item", method = RequestMethod.POST)
    public void print180904SalesRegisterSummaryByCustomerItem(HttpServletResponse response, @RequestBody(required = false) Rpt180904SalesRegisterSummaryByCustomerItemRequest request) throws Exception {
        if (request == null) request = new Rpt180904SalesRegisterSummaryByCustomerItemRequest();
        printReport(response, "1809_04-SalesRegisterSummaryByCustomer&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1809-04-sales-register-summary-by-customer-item", method = RequestMethod.GET)
    public void print180904SalesRegisterSummaryByCustomerItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180904SalesRegisterSummaryByCustomerItem(response, objectMapper.convertValue(query, Rpt180904SalesRegisterSummaryByCustomerItemRequest.class));
    }

    /**
     * Template: 1809_05-SalesRegisterSummaryByItemWithoutPacking.rpt
     * Procedure: [dbo].[USP_SalesFromEvaulation_RegisterSalt]
     * Desktop: InventoryStockEvalautionDetail.SalesFromEvaulation_RegisterSalt
     */
    @RequestMapping(value = "/reports/print/1809-05-sales-register-summary-by-item-without-packing", method = RequestMethod.POST)
    public void print180905SalesRegisterSummaryByItemWithoutPacking(HttpServletResponse response, @RequestBody(required = false) Rpt180905SalesRegisterSummaryByItemWithoutPackingRequest request) throws Exception {
        if (request == null) request = new Rpt180905SalesRegisterSummaryByItemWithoutPackingRequest();
        printReport(response, "1809_05-SalesRegisterSummaryByItemWithoutPacking.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1809-05-sales-register-summary-by-item-without-packing", method = RequestMethod.GET)
    public void print180905SalesRegisterSummaryByItemWithoutPackingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180905SalesRegisterSummaryByItemWithoutPacking(response, objectMapper.convertValue(query, Rpt180905SalesRegisterSummaryByItemWithoutPackingRequest.class));
    }

    /**
     * Template: 1809_06-SalesRegisterSummaryByWarehouse.rpt
     * Procedure: [dbo].[USP_SalesFromEvaulation_RegisterSalt]
     * Desktop: InventoryStockEvalautionDetail.SalesFromEvaulation_RegisterSalt
     */
    @RequestMapping(value = "/reports/print/1809-06-sales-register-summary-by-warehouse", method = RequestMethod.POST)
    public void print180906SalesRegisterSummaryByWarehouse(HttpServletResponse response, @RequestBody(required = false) Rpt180906SalesRegisterSummaryByWarehouseRequest request) throws Exception {
        if (request == null) request = new Rpt180906SalesRegisterSummaryByWarehouseRequest();
        printReport(response, "1809_06-SalesRegisterSummaryByWarehouse.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1809-06-sales-register-summary-by-warehouse", method = RequestMethod.GET)
    public void print180906SalesRegisterSummaryByWarehouseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180906SalesRegisterSummaryByWarehouse(response, objectMapper.convertValue(query, Rpt180906SalesRegisterSummaryByWarehouseRequest.class));
    }

    /**
     * Template: 1809_07-SalesSummaryByCustomer&City.rpt
     * Procedure: [dbo].[USP_SalesFromEvaulation_RegisterSalt]
     * Desktop: InventoryStockEvalautionDetail.SalesFromEvaulation_RegisterSalt
     */
    @RequestMapping(value = "/reports/print/1809-07-sales-summary-by-customer-city", method = RequestMethod.POST)
    public void print180907SalesSummaryByCustomerCity(HttpServletResponse response, @RequestBody(required = false) Rpt180907SalesSummaryByCustomerCityRequest request) throws Exception {
        if (request == null) request = new Rpt180907SalesSummaryByCustomerCityRequest();
        printReport(response, "1809_07-SalesSummaryByCustomer&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1809-07-sales-summary-by-customer-city", method = RequestMethod.GET)
    public void print180907SalesSummaryByCustomerCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180907SalesSummaryByCustomerCity(response, objectMapper.convertValue(query, Rpt180907SalesSummaryByCustomerCityRequest.class));
    }

    /**
     * Template: 1809_09-SalesSummaryByCustomerItem&City.rpt
     * Procedure: [dbo].[USP_SalesFromEvaulation_RegisterSalt]
     * Desktop: InventoryStockEvalautionDetail.SalesFromEvaulation_RegisterSalt
     */
    @RequestMapping(value = "/reports/print/1809-09-sales-summary-by-customer-item-city", method = RequestMethod.POST)
    public void print180909SalesSummaryByCustomerItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt180909SalesSummaryByCustomerItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt180909SalesSummaryByCustomerItemCityRequest();
        printReport(response, "1809_09-SalesSummaryByCustomerItem&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1809-09-sales-summary-by-customer-item-city", method = RequestMethod.GET)
    public void print180909SalesSummaryByCustomerItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180909SalesSummaryByCustomerItemCity(response, objectMapper.convertValue(query, Rpt180909SalesSummaryByCustomerItemCityRequest.class));
    }

    /**
     * Template: 1809_10-SalesSummaryByCustomer&PackSize.rpt
     * Procedure: [dbo].[USP_SalesFromEvaulation_RegisterSalt]
     * Desktop: InventoryStockEvalautionDetail.SalesFromEvaulation_RegisterSalt
     */
    @RequestMapping(value = "/reports/print/1809-10-sales-summary-by-customer-pack-size", method = RequestMethod.POST)
    public void print180910SalesSummaryByCustomerPackSize(HttpServletResponse response, @RequestBody(required = false) Rpt180910SalesSummaryByCustomerPackSizeRequest request) throws Exception {
        if (request == null) request = new Rpt180910SalesSummaryByCustomerPackSizeRequest();
        printReport(response, "1809_10-SalesSummaryByCustomer&PackSize.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1809-10-sales-summary-by-customer-pack-size", method = RequestMethod.GET)
    public void print180910SalesSummaryByCustomerPackSizeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180910SalesSummaryByCustomerPackSize(response, objectMapper.convertValue(query, Rpt180910SalesSummaryByCustomerPackSizeRequest.class));
    }

    /**
     * Template: 1809_11-SalesSummaryByParentCategory.rpt
     * Procedure: [dbo].[USP_SalesFromEvaulation_RegisterSalt]
     * Desktop: InventoryStockEvalautionDetail.SalesFromEvaulation_RegisterSalt
     */
    @RequestMapping(value = "/reports/print/1809-11-sales-summary-by-parent-category", method = RequestMethod.POST)
    public void print180911SalesSummaryByParentCategory(HttpServletResponse response, @RequestBody(required = false) Rpt180911SalesSummaryByParentCategoryRequest request) throws Exception {
        if (request == null) request = new Rpt180911SalesSummaryByParentCategoryRequest();
        printReport(response, "1809_11-SalesSummaryByParentCategory.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1809-11-sales-summary-by-parent-category", method = RequestMethod.GET)
    public void print180911SalesSummaryByParentCategoryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180911SalesSummaryByParentCategory(response, objectMapper.convertValue(query, Rpt180911SalesSummaryByParentCategoryRequest.class));
    }

    /**
     * Template: 1809_12-SalesSummaryByParentCategory&Item.rpt
     * Procedure: [dbo].[USP_SalesFromEvaulation_RegisterSalt]
     * Desktop: InventoryStockEvalautionDetail.SalesFromEvaulation_RegisterSalt
     */
    @RequestMapping(value = "/reports/print/1809-12-sales-summary-by-parent-category-item", method = RequestMethod.POST)
    public void print180912SalesSummaryByParentCategoryItem(HttpServletResponse response, @RequestBody(required = false) Rpt180912SalesSummaryByParentCategoryItemRequest request) throws Exception {
        if (request == null) request = new Rpt180912SalesSummaryByParentCategoryItemRequest();
        printReport(response, "1809_12-SalesSummaryByParentCategory&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1809-12-sales-summary-by-parent-category-item", method = RequestMethod.GET)
    public void print180912SalesSummaryByParentCategoryItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180912SalesSummaryByParentCategoryItem(response, objectMapper.convertValue(query, Rpt180912SalesSummaryByParentCategoryItemRequest.class));
    }

    /**
     * Template: 1809_13-SalesSummaryByParentCategory&Customer.rpt
     * Procedure: [dbo].[USP_SalesFromEvaulation_RegisterSalt]
     * Desktop: InventoryStockEvalautionDetail.SalesFromEvaulation_RegisterSalt
     */
    @RequestMapping(value = "/reports/print/1809-13-sales-summary-by-parent-category-customer", method = RequestMethod.POST)
    public void print180913SalesSummaryByParentCategoryCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt180913SalesSummaryByParentCategoryCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt180913SalesSummaryByParentCategoryCustomerRequest();
        printReport(response, "1809_13-SalesSummaryByParentCategory&Customer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1809-13-sales-summary-by-parent-category-customer", method = RequestMethod.GET)
    public void print180913SalesSummaryByParentCategoryCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180913SalesSummaryByParentCategoryCustomer(response, objectMapper.convertValue(query, Rpt180913SalesSummaryByParentCategoryCustomerRequest.class));
    }

    /**
     * Template: 1809_14-SalesSummaryByItemPackSize&City.rpt
     * Procedure: [dbo].[USP_SalesFromEvaulation_RegisterSalt]
     * Desktop: InventoryStockEvalautionDetail.SalesFromEvaulation_RegisterSalt
     */
    @RequestMapping(value = "/reports/print/1809-14-sales-summary-by-item-pack-size-city", method = RequestMethod.POST)
    public void print180914SalesSummaryByItemPackSizeCity(HttpServletResponse response, @RequestBody(required = false) Rpt180914SalesSummaryByItemPackSizeCityRequest request) throws Exception {
        if (request == null) request = new Rpt180914SalesSummaryByItemPackSizeCityRequest();
        printReport(response, "1809_14-SalesSummaryByItemPackSize&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1809-14-sales-summary-by-item-pack-size-city", method = RequestMethod.GET)
    public void print180914SalesSummaryByItemPackSizeCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180914SalesSummaryByItemPackSizeCity(response, objectMapper.convertValue(query, Rpt180914SalesSummaryByItemPackSizeCityRequest.class));
    }

    /**
     * Template: 1809_15-SalesSummaryByItem&City.rpt
     * Procedure: [dbo].[USP_SalesFromEvaulation_RegisterSalt]
     * Desktop: InventoryStockEvalautionDetail.SalesFromEvaulation_RegisterSalt
     */
    @RequestMapping(value = "/reports/print/1809-15-sales-summary-by-item-city", method = RequestMethod.POST)
    public void print180915SalesSummaryByItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt180915SalesSummaryByItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt180915SalesSummaryByItemCityRequest();
        printReport(response, "1809_15-SalesSummaryByItem&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1809-15-sales-summary-by-item-city", method = RequestMethod.GET)
    public void print180915SalesSummaryByItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print180915SalesSummaryByItemCity(response, objectMapper.convertValue(query, Rpt180915SalesSummaryByItemCityRequest.class));
    }

    /**
     * Template: 1809A-SaleInvoiceDirectSlip_ForSalt.rpt
     * Procedure: [dbo].[USP_SaleInvoiceDirectSlip_ForSalt]
     * Desktop: InvSaleInvoiceReports.SaleInvoiceDirectSlip_ForSalt
     */
    @RequestMapping(value = "/reports/print/1809a-sale-invoice-direct-slip-for-salt", method = RequestMethod.POST)
    public void print1809ASaleInvoiceDirectSlipForSalt(HttpServletResponse response, @RequestBody(required = false) Rpt1809ASaleInvoiceDirectSlipForSaltRequest request) throws Exception {
        if (request == null) request = new Rpt1809ASaleInvoiceDirectSlipForSaltRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1809A-SaleInvoiceDirectSlip_ForSalt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1809a-sale-invoice-direct-slip-for-salt", method = RequestMethod.GET)
    public void print1809ASaleInvoiceDirectSlipForSaltGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1809ASaleInvoiceDirectSlipForSalt(response, objectMapper.convertValue(query, Rpt1809ASaleInvoiceDirectSlipForSaltRequest.class));
    }

    /**
     * Template: 199-GetOutstandingOrdersWithLedgerBalance.rpt
     * Procedure: USP_GetOutstandingOrdersWithLedgerBalance
     * Desktop: SaleOrder.GetOutstandingOrdersWithLedgerBalance
     */
    @RequestMapping(value = "/reports/print/199-get-outstanding-orders-with-ledger-balance", method = RequestMethod.POST)
    public void print199GetOutstandingOrdersWithLedgerBalance(HttpServletResponse response, @RequestBody(required = false) Rpt199GetOutstandingOrdersWithLedgerBalanceRequest request) throws Exception {
        if (request == null) request = new Rpt199GetOutstandingOrdersWithLedgerBalanceRequest();
        printReport(response, "199-GetOutstandingOrdersWithLedgerBalance.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/199-get-outstanding-orders-with-ledger-balance", method = RequestMethod.GET)
    public void print199GetOutstandingOrdersWithLedgerBalanceGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print199GetOutstandingOrdersWithLedgerBalance(response, objectMapper.convertValue(query, Rpt199GetOutstandingOrdersWithLedgerBalanceRequest.class));
    }

    /**
     * Template: 226-RptInvSaleInvoiceRegister.rpt
     * Procedure: Sp_InvSaleInvoiceTrading_CustomerBill_Register
     * Desktop: SalesTrading.GetInvoiceRegister
     */
    @RequestMapping(value = "/reports/print/226-inv-sale-invoice-register", method = RequestMethod.POST)
    public void print226InvSaleInvoiceRegister(HttpServletResponse response, @RequestBody(required = false) Rpt226InvSaleInvoiceRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt226InvSaleInvoiceRegisterRequest();
        printReport(response, "226-RptInvSaleInvoiceRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/226-inv-sale-invoice-register", method = RequestMethod.GET)
    public void print226InvSaleInvoiceRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print226InvSaleInvoiceRegister(response, objectMapper.convertValue(query, Rpt226InvSaleInvoiceRegisterRequest.class));
    }

    /**
     * Template: 263-InvDeliveryOrderForApprovel.rpt
     * Procedure: Sp_Inventory_ReadDashboardPendingForApproval
     * Desktop: Dashboard.ReadInventoryDashboardPendingForApproval
     */
    @RequestMapping(value = "/reports/print/263-inv-delivery-order-for-approvel", method = RequestMethod.POST)
    public void print263InvDeliveryOrderForApprovel(HttpServletResponse response, @RequestBody(required = false) Rpt263InvDeliveryOrderForApprovelRequest request) throws Exception {
        if (request == null) request = new Rpt263InvDeliveryOrderForApprovelRequest();
        printReport(response, "263-InvDeliveryOrderForApprovel.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/263-inv-delivery-order-for-approvel", method = RequestMethod.GET)
    public void print263InvDeliveryOrderForApprovelGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print263InvDeliveryOrderForApprovel(response, objectMapper.convertValue(query, Rpt263InvDeliveryOrderForApprovelRequest.class));
    }

    /**
     * Template: 273-InvRptSaleOrderSlip(A).rpt
     * Procedure: Sp_SaleOrder_RiceSlip_Rpt
     * Desktop: SaleOrderReports.SaleOrderReports273
     */
    @RequestMapping(value = "/reports/print/273-sale-order-slip-a", method = RequestMethod.POST)
    public void print273SaleOrderSlipA(HttpServletResponse response, @RequestBody(required = false) Rpt273SaleOrderSlipARequest request) throws Exception {
        if (request == null) request = new Rpt273SaleOrderSlipARequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "273-InvRptSaleOrderSlip(A).rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/273-sale-order-slip-a", method = RequestMethod.GET)
    public void print273SaleOrderSlipAGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print273SaleOrderSlipA(response, objectMapper.convertValue(query, Rpt273SaleOrderSlipARequest.class));
    }

    /**
     * Template: 274_PreBookingOrder_Slip.rpt
     * Procedure: usp_PreBookingOrder_Slip
     * Desktop: PreBookingOrder.PreBookingOrder_Slip
     */
    @RequestMapping(value = "/reports/print/274-pre-booking-order-slip", method = RequestMethod.POST)
    public void print274PreBookingOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt274PreBookingOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt274PreBookingOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "274_PreBookingOrder_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/274-pre-booking-order-slip", method = RequestMethod.GET)
    public void print274PreBookingOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print274PreBookingOrderSlip(response, objectMapper.convertValue(query, Rpt274PreBookingOrderSlipRequest.class));
    }

    /**
     * Template: 275-InvRptSalesOrderGeneral.rpt
     * Procedure: Sp_SaleOrder_RiceSlip_Rpt
     * Desktop: SaleOrderReports.SaleOrderReports273
     */
    @RequestMapping(value = "/reports/print/275-sales-order-general", method = RequestMethod.POST)
    public void print275SalesOrderGeneral(HttpServletResponse response, @RequestBody(required = false) Rpt275SalesOrderGeneralRequest request) throws Exception {
        if (request == null) request = new Rpt275SalesOrderGeneralRequest();
        printReport(response, "275-InvRptSalesOrderGeneral.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/275-sales-order-general", method = RequestMethod.GET)
    public void print275SalesOrderGeneralGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print275SalesOrderGeneral(response, objectMapper.convertValue(query, Rpt275SalesOrderGeneralRequest.class));
    }

    /**
     * Template: 282-SaleInvoice_DirectFlour.rpt
     * Procedure: sp_InvSaleInvoiceDirectSlip
     * Desktop: InvSaleInvoiceReports.InvSaleInvoiceDirectSlipRice
     */
    @RequestMapping(value = "/reports/print/282-sale-invoice-direct-flour", method = RequestMethod.POST)
    public void print282SaleInvoiceDirectFlour(HttpServletResponse response, @RequestBody(required = false) Rpt282SaleInvoiceDirectFlourRequest request) throws Exception {
        if (request == null) request = new Rpt282SaleInvoiceDirectFlourRequest();
        printReport(response, "282-SaleInvoice_DirectFlour.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/282-sale-invoice-direct-flour", method = RequestMethod.GET)
    public void print282SaleInvoiceDirectFlourGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print282SaleInvoiceDirectFlour(response, objectMapper.convertValue(query, Rpt282SaleInvoiceDirectFlourRequest.class));
    }

    /**
     * Template: 282A-SaleInvoice_DirectFlour.rpt
     * Procedure: sp_InvSaleInvoiceDirectSlip
     * Desktop: InvSaleInvoiceReports.InvSaleInvoiceDirectSlipRice
     */
    @RequestMapping(value = "/reports/print/282a-sale-invoice-direct-flour", method = RequestMethod.POST)
    public void print282ASaleInvoiceDirectFlour(HttpServletResponse response, @RequestBody(required = false) Rpt282ASaleInvoiceDirectFlourRequest request) throws Exception {
        if (request == null) request = new Rpt282ASaleInvoiceDirectFlourRequest();
        printReport(response, "282A-SaleInvoice_DirectFlour.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/282a-sale-invoice-direct-flour", method = RequestMethod.GET)
    public void print282ASaleInvoiceDirectFlourGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print282ASaleInvoiceDirectFlour(response, objectMapper.convertValue(query, Rpt282ASaleInvoiceDirectFlourRequest.class));
    }

    /**
     * Template: 290-InvRptOutwardGatePassSlip.rpt
     * Procedure: Sp_GatePassOutward_SlipAndRegister_Rpt
     * Desktop: GatePassOutwardReports.GatePassOutwardSlipandRegister
     */
    @RequestMapping(value = "/reports/print/290-outward-gate-pass-slip", method = RequestMethod.POST)
    public void print290OutwardGatePassSlip(HttpServletResponse response, @RequestBody(required = false) Rpt290OutwardGatePassSlipRequest request) throws Exception {
        if (request == null) request = new Rpt290OutwardGatePassSlipRequest();
        printReport(response, "290-InvRptOutwardGatePassSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/290-outward-gate-pass-slip", method = RequestMethod.GET)
    public void print290OutwardGatePassSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print290OutwardGatePassSlip(response, objectMapper.convertValue(query, Rpt290OutwardGatePassSlipRequest.class));
    }

    /**
     * Template: 291-InvRptGatePassOutwardRegister.rpt
     * Procedure: Sp_GatePassOutward_SlipAndRegister_Rpt
     * Desktop: GatePassOutwardReports.GatePassOutwardSlipandRegister
     */
    @RequestMapping(value = "/reports/print/291-gate-pass-outward-register", method = RequestMethod.POST)
    public void print291GatePassOutwardRegister(HttpServletResponse response, @RequestBody(required = false) Rpt291GatePassOutwardRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt291GatePassOutwardRegisterRequest();
        printReport(response, "291-InvRptGatePassOutwardRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/291-gate-pass-outward-register", method = RequestMethod.GET)
    public void print291GatePassOutwardRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print291GatePassOutwardRegister(response, objectMapper.convertValue(query, Rpt291GatePassOutwardRegisterRequest.class));
    }

    /**
     * Template: 291A-SaleInvoice_AutoRated.rpt
     * Procedure: sp_InvSaleInvoiceDirectSlip
     * Desktop: InvSaleInvoiceReports.InvSaleInvoiceDirectSlipRice
     */
    @RequestMapping(value = "/reports/print/291a-sale-invoice-auto-rated", method = RequestMethod.POST)
    public void print291ASaleInvoiceAutoRated(HttpServletResponse response, @RequestBody(required = false) Rpt291ASaleInvoiceAutoRatedRequest request) throws Exception {
        if (request == null) request = new Rpt291ASaleInvoiceAutoRatedRequest();
        printReport(response, "291A-SaleInvoice_AutoRated.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/291a-sale-invoice-auto-rated", method = RequestMethod.GET)
    public void print291ASaleInvoiceAutoRatedGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print291ASaleInvoiceAutoRated(response, objectMapper.convertValue(query, Rpt291ASaleInvoiceAutoRatedRequest.class));
    }

    /**
     * Template: 294A-InvRptSaleBillDirectWithoutSO.rpt
     * Procedure: sp_InvSaleInvoiceDirectSlip
     * Desktop: InvSaleInvoiceReports.InvSaleInvoiceDirectSlipRice
     */
    @RequestMapping(value = "/reports/print/294a-sale-bill-direct-without-so", method = RequestMethod.POST)
    public void print294ASaleBillDirectWithoutSO(HttpServletResponse response, @RequestBody(required = false) Rpt294ASaleBillDirectWithoutSORequest request) throws Exception {
        if (request == null) request = new Rpt294ASaleBillDirectWithoutSORequest();
        printReport(response, "294A-InvRptSaleBillDirectWithoutSO.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/294a-sale-bill-direct-without-so", method = RequestMethod.GET)
    public void print294ASaleBillDirectWithoutSOGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print294ASaleBillDirectWithoutSO(response, objectMapper.convertValue(query, Rpt294ASaleBillDirectWithoutSORequest.class));
    }

    /**
     * Template: 294B-InvRptSaleBillDirectWithoutSO.rpt
     * Procedure: sp_InvSaleInvoiceDirectSlip
     * Desktop: InvSaleInvoiceReports.InvSaleInvoiceDirectSlipRice
     */
    @RequestMapping(value = "/reports/print/294b-sale-bill-direct-without-so", method = RequestMethod.POST)
    public void print294BSaleBillDirectWithoutSO(HttpServletResponse response, @RequestBody(required = false) Rpt294BSaleBillDirectWithoutSORequest request) throws Exception {
        if (request == null) request = new Rpt294BSaleBillDirectWithoutSORequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "294B-InvRptSaleBillDirectWithoutSO.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/294b-sale-bill-direct-without-so", method = RequestMethod.GET)
    public void print294BSaleBillDirectWithoutSOGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print294BSaleBillDirectWithoutSO(response, objectMapper.convertValue(query, Rpt294BSaleBillDirectWithoutSORequest.class));
    }

    /**
     * Template: 294C_SaleBillDirectItemSlip.rpt
     * Procedure: sp_InvSaleInvoiceDirectSlip
     * Desktop: InvSaleInvoiceReports.InvSaleInvoiceDirectSlipRice
     */
    @RequestMapping(value = "/reports/print/294c-sale-bill-direct-item-slip", method = RequestMethod.POST)
    public void print294CSaleBillDirectItemSlip(HttpServletResponse response, @RequestBody(required = false) Rpt294CSaleBillDirectItemSlipRequest request) throws Exception {
        if (request == null) request = new Rpt294CSaleBillDirectItemSlipRequest();
        printReport(response, "294C_SaleBillDirectItemSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/294c-sale-bill-direct-item-slip", method = RequestMethod.GET)
    public void print294CSaleBillDirectItemSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print294CSaleBillDirectItemSlip(response, objectMapper.convertValue(query, Rpt294CSaleBillDirectItemSlipRequest.class));
    }

    /**
     * Template: 295-InvRptOutwardGatePassSlipWithItems.rpt
     * Procedure: Sp_GatePassOutward_SlipAndRegister_Rpt
     * Desktop: GatePassOutwardReports.GatePassOutwardSlipandRegister
     */
    @RequestMapping(value = "/reports/print/295-outward-gate-pass-slip-with-items", method = RequestMethod.POST)
    public void print295OutwardGatePassSlipWithItems(HttpServletResponse response, @RequestBody(required = false) Rpt295OutwardGatePassSlipWithItemsRequest request) throws Exception {
        if (request == null) request = new Rpt295OutwardGatePassSlipWithItemsRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "295-InvRptOutwardGatePassSlipWithItems.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/295-outward-gate-pass-slip-with-items", method = RequestMethod.GET)
    public void print295OutwardGatePassSlipWithItemsGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print295OutwardGatePassSlipWithItems(response, objectMapper.convertValue(query, Rpt295OutwardGatePassSlipWithItemsRequest.class));
    }

    /**
     * Template: 299-SalesOrderRegistery.rpt
     * Procedure: [dbo].[USP_SaleOrderDetailRegister_Eng]
     * Desktop: SaleOrder.SaleOrderDetailRegister_Eng
     */
    @RequestMapping(value = "/reports/print/299-sales-order-registery", method = RequestMethod.POST)
    public void print299SalesOrderRegistery(HttpServletResponse response, @RequestBody(required = false) Rpt299SalesOrderRegisteryRequest request) throws Exception {
        if (request == null) request = new Rpt299SalesOrderRegisteryRequest();
        printReport(response, "299-SalesOrderRegistery.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/299-sales-order-registery", method = RequestMethod.GET)
    public void print299SalesOrderRegisteryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print299SalesOrderRegistery(response, objectMapper.convertValue(query, Rpt299SalesOrderRegisteryRequest.class));
    }

    /**
     * Template: 301-InvRepSaleBillCustomer.rpt
     * Procedure: SP_InvSaleInvoice_CustomerBillRice OthersExp_SubRep
     * Desktop: InvSaleInvoiceReports.SalesCustomerBillSubReport
     */
    @RequestMapping(value = "/reports/print/301-inv-rep-sale-bill-customer", method = RequestMethod.POST)
    public void print301InvRepSaleBillCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt301InvRepSaleBillCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt301InvRepSaleBillCustomerRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "301-InvRepSaleBillCustomer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/301-inv-rep-sale-bill-customer", method = RequestMethod.GET)
    public void print301InvRepSaleBillCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print301InvRepSaleBillCustomer(response, objectMapper.convertValue(query, Rpt301InvRepSaleBillCustomerRequest.class));
    }

    /**
     * Template: 302-InvSaleInvoice.rpt
     * Procedure: sp_InvSaleInvoiceDirectRegister
     * Desktop: InvSaleInvoiceReports.InvSaleInvoiceDirectRegister
     */
    @RequestMapping(value = "/reports/print/302-inv-sale-invoice", method = RequestMethod.POST)
    public void print302InvSaleInvoice(HttpServletResponse response, @RequestBody(required = false) Rpt302InvSaleInvoiceRequest request) throws Exception {
        if (request == null) request = new Rpt302InvSaleInvoiceRequest();
        printReport(response, "302-InvSaleInvoice.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/302-inv-sale-invoice", method = RequestMethod.GET)
    public void print302InvSaleInvoiceGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print302InvSaleInvoice(response, objectMapper.convertValue(query, Rpt302InvSaleInvoiceRequest.class));
    }

    /**
     * Template: 303-InvRepSaleBillCustomer-Format-II.rpt
     * Procedure: Sp_InvSaleInvoice_CustomerBillRice_Rpt
     * Desktop: InvSaleInvoiceReports.InvSaleInvoice_CustomerBillRice_Rpt
     */
    @RequestMapping(value = "/reports/print/303-inv-rep-sale-bill-customer-format-ii", method = RequestMethod.POST)
    public void print303InvRepSaleBillCustomerFormatII(HttpServletResponse response, @RequestBody(required = false) Rpt303InvRepSaleBillCustomerFormatIIRequest request) throws Exception {
        if (request == null) request = new Rpt303InvRepSaleBillCustomerFormatIIRequest();
        printReport(response, "303-InvRepSaleBillCustomer-Format-II.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/303-inv-rep-sale-bill-customer-format-ii", method = RequestMethod.GET)
    public void print303InvRepSaleBillCustomerFormatIIGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print303InvRepSaleBillCustomerFormatII(response, objectMapper.convertValue(query, Rpt303InvRepSaleBillCustomerFormatIIRequest.class));
    }

    /**
     * Template: 303A_SaleInvoiceItemSlip.rpt
     * Procedure: Sp_InvSaleInvoice_CustomerBillRice_Rpt
     * Desktop: InvSaleInvoiceReports.InvSaleInvoice_CustomerBillRice_Rpt
     */
    @RequestMapping(value = "/reports/print/303a-sale-invoice-item-slip", method = RequestMethod.POST)
    public void print303ASaleInvoiceItemSlip(HttpServletResponse response, @RequestBody(required = false) Rpt303ASaleInvoiceItemSlipRequest request) throws Exception {
        if (request == null) request = new Rpt303ASaleInvoiceItemSlipRequest();
        printReport(response, "303A_SaleInvoiceItemSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/303a-sale-invoice-item-slip", method = RequestMethod.GET)
    public void print303ASaleInvoiceItemSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print303ASaleInvoiceItemSlip(response, objectMapper.convertValue(query, Rpt303ASaleInvoiceItemSlipRequest.class));
    }

    /**
     * Template: 310-InvrptSalesInvoiceMHI.rpt
     * Procedure: Sp_InvSaleInvoiceTrading_CustomerBill
     * Desktop: InvSaleInvoiceReports.InvSaleInvoiceTradingSlip
     */
    @RequestMapping(value = "/reports/print/310-sales-invoice-mhi", method = RequestMethod.POST)
    public void print310SalesInvoiceMHI(HttpServletResponse response, @RequestBody(required = false) Rpt310SalesInvoiceMHIRequest request) throws Exception {
        if (request == null) request = new Rpt310SalesInvoiceMHIRequest();
        printReport(response, "310-InvrptSalesInvoiceMHI.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/310-sales-invoice-mhi", method = RequestMethod.GET)
    public void print310SalesInvoiceMHIGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print310SalesInvoiceMHI(response, objectMapper.convertValue(query, Rpt310SalesInvoiceMHIRequest.class));
    }

    /**
     * Template: 311-InvrptSalesInvoiceMHII.rpt
     * Procedure: Sp_InvSaleInvoiceTrading_CustomerBill
     * Desktop: InvSaleInvoiceReports.InvSaleInvoiceTradingSlip
     */
    @RequestMapping(value = "/reports/print/311-sales-invoice-mhii", method = RequestMethod.POST)
    public void print311SalesInvoiceMHII(HttpServletResponse response, @RequestBody(required = false) Rpt311SalesInvoiceMHIIRequest request) throws Exception {
        if (request == null) request = new Rpt311SalesInvoiceMHIIRequest();
        printReport(response, "311-InvrptSalesInvoiceMHII.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/311-sales-invoice-mhii", method = RequestMethod.GET)
    public void print311SalesInvoiceMHIIGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print311SalesInvoiceMHII(response, objectMapper.convertValue(query, Rpt311SalesInvoiceMHIIRequest.class));
    }

    /**
     * Template: 312-SalesRegisterSummary.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/312-sales-register-summary", method = RequestMethod.POST)
    public void print312SalesRegisterSummary(HttpServletResponse response, @RequestBody(required = false) Rpt312SalesRegisterSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt312SalesRegisterSummaryRequest();
        printReport(response, "312-SalesRegisterSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/312-sales-register-summary", method = RequestMethod.GET)
    public void print312SalesRegisterSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print312SalesRegisterSummary(response, objectMapper.convertValue(query, Rpt312SalesRegisterSummaryRequest.class));
    }

    /**
     * Template: 313-SalesRegisterSummaryByCustomer.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/313-sales-register-summary-by-customer", method = RequestMethod.POST)
    public void print313SalesRegisterSummaryByCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt313SalesRegisterSummaryByCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt313SalesRegisterSummaryByCustomerRequest();
        printReport(response, "313-SalesRegisterSummaryByCustomer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/313-sales-register-summary-by-customer", method = RequestMethod.GET)
    public void print313SalesRegisterSummaryByCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print313SalesRegisterSummaryByCustomer(response, objectMapper.convertValue(query, Rpt313SalesRegisterSummaryByCustomerRequest.class));
    }

    /**
     * Template: 314-SalesRegisterSummaryByItem.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/314-sales-register-summary-by-item", method = RequestMethod.POST)
    public void print314SalesRegisterSummaryByItem(HttpServletResponse response, @RequestBody(required = false) Rpt314SalesRegisterSummaryByItemRequest request) throws Exception {
        if (request == null) request = new Rpt314SalesRegisterSummaryByItemRequest();
        printReport(response, "314-SalesRegisterSummaryByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/314-sales-register-summary-by-item", method = RequestMethod.GET)
    public void print314SalesRegisterSummaryByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print314SalesRegisterSummaryByItem(response, objectMapper.convertValue(query, Rpt314SalesRegisterSummaryByItemRequest.class));
    }

    /**
     * Template: 315-SalesRegisterSummaryByCustomer&Item.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/315-sales-register-summary-by-customer-item", method = RequestMethod.POST)
    public void print315SalesRegisterSummaryByCustomerItem(HttpServletResponse response, @RequestBody(required = false) Rpt315SalesRegisterSummaryByCustomerItemRequest request) throws Exception {
        if (request == null) request = new Rpt315SalesRegisterSummaryByCustomerItemRequest();
        printReport(response, "315-SalesRegisterSummaryByCustomer&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/315-sales-register-summary-by-customer-item", method = RequestMethod.GET)
    public void print315SalesRegisterSummaryByCustomerItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print315SalesRegisterSummaryByCustomerItem(response, objectMapper.convertValue(query, Rpt315SalesRegisterSummaryByCustomerItemRequest.class));
    }

    /**
     * Template: 316-SalesRegisterSummaryByItemWithoutPacking.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/316-sales-register-summary-by-item-without-packing", method = RequestMethod.POST)
    public void print316SalesRegisterSummaryByItemWithoutPacking(HttpServletResponse response, @RequestBody(required = false) Rpt316SalesRegisterSummaryByItemWithoutPackingRequest request) throws Exception {
        if (request == null) request = new Rpt316SalesRegisterSummaryByItemWithoutPackingRequest();
        printReport(response, "316-SalesRegisterSummaryByItemWithoutPacking.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/316-sales-register-summary-by-item-without-packing", method = RequestMethod.GET)
    public void print316SalesRegisterSummaryByItemWithoutPackingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print316SalesRegisterSummaryByItemWithoutPacking(response, objectMapper.convertValue(query, Rpt316SalesRegisterSummaryByItemWithoutPackingRequest.class));
    }

    /**
     * Template: 317-SalesRegisterSummaryByWarehouse.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/317-sales-register-summary-by-warehouse", method = RequestMethod.POST)
    public void print317SalesRegisterSummaryByWarehouse(HttpServletResponse response, @RequestBody(required = false) Rpt317SalesRegisterSummaryByWarehouseRequest request) throws Exception {
        if (request == null) request = new Rpt317SalesRegisterSummaryByWarehouseRequest();
        printReport(response, "317-SalesRegisterSummaryByWarehouse.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/317-sales-register-summary-by-warehouse", method = RequestMethod.GET)
    public void print317SalesRegisterSummaryByWarehouseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print317SalesRegisterSummaryByWarehouse(response, objectMapper.convertValue(query, Rpt317SalesRegisterSummaryByWarehouseRequest.class));
    }

    /**
     * Template: 318-InvRepSaleBillCustomer.rpt
     * Procedure: Sp_InvSaleInvoice_CustomerBillRice_Rpt
     * Desktop: InvSaleInvoiceReports.InvSaleInvoice_CustomerBillRice_Rpt
     */
    @RequestMapping(value = "/reports/print/318-inv-rep-sale-bill-customer", method = RequestMethod.POST)
    public void print318InvRepSaleBillCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt318InvRepSaleBillCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt318InvRepSaleBillCustomerRequest();
        printReport(response, "318-InvRepSaleBillCustomer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/318-inv-rep-sale-bill-customer", method = RequestMethod.GET)
    public void print318InvRepSaleBillCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print318InvRepSaleBillCustomer(response, objectMapper.convertValue(query, Rpt318InvRepSaleBillCustomerRequest.class));
    }

    /**
     * Template: 318A-InvRepSaleBillCustomer.rpt
     * Procedure: Sp_InvSaleInvoice_CustomerBillRice_Rpt
     * Desktop: InvSaleInvoiceReports.InvSaleInvoice_CustomerBillRice_Rpt
     */
    @RequestMapping(value = "/reports/print/318a-inv-rep-sale-bill-customer", method = RequestMethod.POST)
    public void print318AInvRepSaleBillCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt318AInvRepSaleBillCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt318AInvRepSaleBillCustomerRequest();
        printReport(response, "318A-InvRepSaleBillCustomer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/318a-inv-rep-sale-bill-customer", method = RequestMethod.GET)
    public void print318AInvRepSaleBillCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print318AInvRepSaleBillCustomer(response, objectMapper.convertValue(query, Rpt318AInvRepSaleBillCustomerRequest.class));
    }

    /**
     * Template: 342-SalesSummaryByItem&City.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/342-sales-summary-by-item-city", method = RequestMethod.POST)
    public void print342SalesSummaryByItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt342SalesSummaryByItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt342SalesSummaryByItemCityRequest();
        printReport(response, "342-SalesSummaryByItem&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/342-sales-summary-by-item-city", method = RequestMethod.GET)
    public void print342SalesSummaryByItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print342SalesSummaryByItemCity(response, objectMapper.convertValue(query, Rpt342SalesSummaryByItemCityRequest.class));
    }

    /**
     * Template: 343-SalesSummaryByCustomer&City.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/343-sales-summary-by-customer-city", method = RequestMethod.POST)
    public void print343SalesSummaryByCustomerCity(HttpServletResponse response, @RequestBody(required = false) Rpt343SalesSummaryByCustomerCityRequest request) throws Exception {
        if (request == null) request = new Rpt343SalesSummaryByCustomerCityRequest();
        printReport(response, "343-SalesSummaryByCustomer&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/343-sales-summary-by-customer-city", method = RequestMethod.GET)
    public void print343SalesSummaryByCustomerCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print343SalesSummaryByCustomerCity(response, objectMapper.convertValue(query, Rpt343SalesSummaryByCustomerCityRequest.class));
    }

    /**
     * Template: 344-SalesSummaryByItemPackSize&City.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/344-sales-summary-by-item-pack-size-city", method = RequestMethod.POST)
    public void print344SalesSummaryByItemPackSizeCity(HttpServletResponse response, @RequestBody(required = false) Rpt344SalesSummaryByItemPackSizeCityRequest request) throws Exception {
        if (request == null) request = new Rpt344SalesSummaryByItemPackSizeCityRequest();
        printReport(response, "344-SalesSummaryByItemPackSize&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/344-sales-summary-by-item-pack-size-city", method = RequestMethod.GET)
    public void print344SalesSummaryByItemPackSizeCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print344SalesSummaryByItemPackSizeCity(response, objectMapper.convertValue(query, Rpt344SalesSummaryByItemPackSizeCityRequest.class));
    }

    /**
     * Template: 345-SalesSummaryByCustomerItem&City.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/345-sales-summary-by-customer-item-city", method = RequestMethod.POST)
    public void print345SalesSummaryByCustomerItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt345SalesSummaryByCustomerItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt345SalesSummaryByCustomerItemCityRequest();
        printReport(response, "345-SalesSummaryByCustomerItem&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/345-sales-summary-by-customer-item-city", method = RequestMethod.GET)
    public void print345SalesSummaryByCustomerItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print345SalesSummaryByCustomerItemCity(response, objectMapper.convertValue(query, Rpt345SalesSummaryByCustomerItemCityRequest.class));
    }

    /**
     * Template: 346-SalesSummaryByCustomer&PackSize.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/346-sales-summary-by-customer-pack-size", method = RequestMethod.POST)
    public void print346SalesSummaryByCustomerPackSize(HttpServletResponse response, @RequestBody(required = false) Rpt346SalesSummaryByCustomerPackSizeRequest request) throws Exception {
        if (request == null) request = new Rpt346SalesSummaryByCustomerPackSizeRequest();
        printReport(response, "346-SalesSummaryByCustomer&PackSize.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/346-sales-summary-by-customer-pack-size", method = RequestMethod.GET)
    public void print346SalesSummaryByCustomerPackSizeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print346SalesSummaryByCustomerPackSize(response, objectMapper.convertValue(query, Rpt346SalesSummaryByCustomerPackSizeRequest.class));
    }

    /**
     * Template: 347-SalesSummaryByParentCategory.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/347-sales-summary-by-parent-category", method = RequestMethod.POST)
    public void print347SalesSummaryByParentCategory(HttpServletResponse response, @RequestBody(required = false) Rpt347SalesSummaryByParentCategoryRequest request) throws Exception {
        if (request == null) request = new Rpt347SalesSummaryByParentCategoryRequest();
        printReport(response, "347-SalesSummaryByParentCategory.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/347-sales-summary-by-parent-category", method = RequestMethod.GET)
    public void print347SalesSummaryByParentCategoryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print347SalesSummaryByParentCategory(response, objectMapper.convertValue(query, Rpt347SalesSummaryByParentCategoryRequest.class));
    }

    /**
     * Template: 348-SalesSummaryByParentCategory&Item.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/348-sales-summary-by-parent-category-item", method = RequestMethod.POST)
    public void print348SalesSummaryByParentCategoryItem(HttpServletResponse response, @RequestBody(required = false) Rpt348SalesSummaryByParentCategoryItemRequest request) throws Exception {
        if (request == null) request = new Rpt348SalesSummaryByParentCategoryItemRequest();
        printReport(response, "348-SalesSummaryByParentCategory&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/348-sales-summary-by-parent-category-item", method = RequestMethod.GET)
    public void print348SalesSummaryByParentCategoryItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print348SalesSummaryByParentCategoryItem(response, objectMapper.convertValue(query, Rpt348SalesSummaryByParentCategoryItemRequest.class));
    }

    /**
     * Template: 349-SalesSummaryByParentCategory&Customer.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/349-sales-summary-by-parent-category-customer", method = RequestMethod.POST)
    public void print349SalesSummaryByParentCategoryCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt349SalesSummaryByParentCategoryCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt349SalesSummaryByParentCategoryCustomerRequest();
        printReport(response, "349-SalesSummaryByParentCategory&Customer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/349-sales-summary-by-parent-category-customer", method = RequestMethod.GET)
    public void print349SalesSummaryByParentCategoryCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print349SalesSummaryByParentCategoryCustomer(response, objectMapper.convertValue(query, Rpt349SalesSummaryByParentCategoryCustomerRequest.class));
    }

    /**
     * Template: 350-SalesSummaryByReferenceParty.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/350-sales-summary-by-reference-party", method = RequestMethod.POST)
    public void print350SalesSummaryByReferenceParty(HttpServletResponse response, @RequestBody(required = false) Rpt350SalesSummaryByReferencePartyRequest request) throws Exception {
        if (request == null) request = new Rpt350SalesSummaryByReferencePartyRequest();
        printReport(response, "350-SalesSummaryByReferenceParty.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/350-sales-summary-by-reference-party", method = RequestMethod.GET)
    public void print350SalesSummaryByReferencePartyGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print350SalesSummaryByReferenceParty(response, objectMapper.convertValue(query, Rpt350SalesSummaryByReferencePartyRequest.class));
    }

    /**
     * Template: 351-SalesSummaryByReferenceParty&City.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/351-sales-summary-by-reference-party-city", method = RequestMethod.POST)
    public void print351SalesSummaryByReferencePartyCity(HttpServletResponse response, @RequestBody(required = false) Rpt351SalesSummaryByReferencePartyCityRequest request) throws Exception {
        if (request == null) request = new Rpt351SalesSummaryByReferencePartyCityRequest();
        printReport(response, "351-SalesSummaryByReferenceParty&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/351-sales-summary-by-reference-party-city", method = RequestMethod.GET)
    public void print351SalesSummaryByReferencePartyCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print351SalesSummaryByReferencePartyCity(response, objectMapper.convertValue(query, Rpt351SalesSummaryByReferencePartyCityRequest.class));
    }

    /**
     * Template: 352-SalesSummaryByReferenceParty&PackSize.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/352-sales-summary-by-reference-party-pack-size", method = RequestMethod.POST)
    public void print352SalesSummaryByReferencePartyPackSize(HttpServletResponse response, @RequestBody(required = false) Rpt352SalesSummaryByReferencePartyPackSizeRequest request) throws Exception {
        if (request == null) request = new Rpt352SalesSummaryByReferencePartyPackSizeRequest();
        printReport(response, "352-SalesSummaryByReferenceParty&PackSize.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/352-sales-summary-by-reference-party-pack-size", method = RequestMethod.GET)
    public void print352SalesSummaryByReferencePartyPackSizeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print352SalesSummaryByReferencePartyPackSize(response, objectMapper.convertValue(query, Rpt352SalesSummaryByReferencePartyPackSizeRequest.class));
    }

    /**
     * Template: 358-PendingGatePassForDelivery.rpt
     * Procedure: USP_PendingGatePassForDelivery
     * Desktop: GatePassOutward.PendingGatePassForDelivery_prnt
     */
    @RequestMapping(value = "/reports/print/358-pending-gate-pass-for-delivery", method = RequestMethod.POST)
    public void print358PendingGatePassForDelivery(HttpServletResponse response, @RequestBody(required = false) Rpt358PendingGatePassForDeliveryRequest request) throws Exception {
        if (request == null) request = new Rpt358PendingGatePassForDeliveryRequest();
        printReport(response, "358-PendingGatePassForDelivery.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/358-pending-gate-pass-for-delivery", method = RequestMethod.GET)
    public void print358PendingGatePassForDeliveryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print358PendingGatePassForDelivery(response, objectMapper.convertValue(query, Rpt358PendingGatePassForDeliveryRequest.class));
    }

    /**
     * Template: 360-SalesRegisterSummary.rpt
     * Procedure: USP_Sales_EvaulationDetailReports_Engr
     * Desktop: InventoryStockEvalautionDetail.Sales_EvaulationDetailReports_Engr
     */
    @RequestMapping(value = "/reports/print/360-sales-register-summary", method = RequestMethod.POST)
    public void print360SalesRegisterSummary(HttpServletResponse response, @RequestBody(required = false) Rpt360SalesRegisterSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt360SalesRegisterSummaryRequest();
        printReport(response, "360-SalesRegisterSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/360-sales-register-summary", method = RequestMethod.GET)
    public void print360SalesRegisterSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print360SalesRegisterSummary(response, objectMapper.convertValue(query, Rpt360SalesRegisterSummaryRequest.class));
    }

    /**
     * Template: 361-SalesSummaryByItem.rpt
     * Procedure: USP_Sales_EvaulationDetailReports_Engr
     * Desktop: InventoryStockEvalautionDetail.Sales_EvaulationDetailReports_Engr
     */
    @RequestMapping(value = "/reports/print/361-sales-summary-by-item", method = RequestMethod.POST)
    public void print361SalesSummaryByItem(HttpServletResponse response, @RequestBody(required = false) Rpt361SalesSummaryByItemRequest request) throws Exception {
        if (request == null) request = new Rpt361SalesSummaryByItemRequest();
        printReport(response, "361-SalesSummaryByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/361-sales-summary-by-item", method = RequestMethod.GET)
    public void print361SalesSummaryByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print361SalesSummaryByItem(response, objectMapper.convertValue(query, Rpt361SalesSummaryByItemRequest.class));
    }

    /**
     * Template: 362-SalesSummaryByItem&City.rpt
     * Procedure: USP_Sales_EvaulationDetailReports_Engr
     * Desktop: InventoryStockEvalautionDetail.Sales_EvaulationDetailReports_Engr
     */
    @RequestMapping(value = "/reports/print/362-sales-summary-by-item-city", method = RequestMethod.POST)
    public void print362SalesSummaryByItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt362SalesSummaryByItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt362SalesSummaryByItemCityRequest();
        printReport(response, "362-SalesSummaryByItem&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/362-sales-summary-by-item-city", method = RequestMethod.GET)
    public void print362SalesSummaryByItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print362SalesSummaryByItemCity(response, objectMapper.convertValue(query, Rpt362SalesSummaryByItemCityRequest.class));
    }

    /**
     * Template: 363-SalesSummaryByItem&PackSize.rpt
     * Procedure: USP_Sales_EvaulationDetailReports_Engr
     * Desktop: InventoryStockEvalautionDetail.Sales_EvaulationDetailReports_Engr
     */
    @RequestMapping(value = "/reports/print/363-sales-summary-by-item-pack-size", method = RequestMethod.POST)
    public void print363SalesSummaryByItemPackSize(HttpServletResponse response, @RequestBody(required = false) Rpt363SalesSummaryByItemPackSizeRequest request) throws Exception {
        if (request == null) request = new Rpt363SalesSummaryByItemPackSizeRequest();
        printReport(response, "363-SalesSummaryByItem&PackSize.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/363-sales-summary-by-item-pack-size", method = RequestMethod.GET)
    public void print363SalesSummaryByItemPackSizeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print363SalesSummaryByItemPackSize(response, objectMapper.convertValue(query, Rpt363SalesSummaryByItemPackSizeRequest.class));
    }

    /**
     * Template: 364-SalesSummaryByItem&Warehouse.rpt
     * Procedure: USP_Sales_EvaulationDetailReports_Engr
     * Desktop: InventoryStockEvalautionDetail.Sales_EvaulationDetailReports_Engr
     */
    @RequestMapping(value = "/reports/print/364-sales-summary-by-item-warehouse", method = RequestMethod.POST)
    public void print364SalesSummaryByItemWarehouse(HttpServletResponse response, @RequestBody(required = false) Rpt364SalesSummaryByItemWarehouseRequest request) throws Exception {
        if (request == null) request = new Rpt364SalesSummaryByItemWarehouseRequest();
        printReport(response, "364-SalesSummaryByItem&Warehouse.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/364-sales-summary-by-item-warehouse", method = RequestMethod.GET)
    public void print364SalesSummaryByItemWarehouseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print364SalesSummaryByItemWarehouse(response, objectMapper.convertValue(query, Rpt364SalesSummaryByItemWarehouseRequest.class));
    }

    /**
     * Template: 365-SalesSummaryByItemPackSize&City.rpt
     * Procedure: USP_Sales_EvaulationDetailReports_Engr
     * Desktop: InventoryStockEvalautionDetail.Sales_EvaulationDetailReports_Engr
     */
    @RequestMapping(value = "/reports/print/365-sales-summary-by-item-pack-size-city", method = RequestMethod.POST)
    public void print365SalesSummaryByItemPackSizeCity(HttpServletResponse response, @RequestBody(required = false) Rpt365SalesSummaryByItemPackSizeCityRequest request) throws Exception {
        if (request == null) request = new Rpt365SalesSummaryByItemPackSizeCityRequest();
        printReport(response, "365-SalesSummaryByItemPackSize&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/365-sales-summary-by-item-pack-size-city", method = RequestMethod.GET)
    public void print365SalesSummaryByItemPackSizeCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print365SalesSummaryByItemPackSizeCity(response, objectMapper.convertValue(query, Rpt365SalesSummaryByItemPackSizeCityRequest.class));
    }

    /**
     * Template: 366-SalesSummaryByCustomer.rpt
     * Procedure: USP_Sales_EvaulationDetailReports_Engr
     * Desktop: InventoryStockEvalautionDetail.Sales_EvaulationDetailReports_Engr
     */
    @RequestMapping(value = "/reports/print/366-sales-summary-by-customer", method = RequestMethod.POST)
    public void print366SalesSummaryByCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt366SalesSummaryByCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt366SalesSummaryByCustomerRequest();
        printReport(response, "366-SalesSummaryByCustomer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/366-sales-summary-by-customer", method = RequestMethod.GET)
    public void print366SalesSummaryByCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print366SalesSummaryByCustomer(response, objectMapper.convertValue(query, Rpt366SalesSummaryByCustomerRequest.class));
    }

    /**
     * Template: 366ASalesSummaryByCustomer&Invoice.rpt
     * Procedure: USP_Sales_EvaulationDetailReports_Engr
     * Desktop: InventoryStockEvalautionDetail.Sales_EvaulationDetailReports_Engr
     */
    @RequestMapping(value = "/reports/print/366a-366-a-sales-summary-by-customer-invoice", method = RequestMethod.POST)
    public void print366A366ASalesSummaryByCustomerInvoice(HttpServletResponse response, @RequestBody(required = false) Rpt366A366ASalesSummaryByCustomerInvoiceRequest request) throws Exception {
        if (request == null) request = new Rpt366A366ASalesSummaryByCustomerInvoiceRequest();
        printReport(response, "366ASalesSummaryByCustomer&Invoice.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/366a-366-a-sales-summary-by-customer-invoice", method = RequestMethod.GET)
    public void print366A366ASalesSummaryByCustomerInvoiceGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print366A366ASalesSummaryByCustomerInvoice(response, objectMapper.convertValue(query, Rpt366A366ASalesSummaryByCustomerInvoiceRequest.class));
    }

    /**
     * Template: 366B-SalesSummaryByCustomer&Invoice.rpt
     * Procedure: USP_Sales_EvaulationDetailReports_Engr
     * Desktop: InventoryStockEvalautionDetail.Sales_EvaulationDetailReports_Engr
     */
    @RequestMapping(value = "/reports/print/366b-sales-summary-by-customer-invoice", method = RequestMethod.POST)
    public void print366BSalesSummaryByCustomerInvoice(HttpServletResponse response, @RequestBody(required = false) Rpt366BSalesSummaryByCustomerInvoiceRequest request) throws Exception {
        if (request == null) request = new Rpt366BSalesSummaryByCustomerInvoiceRequest();
        printReport(response, "366B-SalesSummaryByCustomer&Invoice.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/366b-sales-summary-by-customer-invoice", method = RequestMethod.GET)
    public void print366BSalesSummaryByCustomerInvoiceGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print366BSalesSummaryByCustomerInvoice(response, objectMapper.convertValue(query, Rpt366BSalesSummaryByCustomerInvoiceRequest.class));
    }

    /**
     * Template: 367-SalesSummaryByCustomer&Item.rpt
     * Procedure: USP_Sales_EvaulationDetailReports_Engr
     * Desktop: InventoryStockEvalautionDetail.Sales_EvaulationDetailReports_Engr
     */
    @RequestMapping(value = "/reports/print/367-sales-summary-by-customer-item", method = RequestMethod.POST)
    public void print367SalesSummaryByCustomerItem(HttpServletResponse response, @RequestBody(required = false) Rpt367SalesSummaryByCustomerItemRequest request) throws Exception {
        if (request == null) request = new Rpt367SalesSummaryByCustomerItemRequest();
        printReport(response, "367-SalesSummaryByCustomer&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/367-sales-summary-by-customer-item", method = RequestMethod.GET)
    public void print367SalesSummaryByCustomerItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print367SalesSummaryByCustomerItem(response, objectMapper.convertValue(query, Rpt367SalesSummaryByCustomerItemRequest.class));
    }

    /**
     * Template: 368-SalesSummaryByCustomer&City.rpt
     * Procedure: USP_Sales_EvaulationDetailReports_Engr
     * Desktop: InventoryStockEvalautionDetail.Sales_EvaulationDetailReports_Engr
     */
    @RequestMapping(value = "/reports/print/368-sales-summary-by-customer-city", method = RequestMethod.POST)
    public void print368SalesSummaryByCustomerCity(HttpServletResponse response, @RequestBody(required = false) Rpt368SalesSummaryByCustomerCityRequest request) throws Exception {
        if (request == null) request = new Rpt368SalesSummaryByCustomerCityRequest();
        printReport(response, "368-SalesSummaryByCustomer&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/368-sales-summary-by-customer-city", method = RequestMethod.GET)
    public void print368SalesSummaryByCustomerCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print368SalesSummaryByCustomerCity(response, objectMapper.convertValue(query, Rpt368SalesSummaryByCustomerCityRequest.class));
    }

    /**
     * Template: 369-SalesSummaryByCustomerItem&City.rpt
     * Procedure: USP_Sales_EvaulationDetailReports_Engr
     * Desktop: InventoryStockEvalautionDetail.Sales_EvaulationDetailReports_Engr
     */
    @RequestMapping(value = "/reports/print/369-sales-summary-by-customer-item-city", method = RequestMethod.POST)
    public void print369SalesSummaryByCustomerItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt369SalesSummaryByCustomerItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt369SalesSummaryByCustomerItemCityRequest();
        printReport(response, "369-SalesSummaryByCustomerItem&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/369-sales-summary-by-customer-item-city", method = RequestMethod.GET)
    public void print369SalesSummaryByCustomerItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print369SalesSummaryByCustomerItemCity(response, objectMapper.convertValue(query, Rpt369SalesSummaryByCustomerItemCityRequest.class));
    }

    /**
     * Template: 370-SalesSummaryByCustomer&PackSize.rpt
     * Procedure: USP_Sales_EvaulationDetailReports_Engr
     * Desktop: InventoryStockEvalautionDetail.Sales_EvaulationDetailReports_Engr
     */
    @RequestMapping(value = "/reports/print/370-sales-summary-by-customer-pack-size", method = RequestMethod.POST)
    public void print370SalesSummaryByCustomerPackSize(HttpServletResponse response, @RequestBody(required = false) Rpt370SalesSummaryByCustomerPackSizeRequest request) throws Exception {
        if (request == null) request = new Rpt370SalesSummaryByCustomerPackSizeRequest();
        printReport(response, "370-SalesSummaryByCustomer&PackSize.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/370-sales-summary-by-customer-pack-size", method = RequestMethod.GET)
    public void print370SalesSummaryByCustomerPackSizeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print370SalesSummaryByCustomerPackSize(response, objectMapper.convertValue(query, Rpt370SalesSummaryByCustomerPackSizeRequest.class));
    }

    /**
     * Template: 371-SalesSummaryByCity.rpt
     * Procedure: USP_Sales_EvaulationDetailReports_Engr
     * Desktop: InventoryStockEvalautionDetail.Sales_EvaulationDetailReports_Engr
     */
    @RequestMapping(value = "/reports/print/371-sales-summary-by-city", method = RequestMethod.POST)
    public void print371SalesSummaryByCity(HttpServletResponse response, @RequestBody(required = false) Rpt371SalesSummaryByCityRequest request) throws Exception {
        if (request == null) request = new Rpt371SalesSummaryByCityRequest();
        printReport(response, "371-SalesSummaryByCity.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/371-sales-summary-by-city", method = RequestMethod.GET)
    public void print371SalesSummaryByCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print371SalesSummaryByCity(response, objectMapper.convertValue(query, Rpt371SalesSummaryByCityRequest.class));
    }

    /**
     * Template: 372-SalesSummaryByParentCategory.rpt
     * Procedure: USP_Sales_EvaulationDetailReports_Engr
     * Desktop: InventoryStockEvalautionDetail.Sales_EvaulationDetailReports_Engr
     */
    @RequestMapping(value = "/reports/print/372-sales-summary-by-parent-category", method = RequestMethod.POST)
    public void print372SalesSummaryByParentCategory(HttpServletResponse response, @RequestBody(required = false) Rpt372SalesSummaryByParentCategoryRequest request) throws Exception {
        if (request == null) request = new Rpt372SalesSummaryByParentCategoryRequest();
        printReport(response, "372-SalesSummaryByParentCategory.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/372-sales-summary-by-parent-category", method = RequestMethod.GET)
    public void print372SalesSummaryByParentCategoryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print372SalesSummaryByParentCategory(response, objectMapper.convertValue(query, Rpt372SalesSummaryByParentCategoryRequest.class));
    }

    /**
     * Template: 373-SalesSummaryByParentCategory&Item.rpt
     * Procedure: USP_Sales_EvaulationDetailReports_Engr
     * Desktop: InventoryStockEvalautionDetail.Sales_EvaulationDetailReports_Engr
     */
    @RequestMapping(value = "/reports/print/373-sales-summary-by-parent-category-item", method = RequestMethod.POST)
    public void print373SalesSummaryByParentCategoryItem(HttpServletResponse response, @RequestBody(required = false) Rpt373SalesSummaryByParentCategoryItemRequest request) throws Exception {
        if (request == null) request = new Rpt373SalesSummaryByParentCategoryItemRequest();
        printReport(response, "373-SalesSummaryByParentCategory&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/373-sales-summary-by-parent-category-item", method = RequestMethod.GET)
    public void print373SalesSummaryByParentCategoryItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print373SalesSummaryByParentCategoryItem(response, objectMapper.convertValue(query, Rpt373SalesSummaryByParentCategoryItemRequest.class));
    }

    /**
     * Template: 374-SalesSummaryByParentCategory&Customer.rpt
     * Procedure: USP_Sales_EvaulationDetailReports_Engr
     * Desktop: InventoryStockEvalautionDetail.Sales_EvaulationDetailReports_Engr
     */
    @RequestMapping(value = "/reports/print/374-sales-summary-by-parent-category-customer", method = RequestMethod.POST)
    public void print374SalesSummaryByParentCategoryCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt374SalesSummaryByParentCategoryCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt374SalesSummaryByParentCategoryCustomerRequest();
        printReport(response, "374-SalesSummaryByParentCategory&Customer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/374-sales-summary-by-parent-category-customer", method = RequestMethod.GET)
    public void print374SalesSummaryByParentCategoryCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print374SalesSummaryByParentCategoryCustomer(response, objectMapper.convertValue(query, Rpt374SalesSummaryByParentCategoryCustomerRequest.class));
    }

    /**
     * Template: 376-SalesSummaryByCustomer,ItemReferenceParty.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/376-sales-summary-by-customer-item-reference-party", method = RequestMethod.POST)
    public void print376SalesSummaryByCustomerItemReferenceParty(HttpServletResponse response, @RequestBody(required = false) Rpt376SalesSummaryByCustomerItemReferencePartyRequest request) throws Exception {
        if (request == null) request = new Rpt376SalesSummaryByCustomerItemReferencePartyRequest();
        printReport(response, "376-SalesSummaryByCustomer,ItemReferenceParty.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/376-sales-summary-by-customer-item-reference-party", method = RequestMethod.GET)
    public void print376SalesSummaryByCustomerItemReferencePartyGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print376SalesSummaryByCustomerItemReferenceParty(response, objectMapper.convertValue(query, Rpt376SalesSummaryByCustomerItemReferencePartyRequest.class));
    }

    /**
     * Template: 377-SalesSummaryByReferenceParty&Item.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/377-sales-summary-by-reference-party-item", method = RequestMethod.POST)
    public void print377SalesSummaryByReferencePartyItem(HttpServletResponse response, @RequestBody(required = false) Rpt377SalesSummaryByReferencePartyItemRequest request) throws Exception {
        if (request == null) request = new Rpt377SalesSummaryByReferencePartyItemRequest();
        printReport(response, "377-SalesSummaryByReferenceParty&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/377-sales-summary-by-reference-party-item", method = RequestMethod.GET)
    public void print377SalesSummaryByReferencePartyItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print377SalesSummaryByReferencePartyItem(response, objectMapper.convertValue(query, Rpt377SalesSummaryByReferencePartyItemRequest.class));
    }

    /**
     * Template: 378-SalesSummaryByCustomer&ReferenceParty.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/378-sales-summary-by-customer-reference-party", method = RequestMethod.POST)
    public void print378SalesSummaryByCustomerReferenceParty(HttpServletResponse response, @RequestBody(required = false) Rpt378SalesSummaryByCustomerReferencePartyRequest request) throws Exception {
        if (request == null) request = new Rpt378SalesSummaryByCustomerReferencePartyRequest();
        printReport(response, "378-SalesSummaryByCustomer&ReferenceParty.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/378-sales-summary-by-customer-reference-party", method = RequestMethod.GET)
    public void print378SalesSummaryByCustomerReferencePartyGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print378SalesSummaryByCustomerReferenceParty(response, objectMapper.convertValue(query, Rpt378SalesSummaryByCustomerReferencePartyRequest.class));
    }

    /**
     * Template: 379-SalesSummaryByReferenceParty,Item&PackSize.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/379-sales-summary-by-reference-party-item-pack-size", method = RequestMethod.POST)
    public void print379SalesSummaryByReferencePartyItemPackSize(HttpServletResponse response, @RequestBody(required = false) Rpt379SalesSummaryByReferencePartyItemPackSizeRequest request) throws Exception {
        if (request == null) request = new Rpt379SalesSummaryByReferencePartyItemPackSizeRequest();
        printReport(response, "379-SalesSummaryByReferenceParty,Item&PackSize.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/379-sales-summary-by-reference-party-item-pack-size", method = RequestMethod.GET)
    public void print379SalesSummaryByReferencePartyItemPackSizeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print379SalesSummaryByReferencePartyItemPackSize(response, objectMapper.convertValue(query, Rpt379SalesSummaryByReferencePartyItemPackSizeRequest.class));
    }

    /**
     * Template: 397-SaleSummaryByHsCode.rpt
     * Procedure: SpInventory_EvaulationDetailSalesReports
     * Desktop: InventoryStockEvalautionDetail.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/397-sale-summary-by-hs-code", method = RequestMethod.POST)
    public void print397SaleSummaryByHsCode(HttpServletResponse response, @RequestBody(required = false) Rpt397SaleSummaryByHsCodeRequest request) throws Exception {
        if (request == null) request = new Rpt397SaleSummaryByHsCodeRequest();
        printReport(response, "397-SaleSummaryByHsCode.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/397-sale-summary-by-hs-code", method = RequestMethod.GET)
    public void print397SaleSummaryByHsCodeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print397SaleSummaryByHsCode(response, objectMapper.convertValue(query, Rpt397SaleSummaryByHsCodeRequest.class));
    }

    /**
     * Template: 460-InvRptDeliverySchedule.rpt
     * Procedure: Sp_DeliverySchedule_Rpt
     * Desktop: GeneralReprots.DeliveryScheduleSlip
     */
    @RequestMapping(value = "/reports/print/460-delivery-schedule", method = RequestMethod.POST)
    public void print460DeliverySchedule(HttpServletResponse response, @RequestBody(required = false) Rpt460DeliveryScheduleRequest request) throws Exception {
        if (request == null) request = new Rpt460DeliveryScheduleRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "460-InvRptDeliverySchedule.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/460-delivery-schedule", method = RequestMethod.GET)
    public void print460DeliveryScheduleGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print460DeliverySchedule(response, objectMapper.convertValue(query, Rpt460DeliveryScheduleRequest.class));
    }

    /**
     * Template: 7862-PendingOutwardGatePassRegister.rpt
     * Procedure: Sp_GatePassOutward_PendingGatePass
     * Desktop: GatePassOutward.PendingGatePassOutward
     */
    @RequestMapping(value = "/reports/print/7862-pending-outward-gate-pass-register", method = RequestMethod.POST)
    public void print7862PendingOutwardGatePassRegister(HttpServletResponse response, @RequestBody(required = false) Rpt7862PendingOutwardGatePassRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt7862PendingOutwardGatePassRegisterRequest();
        printReport(response, "7862-PendingOutwardGatePassRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/7862-pending-outward-gate-pass-register", method = RequestMethod.GET)
    public void print7862PendingOutwardGatePassRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print7862PendingOutwardGatePassRegister(response, objectMapper.convertValue(query, Rpt7862PendingOutwardGatePassRegisterRequest.class));
    }

    /**
     * Template: 7864-PendingGdnRegister.rpt
     * Procedure: Sp_InvGdn_PendingGdn
     * Desktop: InvGdn.GetPendingGdn
     */
    @RequestMapping(value = "/reports/print/7864-pending-gdn-register", method = RequestMethod.POST)
    public void print7864PendingGdnRegister(HttpServletResponse response, @RequestBody(required = false) Rpt7864PendingGdnRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt7864PendingGdnRegisterRequest();
        printReport(response, "7864-PendingGdnRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/7864-pending-gdn-register", method = RequestMethod.GET)
    public void print7864PendingGdnRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print7864PendingGdnRegister(response, objectMapper.convertValue(query, Rpt7864PendingGdnRegisterRequest.class));
    }

    /**
     * Template: 910-AdvanceDeliveryOrderRegister.rpt
     * Procedure: usp_AdvanceDeliveryOrderRegister
     * Desktop: InvDeliveryOrder.AdvanceDeliveryOrderRegister
     */
    @RequestMapping(value = "/reports/print/910-advance-delivery-order-register", method = RequestMethod.POST)
    public void print910AdvanceDeliveryOrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt910AdvanceDeliveryOrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt910AdvanceDeliveryOrderRegisterRequest();
        printReport(response, "910-AdvanceDeliveryOrderRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/910-advance-delivery-order-register", method = RequestMethod.GET)
    public void print910AdvanceDeliveryOrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print910AdvanceDeliveryOrderRegister(response, objectMapper.convertValue(query, Rpt910AdvanceDeliveryOrderRegisterRequest.class));
    }

    /**
     * Template: 910_01_PendingReservedSaleInvoiceForAdvanceDeliveryOrder.rpt
     * Procedure: [dbo].[usp_PendingReservedSaleInvoiceForAdvanceDeliveryOrder]
     * Desktop: InvSaleInvoice.PendingReservedSaleInvoiceForAdvanceDeliveryOrder
     */
    @RequestMapping(value = "/reports/print/910-01-pending-reserved-sale-invoice-for-advance-delivery-order", method = RequestMethod.POST)
    public void print91001PendingReservedSaleInvoiceForAdvanceDeliveryOrder(HttpServletResponse response, @RequestBody(required = false) Rpt91001PendingReservedSaleInvoiceForAdvanceDeliveryOrderRequest request) throws Exception {
        if (request == null) request = new Rpt91001PendingReservedSaleInvoiceForAdvanceDeliveryOrderRequest();
        printReport(response, "910_01_PendingReservedSaleInvoiceForAdvanceDeliveryOrder.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/910-01-pending-reserved-sale-invoice-for-advance-delivery-order", method = RequestMethod.GET)
    public void print91001PendingReservedSaleInvoiceForAdvanceDeliveryOrderGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print91001PendingReservedSaleInvoiceForAdvanceDeliveryOrder(response, objectMapper.convertValue(query, Rpt91001PendingReservedSaleInvoiceForAdvanceDeliveryOrderRequest.class));
    }

    /**
     * Template: InvSaleInvoiceCustomerBillRiceJournalExpSubRep.rpt
     * Procedure: Sp_InvSaleInvoice_CustomerBillRice_JournalExp_SubRep
     * Desktop: InvSaleInvoiceReports.SaleInvoiceSubReportJournalExpense
     */
    @RequestMapping(value = "/reports/print/inv-sale-invoice-customer-bill-rice-journal-exp-sub-rep", method = RequestMethod.POST)
    public void printInvSaleInvoiceCustomerBillRiceJournalExpSubRep(HttpServletResponse response, @RequestBody(required = false) RptInvSaleInvoiceCustomerBillRiceJournalExpSubRepRequest request) throws Exception {
        if (request == null) request = new RptInvSaleInvoiceCustomerBillRiceJournalExpSubRepRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "InvSaleInvoiceCustomerBillRiceJournalExpSubRep.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/inv-sale-invoice-customer-bill-rice-journal-exp-sub-rep", method = RequestMethod.GET)
    public void printInvSaleInvoiceCustomerBillRiceJournalExpSubRepGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printInvSaleInvoiceCustomerBillRiceJournalExpSubRep(response, objectMapper.convertValue(query, RptInvSaleInvoiceCustomerBillRiceJournalExpSubRepRequest.class));
    }

    /**
     * Template: RptGatepassOutwardDriverInfoSlipA.rpt
     * Procedure: [dbo].[USP_driverBiodata_SlipAndRegister]
     * Desktop: driverBiodata.WorkOrder_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/gatepass-outward-driver-info-slip-a", method = RequestMethod.POST)
    public void printGatepassOutwardDriverInfoSlipA(HttpServletResponse response, @RequestBody(required = false) RptGatepassOutwardDriverInfoSlipARequest request) throws Exception {
        if (request == null) request = new RptGatepassOutwardDriverInfoSlipARequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "RptGatepassOutwardDriverInfoSlipA.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/gatepass-outward-driver-info-slip-a", method = RequestMethod.GET)
    public void printGatepassOutwardDriverInfoSlipAGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printGatepassOutwardDriverInfoSlipA(response, objectMapper.convertValue(query, RptGatepassOutwardDriverInfoSlipARequest.class));
    }

    /**
     * Template: RptGdnSlip.rpt
     * Procedure: Sp_GatePassOutward_GetAllMethod
     * Desktop: GatePassOutward.ReadByGpNoForGdn
     */
    @RequestMapping(value = "/reports/print/gdn-slip", method = RequestMethod.POST)
    public void printGdnSlip(HttpServletResponse response, @RequestBody(required = false) RptGdnSlipRequest request) throws Exception {
        if (request == null) request = new RptGdnSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "RptGdnSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/gdn-slip", method = RequestMethod.GET)
    public void printGdnSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printGdnSlip(response, objectMapper.convertValue(query, RptGdnSlipRequest.class));
    }

    /**
     * Template: SaleInvoiceItemExpense_SubReport.rpt
     * Procedure: [dbo].[USP_InvSaleInvoiceItemExpense_SubReport]
     * Desktop: InvSaleInvoiceReports.SaleInvoiceItemExpenseSubReport
     */
    @RequestMapping(value = "/reports/print/sale-invoice-item-expense-sub-report-2", method = RequestMethod.POST)
    public void printSaleInvoiceItemExpenseSubReportd0cd7874(HttpServletResponse response, @RequestBody(required = false) RptSaleInvoiceItemExpenseSubReportd0cd7874Request request) throws Exception {
        if (request == null) request = new RptSaleInvoiceItemExpenseSubReportd0cd7874Request();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "SaleInvoiceItemExpense_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/sale-invoice-item-expense-sub-report-2", method = RequestMethod.GET)
    public void printSaleInvoiceItemExpenseSubReportd0cd7874Get(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printSaleInvoiceItemExpenseSubReportd0cd7874(response, objectMapper.convertValue(query, RptSaleInvoiceItemExpenseSubReportd0cd7874Request.class));
    }

    /**
     * Template: SaleOrderCustomerExpense_SubReport.rpt
     * Procedure: [dbo].[USP_SaleOrderCustomerExpense_SubReport]
     * Desktop: SaleOrderReports.SaleOrderCustomerExpense_SubReport
     */
    @RequestMapping(value = "/reports/print/sale-order-customer-expense-sub-report", method = RequestMethod.POST)
    public void printSaleOrderCustomerExpenseSubReport(HttpServletResponse response, @RequestBody(required = false) RptSaleOrderCustomerExpenseSubReportRequest request) throws Exception {
        if (request == null) request = new RptSaleOrderCustomerExpenseSubReportRequest();
        printReport(response, "SaleOrderCustomerExpense_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/sale-order-customer-expense-sub-report", method = RequestMethod.GET)
    public void printSaleOrderCustomerExpenseSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printSaleOrderCustomerExpenseSubReport(response, objectMapper.convertValue(query, RptSaleOrderCustomerExpenseSubReportRequest.class));
    }

    /**
     * Template: SaleOrderExtraItemsDetail_SubReport.rpt
     * Procedure: [dbo].[USP_SaleOrderExtraItemsDetail_SubReport]
     * Desktop: SaleOrderReports.SaleOrderExtraItemsDetail_SubReport
     */
    @RequestMapping(value = "/reports/print/sale-order-extra-items-detail-sub-report", method = RequestMethod.POST)
    public void printSaleOrderExtraItemsDetailSubReport(HttpServletResponse response, @RequestBody(required = false) RptSaleOrderExtraItemsDetailSubReportRequest request) throws Exception {
        if (request == null) request = new RptSaleOrderExtraItemsDetailSubReportRequest();
        printReport(response, "SaleOrderExtraItemsDetail_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/sale-order-extra-items-detail-sub-report", method = RequestMethod.GET)
    public void printSaleOrderExtraItemsDetailSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printSaleOrderExtraItemsDetailSubReport(response, objectMapper.convertValue(query, RptSaleOrderExtraItemsDetailSubReportRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
