package com.mst.controllers;

import com.mst.services.AccountLedgerReportsDesktopService;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.*;

/**
 * Account Reports (module 3), group R1 2026-09-30: Party Ledger (screen 49) and General Ledger Statement endpoints on
 * the reports-desktop base path (see AccountLedgerReportsDesktopService). Filter values arrive in the body; tenancy never
 * does. Errors come back as {success:false, message} with the desktop's own message text.
 */
@RestController
@RequestMapping("/accounts/api/reports-desktop")
public class AccountLedgerReportsDesktopRestController {

    @Autowired
    private AccountLedgerReportsDesktopService service;

    private static Integer i(Object v) {
        if (v == null) return null;
        String s = v.toString().trim();
        if (s.isEmpty() || "null".equals(s)) return null;
        try { return (int) Double.parseDouble(s); } catch (NumberFormatException e) { return null; }
    }

    private static String s(Object v) {
        if (v == null) return null;
        String t = v.toString().trim();
        return t.isEmpty() ? null : t;
    }

    private static ResponseEntity<?> ok(Object data) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("data", data);
        return ResponseEntity.ok(m);
    }

    private static ResponseEntity<?> fail(Exception e) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", false);
        String msg = e.getMessage();
        Throwable c = e;
        while (c.getCause() != null && c.getCause() != c) c = c.getCause();
        if (c != e && c.getMessage() != null) msg = c.getMessage();
        m.put("message", msg == null ? e.getClass().getSimpleName() : msg);
        int status = (e instanceof SecurityException) ? 403 : ((e instanceof IllegalArgumentException || e instanceof IllegalStateException) ? 200 : 500);
        return ResponseEntity.status(status).body(m);
    }

    /** CustomerLedger.btnshow_Click -> CustomerLedgerFill. Body: supplierCustomerId, fromDate, toDate. */
    @PostMapping("/customer-ledger")
    public ResponseEntity<?> partyLedger(@RequestBody Map<String, Object> r) {
        try { return ok(service.partyLedger(i(r.get("supplierCustomerId")), s(r.get("fromDate")), s(r.get("toDate")))); }
        catch (Exception e) { return fail(e); }
    }

    /** GeneralLedgerStatment.btnshow_Click. Body: accountId, fromDate, toDate, branchId, projectId. */
    @PostMapping("/general-ledger-statement")
    public ResponseEntity<?> generalLedgerStatement(@RequestBody Map<String, Object> r) {
        try {
            return ok(service.generalLedgerStatement(i(r.get("accountId")), s(r.get("fromDate")), s(r.get("toDate")),
                    i(r.get("branchId")), i(r.get("projectId"))));
        } catch (Exception e) { return fail(e); }
    }

    /** GeneralLedgerStatment combos: companies (CompanyServiceBind), branches (BrancheServiceBind), coa (CoaAllocationGetForComboServiceBind). */
    @GetMapping("/ledger-lookups/{name}")
    public ResponseEntity<?> lookup(@PathVariable("name") String name) {
        try {
            switch (name) {
                case "companies": return ok(service.companies());
                case "branches": return ok(service.branchesAll());
                case "coa": return ok(service.coaForCombo());
                default: return fail(new IllegalArgumentException("Unknown lookup " + name));
            }
        } catch (Exception e) { return fail(e); }
    }
}
