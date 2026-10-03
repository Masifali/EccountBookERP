package com.mst.controllers.ERPPrint;

import com.mst.controllers.ERPPrint.requests.CommissionAgentPrintRequests.*;
import com.mst.reports.prints.ReportPdfService;
import java.util.Map;
import javax.servlet.http.HttpServletResponse;
import org.springframework.web.bind.annotation.*;
import org.springframework.stereotype.Controller;

/** CommissionAgent print actions. Generated from the verified seeder contracts. */
@Controller
public class CommissionAgentPrintController extends ReportPrintSupport {
    // BEGIN GENERATED PRINT ACTIONS

    /**
     * Template: 1050_BuyerInquiryBookingSlip.rpt
     * Procedure: [cmagt].[USP_inquiryBookingMaster_Slip]
     * Desktop: InquiryBookingMaster.BuyerInquiryBooking_Slip
     */
    @RequestMapping(value = "/reports/print/1050-buyer-inquiry-booking-slip", method = RequestMethod.POST)
    public void print1050BuyerInquiryBookingSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1050BuyerInquiryBookingSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1050BuyerInquiryBookingSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1050_BuyerInquiryBookingSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1050-buyer-inquiry-booking-slip", method = RequestMethod.GET)
    public void print1050BuyerInquiryBookingSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1050BuyerInquiryBookingSlip(response, objectMapper.convertValue(query, Rpt1050BuyerInquiryBookingSlipRequest.class));
    }

    /**
     * Template: 1055_GdnBuyerDispatchSlip.rpt
     * Procedure: [cmagt].[USP_gdnBuyerDispatchMaster_Slip]
     * Desktop: gdnBuyerDispatchMaster.gdnBuyerDispatchMaster_Slip
     */
    @RequestMapping(value = "/reports/print/1055-gdn-buyer-dispatch-slip", method = RequestMethod.POST)
    public void print1055GdnBuyerDispatchSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1055GdnBuyerDispatchSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1055GdnBuyerDispatchSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1055_GdnBuyerDispatchSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1055-gdn-buyer-dispatch-slip", method = RequestMethod.GET)
    public void print1055GdnBuyerDispatchSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1055GdnBuyerDispatchSlip(response, objectMapper.convertValue(query, Rpt1055GdnBuyerDispatchSlipRequest.class));
    }

    /**
     * Template: 1054_GrnSupplierLoadingSlip.rpt
     * Procedure: [cmagt].[usp_grnSupplierLoadingMaster_Slip]
     * Desktop: grnSupplierLoadingMaster.grnSupplierLoading_Slip
     */
    @RequestMapping(value = "/reports/print/1054-grn-supplier-loading-slip", method = RequestMethod.POST)
    public void print1054GrnSupplierLoadingSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1054GrnSupplierLoadingSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1054GrnSupplierLoadingSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1054_GrnSupplierLoadingSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1054-grn-supplier-loading-slip", method = RequestMethod.GET)
    public void print1054GrnSupplierLoadingSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1054GrnSupplierLoadingSlip(response, objectMapper.convertValue(query, Rpt1054GrnSupplierLoadingSlipRequest.class));
    }

    /**
     * Template: 1054_01_GrnSupplierLoadingChallanSlip.rpt
     * Procedure: [cmagt].[usp_grnSupplierLoadingMaster_Slip]
     * Desktop: grnSupplierLoadingMaster.grnSupplierLoading_Slip
     */
    @RequestMapping(value = "/reports/print/1054-01-grn-supplier-loading-challan-slip", method = RequestMethod.POST)
    public void print105401GrnSupplierLoadingChallanSlip(HttpServletResponse response, @RequestBody(required = false) Rpt105401GrnSupplierLoadingChallanSlipRequest request) throws Exception {
        if (request == null) request = new Rpt105401GrnSupplierLoadingChallanSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1054_01_GrnSupplierLoadingChallanSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1054-01-grn-supplier-loading-challan-slip", method = RequestMethod.GET)
    public void print105401GrnSupplierLoadingChallanSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print105401GrnSupplierLoadingChallanSlip(response, objectMapper.convertValue(query, Rpt105401GrnSupplierLoadingChallanSlipRequest.class));
    }

    /**
     * Template: 1052_PurchaseOrderSlip.rpt
     * Procedure: [cmagt].[USP_purchaseOrderMaster_Slip]
     * Desktop: purchaseOrderMaster.purchaseOrder_Slip
     */
    @RequestMapping(value = "/reports/print/1052-purchase-order-slip", method = RequestMethod.POST)
    public void print1052PurchaseOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1052PurchaseOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1052PurchaseOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1052_PurchaseOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1052-purchase-order-slip", method = RequestMethod.GET)
    public void print1052PurchaseOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1052PurchaseOrderSlip(response, objectMapper.convertValue(query, Rpt1052PurchaseOrderSlipRequest.class));
    }

    /**
     * Template: 1053_saleOrderSlip.rpt
     * Procedure: [cmagt].[usp_saleOrderMaster_Slip]
     * Desktop: saleOrderMaster.saleOrder_Slip
     */
    @RequestMapping(value = "/reports/print/1053-sale-order-slip", method = RequestMethod.POST)
    public void print1053SaleOrderSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1053SaleOrderSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1053SaleOrderSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1053_saleOrderSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1053-sale-order-slip", method = RequestMethod.GET)
    public void print1053SaleOrderSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1053SaleOrderSlip(response, objectMapper.convertValue(query, Rpt1053SaleOrderSlipRequest.class));
    }

    /**
     * Template: 1050A_BuyerInquiryBookingSlip.rpt
     * Procedure: [cmagt].[USP_inquiryBookingMaster_Slip]
     * Desktop: InquiryBookingMaster.BuyerInquiryBooking_Slip
     */
    @RequestMapping(value = "/reports/print/1050a-buyer-inquiry-booking-slip", method = RequestMethod.POST)
    public void print1050ABuyerInquiryBookingSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1050ABuyerInquiryBookingSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1050ABuyerInquiryBookingSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1050A_BuyerInquiryBookingSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1050a-buyer-inquiry-booking-slip", method = RequestMethod.GET)
    public void print1050ABuyerInquiryBookingSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1050ABuyerInquiryBookingSlip(response, objectMapper.convertValue(query, Rpt1050ABuyerInquiryBookingSlipRequest.class));
    }

    /**
     * Template: 1051_SupplierOfferSlip.rpt
     * Procedure: [cmagt].[USP_purchaseOrderMaster_Slip]
     * Desktop: purchaseOrderMaster.purchaseOrder_Slip
     */
    @RequestMapping(value = "/reports/print/1051-supplier-offer-slip", method = RequestMethod.POST)
    public void print1051SupplierOfferSlip(HttpServletResponse response, @RequestBody(required = false) Rpt1051SupplierOfferSlipRequest request) throws Exception {
        if (request == null) request = new Rpt1051SupplierOfferSlipRequest();
        try { ReportPdfService.require(request.id, "id"); }
        catch (IllegalArgumentException e) { response.sendError(400, e.getMessage()); return; }
        printReport(response, "1051_SupplierOfferSlip.rpt", request.toArgs());
    }

    @RequestMapping(value = "/reports/print/1051-supplier-offer-slip", method = RequestMethod.GET)
    public void print1051SupplierOfferSlipGet(HttpServletResponse response, @RequestParam Map<String, String> query) throws Exception {
        print1051SupplierOfferSlip(response, objectMapper.convertValue(query, Rpt1051SupplierOfferSlipRequest.class));
    }
    // END GENERATED PRINT ACTIONS
}
