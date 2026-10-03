package com.mst.controllers.lab;

import com.mst.services.lab.InProcessAnalysisStepsService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Lab (ModuleId 7) - screen 163 "InProcess Analysis Steps Schedule", desktop form
 * Architecture.WinApp.Lab.LabInProcessAnalysisStepAndParameterSchedule (ScreenName
 * "LabInProcessAnalysisStepAndParameterSchedule"). The page loads everything through
 * {@link InProcessAnalysisStepsRestController} (/api/lab/inprocess-analysis-steps); the rules are in
 * {@link InProcessAnalysisStepsService}.
 */
@Controller
public class InProcessAnalysisStepsViewController {

    @GetMapping("/quality/inprocess-analysis-steps")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "InProcess Analysis Steps Schedule");
        model.addAttribute("screenId", InProcessAnalysisStepsService.SCREEN_ID);
        return "lab/inprocess_analysis_steps";
    }
}
