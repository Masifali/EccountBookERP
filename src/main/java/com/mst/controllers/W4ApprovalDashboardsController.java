package com.mst.controllers;

import com.mst.services.W4ApprovalDashboardsService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * 364 Approval Dashboard (New)     - Architecture.WinApp.ApprovalDashboard\PendingDocumentForMultipleApprovals.cs
 * 368 Approval Dashboard Steel     - Architecture.WinApp.Steel.Approvals\ApprovalDashboard_St.cs
 *
 * The last path segments normalise to the desktop class names ("pendingdocumentformultipleapprovals",
 * "approvaldashboardst"), so ScreenRouteIndex resolves them without a hand-written entry.
 */
@Controller
public class W4ApprovalDashboardsController {

    @Autowired
    private W4ApprovalDashboardsService service;

    @GetMapping("/dashboard/pending-document-for-multiple-approvals")
    public String multi(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("w4Mode", "multi");
        model.addAttribute("w4Title", "Approval Dashboard (New)");
        return "dashboard/w4_approval_dashboards";
    }

    @GetMapping("/dashboard/approval-dashboard-st")
    public String steel(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("w4Mode", "steel");
        model.addAttribute("w4Title", "Approval Dashboard Steel");
        return "dashboard/w4_approval_dashboards";
    }

    /** PendingDocumentForMultipleApprovals.DynamicallyGenerateCards / Refresh_Click. */
    @GetMapping("/api/dashboard/w4-approvals/pending-documents")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> pendingDocuments() {
        return ResponseEntity.ok(service.pendingDocuments());
    }

    /** ApprovalDashboard_St.DynamicallyGenerateCards / Refresh_Click. */
    @GetMapping("/api/dashboard/w4-approvals/steel")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> steel() {
        return ResponseEntity.ok(service.steel());
    }
}
