package com.mst.controllers.lab;

import com.mst.services.lab.PurchaseAnalysisService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Lab (ModuleId 7) — screen 160 "Purchase Analysis", desktop form
 * Architecture.WinApp.Lab.InvLabPurchaseAnalysis (ScreenName "InvLabPurchaseAnalysis"), DocumentTypeId 303.
 * The page loads everything through {@link PurchaseAnalysisRestController} (/api/lab/purchase-analysis);
 * business rules are in {@link PurchaseAnalysisService}.
 */
@Controller
public class PurchaseAnalysisViewController {

    @GetMapping("/quality/purchase-analysis")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Purchase Analysis");
        model.addAttribute("screenId", PurchaseAnalysisService.SCREEN_ID);
        model.addAttribute("documentTypeId", PurchaseAnalysisService.DOCUMENT_TYPE_ID);
        return "lab/purchase_analysis";
    }
}
