package com.mst.controllers.lab;

import com.mst.services.lab.LabOtherReportsService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Lab Report (module 1011) - screen 627 "Lab Purchase Analysis Report (Not Use)", ScreenName InvLabPurchaseReport,
 * desktop form Architecture.WinApp.Lab/InvLabPurchaseReport.cs.
 * Data: {@link LabOtherReportsRestController} (/api/lab/reports/lab-purchase-report).
 */
@Controller
public class LabPurchaseReportViewController {

    @GetMapping("/quality/reports/lab-purchase-analysis-report-old")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Lab Purchase Analysis Report (Not Use)");
        model.addAttribute("screenId", LabOtherReportsService.PURCHASE_REPORT_SCREEN);
        model.addAttribute("screenName", LabOtherReportsService.PURCHASE_REPORT_NAME);
        return "lab/reports/lab_purchase_report";
    }
}
