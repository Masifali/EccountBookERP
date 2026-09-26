package com.mst.controllers;

import com.mst.models.dto.DesktopReceivablesRequest;
import com.mst.services.DesktopReceivablesService;
import java.util.*;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

@Controller
public class DesktopReceivablesController {
    private final DesktopReceivablesService service;
    public DesktopReceivablesController(DesktopReceivablesService service) { this.service=service; }
    @GetMapping({"/accounts/reports/receivables-by-due-dates","/reports/receivables-by-due-dates"})
    public String dueDates(Model model) { return page("receivables-by-due-dates",model); }
    @GetMapping({"/accounts/reports/receivables-receipt-schedule","/reports/receivables-receipt-schedule"})
    public String schedule(Model model) { return page("receivables-receipt-schedule",model); }
    @GetMapping({"/accounts/reports/receivables-report","/reports/receivables-report"})
    public String receivables(Model model) { return page("receivables-report",model); }
    @GetMapping({"/accounts/reports/payables-report","/reports/payables-report"})
    public String payables(Model model) { return page("payables-report",model); }
    private String page(String report,Model model) {
        model.addAttribute("reportKind",report);
        return "accounts/reports/desktop_receivables";
    }
    @GetMapping("/api/accounts/desktop-reports/{report:receivables-by-due-dates|receivables-receipt-schedule|receivables-report|payables-report}")
    @ResponseBody
    public List<Map<String,Object>> load(@PathVariable String report,@ModelAttribute DesktopReceivablesRequest request) {
        return service.load(report,request);
    }
}
