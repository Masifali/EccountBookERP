package com.mst.controllers.lab;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * 626 LabDataVehicleWiseByParent "Purchase Analysis By Vehicle" (module 1011 Lab Report) - desktop Architecture.WinApp.Lab/LabDataVehicleWiseByParent.cs.
 * Data: LabPurchaseReportsRestController /api/lab/reports/purchase-by-vehicle.
 */
@Controller
public class LabPurchaseAnalysisByVehicleViewController {

    @GetMapping("/quality/reports/purchase-analysis-by-vehicle")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Purchase Analysis By Vehicle");
        return "lab/reports/purchase_analysis_by_vehicle";
    }
}
