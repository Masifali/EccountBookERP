package com.mst.controllers.sale.steel;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/** Page of screen 544 SaleInvoiceDirect_St (Architecture.WinApp.Steel.Sale, Sale Steel module 84). */
@Controller
public class SaleSaleInvoiceDirectStViewController {

    @GetMapping("/sale/steel/sale-invoice-direct")
    public String saleInvoiceDirect(Model model) {
        model.addAttribute("activeMenu", "sale");
        model.addAttribute("moduleTitle", "Sale Invoice Direct");
        return "sale/steel/sale_invoice_direct_st";
    }
}
