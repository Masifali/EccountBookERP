package com.mst.controllers;

import com.mst.services.AuditLogReportService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.ResponseBody;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.function.Supplier;

/**
 * Admin Panel -> "Audit Log Report" (DashboardNew.cs:2666 btnAuditLogReport_Click -> Audit_Dashboard.AuditLogReport).
 *
 *   /admin/audit-log-report                          the form
 *   GET /api/admin/audit-log-report/load             frmGRNHistory_Load (Date Type, users, dtScreens, ActiveYr)
 *   GET /api/admin/audit-log-report/refresh          btnRefresh_Click -> AllDropDownBind
 *   GET /api/admin/audit-log-report/show             btnshow_Click -> gridHisory -> USP_UserAudit_Report
 *   GET /api/admin/audit-log-report/report-methods   Slip button -> ReportsMethod.FormHistory
 *
 * Every call is gated on RoleName == "Admin" (AuditLogReportService.admin), the desktop's only gate.
 */
@Controller
public class AuditLogReportController {

    private static final String API = "/api/admin/audit-log-report";

    @Autowired private AuditLogReportService service;

    @GetMapping("/admin/audit-log-report")
    public String page(Model model) {
        service.admin();
        model.addAttribute("activeMenu", "apps");
        return "admin/audit_log_report";
    }

    @GetMapping(API + "/load") @ResponseBody
    public ResponseEntity<?> load() { return run(service::load, "Load failed."); }

    @GetMapping(API + "/refresh") @ResponseBody
    public ResponseEntity<?> refresh() { return run(service::refresh, "Refresh failed."); }

    @GetMapping(API + "/show") @ResponseBody
    public ResponseEntity<?> show(@RequestParam(required = false) String fromDate,
                                  @RequestParam(required = false) String toDate,
                                  @RequestParam(required = false) Integer userId) {
        return run(() -> service.show(fromDate, toDate, userId), "Show failed.");
    }

    @GetMapping(API + "/report-methods") @ResponseBody
    public ResponseEntity<?> reportMethods() { return run(service::reportMethods, "Load failed."); }

    // ------------------------------------------------------------------ plumbing

    private static ResponseEntity<?> run(Supplier<Object> call, String fallback) {
        try {
            return ResponseEntity.ok(call.get());
        } catch (IllegalArgumentException | IllegalStateException e) {
            return ResponseEntity.badRequest().body(fail(root(e, fallback)));
        } catch (AccessDeniedException e) {
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(fail(root(e, "Access denied.")));
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(fail(root(e, fallback)));
        }
    }

    /** The desktop shows ex.Message - for a RAISERROR that is the procedure's own text. */
    private static String root(Throwable e, String fallback) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        String m = t.getMessage();
        if (m == null || m.trim().isEmpty()) m = e.getMessage();
        return m == null || m.trim().isEmpty() ? fallback : m;
    }

    private static Map<String, Object> fail(String message) {
        Map<String, Object> r = new LinkedHashMap<>();
        r.put("success", false);
        r.put("message", message);
        return r;
    }
}
