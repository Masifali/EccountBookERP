package com.mst.controllers;

import com.mst.services.StockInTransitService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Supplier Purchases (ModuleId 5) — screen 868 "Stock In Transit", desktop form
 * Architecture.WinApp.Purchase.frmSupplierDispatchPreBill (ScreenName "frmSupplierDispatchPreBill"),
 * DocumentTypeId 251. The page loads everything through {@link StockInTransitRestController}
 * (/api/purchase/stock-in-transit); business rules are in {@link StockInTransitService}.
 */
@Controller
public class StockInTransitViewController {

    @GetMapping("/purchase/stock-in-transit")
    public String page(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Stock In Transit");
        model.addAttribute("screenId", StockInTransitService.SCREEN_ID);
        model.addAttribute("documentTypeId", StockInTransitService.DOCUMENT_TYPE_ID);
        return "purchase/stock_in_transit";
    }
}
