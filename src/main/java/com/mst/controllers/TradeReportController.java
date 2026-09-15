package com.mst.controllers;

import com.mst.models.dto.TradeReportRequest;
import com.mst.services.TradeReportService;
import java.util.*;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

@Controller
public class TradeReportController {
    private final TradeReportService service;
    public TradeReportController(TradeReportService service) { this.service=service; }
    @GetMapping("/accounts/reports/trade-payables")
    public String payables(Model model) { model.addAttribute("accountClass",3); return "accounts/reports/trade_accounts"; }
    @GetMapping("/accounts/reports/trade-receivables")
    public String receivables(Model model) { model.addAttribute("accountClass",2); return "accounts/reports/trade_accounts"; }
    @GetMapping("/accounts/reports/all-payables")
    public String allPayables() { return "accounts/reports/all_payables"; }
    @GetMapping({"/accounts/reports/supplier-aging", "/accounts/reports/payables-aging-new"})
    public String supplierAging() { return "accounts/reports/payables_aging_new"; }
    @GetMapping({"/accounts/reports/customer-aging", "/accounts/reports/receivables-aging-new"})
    public String customerAging() { return "accounts/reports/receivables_aging_new"; }
    @GetMapping("/api/accounts/trade-report/lookups") @ResponseBody
    public Map<String,Object> lookups() { return service.lookups(); }
    @GetMapping("/api/accounts/trade-report/accounts") @ResponseBody
    public List<Map<String,Object>> accounts(@RequestParam(defaultValue="0") int costCenter) { return service.accounts(costCenter); }
    @GetMapping("/api/accounts/trade-report") @ResponseBody
    public List<Map<String,Object>> report(@ModelAttribute TradeReportRequest request) { return service.load(request); }
}
