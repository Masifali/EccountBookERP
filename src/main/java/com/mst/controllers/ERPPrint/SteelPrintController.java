package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.SteelPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** Steel print actions. Generated from the verified seeder contracts. */
@Controller
public class SteelPrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 1500-PurchaseOrderSlip_Rpt.rpt
     * Procedure: [ST].[USP_PurchaseOrderSlip_Rpt]
     * Desktop: PurchaseOrder.PurchaseOrderSlipReport1500
     */
    @RequestMapping(value = "/reports/print/1500-purchase-order-slip", method = RequestMethod.POST)
    public void print1500PurchaseOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1500PurchaseOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1500PurchaseOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1500-PurchaseOrderSlip_Rpt.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1500-purchase-order-slip", method = RequestMethod.GET)
    public void print1500PurchaseOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1500PurchaseOrderSlip(response, objectMapper.convertValue(query, Rpt1500PurchaseOrderSlipRequest.class));
    }

    /**
     * Template: 1501-PurchaseOrder_Register.rpt
     * Procedure: [ST].[USP_PurchaseOrder_Register]
     * Desktop: PurchaseOrder.PurchaseOrderRegisterST
     */
    @RequestMapping(value = "/reports/print/1501-purchase-order-register", method = RequestMethod.POST)
    public void print1501PurchaseOrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1501PurchaseOrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1501PurchaseOrderRegisterRequest();
        printReport(response, "1501-PurchaseOrder_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1501-purchase-order-register", method = RequestMethod.GET)
    public void print1501PurchaseOrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1501PurchaseOrderRegister(response, objectMapper.convertValue(query, Rpt1501PurchaseOrderRegisterRequest.class));
    }

    /**
     * Template: 1502-PurchaseOrder_Register.rpt
     * Procedure: [ST].[USP_PurchaseOrder_Register]
     * Desktop: PurchaseOrder.PurchaseOrderRegisterST
     */
    @RequestMapping(value = "/reports/print/1502-purchase-order-register", method = RequestMethod.POST)
    public void print1502PurchaseOrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1502PurchaseOrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1502PurchaseOrderRegisterRequest();
        printReport(response, "1502-PurchaseOrder_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1502-purchase-order-register", method = RequestMethod.GET)
    public void print1502PurchaseOrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1502PurchaseOrderRegister(response, objectMapper.convertValue(query, Rpt1502PurchaseOrderRegisterRequest.class));
    }

    /**
     * Template: 1503-InvGrnSlip.rpt
     * Procedure: [ST].[USp_InvGrnSlipAndRegister]
     * Desktop: InvGrn.InvGrnSlip1503
     */
    @RequestMapping(value = "/reports/print/1503-inv-grn-slip", method = RequestMethod.POST)
    public void print1503InvGrnSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1503InvGrnSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1503InvGrnSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1503-InvGrnSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1503-inv-grn-slip", method = RequestMethod.GET)
    public void print1503InvGrnSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1503InvGrnSlip(response, objectMapper.convertValue(query, Rpt1503InvGrnSlipRequest.class));
    }

    /**
     * Template: 1504-InvGrnSlipAndRegister.rpt
     * Procedure: [ST].[USp_InvGrnSlipAndRegister]
     * Desktop: InvGrn.InvGrnSlip1503
     */
    @RequestMapping(value = "/reports/print/1504-inv-grn-slip-and-register", method = RequestMethod.POST)
    public void print1504InvGrnSlipAndRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1504InvGrnSlipAndRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1504InvGrnSlipAndRegisterRequest();
        printReport(response, "1504-InvGrnSlipAndRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1504-inv-grn-slip-and-register", method = RequestMethod.GET)
    public void print1504InvGrnSlipAndRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1504InvGrnSlipAndRegister(response, objectMapper.convertValue(query, Rpt1504InvGrnSlipAndRegisterRequest.class));
    }

    /**
     * Template: 1505-InvPurchaseInvoiceRegister.rpt
     * Procedure: [ST].[USP_InvPurchaseInvoiceRegister]
     * Desktop: InvPurchaseInvoice.PurchaseInvoiceRegisterSteel
     */
    @RequestMapping(value = "/reports/print/1505-inv-purchase-invoice-register", method = RequestMethod.POST)
    public void print1505InvPurchaseInvoiceRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1505InvPurchaseInvoiceRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1505InvPurchaseInvoiceRegisterRequest();
        printReport(response, "1505-InvPurchaseInvoiceRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1505-inv-purchase-invoice-register", method = RequestMethod.GET)
    public void print1505InvPurchaseInvoiceRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1505InvPurchaseInvoiceRegister(response, objectMapper.convertValue(query, Rpt1505InvPurchaseInvoiceRegisterRequest.class));
    }

    /**
     * Template: 1506-PurchaseInvoiceRegisterAvgRateItemSupplier.rpt
     * Procedure: [ST].[USP_InvPurchaseInvoiceRegister_AvgRatesComparisonsByItemSupplier]
     * Desktop: InvPurchaseInvoice.PurchaseInvoiceAvgRateBySupplierForSteel
     */
    @RequestMapping(value = "/reports/print/1506-purchase-invoice-register-avg-rate-item-supplier", method = RequestMethod.POST)
    public void print1506PurchaseInvoiceRegisterAvgRateItemSupplier(HttpServletResponse response, @RequestBody(required = false) Rpt1506PurchaseInvoiceRegisterAvgRateItemSupplierRequest request) throws Exception {
        if (request == null) request = new Rpt1506PurchaseInvoiceRegisterAvgRateItemSupplierRequest();
        printReport(response, "1506-PurchaseInvoiceRegisterAvgRateItemSupplier.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1506-purchase-invoice-register-avg-rate-item-supplier", method = RequestMethod.GET)
    public void print1506PurchaseInvoiceRegisterAvgRateItemSupplierGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1506PurchaseInvoiceRegisterAvgRateItemSupplier(response, objectMapper.convertValue(query, Rpt1506PurchaseInvoiceRegisterAvgRateItemSupplierRequest.class));
    }

    /**
     * Template: 1507-PurchaseInvoiceRegisterAvgRateByItem.rpt
     * Procedure: [ST].[Sp_InvPurchaseInvoiceRegister_AvgRatesComparisonsByItem]
     * Desktop: InvPurchaseInvoice.PurchaseInvoiceAvgRateByItemForSteel
     */
    @RequestMapping(value = "/reports/print/1507-purchase-invoice-register-avg-rate-by-item", method = RequestMethod.POST)
    public void print1507PurchaseInvoiceRegisterAvgRateByItem(HttpServletResponse response, @RequestBody(required = false) Rpt1507PurchaseInvoiceRegisterAvgRateByItemRequest request) throws Exception {
        if (request == null) request = new Rpt1507PurchaseInvoiceRegisterAvgRateByItemRequest();
        printReport(response, "1507-PurchaseInvoiceRegisterAvgRateByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1507-purchase-invoice-register-avg-rate-by-item", method = RequestMethod.GET)
    public void print1507PurchaseInvoiceRegisterAvgRateByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1507PurchaseInvoiceRegisterAvgRateByItem(response, objectMapper.convertValue(query, Rpt1507PurchaseInvoiceRegisterAvgRateByItemRequest.class));
    }

    /**
     * Template: 1508-PurchaseBillSupplierRiceItemSlip.rpt
     * Procedure: [ST].[USP_InvPurchaseInvoice_PartySlip]
     * Desktop: InvPurchaseInvoice.InvPurchaseInvoiceSlipReport
     */
    @RequestMapping(value = "/reports/print/1508-purchase-bill-supplier-rice-item-slip", method = RequestMethod.POST)
    public void print1508PurchaseBillSupplierRiceItemSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1508PurchaseBillSupplierRiceItemSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1508PurchaseBillSupplierRiceItemSlipRequest();
        printReport(response, "1508-PurchaseBillSupplierRiceItemSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1508-purchase-bill-supplier-rice-item-slip", method = RequestMethod.GET)
    public void print1508PurchaseBillSupplierRiceItemSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1508PurchaseBillSupplierRiceItemSlip(response, objectMapper.convertValue(query, Rpt1508PurchaseBillSupplierRiceItemSlipRequest.class));
    }

    /**
     * Template: 1508A-InvPurchaseInvoicePartySlip.rpt
     * Procedure: [ST].[USP_InvPurchaseInvoice_PartySlip]
     * Desktop: InvPurchaseInvoice.InvPurchaseInvoiceSlipReport
     */
    @RequestMapping(value = "/reports/print/1508a-inv-purchase-invoice-party-slip", method = RequestMethod.POST)
    public void print1508AInvPurchaseInvoicePartySlip(HttpServletResponse response, @RequestBody(required = false) Rpt1508AInvPurchaseInvoicePartySlipRequest request) throws Exception {
        if (request == null) request = new Rpt1508AInvPurchaseInvoicePartySlipRequest();
        printReport(response, "1508A-InvPurchaseInvoicePartySlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1508a-inv-purchase-invoice-party-slip", method = RequestMethod.GET)
    public void print1508AInvPurchaseInvoicePartySlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1508AInvPurchaseInvoicePartySlip(response, objectMapper.convertValue(query, Rpt1508AInvPurchaseInvoicePartySlipRequest.class));
    }

    /**
     * Template: 1509-SaleOrderSlipAndRegister.rpt
     * Procedure: [ST].[USP_SaleOrderSlipAndRegister]
     * Desktop: SaleOrder.InvSaleOrderSlipRegisterForSteel
     */
    @RequestMapping(value = "/reports/print/1509-sale-order-slip-and-register", method = RequestMethod.POST)
    public void print1509SaleOrderSlipAndRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1509SaleOrderSlipAndRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1509SaleOrderSlipAndRegisterRequest();
        printReport(response, "1509-SaleOrderSlipAndRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1509-sale-order-slip-and-register", method = RequestMethod.GET)
    public void print1509SaleOrderSlipAndRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1509SaleOrderSlipAndRegister(response, objectMapper.convertValue(query, Rpt1509SaleOrderSlipAndRegisterRequest.class));
    }

    /**
     * Template: 1510-PurchaseInvoiceDirectPartySlip.rpt
     * Procedure: [ST].[USP_InvPurchaseInvoiceDirect_PartySlip]
     * Desktop: InvPurchaseInvoice.InvPurchaseInvoiceDirectSlipReport
     */
    @RequestMapping(value = "/reports/print/1510-purchase-invoice-direct-party-slip", method = RequestMethod.POST)
    public void print1510PurchaseInvoiceDirectPartySlip(HttpServletResponse response, @RequestBody(required = false) Rpt1510PurchaseInvoiceDirectPartySlipRequest request) throws Exception {
        if (request == null) request = new Rpt1510PurchaseInvoiceDirectPartySlipRequest();
        printReport(response, "1510-PurchaseInvoiceDirectPartySlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1510-purchase-invoice-direct-party-slip", method = RequestMethod.GET)
    public void print1510PurchaseInvoiceDirectPartySlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1510PurchaseInvoiceDirectPartySlip(response, objectMapper.convertValue(query, Rpt1510PurchaseInvoiceDirectPartySlipRequest.class));
    }

    /**
     * Template: 1510A-PurchaseInvoiceDirectPartySlip.rpt
     * Procedure: [ST].[USP_InvPurchaseInvoiceDirect_PartySlip]
     * Desktop: InvPurchaseInvoice.InvPurchaseInvoiceDirectSlipReport
     */
    @RequestMapping(value = "/reports/print/1510a-purchase-invoice-direct-party-slip", method = RequestMethod.POST)
    public void print1510APurchaseInvoiceDirectPartySlip(HttpServletResponse response, @RequestBody(required = false) Rpt1510APurchaseInvoiceDirectPartySlipRequest request) throws Exception {
        if (request == null) request = new Rpt1510APurchaseInvoiceDirectPartySlipRequest();
        printReport(response, "1510A-PurchaseInvoiceDirectPartySlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1510a-purchase-invoice-direct-party-slip", method = RequestMethod.GET)
    public void print1510APurchaseInvoiceDirectPartySlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1510APurchaseInvoiceDirectPartySlip(response, objectMapper.convertValue(query, Rpt1510APurchaseInvoiceDirectPartySlipRequest.class));
    }

    /**
     * Template: 1511-SaleOrderSlipAndRegister.rpt
     * Procedure: [ST].[USP_SaleOrderSlipAndRegister]
     * Desktop: SaleOrder.InvSaleOrderSlipRegisterForSteel
     */
    @RequestMapping(value = "/reports/print/1511-sale-order-slip-and-register", method = RequestMethod.POST)
    public void print1511SaleOrderSlipAndRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1511SaleOrderSlipAndRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1511SaleOrderSlipAndRegisterRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1511-SaleOrderSlipAndRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1511-sale-order-slip-and-register", method = RequestMethod.GET)
    public void print1511SaleOrderSlipAndRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1511SaleOrderSlipAndRegister(response, objectMapper.convertValue(query, Rpt1511SaleOrderSlipAndRegisterRequest.class));
    }

    /**
     * Template: 1512-FoodProductionSlip.rpt
     * Procedure: [ST].[USP_FoodProduction_Slip]
     * Desktop: ProductionReports.FoodProductionSlip
     */
    @RequestMapping(value = "/reports/print/1512-food-production-slip", method = RequestMethod.POST)
    public void print1512FoodProductionSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1512FoodProductionSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1512FoodProductionSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1512-FoodProductionSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1512-food-production-slip", method = RequestMethod.GET)
    public void print1512FoodProductionSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1512FoodProductionSlip(response, objectMapper.convertValue(query, Rpt1512FoodProductionSlipRequest.class));
    }

    /**
     * Template: 1512-GatePassOutwardSlipAndRegisterSteel.rpt
     * Procedure: [ST].[USp_GatePassOutward_SlipAndRegisterSteel_Rpt]
     * Desktop: GatePassOutwardReports.GatePassOutwardSlipandRegisterForSteel
     */
    @RequestMapping(value = "/reports/print/1512-gate-pass-outward-slip-and-register-steel", method = RequestMethod.POST)
    public void print1512GatePassOutwardSlipAndRegisterSteel(HttpServletResponse response, @RequestBody(required = false) Rpt1512GatePassOutwardSlipAndRegisterSteelRequest request) throws Exception {
        if (request == null) request = new Rpt1512GatePassOutwardSlipAndRegisterSteelRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1512-GatePassOutwardSlipAndRegisterSteel.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1512-gate-pass-outward-slip-and-register-steel", method = RequestMethod.GET)
    public void print1512GatePassOutwardSlipAndRegisterSteelGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1512GatePassOutwardSlipAndRegisterSteel(response, objectMapper.convertValue(query, Rpt1512GatePassOutwardSlipAndRegisterSteelRequest.class));
    }

    /**
     * Template: 1513-DeliveryOrderSlip.rpt
     * Procedure: [ST].[USP_DeliveryOrder_SlipReport]
     * Desktop: InvDeliveryOrder.DeliveryOrderSlipForSteel
     */
    @RequestMapping(value = "/reports/print/1513-delivery-order-slip", method = RequestMethod.POST)
    public void print1513DeliveryOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1513DeliveryOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1513DeliveryOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1513-DeliveryOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1513-delivery-order-slip", method = RequestMethod.GET)
    public void print1513DeliveryOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1513DeliveryOrderSlip(response, objectMapper.convertValue(query, Rpt1513DeliveryOrderSlipRequest.class));
    }

    /**
     * Template: 1513-FoodProductionSummery_WithOutValue.rpt
     * Procedure: [ST].[USP_FoodProductionSummery_WithOutValue]
     * Desktop: ProductionReports.InvFoodProduction_Summery_WithOutValue_Rpt
     */
    @RequestMapping(value = "/reports/print/1513-food-production-summery-with-out-value", method = RequestMethod.POST)
    public void print1513FoodProductionSummeryWithOutValue(HttpServletResponse response, @RequestBody(required = false) Rpt1513FoodProductionSummeryWithOutValueRequest request) throws Exception {
        if (request == null) request = new Rpt1513FoodProductionSummeryWithOutValueRequest();
        printReport(response, "1513-FoodProductionSummery_WithOutValue.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1513-food-production-summery-with-out-value", method = RequestMethod.GET)
    public void print1513FoodProductionSummeryWithOutValueGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1513FoodProductionSummeryWithOutValue(response, objectMapper.convertValue(query, Rpt1513FoodProductionSummeryWithOutValueRequest.class));
    }

    /**
     * Template: 1513A-DeliveryOrderSlip&Register.rpt
     * Procedure: [ST].[USp_DeliveryOrderRegister]
     * Desktop: InvDeliveryOrder.DeliveryOrderRegister
     */
    @RequestMapping(value = "/reports/print/1513a-delivery-order-slip-register", method = RequestMethod.POST)
    public void print1513ADeliveryOrderSlipRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1513ADeliveryOrderSlipRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1513ADeliveryOrderSlipRegisterRequest();
        printReport(response, "1513A-DeliveryOrderSlip&Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1513a-delivery-order-slip-register", method = RequestMethod.GET)
    public void print1513ADeliveryOrderSlipRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1513ADeliveryOrderSlipRegister(response, objectMapper.convertValue(query, Rpt1513ADeliveryOrderSlipRegisterRequest.class));
    }

    /**
     * Template: 1513A_ProductionSummeryWithValues.rpt
     * Procedure: [ST].[USP_FoodProduction_Summery_WithValues]
     * Desktop: InvFoodProduction.InvFoodProductionRecoverySummeryReport
     */
    @RequestMapping(value = "/reports/print/1513a-production-summery-with-values", method = RequestMethod.POST)
    public void print1513AProductionSummeryWithValues(HttpServletResponse response, @RequestBody(required = false) Rpt1513AProductionSummeryWithValuesRequest request) throws Exception {
        if (request == null) request = new Rpt1513AProductionSummeryWithValuesRequest();
        printReport(response, "1513A_ProductionSummeryWithValues.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1513a-production-summery-with-values", method = RequestMethod.GET)
    public void print1513AProductionSummeryWithValuesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1513AProductionSummeryWithValues(response, objectMapper.convertValue(query, Rpt1513AProductionSummeryWithValuesRequest.class));
    }

    /**
     * Template: 1514-FoodProductionOverHeadJobOrderWise_Slip.rpt
     * Procedure: [ST].[USP_FoodProductionOverHeadJobOrderWise_Slip]
     * Desktop: ProductionReports.InvFoodProductionOverHeadReportByJobOrder
     */
    @RequestMapping(value = "/reports/print/1514-food-production-over-head-job-order-wise-slip", method = RequestMethod.POST)
    public void print1514FoodProductionOverHeadJobOrderWiseSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1514FoodProductionOverHeadJobOrderWiseSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1514FoodProductionOverHeadJobOrderWiseSlipRequest();
        printReport(response, "1514-FoodProductionOverHeadJobOrderWise_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1514-food-production-over-head-job-order-wise-slip", method = RequestMethod.GET)
    public void print1514FoodProductionOverHeadJobOrderWiseSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1514FoodProductionOverHeadJobOrderWiseSlip(response, objectMapper.convertValue(query, Rpt1514FoodProductionOverHeadJobOrderWiseSlipRequest.class));
    }

    /**
     * Template: 1514-InvRepSaleBillCustomer.rpt
     * Procedure: [ST].[USP_InvSaleInvoice_CustomerBill]
     * Desktop: InvSaleInvoice.SaleInvoiceSlipReport
     */
    @RequestMapping(value = "/reports/print/1514-inv-rep-sale-bill-customer", method = RequestMethod.POST)
    public void print1514InvRepSaleBillCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt1514InvRepSaleBillCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt1514InvRepSaleBillCustomerRequest();
        printReport(response, "1514-InvRepSaleBillCustomer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1514-inv-rep-sale-bill-customer", method = RequestMethod.GET)
    public void print1514InvRepSaleBillCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1514InvRepSaleBillCustomer(response, objectMapper.convertValue(query, Rpt1514InvRepSaleBillCustomerRequest.class));
    }

    /**
     * Template: 1514A-SaleBillCustomer-Format-II.rpt
     * Procedure: [ST].[USP_InvSaleInvoice_CustomerBill]
     * Desktop: InvSaleInvoice.SaleInvoiceSlipReport
     */
    @RequestMapping(value = "/reports/print/1514a-sale-bill-customer-format-ii", method = RequestMethod.POST)
    public void print1514ASaleBillCustomerFormatII(HttpServletResponse response, @RequestBody(required = false) Rpt1514ASaleBillCustomerFormatIIRequest request) throws Exception {
        if (request == null) request = new Rpt1514ASaleBillCustomerFormatIIRequest();
        printReport(response, "1514A-SaleBillCustomer-Format-II.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1514a-sale-bill-customer-format-ii", method = RequestMethod.GET)
    public void print1514ASaleBillCustomerFormatIIGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1514ASaleBillCustomerFormatII(response, objectMapper.convertValue(query, Rpt1514ASaleBillCustomerFormatIIRequest.class));
    }

    /**
     * Template: 1515-InvRptGdnSlipAndRegister.rpt
     * Procedure: [ST].[USP_InvGdn_SlipAndRegisterReport]
     * Desktop: InvGdn.InvGdnSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1515-gdn-slip-and-register", method = RequestMethod.POST)
    public void print1515GdnSlipAndRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1515GdnSlipAndRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1515GdnSlipAndRegisterRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1515-InvRptGdnSlipAndRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1515-gdn-slip-and-register", method = RequestMethod.GET)
    public void print1515GdnSlipAndRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1515GdnSlipAndRegister(response, objectMapper.convertValue(query, Rpt1515GdnSlipAndRegisterRequest.class));
    }

    /**
     * Template: 1515A-InvRptGdnSlipDliveryChallan.rpt
     * Procedure: [ST].[USP_InvGdn_SlipAndRegisterReport]
     * Desktop: InvGdn.InvGdnSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1515a-gdn-slip-dlivery-challan", method = RequestMethod.POST)
    public void print1515AGdnSlipDliveryChallan(HttpServletResponse response, @RequestBody(required = false) Rpt1515AGdnSlipDliveryChallanRequest request) throws Exception {
        if (request == null) request = new Rpt1515AGdnSlipDliveryChallanRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1515A-InvRptGdnSlipDliveryChallan.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1515a-gdn-slip-dlivery-challan", method = RequestMethod.GET)
    public void print1515AGdnSlipDliveryChallanGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1515AGdnSlipDliveryChallan(response, objectMapper.convertValue(query, Rpt1515AGdnSlipDliveryChallanRequest.class));
    }

    /**
     * Template: 1516-SaleInvoiceCustomerBill.rpt
     * Procedure: [ST].[USP_InvSaleInvoice_CustomerDirectBill]
     * Desktop: InvSaleInvoice.SaleInvoiceDirectSlipReport
     */
    @RequestMapping(value = "/reports/print/1516-sale-invoice-customer-bill", method = RequestMethod.POST)
    public void print1516SaleInvoiceCustomerBill(HttpServletResponse response, @RequestBody(required = false) Rpt1516SaleInvoiceCustomerBillRequest request) throws Exception {
        if (request == null) request = new Rpt1516SaleInvoiceCustomerBillRequest();
        printReport(response, "1516-SaleInvoiceCustomerBill.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1516-sale-invoice-customer-bill", method = RequestMethod.GET)
    public void print1516SaleInvoiceCustomerBillGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1516SaleInvoiceCustomerBill(response, objectMapper.convertValue(query, Rpt1516SaleInvoiceCustomerBillRequest.class));
    }

    /**
     * Template: 1516A-SaleBillDirectSupplierBill.rpt
     * Procedure: [ST].[USP_InvSaleInvoice_CustomerDirectBill]
     * Desktop: InvSaleInvoice.SaleInvoiceDirectSlipReport
     */
    @RequestMapping(value = "/reports/print/1516a-sale-bill-direct-supplier-bill", method = RequestMethod.POST)
    public void print1516ASaleBillDirectSupplierBill(HttpServletResponse response, @RequestBody(required = false) Rpt1516ASaleBillDirectSupplierBillRequest request) throws Exception {
        if (request == null) request = new Rpt1516ASaleBillDirectSupplierBillRequest();
        printReport(response, "1516A-SaleBillDirectSupplierBill.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1516a-sale-bill-direct-supplier-bill", method = RequestMethod.GET)
    public void print1516ASaleBillDirectSupplierBillGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1516ASaleBillDirectSupplierBill(response, objectMapper.convertValue(query, Rpt1516ASaleBillDirectSupplierBillRequest.class));
    }

    /**
     * Template: 1517-DeliveryOrderRegister.rpt
     * Procedure: [ST].[USp_DeliveryOrderRegister]
     * Desktop: InvDeliveryOrder.DeliveryOrderRegister
     */
    @RequestMapping(value = "/reports/print/1517-delivery-order-register", method = RequestMethod.POST)
    public void print1517DeliveryOrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1517DeliveryOrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1517DeliveryOrderRegisterRequest();
        printReport(response, "1517-DeliveryOrderRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1517-delivery-order-register", method = RequestMethod.GET)
    public void print1517DeliveryOrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1517DeliveryOrderRegister(response, objectMapper.convertValue(query, Rpt1517DeliveryOrderRegisterRequest.class));
    }

    /**
     * Template: 1520-InvRptGatePassOutwardRegister.rpt
     * Procedure: [ST].[USp_GatePassOutward_SlipAndRegisterSteel_Rpt]
     * Desktop: GatePassOutwardReports.GatePassOutwardSlipandRegisterForSteel
     */
    @RequestMapping(value = "/reports/print/1520-gate-pass-outward-register", method = RequestMethod.POST)
    public void print1520GatePassOutwardRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1520GatePassOutwardRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1520GatePassOutwardRegisterRequest();
        printReport(response, "1520-InvRptGatePassOutwardRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1520-gate-pass-outward-register", method = RequestMethod.GET)
    public void print1520GatePassOutwardRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1520GatePassOutwardRegister(response, objectMapper.convertValue(query, Rpt1520GatePassOutwardRegisterRequest.class));
    }

    /**
     * Template: 1521-InvGdn_SlipAndRegisterReport.rpt
     * Procedure: [ST].[USP_InvGdn_SlipAndRegisterReport]
     * Desktop: InvGdn.InvGdnSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1521-inv-gdn-slip-and-register-report", method = RequestMethod.POST)
    public void print1521InvGdnSlipAndRegisterReport(HttpServletResponse response, @RequestBody(required = false) Rpt1521InvGdnSlipAndRegisterReportRequest request) throws Exception {
        if (request == null) request = new Rpt1521InvGdnSlipAndRegisterReportRequest();
        printReport(response, "1521-InvGdn_SlipAndRegisterReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1521-inv-gdn-slip-and-register-report", method = RequestMethod.GET)
    public void print1521InvGdnSlipAndRegisterReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1521InvGdnSlipAndRegisterReport(response, objectMapper.convertValue(query, Rpt1521InvGdnSlipAndRegisterReportRequest.class));
    }

    /**
     * Template: 1522-SalesRegisterSummary.rpt
     * Procedure: [ST].[USP-SaleInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.SaleInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1522-sales-register-summary", method = RequestMethod.POST)
    public void print1522SalesRegisterSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1522SalesRegisterSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1522SalesRegisterSummaryRequest();
        printReport(response, "1522-SalesRegisterSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1522-sales-register-summary", method = RequestMethod.GET)
    public void print1522SalesRegisterSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1522SalesRegisterSummary(response, objectMapper.convertValue(query, Rpt1522SalesRegisterSummaryRequest.class));
    }

    /**
     * Template: 1523-SalesRegisterSummaryByCustomer.rpt
     * Procedure: [ST].[USP-SaleInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.SaleInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1523-sales-register-summary-by-customer", method = RequestMethod.POST)
    public void print1523SalesRegisterSummaryByCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt1523SalesRegisterSummaryByCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt1523SalesRegisterSummaryByCustomerRequest();
        printReport(response, "1523-SalesRegisterSummaryByCustomer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1523-sales-register-summary-by-customer", method = RequestMethod.GET)
    public void print1523SalesRegisterSummaryByCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1523SalesRegisterSummaryByCustomer(response, objectMapper.convertValue(query, Rpt1523SalesRegisterSummaryByCustomerRequest.class));
    }

    /**
     * Template: 1524-SalesRegisterSummaryByItem.rpt
     * Procedure: [ST].[USP-SaleInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.SaleInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1524-sales-register-summary-by-item", method = RequestMethod.POST)
    public void print1524SalesRegisterSummaryByItem(HttpServletResponse response, @RequestBody(required = false) Rpt1524SalesRegisterSummaryByItemRequest request) throws Exception {
        if (request == null) request = new Rpt1524SalesRegisterSummaryByItemRequest();
        printReport(response, "1524-SalesRegisterSummaryByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1524-sales-register-summary-by-item", method = RequestMethod.GET)
    public void print1524SalesRegisterSummaryByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1524SalesRegisterSummaryByItem(response, objectMapper.convertValue(query, Rpt1524SalesRegisterSummaryByItemRequest.class));
    }

    /**
     * Template: 1524_04-PurchaseRegisterSummaryByItem&PackSize.rpt
     * Procedure: [ST].[USP-PurchaseInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.PurchaseInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1524-04-purchase-register-summary-by-item-pack-size", method = RequestMethod.POST)
    public void print152404PurchaseRegisterSummaryByItemPackSize(HttpServletResponse response, @RequestBody(required = false) Rpt152404PurchaseRegisterSummaryByItemPackSizeRequest request) throws Exception {
        if (request == null) request = new Rpt152404PurchaseRegisterSummaryByItemPackSizeRequest();
        printReport(response, "1524_04-PurchaseRegisterSummaryByItem&PackSize.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1524-04-purchase-register-summary-by-item-pack-size", method = RequestMethod.GET)
    public void print152404PurchaseRegisterSummaryByItemPackSizeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print152404PurchaseRegisterSummaryByItemPackSize(response, objectMapper.convertValue(query, Rpt152404PurchaseRegisterSummaryByItemPackSizeRequest.class));
    }

    /**
     * Template: 1525-SalesRegisterSummaryByCustomer&Item.rpt
     * Procedure: [ST].[USP-SaleInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.SaleInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1525-sales-register-summary-by-customer-item", method = RequestMethod.POST)
    public void print1525SalesRegisterSummaryByCustomerItem(HttpServletResponse response, @RequestBody(required = false) Rpt1525SalesRegisterSummaryByCustomerItemRequest request) throws Exception {
        if (request == null) request = new Rpt1525SalesRegisterSummaryByCustomerItemRequest();
        printReport(response, "1525-SalesRegisterSummaryByCustomer&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1525-sales-register-summary-by-customer-item", method = RequestMethod.GET)
    public void print1525SalesRegisterSummaryByCustomerItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1525SalesRegisterSummaryByCustomerItem(response, objectMapper.convertValue(query, Rpt1525SalesRegisterSummaryByCustomerItemRequest.class));
    }

    /**
     * Template: 1526-SalesRegisterSummaryByItemWithoutPacking.rpt
     * Procedure: [ST].[USP-SaleInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.SaleInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1526-sales-register-summary-by-item-without-packing", method = RequestMethod.POST)
    public void print1526SalesRegisterSummaryByItemWithoutPacking(HttpServletResponse response, @RequestBody(required = false) Rpt1526SalesRegisterSummaryByItemWithoutPackingRequest request) throws Exception {
        if (request == null) request = new Rpt1526SalesRegisterSummaryByItemWithoutPackingRequest();
        printReport(response, "1526-SalesRegisterSummaryByItemWithoutPacking.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1526-sales-register-summary-by-item-without-packing", method = RequestMethod.GET)
    public void print1526SalesRegisterSummaryByItemWithoutPackingGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1526SalesRegisterSummaryByItemWithoutPacking(response, objectMapper.convertValue(query, Rpt1526SalesRegisterSummaryByItemWithoutPackingRequest.class));
    }

    /**
     * Template: 1527-SalesRegisterSummaryByWarehouse.rpt
     * Procedure: [ST].[USP-SaleInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.SaleInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1527-sales-register-summary-by-warehouse", method = RequestMethod.POST)
    public void print1527SalesRegisterSummaryByWarehouse(HttpServletResponse response, @RequestBody(required = false) Rpt1527SalesRegisterSummaryByWarehouseRequest request) throws Exception {
        if (request == null) request = new Rpt1527SalesRegisterSummaryByWarehouseRequest();
        printReport(response, "1527-SalesRegisterSummaryByWarehouse.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1527-sales-register-summary-by-warehouse", method = RequestMethod.GET)
    public void print1527SalesRegisterSummaryByWarehouseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1527SalesRegisterSummaryByWarehouse(response, objectMapper.convertValue(query, Rpt1527SalesRegisterSummaryByWarehouseRequest.class));
    }

    /**
     * Template: 1528-PurchaseRegisterSummary.rpt
     * Procedure: [ST].[USP-PurchaseInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.PurchaseInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1528-purchase-register-summary", method = RequestMethod.POST)
    public void print1528PurchaseRegisterSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1528PurchaseRegisterSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1528PurchaseRegisterSummaryRequest();
        printReport(response, "1528-PurchaseRegisterSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1528-purchase-register-summary", method = RequestMethod.GET)
    public void print1528PurchaseRegisterSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1528PurchaseRegisterSummary(response, objectMapper.convertValue(query, Rpt1528PurchaseRegisterSummaryRequest.class));
    }

    /**
     * Template: 1528-SalesSummaryByItem&City.rpt
     * Procedure: [ST].[USP-SaleInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.SaleInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1528-sales-summary-by-item-city", method = RequestMethod.POST)
    public void print1528SalesSummaryByItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt1528SalesSummaryByItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt1528SalesSummaryByItemCityRequest();
        printReport(response, "1528-SalesSummaryByItem&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1528-sales-summary-by-item-city", method = RequestMethod.GET)
    public void print1528SalesSummaryByItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1528SalesSummaryByItemCity(response, objectMapper.convertValue(query, Rpt1528SalesSummaryByItemCityRequest.class));
    }

    /**
     * Template: 1528_01-PurchaseRegisterSummaryBySupplier.rpt
     * Procedure: [ST].[USP-PurchaseInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.PurchaseInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1528-01-purchase-register-summary-by-supplier", method = RequestMethod.POST)
    public void print152801PurchaseRegisterSummaryBySupplier(HttpServletResponse response, @RequestBody(required = false) Rpt152801PurchaseRegisterSummaryBySupplierRequest request) throws Exception {
        if (request == null) request = new Rpt152801PurchaseRegisterSummaryBySupplierRequest();
        printReport(response, "1528_01-PurchaseRegisterSummaryBySupplier.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1528-01-purchase-register-summary-by-supplier", method = RequestMethod.GET)
    public void print152801PurchaseRegisterSummaryBySupplierGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print152801PurchaseRegisterSummaryBySupplier(response, objectMapper.convertValue(query, Rpt152801PurchaseRegisterSummaryBySupplierRequest.class));
    }

    /**
     * Template: 1528_02-PurchaseRegisterSummaryByItem.rpt
     * Procedure: [ST].[USP-PurchaseInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.PurchaseInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1528-02-purchase-register-summary-by-item", method = RequestMethod.POST)
    public void print152802PurchaseRegisterSummaryByItem(HttpServletResponse response, @RequestBody(required = false) Rpt152802PurchaseRegisterSummaryByItemRequest request) throws Exception {
        if (request == null) request = new Rpt152802PurchaseRegisterSummaryByItemRequest();
        printReport(response, "1528_02-PurchaseRegisterSummaryByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1528-02-purchase-register-summary-by-item", method = RequestMethod.GET)
    public void print152802PurchaseRegisterSummaryByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print152802PurchaseRegisterSummaryByItem(response, objectMapper.convertValue(query, Rpt152802PurchaseRegisterSummaryByItemRequest.class));
    }

    /**
     * Template: 1528_03-PurchaseRegisterSummaryByItem&City.rpt
     * Procedure: [ST].[USP-PurchaseInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.PurchaseInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1528-03-purchase-register-summary-by-item-city", method = RequestMethod.POST)
    public void print152803PurchaseRegisterSummaryByItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt152803PurchaseRegisterSummaryByItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt152803PurchaseRegisterSummaryByItemCityRequest();
        printReport(response, "1528_03-PurchaseRegisterSummaryByItem&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1528-03-purchase-register-summary-by-item-city", method = RequestMethod.GET)
    public void print152803PurchaseRegisterSummaryByItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print152803PurchaseRegisterSummaryByItemCity(response, objectMapper.convertValue(query, Rpt152803PurchaseRegisterSummaryByItemCityRequest.class));
    }

    /**
     * Template: 1528_05-SalesRegisterSummaryByItem&Warehouse.rpt
     * Procedure: [ST].[USP-PurchaseInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.PurchaseInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1528-05-sales-register-summary-by-item-warehouse", method = RequestMethod.POST)
    public void print152805SalesRegisterSummaryByItemWarehouse(HttpServletResponse response, @RequestBody(required = false) Rpt152805SalesRegisterSummaryByItemWarehouseRequest request) throws Exception {
        if (request == null) request = new Rpt152805SalesRegisterSummaryByItemWarehouseRequest();
        printReport(response, "1528_05-SalesRegisterSummaryByItem&Warehouse.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1528-05-sales-register-summary-by-item-warehouse", method = RequestMethod.GET)
    public void print152805SalesRegisterSummaryByItemWarehouseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print152805SalesRegisterSummaryByItemWarehouse(response, objectMapper.convertValue(query, Rpt152805SalesRegisterSummaryByItemWarehouseRequest.class));
    }

    /**
     * Template: 1528_06-PurchaseSummaryByItemPackSize&City.rpt
     * Procedure: [ST].[USP-PurchaseInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.PurchaseInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1528-06-purchase-summary-by-item-pack-size-city", method = RequestMethod.POST)
    public void print152806PurchaseSummaryByItemPackSizeCity(HttpServletResponse response, @RequestBody(required = false) Rpt152806PurchaseSummaryByItemPackSizeCityRequest request) throws Exception {
        if (request == null) request = new Rpt152806PurchaseSummaryByItemPackSizeCityRequest();
        printReport(response, "1528_06-PurchaseSummaryByItemPackSize&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1528-06-purchase-summary-by-item-pack-size-city", method = RequestMethod.GET)
    public void print152806PurchaseSummaryByItemPackSizeCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print152806PurchaseSummaryByItemPackSizeCity(response, objectMapper.convertValue(query, Rpt152806PurchaseSummaryByItemPackSizeCityRequest.class));
    }

    /**
     * Template: 1528_07-PurchaseRegisterSummaryBySupplier&Item.rpt
     * Procedure: [ST].[USP-PurchaseInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.PurchaseInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1528-07-purchase-register-summary-by-supplier-item", method = RequestMethod.POST)
    public void print152807PurchaseRegisterSummaryBySupplierItem(HttpServletResponse response, @RequestBody(required = false) Rpt152807PurchaseRegisterSummaryBySupplierItemRequest request) throws Exception {
        if (request == null) request = new Rpt152807PurchaseRegisterSummaryBySupplierItemRequest();
        printReport(response, "1528_07-PurchaseRegisterSummaryBySupplier&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1528-07-purchase-register-summary-by-supplier-item", method = RequestMethod.GET)
    public void print152807PurchaseRegisterSummaryBySupplierItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print152807PurchaseRegisterSummaryBySupplierItem(response, objectMapper.convertValue(query, Rpt152807PurchaseRegisterSummaryBySupplierItemRequest.class));
    }

    /**
     * Template: 1528_08-PurchaseSummaryBySupplier&City.rpt
     * Procedure: [ST].[USP-PurchaseInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.PurchaseInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1528-08-purchase-summary-by-supplier-city", method = RequestMethod.POST)
    public void print152808PurchaseSummaryBySupplierCity(HttpServletResponse response, @RequestBody(required = false) Rpt152808PurchaseSummaryBySupplierCityRequest request) throws Exception {
        if (request == null) request = new Rpt152808PurchaseSummaryBySupplierCityRequest();
        printReport(response, "1528_08-PurchaseSummaryBySupplier&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1528-08-purchase-summary-by-supplier-city", method = RequestMethod.GET)
    public void print152808PurchaseSummaryBySupplierCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print152808PurchaseSummaryBySupplierCity(response, objectMapper.convertValue(query, Rpt152808PurchaseSummaryBySupplierCityRequest.class));
    }

    /**
     * Template: 1528_09-PurchaseSummaryBySupplierItem&City.rpt
     * Procedure: [ST].[USP-PurchaseInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.PurchaseInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1528-09-purchase-summary-by-supplier-item-city", method = RequestMethod.POST)
    public void print152809PurchaseSummaryBySupplierItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt152809PurchaseSummaryBySupplierItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt152809PurchaseSummaryBySupplierItemCityRequest();
        printReport(response, "1528_09-PurchaseSummaryBySupplierItem&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1528-09-purchase-summary-by-supplier-item-city", method = RequestMethod.GET)
    public void print152809PurchaseSummaryBySupplierItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print152809PurchaseSummaryBySupplierItemCity(response, objectMapper.convertValue(query, Rpt152809PurchaseSummaryBySupplierItemCityRequest.class));
    }

    /**
     * Template: 1528_10-PurchaseSummaryBySupplier&PackSize.rpt
     * Procedure: [ST].[USP-PurchaseInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.PurchaseInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1528-10-purchase-summary-by-supplier-pack-size", method = RequestMethod.POST)
    public void print152810PurchaseSummaryBySupplierPackSize(HttpServletResponse response, @RequestBody(required = false) Rpt152810PurchaseSummaryBySupplierPackSizeRequest request) throws Exception {
        if (request == null) request = new Rpt152810PurchaseSummaryBySupplierPackSizeRequest();
        printReport(response, "1528_10-PurchaseSummaryBySupplier&PackSize.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1528-10-purchase-summary-by-supplier-pack-size", method = RequestMethod.GET)
    public void print152810PurchaseSummaryBySupplierPackSizeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print152810PurchaseSummaryBySupplierPackSize(response, objectMapper.convertValue(query, Rpt152810PurchaseSummaryBySupplierPackSizeRequest.class));
    }

    /**
     * Template: 1528_11-PurchaseSummaryByParentCategory.rpt
     * Procedure: [ST].[USP-PurchaseInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.PurchaseInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1528-11-purchase-summary-by-parent-category", method = RequestMethod.POST)
    public void print152811PurchaseSummaryByParentCategory(HttpServletResponse response, @RequestBody(required = false) Rpt152811PurchaseSummaryByParentCategoryRequest request) throws Exception {
        if (request == null) request = new Rpt152811PurchaseSummaryByParentCategoryRequest();
        printReport(response, "1528_11-PurchaseSummaryByParentCategory.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1528-11-purchase-summary-by-parent-category", method = RequestMethod.GET)
    public void print152811PurchaseSummaryByParentCategoryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print152811PurchaseSummaryByParentCategory(response, objectMapper.convertValue(query, Rpt152811PurchaseSummaryByParentCategoryRequest.class));
    }

    /**
     * Template: 1528_12-PurchaseSummaryByParentCategory&Item.rpt
     * Procedure: [ST].[USP-PurchaseInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.PurchaseInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1528-12-purchase-summary-by-parent-category-item", method = RequestMethod.POST)
    public void print152812PurchaseSummaryByParentCategoryItem(HttpServletResponse response, @RequestBody(required = false) Rpt152812PurchaseSummaryByParentCategoryItemRequest request) throws Exception {
        if (request == null) request = new Rpt152812PurchaseSummaryByParentCategoryItemRequest();
        printReport(response, "1528_12-PurchaseSummaryByParentCategory&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1528-12-purchase-summary-by-parent-category-item", method = RequestMethod.GET)
    public void print152812PurchaseSummaryByParentCategoryItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print152812PurchaseSummaryByParentCategoryItem(response, objectMapper.convertValue(query, Rpt152812PurchaseSummaryByParentCategoryItemRequest.class));
    }

    /**
     * Template: 1528_13-PurchaseSummaryByParentCategory&Supplier.rpt
     * Procedure: [ST].[USP-PurchaseInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.PurchaseInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1528-13-purchase-summary-by-parent-category-supplier", method = RequestMethod.POST)
    public void print152813PurchaseSummaryByParentCategorySupplier(HttpServletResponse response, @RequestBody(required = false) Rpt152813PurchaseSummaryByParentCategorySupplierRequest request) throws Exception {
        if (request == null) request = new Rpt152813PurchaseSummaryByParentCategorySupplierRequest();
        printReport(response, "1528_13-PurchaseSummaryByParentCategory&Supplier.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1528-13-purchase-summary-by-parent-category-supplier", method = RequestMethod.GET)
    public void print152813PurchaseSummaryByParentCategorySupplierGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print152813PurchaseSummaryByParentCategorySupplier(response, objectMapper.convertValue(query, Rpt152813PurchaseSummaryByParentCategorySupplierRequest.class));
    }

    /**
     * Template: 1529-SalesSummaryByCustomer&City.rpt
     * Procedure: [ST].[USP-SaleInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.SaleInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1529-sales-summary-by-customer-city", method = RequestMethod.POST)
    public void print1529SalesSummaryByCustomerCity(HttpServletResponse response, @RequestBody(required = false) Rpt1529SalesSummaryByCustomerCityRequest request) throws Exception {
        if (request == null) request = new Rpt1529SalesSummaryByCustomerCityRequest();
        printReport(response, "1529-SalesSummaryByCustomer&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1529-sales-summary-by-customer-city", method = RequestMethod.GET)
    public void print1529SalesSummaryByCustomerCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1529SalesSummaryByCustomerCity(response, objectMapper.convertValue(query, Rpt1529SalesSummaryByCustomerCityRequest.class));
    }

    /**
     * Template: 1530-SalesSummaryByItemPackSize&City.rpt
     * Procedure: [ST].[USP-SaleInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.SaleInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1530-sales-summary-by-item-pack-size-city", method = RequestMethod.POST)
    public void print1530SalesSummaryByItemPackSizeCity(HttpServletResponse response, @RequestBody(required = false) Rpt1530SalesSummaryByItemPackSizeCityRequest request) throws Exception {
        if (request == null) request = new Rpt1530SalesSummaryByItemPackSizeCityRequest();
        printReport(response, "1530-SalesSummaryByItemPackSize&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1530-sales-summary-by-item-pack-size-city", method = RequestMethod.GET)
    public void print1530SalesSummaryByItemPackSizeCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1530SalesSummaryByItemPackSizeCity(response, objectMapper.convertValue(query, Rpt1530SalesSummaryByItemPackSizeCityRequest.class));
    }

    /**
     * Template: 1531-SalesSummaryByCustomerItem&City.rpt
     * Procedure: [ST].[USP-SaleInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.SaleInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1531-sales-summary-by-customer-item-city", method = RequestMethod.POST)
    public void print1531SalesSummaryByCustomerItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt1531SalesSummaryByCustomerItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt1531SalesSummaryByCustomerItemCityRequest();
        printReport(response, "1531-SalesSummaryByCustomerItem&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1531-sales-summary-by-customer-item-city", method = RequestMethod.GET)
    public void print1531SalesSummaryByCustomerItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1531SalesSummaryByCustomerItemCity(response, objectMapper.convertValue(query, Rpt1531SalesSummaryByCustomerItemCityRequest.class));
    }

    /**
     * Template: 1532-SalesSummaryByCustomer&PackSize.rpt
     * Procedure: [ST].[USP-SaleInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.SaleInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1532-sales-summary-by-customer-pack-size", method = RequestMethod.POST)
    public void print1532SalesSummaryByCustomerPackSize(HttpServletResponse response, @RequestBody(required = false) Rpt1532SalesSummaryByCustomerPackSizeRequest request) throws Exception {
        if (request == null) request = new Rpt1532SalesSummaryByCustomerPackSizeRequest();
        printReport(response, "1532-SalesSummaryByCustomer&PackSize.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1532-sales-summary-by-customer-pack-size", method = RequestMethod.GET)
    public void print1532SalesSummaryByCustomerPackSizeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1532SalesSummaryByCustomerPackSize(response, objectMapper.convertValue(query, Rpt1532SalesSummaryByCustomerPackSizeRequest.class));
    }

    /**
     * Template: 1533-SalesSummaryByParentCategory.rpt
     * Procedure: [ST].[USP-SaleInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.SaleInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1533-sales-summary-by-parent-category", method = RequestMethod.POST)
    public void print1533SalesSummaryByParentCategory(HttpServletResponse response, @RequestBody(required = false) Rpt1533SalesSummaryByParentCategoryRequest request) throws Exception {
        if (request == null) request = new Rpt1533SalesSummaryByParentCategoryRequest();
        printReport(response, "1533-SalesSummaryByParentCategory.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1533-sales-summary-by-parent-category", method = RequestMethod.GET)
    public void print1533SalesSummaryByParentCategoryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1533SalesSummaryByParentCategory(response, objectMapper.convertValue(query, Rpt1533SalesSummaryByParentCategoryRequest.class));
    }

    /**
     * Template: 1534-SalesSummaryByParentCategory&Item.rpt
     * Procedure: [ST].[USP-SaleInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.SaleInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1534-sales-summary-by-parent-category-item", method = RequestMethod.POST)
    public void print1534SalesSummaryByParentCategoryItem(HttpServletResponse response, @RequestBody(required = false) Rpt1534SalesSummaryByParentCategoryItemRequest request) throws Exception {
        if (request == null) request = new Rpt1534SalesSummaryByParentCategoryItemRequest();
        printReport(response, "1534-SalesSummaryByParentCategory&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1534-sales-summary-by-parent-category-item", method = RequestMethod.GET)
    public void print1534SalesSummaryByParentCategoryItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1534SalesSummaryByParentCategoryItem(response, objectMapper.convertValue(query, Rpt1534SalesSummaryByParentCategoryItemRequest.class));
    }

    /**
     * Template: 1535-SalesSummaryByParentCategory&Customer.rpt
     * Procedure: [ST].[USP-SaleInvoiceRegisterWithActivities]
     * Desktop: InventoryStockEvalautionDetail.SaleInvoiceRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1535-sales-summary-by-parent-category-customer", method = RequestMethod.POST)
    public void print1535SalesSummaryByParentCategoryCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt1535SalesSummaryByParentCategoryCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt1535SalesSummaryByParentCategoryCustomerRequest();
        printReport(response, "1535-SalesSummaryByParentCategory&Customer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1535-sales-summary-by-parent-category-customer", method = RequestMethod.GET)
    public void print1535SalesSummaryByParentCategoryCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1535SalesSummaryByParentCategoryCustomer(response, objectMapper.convertValue(query, Rpt1535SalesSummaryByParentCategoryCustomerRequest.class));
    }

    /**
     * Template: 1542-ItemStockSummary.rpt
     * Procedure: [ST].[USP-StockSummaryByQtyAndWeightReport]
     * Desktop: StocksReport.StockSummaryByQtyAndWeightReport
     */
    @RequestMapping(value = "/reports/print/1542-item-stock-summary", method = RequestMethod.POST)
    public void print1542ItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1542ItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1542ItemStockSummaryRequest();
        printReport(response, "1542-ItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1542-item-stock-summary", method = RequestMethod.GET)
    public void print1542ItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1542ItemStockSummary(response, objectMapper.convertValue(query, Rpt1542ItemStockSummaryRequest.class));
    }

    /**
     * Template: 1543-ItemandWarehouseStockSummary.rpt
     * Procedure: [ST].[USP-StockSummaryByQtyAndWeightReport]
     * Desktop: StocksReport.StockSummaryByQtyAndWeightReport
     */
    @RequestMapping(value = "/reports/print/1543-itemand-warehouse-stock-summary", method = RequestMethod.POST)
    public void print1543ItemandWarehouseStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1543ItemandWarehouseStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1543ItemandWarehouseStockSummaryRequest();
        printReport(response, "1543-ItemandWarehouseStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1543-itemand-warehouse-stock-summary", method = RequestMethod.GET)
    public void print1543ItemandWarehouseStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1543ItemandWarehouseStockSummary(response, objectMapper.convertValue(query, Rpt1543ItemandWarehouseStockSummaryRequest.class));
    }

    /**
     * Template: 1544-WarehouseAndItemStockSummary.rpt
     * Procedure: [ST].[USP-StockSummaryByQtyAndWeightReport]
     * Desktop: StocksReport.StockSummaryByQtyAndWeightReport
     */
    @RequestMapping(value = "/reports/print/1544-warehouse-and-item-stock-summary", method = RequestMethod.POST)
    public void print1544WarehouseAndItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1544WarehouseAndItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1544WarehouseAndItemStockSummaryRequest();
        printReport(response, "1544-WarehouseAndItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1544-warehouse-and-item-stock-summary", method = RequestMethod.GET)
    public void print1544WarehouseAndItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1544WarehouseAndItemStockSummary(response, objectMapper.convertValue(query, Rpt1544WarehouseAndItemStockSummaryRequest.class));
    }

    /**
     * Template: 1545-WarehouseandJoblotandItemStockSummary.rpt
     * Procedure: [ST].[USP-StockSummaryByQtyAndWeightReport]
     * Desktop: StocksReport.StockSummaryByQtyAndWeightReport
     */
    @RequestMapping(value = "/reports/print/1545-warehouseand-joblotand-item-stock-summary", method = RequestMethod.POST)
    public void print1545WarehouseandJoblotandItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1545WarehouseandJoblotandItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1545WarehouseandJoblotandItemStockSummaryRequest();
        printReport(response, "1545-WarehouseandJoblotandItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1545-warehouseand-joblotand-item-stock-summary", method = RequestMethod.GET)
    public void print1545WarehouseandJoblotandItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1545WarehouseandJoblotandItemStockSummary(response, objectMapper.convertValue(query, Rpt1545WarehouseandJoblotandItemStockSummaryRequest.class));
    }

    /**
     * Template: 1546-ItemandPackSizeStockSummary.rpt
     * Procedure: [ST].[USP-StockSummaryByQtyAndWeightReport]
     * Desktop: StocksReport.StockSummaryByQtyAndWeightReport
     */
    @RequestMapping(value = "/reports/print/1546-itemand-pack-size-stock-summary", method = RequestMethod.POST)
    public void print1546ItemandPackSizeStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1546ItemandPackSizeStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1546ItemandPackSizeStockSummaryRequest();
        printReport(response, "1546-ItemandPackSizeStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1546-itemand-pack-size-stock-summary", method = RequestMethod.GET)
    public void print1546ItemandPackSizeStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1546ItemandPackSizeStockSummary(response, objectMapper.convertValue(query, Rpt1546ItemandPackSizeStockSummaryRequest.class));
    }

    /**
     * Template: 1547-JobLotandItemStockSummary.rpt
     * Procedure: [ST].[USP-StockSummaryByQtyAndWeightReport]
     * Desktop: StocksReport.StockSummaryByQtyAndWeightReport
     */
    @RequestMapping(value = "/reports/print/1547-job-lotand-item-stock-summary", method = RequestMethod.POST)
    public void print1547JobLotandItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1547JobLotandItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1547JobLotandItemStockSummaryRequest();
        printReport(response, "1547-JobLotandItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1547-job-lotand-item-stock-summary", method = RequestMethod.GET)
    public void print1547JobLotandItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1547JobLotandItemStockSummary(response, objectMapper.convertValue(query, Rpt1547JobLotandItemStockSummaryRequest.class));
    }

    /**
     * Template: 1548-ItemandPackingTypeStockSummary.rpt
     * Procedure: [ST].[USP-StockSummaryByQtyAndWeightReport]
     * Desktop: StocksReport.StockSummaryByQtyAndWeightReport
     */
    @RequestMapping(value = "/reports/print/1548-itemand-packing-type-stock-summary", method = RequestMethod.POST)
    public void print1548ItemandPackingTypeStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1548ItemandPackingTypeStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1548ItemandPackingTypeStockSummaryRequest();
        printReport(response, "1548-ItemandPackingTypeStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1548-itemand-packing-type-stock-summary", method = RequestMethod.GET)
    public void print1548ItemandPackingTypeStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1548ItemandPackingTypeStockSummary(response, objectMapper.convertValue(query, Rpt1548ItemandPackingTypeStockSummaryRequest.class));
    }

    /**
     * Template: 1549-ItemandPackSizeandPackingTypeStockSummary.rpt
     * Procedure: [ST].[USP-StockSummaryByQtyAndWeightReport]
     * Desktop: StocksReport.StockSummaryByQtyAndWeightReport
     */
    @RequestMapping(value = "/reports/print/1549-itemand-pack-sizeand-packing-type-stock-summary", method = RequestMethod.POST)
    public void print1549ItemandPackSizeandPackingTypeStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1549ItemandPackSizeandPackingTypeStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1549ItemandPackSizeandPackingTypeStockSummaryRequest();
        printReport(response, "1549-ItemandPackSizeandPackingTypeStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1549-itemand-pack-sizeand-packing-type-stock-summary", method = RequestMethod.GET)
    public void print1549ItemandPackSizeandPackingTypeStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1549ItemandPackSizeandPackingTypeStockSummary(response, objectMapper.convertValue(query, Rpt1549ItemandPackSizeandPackingTypeStockSummaryRequest.class));
    }

    /**
     * Template: 1550-ItemStockSummary.rpt
     * Procedure: [ST].[USP_StockSummaryWithValuesReport]
     * Desktop: StocksReport.StockSummaryWithValuesReport
     */
    @RequestMapping(value = "/reports/print/1550-item-stock-summary", method = RequestMethod.POST)
    public void print1550ItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1550ItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1550ItemStockSummaryRequest();
        printReport(response, "1550-ItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1550-item-stock-summary", method = RequestMethod.GET)
    public void print1550ItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1550ItemStockSummary(response, objectMapper.convertValue(query, Rpt1550ItemStockSummaryRequest.class));
    }

    /**
     * Template: 1551-ItemandWarehouseStockSummary.rpt
     * Procedure: [ST].[USP_StockSummaryWithValuesReport]
     * Desktop: StocksReport.StockSummaryWithValuesReport
     */
    @RequestMapping(value = "/reports/print/1551-itemand-warehouse-stock-summary", method = RequestMethod.POST)
    public void print1551ItemandWarehouseStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1551ItemandWarehouseStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1551ItemandWarehouseStockSummaryRequest();
        printReport(response, "1551-ItemandWarehouseStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1551-itemand-warehouse-stock-summary", method = RequestMethod.GET)
    public void print1551ItemandWarehouseStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1551ItemandWarehouseStockSummary(response, objectMapper.convertValue(query, Rpt1551ItemandWarehouseStockSummaryRequest.class));
    }

    /**
     * Template: 1552-WarehouseAndItemStockSummary.rpt
     * Procedure: [ST].[USP_StockSummaryWithValuesReport]
     * Desktop: StocksReport.StockSummaryWithValuesReport
     */
    @RequestMapping(value = "/reports/print/1552-warehouse-and-item-stock-summary", method = RequestMethod.POST)
    public void print1552WarehouseAndItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1552WarehouseAndItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1552WarehouseAndItemStockSummaryRequest();
        printReport(response, "1552-WarehouseAndItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1552-warehouse-and-item-stock-summary", method = RequestMethod.GET)
    public void print1552WarehouseAndItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1552WarehouseAndItemStockSummary(response, objectMapper.convertValue(query, Rpt1552WarehouseAndItemStockSummaryRequest.class));
    }

    /**
     * Template: 1553-JobLotandItemStockSummary.rpt
     * Procedure: [ST].[USP_StockSummaryWithValuesReport]
     * Desktop: StocksReport.StockSummaryWithValuesReport
     */
    @RequestMapping(value = "/reports/print/1553-job-lotand-item-stock-summary", method = RequestMethod.POST)
    public void print1553JobLotandItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1553JobLotandItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1553JobLotandItemStockSummaryRequest();
        printReport(response, "1553-JobLotandItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1553-job-lotand-item-stock-summary", method = RequestMethod.GET)
    public void print1553JobLotandItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1553JobLotandItemStockSummary(response, objectMapper.convertValue(query, Rpt1553JobLotandItemStockSummaryRequest.class));
    }

    /**
     * Template: 1554-WarehouseandJoblotandItemStockSummary.rpt
     * Procedure: [ST].[USP_StockSummaryWithValuesReport]
     * Desktop: StocksReport.StockSummaryWithValuesReport
     */
    @RequestMapping(value = "/reports/print/1554-warehouseand-joblotand-item-stock-summary", method = RequestMethod.POST)
    public void print1554WarehouseandJoblotandItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1554WarehouseandJoblotandItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1554WarehouseandJoblotandItemStockSummaryRequest();
        printReport(response, "1554-WarehouseandJoblotandItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1554-warehouseand-joblotand-item-stock-summary", method = RequestMethod.GET)
    public void print1554WarehouseandJoblotandItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1554WarehouseandJoblotandItemStockSummary(response, objectMapper.convertValue(query, Rpt1554WarehouseandJoblotandItemStockSummaryRequest.class));
    }

    /**
     * Template: 1555-ItemandPackSizeStockSummary.rpt
     * Procedure: [ST].[USP_StockSummaryWithValuesReport]
     * Desktop: StocksReport.StockSummaryWithValuesReport
     */
    @RequestMapping(value = "/reports/print/1555-itemand-pack-size-stock-summary", method = RequestMethod.POST)
    public void print1555ItemandPackSizeStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1555ItemandPackSizeStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1555ItemandPackSizeStockSummaryRequest();
        printReport(response, "1555-ItemandPackSizeStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1555-itemand-pack-size-stock-summary", method = RequestMethod.GET)
    public void print1555ItemandPackSizeStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1555ItemandPackSizeStockSummary(response, objectMapper.convertValue(query, Rpt1555ItemandPackSizeStockSummaryRequest.class));
    }

    /**
     * Template: 1556-ItemandPackingTypeStockSummary.rpt
     * Procedure: [ST].[USP_StockSummaryWithValuesReport]
     * Desktop: StocksReport.StockSummaryWithValuesReport
     */
    @RequestMapping(value = "/reports/print/1556-itemand-packing-type-stock-summary", method = RequestMethod.POST)
    public void print1556ItemandPackingTypeStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1556ItemandPackingTypeStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1556ItemandPackingTypeStockSummaryRequest();
        printReport(response, "1556-ItemandPackingTypeStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1556-itemand-packing-type-stock-summary", method = RequestMethod.GET)
    public void print1556ItemandPackingTypeStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1556ItemandPackingTypeStockSummary(response, objectMapper.convertValue(query, Rpt1556ItemandPackingTypeStockSummaryRequest.class));
    }

    /**
     * Template: 1557-ItemandPackSizeandPackingTypeStockSummary.rpt
     * Procedure: [ST].[USP_StockSummaryWithValuesReport]
     * Desktop: StocksReport.StockSummaryWithValuesReport
     */
    @RequestMapping(value = "/reports/print/1557-itemand-pack-sizeand-packing-type-stock-summary", method = RequestMethod.POST)
    public void print1557ItemandPackSizeandPackingTypeStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1557ItemandPackSizeandPackingTypeStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1557ItemandPackSizeandPackingTypeStockSummaryRequest();
        printReport(response, "1557-ItemandPackSizeandPackingTypeStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1557-itemand-pack-sizeand-packing-type-stock-summary", method = RequestMethod.GET)
    public void print1557ItemandPackSizeandPackingTypeStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1557ItemandPackSizeandPackingTypeStockSummary(response, objectMapper.convertValue(query, Rpt1557ItemandPackSizeandPackingTypeStockSummaryRequest.class));
    }

    /**
     * Template: 1558-InvStockRptInventoryTransactionsA.rpt
     * Procedure: Sp_InventoryTransactions_GenerateTransactionsLedgerStocks
     * Desktop: InventoryStockEvalautionDetail.InventoryTransactions_GenerateTransactionsLedgerStocks
     */
    @RequestMapping(value = "/reports/print/1558-inv-stock-inventory-transactions-a", method = RequestMethod.POST)
    public void print1558InvStockInventoryTransactionsA(HttpServletResponse response, @RequestBody(required = false) Rpt1558InvStockInventoryTransactionsARequest request) throws Exception {
        if (request == null) request = new Rpt1558InvStockInventoryTransactionsARequest();
        printReport(response, "1558-InvStockRptInventoryTransactionsA.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1558-inv-stock-inventory-transactions-a", method = RequestMethod.GET)
    public void print1558InvStockInventoryTransactionsAGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1558InvStockInventoryTransactionsA(response, objectMapper.convertValue(query, Rpt1558InvStockInventoryTransactionsARequest.class));
    }

    /**
     * Template: 1559-InvStockRptInventoryTransactionsStocks.rpt
     * Procedure: [ST].[USP-InventoryTransactionsReport]
     * Desktop: InventoryStockEvalautionDetail.SteelTransactionStockGenrate
     */
    @RequestMapping(value = "/reports/print/1559-inv-stock-inventory-transactions-stocks", method = RequestMethod.POST)
    public void print1559InvStockInventoryTransactionsStocks(HttpServletResponse response, @RequestBody(required = false) Rpt1559InvStockInventoryTransactionsStocksRequest request) throws Exception {
        if (request == null) request = new Rpt1559InvStockInventoryTransactionsStocksRequest();
        printReport(response, "1559-InvStockRptInventoryTransactionsStocks.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1559-inv-stock-inventory-transactions-stocks", method = RequestMethod.GET)
    public void print1559InvStockInventoryTransactionsStocksGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1559InvStockInventoryTransactionsStocks(response, objectMapper.convertValue(query, Rpt1559InvStockInventoryTransactionsStocksRequest.class));
    }

    /**
     * Template: 1563-ProductionJobOrderSlip.rpt
     * Procedure: [ST].[USP_ProductionJobOrder_REPORT]
     * Desktop: InvProductionJobOrder.JobOrderPrintNew
     */
    @RequestMapping(value = "/reports/print/1563-production-job-order-slip", method = RequestMethod.POST)
    public void print1563ProductionJobOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1563ProductionJobOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1563ProductionJobOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1563-ProductionJobOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1563-production-job-order-slip", method = RequestMethod.GET)
    public void print1563ProductionJobOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1563ProductionJobOrderSlip(response, objectMapper.convertValue(query, Rpt1563ProductionJobOrderSlipRequest.class));
    }

    /**
     * Template: 1853-DeliveryOrderRegister.rpt
     * Procedure: [ST].[USp_DeliveryOrderRegister]
     * Desktop: InvDeliveryOrder.DeliveryOrderRegister
     */
    @RequestMapping(value = "/reports/print/1853-delivery-order-register", method = RequestMethod.POST)
    public void print1853DeliveryOrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1853DeliveryOrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1853DeliveryOrderRegisterRequest();
        printReport(response, "1853-DeliveryOrderRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1853-delivery-order-register", method = RequestMethod.GET)
    public void print1853DeliveryOrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1853DeliveryOrderRegister(response, objectMapper.convertValue(query, Rpt1853DeliveryOrderRegisterRequest.class));
    }

    /**
     * Template: 1853A-DeliveryOrderRegister.rpt
     * Procedure: [ST].[USp_DeliveryOrderRegister]
     * Desktop: InvDeliveryOrder.DeliveryOrderRegister
     */
    @RequestMapping(value = "/reports/print/1853a-delivery-order-register", method = RequestMethod.POST)
    public void print1853ADeliveryOrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1853ADeliveryOrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1853ADeliveryOrderRegisterRequest();
        printReport(response, "1853A-DeliveryOrderRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1853a-delivery-order-register", method = RequestMethod.GET)
    public void print1853ADeliveryOrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1853ADeliveryOrderRegister(response, objectMapper.convertValue(query, Rpt1853ADeliveryOrderRegisterRequest.class));
    }

    /**
     * Template: 630-PreCostProductionJobOrderSlip.rpt
     * Procedure: [ST].[USP_ProductionJobOrder_REPORT]
     * Desktop: InvProductionJobOrder.JobOrderPrintNew
     */
    @RequestMapping(value = "/reports/print/630-pre-cost-production-job-order-slip", method = RequestMethod.POST)
    public void print630PreCostProductionJobOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt630PreCostProductionJobOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt630PreCostProductionJobOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "630-PreCostProductionJobOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/630-pre-cost-production-job-order-slip", method = RequestMethod.GET)
    public void print630PreCostProductionJobOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print630PreCostProductionJobOrderSlip(response, objectMapper.convertValue(query, Rpt630PreCostProductionJobOrderSlipRequest.class));
    }

    /**
     * Template: 7861-PendingInwardGatePassRegister.rpt
     * Procedure: Sp_InvGrn_GetPendingGrn
     * Desktop: InvGrn.GetPendingGrn
     */
    @RequestMapping(value = "/reports/print/7861-pending-inward-gate-pass-register", method = RequestMethod.POST)
    public void print7861PendingInwardGatePassRegister(HttpServletResponse response, @RequestBody(required = false) Rpt7861PendingInwardGatePassRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt7861PendingInwardGatePassRegisterRequest();
        printReport(response, "7861-PendingInwardGatePassRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/7861-pending-inward-gate-pass-register", method = RequestMethod.GET)
    public void print7861PendingInwardGatePassRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print7861PendingInwardGatePassRegister(response, objectMapper.convertValue(query, Rpt7861PendingInwardGatePassRegisterRequest.class));
    }

    /**
     * Template: 7863-PendingGrnRegister.rpt
     * Procedure: Sp_InvGrn_GetPendingGrn
     * Desktop: InvGrn.GetPendingGrn
     */
    @RequestMapping(value = "/reports/print/7863-pending-grn-register", method = RequestMethod.POST)
    public void print7863PendingGrnRegister(HttpServletResponse response, @RequestBody(required = false) Rpt7863PendingGrnRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt7863PendingGrnRegisterRequest();
        printReport(response, "7863-PendingGrnRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/7863-pending-grn-register", method = RequestMethod.GET)
    public void print7863PendingGrnRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print7863PendingGrnRegister(response, objectMapper.convertValue(query, Rpt7863PendingGrnRegisterRequest.class));
    }

    /**
     * Template: ProductionJobOrderInputSubReport.rpt
     * Procedure: [ST].[USP_ProductionJobOrderInput_REPORT]
     * Desktop: InvProductionJobOrder.JobOrderInPutSubReport
     */
    @RequestMapping(value = "/reports/print/production-job-order-input-sub-report", method = RequestMethod.POST)
    public void printProductionJobOrderInputSubReport(HttpServletResponse response, @RequestBody(required = false) RptProductionJobOrderInputSubReportRequest request) throws Exception {
        if (request == null) request = new RptProductionJobOrderInputSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "ProductionJobOrderInputSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/production-job-order-input-sub-report", method = RequestMethod.GET)
    public void printProductionJobOrderInputSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printProductionJobOrderInputSubReport(response, objectMapper.convertValue(query, RptProductionJobOrderInputSubReportRequest.class));
    }

    /**
     * Template: ProductionJobOrderOutputSubReport.rpt
     * Procedure: [ST].[USP_ProductionJobOrderOutput_REPORT]
     * Desktop: InvProductionJobOrder.JobOrderOutPutSubReport
     */
    @RequestMapping(value = "/reports/print/production-job-order-output-sub-report", method = RequestMethod.POST)
    public void printProductionJobOrderOutputSubReport(HttpServletResponse response, @RequestBody(required = false) RptProductionJobOrderOutputSubReportRequest request) throws Exception {
        if (request == null) request = new RptProductionJobOrderOutputSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "ProductionJobOrderOutputSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/production-job-order-output-sub-report", method = RequestMethod.GET)
    public void printProductionJobOrderOutputSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printProductionJobOrderOutputSubReport(response, objectMapper.convertValue(query, RptProductionJobOrderOutputSubReportRequest.class));
    }

    /**
     * Template: ProductionJobOrderOverHeadsSubReport.rpt
     * Procedure: [ST].[USP_ProductionJobOrderOverHeads_REPORT]
     * Desktop: InvProductionJobOrder.JobOrderOverHeadSubReport
     */
    @RequestMapping(value = "/reports/print/production-job-order-over-heads-sub-report", method = RequestMethod.POST)
    public void printProductionJobOrderOverHeadsSubReport(HttpServletResponse response, @RequestBody(required = false) RptProductionJobOrderOverHeadsSubReportRequest request) throws Exception {
        if (request == null) request = new RptProductionJobOrderOverHeadsSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "ProductionJobOrderOverHeadsSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/production-job-order-over-heads-sub-report", method = RequestMethod.GET)
    public void printProductionJobOrderOverHeadsSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printProductionJobOrderOverHeadsSubReport(response, objectMapper.convertValue(query, RptProductionJobOrderOverHeadsSubReportRequest.class));
    }

    /**
     * Template: ProductionJobOrderPackingMaterialsSubReport.rpt
     * Procedure: USP_InvProductionJobOrderPackingMaterial_Report
     * Desktop: InvProductionJobOrder.JobOrderPackingMaterialSubReport
     */
    @RequestMapping(value = "/reports/print/production-job-order-packing-materials-sub-report", method = RequestMethod.POST)
    public void printProductionJobOrderPackingMaterialsSubReport(HttpServletResponse response, @RequestBody(required = false) RptProductionJobOrderPackingMaterialsSubReportRequest request) throws Exception {
        if (request == null) request = new RptProductionJobOrderPackingMaterialsSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "ProductionJobOrderPackingMaterialsSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/production-job-order-packing-materials-sub-report", method = RequestMethod.GET)
    public void printProductionJobOrderPackingMaterialsSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printProductionJobOrderPackingMaterialsSubReport(response, objectMapper.convertValue(query, RptProductionJobOrderPackingMaterialsSubReportRequest.class));
    }

    /**
     * Template: PurchaseInvoiceItemOthersAddLessSubReport.rpt
     * Procedure: [ST].[USP_PurchaseInvoice_ItemOthersAddLess_SubReport]
     * Desktop: InvPurchaseInvoice.InvPurchaseInvoiceSlipReport1508SupReprt
     */
    @RequestMapping(value = "/reports/print/purchase-invoice-item-others-add-less-sub-report", method = RequestMethod.POST)
    public void printPurchaseInvoiceItemOthersAddLessSubReport(HttpServletResponse response, @RequestBody(required = false) RptPurchaseInvoiceItemOthersAddLessSubReportRequest request) throws Exception {
        if (request == null) request = new RptPurchaseInvoiceItemOthersAddLessSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "PurchaseInvoiceItemOthersAddLessSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/purchase-invoice-item-others-add-less-sub-report", method = RequestMethod.GET)
    public void printPurchaseInvoiceItemOthersAddLessSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printPurchaseInvoiceItemOthersAddLessSubReport(response, objectMapper.convertValue(query, RptPurchaseInvoiceItemOthersAddLessSubReportRequest.class));
    }

    /**
     * Template: PurchaseInvoicePartyBillOthersAddLessSubReport.rpt
     * Procedure: [ST].[USP_PurchaseInvoice_PartyBillOthersAddLess_SubReport]
     * Desktop: InvPurchaseInvoice.InvPurchaseInvoiceSlipReport1508ASupReprt
     */
    @RequestMapping(value = "/reports/print/purchase-invoice-party-bill-others-add-less-sub-report", method = RequestMethod.POST)
    public void printPurchaseInvoicePartyBillOthersAddLessSubReport(HttpServletResponse response, @RequestBody(required = false) RptPurchaseInvoicePartyBillOthersAddLessSubReportRequest request) throws Exception {
        if (request == null) request = new RptPurchaseInvoicePartyBillOthersAddLessSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "PurchaseInvoicePartyBillOthersAddLessSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/purchase-invoice-party-bill-others-add-less-sub-report", method = RequestMethod.GET)
    public void printPurchaseInvoicePartyBillOthersAddLessSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printPurchaseInvoicePartyBillOthersAddLessSubReport(response, objectMapper.convertValue(query, RptPurchaseInvoicePartyBillOthersAddLessSubReportRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
