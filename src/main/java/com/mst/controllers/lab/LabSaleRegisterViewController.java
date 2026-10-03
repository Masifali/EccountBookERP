package com.mst.controllers.lab;

import com.mst.services.lab.LabOtherReportsService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Lab Report (module 1011) - screen 628 "Lab Sale Analysis Report", ScreenName InvLabSaleRegister,
 * desktop form Architecture.WinApp.Lab/InvLabSaleRegister.cs.
 * Data: {@link LabOtherReportsRestController} (/api/lab/reports/lab-sale-register).
 */
@Controller
public class LabSaleRegisterViewController {

    @GetMapping("/quality/reports/lab-sale-analysis-report")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Lab Sale Analysis Report");
        model.addAttribute("screenId", LabOtherReportsService.SALE_REGISTER_SCREEN);
        model.addAttribute("screenName", LabOtherReportsService.SALE_REGISTER_NAME);
        return "lab/reports/lab_sale_register";
    }
}
