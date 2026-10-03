package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

/**
 * Admin Panel -> "PL &amp; BS Notes" - DashboardNew.btnPlBSNotes_Click (:2740) opens
 * Architecture.WinApp.Account_Definition.BsPlSettingForm(UserAccount).
 *
 * ---------------------------------------------------------------------------------------------
 * PROCEDURES - exactly the four calls the desktop BLL makes
 * ---------------------------------------------------------------------------------------------
 * Tab "Change Notes For Group Accounts"
 *   GridLoad()      PLBSSetting.GetAllForBsPlSetting -> Sp_PLBSSetting_GetAllMethod
 *                   @OrganizationId, @CompanyId, @Note (ReqType 'PL'/'BS'), @AccountClassId only
 *                   when non-zero (2 Assets / 3 Liabilities, BS only), @Activity='GetAllForBsPlSetting'
 *   GridSettings()  AccountNotes.ReadByPLNote -> SP_AccountNotes_ReadAllMethodBySPType
 *                   @AccountNotesType='ReadAllMethodByPLNote' (always called);
 *                   AccountNotes.ReadByBSNote(2|3) -> same proc, 'ReadAllMethodByBSNote', @AccountClassId
 *                   (only when Assets / Liabilities is checked)
 *   btnUpdate       PLBSSetting.Save -> DAL SetData: ONE transaction, Sp_PLBSSetting_Insert per row via
 *                   GenericProvider.SetProc, which binds every non-virtual model property (11 params).
 * Tab "Change Note Title"
 *   GridNoteTitleFill  AccountNotes.ReadAllNotes(Note) -> 'ReadAllNotes', @Note only when 'PL'/'BS'
 *   btnUpdateNote      AccountNotes.UpdateTitleById -> 'UpdateTitleById' @Id, @Note per row, each on its
 *                      own connection (GetDataTableProc) - NO transaction, as on the desktop.
 *
 * Gate: the form has no ScreenDefinition row and checks no right; it is reachable only through
 * Admin Panel, which DashboardNew draws only for RoleName "Admin" (DashboardNew.cs:1191). The same
 * comparison is made server-side for every call.
 */
@Service
public class PlBsNotesService {

    private static final String P_SETTING = "Sp_PLBSSetting_GetAllMethod";
    private static final String P_INSERT = "Sp_PLBSSetting_Insert";
    private static final String P_NOTES = "SP_AccountNotes_ReadAllMethodBySPType";

    private final CurrentUserContext context;
    private final JdbcTemplate jdbc;

    public PlBsNotesService(CurrentUserContext context, JdbcTemplate jdbc) {
        this.context = context;
        this.jdbc = jdbc;
    }

    public boolean canOpen() {
        try { return "Admin".equals(context.currentRoleName()); }
        catch (RuntimeException e) { return false; }
    }

    private UserAccount admin() {
        UserAccount u = context.requireAccountingUser();
        if (!"Admin".equals(context.currentRoleName())) {
            throw new AccessDeniedException(
                    "PL & BS Notes is on the desktop's Admin Panel, which is shown only to the Admin role.");
        }
        return u;
    }

    /* ------------------------------------------------------------------ tab 1: GridLoad + GridSettings */

