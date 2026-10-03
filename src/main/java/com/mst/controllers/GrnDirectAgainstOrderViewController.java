package com.mst.controllers;

import com.mst.services.GrnDirectAgainstOrderService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * GRN Direct Against Order - Architecture.WinApp.Purchase.GRNDirectAgainstOrder (ScreenDefinition 133 "GRNDirectAgainstOrder",
 * DocumentTypeId 169, Purchase module 5). The page only; its API is GrnDirectAgainstOrderRestController.
 */
@Controller
public class GrnDirectAgainstOrderViewController {

    private final GrnDirectAgainstOrderService service;

    public GrnDirectAgainstOrderViewController(GrnDirectAgainstOrderService service) { this.service = service; }

    @GetMapping("/purchase/grn-direct-against-order")
    public String page(Model model) {
        service.requireView();
        model.addAttribute("activeMenu", "purchase");
        model.addAttribute("moduleTitle", "GRN Direct Against Order");
        model.addAttribute("screenId", 133);
        model.addAttribute("documentTypeId", 169);
        return "purchase/grn_direct_against_order";
    }
}
