package com.mst.controllers.quality;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

@Controller
@RequestMapping("/quality")
public class QualityControlViewController {

    @GetMapping({"", "/", "/dashboard"})
    public String dashboard(Model model) {
        model.addAttribute("activeMenu", "quality");
        return "quality_control/dashboard";
    }

    @GetMapping("/lab")
    public String labMaster(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Lab Master Definition");
        return "quality_control/lab";
    }

    @GetMapping("/lab-report")
    public String labReport(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Lab Report");
        return "quality_control/lab_report";
    }

    @GetMapping("/item-analysis-parameter")
    public String itemAnalysisParameter(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Item Analysis Parameter");
        return "quality_control/item_analysis_parameter";
    }

    @GetMapping("/analysis-group")
    public String analysisGroup(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Analysis Group Definition");
        return "quality_control/analysis_group";
    }

    @GetMapping("/group-analysis-standards")
    public String groupAnalysisStandards(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Group Analysis Standards");
        return "quality_control/group_analysis_standards";
    }

    @GetMapping("/sample-log-register")
    public String sampleLogRegister(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Sample Log Register");
        return "quality_control/sample_log_register";
    }

    @GetMapping("/sample-analysis")
    public String sampleAnalysis(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Sample Analysis");
        return "quality_control/sample_analysis";
    }

    @GetMapping("/purchase-analysis")
    public String purchaseAnalysis(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Purchase Analysis");
        return "quality_control/purchase_analysis";
    }

    @GetMapping("/inprocess-analysis-steps")
    public String inprocessAnalysisSteps(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "InProcess Analysis Steps Schedule");
        return "quality_control/inprocess_analysis_steps";
    }

    @GetMapping("/inprocess-analysis")
    public String inprocessAnalysis(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "In-Process Analysis");
        return "quality_control/inprocess_analysis";
    }

    @GetMapping("/reports/inprocess-analysis-report")
    public String inprocessAnalysisReport(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "In-Process Analysis Report");
        return "quality_control/reports/inprocess_analysis_report";
    }

    @GetMapping("/reports/lab-sample-analysis-report")
    public String labSampleAnalysisReport(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Lab Sample Analysis Report");
        return "quality_control/reports/lab_sample_analysis_report";
    }

    @GetMapping("/reports/sample-analysis-register")
    public String sampleAnalysisRegister(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Sample Analysis Register");
        return "quality_control/reports/sample_analysis_register";
    }

    @GetMapping("/reports/purchase-analysis-by-vehicle")
    public String purchaseAnalysisByVehicle(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Purchase Analysis By Vehicle");
        return "quality_control/reports/purchase_analysis_by_vehicle";
    }

    @GetMapping("/reports/purchase-analysis-report")
    public String purchaseAnalysisReport(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Purchase Analysis Report");
        return "quality_control/reports/purchase_analysis_report";
    }

    @GetMapping("/reports/lab-purchase-analysis-periodic-report")
    public String labPurchaseAnalysisPeriodicReport(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Lab Purchase Analysis Periodic Report");
        return "quality_control/reports/lab_purchase_analysis_periodic_report";
    }
}
