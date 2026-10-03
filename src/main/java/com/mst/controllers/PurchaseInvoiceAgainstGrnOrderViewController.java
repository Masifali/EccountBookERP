package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Page of Architecture.WinApp.Purchase.PurchaseInvoiceAgainstGrnOrder (dbo.ScreenDefinition 132, module 5 Supplier
 * Purchases, DocumentTypeId 172). The page reads rights, lookups and the next number from
 * /api/purchase-invoice-against-grn-order/init.
 */
@Controller
public class PurchaseInvoiceAgainstGrnOrderViewController {

    @GetMapping("/purchase/purchase-invoice-against-grn-order")
    public String page(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Purchase Invoice Against Grn Order");
        model.addAttribute("documentTypeId", 172);
        return "purchase/purchase_invoice_against_grn_order";
    }
}
