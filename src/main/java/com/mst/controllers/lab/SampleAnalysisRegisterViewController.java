package com.mst.controllers.lab;

import com.mst.services.lab.LabRegisterReportsService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Lab Report (module 1011) - screen 632 "Sample Analysis Register", ScreenName SampleAnalysisRegister.
 * Its TargetUrl is ALSO Architecture.WinApp.Lab.InvLabSampleRegister (seed_screendef.txt:583): the same
 * desktop form as 629 behind a separate rights row. The form reads neither its Tag nor any right, so
 * the page is identical; only the ScreenName whose View grant is required differs.
 * Data: {@link LabRegisterReportsRestController} (/api/lab/reports/sample-register).
 */
@Controller
public class SampleAnalysisRegisterViewController {

    @GetMapping("/quality/reports/sample-analysis-register")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Sample Analysis Register");
        model.addAttribute("screenId", LabRegisterReportsService.SAMPLE_REGISTER_SCREEN);
        model.addAttribute("screenName", LabRegisterReportsService.SAMPLE_REGISTER_NAME);
        return "lab/reports/sample_register";
    }
}
