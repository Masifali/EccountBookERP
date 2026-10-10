package com.mst.controllers.sale.engr;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/** Page of screen 779 frmSaleOrderEngr (Architecture.WinApp.Mfg.Sale.frmSaleOrderEngr, Sale Engr module 134). */
@Controller
public class SaleSoMfgEngrPageController {

    @GetMapping("/sale/engr/mfg/sale-order")
    public String page(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sale Order");
        return "sale/engr/mfg/sale_order_mfg_engr";
    }
}
