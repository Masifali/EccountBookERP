package com.mst.controllers.sale.engr;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/** Page of screen 847 frmSaleInvoiceEngr (Architecture.WinApp.Mfg.Sale), Sale Invoice Against Gdn. */
@Controller
public class SaleInvoiceGdnMfgEngrViewController {

    @GetMapping("/sale/engr/mfg/sale-invoice-gdn")
    public String page(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sale Invoice Against Gdn");
        return "sale/engr/sale_invoice_gdn_mfg_engr";
    }
}
