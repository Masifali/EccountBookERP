package com.mst.controllers.lab;

import com.mst.services.lab.AnalysisGroupService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Lab (module 7) — screen 157 "Analysis Group", desktop form Architecture.WinApp.Lab.InvLabAnalysisGroup
 * (ScreenName "InvLabAnalysisGroup"). The page loads everything through
 * {@link AnalysisGroupRestController} (/api/lab/analysis-group).
 */
@Controller
public class AnalysisGroupViewController {

    @GetMapping("/quality/analysis-group")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Analysis Group");
        model.addAttribute("screenId", AnalysisGroupService.SCREEN_ID);
        return "lab/analysis_group";
    }
}
