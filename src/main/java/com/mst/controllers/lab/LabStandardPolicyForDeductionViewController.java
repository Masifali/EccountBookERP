package com.mst.controllers.lab;

import com.mst.services.lab.LabStandardPolicyForDeductionService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Lab (ModuleId 7) — screen 166 "Lab Standard Policy For Deduction (Not Use)", desktop form
 * Architecture.WinApp.Lab.InvLabStandardPolicyForDeduction (ScreenName "InvLabStandardPolicyForDeduction").
 * The page loads everything through {@link LabStandardPolicyForDeductionRestController}
 * (/api/lab/standard-policy-for-deduction); the rules are in {@link LabStandardPolicyForDeductionService}.
 */
@Controller
public class LabStandardPolicyForDeductionViewController {

    @GetMapping("/quality/lab-standard-policy-for-deduction")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Lab Standard Policy For Deduction (Not Use)");
        model.addAttribute("screenId", LabStandardPolicyForDeductionService.SCREEN_ID);
        return "lab/standard_policy_for_deduction";
    }
}
