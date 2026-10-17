package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

/** 476 Architecture.WinApp.Inventory_Reports.frmPurchaseInvoiceHistory "Purchase Report" (Purchase Reports, module 52). Data: PurchaseInvoiceHistoryRestController. */
@Controller
@RequestMapping("/purchase")
public class PurchaseInvoiceHistoryPageController {
    @GetMapping("/reports/purchase-invoice-history")
    public String page(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Purchase Report");
        return "purchase/reports/purchase_invoice_history_report";
    }
}
