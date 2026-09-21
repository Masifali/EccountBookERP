package com.mst.controllers.cmagt;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

/**
 * Commission Trading module menu screens, served at the route the application menu
 * actually links to: modules.html's Commission Trading card points at "/commission".
 * That mapping previously lived in MainModulesController and redirected to
 * "/purchase/purchase-order", an unrelated module; it now belongs to this controller.
 *
 * The two menu pages reproduce the desktop's "Commission Trading" screen board: the
 * Commission Agent Portal card (7 screens) and the Commission Trading Reports card
 * (5 reports).
 *
 * Screen-to-desktop-form map. Each DocumentTypeId below was read out of that form's own
 * source, NOT assumed from the enum - and the two disagree at the end of the range:
 *
 *   Buyer Inquiry Booking               frmBuyerInquiryBooking.cs                1050
 *   Supplier Offer                      frmSupplierOfferCmagt.cs                 1051
 *   Purchase Order / Deal With Supplier frmPurchaseOrderCmagt.cs                 1052
 *   Sale Order / Deal With Buyer        frmSaleOrderCmagt.cs                     1053   <-- built
 *   GRN Loading Challan                 frmGrnLoadingChallanCmagt.cs             1054
 *   Goods Dispatching Note              frmGoodsDispatchingNoteCmagt.cs          1055
 *   Commission Agent Bill Against Gdn   frmCommissionAgentTradeBillAgainstGdn.cs 1056
 *
 * Architecture.Model.CommissionAgent.DocumentTypeEnum only declares 1050-1055
 * (BuyerInquiry..GdnDispatched); the Trade Bill's 1056 has no enum member, so the module's
 * real range is 1050-1056.
 *
 * Reports are NOT uniformly document-typed. Sale Order Report uses 1053 and Purchase Order
 * Report uses 1052, but Grn Supplier Loading Report, Gdn Buyer Dispatch Report and Agent
 * Trade Bill Register declare no DocumentTypeId at all - they filter through their own
 * report methods. No document type is invented for them.
 *
 * Every route below renders its OWN page. Screens that have not been migrated yet render an
 * explicit placeholder naming their desktop form - they deliberately never fall back to
 * another screen's template, because two unrelated URLs rendering the same page is a bug,
 * not a shortcut.
 */
@Controller
@RequestMapping("/commission")
public class CmagtModuleViewController {

    private static final String VIEW_STUB = "cmagt/cmagt_not_implemented";

    @GetMapping({"", "/", "/dashboard"})
    public String dashboard(Model model) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("moduleTitle", "Commission Trading");
        return "cmagt/cmagt_dashboard";
    }

    @GetMapping("/reports")
    public String reports(Model model) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("moduleTitle", "Commission Trading Reports");
        return "cmagt/cmagt_reports";
    }

    // ---- transaction screens not yet migrated -----------------------------

    @GetMapping("/buyer-inquiry-booking")
    public String buyerInquiryBooking(Model model) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("moduleTitle", "Buyer Inquiry Booking");
        return "cmagt/buyer_inquiry_booking";
    }

    @GetMapping("/supplier-offer")
    public String supplierOffer(Model model) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("moduleTitle", "Supplier Offer");
        return "cmagt/supplier_offer_cmagt";
    }

    @GetMapping("/purchase-order")
    public String purchaseOrder(Model model) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("moduleTitle", "Purchase Order / Deal With Supplier");
        return "cmagt/purchase_order_cmagt";
    }

    @GetMapping("/grn-loading-challan")
    public String grnLoadingChallan(Model model) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("moduleTitle", "GRN Loading Challan");
        return "cmagt/grn_loading_challan_cmagt";
    }

    @GetMapping("/goods-dispatching-note")
    public String goodsDispatchingNote(Model model) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("moduleTitle", "Goods Dispatching Note");
        return "cmagt/goods_dispatching_note_cmagt";
    }

    @GetMapping("/trade-bill-against-gdn")
    public String tradeBillAgainstGdn(Model model) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("moduleTitle", "Commission Agent Bill Against Gdn");
        return "cmagt/trade_bill_against_gdn";
    }

    // ---- reports not yet migrated -----------------------------------------

    @GetMapping("/reports/sale-order")
    public String reportSaleOrder(Model model) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("moduleTitle", "Sale Order Report");
        return "cmagt/reports/sale_order_report";
    }

    @GetMapping("/reports/purchase-order")
    public String reportPurchaseOrder(Model model) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("moduleTitle", "Purchase Order Report");
        return "cmagt/reports/purchase_order_report";
    }

    @GetMapping("/reports/grn-supplier-loading")
    public String reportGrnSupplierLoading(Model model) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("moduleTitle", "Grn Supplier Loading Report");
        return "cmagt/reports/grn_supplier_loading_report";
    }

    @GetMapping("/reports/gdn-buyer-dispatch")
    public String reportGdnBuyerDispatch(Model model) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("moduleTitle", "Gdn Buyer Dispatch Report");
        return "cmagt/reports/gdn_buyer_dispatch_report";
    }

    @GetMapping("/reports/agent-trade-bill-register")
    public String reportAgentTradeBillRegister(Model model) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("moduleTitle", "Agent Trade Bill Register");
        return "cmagt/reports/agent_trade_bill_register";
    }

    private String stub(Model model, String title, String desktopForm, Integer documentTypeId) {
        model.addAttribute("activeMenu", "commission");
        model.addAttribute("moduleTitle", title);
        model.addAttribute("desktopForm", desktopForm);
        model.addAttribute("documentTypeId", documentTypeId);
        return VIEW_STUB;
    }
}
