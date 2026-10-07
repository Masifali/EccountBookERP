package com.mst.controllers;

import com.mst.models.UserAccount;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import com.mst.services.CoaAllocationUpdateService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Supplier;

/**
 * Screen 6 "Update COA Allocation" (Architecture.WinApp.Reconciliation.COAAllocation).
 *   GET  /accounts/reconciliation/update-coa-allocation              the form
 *   GET  /api/accounts/coa-allocation/load                           DayBookVoucher_Load (both combos)
 *   GET  /api/accounts/coa-allocation/rows?parentAccountCode=        btnSearch_Click
 *   GET  /api/accounts/coa-allocation/account/{id}                   CmbAccountTitle_Leave
 *   POST /api/accounts/coa-allocation/update-account                 btnAccountUpdate_Click
 *   POST /api/accounts/coa-allocation/update-rows                    btnParentUpdate_Click
 * The desktop checks no Save/Update right (opening the screen is the right), so the screen's View right (6) is enforced.
 */
@Controller
public class CoaAllocationUpdateController {
    private static final int SCREEN_ID = 6;
    private static final String API = "/api/accounts/coa-allocation";

    @Autowired private CoaAllocationUpdateService service;
    @Autowired private CurrentUserContext context;
    @Autowired private DesktopReportRights rights;

    private void gate() {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
    }

    @GetMapping("/accounts/reconciliation/update-coa-allocation")
    public String page(Model model) {
        gate();
        model.addAttribute("activeMenu", "accounts");
        model.addAttribute("moduleTitle", "COA Allocation");
        return "accounts/update_coa_allocation";
    }

    @GetMapping(API + "/load") @ResponseBody
    public ResponseEntity<?> load() { return run(() -> { gate(); return service.load(); }); }

    @GetMapping(API + "/rows") @ResponseBody
    public ResponseEntity<?> rows(@RequestParam(required = false) String parentAccountCode) {
        return run(() -> { gate(); return service.rows(parentAccountCode); });
    }

    @GetMapping(API + "/account/{id}") @ResponseBody
    public ResponseEntity<?> account(@PathVariable int id) { return run(() -> { gate(); return service.pageNoAndStatus(id); }); }

    @PostMapping(API + "/update-account") @ResponseBody
    public ResponseEntity<?> updateAccount(@RequestBody Map<String, Object> r) {
        return run(() -> {
            gate();
            service.updateAccount(num(r.get("chartofAccountId")), str(r.get("glPageNo")), Boolean.parseBoolean(String.valueOf(r.get("isActive"))));
            return "Record Update Successfully";
        });
    }

    @PostMapping(API + "/update-rows") @ResponseBody
    @SuppressWarnings("unchecked")
    public ResponseEntity<?> updateRows(@RequestBody Map<String, Object> r) {
        return run(() -> {
            gate();
            service.updateRows(str(r.get("parentAccountCode")), (List<Map<String, Object>>) r.get("rows"));
            return "Record Update Successfully";
        });
    }

    private static int num(Object o) {
        if (o == null || o.toString().isBlank()) return 0;
        try { return (int) Double.parseDouble(o.toString().trim()); } catch (NumberFormatException e) { return 0; }
    }

    private static String str(Object o) { return o == null ? null : o.toString(); }

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
