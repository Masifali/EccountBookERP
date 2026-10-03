package com.mst.controllers;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

/**
 * Screen 84 "BS and PL Breakup" (Architecture.WinApp.Account_Reports.BSandPLBreakup) - the hub card's page.
 * A literal mapping, so it wins over AccountsModuleViewController's /accounts/reports/{x} pattern.
 */
@Controller
public class BsPlBreakupViewController {

    @GetMapping("/accounts/reports/bs-pl-breakup")
    public String page(Model model) {
        model.addAttribute("moduleTitle", "BS and PL Breakup");
        return "accounts/reports/bs_pl_breakup";
    }
}
