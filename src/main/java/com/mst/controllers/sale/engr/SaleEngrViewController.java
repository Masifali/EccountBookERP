package com.mst.controllers.sale.engr;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

/** Pages of the Sale Engr screens (module 83, Architecture.WinApp.SaleTrading.*). */
@Controller
@RequestMapping("/sale/engr")
public class SaleEngrViewController {

    private String page(Model model, String title, String template) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", title);
        return template;
    }

    @GetMapping("/outward-gate-pass")
    public String outwardGatePass(Model model) { return page(model, "Outward GatePass", "sale/engr/outward_gate_pass_engr"); }

    @GetMapping("/delivery-order")
    public String deliveryOrder(Model model) { return page(model, "Delivery Order", "sale/engr/delivery_order_engr"); }

    @GetMapping("/gdn")
    public String gdn(Model model) { return page(model, "Goods Dispatch Notes", "sale/engr/gdn_engr"); }

    @GetMapping("/sale-order")
    public String saleOrder(Model model) { return page(model, "Sale Order", "sale/engr/sale_order_engr"); }

    @GetMapping("/sale-invoice")
    public String saleInvoice(Model model) { return page(model, "Sale Invoice", "sale/engr/sale_invoice_engr"); }

    @GetMapping("/sale-invoice-direct")
    public String saleInvoiceDirect(Model model) { return page(model, "Sale Invoice Direct", "sale/engr/sale_invoice_direct_engr"); }

    @GetMapping("/sale-invoice-return")
    public String saleInvoiceReturn(Model model) { return page(model, "Sale Invoice Return", "sale/engr/sale_invoice_return_engr"); }
}
