package com.mst.controllers.lab;

import com.mst.services.lab.InProcessAnalysisService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Lab (ModuleId 7) — screen 162 "In-Process Analysis", desktop form
 * Architecture.WinApp.Lab.InvLabAnalysisInProcess (ScreenName "InvLabAnalysisInProcess"), DocumentTypeId 306.
 * The page loads everything through {@link InProcessAnalysisRestController} (/api/lab/inprocess-analysis);
 * business rules are in {@link InProcessAnalysisService}.
 */
@Controller
public class InProcessAnalysisViewController {

    @GetMapping("/quality/inprocess-analysis")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "In-Process Analysis");
        model.addAttribute("screenId", InProcessAnalysisService.SCREEN_ID);
        model.addAttribute("documentTypeId", InProcessAnalysisService.DOCUMENT_TYPE_ID);
        return "lab/inprocess_analysis";
    }
}
