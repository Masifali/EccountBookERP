package com.mst.controllers.sale.engr;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/** Page of screen 849 frmSaleInvoiceReturnEngr (Architecture.WinApp.Mfg.Sale), Sale Invoice Return. */
@Controller
public class SaleInvoiceReturnMfgEngrViewController {

    @GetMapping("/sale/engr/mfg/sale-invoice-return")
    public String page(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sale Invoice Return");
        return "sale/engr/sale_invoice_return_mfg_engr";
    }
}
