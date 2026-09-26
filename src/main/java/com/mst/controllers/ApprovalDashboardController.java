package com.mst.controllers;

import com.mst.services.ApprovalDashboardService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Approvals (Dashboard) - Architecture.WinApp.ApprovalDashboard\ApprovalDashboard.cs.
 *
 * The path's last segment normalises to "approvaldashboard", the TargetUrl class name, so the
 * DashBoard card links here on its own. With Undo Approval already built, this completes the
 * "Approvals && Undo Approvals DashBoards" module.
 */
@Controller
public class ApprovalDashboardController {

    @Autowired
    private ApprovalDashboardService service;

    @GetMapping("/dashboard/approval-dashboard")
    public String page(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Approval Dashboard");
        return "dashboard/approval_dashboard";
    }

    /** ApprovalDashboard_Load, :105-130. */
    @GetMapping("/api/dashboard/approvals/setup")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> setup() {
        return ResponseEntity.ok(service.setup());
    }

    /** DynamicallyGenerateCards(), :159-242. */
    @GetMapping("/api/dashboard/approvals/cards")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> cards(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(defaultValue = "0") int branchId) {
        return ResponseEntity.ok(service.cards(fromDate, toDate, branchId));
    }
}
