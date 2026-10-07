package com.mst.controllers;

import com.mst.models.UserAccount;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import com.mst.services.CapitalOwnerEquityService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.Map;
import java.util.function.Supplier;

/**
 * Screen 13 "Capital Owner Equity Report" (Architecture.WinApp.Account_Reports.CapitalOwnerEquityReport).
 *   GET  /accounts/reports/capital-owner-equity                 the form
 *   GET  /api/accounts/capital-owner-equity/load                VoucherValidation_Load / btnRefresh (AccountTitleFill)
 *   POST /api/accounts/capital-owner-equity/show                btnShowSelectedTrial_Click
 * Print-120A posts the shown rows (dtHeader) to POST /reports/print/grid with 120A-CapitalOwnerEquityReport.rpt.
 * Opening the screen is the right the desktop checks (screen 13, View).
 */
@Controller
public class CapitalOwnerEquityController {
    private static final int SCREEN_ID = 13;
    private static final String API = "/api/accounts/capital-owner-equity";

    @Autowired private CapitalOwnerEquityService service;
    @Autowired private CurrentUserContext context;
    @Autowired private DesktopReportRights rights;

    private void gate() {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
    }

    @GetMapping("/accounts/reports/capital-owner-equity")
    public String page(Model model) {
        gate();
        model.addAttribute("activeMenu", "accounts");
        model.addAttribute("moduleTitle", "Capital Owner Equity Report");
        return "accounts/reports/capital_owner_equity";
    }

    @GetMapping(API + "/load") @ResponseBody
    public ResponseEntity<?> load() { return run(() -> { gate(); return service.load(); }); }

    @PostMapping(API + "/show") @ResponseBody
    public ResponseEntity<?> show(@RequestBody Map<String, Object> r) {
        return run(() -> {
            gate();
            Object a = r.get("accountId");
            Integer id = null;
            try { if (a != null && !a.toString().isBlank()) id = (int) Double.parseDouble(a.toString().trim()); } catch (NumberFormatException e) { id = null; }
            return service.show(id, r.get("toDate") == null ? null : r.get("toDate").toString(),
                    Boolean.parseBoolean(String.valueOf(r.get("skipZero"))), r.get("profit") == null ? "" : r.get("profit").toString());
        });
    }

    private ResponseEntity<?> run(Supplier<Object> work) {
        Map<String, Object> m = new LinkedHashMap<>();
        try {
            m.put("success", true);
            m.put("data", work.get());
            return ResponseEntity.ok(m);
        } catch (AccessDeniedException e) {
            m.clear(); m.put("success", false); m.put("message", e.getMessage());
            return ResponseEntity.status(HttpStatus.FORBIDDEN).body(m);
        } catch (Exception e) {
            Throwable c = e;
            while (c.getCause() != null && c.getCause() != c) c = c.getCause();
            m.clear(); m.put("success", false); m.put("message", c.getMessage() == null ? e.getClass().getSimpleName() : c.getMessage());
            boolean bad = e instanceof IllegalArgumentException || e instanceof IllegalStateException;
            return ResponseEntity.status(bad ? HttpStatus.OK : HttpStatus.INTERNAL_SERVER_ERROR).body(m);
        }
    }
}
