package com.mst.controllers;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.ResponseEntity;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.web.bind.annotation.*;

import java.io.File;
import java.util.*;

/**
 * Trial Balance family (screens 51 TrialBalance, 81 SelectedTrialBalance, 82 frmTrialBalancesAllLevel) -
 * the two desktop print sources the pages had no endpoint for (2026-10-02, group L-TRIAL-BALANCE-FAMILY):
 *
 *   GET  /accounts/api/reports-desktop/tb-family/dynamic-reports?folder=SelectedTrialBalance
 *        CommonServices.DynamicReportsLoad(FolderName): the .rpt files of <report root>\<FolderName>
 *        (Directory.GetFiles(path, "*rpt")), shown by file name without extension as the items of the
 *        print drop-downs: TrialBalance.tsDrowdownTrial ("TrialBalance"), TrialBalance/SelectedTrialBalance
 *        .tsDropDownLevelWise ("Trial Balance Level Wise"), SelectedTrialBalance.tsDropDownSelectedTrial
 *        ("SelectedTrialBalance"), frmTrialBalancesAllLevel.tsDropDown 135-Print ("HararicalTrialBalance").
 *        The report root is reports.crystal.template-root (the desktop's TPSSReportDb folder). Only these
 *        four folder names are accepted. A missing folder lists nothing (the desktop creates it and lists nothing).
 *
 *   POST /accounts/api/reports-desktop/tb-family/level-wise-rows {fromDate, toDate, languageId}
 *        SelectedTrialBalance.tsDropDownLevelWise_DropDownItemClicked -> VoucherReports.TrialBalanceAllLevels
 *        -> Sp_Accounts_TrialBalancesAllLevels_Rpt: @OrganizationId, @CompanyId, @FinancialYearId always;
 *        @UserId when != 0; @FromDate / @ToDate when set; @LanguageId when != 0. (The form also sets
 *        ZeroBalanceType, but the BLL method never adds it.) The page prints the rows into the clicked .rpt.
 *
 * Tenancy, user and financial year come from CurrentUserContext only.
 */
@RestController
@RequestMapping("/accounts/api/reports-desktop/tb-family")
public class TrialBalanceFamilyRestController {

    private static final Set<String> FOLDERS = new LinkedHashSet<>(Arrays.asList(
            "TrialBalance", "Trial Balance Level Wise", "SelectedTrialBalance", "HararicalTrialBalance"));

    @Autowired private JdbcTemplate jdbc;
    @Autowired private CurrentUserContext ctx;

    @Value("${reports.crystal.template-root:}")
    private String reportRoot;

    @GetMapping("/dynamic-reports")
    public ResponseEntity<?> dynamicReports(@RequestParam("folder") String folder) {
        try {
            ctx.currentUserId();
            if (!FOLDERS.contains(folder)) throw new IllegalArgumentException("Unknown report folder " + folder);
            List<String> out = new ArrayList<>();
            if (reportRoot != null && !reportRoot.isBlank()) {
                File[] files = new File(reportRoot, folder).listFiles();
                if (files != null) {
                    Arrays.sort(files, Comparator.comparing(f -> f.getName().toLowerCase(Locale.ROOT)));
                    for (File f : files) {
                        String n = f.getName();
                        if (!f.isFile() || !n.toLowerCase(Locale.ROOT).endsWith("rpt")) continue;
                        int dot = n.lastIndexOf('.');
                        out.add(dot > 0 ? n.substring(0, dot) : n);       // Path.GetFileNameWithoutExtension
                    }
                }
            }
            return ok(out);
        } catch (Exception e) { return fail(e); }
    }

    @PostMapping("/level-wise-rows")
    public ResponseEntity<?> levelWiseRows(@RequestBody Map<String, Object> r) {
        try {
            int user = ctx.currentUserId();
            Integer lang = i(r.get("languageId"));
            return ok(DesktopProc.rows(jdbc, "Sp_Accounts_TrialBalancesAllLevels_Rpt", DesktopProc.params(
                    "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                    "FinancialYearId", ctx.currentFinancialYearId(),
                    "UserId", user != 0 ? user : null,
                    "FromDate", date(r.get("fromDate")), "ToDate", date(r.get("toDate")),
                    "LanguageId", lang != null && lang != 0 ? lang : null)));
        } catch (Exception e) { return fail(e); }
    }

    // ------------------------------------------------------------------ helpers

    private static Integer i(Object v) {
        if (v == null) return null;
        String s = v.toString().trim();
        if (s.isEmpty() || "null".equals(s)) return null;
        try { return (int) Double.parseDouble(s); } catch (NumberFormatException e) { return null; }
    }

    private static java.sql.Date date(Object v) {
        if (v == null) return null;
        String t = v.toString().trim();
        if (t.isEmpty()) return null;
        if (t.length() > 10) t = t.substring(0, 10);
        return java.sql.Date.valueOf(t);
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
        Throwable c = e;
        while (c.getCause() != null && c.getCause() != c) c = c.getCause();
        String msg = c.getMessage() != null ? c.getMessage() : e.getMessage();
        m.put("message", msg == null ? e.getClass().getSimpleName() : msg);
        int status = (e instanceof SecurityException || e instanceof org.springframework.security.access.AccessDeniedException) ? 403 : (e instanceof IllegalArgumentException ? 400 : 500);
        return ResponseEntity.status(status).body(m);
    }
}
