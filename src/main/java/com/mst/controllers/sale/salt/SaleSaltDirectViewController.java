package com.mst.controllers.sale.salt;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/** Page of screen 585 frmSaleDirectInvoice (Architecture.WinApp.SaleForSalt, Sale Salt module 94). */
@Controller
public class SaleSaltDirectViewController {

    @GetMapping("/sale/salt/sale-direct-invoice")
    public String saleDirectInvoice(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sale Direct Invoice");
        return "sale/salt/sale_direct_invoice_salt";
    }
}
