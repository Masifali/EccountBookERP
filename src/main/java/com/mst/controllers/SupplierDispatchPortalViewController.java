package com.mst.controllers;

import com.mst.services.SupplierDispatchPortalService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Screen 788 "Supplier Dispatch" - Architecture.WinApp.Purchase.frmSupplierDispatch (ScreenName "frmSupplierDispatch"),
 * DocumentTypeId 246, Supplier Portal (ModuleId 131). The page loads everything through
 * {@link SupplierDispatchPortalRestController} (/api/purchase/supplier-dispatch); the rules are in
 * {@link SupplierDispatchPortalService}.
 */
@Controller
public class SupplierDispatchPortalViewController {

    @GetMapping("/purchase/supplier-dispatch")
    public String page(Model model) {
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "Supplier Dispatch");
        model.addAttribute("screenId", SupplierDispatchPortalService.SCREEN_ID);
        model.addAttribute("documentTypeId", SupplierDispatchPortalService.DOCUMENT_TYPE_ID);
        return "purchase/supplier_dispatch";
    }
}
