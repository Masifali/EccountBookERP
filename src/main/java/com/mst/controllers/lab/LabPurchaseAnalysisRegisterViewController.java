package com.mst.controllers.lab;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * 633 InvLabPurchaseRegister "Purchase Analylsis Report" (module 1011 Lab Report) - desktop Architecture.WinApp.Lab/InvLabPurchaseRegister.cs.
 * Data: LabPurchaseReportsRestController /api/lab/reports/purchase-register.
 */
@Controller
public class LabPurchaseAnalysisRegisterViewController {

    @GetMapping("/quality/reports/purchase-analysis-report")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Purchase Analylsis Report");
        return "lab/reports/purchase_analysis_register";
    }
}
