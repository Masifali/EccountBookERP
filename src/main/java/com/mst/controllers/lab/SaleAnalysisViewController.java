package com.mst.controllers.lab;

import com.mst.services.lab.SaleAnalysisService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Lab (ModuleId 7) — screen 161 "Sale Analysis", desktop form
 * Architecture.WinApp.Lab.InvLabSaleAnalysis (ScreenName "InvLabSaleAnalysis"), DocumentTypeId 304.
 * The page loads everything through {@link SaleAnalysisRestController} (/api/lab/sale-analysis);
 * business rules are in {@link SaleAnalysisService}.
 */
@Controller
public class SaleAnalysisViewController {

    @GetMapping("/quality/sale-analysis")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Sale Analysis");
        model.addAttribute("screenId", SaleAnalysisService.SCREEN_ID);
        model.addAttribute("documentTypeId", SaleAnalysisService.DOCUMENT_TYPE_ID);
        return "lab/sale_analysis";
    }
}
