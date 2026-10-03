package com.mst.controllers.lab;

import com.mst.services.lab.GroupAnalysisStandardsService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Lab (ModuleId 7) - screen 158 "Group Analysis Standards", desktop form
 * Architecture.WinApp.Lab.InvLabGroupAnalysisStandards (ScreenName "InvLabGroupAnalysisStandards").
 * The page loads everything through {@link GroupAnalysisStandardsRestController}
 * (/api/lab/group-analysis-standards); the rules are in {@link GroupAnalysisStandardsService}.
 */
@Controller
public class GroupAnalysisStandardsViewController {

    @GetMapping("/quality/group-analysis-standards")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Group Analysis Standards");
        model.addAttribute("screenId", GroupAnalysisStandardsService.SCREEN_ID);
        return "lab/group_analysis_standards";
    }
}
