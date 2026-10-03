package com.mst.controllers.lab;

import com.mst.services.lab.LabRegisterReportsService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Lab Report (module 1011) - screen 629 "Lab Sample Analysis Report", ScreenName InvLabSampleRegister,
 * desktop form Architecture.WinApp.Lab.InvLabSampleRegister (seed_screendef.txt:580).
 * Data: {@link LabRegisterReportsRestController} (/api/lab/reports/sample-register).
 */
@Controller
public class LabSampleRegisterReportViewController {

    @GetMapping("/quality/reports/lab-sample-analysis-report")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Lab Sample Analysis Report");
        model.addAttribute("screenId", LabRegisterReportsService.SAMPLE_REPORT_SCREEN);
        model.addAttribute("screenName", LabRegisterReportsService.SAMPLE_REPORT_NAME);
        return "lab/reports/sample_register";
    }
}
