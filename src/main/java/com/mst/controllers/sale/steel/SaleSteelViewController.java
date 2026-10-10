package com.mst.controllers.sale.steel;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

/** Pages of the Sale Steel screens (module 84, Architecture.WinApp.Steel.Sale.*). */
@Controller
@RequestMapping("/sale/steel")
public class SaleSteelViewController {

    private String page(Model model, String title, String template) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", title);
        return template;
    }

    @GetMapping("/outward-gate-pass")
    public String outwardGatePass(Model model) { return page(model, "Outward Gate Pass", "sale/steel/outward_gate_pass_st"); }

    @GetMapping("/delivery-order")
    public String deliveryOrder(Model model) { return page(model, "Delivery Order", "sale/steel/delivery_order_st"); }

    @GetMapping("/sale-order")
    public String saleOrder(Model model) { return page(model, "Sale Order", "sale/steel/sale_order_st"); }

    @GetMapping("/gdn")
    public String gdn(Model model) { return page(model, "Goods Dispatch Notes", "sale/steel/inv_gdn_st"); }
}
