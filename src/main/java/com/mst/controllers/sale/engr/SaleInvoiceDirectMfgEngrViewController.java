package com.mst.controllers.sale.engr;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/** Page of screen 780 frmSaleInvoiceDirectEngr (Architecture.WinApp.Mfg.Sale), Sale Invoice Direct. */
@Controller
public class SaleInvoiceDirectMfgEngrViewController {

    @GetMapping("/sale/engr/mfg/sale-invoice-direct")
    public String page(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sale Invoice Direct");
        return "sale/engr/sale_invoice_direct_mfg_engr";
    }
}
