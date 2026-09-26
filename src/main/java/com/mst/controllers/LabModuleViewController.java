package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

/**
 * Module 7 - Lab. The desktop module holds eight screens:
 *
 *   155  InvLabSampleLogRegister                       Sample Log Register
 *   156  InvLabAnalysisItems                           Item Analysis Parameter      <- built
 *   157  InvLabAnalysisGroup                           Analysis Group
 *   158  InvLabGroupAnalysisStandards                  Group Analysis Standards
 *   159  InvLabSampleAnalysis                          Sample Analysis
 *   160  InvLabPurchaseAnalysis                        Purchase Analysis
 *   162  InvLabAnalysisInProcess                       In-Process Analysis
 *   163  LabInProcessAnalysisStepAndParameterSchedule  InProcess Analysis Steps Schedule
 *
 * (161 is absent from the rights dump this list was taken from, which covers one user's grants -
 * so it exists but is not evidenced there. It is not guessed at.)
 *
 * Lab reports are a separate module, 1011, and are not part of this controller.
 *
 * Each screen gets its own route. They are NOT served from one parameterised endpoint: the
 * desktop forms read different tables through different procedures, and collapsing them would
 * repeat the mistake already found in the purchase-invoice family, where two unrelated forms were
 * rendered from one generic scaffold and matched neither.
 */
@Controller
@RequestMapping("/lab")
public class LabModuleViewController {

    @GetMapping({"", "/", "/dashboard"})
    public String labDashboard(Model model) {
        model.addAttribute("activeMenu", "lab");
        model.addAttribute("moduleTitle", "Lab");
        return "lab/lab_dashboard";
    }

    /** Screen 156 - desktop InvLabAnalysisItems.cs. */
    @GetMapping("/item-analysis-parameter")
    public String itemAnalysisParameter(Model model) {
        model.addAttribute("activeMenu", "lab");
        model.addAttribute("moduleTitle", "Item Analysis Parameter");
        return "lab/item_analysis_parameter";
    }
}
