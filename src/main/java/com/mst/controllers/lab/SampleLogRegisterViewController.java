package com.mst.controllers.lab;

import com.mst.services.lab.SampleLogRegisterService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Lab (ModuleId 7) — screen 155 "Sample Log Register", desktop form
 * Architecture.WinApp.Lab.InvLabSampleLogRegister (ScreenName "InvLabSampleLogRegister"),
 * DocumentTypeId 301. The page loads everything through {@link SampleLogRegisterRestController}
 * (/api/lab/sample-log-register); business rules are in {@link SampleLogRegisterService}.
 */
@Controller
public class SampleLogRegisterViewController {

    @GetMapping("/quality/sample-log-register")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Sample Log Register");
        model.addAttribute("screenId", SampleLogRegisterService.SCREEN_ID);
        model.addAttribute("documentTypeId", SampleLogRegisterService.DOCUMENT_TYPE_ID);
        return "lab/sample_log_register";
    }
}
