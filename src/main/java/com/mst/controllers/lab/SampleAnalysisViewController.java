package com.mst.controllers.lab;

import com.mst.services.lab.SampleAnalysisService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Lab (ModuleId 7) — screen 159 "Sample Analysis", desktop form
 * Architecture.WinApp.Lab.InvLabSampleAnalysis (ScreenName "InvLabSampleAnalysis"), DocumentTypeId 302.
 * The page loads everything through {@link SampleAnalysisRestController} (/api/lab/sample-analysis);
 * business rules are in {@link SampleAnalysisService}.
 */
@Controller
public class SampleAnalysisViewController {

    @GetMapping("/quality/sample-analysis")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Sample Analysis");
        model.addAttribute("screenId", SampleAnalysisService.SCREEN_ID);
        model.addAttribute("documentTypeId", SampleAnalysisService.DOCUMENT_TYPE_ID);
        return "lab/sample_analysis";
    }
}
