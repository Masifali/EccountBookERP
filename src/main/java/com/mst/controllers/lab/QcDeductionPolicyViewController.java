package com.mst.controllers.lab;

import com.mst.services.lab.QcDeductionPolicyService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Lab (ModuleId 7) — screen 164 "Lab Deduction Policy For Purchase", desktop form
 * Architecture.WinApp.QCL.frmQcDeductionPolicy (ScreenName "frmQcDeductionPolicy").
 * The page loads everything through {@link QcDeductionPolicyRestController}
 * (/api/lab/qc-deduction-policy); business rules are in {@link QcDeductionPolicyService}.
 */
@Controller
public class QcDeductionPolicyViewController {

    @GetMapping("/quality/lab-deduction-policy-for-purchase")
    public String page(Model model) {
        model.addAttribute("activeMenu", "quality");
        model.addAttribute("moduleTitle", "Lab Deduction Policy For Purchase");
        model.addAttribute("screenId", QcDeductionPolicyService.SCREEN_ID);
        return "lab/qc_deduction_policy";
    }
}
