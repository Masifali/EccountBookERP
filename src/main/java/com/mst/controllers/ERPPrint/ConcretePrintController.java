package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.ConcretePrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** Concrete print actions. Generated from the verified seeder contracts. */
@Controller
public class ConcretePrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 1104_PurchaseInvoiceDirectItemSlip.rpt
     * Procedure: [pcc].[USP_InvPurchaseInvoice_DirectSlip]
     * Desktop: InvPurchaseInvoice.PurchaseInvoiceDirectSlip
     */
    @RequestMapping(value = "/reports/print/1104-purchase-invoice-direct-item-slip", method = RequestMethod.POST)
    public void print1104PurchaseInvoiceDirectItemSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1104PurchaseInvoiceDirectItemSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1104PurchaseInvoiceDirectItemSlipRequest();
        printReport(response, "1104_PurchaseInvoiceDirectItemSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1104-purchase-invoice-direct-item-slip", method = RequestMethod.GET)
    public void print1104PurchaseInvoiceDirectItemSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1104PurchaseInvoiceDirectItemSlip(response, objectMapper.convertValue(query, Rpt1104PurchaseInvoiceDirectItemSlipRequest.class));
    }

    /**
     * Template: 1104A_PurchaseInvoiceDirectPartySlip.rpt
     * Procedure: [pcc].[USP_InvPurchaseInvoice_DirectSlip]
     * Desktop: InvPurchaseInvoice.PurchaseInvoiceDirectSlip
     */
    @RequestMapping(value = "/reports/print/1104a-purchase-invoice-direct-party-slip", method = RequestMethod.POST)
    public void print1104APurchaseInvoiceDirectPartySlip(HttpServletResponse response, @RequestBody(required = false) Rpt1104APurchaseInvoiceDirectPartySlipRequest request) throws Exception {
        if (request == null) request = new Rpt1104APurchaseInvoiceDirectPartySlipRequest();
        printReport(response, "1104A_PurchaseInvoiceDirectPartySlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1104a-purchase-invoice-direct-party-slip", method = RequestMethod.GET)
    public void print1104APurchaseInvoiceDirectPartySlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1104APurchaseInvoiceDirectPartySlip(response, objectMapper.convertValue(query, Rpt1104APurchaseInvoiceDirectPartySlipRequest.class));
    }

    /**
     * Template: 1120_Recipe_Slip.rpt
     * Procedure: [pcc].[USP_BillOfMaterial_Register]
     * Desktop: BomHeader.BillOfMaterial_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1120-recipe-slip", method = RequestMethod.POST)
    public void print1120RecipeSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1120RecipeSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1120RecipeSlipRequest();
        printReport(response, "1120_Recipe_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1120-recipe-slip", method = RequestMethod.GET)
    public void print1120RecipeSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1120RecipeSlip(response, objectMapper.convertValue(query, Rpt1120RecipeSlipRequest.class));
    }

    /**
     * Template: 1120_Recipe_SubReport.rpt
     * Procedure: [pcc].[USP_BillOfMaterialAllDetails_SubReport]
     * Desktop: BomHeader.BillOfMaterialAllDetails_SubReport
     */
    @RequestMapping(value = "/reports/print/1120-recipe-sub-report", method = RequestMethod.POST)
    public void print1120RecipeSubReport(HttpServletResponse response, @RequestBody(required = false) Rpt1120RecipeSubReportRequest request) throws Exception {
        if (request == null) request = new Rpt1120RecipeSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1120_Recipe_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1120-recipe-sub-report", method = RequestMethod.GET)
    public void print1120RecipeSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1120RecipeSubReport(response, objectMapper.convertValue(query, Rpt1120RecipeSubReportRequest.class));
    }

    /**
     * Template: 1121_Production_Slip.rpt
     * Procedure: [pcc].[USP_Production_SlipAndRegister]
     * Desktop: ProductionHeader.Production_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1121-production-slip", method = RequestMethod.POST)
    public void print1121ProductionSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1121ProductionSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1121ProductionSlipRequest();
        printReport(response, "1121_Production_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1121-production-slip", method = RequestMethod.GET)
    public void print1121ProductionSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1121ProductionSlip(response, objectMapper.convertValue(query, Rpt1121ProductionSlipRequest.class));
    }

    /**
     * Template: 1122_Production_Slip.rpt
     * Procedure: [pcc].[USP_Production_SlipAndRegister]
     * Desktop: ProductionHeader.Production_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1122-production-slip", method = RequestMethod.POST)
    public void print1122ProductionSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1122ProductionSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1122ProductionSlipRequest();
        printReport(response, "1122_Production_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1122-production-slip", method = RequestMethod.GET)
    public void print1122ProductionSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1122ProductionSlip(response, objectMapper.convertValue(query, Rpt1122ProductionSlipRequest.class));
    }

    /**
     * Template: 126_01-SalesDetailRegisterStoreAndPm.rpt
     * Procedure: pcc.USP_GetDataForDropDownFromGdn
     * Desktop: InvGdn.GetDataForDropDownFromGdn
     */
    @RequestMapping(value = "/reports/print/126-01-sales-detail-register-store-and-pm", method = RequestMethod.POST)
    public void print12601SalesDetailRegisterStoreAndPm(HttpServletResponse response, @RequestBody(required = false) Rpt12601SalesDetailRegisterStoreAndPmRequest request) throws Exception {
        if (request == null) request = new Rpt12601SalesDetailRegisterStoreAndPmRequest();
        printReport(response, "126_01-SalesDetailRegisterStoreAndPm.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/126-01-sales-detail-register-store-and-pm", method = RequestMethod.GET)
    public void print12601SalesDetailRegisterStoreAndPmGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print12601SalesDetailRegisterStoreAndPm(response, objectMapper.convertValue(query, Rpt12601SalesDetailRegisterStoreAndPmRequest.class));
    }

    /**
     * Template: 126_02-SalesCustomerAndItemWiseRegisterStoreAndPm.rpt
     * Procedure: pcc.USP_GetDataForDropDownFromGdn
     * Desktop: InvGdn.GetDataForDropDownFromGdn
     */
    @RequestMapping(value = "/reports/print/126-02-sales-customer-and-item-wise-register-store-and-pm", method = RequestMethod.POST)
    public void print12602SalesCustomerAndItemWiseRegisterStoreAndPm(HttpServletResponse response, @RequestBody(required = false) Rpt12602SalesCustomerAndItemWiseRegisterStoreAndPmRequest request) throws Exception {
        if (request == null) request = new Rpt12602SalesCustomerAndItemWiseRegisterStoreAndPmRequest();
        printReport(response, "126_02-SalesCustomerAndItemWiseRegisterStoreAndPm.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/126-02-sales-customer-and-item-wise-register-store-and-pm", method = RequestMethod.GET)
    public void print12602SalesCustomerAndItemWiseRegisterStoreAndPmGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print12602SalesCustomerAndItemWiseRegisterStoreAndPm(response, objectMapper.convertValue(query, Rpt12602SalesCustomerAndItemWiseRegisterStoreAndPmRequest.class));
    }

    /**
     * Template: 126_03-SalesItemAndConditionWiseRegisterStoreAndPm.rpt
     * Procedure: pcc.USP_GetDataForDropDownFromGdn
     * Desktop: InvGdn.GetDataForDropDownFromGdn
     */
    @RequestMapping(value = "/reports/print/126-03-sales-item-and-condition-wise-register-store-and-pm", method = RequestMethod.POST)
    public void print12603SalesItemAndConditionWiseRegisterStoreAndPm(HttpServletResponse response, @RequestBody(required = false) Rpt12603SalesItemAndConditionWiseRegisterStoreAndPmRequest request) throws Exception {
        if (request == null) request = new Rpt12603SalesItemAndConditionWiseRegisterStoreAndPmRequest();
        printReport(response, "126_03-SalesItemAndConditionWiseRegisterStoreAndPm.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/126-03-sales-item-and-condition-wise-register-store-and-pm", method = RequestMethod.GET)
    public void print12603SalesItemAndConditionWiseRegisterStoreAndPmGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print12603SalesItemAndConditionWiseRegisterStoreAndPm(response, objectMapper.convertValue(query, Rpt12603SalesItemAndConditionWiseRegisterStoreAndPmRequest.class));
    }

    /**
     * Template: 1605_01_OrderRegister.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1605-01-order-register", method = RequestMethod.POST)
    public void print160501OrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt160501OrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt160501OrderRegisterRequest();
        printReport(response, "1605_01_OrderRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1605-01-order-register", method = RequestMethod.GET)
    public void print160501OrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print160501OrderRegister(response, objectMapper.convertValue(query, Rpt160501OrderRegisterRequest.class));
    }

    /**
     * Template: 1605_02_OrderSummaryByItem&PackSize.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1605-02-order-summary-by-item-pack-size", method = RequestMethod.POST)
    public void print160502OrderSummaryByItemPackSize(HttpServletResponse response, @RequestBody(required = false) Rpt160502OrderSummaryByItemPackSizeRequest request) throws Exception {
        if (request == null) request = new Rpt160502OrderSummaryByItemPackSizeRequest();
        printReport(response, "1605_02_OrderSummaryByItem&PackSize.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1605-02-order-summary-by-item-pack-size", method = RequestMethod.GET)
    public void print160502OrderSummaryByItemPackSizeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print160502OrderSummaryByItemPackSize(response, objectMapper.convertValue(query, Rpt160502OrderSummaryByItemPackSizeRequest.class));
    }

    /**
     * Template: 1605_03_OrderSummaryByItem,PackSize&City.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1605-03-order-summary-by-item-pack-size-city", method = RequestMethod.POST)
    public void print160503OrderSummaryByItemPackSizeCity(HttpServletResponse response, @RequestBody(required = false) Rpt160503OrderSummaryByItemPackSizeCityRequest request) throws Exception {
        if (request == null) request = new Rpt160503OrderSummaryByItemPackSizeCityRequest();
        printReport(response, "1605_03_OrderSummaryByItem,PackSize&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1605-03-order-summary-by-item-pack-size-city", method = RequestMethod.GET)
    public void print160503OrderSummaryByItemPackSizeCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print160503OrderSummaryByItemPackSizeCity(response, objectMapper.convertValue(query, Rpt160503OrderSummaryByItemPackSizeCityRequest.class));
    }

    /**
     * Template: 1605_04_OrderSummaryByCustomer&PackSize.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1605-04-order-summary-by-customer-pack-size", method = RequestMethod.POST)
    public void print160504OrderSummaryByCustomerPackSize(HttpServletResponse response, @RequestBody(required = false) Rpt160504OrderSummaryByCustomerPackSizeRequest request) throws Exception {
        if (request == null) request = new Rpt160504OrderSummaryByCustomerPackSizeRequest();
        printReport(response, "1605_04_OrderSummaryByCustomer&PackSize.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1605-04-order-summary-by-customer-pack-size", method = RequestMethod.GET)
    public void print160504OrderSummaryByCustomerPackSizeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print160504OrderSummaryByCustomerPackSize(response, objectMapper.convertValue(query, Rpt160504OrderSummaryByCustomerPackSizeRequest.class));
    }

    /**
     * Template: 1605_05_OrderSummaryByItem.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1605-05-order-summary-by-item", method = RequestMethod.POST)
    public void print160505OrderSummaryByItem(HttpServletResponse response, @RequestBody(required = false) Rpt160505OrderSummaryByItemRequest request) throws Exception {
        if (request == null) request = new Rpt160505OrderSummaryByItemRequest();
        printReport(response, "1605_05_OrderSummaryByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1605-05-order-summary-by-item", method = RequestMethod.GET)
    public void print160505OrderSummaryByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print160505OrderSummaryByItem(response, objectMapper.convertValue(query, Rpt160505OrderSummaryByItemRequest.class));
    }

    /**
     * Template: 1605_06_OrderSummaryByItem&City.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1605-06-order-summary-by-item-city", method = RequestMethod.POST)
    public void print160506OrderSummaryByItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt160506OrderSummaryByItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt160506OrderSummaryByItemCityRequest();
        printReport(response, "1605_06_OrderSummaryByItem&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1605-06-order-summary-by-item-city", method = RequestMethod.GET)
    public void print160506OrderSummaryByItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print160506OrderSummaryByItemCity(response, objectMapper.convertValue(query, Rpt160506OrderSummaryByItemCityRequest.class));
    }

    /**
     * Template: 1605_07_OrderSummaryByCustomer.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1605-07-order-summary-by-customer", method = RequestMethod.POST)
    public void print160507OrderSummaryByCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt160507OrderSummaryByCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt160507OrderSummaryByCustomerRequest();
        printReport(response, "1605_07_OrderSummaryByCustomer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1605-07-order-summary-by-customer", method = RequestMethod.GET)
    public void print160507OrderSummaryByCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print160507OrderSummaryByCustomer(response, objectMapper.convertValue(query, Rpt160507OrderSummaryByCustomerRequest.class));
    }

    /**
     * Template: 1605_08_OrderSummaryByCustomer&City.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1605-08-order-summary-by-customer-city", method = RequestMethod.POST)
    public void print160508OrderSummaryByCustomerCity(HttpServletResponse response, @RequestBody(required = false) Rpt160508OrderSummaryByCustomerCityRequest request) throws Exception {
        if (request == null) request = new Rpt160508OrderSummaryByCustomerCityRequest();
        printReport(response, "1605_08_OrderSummaryByCustomer&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1605-08-order-summary-by-customer-city", method = RequestMethod.GET)
    public void print160508OrderSummaryByCustomerCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print160508OrderSummaryByCustomerCity(response, objectMapper.convertValue(query, Rpt160508OrderSummaryByCustomerCityRequest.class));
    }

    /**
     * Template: 1605_09_OrderSummaryByCustomer&Item.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1605-09-order-summary-by-customer-item", method = RequestMethod.POST)
    public void print160509OrderSummaryByCustomerItem(HttpServletResponse response, @RequestBody(required = false) Rpt160509OrderSummaryByCustomerItemRequest request) throws Exception {
        if (request == null) request = new Rpt160509OrderSummaryByCustomerItemRequest();
        printReport(response, "1605_09_OrderSummaryByCustomer&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1605-09-order-summary-by-customer-item", method = RequestMethod.GET)
    public void print160509OrderSummaryByCustomerItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print160509OrderSummaryByCustomerItem(response, objectMapper.convertValue(query, Rpt160509OrderSummaryByCustomerItemRequest.class));
    }

    /**
     * Template: 1605_10_OrderSummaryByCustomer,Item&City.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1605-10-order-summary-by-customer-item-city", method = RequestMethod.POST)
    public void print160510OrderSummaryByCustomerItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt160510OrderSummaryByCustomerItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt160510OrderSummaryByCustomerItemCityRequest();
        printReport(response, "1605_10_OrderSummaryByCustomer,Item&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1605-10-order-summary-by-customer-item-city", method = RequestMethod.GET)
    public void print160510OrderSummaryByCustomerItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print160510OrderSummaryByCustomerItemCity(response, objectMapper.convertValue(query, Rpt160510OrderSummaryByCustomerItemCityRequest.class));
    }

    /**
     * Template: 1605_11_OrderSummaryByCustomer&Order.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1605-11-order-summary-by-customer-order", method = RequestMethod.POST)
    public void print160511OrderSummaryByCustomerOrder(HttpServletResponse response, @RequestBody(required = false) Rpt160511OrderSummaryByCustomerOrderRequest request) throws Exception {
        if (request == null) request = new Rpt160511OrderSummaryByCustomerOrderRequest();
        printReport(response, "1605_11_OrderSummaryByCustomer&Order.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1605-11-order-summary-by-customer-order", method = RequestMethod.GET)
    public void print160511OrderSummaryByCustomerOrderGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print160511OrderSummaryByCustomerOrder(response, objectMapper.convertValue(query, Rpt160511OrderSummaryByCustomerOrderRequest.class));
    }

    /**
     * Template: 1617_USP_BillOfMaterial_Slip.rpt
     * Procedure: [pcc].[USP_BillOfMaterial_Register]
     * Desktop: BomHeader.BillOfMaterial_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1617-usp-bill-of-material-slip", method = RequestMethod.POST)
    public void print1617USPBillOfMaterialSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1617USPBillOfMaterialSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1617USPBillOfMaterialSlipRequest();
        printReport(response, "1617_USP_BillOfMaterial_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1617-usp-bill-of-material-slip", method = RequestMethod.GET)
    public void print1617USPBillOfMaterialSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1617USPBillOfMaterialSlip(response, objectMapper.convertValue(query, Rpt1617USPBillOfMaterialSlipRequest.class));
    }

    /**
     * Template: 1633_USP_BillOfMaterial_Slip.rpt
     * Procedure: [pcc].[USP_BillOfMaterial_Register]
     * Desktop: BomHeader.BillOfMaterial_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1633-usp-bill-of-material-slip", method = RequestMethod.POST)
    public void print1633USPBillOfMaterialSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1633USPBillOfMaterialSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1633USPBillOfMaterialSlipRequest();
        printReport(response, "1633_USP_BillOfMaterial_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1633-usp-bill-of-material-slip", method = RequestMethod.GET)
    public void print1633USPBillOfMaterialSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1633USPBillOfMaterialSlip(response, objectMapper.convertValue(query, Rpt1633USPBillOfMaterialSlipRequest.class));
    }

    /**
     * Template: 1634_BillOfMaterialAllDetails_SubReport.rpt
     * Procedure: [pcc].[USP_BillOfMaterialAllDetails_SubReport]
     * Desktop: BomHeader.BillOfMaterialAllDetails_SubReport
     */
    @RequestMapping(value = "/reports/print/1634-bill-of-material-all-details-sub-report", method = RequestMethod.POST)
    public void print1634BillOfMaterialAllDetailsSubReport(HttpServletResponse response, @RequestBody(required = false) Rpt1634BillOfMaterialAllDetailsSubReportRequest request) throws Exception {
        if (request == null) request = new Rpt1634BillOfMaterialAllDetailsSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1634_BillOfMaterialAllDetails_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1634-bill-of-material-all-details-sub-report", method = RequestMethod.GET)
    public void print1634BillOfMaterialAllDetailsSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1634BillOfMaterialAllDetailsSubReport(response, objectMapper.convertValue(query, Rpt1634BillOfMaterialAllDetailsSubReportRequest.class));
    }

    /**
     * Template: 1664_BillOfMaterialCasting_Slip.rpt
     * Procedure: [pcc].[USP_BillOfMaterial_Register]
     * Desktop: BomHeader.BillOfMaterial_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1664-bill-of-material-casting-slip", method = RequestMethod.POST)
    public void print1664BillOfMaterialCastingSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1664BillOfMaterialCastingSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1664BillOfMaterialCastingSlipRequest();
        printReport(response, "1664_BillOfMaterialCasting_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1664-bill-of-material-casting-slip", method = RequestMethod.GET)
    public void print1664BillOfMaterialCastingSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1664BillOfMaterialCastingSlip(response, objectMapper.convertValue(query, Rpt1664BillOfMaterialCastingSlipRequest.class));
    }

    /**
     * Template: 1850_Production_Slip.rpt
     * Procedure: [pcc].[USP_Production_SlipAndRegister]
     * Desktop: ProductionHeader.Production_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1850-production-slip", method = RequestMethod.POST)
    public void print1850ProductionSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1850ProductionSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1850ProductionSlipRequest();
        printReport(response, "1850_Production_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1850-production-slip", method = RequestMethod.GET)
    public void print1850ProductionSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1850ProductionSlip(response, objectMapper.convertValue(query, Rpt1850ProductionSlipRequest.class));
    }

    /**
     * Template: 1851_USP_BillOfMaterial_Slip.rpt
     * Procedure: [pcc].[USP_BillOfMaterial_Register]
     * Desktop: BomHeader.BillOfMaterial_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1851-usp-bill-of-material-slip", method = RequestMethod.POST)
    public void print1851USPBillOfMaterialSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1851USPBillOfMaterialSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1851USPBillOfMaterialSlipRequest();
        printReport(response, "1851_USP_BillOfMaterial_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1851-usp-bill-of-material-slip", method = RequestMethod.GET)
    public void print1851USPBillOfMaterialSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1851USPBillOfMaterialSlip(response, objectMapper.convertValue(query, Rpt1851USPBillOfMaterialSlipRequest.class));
    }

    /**
     * Template: 1852-ApprovalRegister.rpt
     * Procedure: [pcc].[USP_SaleOrderApprovalHistory]
     * Desktop: SaleOrder.SaleOrderApprovalHistory
     */
    @RequestMapping(value = "/reports/print/1852-approval-register", method = RequestMethod.POST)
    public void print1852ApprovalRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1852ApprovalRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1852ApprovalRegisterRequest();
        printReport(response, "1852-ApprovalRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-approval-register", method = RequestMethod.GET)
    public void print1852ApprovalRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1852ApprovalRegister(response, objectMapper.convertValue(query, Rpt1852ApprovalRegisterRequest.class));
    }

    /**
     * Template: 1852-InvRptSaleOrderSlip.rpt
     * Procedure: [pcc].[USP_SaleOrderSlipAndRegister]
     * Desktop: SaleOrder.SaleOrderSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1852-sale-order-slip", method = RequestMethod.POST)
    public void print1852SaleOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1852SaleOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1852SaleOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1852-InvRptSaleOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-sale-order-slip", method = RequestMethod.GET)
    public void print1852SaleOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1852SaleOrderSlip(response, objectMapper.convertValue(query, Rpt1852SaleOrderSlipRequest.class));
    }

    /**
     * Template: 1852-SaleOrderRegister.rpt
     * Procedure: [pcc].[USP_SaleOrderSlipAndRegister]
     * Desktop: SaleOrder.SaleOrderSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1852-sale-order-register", method = RequestMethod.POST)
    public void print1852SaleOrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1852SaleOrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1852SaleOrderRegisterRequest();
        printReport(response, "1852-SaleOrderRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-sale-order-register", method = RequestMethod.GET)
    public void print1852SaleOrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1852SaleOrderRegister(response, objectMapper.convertValue(query, Rpt1852SaleOrderRegisterRequest.class));
    }

    /**
     * Template: 1852_01-OrderRegister.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-01-order-register", method = RequestMethod.POST)
    public void print185201OrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt185201OrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt185201OrderRegisterRequest();
        printReport(response, "1852_01-OrderRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-01-order-register", method = RequestMethod.GET)
    public void print185201OrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185201OrderRegister(response, objectMapper.convertValue(query, Rpt185201OrderRegisterRequest.class));
    }

    /**
     * Template: 1852_02-OrderSummaryByItem.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-02-order-summary-by-item", method = RequestMethod.POST)
    public void print185202OrderSummaryByItem(HttpServletResponse response, @RequestBody(required = false) Rpt185202OrderSummaryByItemRequest request) throws Exception {
        if (request == null) request = new Rpt185202OrderSummaryByItemRequest();
        printReport(response, "1852_02-OrderSummaryByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-02-order-summary-by-item", method = RequestMethod.GET)
    public void print185202OrderSummaryByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185202OrderSummaryByItem(response, objectMapper.convertValue(query, Rpt185202OrderSummaryByItemRequest.class));
    }

    /**
     * Template: 1852_03-OrderSummaryByItem&Varient.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-03-order-summary-by-item-varient", method = RequestMethod.POST)
    public void print185203OrderSummaryByItemVarient(HttpServletResponse response, @RequestBody(required = false) Rpt185203OrderSummaryByItemVarientRequest request) throws Exception {
        if (request == null) request = new Rpt185203OrderSummaryByItemVarientRequest();
        printReport(response, "1852_03-OrderSummaryByItem&Varient.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-03-order-summary-by-item-varient", method = RequestMethod.GET)
    public void print185203OrderSummaryByItemVarientGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185203OrderSummaryByItemVarient(response, objectMapper.convertValue(query, Rpt185203OrderSummaryByItemVarientRequest.class));
    }

    /**
     * Template: 1852_04-OrderSummaryByItem&City.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-04-order-summary-by-item-city", method = RequestMethod.POST)
    public void print185204OrderSummaryByItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt185204OrderSummaryByItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt185204OrderSummaryByItemCityRequest();
        printReport(response, "1852_04-OrderSummaryByItem&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-04-order-summary-by-item-city", method = RequestMethod.GET)
    public void print185204OrderSummaryByItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185204OrderSummaryByItemCity(response, objectMapper.convertValue(query, Rpt185204OrderSummaryByItemCityRequest.class));
    }

    /**
     * Template: 1852_05-OrderSummaryByItem_Varient&City.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-05-order-summary-by-item-varient-city", method = RequestMethod.POST)
    public void print185205OrderSummaryByItemVarientCity(HttpServletResponse response, @RequestBody(required = false) Rpt185205OrderSummaryByItemVarientCityRequest request) throws Exception {
        if (request == null) request = new Rpt185205OrderSummaryByItemVarientCityRequest();
        printReport(response, "1852_05-OrderSummaryByItem_Varient&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-05-order-summary-by-item-varient-city", method = RequestMethod.GET)
    public void print185205OrderSummaryByItemVarientCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185205OrderSummaryByItemVarientCity(response, objectMapper.convertValue(query, Rpt185205OrderSummaryByItemVarientCityRequest.class));
    }

    /**
     * Template: 1852_06-OrderSummaryByCustomer.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-06-order-summary-by-customer", method = RequestMethod.POST)
    public void print185206OrderSummaryByCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt185206OrderSummaryByCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt185206OrderSummaryByCustomerRequest();
        printReport(response, "1852_06-OrderSummaryByCustomer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-06-order-summary-by-customer", method = RequestMethod.GET)
    public void print185206OrderSummaryByCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185206OrderSummaryByCustomer(response, objectMapper.convertValue(query, Rpt185206OrderSummaryByCustomerRequest.class));
    }

    /**
     * Template: 1852_07-OrderSummaryByCustomerItem.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-07-order-summary-by-customer-item", method = RequestMethod.POST)
    public void print185207OrderSummaryByCustomerItem(HttpServletResponse response, @RequestBody(required = false) Rpt185207OrderSummaryByCustomerItemRequest request) throws Exception {
        if (request == null) request = new Rpt185207OrderSummaryByCustomerItemRequest();
        printReport(response, "1852_07-OrderSummaryByCustomerItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-07-order-summary-by-customer-item", method = RequestMethod.GET)
    public void print185207OrderSummaryByCustomerItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185207OrderSummaryByCustomerItem(response, objectMapper.convertValue(query, Rpt185207OrderSummaryByCustomerItemRequest.class));
    }

    /**
     * Template: 1852_08-OrderSummaryByCustomer&City.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-08-order-summary-by-customer-city", method = RequestMethod.POST)
    public void print185208OrderSummaryByCustomerCity(HttpServletResponse response, @RequestBody(required = false) Rpt185208OrderSummaryByCustomerCityRequest request) throws Exception {
        if (request == null) request = new Rpt185208OrderSummaryByCustomerCityRequest();
        printReport(response, "1852_08-OrderSummaryByCustomer&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-08-order-summary-by-customer-city", method = RequestMethod.GET)
    public void print185208OrderSummaryByCustomerCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185208OrderSummaryByCustomerCity(response, objectMapper.convertValue(query, Rpt185208OrderSummaryByCustomerCityRequest.class));
    }

    /**
     * Template: 1852_09-OrderSummaryByCustomer_Item&City.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-09-order-summary-by-customer-item-city", method = RequestMethod.POST)
    public void print185209OrderSummaryByCustomerItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt185209OrderSummaryByCustomerItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt185209OrderSummaryByCustomerItemCityRequest();
        printReport(response, "1852_09-OrderSummaryByCustomer_Item&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-09-order-summary-by-customer-item-city", method = RequestMethod.GET)
    public void print185209OrderSummaryByCustomerItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185209OrderSummaryByCustomerItemCity(response, objectMapper.convertValue(query, Rpt185209OrderSummaryByCustomerItemCityRequest.class));
    }

    /**
     * Template: 1852_10-OrderSummaryByCustomer&Varient.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-10-order-summary-by-customer-varient", method = RequestMethod.POST)
    public void print185210OrderSummaryByCustomerVarient(HttpServletResponse response, @RequestBody(required = false) Rpt185210OrderSummaryByCustomerVarientRequest request) throws Exception {
        if (request == null) request = new Rpt185210OrderSummaryByCustomerVarientRequest();
        printReport(response, "1852_10-OrderSummaryByCustomer&Varient.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-10-order-summary-by-customer-varient", method = RequestMethod.GET)
    public void print185210OrderSummaryByCustomerVarientGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185210OrderSummaryByCustomerVarient(response, objectMapper.convertValue(query, Rpt185210OrderSummaryByCustomerVarientRequest.class));
    }

    /**
     * Template: 1852_11-OrderSummaryByCustomer_ItemVarient.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-11-order-summary-by-customer-item-varient", method = RequestMethod.POST)
    public void print185211OrderSummaryByCustomerItemVarient(HttpServletResponse response, @RequestBody(required = false) Rpt185211OrderSummaryByCustomerItemVarientRequest request) throws Exception {
        if (request == null) request = new Rpt185211OrderSummaryByCustomerItemVarientRequest();
        printReport(response, "1852_11-OrderSummaryByCustomer_ItemVarient.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-11-order-summary-by-customer-item-varient", method = RequestMethod.GET)
    public void print185211OrderSummaryByCustomerItemVarientGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185211OrderSummaryByCustomerItemVarient(response, objectMapper.convertValue(query, Rpt185211OrderSummaryByCustomerItemVarientRequest.class));
    }

    /**
     * Template: 1852_12-OrderSummaryByCustomer&ReferenceParty.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-12-order-summary-by-customer-reference-party", method = RequestMethod.POST)
    public void print185212OrderSummaryByCustomerReferenceParty(HttpServletResponse response, @RequestBody(required = false) Rpt185212OrderSummaryByCustomerReferencePartyRequest request) throws Exception {
        if (request == null) request = new Rpt185212OrderSummaryByCustomerReferencePartyRequest();
        printReport(response, "1852_12-OrderSummaryByCustomer&ReferenceParty.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-12-order-summary-by-customer-reference-party", method = RequestMethod.GET)
    public void print185212OrderSummaryByCustomerReferencePartyGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185212OrderSummaryByCustomerReferenceParty(response, objectMapper.convertValue(query, Rpt185212OrderSummaryByCustomerReferencePartyRequest.class));
    }

    /**
     * Template: 1852_13-OrderSummaryByCustomerItem&ReferenceParty.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-13-order-summary-by-customer-item-reference-party", method = RequestMethod.POST)
    public void print185213OrderSummaryByCustomerItemReferenceParty(HttpServletResponse response, @RequestBody(required = false) Rpt185213OrderSummaryByCustomerItemReferencePartyRequest request) throws Exception {
        if (request == null) request = new Rpt185213OrderSummaryByCustomerItemReferencePartyRequest();
        printReport(response, "1852_13-OrderSummaryByCustomerItem&ReferenceParty.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-13-order-summary-by-customer-item-reference-party", method = RequestMethod.GET)
    public void print185213OrderSummaryByCustomerItemReferencePartyGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185213OrderSummaryByCustomerItemReferenceParty(response, objectMapper.convertValue(query, Rpt185213OrderSummaryByCustomerItemReferencePartyRequest.class));
    }

    /**
     * Template: 1852_14-OrderSummaryByCustomerVarient&ReferenceParty.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-14-order-summary-by-customer-varient-reference-party", method = RequestMethod.POST)
    public void print185214OrderSummaryByCustomerVarientReferenceParty(HttpServletResponse response, @RequestBody(required = false) Rpt185214OrderSummaryByCustomerVarientReferencePartyRequest request) throws Exception {
        if (request == null) request = new Rpt185214OrderSummaryByCustomerVarientReferencePartyRequest();
        printReport(response, "1852_14-OrderSummaryByCustomerVarient&ReferenceParty.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-14-order-summary-by-customer-varient-reference-party", method = RequestMethod.GET)
    public void print185214OrderSummaryByCustomerVarientReferencePartyGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185214OrderSummaryByCustomerVarientReferenceParty(response, objectMapper.convertValue(query, Rpt185214OrderSummaryByCustomerVarientReferencePartyRequest.class));
    }

    /**
     * Template: 1852_15-OrderSummaryByCustomer_Item_Varient&ReferenceParty.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-15-order-summary-by-customer-item-varient-reference-party", method = RequestMethod.POST)
    public void print185215OrderSummaryByCustomerItemVarientReferenceParty(HttpServletResponse response, @RequestBody(required = false) Rpt185215OrderSummaryByCustomerItemVarientReferencePartyRequest request) throws Exception {
        if (request == null) request = new Rpt185215OrderSummaryByCustomerItemVarientReferencePartyRequest();
        printReport(response, "1852_15-OrderSummaryByCustomer_Item_Varient&ReferenceParty.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-15-order-summary-by-customer-item-varient-reference-party", method = RequestMethod.GET)
    public void print185215OrderSummaryByCustomerItemVarientReferencePartyGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185215OrderSummaryByCustomerItemVarientReferenceParty(response, objectMapper.convertValue(query, Rpt185215OrderSummaryByCustomerItemVarientReferencePartyRequest.class));
    }

    /**
     * Template: 1852_16-OrderSummaryByReferenceParty.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-16-order-summary-by-reference-party", method = RequestMethod.POST)
    public void print185216OrderSummaryByReferenceParty(HttpServletResponse response, @RequestBody(required = false) Rpt185216OrderSummaryByReferencePartyRequest request) throws Exception {
        if (request == null) request = new Rpt185216OrderSummaryByReferencePartyRequest();
        printReport(response, "1852_16-OrderSummaryByReferenceParty.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-16-order-summary-by-reference-party", method = RequestMethod.GET)
    public void print185216OrderSummaryByReferencePartyGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185216OrderSummaryByReferenceParty(response, objectMapper.convertValue(query, Rpt185216OrderSummaryByReferencePartyRequest.class));
    }

    /**
     * Template: 1852_17-OrderSummaryByReferenceParty&Item.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-17-order-summary-by-reference-party-item", method = RequestMethod.POST)
    public void print185217OrderSummaryByReferencePartyItem(HttpServletResponse response, @RequestBody(required = false) Rpt185217OrderSummaryByReferencePartyItemRequest request) throws Exception {
        if (request == null) request = new Rpt185217OrderSummaryByReferencePartyItemRequest();
        printReport(response, "1852_17-OrderSummaryByReferenceParty&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-17-order-summary-by-reference-party-item", method = RequestMethod.GET)
    public void print185217OrderSummaryByReferencePartyItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185217OrderSummaryByReferencePartyItem(response, objectMapper.convertValue(query, Rpt185217OrderSummaryByReferencePartyItemRequest.class));
    }

    /**
     * Template: 1852_18-OrderSummaryByReferenceParty&Varient.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-18-order-summary-by-reference-party-varient", method = RequestMethod.POST)
    public void print185218OrderSummaryByReferencePartyVarient(HttpServletResponse response, @RequestBody(required = false) Rpt185218OrderSummaryByReferencePartyVarientRequest request) throws Exception {
        if (request == null) request = new Rpt185218OrderSummaryByReferencePartyVarientRequest();
        printReport(response, "1852_18-OrderSummaryByReferenceParty&Varient.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-18-order-summary-by-reference-party-varient", method = RequestMethod.GET)
    public void print185218OrderSummaryByReferencePartyVarientGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185218OrderSummaryByReferencePartyVarient(response, objectMapper.convertValue(query, Rpt185218OrderSummaryByReferencePartyVarientRequest.class));
    }

    /**
     * Template: 1852_19-OrderSummaryByReferenceParty_Item&Varient.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-19-order-summary-by-reference-party-item-varient", method = RequestMethod.POST)
    public void print185219OrderSummaryByReferencePartyItemVarient(HttpServletResponse response, @RequestBody(required = false) Rpt185219OrderSummaryByReferencePartyItemVarientRequest request) throws Exception {
        if (request == null) request = new Rpt185219OrderSummaryByReferencePartyItemVarientRequest();
        printReport(response, "1852_19-OrderSummaryByReferenceParty_Item&Varient.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-19-order-summary-by-reference-party-item-varient", method = RequestMethod.GET)
    public void print185219OrderSummaryByReferencePartyItemVarientGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185219OrderSummaryByReferencePartyItemVarient(response, objectMapper.convertValue(query, Rpt185219OrderSummaryByReferencePartyItemVarientRequest.class));
    }

    /**
     * Template: 1852_20-OrderSummaryByReferenceParty&City.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-20-order-summary-by-reference-party-city", method = RequestMethod.POST)
    public void print185220OrderSummaryByReferencePartyCity(HttpServletResponse response, @RequestBody(required = false) Rpt185220OrderSummaryByReferencePartyCityRequest request) throws Exception {
        if (request == null) request = new Rpt185220OrderSummaryByReferencePartyCityRequest();
        printReport(response, "1852_20-OrderSummaryByReferenceParty&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-20-order-summary-by-reference-party-city", method = RequestMethod.GET)
    public void print185220OrderSummaryByReferencePartyCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185220OrderSummaryByReferencePartyCity(response, objectMapper.convertValue(query, Rpt185220OrderSummaryByReferencePartyCityRequest.class));
    }

    /**
     * Template: 1852_21-dtOrderSummaryByOrderNoAndCustomer.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-21-dt-order-summary-by-order-no-and-customer", method = RequestMethod.POST)
    public void print185221DtOrderSummaryByOrderNoAndCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt185221DtOrderSummaryByOrderNoAndCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt185221DtOrderSummaryByOrderNoAndCustomerRequest();
        printReport(response, "1852_21-dtOrderSummaryByOrderNoAndCustomer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-21-dt-order-summary-by-order-no-and-customer", method = RequestMethod.GET)
    public void print185221DtOrderSummaryByOrderNoAndCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185221DtOrderSummaryByOrderNoAndCustomer(response, objectMapper.convertValue(query, Rpt185221DtOrderSummaryByOrderNoAndCustomerRequest.class));
    }

    /**
     * Template: 1852_22-OrderSummaryByOrderNoAndSalesMan.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/1852-22-order-summary-by-order-no-and-sales-man", method = RequestMethod.POST)
    public void print185222OrderSummaryByOrderNoAndSalesMan(HttpServletResponse response, @RequestBody(required = false) Rpt185222OrderSummaryByOrderNoAndSalesManRequest request) throws Exception {
        if (request == null) request = new Rpt185222OrderSummaryByOrderNoAndSalesManRequest();
        printReport(response, "1852_22-OrderSummaryByOrderNoAndSalesMan.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852-22-order-summary-by-order-no-and-sales-man", method = RequestMethod.GET)
    public void print185222OrderSummaryByOrderNoAndSalesManGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185222OrderSummaryByOrderNoAndSalesMan(response, objectMapper.convertValue(query, Rpt185222OrderSummaryByOrderNoAndSalesManRequest.class));
    }

    /**
     * Template: 1852A-SaleOrder_Register.rpt
     * Procedure: [pcc].[USP_SaleOrderSlipAndRegister]
     * Desktop: SaleOrder.SaleOrderSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1852a-sale-order-register", method = RequestMethod.POST)
    public void print1852ASaleOrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1852ASaleOrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1852ASaleOrderRegisterRequest();
        printReport(response, "1852A-SaleOrder_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1852a-sale-order-register", method = RequestMethod.GET)
    public void print1852ASaleOrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1852ASaleOrderRegister(response, objectMapper.convertValue(query, Rpt1852ASaleOrderRegisterRequest.class));
    }

    /**
     * Template: 1853-DeliveryOrderSlip.rpt
     * Procedure: [pcc].[USP_InvDeliveryOrder_Slip]
     * Desktop: InvDeliveryOrder.DeliveryOrder_SlipConcrete
     */
    @RequestMapping(value = "/reports/print/1853-delivery-order-slip", method = RequestMethod.POST)
    public void print1853DeliveryOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1853DeliveryOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1853DeliveryOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1853-DeliveryOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1853-delivery-order-slip", method = RequestMethod.GET)
    public void print1853DeliveryOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1853DeliveryOrderSlip(response, objectMapper.convertValue(query, Rpt1853DeliveryOrderSlipRequest.class));
    }

    /**
     * Template: 1853A-DeliveryOrderSlip.rpt
     * Procedure: [pcc].[USP_InvDeliveryOrder_Slip]
     * Desktop: InvDeliveryOrder.DeliveryOrder_SlipConcrete
     */
    @RequestMapping(value = "/reports/print/1853a-delivery-order-slip", method = RequestMethod.POST)
    public void print1853ADeliveryOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1853ADeliveryOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1853ADeliveryOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1853A-DeliveryOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1853a-delivery-order-slip", method = RequestMethod.GET)
    public void print1853ADeliveryOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1853ADeliveryOrderSlip(response, objectMapper.convertValue(query, Rpt1853ADeliveryOrderSlipRequest.class));
    }

    /**
     * Template: 1855-GDNRegister.rpt
     * Procedure: [pcc].[USP_GdnRegister]
     * Desktop: InvGdn.GdnRegister
     */
    @RequestMapping(value = "/reports/print/1855-gdn-register", method = RequestMethod.POST)
    public void print1855GDNRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1855GDNRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1855GDNRegisterRequest();
        printReport(response, "1855-GDNRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1855-gdn-register", method = RequestMethod.GET)
    public void print1855GDNRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1855GDNRegister(response, objectMapper.convertValue(query, Rpt1855GDNRegisterRequest.class));
    }

    /**
     * Template: 1855-InvGdn_Slip.rpt
     * Procedure: [pcc].[USP_InvGdn_Slip]
     * Desktop: InvGdn.GdnConcrete_Slip
     */
    @RequestMapping(value = "/reports/print/1855-inv-gdn-slip", method = RequestMethod.POST)
    public void print1855InvGdnSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1855InvGdnSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1855InvGdnSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1855-InvGdn_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1855-inv-gdn-slip", method = RequestMethod.GET)
    public void print1855InvGdnSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1855InvGdnSlip(response, objectMapper.convertValue(query, Rpt1855InvGdnSlipRequest.class));
    }

    /**
     * Template: 1855A-GDNRegister.rpt
     * Procedure: [pcc].[USP_GdnRegister]
     * Desktop: InvGdn.GdnRegister
     */
    @RequestMapping(value = "/reports/print/1855a-gdn-register", method = RequestMethod.POST)
    public void print1855AGDNRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1855AGDNRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1855AGDNRegisterRequest();
        printReport(response, "1855A-GDNRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1855a-gdn-register", method = RequestMethod.GET)
    public void print1855AGDNRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1855AGDNRegister(response, objectMapper.convertValue(query, Rpt1855AGDNRegisterRequest.class));
    }

    /**
     * Template: 1856A-SaleInvoiceDirect_Slip.rpt
     * Procedure: pcc.USP_InvSaleInvoice_DirectSlip
     * Desktop: InvSaleInvoice.SaleInvoiceDirectSlip
     */
    @RequestMapping(value = "/reports/print/1856a-sale-invoice-direct-slip", method = RequestMethod.POST)
    public void print1856ASaleInvoiceDirectSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1856ASaleInvoiceDirectSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1856ASaleInvoiceDirectSlipRequest();
        printReport(response, "1856A-SaleInvoiceDirect_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1856a-sale-invoice-direct-slip", method = RequestMethod.GET)
    public void print1856ASaleInvoiceDirectSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1856ASaleInvoiceDirectSlip(response, objectMapper.convertValue(query, Rpt1856ASaleInvoiceDirectSlipRequest.class));
    }

    /**
     * Template: 1859-PurchaseInvoice_ApprovalRegister.rpt
     * Procedure: [pcc].[USP_PurchaseInvoice_ApprovalHistory]
     * Desktop: InvPurchaseInvoice.PurchaseInvoice_ApprovalHistory
     */
    @RequestMapping(value = "/reports/print/1859-purchase-invoice-approval-register", method = RequestMethod.POST)
    public void print1859PurchaseInvoiceApprovalRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1859PurchaseInvoiceApprovalRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1859PurchaseInvoiceApprovalRegisterRequest();
        printReport(response, "1859-PurchaseInvoice_ApprovalRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1859-purchase-invoice-approval-register", method = RequestMethod.GET)
    public void print1859PurchaseInvoiceApprovalRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1859PurchaseInvoiceApprovalRegister(response, objectMapper.convertValue(query, Rpt1859PurchaseInvoiceApprovalRegisterRequest.class));
    }

    /**
     * Template: 1859_01-PurchaseRegisterSummary.rpt
     * Procedure: [pcc].[USP_Purchase_EvaulationDetailReports]
     * Desktop: InvPurchaseInvoice.Purchase_EvaulationDetailReports
     */
    @RequestMapping(value = "/reports/print/1859-01-purchase-register-summary", method = RequestMethod.POST)
    public void print185901PurchaseRegisterSummary(HttpServletResponse response, @RequestBody(required = false) Rpt185901PurchaseRegisterSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt185901PurchaseRegisterSummaryRequest();
        printReport(response, "1859_01-PurchaseRegisterSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1859-01-purchase-register-summary", method = RequestMethod.GET)
    public void print185901PurchaseRegisterSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185901PurchaseRegisterSummary(response, objectMapper.convertValue(query, Rpt185901PurchaseRegisterSummaryRequest.class));
    }

    /**
     * Template: 1859_02-PurchaseRegisterSummaryByItem.rpt
     * Procedure: [pcc].[USP_Purchase_EvaulationDetailReports]
     * Desktop: InvPurchaseInvoice.Purchase_EvaulationDetailReports
     */
    @RequestMapping(value = "/reports/print/1859-02-purchase-register-summary-by-item", method = RequestMethod.POST)
    public void print185902PurchaseRegisterSummaryByItem(HttpServletResponse response, @RequestBody(required = false) Rpt185902PurchaseRegisterSummaryByItemRequest request) throws Exception {
        if (request == null) request = new Rpt185902PurchaseRegisterSummaryByItemRequest();
        printReport(response, "1859_02-PurchaseRegisterSummaryByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1859-02-purchase-register-summary-by-item", method = RequestMethod.GET)
    public void print185902PurchaseRegisterSummaryByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185902PurchaseRegisterSummaryByItem(response, objectMapper.convertValue(query, Rpt185902PurchaseRegisterSummaryByItemRequest.class));
    }

    /**
     * Template: 1859_03-PurchaseSummaryByItem&City.rpt
     * Procedure: [pcc].[USP_Purchase_EvaulationDetailReports]
     * Desktop: InvPurchaseInvoice.Purchase_EvaulationDetailReports
     */
    @RequestMapping(value = "/reports/print/1859-03-purchase-summary-by-item-city", method = RequestMethod.POST)
    public void print185903PurchaseSummaryByItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt185903PurchaseSummaryByItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt185903PurchaseSummaryByItemCityRequest();
        printReport(response, "1859_03-PurchaseSummaryByItem&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1859-03-purchase-summary-by-item-city", method = RequestMethod.GET)
    public void print185903PurchaseSummaryByItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185903PurchaseSummaryByItemCity(response, objectMapper.convertValue(query, Rpt185903PurchaseSummaryByItemCityRequest.class));
    }

    /**
     * Template: 1859_04-PurchaseRegisterSummaryByItem&Varient.rpt
     * Procedure: [pcc].[USP_Purchase_EvaulationDetailReports]
     * Desktop: InvPurchaseInvoice.Purchase_EvaulationDetailReports
     */
    @RequestMapping(value = "/reports/print/1859-04-purchase-register-summary-by-item-varient", method = RequestMethod.POST)
    public void print185904PurchaseRegisterSummaryByItemVarient(HttpServletResponse response, @RequestBody(required = false) Rpt185904PurchaseRegisterSummaryByItemVarientRequest request) throws Exception {
        if (request == null) request = new Rpt185904PurchaseRegisterSummaryByItemVarientRequest();
        printReport(response, "1859_04-PurchaseRegisterSummaryByItem&Varient.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1859-04-purchase-register-summary-by-item-varient", method = RequestMethod.GET)
    public void print185904PurchaseRegisterSummaryByItemVarientGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185904PurchaseRegisterSummaryByItemVarient(response, objectMapper.convertValue(query, Rpt185904PurchaseRegisterSummaryByItemVarientRequest.class));
    }

    /**
     * Template: 1859_05-PurchaseRegisterSummaryByItem&Warehouse.rpt
     * Procedure: [pcc].[USP_Purchase_EvaulationDetailReports]
     * Desktop: InvPurchaseInvoice.Purchase_EvaulationDetailReports
     */
    @RequestMapping(value = "/reports/print/1859-05-purchase-register-summary-by-item-warehouse", method = RequestMethod.POST)
    public void print185905PurchaseRegisterSummaryByItemWarehouse(HttpServletResponse response, @RequestBody(required = false) Rpt185905PurchaseRegisterSummaryByItemWarehouseRequest request) throws Exception {
        if (request == null) request = new Rpt185905PurchaseRegisterSummaryByItemWarehouseRequest();
        printReport(response, "1859_05-PurchaseRegisterSummaryByItem&Warehouse.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1859-05-purchase-register-summary-by-item-warehouse", method = RequestMethod.GET)
    public void print185905PurchaseRegisterSummaryByItemWarehouseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185905PurchaseRegisterSummaryByItemWarehouse(response, objectMapper.convertValue(query, Rpt185905PurchaseRegisterSummaryByItemWarehouseRequest.class));
    }

    /**
     * Template: 1859_06-PurchaseSummaryByItemPackSize&City.rpt
     * Procedure: [pcc].[USP_Purchase_EvaulationDetailReports]
     * Desktop: InvPurchaseInvoice.Purchase_EvaulationDetailReports
     */
    @RequestMapping(value = "/reports/print/1859-06-purchase-summary-by-item-pack-size-city", method = RequestMethod.POST)
    public void print185906PurchaseSummaryByItemPackSizeCity(HttpServletResponse response, @RequestBody(required = false) Rpt185906PurchaseSummaryByItemPackSizeCityRequest request) throws Exception {
        if (request == null) request = new Rpt185906PurchaseSummaryByItemPackSizeCityRequest();
        printReport(response, "1859_06-PurchaseSummaryByItemPackSize&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1859-06-purchase-summary-by-item-pack-size-city", method = RequestMethod.GET)
    public void print185906PurchaseSummaryByItemPackSizeCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185906PurchaseSummaryByItemPackSizeCity(response, objectMapper.convertValue(query, Rpt185906PurchaseSummaryByItemPackSizeCityRequest.class));
    }

    /**
     * Template: 1859_07-PurchaseRegisterSummaryBySupplier.rpt
     * Procedure: [pcc].[USP_Purchase_EvaulationDetailReports]
     * Desktop: InvPurchaseInvoice.Purchase_EvaulationDetailReports
     */
    @RequestMapping(value = "/reports/print/1859-07-purchase-register-summary-by-supplier", method = RequestMethod.POST)
    public void print185907PurchaseRegisterSummaryBySupplier(HttpServletResponse response, @RequestBody(required = false) Rpt185907PurchaseRegisterSummaryBySupplierRequest request) throws Exception {
        if (request == null) request = new Rpt185907PurchaseRegisterSummaryBySupplierRequest();
        printReport(response, "1859_07-PurchaseRegisterSummaryBySupplier.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1859-07-purchase-register-summary-by-supplier", method = RequestMethod.GET)
    public void print185907PurchaseRegisterSummaryBySupplierGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185907PurchaseRegisterSummaryBySupplier(response, objectMapper.convertValue(query, Rpt185907PurchaseRegisterSummaryBySupplierRequest.class));
    }

    /**
     * Template: 1859_08-PurchaseSummaryBySupplier&Item.rpt
     * Procedure: [pcc].[USP_Purchase_EvaulationDetailReports]
     * Desktop: InvPurchaseInvoice.Purchase_EvaulationDetailReports
     */
    @RequestMapping(value = "/reports/print/1859-08-purchase-summary-by-supplier-item", method = RequestMethod.POST)
    public void print185908PurchaseSummaryBySupplierItem(HttpServletResponse response, @RequestBody(required = false) Rpt185908PurchaseSummaryBySupplierItemRequest request) throws Exception {
        if (request == null) request = new Rpt185908PurchaseSummaryBySupplierItemRequest();
        printReport(response, "1859_08-PurchaseSummaryBySupplier&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1859-08-purchase-summary-by-supplier-item", method = RequestMethod.GET)
    public void print185908PurchaseSummaryBySupplierItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185908PurchaseSummaryBySupplierItem(response, objectMapper.convertValue(query, Rpt185908PurchaseSummaryBySupplierItemRequest.class));
    }

    /**
     * Template: 1859_09-PurchaseRegisterSummaryBySupplier&City.rpt
     * Procedure: [pcc].[USP_Purchase_EvaulationDetailReports]
     * Desktop: InvPurchaseInvoice.Purchase_EvaulationDetailReports
     */
    @RequestMapping(value = "/reports/print/1859-09-purchase-register-summary-by-supplier-city", method = RequestMethod.POST)
    public void print185909PurchaseRegisterSummaryBySupplierCity(HttpServletResponse response, @RequestBody(required = false) Rpt185909PurchaseRegisterSummaryBySupplierCityRequest request) throws Exception {
        if (request == null) request = new Rpt185909PurchaseRegisterSummaryBySupplierCityRequest();
        printReport(response, "1859_09-PurchaseRegisterSummaryBySupplier&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1859-09-purchase-register-summary-by-supplier-city", method = RequestMethod.GET)
    public void print185909PurchaseRegisterSummaryBySupplierCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185909PurchaseRegisterSummaryBySupplierCity(response, objectMapper.convertValue(query, Rpt185909PurchaseRegisterSummaryBySupplierCityRequest.class));
    }

    /**
     * Template: 1859_10-PurchaseSummaryBySupplierItem&City.rpt
     * Procedure: [pcc].[USP_Purchase_EvaulationDetailReports]
     * Desktop: InvPurchaseInvoice.Purchase_EvaulationDetailReports
     */
    @RequestMapping(value = "/reports/print/1859-10-purchase-summary-by-supplier-item-city", method = RequestMethod.POST)
    public void print185910PurchaseSummaryBySupplierItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt185910PurchaseSummaryBySupplierItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt185910PurchaseSummaryBySupplierItemCityRequest();
        printReport(response, "1859_10-PurchaseSummaryBySupplierItem&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1859-10-purchase-summary-by-supplier-item-city", method = RequestMethod.GET)
    public void print185910PurchaseSummaryBySupplierItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185910PurchaseSummaryBySupplierItemCity(response, objectMapper.convertValue(query, Rpt185910PurchaseSummaryBySupplierItemCityRequest.class));
    }

    /**
     * Template: 1859_11-PurchaseSummaryBySupplier&Varient.rpt
     * Procedure: [pcc].[USP_Purchase_EvaulationDetailReports]
     * Desktop: InvPurchaseInvoice.Purchase_EvaulationDetailReports
     */
    @RequestMapping(value = "/reports/print/1859-11-purchase-summary-by-supplier-varient", method = RequestMethod.POST)
    public void print185911PurchaseSummaryBySupplierVarient(HttpServletResponse response, @RequestBody(required = false) Rpt185911PurchaseSummaryBySupplierVarientRequest request) throws Exception {
        if (request == null) request = new Rpt185911PurchaseSummaryBySupplierVarientRequest();
        printReport(response, "1859_11-PurchaseSummaryBySupplier&Varient.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1859-11-purchase-summary-by-supplier-varient", method = RequestMethod.GET)
    public void print185911PurchaseSummaryBySupplierVarientGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185911PurchaseSummaryBySupplierVarient(response, objectMapper.convertValue(query, Rpt185911PurchaseSummaryBySupplierVarientRequest.class));
    }

    /**
     * Template: 1859_12-PurchaseSummaryByCity.rpt
     * Procedure: [pcc].[USP_Purchase_EvaulationDetailReports]
     * Desktop: InvPurchaseInvoice.Purchase_EvaulationDetailReports
     */
    @RequestMapping(value = "/reports/print/1859-12-purchase-summary-by-city", method = RequestMethod.POST)
    public void print185912PurchaseSummaryByCity(HttpServletResponse response, @RequestBody(required = false) Rpt185912PurchaseSummaryByCityRequest request) throws Exception {
        if (request == null) request = new Rpt185912PurchaseSummaryByCityRequest();
        printReport(response, "1859_12-PurchaseSummaryByCity.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1859-12-purchase-summary-by-city", method = RequestMethod.GET)
    public void print185912PurchaseSummaryByCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185912PurchaseSummaryByCity(response, objectMapper.convertValue(query, Rpt185912PurchaseSummaryByCityRequest.class));
    }

    /**
     * Template: 1859_13-PurchaseSummaryByParentCategory.rpt
     * Procedure: [pcc].[USP_Purchase_EvaulationDetailReports]
     * Desktop: InvPurchaseInvoice.Purchase_EvaulationDetailReports
     */
    @RequestMapping(value = "/reports/print/1859-13-purchase-summary-by-parent-category", method = RequestMethod.POST)
    public void print185913PurchaseSummaryByParentCategory(HttpServletResponse response, @RequestBody(required = false) Rpt185913PurchaseSummaryByParentCategoryRequest request) throws Exception {
        if (request == null) request = new Rpt185913PurchaseSummaryByParentCategoryRequest();
        printReport(response, "1859_13-PurchaseSummaryByParentCategory.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1859-13-purchase-summary-by-parent-category", method = RequestMethod.GET)
    public void print185913PurchaseSummaryByParentCategoryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185913PurchaseSummaryByParentCategory(response, objectMapper.convertValue(query, Rpt185913PurchaseSummaryByParentCategoryRequest.class));
    }

    /**
     * Template: 1859_14-PurchaseSummaryByParentCategory&Item.rpt
     * Procedure: [pcc].[USP_Purchase_EvaulationDetailReports]
     * Desktop: InvPurchaseInvoice.Purchase_EvaulationDetailReports
     */
    @RequestMapping(value = "/reports/print/1859-14-purchase-summary-by-parent-category-item", method = RequestMethod.POST)
    public void print185914PurchaseSummaryByParentCategoryItem(HttpServletResponse response, @RequestBody(required = false) Rpt185914PurchaseSummaryByParentCategoryItemRequest request) throws Exception {
        if (request == null) request = new Rpt185914PurchaseSummaryByParentCategoryItemRequest();
        printReport(response, "1859_14-PurchaseSummaryByParentCategory&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1859-14-purchase-summary-by-parent-category-item", method = RequestMethod.GET)
    public void print185914PurchaseSummaryByParentCategoryItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185914PurchaseSummaryByParentCategoryItem(response, objectMapper.convertValue(query, Rpt185914PurchaseSummaryByParentCategoryItemRequest.class));
    }

    /**
     * Template: 1859_15-PurchaseSummaryByParentCategory&Supplier.rpt
     * Procedure: [pcc].[USP_Purchase_EvaulationDetailReports]
     * Desktop: InvPurchaseInvoice.Purchase_EvaulationDetailReports
     */
    @RequestMapping(value = "/reports/print/1859-15-purchase-summary-by-parent-category-supplier", method = RequestMethod.POST)
    public void print185915PurchaseSummaryByParentCategorySupplier(HttpServletResponse response, @RequestBody(required = false) Rpt185915PurchaseSummaryByParentCategorySupplierRequest request) throws Exception {
        if (request == null) request = new Rpt185915PurchaseSummaryByParentCategorySupplierRequest();
        printReport(response, "1859_15-PurchaseSummaryByParentCategory&Supplier.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1859-15-purchase-summary-by-parent-category-supplier", method = RequestMethod.GET)
    public void print185915PurchaseSummaryByParentCategorySupplierGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print185915PurchaseSummaryByParentCategorySupplier(response, objectMapper.convertValue(query, Rpt185915PurchaseSummaryByParentCategorySupplierRequest.class));
    }

    /**
     * Template: 1859_InvPurchaseInvoice_DirectSlip.rpt
     * Procedure: [pcc].[USP_InvPurchaseInvoice_DirectSlip]
     * Desktop: InvPurchaseInvoice.PurchaseInvoiceDirectSlip
     */
    @RequestMapping(value = "/reports/print/1859-inv-purchase-invoice-direct-slip", method = RequestMethod.POST)
    public void print1859InvPurchaseInvoiceDirectSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1859InvPurchaseInvoiceDirectSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1859InvPurchaseInvoiceDirectSlipRequest();
        printReport(response, "1859_InvPurchaseInvoice_DirectSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1859-inv-purchase-invoice-direct-slip", method = RequestMethod.GET)
    public void print1859InvPurchaseInvoiceDirectSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1859InvPurchaseInvoiceDirectSlip(response, objectMapper.convertValue(query, Rpt1859InvPurchaseInvoiceDirectSlipRequest.class));
    }

    /**
     * Template: 1859A_InvPurchaseInvoice_DirectSlip.rpt
     * Procedure: [pcc].[USP_InvPurchaseInvoice_DirectSlip]
     * Desktop: InvPurchaseInvoice.PurchaseInvoiceDirectSlip
     */
    @RequestMapping(value = "/reports/print/1859a-inv-purchase-invoice-direct-slip", method = RequestMethod.POST)
    public void print1859AInvPurchaseInvoiceDirectSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1859AInvPurchaseInvoiceDirectSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1859AInvPurchaseInvoiceDirectSlipRequest();
        printReport(response, "1859A_InvPurchaseInvoice_DirectSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1859a-inv-purchase-invoice-direct-slip", method = RequestMethod.GET)
    public void print1859AInvPurchaseInvoiceDirectSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1859AInvPurchaseInvoiceDirectSlip(response, objectMapper.convertValue(query, Rpt1859AInvPurchaseInvoiceDirectSlipRequest.class));
    }

    /**
     * Template: 1860-StockSummaryDetail.rpt
     * Procedure: [pcc].[USP_ItemStockReportWithOutValues]
     * Desktop: StocksReport.ItemStockReportWithOutValuesConcrete
     */
    @RequestMapping(value = "/reports/print/1860-stock-summary-detail", method = RequestMethod.POST)
    public void print1860StockSummaryDetail(HttpServletResponse response, @RequestBody(required = false) Rpt1860StockSummaryDetailRequest request) throws Exception {
        if (request == null) request = new Rpt1860StockSummaryDetailRequest();
        printReport(response, "1860-StockSummaryDetail.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1860-stock-summary-detail", method = RequestMethod.GET)
    public void print1860StockSummaryDetailGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1860StockSummaryDetail(response, objectMapper.convertValue(query, Rpt1860StockSummaryDetailRequest.class));
    }

    /**
     * Template: 1860-StockTransferManual-Slip.rpt
     * Procedure: [Mfg].[USP_InvStockTransferSlipRegister]
     * Desktop: InvStockTransferHeader.StockTransferRegister
     */
    @RequestMapping(value = "/reports/print/1860-stock-transfer-manual-slip", method = RequestMethod.POST)
    public void print1860StockTransferManualSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1860StockTransferManualSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1860StockTransferManualSlipRequest();
        printReport(response, "1860-StockTransferManual-Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1860-stock-transfer-manual-slip", method = RequestMethod.GET)
    public void print1860StockTransferManualSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1860StockTransferManualSlip(response, objectMapper.convertValue(query, Rpt1860StockTransferManualSlipRequest.class));
    }

    /**
     * Template: 1860-StockTransferManual_Register.rpt
     * Procedure: [pcc].[USP_InvStockTransferSlipRegister]
     * Desktop: InvStockTransferHeader.StockTransferRegister
     */
    @RequestMapping(value = "/reports/print/1860-stock-transfer-manual-register", method = RequestMethod.POST)
    public void print1860StockTransferManualRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1860StockTransferManualRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1860StockTransferManualRegisterRequest();
        printReport(response, "1860-StockTransferManual_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1860-stock-transfer-manual-register", method = RequestMethod.GET)
    public void print1860StockTransferManualRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1860StockTransferManualRegister(response, objectMapper.convertValue(query, Rpt1860StockTransferManualRegisterRequest.class));
    }

    /**
     * Template: 1861-ItemStockSummary.rpt
     * Procedure: [pcc].[USP_ItemStockReportWithOutValues]
     * Desktop: StocksReport.ItemStockReportWithOutValuesConcrete
     */
    @RequestMapping(value = "/reports/print/1861-item-stock-summary", method = RequestMethod.POST)
    public void print1861ItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1861ItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1861ItemStockSummaryRequest();
        printReport(response, "1861-ItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1861-item-stock-summary", method = RequestMethod.GET)
    public void print1861ItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1861ItemStockSummary(response, objectMapper.convertValue(query, Rpt1861ItemStockSummaryRequest.class));
    }

    /**
     * Template: 1861-SaleInvoice_Slip.rpt
     * Procedure: [pcc].[USP_InvSaleInvoice_Slip]
     * Desktop: InvSaleInvoice.SaleInvoiceSlip
     */
    @RequestMapping(value = "/reports/print/1861-sale-invoice-slip", method = RequestMethod.POST)
    public void print1861SaleInvoiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1861SaleInvoiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1861SaleInvoiceSlipRequest();
        printReport(response, "1861-SaleInvoice_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1861-sale-invoice-slip", method = RequestMethod.GET)
    public void print1861SaleInvoiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1861SaleInvoiceSlip(response, objectMapper.convertValue(query, Rpt1861SaleInvoiceSlipRequest.class));
    }

    /**
     * Template: 1861-SaleInvoiceApprovalRegister.rpt
     * Procedure: [pcc].[USP_SaleInvoice_ApprovalHistory]
     * Desktop: InvSaleInvoice.SaleInvoice_ApprovalHistory
     */
    @RequestMapping(value = "/reports/print/1861-sale-invoice-approval-register", method = RequestMethod.POST)
    public void print1861SaleInvoiceApprovalRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1861SaleInvoiceApprovalRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1861SaleInvoiceApprovalRegisterRequest();
        printReport(response, "1861-SaleInvoiceApprovalRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1861-sale-invoice-approval-register", method = RequestMethod.GET)
    public void print1861SaleInvoiceApprovalRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1861SaleInvoiceApprovalRegister(response, objectMapper.convertValue(query, Rpt1861SaleInvoiceApprovalRegisterRequest.class));
    }

    /**
     * Template: 1861A-SaleInvoice_Slip.rpt
     * Procedure: [pcc].[USP_InvSaleInvoice_Slip]
     * Desktop: InvSaleInvoice.SaleInvoiceSlip
     */
    @RequestMapping(value = "/reports/print/1861a-sale-invoice-slip", method = RequestMethod.POST)
    public void print1861ASaleInvoiceSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1861ASaleInvoiceSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1861ASaleInvoiceSlipRequest();
        printReport(response, "1861A-SaleInvoice_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1861a-sale-invoice-slip", method = RequestMethod.GET)
    public void print1861ASaleInvoiceSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1861ASaleInvoiceSlip(response, objectMapper.convertValue(query, Rpt1861ASaleInvoiceSlipRequest.class));
    }

    /**
     * Template: 1862-ItemandWarehouseStockSummary.rpt
     * Procedure: [pcc].[USP_ItemStockReportWithOutValues]
     * Desktop: StocksReport.ItemStockReportWithOutValuesConcrete
     */
    @RequestMapping(value = "/reports/print/1862-itemand-warehouse-stock-summary", method = RequestMethod.POST)
    public void print1862ItemandWarehouseStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1862ItemandWarehouseStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1862ItemandWarehouseStockSummaryRequest();
        printReport(response, "1862-ItemandWarehouseStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1862-itemand-warehouse-stock-summary", method = RequestMethod.GET)
    public void print1862ItemandWarehouseStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1862ItemandWarehouseStockSummary(response, objectMapper.convertValue(query, Rpt1862ItemandWarehouseStockSummaryRequest.class));
    }

    /**
     * Template: 1863-WarehouseAndItemStockSummary.rpt
     * Procedure: [pcc].[USP_ItemStockReportWithOutValues]
     * Desktop: StocksReport.ItemStockReportWithOutValuesConcrete
     */
    @RequestMapping(value = "/reports/print/1863-warehouse-and-item-stock-summary", method = RequestMethod.POST)
    public void print1863WarehouseAndItemStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1863WarehouseAndItemStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1863WarehouseAndItemStockSummaryRequest();
        printReport(response, "1863-WarehouseAndItemStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1863-warehouse-and-item-stock-summary", method = RequestMethod.GET)
    public void print1863WarehouseAndItemStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1863WarehouseAndItemStockSummary(response, objectMapper.convertValue(query, Rpt1863WarehouseAndItemStockSummaryRequest.class));
    }

    /**
     * Template: 1863_01-OrderRegister.rpt
     * Procedure: [pcc].[USP_PurchaseOrderSummaryRegister]
     * Desktop: PurchaseOrder.PurchaseOrderSummaryRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1863-01-order-register", method = RequestMethod.POST)
    public void print186301OrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt186301OrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt186301OrderRegisterRequest();
        printReport(response, "1863_01-OrderRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1863-01-order-register", method = RequestMethod.GET)
    public void print186301OrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print186301OrderRegister(response, objectMapper.convertValue(query, Rpt186301OrderRegisterRequest.class));
    }

    /**
     * Template: 1863_02-OrderSummaryByItem.rpt
     * Procedure: [pcc].[USP_PurchaseOrderSummaryRegister]
     * Desktop: PurchaseOrder.PurchaseOrderSummaryRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1863-02-order-summary-by-item", method = RequestMethod.POST)
    public void print186302OrderSummaryByItem(HttpServletResponse response, @RequestBody(required = false) Rpt186302OrderSummaryByItemRequest request) throws Exception {
        if (request == null) request = new Rpt186302OrderSummaryByItemRequest();
        printReport(response, "1863_02-OrderSummaryByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1863-02-order-summary-by-item", method = RequestMethod.GET)
    public void print186302OrderSummaryByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print186302OrderSummaryByItem(response, objectMapper.convertValue(query, Rpt186302OrderSummaryByItemRequest.class));
    }

    /**
     * Template: 1863_03-OrderSummaryByItem&Varient.rpt
     * Procedure: [pcc].[USP_PurchaseOrderSummaryRegister]
     * Desktop: PurchaseOrder.PurchaseOrderSummaryRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1863-03-order-summary-by-item-varient", method = RequestMethod.POST)
    public void print186303OrderSummaryByItemVarient(HttpServletResponse response, @RequestBody(required = false) Rpt186303OrderSummaryByItemVarientRequest request) throws Exception {
        if (request == null) request = new Rpt186303OrderSummaryByItemVarientRequest();
        printReport(response, "1863_03-OrderSummaryByItem&Varient.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1863-03-order-summary-by-item-varient", method = RequestMethod.GET)
    public void print186303OrderSummaryByItemVarientGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print186303OrderSummaryByItemVarient(response, objectMapper.convertValue(query, Rpt186303OrderSummaryByItemVarientRequest.class));
    }

    /**
     * Template: 1863_04-OrderSummaryByItem&City.rpt
     * Procedure: [pcc].[USP_PurchaseOrderSummaryRegister]
     * Desktop: PurchaseOrder.PurchaseOrderSummaryRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1863-04-order-summary-by-item-city", method = RequestMethod.POST)
    public void print186304OrderSummaryByItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt186304OrderSummaryByItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt186304OrderSummaryByItemCityRequest();
        printReport(response, "1863_04-OrderSummaryByItem&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1863-04-order-summary-by-item-city", method = RequestMethod.GET)
    public void print186304OrderSummaryByItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print186304OrderSummaryByItemCity(response, objectMapper.convertValue(query, Rpt186304OrderSummaryByItemCityRequest.class));
    }

    /**
     * Template: 1863_05-OrderSummaryByItem_Varient&City.rpt
     * Procedure: [pcc].[USP_PurchaseOrderSummaryRegister]
     * Desktop: PurchaseOrder.PurchaseOrderSummaryRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1863-05-order-summary-by-item-varient-city", method = RequestMethod.POST)
    public void print186305OrderSummaryByItemVarientCity(HttpServletResponse response, @RequestBody(required = false) Rpt186305OrderSummaryByItemVarientCityRequest request) throws Exception {
        if (request == null) request = new Rpt186305OrderSummaryByItemVarientCityRequest();
        printReport(response, "1863_05-OrderSummaryByItem_Varient&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1863-05-order-summary-by-item-varient-city", method = RequestMethod.GET)
    public void print186305OrderSummaryByItemVarientCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print186305OrderSummaryByItemVarientCity(response, objectMapper.convertValue(query, Rpt186305OrderSummaryByItemVarientCityRequest.class));
    }

    /**
     * Template: 1863_06-OrderSummaryBySupplier.rpt
     * Procedure: [pcc].[USP_PurchaseOrderSummaryRegister]
     * Desktop: PurchaseOrder.PurchaseOrderSummaryRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1863-06-order-summary-by-supplier", method = RequestMethod.POST)
    public void print186306OrderSummaryBySupplier(HttpServletResponse response, @RequestBody(required = false) Rpt186306OrderSummaryBySupplierRequest request) throws Exception {
        if (request == null) request = new Rpt186306OrderSummaryBySupplierRequest();
        printReport(response, "1863_06-OrderSummaryBySupplier.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1863-06-order-summary-by-supplier", method = RequestMethod.GET)
    public void print186306OrderSummaryBySupplierGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print186306OrderSummaryBySupplier(response, objectMapper.convertValue(query, Rpt186306OrderSummaryBySupplierRequest.class));
    }

    /**
     * Template: 1863_07-OrderSummaryBySupplierItem.rpt
     * Procedure: [pcc].[USP_PurchaseOrderSummaryRegister]
     * Desktop: PurchaseOrder.PurchaseOrderSummaryRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1863-07-order-summary-by-supplier-item", method = RequestMethod.POST)
    public void print186307OrderSummaryBySupplierItem(HttpServletResponse response, @RequestBody(required = false) Rpt186307OrderSummaryBySupplierItemRequest request) throws Exception {
        if (request == null) request = new Rpt186307OrderSummaryBySupplierItemRequest();
        printReport(response, "1863_07-OrderSummaryBySupplierItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1863-07-order-summary-by-supplier-item", method = RequestMethod.GET)
    public void print186307OrderSummaryBySupplierItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print186307OrderSummaryBySupplierItem(response, objectMapper.convertValue(query, Rpt186307OrderSummaryBySupplierItemRequest.class));
    }

    /**
     * Template: 1863_08-OrderSummaryBySupplier&City.rpt
     * Procedure: [pcc].[USP_PurchaseOrderSummaryRegister]
     * Desktop: PurchaseOrder.PurchaseOrderSummaryRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1863-08-order-summary-by-supplier-city", method = RequestMethod.POST)
    public void print186308OrderSummaryBySupplierCity(HttpServletResponse response, @RequestBody(required = false) Rpt186308OrderSummaryBySupplierCityRequest request) throws Exception {
        if (request == null) request = new Rpt186308OrderSummaryBySupplierCityRequest();
        printReport(response, "1863_08-OrderSummaryBySupplier&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1863-08-order-summary-by-supplier-city", method = RequestMethod.GET)
    public void print186308OrderSummaryBySupplierCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print186308OrderSummaryBySupplierCity(response, objectMapper.convertValue(query, Rpt186308OrderSummaryBySupplierCityRequest.class));
    }

    /**
     * Template: 1863_09-OrderSummaryBySupplier_Item&City.rpt
     * Procedure: [pcc].[USP_PurchaseOrderSummaryRegister]
     * Desktop: PurchaseOrder.PurchaseOrderSummaryRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1863-09-order-summary-by-supplier-item-city", method = RequestMethod.POST)
    public void print186309OrderSummaryBySupplierItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt186309OrderSummaryBySupplierItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt186309OrderSummaryBySupplierItemCityRequest();
        printReport(response, "1863_09-OrderSummaryBySupplier_Item&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1863-09-order-summary-by-supplier-item-city", method = RequestMethod.GET)
    public void print186309OrderSummaryBySupplierItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print186309OrderSummaryBySupplierItemCity(response, objectMapper.convertValue(query, Rpt186309OrderSummaryBySupplierItemCityRequest.class));
    }

    /**
     * Template: 1863_10-OrderSummaryBySupplier&Varient.rpt
     * Procedure: [pcc].[USP_PurchaseOrderSummaryRegister]
     * Desktop: PurchaseOrder.PurchaseOrderSummaryRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1863-10-order-summary-by-supplier-varient", method = RequestMethod.POST)
    public void print186310OrderSummaryBySupplierVarient(HttpServletResponse response, @RequestBody(required = false) Rpt186310OrderSummaryBySupplierVarientRequest request) throws Exception {
        if (request == null) request = new Rpt186310OrderSummaryBySupplierVarientRequest();
        printReport(response, "1863_10-OrderSummaryBySupplier&Varient.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1863-10-order-summary-by-supplier-varient", method = RequestMethod.GET)
    public void print186310OrderSummaryBySupplierVarientGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print186310OrderSummaryBySupplierVarient(response, objectMapper.convertValue(query, Rpt186310OrderSummaryBySupplierVarientRequest.class));
    }

    /**
     * Template: 1863_11-OrderSummaryBySupplier_ItemVarient.rpt
     * Procedure: [pcc].[USP_PurchaseOrderSummaryRegister]
     * Desktop: PurchaseOrder.PurchaseOrderSummaryRegisterWithActivities
     */
    @RequestMapping(value = "/reports/print/1863-11-order-summary-by-supplier-item-varient", method = RequestMethod.POST)
    public void print186311OrderSummaryBySupplierItemVarient(HttpServletResponse response, @RequestBody(required = false) Rpt186311OrderSummaryBySupplierItemVarientRequest request) throws Exception {
        if (request == null) request = new Rpt186311OrderSummaryBySupplierItemVarientRequest();
        printReport(response, "1863_11-OrderSummaryBySupplier_ItemVarient.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1863-11-order-summary-by-supplier-item-varient", method = RequestMethod.GET)
    public void print186311OrderSummaryBySupplierItemVarientGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print186311OrderSummaryBySupplierItemVarient(response, objectMapper.convertValue(query, Rpt186311OrderSummaryBySupplierItemVarientRequest.class));
    }

    /**
     * Template: 1863_PurchaseOrderApprovalRegister.rpt
     * Procedure: [pcc].[USP_PurchaseOrderApprovalHistory]
     * Desktop: PurchaseOrder.PurchaseOrderApprovalHistory
     */
    @RequestMapping(value = "/reports/print/1863-purchase-order-approval-register", method = RequestMethod.POST)
    public void print1863PurchaseOrderApprovalRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1863PurchaseOrderApprovalRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1863PurchaseOrderApprovalRegisterRequest();
        printReport(response, "1863_PurchaseOrderApprovalRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1863-purchase-order-approval-register", method = RequestMethod.GET)
    public void print1863PurchaseOrderApprovalRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1863PurchaseOrderApprovalRegister(response, objectMapper.convertValue(query, Rpt1863PurchaseOrderApprovalRegisterRequest.class));
    }

    /**
     * Template: 1863A-PuchaseOrder_Register.rpt
     * Procedure: [pcc].[USP_PurchaseOrder_SlipAndRegister]
     * Desktop: PurchaseOrder.PurchaseOrder_Slip
     */
    @RequestMapping(value = "/reports/print/1863a-puchase-order-register", method = RequestMethod.POST)
    public void print1863APuchaseOrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1863APuchaseOrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1863APuchaseOrderRegisterRequest();
        printReport(response, "1863A-PuchaseOrder_Register.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1863a-puchase-order-register", method = RequestMethod.GET)
    public void print1863APuchaseOrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1863APuchaseOrderRegister(response, objectMapper.convertValue(query, Rpt1863APuchaseOrderRegisterRequest.class));
    }

    /**
     * Template: 1864-GrnRegister.rpt
     * Procedure: [pcc].[USP_InvGrn_SlipAndRegister]
     * Desktop: InvGrn.GrnSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1864-grn-register", method = RequestMethod.POST)
    public void print1864GrnRegister(HttpServletResponse response, @RequestBody(required = false) Rpt1864GrnRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt1864GrnRegisterRequest();
        printReport(response, "1864-GrnRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1864-grn-register", method = RequestMethod.GET)
    public void print1864GrnRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1864GrnRegister(response, objectMapper.convertValue(query, Rpt1864GrnRegisterRequest.class));
    }

    /**
     * Template: 1864-ItemandPackSizeStockSummary.rpt
     * Procedure: [pcc].[USP_ItemStockReportWithOutValues]
     * Desktop: StocksReport.ItemStockReportWithOutValuesConcrete
     */
    @RequestMapping(value = "/reports/print/1864-itemand-pack-size-stock-summary", method = RequestMethod.POST)
    public void print1864ItemandPackSizeStockSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1864ItemandPackSizeStockSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1864ItemandPackSizeStockSummaryRequest();
        printReport(response, "1864-ItemandPackSizeStockSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1864-itemand-pack-size-stock-summary", method = RequestMethod.GET)
    public void print1864ItemandPackSizeStockSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1864ItemandPackSizeStockSummary(response, objectMapper.convertValue(query, Rpt1864ItemandPackSizeStockSummaryRequest.class));
    }

    /**
     * Template: 1864_GoodsRecieptNotesFinish.rpt
     * Procedure: [pcc].[USP_InvGrn_SlipAndRegister]
     * Desktop: InvGrn.GrnSlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1864-goods-reciept-notes-finish", method = RequestMethod.POST)
    public void print1864GoodsRecieptNotesFinish(HttpServletResponse response, @RequestBody(required = false) Rpt1864GoodsRecieptNotesFinishRequest request) throws Exception {
        if (request == null) request = new Rpt1864GoodsRecieptNotesFinishRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1864_GoodsRecieptNotesFinish.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1864-goods-reciept-notes-finish", method = RequestMethod.GET)
    public void print1864GoodsRecieptNotesFinishGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1864GoodsRecieptNotesFinish(response, objectMapper.convertValue(query, Rpt1864GoodsRecieptNotesFinishRequest.class));
    }

    /**
     * Template: 1865_01_SalesWages_SummaryRegister.rpt
     * Procedure: pcc.usp_SalesWages_Register_Summary
     * Desktop: SaleOrder.SaleWagesRegisterSummary
     */
    @RequestMapping(value = "/reports/print/1865-01-sales-wages-summary-register", method = RequestMethod.POST)
    public void print186501SalesWagesSummaryRegister(HttpServletResponse response, @RequestBody(required = false) Rpt186501SalesWagesSummaryRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt186501SalesWagesSummaryRegisterRequest();
        printReport(response, "1865_01_SalesWages_SummaryRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1865-01-sales-wages-summary-register", method = RequestMethod.GET)
    public void print186501SalesWagesSummaryRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print186501SalesWagesSummaryRegister(response, objectMapper.convertValue(query, Rpt186501SalesWagesSummaryRegisterRequest.class));
    }

    /**
     * Template: 1866-InvGdnDirect_Slip.rpt
     * Procedure: [pcc].[USP_InvGdn_Slip]
     * Desktop: InvGdn.GdnConcrete_Slip
     */
    @RequestMapping(value = "/reports/print/1866-inv-gdn-direct-slip", method = RequestMethod.POST)
    public void print1866InvGdnDirectSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1866InvGdnDirectSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1866InvGdnDirectSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1866-InvGdnDirect_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1866-inv-gdn-direct-slip", method = RequestMethod.GET)
    public void print1866InvGdnDirectSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1866InvGdnDirectSlip(response, objectMapper.convertValue(query, Rpt1866InvGdnDirectSlipRequest.class));
    }

    /**
     * Template: 1867_Conversion_Slip.rpt
     * Procedure: [pcc].[USP_Conversion_SlipAndRegister]
     * Desktop: ConversionHeader.Conversion_SlipAndRegister
     */
    @RequestMapping(value = "/reports/print/1867-conversion-slip", method = RequestMethod.POST)
    public void print1867ConversionSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1867ConversionSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1867ConversionSlipRequest();
        printReport(response, "1867_Conversion_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1867-conversion-slip", method = RequestMethod.GET)
    public void print1867ConversionSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1867ConversionSlip(response, objectMapper.convertValue(query, Rpt1867ConversionSlipRequest.class));
    }

    /**
     * Template: 1868_ContractorWagesBillSlip.rpt
     * Procedure: [pcc].[USP_ContractorWagesBill_Slip]
     * Desktop: ContractorWagesBill.ContractorWagesBill_Slip
     */
    @RequestMapping(value = "/reports/print/1868-contractor-wages-bill-slip", method = RequestMethod.POST)
    public void print1868ContractorWagesBillSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1868ContractorWagesBillSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1868ContractorWagesBillSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1868_ContractorWagesBillSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1868-contractor-wages-bill-slip", method = RequestMethod.GET)
    public void print1868ContractorWagesBillSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1868ContractorWagesBillSlip(response, objectMapper.convertValue(query, Rpt1868ContractorWagesBillSlipRequest.class));
    }

    /**
     * Template: 1869_ContractorWagesBillSlip.rpt
     * Procedure: [pcc].[USP_ContractorWagesBill_Slip]
     * Desktop: ContractorWagesBill.ContractorWagesBill_Slip
     */
    @RequestMapping(value = "/reports/print/1869-contractor-wages-bill-slip", method = RequestMethod.POST)
    public void print1869ContractorWagesBillSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1869ContractorWagesBillSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1869ContractorWagesBillSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1869_ContractorWagesBillSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1869-contractor-wages-bill-slip", method = RequestMethod.GET)
    public void print1869ContractorWagesBillSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1869ContractorWagesBillSlip(response, objectMapper.convertValue(query, Rpt1869ContractorWagesBillSlipRequest.class));
    }

    /**
     * Template: 1870_01_WorkOrderDetailReport.rpt
     * Procedure: [pcc].[USP_WorkOrder_Report]
     * Desktop: WorkOrderHeader.WorkOrder_Report
     */
    @RequestMapping(value = "/reports/print/1870-01-work-order-detail-report", method = RequestMethod.POST)
    public void print187001WorkOrderDetailReport(HttpServletResponse response, @RequestBody(required = false) Rpt187001WorkOrderDetailReportRequest request) throws Exception {
        if (request == null) request = new Rpt187001WorkOrderDetailReportRequest();
        printReport(response, "1870_01_WorkOrderDetailReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1870-01-work-order-detail-report", method = RequestMethod.GET)
    public void print187001WorkOrderDetailReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print187001WorkOrderDetailReport(response, objectMapper.convertValue(query, Rpt187001WorkOrderDetailReportRequest.class));
    }

    /**
     * Template: 1870_WorkOrderConcrete_Slip.rpt
     * Procedure: [pcc].[USP_WorkOrderHeader_Slip]
     * Desktop: WorkOrderHeader.WorkOrderHeader_Slip
     */
    @RequestMapping(value = "/reports/print/1870-work-order-concrete-slip", method = RequestMethod.POST)
    public void print1870WorkOrderConcreteSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1870WorkOrderConcreteSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1870WorkOrderConcreteSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1870_WorkOrderConcrete_Slip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1870-work-order-concrete-slip", method = RequestMethod.GET)
    public void print1870WorkOrderConcreteSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1870WorkOrderConcreteSlip(response, objectMapper.convertValue(query, Rpt1870WorkOrderConcreteSlipRequest.class));
    }

    /**
     * Template: 1880-SalesRegisterSummary.rpt
     * Procedure: [pcc].[USP_Sales_EvaulationDetailReports]
     * Desktop: InvSaleInvoice.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/1880-sales-register-summary", method = RequestMethod.POST)
    public void print1880SalesRegisterSummary(HttpServletResponse response, @RequestBody(required = false) Rpt1880SalesRegisterSummaryRequest request) throws Exception {
        if (request == null) request = new Rpt1880SalesRegisterSummaryRequest();
        printReport(response, "1880-SalesRegisterSummary.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1880-sales-register-summary", method = RequestMethod.GET)
    public void print1880SalesRegisterSummaryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1880SalesRegisterSummary(response, objectMapper.convertValue(query, Rpt1880SalesRegisterSummaryRequest.class));
    }

    /**
     * Template: 1881-SalesSummaryByParentCategory.rpt
     * Procedure: [pcc].[USP_Sales_EvaulationDetailReports]
     * Desktop: InvSaleInvoice.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/1881-sales-summary-by-parent-category", method = RequestMethod.POST)
    public void print1881SalesSummaryByParentCategory(HttpServletResponse response, @RequestBody(required = false) Rpt1881SalesSummaryByParentCategoryRequest request) throws Exception {
        if (request == null) request = new Rpt1881SalesSummaryByParentCategoryRequest();
        printReport(response, "1881-SalesSummaryByParentCategory.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1881-sales-summary-by-parent-category", method = RequestMethod.GET)
    public void print1881SalesSummaryByParentCategoryGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1881SalesSummaryByParentCategory(response, objectMapper.convertValue(query, Rpt1881SalesSummaryByParentCategoryRequest.class));
    }

    /**
     * Template: 1882-SalesSummaryByParentCategory&Item.rpt
     * Procedure: [pcc].[USP_Sales_EvaulationDetailReports]
     * Desktop: InvSaleInvoice.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/1882-sales-summary-by-parent-category-item", method = RequestMethod.POST)
    public void print1882SalesSummaryByParentCategoryItem(HttpServletResponse response, @RequestBody(required = false) Rpt1882SalesSummaryByParentCategoryItemRequest request) throws Exception {
        if (request == null) request = new Rpt1882SalesSummaryByParentCategoryItemRequest();
        printReport(response, "1882-SalesSummaryByParentCategory&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1882-sales-summary-by-parent-category-item", method = RequestMethod.GET)
    public void print1882SalesSummaryByParentCategoryItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1882SalesSummaryByParentCategoryItem(response, objectMapper.convertValue(query, Rpt1882SalesSummaryByParentCategoryItemRequest.class));
    }

    /**
     * Template: 1883-SalesSummaryByParentCategory&Customter.rpt
     * Procedure: [pcc].[USP_Sales_EvaulationDetailReports]
     * Desktop: InvSaleInvoice.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/1883-sales-summary-by-parent-category-customter", method = RequestMethod.POST)
    public void print1883SalesSummaryByParentCategoryCustomter(HttpServletResponse response, @RequestBody(required = false) Rpt1883SalesSummaryByParentCategoryCustomterRequest request) throws Exception {
        if (request == null) request = new Rpt1883SalesSummaryByParentCategoryCustomterRequest();
        printReport(response, "1883-SalesSummaryByParentCategory&Customter.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1883-sales-summary-by-parent-category-customter", method = RequestMethod.GET)
    public void print1883SalesSummaryByParentCategoryCustomterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1883SalesSummaryByParentCategoryCustomter(response, objectMapper.convertValue(query, Rpt1883SalesSummaryByParentCategoryCustomterRequest.class));
    }

    /**
     * Template: 1884-SalesSummaryByVehicles.rpt
     * Procedure: [pcc].[USP_Sales_EvaulationDetailReports]
     * Desktop: InvSaleInvoice.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/1884-sales-summary-by-vehicles", method = RequestMethod.POST)
    public void print1884SalesSummaryByVehicles(HttpServletResponse response, @RequestBody(required = false) Rpt1884SalesSummaryByVehiclesRequest request) throws Exception {
        if (request == null) request = new Rpt1884SalesSummaryByVehiclesRequest();
        printReport(response, "1884-SalesSummaryByVehicles.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1884-sales-summary-by-vehicles", method = RequestMethod.GET)
    public void print1884SalesSummaryByVehiclesGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1884SalesSummaryByVehicles(response, objectMapper.convertValue(query, Rpt1884SalesSummaryByVehiclesRequest.class));
    }

    /**
     * Template: 1885-SalesSummaryByCustomerItemandVareient.rpt
     * Procedure: [pcc].[USP_Sales_EvaulationDetailReports]
     * Desktop: InvSaleInvoice.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/1885-sales-summary-by-customer-itemand-vareient", method = RequestMethod.POST)
    public void print1885SalesSummaryByCustomerItemandVareient(HttpServletResponse response, @RequestBody(required = false) Rpt1885SalesSummaryByCustomerItemandVareientRequest request) throws Exception {
        if (request == null) request = new Rpt1885SalesSummaryByCustomerItemandVareientRequest();
        printReport(response, "1885-SalesSummaryByCustomerItemandVareient.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1885-sales-summary-by-customer-itemand-vareient", method = RequestMethod.GET)
    public void print1885SalesSummaryByCustomerItemandVareientGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1885SalesSummaryByCustomerItemandVareient(response, objectMapper.convertValue(query, Rpt1885SalesSummaryByCustomerItemandVareientRequest.class));
    }

    /**
     * Template: 1886-StockReportWithDocumentWise.rpt
     * Procedure: pcc.USP_StockReportWithDocumentWise
     * Desktop: StocksReport.StockReportWithDocumentWise
     */
    @RequestMapping(value = "/reports/print/1886-stock-report-with-document-wise", method = RequestMethod.POST)
    public void print1886StockReportWithDocumentWise(HttpServletResponse response, @RequestBody(required = false) Rpt1886StockReportWithDocumentWiseRequest request) throws Exception {
        if (request == null) request = new Rpt1886StockReportWithDocumentWiseRequest();
        printReport(response, "1886-StockReportWithDocumentWise.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1886-stock-report-with-document-wise", method = RequestMethod.GET)
    public void print1886StockReportWithDocumentWiseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1886StockReportWithDocumentWise(response, objectMapper.convertValue(query, Rpt1886StockReportWithDocumentWiseRequest.class));
    }

    /**
     * Template: 1886_01_StockReportWithDocumentWiseIncludeOrders.rpt
     * Procedure: pcc.USP_StockReportWithDocumentWiseIncludeOrders
     * Desktop: StocksReport.StockReportWithDocumentWiseIncludeOrders
     */
    @RequestMapping(value = "/reports/print/1886-01-stock-report-with-document-wise-include-orders", method = RequestMethod.POST)
    public void print188601StockReportWithDocumentWiseIncludeOrders(HttpServletResponse response, @RequestBody(required = false) Rpt188601StockReportWithDocumentWiseIncludeOrdersRequest request) throws Exception {
        if (request == null) request = new Rpt188601StockReportWithDocumentWiseIncludeOrdersRequest();
        printReport(response, "1886_01_StockReportWithDocumentWiseIncludeOrders.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1886-01-stock-report-with-document-wise-include-orders", method = RequestMethod.GET)
    public void print188601StockReportWithDocumentWiseIncludeOrdersGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print188601StockReportWithDocumentWiseIncludeOrders(response, objectMapper.convertValue(query, Rpt188601StockReportWithDocumentWiseIncludeOrdersRequest.class));
    }

    /**
     * Template: 1887-GetReportByFiFo.rpt
     * Procedure: [pcc].[USP_GetStockByFifo_Report]
     * Desktop: InvStockTransferHeader.StockByFiFo
     */
    @RequestMapping(value = "/reports/print/1887-get-report-by-fi-fo", method = RequestMethod.POST)
    public void print1887GetReportByFiFo(HttpServletResponse response, @RequestBody(required = false) Rpt1887GetReportByFiFoRequest request) throws Exception {
        if (request == null) request = new Rpt1887GetReportByFiFoRequest();
        printReport(response, "1887-GetReportByFiFo.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1887-get-report-by-fi-fo", method = RequestMethod.GET)
    public void print1887GetReportByFiFoGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1887GetReportByFiFo(response, objectMapper.convertValue(query, Rpt1887GetReportByFiFoRequest.class));
    }

    /**
     * Template: 1889-SalesRegisterSummaryByItem.rpt
     * Procedure: [pcc].[USP_Sales_EvaulationDetailReports]
     * Desktop: InvSaleInvoice.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/1889-sales-register-summary-by-item", method = RequestMethod.POST)
    public void print1889SalesRegisterSummaryByItem(HttpServletResponse response, @RequestBody(required = false) Rpt1889SalesRegisterSummaryByItemRequest request) throws Exception {
        if (request == null) request = new Rpt1889SalesRegisterSummaryByItemRequest();
        printReport(response, "1889-SalesRegisterSummaryByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1889-sales-register-summary-by-item", method = RequestMethod.GET)
    public void print1889SalesRegisterSummaryByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1889SalesRegisterSummaryByItem(response, objectMapper.convertValue(query, Rpt1889SalesRegisterSummaryByItemRequest.class));
    }

    /**
     * Template: 1890-SalesSummaryByItem&City.rpt
     * Procedure: [pcc].[USP_Sales_EvaulationDetailReports]
     * Desktop: InvSaleInvoice.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/1890-sales-summary-by-item-city", method = RequestMethod.POST)
    public void print1890SalesSummaryByItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt1890SalesSummaryByItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt1890SalesSummaryByItemCityRequest();
        printReport(response, "1890-SalesSummaryByItem&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1890-sales-summary-by-item-city", method = RequestMethod.GET)
    public void print1890SalesSummaryByItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1890SalesSummaryByItemCity(response, objectMapper.convertValue(query, Rpt1890SalesSummaryByItemCityRequest.class));
    }

    /**
     * Template: 1891-SalesRegisterSummaryByItem&Varient.rpt
     * Procedure: [pcc].[USP_Sales_EvaulationDetailReports]
     * Desktop: InvSaleInvoice.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/1891-sales-register-summary-by-item-varient", method = RequestMethod.POST)
    public void print1891SalesRegisterSummaryByItemVarient(HttpServletResponse response, @RequestBody(required = false) Rpt1891SalesRegisterSummaryByItemVarientRequest request) throws Exception {
        if (request == null) request = new Rpt1891SalesRegisterSummaryByItemVarientRequest();
        printReport(response, "1891-SalesRegisterSummaryByItem&Varient.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1891-sales-register-summary-by-item-varient", method = RequestMethod.GET)
    public void print1891SalesRegisterSummaryByItemVarientGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1891SalesRegisterSummaryByItemVarient(response, objectMapper.convertValue(query, Rpt1891SalesRegisterSummaryByItemVarientRequest.class));
    }

    /**
     * Template: 1892-SalesRegisterSummaryByItem&Warehouse.rpt
     * Procedure: [pcc].[USP_Sales_EvaulationDetailReports]
     * Desktop: InvSaleInvoice.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/1892-sales-register-summary-by-item-warehouse", method = RequestMethod.POST)
    public void print1892SalesRegisterSummaryByItemWarehouse(HttpServletResponse response, @RequestBody(required = false) Rpt1892SalesRegisterSummaryByItemWarehouseRequest request) throws Exception {
        if (request == null) request = new Rpt1892SalesRegisterSummaryByItemWarehouseRequest();
        printReport(response, "1892-SalesRegisterSummaryByItem&Warehouse.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1892-sales-register-summary-by-item-warehouse", method = RequestMethod.GET)
    public void print1892SalesRegisterSummaryByItemWarehouseGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1892SalesRegisterSummaryByItemWarehouse(response, objectMapper.convertValue(query, Rpt1892SalesRegisterSummaryByItemWarehouseRequest.class));
    }

    /**
     * Template: 1893-SalesSummaryByItemPackSize&City.rpt
     * Procedure: [pcc].[USP_Sales_EvaulationDetailReports]
     * Desktop: InvSaleInvoice.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/1893-sales-summary-by-item-pack-size-city", method = RequestMethod.POST)
    public void print1893SalesSummaryByItemPackSizeCity(HttpServletResponse response, @RequestBody(required = false) Rpt1893SalesSummaryByItemPackSizeCityRequest request) throws Exception {
        if (request == null) request = new Rpt1893SalesSummaryByItemPackSizeCityRequest();
        printReport(response, "1893-SalesSummaryByItemPackSize&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1893-sales-summary-by-item-pack-size-city", method = RequestMethod.GET)
    public void print1893SalesSummaryByItemPackSizeCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1893SalesSummaryByItemPackSizeCity(response, objectMapper.convertValue(query, Rpt1893SalesSummaryByItemPackSizeCityRequest.class));
    }

    /**
     * Template: 1894-SalesRegisterSummaryByCustomer.rpt
     * Procedure: [pcc].[USP_Sales_EvaulationDetailReports]
     * Desktop: InvSaleInvoice.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/1894-sales-register-summary-by-customer", method = RequestMethod.POST)
    public void print1894SalesRegisterSummaryByCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt1894SalesRegisterSummaryByCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt1894SalesRegisterSummaryByCustomerRequest();
        printReport(response, "1894-SalesRegisterSummaryByCustomer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1894-sales-register-summary-by-customer", method = RequestMethod.GET)
    public void print1894SalesRegisterSummaryByCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1894SalesRegisterSummaryByCustomer(response, objectMapper.convertValue(query, Rpt1894SalesRegisterSummaryByCustomerRequest.class));
    }

    /**
     * Template: 1895-SalesSummaryByCustomer&Item.rpt
     * Procedure: [pcc].[USP_Sales_EvaulationDetailReports]
     * Desktop: InvSaleInvoice.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/1895-sales-summary-by-customer-item", method = RequestMethod.POST)
    public void print1895SalesSummaryByCustomerItem(HttpServletResponse response, @RequestBody(required = false) Rpt1895SalesSummaryByCustomerItemRequest request) throws Exception {
        if (request == null) request = new Rpt1895SalesSummaryByCustomerItemRequest();
        printReport(response, "1895-SalesSummaryByCustomer&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1895-sales-summary-by-customer-item", method = RequestMethod.GET)
    public void print1895SalesSummaryByCustomerItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1895SalesSummaryByCustomerItem(response, objectMapper.convertValue(query, Rpt1895SalesSummaryByCustomerItemRequest.class));
    }

    /**
     * Template: 1896-SalesRegisterSummaryByCustomer&City.rpt
     * Procedure: [pcc].[USP_Sales_EvaulationDetailReports]
     * Desktop: InvSaleInvoice.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/1896-sales-register-summary-by-customer-city", method = RequestMethod.POST)
    public void print1896SalesRegisterSummaryByCustomerCity(HttpServletResponse response, @RequestBody(required = false) Rpt1896SalesRegisterSummaryByCustomerCityRequest request) throws Exception {
        if (request == null) request = new Rpt1896SalesRegisterSummaryByCustomerCityRequest();
        printReport(response, "1896-SalesRegisterSummaryByCustomer&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1896-sales-register-summary-by-customer-city", method = RequestMethod.GET)
    public void print1896SalesRegisterSummaryByCustomerCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1896SalesRegisterSummaryByCustomerCity(response, objectMapper.convertValue(query, Rpt1896SalesRegisterSummaryByCustomerCityRequest.class));
    }

    /**
     * Template: 1897-SalesSummaryByCustomer&Varient.rpt
     * Procedure: [pcc].[USP_Sales_EvaulationDetailReports]
     * Desktop: InvSaleInvoice.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/1897-sales-summary-by-customer-varient", method = RequestMethod.POST)
    public void print1897SalesSummaryByCustomerVarient(HttpServletResponse response, @RequestBody(required = false) Rpt1897SalesSummaryByCustomerVarientRequest request) throws Exception {
        if (request == null) request = new Rpt1897SalesSummaryByCustomerVarientRequest();
        printReport(response, "1897-SalesSummaryByCustomer&Varient.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1897-sales-summary-by-customer-varient", method = RequestMethod.GET)
    public void print1897SalesSummaryByCustomerVarientGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1897SalesSummaryByCustomerVarient(response, objectMapper.convertValue(query, Rpt1897SalesSummaryByCustomerVarientRequest.class));
    }

    /**
     * Template: 1898-SalesSummaryByCustomerItem&City.rpt
     * Procedure: [pcc].[USP_Sales_EvaulationDetailReports]
     * Desktop: InvSaleInvoice.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/1898-sales-summary-by-customer-item-city", method = RequestMethod.POST)
    public void print1898SalesSummaryByCustomerItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt1898SalesSummaryByCustomerItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt1898SalesSummaryByCustomerItemCityRequest();
        printReport(response, "1898-SalesSummaryByCustomerItem&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1898-sales-summary-by-customer-item-city", method = RequestMethod.GET)
    public void print1898SalesSummaryByCustomerItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1898SalesSummaryByCustomerItemCity(response, objectMapper.convertValue(query, Rpt1898SalesSummaryByCustomerItemCityRequest.class));
    }

    /**
     * Template: 1899-SalesSummaryByCity.rpt
     * Procedure: [pcc].[USP_Sales_EvaulationDetailReports]
     * Desktop: InvSaleInvoice.EvaulationDetailSalesReports
     */
    @RequestMapping(value = "/reports/print/1899-sales-summary-by-city", method = RequestMethod.POST)
    public void print1899SalesSummaryByCity(HttpServletResponse response, @RequestBody(required = false) Rpt1899SalesSummaryByCityRequest request) throws Exception {
        if (request == null) request = new Rpt1899SalesSummaryByCityRequest();
        printReport(response, "1899-SalesSummaryByCity.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1899-sales-summary-by-city", method = RequestMethod.GET)
    public void print1899SalesSummaryByCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1899SalesSummaryByCity(response, objectMapper.convertValue(query, Rpt1899SalesSummaryByCityRequest.class));
    }

    /**
     * Template: 347-OrderRegister.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/347-order-register", method = RequestMethod.POST)
    public void print347OrderRegister(HttpServletResponse response, @RequestBody(required = false) Rpt347OrderRegisterRequest request) throws Exception {
        if (request == null) request = new Rpt347OrderRegisterRequest();
        printReport(response, "347-OrderRegister.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/347-order-register", method = RequestMethod.GET)
    public void print347OrderRegisterGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print347OrderRegister(response, objectMapper.convertValue(query, Rpt347OrderRegisterRequest.class));
    }

    /**
     * Template: 348-OrderSummaryByItem&PackSize.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/348-order-summary-by-item-pack-size", method = RequestMethod.POST)
    public void print348OrderSummaryByItemPackSize(HttpServletResponse response, @RequestBody(required = false) Rpt348OrderSummaryByItemPackSizeRequest request) throws Exception {
        if (request == null) request = new Rpt348OrderSummaryByItemPackSizeRequest();
        printReport(response, "348-OrderSummaryByItem&PackSize.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/348-order-summary-by-item-pack-size", method = RequestMethod.GET)
    public void print348OrderSummaryByItemPackSizeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print348OrderSummaryByItemPackSize(response, objectMapper.convertValue(query, Rpt348OrderSummaryByItemPackSizeRequest.class));
    }

    /**
     * Template: 349-OrderSummaryByItem,PackSize&City.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/349-order-summary-by-item-pack-size-city", method = RequestMethod.POST)
    public void print349OrderSummaryByItemPackSizeCity(HttpServletResponse response, @RequestBody(required = false) Rpt349OrderSummaryByItemPackSizeCityRequest request) throws Exception {
        if (request == null) request = new Rpt349OrderSummaryByItemPackSizeCityRequest();
        printReport(response, "349-OrderSummaryByItem,PackSize&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/349-order-summary-by-item-pack-size-city", method = RequestMethod.GET)
    public void print349OrderSummaryByItemPackSizeCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print349OrderSummaryByItemPackSizeCity(response, objectMapper.convertValue(query, Rpt349OrderSummaryByItemPackSizeCityRequest.class));
    }

    /**
     * Template: 350-OrderSummaryByCustomer&PackSize.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/350-order-summary-by-customer-pack-size", method = RequestMethod.POST)
    public void print350OrderSummaryByCustomerPackSize(HttpServletResponse response, @RequestBody(required = false) Rpt350OrderSummaryByCustomerPackSizeRequest request) throws Exception {
        if (request == null) request = new Rpt350OrderSummaryByCustomerPackSizeRequest();
        printReport(response, "350-OrderSummaryByCustomer&PackSize.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/350-order-summary-by-customer-pack-size", method = RequestMethod.GET)
    public void print350OrderSummaryByCustomerPackSizeGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print350OrderSummaryByCustomerPackSize(response, objectMapper.convertValue(query, Rpt350OrderSummaryByCustomerPackSizeRequest.class));
    }

    /**
     * Template: 351-OrderSummaryByItem.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/351-order-summary-by-item", method = RequestMethod.POST)
    public void print351OrderSummaryByItem(HttpServletResponse response, @RequestBody(required = false) Rpt351OrderSummaryByItemRequest request) throws Exception {
        if (request == null) request = new Rpt351OrderSummaryByItemRequest();
        printReport(response, "351-OrderSummaryByItem.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/351-order-summary-by-item", method = RequestMethod.GET)
    public void print351OrderSummaryByItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print351OrderSummaryByItem(response, objectMapper.convertValue(query, Rpt351OrderSummaryByItemRequest.class));
    }

    /**
     * Template: 352-OrderSummaryByItem&City.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/352-order-summary-by-item-city", method = RequestMethod.POST)
    public void print352OrderSummaryByItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt352OrderSummaryByItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt352OrderSummaryByItemCityRequest();
        printReport(response, "352-OrderSummaryByItem&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/352-order-summary-by-item-city", method = RequestMethod.GET)
    public void print352OrderSummaryByItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print352OrderSummaryByItemCity(response, objectMapper.convertValue(query, Rpt352OrderSummaryByItemCityRequest.class));
    }

    /**
     * Template: 353-OrderSummaryByCustomer.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/353-order-summary-by-customer", method = RequestMethod.POST)
    public void print353OrderSummaryByCustomer(HttpServletResponse response, @RequestBody(required = false) Rpt353OrderSummaryByCustomerRequest request) throws Exception {
        if (request == null) request = new Rpt353OrderSummaryByCustomerRequest();
        printReport(response, "353-OrderSummaryByCustomer.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/353-order-summary-by-customer", method = RequestMethod.GET)
    public void print353OrderSummaryByCustomerGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print353OrderSummaryByCustomer(response, objectMapper.convertValue(query, Rpt353OrderSummaryByCustomerRequest.class));
    }

    /**
     * Template: 354-OrderSummaryByCustomer&City.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/354-order-summary-by-customer-city", method = RequestMethod.POST)
    public void print354OrderSummaryByCustomerCity(HttpServletResponse response, @RequestBody(required = false) Rpt354OrderSummaryByCustomerCityRequest request) throws Exception {
        if (request == null) request = new Rpt354OrderSummaryByCustomerCityRequest();
        printReport(response, "354-OrderSummaryByCustomer&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/354-order-summary-by-customer-city", method = RequestMethod.GET)
    public void print354OrderSummaryByCustomerCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print354OrderSummaryByCustomerCity(response, objectMapper.convertValue(query, Rpt354OrderSummaryByCustomerCityRequest.class));
    }

    /**
     * Template: 355-OrderSummaryByCustomer&Item.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/355-order-summary-by-customer-item", method = RequestMethod.POST)
    public void print355OrderSummaryByCustomerItem(HttpServletResponse response, @RequestBody(required = false) Rpt355OrderSummaryByCustomerItemRequest request) throws Exception {
        if (request == null) request = new Rpt355OrderSummaryByCustomerItemRequest();
        printReport(response, "355-OrderSummaryByCustomer&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/355-order-summary-by-customer-item", method = RequestMethod.GET)
    public void print355OrderSummaryByCustomerItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print355OrderSummaryByCustomerItem(response, objectMapper.convertValue(query, Rpt355OrderSummaryByCustomerItemRequest.class));
    }

    /**
     * Template: 356-OrderSummaryByCustomer,Item&City.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/356-order-summary-by-customer-item-city", method = RequestMethod.POST)
    public void print356OrderSummaryByCustomerItemCity(HttpServletResponse response, @RequestBody(required = false) Rpt356OrderSummaryByCustomerItemCityRequest request) throws Exception {
        if (request == null) request = new Rpt356OrderSummaryByCustomerItemCityRequest();
        printReport(response, "356-OrderSummaryByCustomer,Item&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/356-order-summary-by-customer-item-city", method = RequestMethod.GET)
    public void print356OrderSummaryByCustomerItemCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print356OrderSummaryByCustomerItemCity(response, objectMapper.convertValue(query, Rpt356OrderSummaryByCustomerItemCityRequest.class));
    }

    /**
     * Template: 357-OrderSummaryByCustomerandReferenceParty.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/357-order-summary-by-customerand-reference-party", method = RequestMethod.POST)
    public void print357OrderSummaryByCustomerandReferenceParty(HttpServletResponse response, @RequestBody(required = false) Rpt357OrderSummaryByCustomerandReferencePartyRequest request) throws Exception {
        if (request == null) request = new Rpt357OrderSummaryByCustomerandReferencePartyRequest();
        printReport(response, "357-OrderSummaryByCustomerandReferenceParty.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/357-order-summary-by-customerand-reference-party", method = RequestMethod.GET)
    public void print357OrderSummaryByCustomerandReferencePartyGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print357OrderSummaryByCustomerandReferenceParty(response, objectMapper.convertValue(query, Rpt357OrderSummaryByCustomerandReferencePartyRequest.class));
    }

    /**
     * Template: 358-OrderSummaryByCustomer&Item&ReferenceParty.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/358-order-summary-by-customer-item-reference-party", method = RequestMethod.POST)
    public void print358OrderSummaryByCustomerItemReferenceParty(HttpServletResponse response, @RequestBody(required = false) Rpt358OrderSummaryByCustomerItemReferencePartyRequest request) throws Exception {
        if (request == null) request = new Rpt358OrderSummaryByCustomerItemReferencePartyRequest();
        printReport(response, "358-OrderSummaryByCustomer&Item&ReferenceParty.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/358-order-summary-by-customer-item-reference-party", method = RequestMethod.GET)
    public void print358OrderSummaryByCustomerItemReferencePartyGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print358OrderSummaryByCustomerItemReferenceParty(response, objectMapper.convertValue(query, Rpt358OrderSummaryByCustomerItemReferencePartyRequest.class));
    }

    /**
     * Template: 359-OrderSummaryByReferenceParty.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/359-order-summary-by-reference-party", method = RequestMethod.POST)
    public void print359OrderSummaryByReferenceParty(HttpServletResponse response, @RequestBody(required = false) Rpt359OrderSummaryByReferencePartyRequest request) throws Exception {
        if (request == null) request = new Rpt359OrderSummaryByReferencePartyRequest();
        printReport(response, "359-OrderSummaryByReferenceParty.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/359-order-summary-by-reference-party", method = RequestMethod.GET)
    public void print359OrderSummaryByReferencePartyGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print359OrderSummaryByReferenceParty(response, objectMapper.convertValue(query, Rpt359OrderSummaryByReferencePartyRequest.class));
    }

    /**
     * Template: 360-OrderSummaryByReferenceParty&City.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/360-order-summary-by-reference-party-city", method = RequestMethod.POST)
    public void print360OrderSummaryByReferencePartyCity(HttpServletResponse response, @RequestBody(required = false) Rpt360OrderSummaryByReferencePartyCityRequest request) throws Exception {
        if (request == null) request = new Rpt360OrderSummaryByReferencePartyCityRequest();
        printReport(response, "360-OrderSummaryByReferenceParty&City.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/360-order-summary-by-reference-party-city", method = RequestMethod.GET)
    public void print360OrderSummaryByReferencePartyCityGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print360OrderSummaryByReferencePartyCity(response, objectMapper.convertValue(query, Rpt360OrderSummaryByReferencePartyCityRequest.class));
    }

    /**
     * Template: 361-OrderSummaryByReferenceParty&Item.rpt
     * Procedure: [pcc].[USP_SaleOrderSummaryRegister]
     * Desktop: SaleOrder.SaleOrderSummaryRegister
     */
    @RequestMapping(value = "/reports/print/361-order-summary-by-reference-party-item", method = RequestMethod.POST)
    public void print361OrderSummaryByReferencePartyItem(HttpServletResponse response, @RequestBody(required = false) Rpt361OrderSummaryByReferencePartyItemRequest request) throws Exception {
        if (request == null) request = new Rpt361OrderSummaryByReferencePartyItemRequest();
        printReport(response, "361-OrderSummaryByReferenceParty&Item.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/361-order-summary-by-reference-party-item", method = RequestMethod.GET)
    public void print361OrderSummaryByReferencePartyItemGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print361OrderSummaryByReferencePartyItem(response, objectMapper.convertValue(query, Rpt361OrderSummaryByReferencePartyItemRequest.class));
    }

    /**
     * Template: BillOfMaterialAllDetails_SubReport.rpt
     * Procedure: [pcc].[USP_BillOfMaterialAllDetails_SubReport]
     * Desktop: BomHeader.BillOfMaterialAllDetails_SubReport
     */
    @RequestMapping(value = "/reports/print/bill-of-material-all-details-sub-report", method = RequestMethod.POST)
    public void printBillOfMaterialAllDetailsSubReport(HttpServletResponse response, @RequestBody(required = false) RptBillOfMaterialAllDetailsSubReportRequest request) throws Exception {
        if (request == null) request = new RptBillOfMaterialAllDetailsSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "BillOfMaterialAllDetails_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/bill-of-material-all-details-sub-report", method = RequestMethod.GET)
    public void printBillOfMaterialAllDetailsSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printBillOfMaterialAllDetailsSubReport(response, objectMapper.convertValue(query, RptBillOfMaterialAllDetailsSubReportRequest.class));
    }

    /**
     * Template: BillOfMaterialDetails_SubReport.rpt
     * Procedure: [pcc].[USP_WorkOrder_BomDetail_Report]
     * Desktop: BomHeader.BOM_DetailReport
     */
    @RequestMapping(value = "/reports/print/bill-of-material-details-sub-report", method = RequestMethod.POST)
    public void printBillOfMaterialDetailsSubReport(HttpServletResponse response, @RequestBody(required = false) RptBillOfMaterialDetailsSubReportRequest request) throws Exception {
        if (request == null) request = new RptBillOfMaterialDetailsSubReportRequest();
        printReport(response, "BillOfMaterialDetails_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/bill-of-material-details-sub-report", method = RequestMethod.GET)
    public void printBillOfMaterialDetailsSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printBillOfMaterialDetailsSubReport(response, objectMapper.convertValue(query, RptBillOfMaterialDetailsSubReportRequest.class));
    }

    /**
     * Template: ConversionAllDetails_SubReport.rpt
     * Procedure: [pcc].[USP_ConversionAllDetails_SubReport]
     * Desktop: ConversionHeader.ConversionAllDetails_SubReport
     */
    @RequestMapping(value = "/reports/print/conversion-all-details-sub-report", method = RequestMethod.POST)
    public void printConversionAllDetailsSubReport(HttpServletResponse response, @RequestBody(required = false) RptConversionAllDetailsSubReportRequest request) throws Exception {
        if (request == null) request = new RptConversionAllDetailsSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "ConversionAllDetails_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/conversion-all-details-sub-report", method = RequestMethod.GET)
    public void printConversionAllDetailsSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printConversionAllDetailsSubReport(response, objectMapper.convertValue(query, RptConversionAllDetailsSubReportRequest.class));
    }

    /**
     * Template: InvGdn_SubReport.rpt
     * Procedure: [pcc].[USP_InvGdnWagesDetail_SubReport]
     * Desktop: InvGdn.GdnWagesDetail_SubReport
     */
    @RequestMapping(value = "/reports/print/inv-gdn-sub-report", method = RequestMethod.POST)
    public void printInvGdnSubReport(HttpServletResponse response, @RequestBody(required = false) RptInvGdnSubReportRequest request) throws Exception {
        if (request == null) request = new RptInvGdnSubReportRequest();
        printReport(response, "InvGdn_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/inv-gdn-sub-report", method = RequestMethod.GET)
    public void printInvGdnSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printInvGdnSubReport(response, objectMapper.convertValue(query, RptInvGdnSubReportRequest.class));
    }

    /**
     * Template: InvPurchaseInvoice_SupplierBillExpense_SubReport.rpt
     * Procedure: [pcc].[USP_InvPurchaseInvoice_SupplierBillExpense_SubReport]
     * Desktop: InvPurchaseInvoice.PurchaseSupplierBillSubReport
     */
    @RequestMapping(value = "/reports/print/inv-purchase-invoice-supplier-bill-expense-sub-report", method = RequestMethod.POST)
    public void printInvPurchaseInvoiceSupplierBillExpenseSubReport(HttpServletResponse response, @RequestBody(required = false) RptInvPurchaseInvoiceSupplierBillExpenseSubReportRequest request) throws Exception {
        if (request == null) request = new RptInvPurchaseInvoiceSupplierBillExpenseSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "InvPurchaseInvoice_SupplierBillExpense_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/inv-purchase-invoice-supplier-bill-expense-sub-report", method = RequestMethod.GET)
    public void printInvPurchaseInvoiceSupplierBillExpenseSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printInvPurchaseInvoiceSupplierBillExpenseSubReport(response, objectMapper.convertValue(query, RptInvPurchaseInvoiceSupplierBillExpenseSubReportRequest.class));
    }

    /**
     * Template: ProductionAllDetails_SubReport.rpt
     * Procedure: [pcc].[USP_ProductionAllDetails_SubReport]
     * Desktop: ProductionHeader.ProductionAllDetails_SubReport
     */
    @RequestMapping(value = "/reports/print/production-all-details-sub-report", method = RequestMethod.POST)
    public void printProductionAllDetailsSubReport(HttpServletResponse response, @RequestBody(required = false) RptProductionAllDetailsSubReportRequest request) throws Exception {
        if (request == null) request = new RptProductionAllDetailsSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "ProductionAllDetails_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/production-all-details-sub-report", method = RequestMethod.GET)
    public void printProductionAllDetailsSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printProductionAllDetailsSubReport(response, objectMapper.convertValue(query, RptProductionAllDetailsSubReportRequest.class));
    }

    /**
     * Template: SaleInvoice_ItemExpenseSubReport.rpt
     * Procedure: [pcc].[USP_InvSaleInvoiceItemExpense_SubReport]
     * Desktop: InvSaleInvoice.SaleInvoiceItemExpenseSubReport
     */
    @RequestMapping(value = "/reports/print/sale-invoice-item-expense-sub-report", method = RequestMethod.POST)
    public void printSaleInvoiceItemExpenseSubReport(HttpServletResponse response, @RequestBody(required = false) RptSaleInvoiceItemExpenseSubReportRequest request) throws Exception {
        if (request == null) request = new RptSaleInvoiceItemExpenseSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "SaleInvoice_ItemExpenseSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/sale-invoice-item-expense-sub-report", method = RequestMethod.GET)
    public void printSaleInvoiceItemExpenseSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printSaleInvoiceItemExpenseSubReport(response, objectMapper.convertValue(query, RptSaleInvoiceItemExpenseSubReportRequest.class));
    }

    /**
     * Template: SaleInvoice_SubReport.rpt
     * Procedure: pcc.USP_InvSaleInvoice_SubReport
     * Desktop: InvSaleInvoice.SalesCustomerBillSubReport
     */
    @RequestMapping(value = "/reports/print/sale-invoice-sub-report", method = RequestMethod.POST)
    public void printSaleInvoiceSubReport(HttpServletResponse response, @RequestBody(required = false) RptSaleInvoiceSubReportRequest request) throws Exception {
        if (request == null) request = new RptSaleInvoiceSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "SaleInvoice_SubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/sale-invoice-sub-report", method = RequestMethod.GET)
    public void printSaleInvoiceSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printSaleInvoiceSubReport(response, objectMapper.convertValue(query, RptSaleInvoiceSubReportRequest.class));
    }

    /**
     * Template: WorkOrder_InputSubReport.rpt
     * Procedure: [pcc].[USP_WorkOrder_InputSubReport]
     * Desktop: WorkOrderHeader.WorkOrder_InputSubReport
     */
    @RequestMapping(value = "/reports/print/work-order-input-sub-report", method = RequestMethod.POST)
    public void printWorkOrderInputSubReport(HttpServletResponse response, @RequestBody(required = false) RptWorkOrderInputSubReportRequest request) throws Exception {
        if (request == null) request = new RptWorkOrderInputSubReportRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "WorkOrder_InputSubReport.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/work-order-input-sub-report", method = RequestMethod.GET)
    public void printWorkOrderInputSubReportGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        printWorkOrderInputSubReport(response, objectMapper.convertValue(query, RptWorkOrderInputSubReportRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
