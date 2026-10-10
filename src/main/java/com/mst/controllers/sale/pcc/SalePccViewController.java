package com.mst.controllers.sale.pcc;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

/** Pages of the Sale Pcc forms (module 85, Architecture.WinApp.pcc.Sale.*). */
@Controller
@RequestMapping("/sale/pcc")
public class SalePccViewController {

    private String page(Model model, String title, String template) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", title);
        return template;
    }

    @GetMapping("/delivery-order")
    public String deliveryOrder(Model model) { return page(model, "Delivery Order Concrete", "sale/pcc/delivery_order_concrete"); }
}
