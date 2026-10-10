package com.mst.controllers.sale.pcc;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

/** Page of screen 553 SaleInvoiceReturnConcrete (Sale Pcc, module 85). */
@Controller
@RequestMapping("/sale/pcc")
public class SalePccInvoiceReturnViewController {
    @GetMapping("/sale-invoice-return")
    public String saleInvoiceReturn(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sale Invoice Return");
        return "sale/pcc/sale_invoice_return_concrete";
    }
}
