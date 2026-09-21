package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ReportConfigRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Admin Panel -> "Define Reports" (frmReportConfig, 719 lines).
 *
 * ---------------------------------------------------------------------------------------------
 * THE GATE
 * ---------------------------------------------------------------------------------------------
 * Like every Admin Panel form, frmReportConfig checks no rights itself - it has no
 * ScreenDefinition row. Its only gate is DashboardNew drawing the menu for RoleName "Admin"
 * (DashboardNew.cs:1193). A URL is reachable whether or not a link was drawn, so the same
 * comparison is made server-side here.
 *
 * ---------------------------------------------------------------------------------------------
 * THERE IS NO VALIDATION ON THIS FORM, AND THAT IS NOT AN OMISSION
 * ---------------------------------------------------------------------------------------------
 * Insert() has no FormValidation() and checks nothing: it asks the Yes/No question and saves.
 * A blank Report Title saves on the desktop today. No validation is added here, because adding
 * one would make the web refuse rows the desktop accepts.
 *
 * ---------------------------------------------------------------------------------------------
 * WHAT AN UPDATE DOES TO THE FIELDS THIS FORM HAS NO INPUT FOR
 * ---------------------------------------------------------------------------------------------
 * Insert() sets seven fields from the screen plus the four audit fields, and leaves the rest of
 * the model at its C# defaults. Sp_ReportConfig_Update assigns every column, so an update:
 *   - sets ReportSeqNo to 0            (the model's default; the INSERT computes MAX+1 instead)
 *   - blanks ReportIconURL, TargetSource, TargetFunctionName
 *   - rewrites CreatedById and CreatedOn with the CURRENT user and NOW, losing the original
 *     creation audit - the desktop assigns obj.CreatedById/CreatedOn unconditionally, not only
 *     on insert
 * Reproduced exactly, because the instruction is that both sides write the same row. Recorded in
 * the project notes so it can be decided on deliberately.
 */
@Service
public class ReportConfigService {

    private final CurrentUserContext context;
    private final ReportConfigRepository repo;

    public ReportConfigService(CurrentUserContext context, ReportConfigRepository repo) {
        this.context = context;
        this.repo = repo;
    }

    public boolean canOpen() {
        try { return "Admin".equals(context.currentRoleName()); }
        catch (RuntimeException e) { return false; }
    }

    private UserAccount admin() {
        UserAccount u = context.requireAccountingUser();
        if (!"Admin".equals(context.currentRoleName())) {
            throw new AccessDeniedException(
                    "Define Reports is on the desktop's Admin Panel, which is shown only to the Admin role.");
        }
        return u;
    }

    // ================================================================================== read

    /** GridFill() - tabControl1_SelectedIndexChanged runs it when the History tab is selected. */
    public List<Map<String, Object>> history() {
        admin();
        return repo.all();
    }

    /** RetrivedData(Id) - grdhistory_DoubleClick passes the row's ReportConfigId. */
    public Map<String, Object> byId(int id) {
        admin();
        Map<String, Object> r = repo.byId(id);
        if (r == null) throw new IllegalArgumentException("Report not found.");
        return r;
    }

    // ================================================================================== write

    /** Insert(). ActionTypeId 1 on save, 2 on update - the desktop's own values. */
    @Transactional
    public Map<String, Object> save(Map<String, Object> form) {
        UserAccount u = admin();

        int recId = ReportConfigRepository.intOf(form.get("id"));
        /* btnsave_Click sets RecId = 0 before calling Insert(), so Save always inserts. The page
           sends id 0 for Save and the loaded id for Update; an id that does not exist is refused
           rather than silently turning into an insert. */
        if (recId > 0 && repo.byId(recId) == null) {
            throw new IllegalArgumentException("Report not found.");
        }

        long userId = u.getId() == null ? 0L : u.getId();
        Timestamp now = new Timestamp(System.currentTimeMillis());

        Map<String, Object> m = ReportConfigRepository.blankModel();
        m.put("ReportConfigId",      recId);
        m.put("ReportTitle",         text(form.get("reportTitle")));
        m.put("ReportShortName",     text(form.get("reportShortName")));
        m.put("ReportFolder",        text(form.get("reportFolder")));
        m.put("ReportFileName",      text(form.get("reportFileName")));
        m.put("ReportProcedureName", text(form.get("reportProcedureName")));
        m.put("IsSubReport",         flag(form.get("isSubReport")));
        m.put("IsActive",            flag(form.get("isActive")));
        m.put("ActionTypeId",        recId == 0 ? 1 : 2);
        m.put("AlteredById",         userId);
        m.put("AlteredOn",           now);
        m.put("CreatedById",         userId);   // the desktop sets these on UPDATE too
        m.put("CreatedOn",           now);
        /* Left at the C# defaults the desktop sends, so both write identical rows:
           ReportSeqNo 0, and null for ReportIconURL, TargetSource, TargetFunctionName. */
        m.put("ReportSeqNo",         0);

        int id = repo.save(m);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("message", recId > 0 ? "Update Successfully" : "Save Successfully");
        out.put("rows", repo.all());
        return out;
    }

    private static String text(Object v) { return v == null ? "" : String.valueOf(v); }

    private static boolean flag(Object v) {
        return Boolean.TRUE.equals(v) || "true".equalsIgnoreCase(String.valueOf(v)) || "1".equals(String.valueOf(v));
    }
}
