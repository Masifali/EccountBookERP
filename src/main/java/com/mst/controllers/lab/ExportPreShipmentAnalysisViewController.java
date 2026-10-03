package com.mst.controllers.lab;

import com.mst.services.lab.ExportPreShipmentAnalysisService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Lab (module 7) — screen 168 "Export Pre Shipment Analysis", desktop form
 * Architecture.WinApp.Export.EximPreProductionLab (ScreenName "EximPreProductionLab"). The page loads
 * everything through {@link ExportPreShipmentAnalysisRestController} (/api/lab/export-pre-shipment-analysis).
 */
@Controller
public class ExportPreShipmentAnalysisViewController {

    @GetMapping("/quality/export-pre-shipment-analysis")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Export Pre Shipment Analysis");
        model.addAttribute("screenId", ExportPreShipmentAnalysisService.SCREEN_ID);
        return "lab/export_pre_shipment_analysis";
    }
}
