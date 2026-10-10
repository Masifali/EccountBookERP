package com.mst.controllers.sale.pcc;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

/** Page of screen 547 SaleInvoiceAgainstGDNConcrete (Sale Pcc, module 85). */
@Controller
@RequestMapping("/sale/pcc")
public class SalePccInvoiceGdnViewController {
    @GetMapping("/sale-invoice-against-gdn")
    public String saleInvoiceAgainstGdn(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sale Invoice Against GDN");
        return "sale/pcc/sale_invoice_against_gdn_concrete";
    }
}
