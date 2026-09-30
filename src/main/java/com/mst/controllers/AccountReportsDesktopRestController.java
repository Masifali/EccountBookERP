package com.mst.controllers;

import com.mst.services.AccountReportsDesktopService;

import javax.servlet.http.HttpSession;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.*;

/**
 * Account Reports (module 3) - desktop-exact endpoints used by the rechecked report pages
 * (see AccountReportsDesktopService). Every filter value arrives in the body; tenancy never does.
 * Errors come back as {success:false, message} with the desktop's own message text.
 */
@RestController
@RequestMapping("/accounts/api/reports-desktop")
public class AccountReportsDesktopRestController {

    private static final String GL_DETAIL_IDS = "accReportsDesktop.gl.detailIds";

    @Autowired
    private AccountReportsDesktopService service;

    // ------------------------------------------------------------------ helpers

    private static Integer i(Object v) {
        if (v == null) return null;
        String s = v.toString().trim();
        if (s.isEmpty() || "null".equals(s)) return null;
        try { return (int) Double.parseDouble(s); } catch (NumberFormatException e) { return null; }
    }

    private static double d(Object v) {
        if (v == null) return 0.0;
        String s = v.toString().trim().replace(",", "");
        if (s.isEmpty()) return 0.0;
        try { return Double.parseDouble(s); } catch (NumberFormatException e) { return 0.0; }
    }

    private static String s(Object v) {
        if (v == null) return null;
        String t = v.toString().trim();
        return t.isEmpty() ? null : t;
    }

    /** Multi-select values may arrive as a JSON array or a CSV string. */
    private static String csv(Object v) {
        if (v == null) return null;
        if (v instanceof Collection) {
            List<String> parts = new ArrayList<>();
            for (Object o : (Collection<?>) v) if (o != null && !o.toString().isBlank()) parts.add(o.toString().trim());
            return parts.isEmpty() ? null : String.join(",", parts);
        }
        return s(v);
    }

