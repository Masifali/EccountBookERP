package com.mst.controllers.sale.engr;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/** Page of screen 808 frmDeliveryOrderEngr (Architecture.WinApp.Mfg.frmDeliveryOrderEngr, Sale Engr module 134). */
@Controller
public class SaleDeliveryOrderMfgEngrPageController {

    @GetMapping("/sale/engr/mfg/delivery-order")
    public String page(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Delivery Order");
        return "sale/engr/mfg/delivery_order_mfg_engr";
    }
}
