package com.mst.controllers.sale.steel;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/** Page of screen 543 SaleInvoice_St (Architecture.WinApp.Steel.Sale, Sale Steel module 84). */
@Controller
public class SaleSaleInvoiceStViewController {

    @GetMapping("/sale/steel/sale-invoice")
    public String saleInvoice(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sale Invoice");
        return "sale/steel/sale_invoice_st";
    }
}
