package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.FeedMillPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** FeedMill print actions. Generated from the verified seeder contracts. */
@Controller
public class FeedMillPrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 1100_PurchaseOrderSlip.rpt
     * Procedure: [fed].[USP_PurchaseOrder_SlipAndRegister]
     * Desktop: PurchaseOrder.PurchaseOrder_Slip
     */
    @RequestMapping(value = "/reports/print/1100-purchase-order-slip", method = RequestMethod.POST)
    public void print1100PurchaseOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1100PurchaseOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1100PurchaseOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1100_PurchaseOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1100-purchase-order-slip", method = RequestMethod.GET)
    public void print1100PurchaseOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1100PurchaseOrderSlip(response, objectMapper.convertValue(query, Rpt1100PurchaseOrderSlipRequest.class));
    }

    /**
     * Template: 1110-SaleOrderSlip.rpt
     * Procedure: [fed].[usp_SaleOrderSlip]
     * Desktop: SaleOrder.SaleOrderSlip
     */
    @RequestMapping(value = "/reports/print/1110-sale-order-slip", method = RequestMethod.POST)
    public void print1110SaleOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1110SaleOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1110SaleOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1110-SaleOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1110-sale-order-slip", method = RequestMethod.GET)
    public void print1110SaleOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1110SaleOrderSlip(response, objectMapper.convertValue(query, Rpt1110SaleOrderSlipRequest.class));
    }

    /**
     * Template: 1110_CustomerDiscountPolicySlip.rpt
     * Procedure: [fed].[USP_CustomerDiscountPolicySlip]
     * Desktop: CustomerDiscountPolicy.CustomerDiscountPolicySlip
     */
    @RequestMapping(value = "/reports/print/1110-customer-discount-policy-slip", method = RequestMethod.POST)
    public void print1110CustomerDiscountPolicySlip(HttpServletResponse response, @RequestBody(required = false) Rpt1110CustomerDiscountPolicySlipRequest request) throws Exception {
        if (request == null) request = new Rpt1110CustomerDiscountPolicySlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1110_CustomerDiscountPolicySlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1110-customer-discount-policy-slip", method = RequestMethod.GET)
    public void print1110CustomerDiscountPolicySlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1110CustomerDiscountPolicySlip(response, objectMapper.convertValue(query, Rpt1110CustomerDiscountPolicySlipRequest.class));
    }

    /**
     * Template: 1115-InvRepSaleBillCustomer.rpt
     * Procedure: fed.usp_InvSaleInvoiceDirectSlip
     * Desktop: InvSaleInvoice.InvSaleInvoiceDirectSlip
     */
    @RequestMapping(value = "/reports/print/1115-inv-rep-sale-bill-customer", method = RequestMethod.POST)
    public void print1115InvRepSaleBillCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt1115InvRepSaleBillCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt1115InvRepSaleBillCustomerRequest();
        printReport(response, "1115-InvRepSaleBillCustomer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1115-inv-rep-sale-bill-customer", method = RequestMethod.GET)
    public void print1115InvRepSaleBillCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1115InvRepSaleBillCustomer(response, objectMapper.convertValue(query, Rpt1115InvRepSaleBillCustomerRequest.class));
    }

    /**
     * Template: 1863-InvRptPurchaseOrderSlip.rpt
     * Procedure: [fed].[USP_PurchaseOrder_SlipAndRegister]
     * Desktop: PurchaseOrder.PurchaseOrder_Slip
     */
    @RequestMapping(value = "/reports/print/1863-purchase-order-slip", method = RequestMethod.POST)
    public void print1863PurchaseOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1863PurchaseOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1863PurchaseOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1863-InvRptPurchaseOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1863-purchase-order-slip", method = RequestMethod.GET)
    public void print1863PurchaseOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1863PurchaseOrderSlip(response, objectMapper.convertValue(query, Rpt1863PurchaseOrderSlipRequest.class));
    }

    /**
     * Template: 481-ItemPricingScheduleListGroupWise.rpt
     * Procedure: fed.usp_GetLastItemRateByItemIdRateUomAndGroupId
     * Desktop: ItemPricingSchedule.GetLastItemRateByItemIdRateUomAndGroupId
     */
    @RequestMapping(value = "/reports/print/481-item-pricing-schedule-list-group-wise", method = RequestMethod.POST)
    public void print481ItemPricingScheduleListGroupWise(HttpServletResponse response, @RequestBody(required = false) Rpt481ItemPricingScheduleListGroupWiseRequest request) throws Exception {
        if (request == null) request = new Rpt481ItemPricingScheduleListGroupWiseRequest();
        printReport(response, "481-ItemPricingScheduleListGroupWise.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/481-item-pricing-schedule-list-group-wise", method = RequestMethod.GET)
    public void print481ItemPricingScheduleListGroupWiseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print481ItemPricingScheduleListGroupWise(response, objectMapper.convertValue(query, Rpt481ItemPricingScheduleListGroupWiseRequest.class));
    }

    /**
     * Template: InvPurchaseInvoice_ItemExpense_SubReport.rpt
     * Procedure: [fed].[USP_InvPurchaseInvoice_ItemExpense_SubReport]
     * Desktop: InvPurchaseInvoice.PurchaseInvoiceSupReprtForItemExpense
     */
    @RequestMapping(value = "/reports/print/inv-purchase-invoice-item-expense-sub-report", method = RequestMethod.POST)
    public void printInvPurchaseInvoiceItemExpenseSubReport(HttpServletResponse response, @RequestBody(required = false) RptInvPurchaseInvoiceItemExpenseSubReportRequest request) throws Exception {
        if (request == null) request = new RptInvPurchaseInvoiceItemExpenseSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "InvPurchaseInvoice_ItemExpense_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/inv-purchase-invoice-item-expense-sub-report", method = RequestMethod.GET)
    public void printInvPurchaseInvoiceItemExpenseSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printInvPurchaseInvoiceItemExpenseSubReport(response, objectMapper.convertValue(query, RptInvPurchaseInvoiceItemExpenseSubReportRequest.class));
    }

    /**
     * Template: PurchaseInvoice_SupplierExpense_SubReport.rpt
     * Procedure: [fed].[USP_InvPurchaseInvoice_SupplierExpense_SubReport]
     * Desktop: InvPurchaseInvoice.PurchaseInvoiceSupReprtForSupplierExpense
     */
    @RequestMapping(value = "/reports/print/purchase-invoice-supplier-expense-sub-report", method = RequestMethod.POST)
    public void printPurchaseInvoiceSupplierExpenseSubReport(HttpServletResponse response, @RequestBody(required = false) RptPurchaseInvoiceSupplierExpenseSubReportRequest request) throws Exception {
        if (request == null) request = new RptPurchaseInvoiceSupplierExpenseSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "PurchaseInvoice_SupplierExpense_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/purchase-invoice-supplier-expense-sub-report", method = RequestMethod.GET)
    public void printPurchaseInvoiceSupplierExpenseSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printPurchaseInvoiceSupplierExpenseSubReport(response, objectMapper.convertValue(query, RptPurchaseInvoiceSupplierExpenseSubReportRequest.class));
    }

    /**
     * Template: SaleOrderPaymentTermsDetail_SubReport.rpt
     * Procedure: [fed].[usp_SaleOrderPaymentTermsDetail_SubReport]
     * Desktop: SaleOrder.SaleOrderPaymentTermDetail_SubReport
     */
    @RequestMapping(value = "/reports/print/sale-order-payment-terms-detail-sub-report", method = RequestMethod.POST)
    public void printSaleOrderPaymentTermsDetailSubReport(HttpServletResponse response, @RequestBody(required = false) RptSaleOrderPaymentTermsDetailSubReportRequest request) throws Exception {
        if (request == null) request = new RptSaleOrderPaymentTermsDetailSubReportRequest();
        printReport(response, "SaleOrderPaymentTermsDetail_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/sale-order-payment-terms-detail-sub-report", method = RequestMethod.GET)
    public void printSaleOrderPaymentTermsDetailSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printSaleOrderPaymentTermsDetailSubReport(response, objectMapper.convertValue(query, RptSaleOrderPaymentTermsDetailSubReportRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
