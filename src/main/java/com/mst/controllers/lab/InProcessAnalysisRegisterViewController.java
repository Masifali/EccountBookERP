package com.mst.controllers.lab;

import com.mst.services.lab.LabRegisterReportsService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Lab Report (module 1011) - screen 631 "In-Process Analysis Report", ScreenName InProcessLabAnalysisRegister,
 * desktop form Architecture.WinApp.Lab.InProcessLabAnalysisRegister (seed_screendef.txt:582).
 * Data: {@link LabRegisterReportsRestController} (/api/lab/reports/inprocess-register).
 */
@Controller
public class InProcessAnalysisRegisterViewController {

    @GetMapping("/quality/reports/inprocess-analysis-report")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "In-Process Analysis Report");
        model.addAttribute("screenId", LabRegisterReportsService.IN_PROCESS_SCREEN);
        model.addAttribute("screenName", LabRegisterReportsService.IN_PROCESS_NAME);
        return "lab/reports/inprocess_analysis_register";
    }
}
