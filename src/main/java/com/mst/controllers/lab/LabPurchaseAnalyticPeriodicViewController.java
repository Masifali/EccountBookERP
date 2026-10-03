package com.mst.controllers.lab;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * 630 LabPurchaseAnalyticPeriodic "Lab Purchase Analysis Periodic Report" (module 1011 Lab Report) - desktop Architecture.WinApp.Lab/LabPurchaseAnalyticPeriodic.cs.
 * Data: LabPurchaseReportsRestController /api/lab/reports/purchase-periodic.
 */
@Controller
public class LabPurchaseAnalyticPeriodicViewController {

    @GetMapping("/quality/reports/lab-purchase-analysis-periodic-report")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Lab Purchase Analysis Periodic Report");
        return "lab/reports/purchase_analytic_periodic";
    }
}
