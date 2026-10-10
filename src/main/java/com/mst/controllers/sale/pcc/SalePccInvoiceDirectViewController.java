package com.mst.controllers.sale.pcc;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

/** Page of screen 546 SaleInvoiceDirectConcrete (Sale Pcc, module 85). */
@Controller
@RequestMapping("/sale/pcc")
public class SalePccInvoiceDirectViewController {
    @GetMapping("/sale-invoice-direct")
    public String saleInvoiceDirect(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sale Invoice Direct");
        return "sale/pcc/sale_invoice_direct_concrete";
    }
}