    private static boolean b(Object v) {
        return v != null && Boolean.parseBoolean(v.toString());
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

    // ------------------------------------------------------------------ form context + lookups

    @GetMapping("/context")
    public ResponseEntity<?> context() {
        try { return ok(service.formContext()); } catch (Exception e) { return fail(e); }
    }

    @GetMapping("/general-ledger/config")
    public ResponseEntity<?> generalLedgerConfig() {
        try { return ok(service.generalLedgerConfig()); } catch (Exception e) { return fail(e); }
    }

    @GetMapping("/lookups/{name}")
    public ResponseEntity<?> lookup(@PathVariable("name") String name,
                                    @RequestParam(value = "costCenterId", required = false) Integer costCenterId,
                                    @RequestParam(value = "accountId", required = false) Integer accountId,
                                    @RequestParam(value = "accountTypeId", required = false) Integer accountTypeId) {
        try {
            switch (name) {
                case "languages": return ok(service.languages());
                case "cost-centers": return ok(service.costCenters());
                case "detail-accounts": return ok(service.detailAccounts(costCenterId));
                case "subsidiary-accounts": return ok(service.subsidiaryAccounts(costCenterId, accountId));
                case "branches": return ok(service.branches());
                case "document-types": return ok(service.documentTypes());
                case "custom-groups": return ok(service.customGroups());
                case "account-groups": return ok(service.accountGroups());
                case "cities": return ok(service.cities());
                case "coa-allocation": return ok(service.coaAllocation());
                case "supplier-customers": return ok(service.supplierCustomers());
                case "last-record-date": return ok(service.lastRecordDate(accountTypeId == null ? 0 : accountTypeId));
                default: return fail(new IllegalArgumentException("Unknown lookup " + name));
            }
        } catch (Exception e) { return fail(e); }
    }

    @PostMapping("/account-info")
    public ResponseEntity<?> accountInfo(@RequestBody Map<String, Object> r) {
        try { return ok(service.accountInfo(i(r.get("accountId")), s(r.get("fromDate")), s(r.get("toDate")), i(r.get("costCenterId")))); }
        catch (Exception e) { return fail(e); }
    }

    // ------------------------------------------------------------------ 79 General Ledger

    @PostMapping("/general-ledger")
    public ResponseEntity<?> generalLedger(@RequestBody Map<String, Object> r, HttpSession session) {
        try {
            String mode = s(r.get("mode"));
            Map<String, Object> res = service.generalLedger(mode == null ? "detail" : mode, i(r.get("accountId")),
                    s(r.get("fromDate")), s(r.get("toDate")), b(r.get("includeUnposted")), i(r.get("languageId")),
                    i(r.get("subsidiaryAccountId")), i(r.get("subsidiaryTypeId")), i(r.get("branchId")), i(r.get("costCenterId")));
            if ("detail".equals(res.get("mode"))) {
                Set<Integer> ids = new HashSet<>();
                Object rowsObj = res.get("rows");
                if (rowsObj instanceof List) for (Object o : (List<?>) rowsObj) {
                    Integer id = i(((Map<?, ?>) o).get("DetailId"));
                    if (id != null && id > 0) ids.add(id);
                }
                session.setAttribute(GL_DETAIL_IDS, ids);
            }
            return ok(res);
        } catch (Exception e) { return fail(e); }
    }

    @SuppressWarnings("unchecked")
    @PostMapping("/general-ledger/bookmarks")
    public ResponseEntity<?> saveBookmarks(@RequestBody Map<String, Object> r, HttpSession session) {
        try {
            Object items = r.get("rows");
            List<Map<String, Object>> list = items instanceof List ? (List<Map<String, Object>>) items : Collections.emptyList();
            Set<Integer> allowed = (Set<Integer>) session.getAttribute(GL_DETAIL_IDS);
            int n = service.saveBookmarks(list, allowed);
            return ok(Map.of("saved", n, "message", "Bookmark Status Saved Successfully"));
        } catch (Exception e) { return fail(e); }
    }

    // ------------------------------------------------------------------ 51 / 81 / 82 Trial balances

    @PostMapping("/trial-balance")
    public ResponseEntity<?> trialBalance(@RequestBody Map<String, Object> r) {
        try {
            return ok(service.trialBalance(s(r.get("fromDate")), s(r.get("toDate")), b(r.get("skipZero")),
                    d(r.get("clDebit")), d(r.get("clCredit")), b(r.get("includeUnposted")), i(r.get("languageId")),
                    csv(r.get("documentTypeIds")), csv(r.get("branchesIds")), csv(r.get("accountTypeIds")),
                    i(r.get("customGroupId")), i(r.get("groupAccountId")), b(r.get("skipCgs"))));
        } catch (Exception e) { return fail(e); }
    }

    @PostMapping("/selected-trial-balance")
    public ResponseEntity<?> selectedTrialBalance(@RequestBody Map<String, Object> r) {
        try {
            return ok(service.selectedTrialBalance(s(r.get("fromDate")), s(r.get("toDate")), d(r.get("clDebit")),
                    d(r.get("clCredit")), i(r.get("groupAccountId")), i(r.get("cityId")), b(r.get("includeUnposted")),
                    i(r.get("languageId")), i(r.get("customGroupId")), b(r.get("skipZero")), b(r.get("skipCgs")),
                    csv(r.get("documentTypeIds")), csv(r.get("branchesIds"))));
        } catch (Exception e) { return fail(e); }
    }

    @PostMapping("/trial-balance-all-levels")
    public ResponseEntity<?> trialBalanceAllLevels(@RequestBody Map<String, Object> r) {
        try { return ok(service.trialBalanceAllLevels(s(r.get("fromDate")), s(r.get("toDate")), b(r.get("skipZero")))); }
        catch (Exception e) { return fail(e); }
    }

    // ------------------------------------------------------------------ 53 Voucher Report

    @PostMapping("/voucher-report")
    public ResponseEntity<?> voucherReport(@RequestBody Map<String, Object> r) {
        try {
            return ok(service.voucherReport(s(r.get("dateFilter")), s(r.get("fromDate")), s(r.get("toDate")),
                    i(r.get("fromDocNo")), i(r.get("toDocNo")), i(r.get("accountId")), i(r.get("customGroupId")),
                    s(r.get("manualBillNo")), s(r.get("approval")), b(r.get("skipCgs")), i(r.get("languageId")),
                    csv(r.get("documentTypeIds"))));
        } catch (Exception e) { return fail(e); }
    }

    // ------------------------------------------------------------------ 52 Chart of Account

    @PostMapping("/chart-of-accounts")
    public ResponseEntity<?> chartOfAccounts(@RequestBody Map<String, Object> r) {
        try {
            return ok(service.chartOfAccounts(i(r.get("accountId")), csv(r.get("customGroupIds")), s(r.get("status")),
                    s(r.get("accountLevels"))));
        } catch (Exception e) { return fail(e); }
    }

    @PostMapping("/chart-of-accounts/other-language-title")
    public ResponseEntity<?> saveOtherLanguageTitle(@RequestBody Map<String, Object> r) {
        try {
            Object t = r.get("title");
            service.saveOtherLanguageTitle(i(r.get("chartOfAccountId")), i(r.get("languageId")), t == null ? null : t.toString());
            return ok(Map.of("message", "Record Update Successfully...;"));
        } catch (Exception e) { return fail(e); }
    }

    // ------------------------------------------------------------------ 69 Activity Summary

    @PostMapping("/activity-summary")
    public ResponseEntity<?> activitySummary(@RequestBody Map<String, Object> r) {
        try {
            return ok(service.activitySummary(s(r.get("dateFilter")), s(r.get("fromDate")), s(r.get("toDate")),
                    b(r.get("includeUnposted")), i(r.get("reportTypeId")), csv(r.get("documentTypeIds")),
                    csv(r.get("branchesIds")), i(r.get("languageId"))));
        } catch (Exception e) { return fail(e); }
    }

    // ------------------------------------------------------------------ 73 / 74 Bank and Cash balances

    @PostMapping("/bank-balances")
    public ResponseEntity<?> bankBalances(@RequestBody Map<String, Object> r) {
        try {
            return ok(service.bankBalances(s(r.get("fromDate")), s(r.get("toDate")), csv(r.get("branchesIds")),
                    i(r.get("languageId")), b(r.get("excludeZero")), r.get("withDetail") == null || b(r.get("withDetail"))));
        } catch (Exception e) { return fail(e); }
    }

    @PostMapping("/cash-balances")
    public ResponseEntity<?> cashBalances(@RequestBody Map<String, Object> r) {
        try {
            return ok(service.cashBalances(s(r.get("fromDate")), s(r.get("toDate")), i(r.get("accountId")),
                    csv(r.get("branchesIds")), i(r.get("languageId")), r.get("withDetail") == null || b(r.get("withDetail"))));
        } catch (Exception e) { return fail(e); }
    }
}