    /**
     * GridLoad() then GridSettings(). {@code note} is ReqType ("BS" when radBS is checked, else "PL");
     * {@code classId} is AccouuntClassId (2 Assets / 3 Liabilities, 0 when the two are hidden).
     */
    public Map<String, Object> accounts(String note, int classId) {
        admin();
        String reqType = "BS".equals(note) ? "BS" : "PL";
        List<Map<String, Object>> raw = readAccounts(reqType, classId);

        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            /* table.Rows.Add(PlBsId, Id, AccountTitle, NoteTitle, NoteRole, AccountClass, ClassName) */
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("PlBsId", r.get("PlBsId"));
            m.put("ChartofAccountId", r.get("Id"));
            m.put("AccountTitle", r.get("AccountTitle"));
            m.put("NoteTitle", r.get("NoteTitle"));
            m.put("NoteRole", r.get("NoteRole"));
            m.put("AccountClassId", r.get("AccountClass"));
            m.put("ClassName", r.get("ClassName"));
            rows.add(m);
        }

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        /* GridSettings() runs only when GridLoad bound rows (dtchartofaccount.Rows.Count > 0). */
        if (!raw.isEmpty()) {
            List<Map<String, Object>> bsNotes = new ArrayList<>();
            if (classId == 2 || classId == 3) {
                bsNotes = DesktopProc.rows(jdbc, P_NOTES, DesktopProc.params(
                        "AccountNotesType", "ReadAllMethodByBSNote", "AccountClassId", classId));
            }
            List<Map<String, Object>> plNotes = DesktopProc.rows(jdbc, P_NOTES, DesktopProc.params(
                    "AccountNotesType", "ReadAllMethodByPLNote"));
            /* radPL.Checked -> caption "PLNotes", value list dtPLNotes; else "BSNotes", dtBSNotes */
            boolean pl = !"BS".equals(reqType);
            out.put("noteCaption", pl ? "PLNotes" : "BSNotes");
            out.put("valueList", valueList(pl ? plNotes : bsNotes));
        }
        return out;
    }

    private List<Map<String, Object>> readAccounts(String reqType, int classId) {
        return DesktopProc.rows(jdbc, P_SETTING, DesktopProc.params(
                "OrganizationId", context.currentOrganizationId(),
                "CompanyId", context.currentCompanyId(),
                "Note", reqType,
                "AccountClassId", classId != 0 ? classId : null,   /* only when != 0 */
                "Activity", "GetAllForBsPlSetting"));
    }

    private static List<Map<String, Object>> valueList(List<Map<String, Object>> notes) {
        List<Map<String, Object>> vl = new ArrayList<>();
        for (Map<String, Object> n : notes) {          /* PopulateValueList(..., "Id", "NoteTitle") */
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", n.get("Id"));
            m.put("NoteTitle", n.get("NoteTitle"));
            vl.add(m);
        }
        return vl;
    }

    /**
     * btnUpdate_Click. For each loaded row: when ToInt(cell) != 0 and != ToInt(loaded NoteTitle) a
     * PLBSSetting is added with BSNoteId/PLNoteId from the radio checked AT UPDATE TIME (mode), as
     * the desktop reads radBS/radPL there, not at load.
     *
     * The "loaded" rows (the desktop's dtchartofaccount) are re-read here with the filter they were
     * loaded with, so a ChartofAccount id outside the signed-in company can never be written.
     */
    @Transactional
    public Map<String, Object> updateAccounts(String mode, String loadNote, int loadClassId,
                                              List<Map<String, Object>> cells) {
        UserAccount u = admin();
        String reqType = "BS".equals(loadNote) ? "BS" : "PL";
        List<Map<String, Object>> loaded = readAccounts(reqType, loadClassId);
        Map<Integer, Object> loadedTitle = new HashMap<>();
        for (Map<String, Object> r : loaded) loadedTitle.put(toInt(r.get("Id")), r.get("NoteTitle"));

        Set<Integer> noteIds = new HashSet<>();
        for (Map<String, Object> n : DesktopProc.rows(jdbc, P_NOTES, DesktopProc.params("AccountNotesType", "ReadAllNotes"))) {
            noteIds.add(toInt(n.get("Id")));
        }

        int org = context.currentOrganizationId();
        int company = context.currentCompanyId();
        int year = context.currentFinancialYearId();            /* clsGlobalVariables.ActiveYr.Id */
        /* UserAccount.EntryUserId / ModifyUserId - the account row's own columns, which
           Sp_UserAccount_Login returns; a C# int, so an unset value is 0. */
        int entryUser = u.getEntryUserId() == null ? 0 : u.getEntryUserId();
        int modifyUser = u.getModifyUserId() == null ? 0 : u.getModifyUserId();

        List<Map<String, Object>> list = new ArrayList<>();
        if (cells != null) {
            for (Map<String, Object> c : cells) {
                int coaId = toInt(c.get("ChartofAccountId"));
                if (!loadedTitle.containsKey(coaId)) continue;      /* not a loaded row */
                int value = toInt(c.get("NoteTitle"));
                if (value == 0 || value == toInt(loadedTitle.get(coaId))) continue;
                if (!noteIds.contains(value)) {                      /* LimitToList */
                    throw new IllegalArgumentException("Invalid note selected.");
                }
                Timestamp now = new Timestamp(System.currentTimeMillis());
                /* GenericProvider.SetProc - every non-virtual property of Model.PLBSSetting */
                Map<String, Object> p = DesktopProc.params(
                        "EntryDate", now,
                        "ModifyDate", now,
                        "BSNoteId", "BS".equals(mode) ? value : 0,
                        "ChartOfAccountId", coaId,
                        "CompanyId", company,
                        "EntryUserId", entryUser,
                        "FinancialYearId", year,
                        "Id", 0,
                        "ModifyUserId", modifyUser,
                        "OrganizationId", org,
                        "PLNoteId", "BS".equals(mode) ? 0 : value);
                list.add(p);
            }
        }
        for (Map<String, Object> p : list) DesktopProc.setProc(jdbc, P_INSERT, p);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("count", list.size());
        if (!list.isEmpty()) out.put("message", "Receord Update Successfully");
        return out;
    }

    /* ------------------------------------------------------------------ tab 2: Change Note Title */

    /** GridNoteTitleFill(): Note '' (All) / 'PL' / 'BS'. */
    public Map<String, Object> notes(String note) {
        admin();
        List<Map<String, Object>> raw = readNotes(note);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            /* tableNote.Rows.Add(Id, NoteTitle, NoteRole, AccountClass, ClassName) */
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", r.get("Id"));
            m.put("NoteTitle", r.get("NoteTitle"));
            m.put("NoteRole", r.get("NoteRole"));
            m.put("AccountClassId", r.get("AccountClass"));
            m.put("ClassName", r.get("ClassName"));
            rows.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        return out;
    }

    private List<Map<String, Object>> readNotes(String note) {
        String n = ("PL".equals(note) || "BS".equals(note)) ? note : null;   /* only when non-empty */
        return DesktopProc.rows(jdbc, P_NOTES, DesktopProc.params(
                "AccountNotesType", "ReadAllNotes", "Note", n));
    }

    /**
     * btnUpdateNote_Click: rows with Id != 0 whose NoteTitle differs from the loaded one, then
     * UpdateTitleById per row, each its own call (no transaction on the desktop either).
     */
    public Map<String, Object> updateNotes(String loadNote, List<Map<String, Object>> cells) {
        admin();
        Map<Integer, String> loaded = new HashMap<>();
        for (Map<String, Object> r : readNotes(loadNote)) loaded.put(toInt(r.get("Id")), str(r.get("NoteTitle")));

        List<Object[]> list = new ArrayList<>();
        if (cells != null) {
            for (Map<String, Object> c : cells) {
                int id = toInt(c.get("Id"));
                if (id == 0 || !loaded.containsKey(id)) continue;
                String title = str(c.get("NoteTitle"));
                if (title.equals(loaded.get(id))) continue;
                list.add(new Object[] { id, title });
            }
        }
        for (Object[] x : list) {
            DesktopProc.rows(jdbc, P_NOTES, DesktopProc.params(
                    "Id", x[0], "Note", x[1], "AccountNotesType", "UpdateTitleById"));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("count", list.size());
        if (!list.isEmpty()) out.put("message", "Receord Update Successfully");
        return out;
    }

    /* ------------------------------------------------------------------ Conversion.ToInt / ToString */

    private static int toInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return 0;
        try { return Integer.parseInt(s); } catch (NumberFormatException e) { return 0; }
    }

    private static String str(Object v) {
        return v == null ? "" : String.valueOf(v);
    }
}
