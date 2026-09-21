package com.mst.controllers;

import com.mst.services.PendingWorksDashboardService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.Map;

/**
 * Purchase &amp; Sales Pending Works (Dashboard) -
 * Architecture.WinApp.Dashboard\frmPendingWorksRpt.cs. The only screen in the
 * "Followups &amp;&amp; Pending Works DashBoards" module.
 *
 * The path's last segment normalises to "pendingworksrpt", which is what the TargetUrl class
 * frmPendingWorksRpt reduces to once ScreenRouteIndex strips its "frm" prefix, so the DashBoard
 * card links here on its own.
 */
@Controller
public class PendingWorksDashboardController {

    @Autowired
    private PendingWorksDashboardService service;

    @GetMapping("/dashboard/pending-works-rpt")
    public String page(Model model) {
        model.addAttribute("activeMenu", "dashboard");
        model.addAttribute("moduleTitle", "Purchase & Sales Pending Works");
        return "dashboard/pending_works_rpt";
    }

    /** BranchesFill() plus ERP feature 11 (form :99-136). */
    @GetMapping("/api/dashboard/pending-works/setup")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> setup() {
        return ResponseEntity.ok(service.setup());
    }

    /** AllCardsBind(), form :139-282 - @ReportType = 'FiguresOnly'. */
    @GetMapping("/api/dashboard/pending-works/cards")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> cards(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(defaultValue = "1") int typeId,
            @RequestParam(required = false) String branchIds) {
        return ResponseEntity.ok(service.cards(fromDate, toDate, typeId, branchIds));
    }

    /**
     * GetDetailPendingWorkForPurchase(), form :351-369 -
     * @ReportType = 'DetailByPendingWorkCaption' with the clicked card's department and caption.
     */
    @GetMapping("/api/dashboard/pending-works/detail")
    @ResponseBody
    public ResponseEntity<Map<String, Object>> detail(
            @RequestParam(required = false) String fromDate,
            @RequestParam(required = false) String toDate,
            @RequestParam(defaultValue = "1") int typeId,
            @RequestParam(required = false) String branchIds,
            @RequestParam String department,
            @RequestParam String pendingWorkName) {
        return ResponseEntity.ok(service.detail(fromDate, toDate, typeId, branchIds,
                                                department, pendingWorkName));
    }
}
