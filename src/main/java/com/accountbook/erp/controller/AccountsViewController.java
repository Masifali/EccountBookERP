package com.accountbook.erp.controller;

import com.accountbook.erp.service.AccountsService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

@Controller
@RequestMapping("/accounts")
public class AccountsViewController {

    @Autowired
    private AccountsService accountsService;

    @GetMapping("/dashboard")
    public String showDashboard(Model model) {
        model.addAttribute("moduleName", "Accounts");
        model.addAttribute("userName", "Numan Rasheed (Admin)");
        model.addAttribute("companyName", "Golden Ace Rice Mills (Pvt) Ltd.");
        model.addAttribute("fiscalYear", "2026-27");
        model.addAttribute("submodules", accountsService.getSubModules());
        return "accounts/dashboard";
    }
}
