package com.mst.services.lab;

import com.mst.repositories.lab.GroupAnalysisStandardsRepository;
import com.mst.security.CurrentUserContext;
import com.mst.services.StoreScreenRights;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Screen 158 "Group Analysis Standards" (ModuleId 7) - desktop form
 * Architecture.WinApp.Lab/InvLabGroupAnalysisStandards.cs (":n" below), BLL
 * 0406_Architecture.BLL.Lab.InvLabGroupAnalysisStandards.cs, table InvLabGroupAnalysisStandards.
 *
 * The form has New / Refresh / Update / Save only: no delete, no print, no history tab, and no
 * Yes/No prompt. Tenancy (OrganizationId, CompanyId) comes from the session, never from the browser.
 *
 * RIGHTS. The desktop form itself contains no rights code at all (there is no call to
 * CommonServices.SetRightsValueInRightsObject in the .cs); the only gate on the desktop is the menu,
 * which lists a screen for users that hold a grant on it. The web page is reachable by URL, so the
 * View right of ScreenName "InvLabGroupAnalysisStandards" (seed_screendef.txt Id 158) is required for
 * every call, and Save / Update are required for the two writes. See the report, "decisions needed".
 */
@Service
public class GroupAnalysisStandardsService {

    private static final org.slf4j.Logger LOG = org.slf4j.LoggerFactory.getLogger(GroupAnalysisStandardsService.class);

    public static final int SCREEN_ID = 158;
    public static final String SCREEN_NAME = "InvLabGroupAnalysisStandards";

    private final GroupAnalysisStandardsRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public GroupAnalysisStandardsService(GroupAnalysisStandardsRepository repo, StoreScreenRights rights,
                                         CurrentUserContext ctx) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ------------------------------------------------------------------------------ reads

    /** InvLabAnalysisGroup_Load (:349): gridfill(), GroupFill(), ItemFill(). */
    public Map<String, Object> lookups() {
        Map<String, Boolean> r = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", r);
        out.put("screenId", SCREEN_ID);
        List<String> errors = new ArrayList<>();
        out.put("rows", safe("gridfill", errors, this::rowsNoCheck));
        out.put("groups", safe("GroupFill", errors, this::groupsNoCheck));
        out.put("items", safe("ItemFill", errors, this::itemsNoCheck));
        out.put("errors", errors);
        return out;
    }

    /** btnRefresh_Click (:463): gridfill(), ItemFill(), GroupFill(). */
    public Map<String, Object> refresh() {
        requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        List<String> errors = new ArrayList<>();
        out.put("rows", safe("gridfill", errors, this::rowsNoCheck));
        out.put("items", safe("ItemFill", errors, this::itemsNoCheck));
        out.put("groups", safe("GroupFill", errors, this::groupsNoCheck));
        out.put("errors", errors);
        return out;
    }

    /**
     * One list of the form load. On the desktop every fill (ItemFill, CityFill, GroupFill, BindPlantName ...)
     * has its own try / catch -> MessageBox.Show(ex.Message): a fill that fails leaves ITS combo empty and
     * the other fills still run. Reading all of them in one unguarded call made one failing procedure
     * empty every dropdown of the page. Here a failing list comes back empty and its message is added to
     * "errors", which the page shows one by one.
     */
    private static List<Map<String, Object>> safe(String fill, List<String> errors,
                                                  java.util.function.Supplier<List<Map<String, Object>>> read) {
        try {
            List<Map<String, Object>> rows = read.get();
            return rows == null ? new ArrayList<>() : rows;
        } catch (org.springframework.dao.DataAccessException e) {
            Throwable r = e;
            while (r.getCause() != null && r.getCause() != r) r = r.getCause();
            String text = r.getMessage() == null || r.getMessage().trim().isEmpty() ? "Database Error" : r.getMessage().trim();
            LOG.warn("{} failed: {}", fill, text);
            errors.add(fill + ": " + text);
            return new ArrayList<>();
        }
    }

    /** gridfill (:282) - InvLabGroupAnalysisStandards.GetAllOrById(OrganizationId, CompanyId). */
    public List<Map<String, Object>> rows() {
        requireView();
        return rowsNoCheck();
    }

    /** grdfrm_DoubleClick (:366) - GetAllOrById(OrganizationId, CompanyId, Id); null when no row comes back. */
    public Map<String, Object> byId(int id) {
        requireView();
        if (id == 0) return null;                 // an Id of 0 would not be sent and would read every row
        List<Map<String, Object>> rows = repo.getAllOrById(ctx.currentOrganizationId(), ctx.currentCompanyId(), id);
        return rows.isEmpty() ? null : rows.get(0);
    }

    private List<Map<String, Object>> rowsNoCheck() {
        return repo.getAllOrById(ctx.currentOrganizationId(), ctx.currentCompanyId(), 0);
    }

    /** GroupFill (:177) - value member Id, display member AnalysisGroupDescription. */
    private List<Map<String, Object>> groupsNoCheck() {
        return repo.analysisGroups(ctx.currentOrganizationId(), ctx.currentCompanyId(), 0);
    }

    /** ItemFill (:151) - value member Id, display member AnalysisParameterDescription. */
    private List<Map<String, Object>> itemsNoCheck() {
        return repo.analysisItems(ctx.currentOrganizationId(), ctx.currentCompanyId(), 0);
    }

    // ------------------------------------------------------------------------------ save / update

    /**
     * btnsave_Click (:208, Id not set -> Insert) and btnupdate_Click (:244, Id = RecId -> Update).
     * formvalidation (:92) in desktop order with the desktop's texts; then one procedure call.
     * The procedure's own RAISERRORs (standard outside the parameter's Min/Max, "Record Not Update
     * because record already has exist") surface as the message, as on the desktop (catch -> MessageBox).
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> body) {
        Map<String, Boolean> r = requireView();
        int id = toInt(body.get("id"));
        if (id == 0 && !Boolean.TRUE.equals(r.get("save")))
            throw new AccessDeniedException("You do not have the Save right for this screen.");
        if (id != 0 && !Boolean.TRUE.equals(r.get("update")))
            throw new AccessDeniedException("You do not have the Update right for this screen.");

        int org = ctx.currentOrganizationId();
        int company = ctx.currentCompanyId();

        // formvalidation (:92-131)
        int groupId = toInt(body.get("groupId"));                    // Conversion.ToInt(cmbgroup.Value) (:225)
        int itemId = toInt(body.get("itemId"));                      // Conversion.ToInt(cmbitem.Value) (:226)
        String minText = text(body.get("minValue"));
        String maxText = text(body.get("maxValue"));
        // cmbgroup.Text == "" (:94). The combo is LimitToList, so a text is always a row of the bound
        // list; the id is therefore checked against the same list the combo shows.
        if (groupId == 0 || repo.analysisGroups(org, company, groupId).isEmpty())
            throw new IllegalArgumentException("Please select Analysis Group");
        if (itemId == 0 || repo.analysisItems(org, company, itemId).isEmpty())           // (:100)
            throw new IllegalArgumentException("Please select Analysis Parameter");
        if (minText.isEmpty()) throw new IllegalArgumentException("Min Value Required");   // (:106)
        if (maxText.isEmpty()) throw new IllegalArgumentException("Max Value Required");   // (:112)
        double min = toDouble(minText);
        double max = toDouble(maxText);
        if (min == max) throw new IllegalArgumentException("Min Value cant equal to Max Value");       // (:118)
        if (min > max) throw new IllegalArgumentException("Min Value cant Greater than Max Value");    // (:124)

        // Web-only tenancy guard: Sp_InvLabGroupAnalysisStandards_Update filters on Id alone, and the id
        // arrives from the browser. The desktop can only hold an Id it read through the company filter.
        if (id != 0 && repo.getAllOrById(org, company, id).isEmpty())
            throw new AccessDeniedException("This record does not belong to the current company.");

        boolean editable = toBool(body.get("editableAfterApproval"));  // chkEditableAfterApproval.Checked (:231)
        Integer saved = repo.save(id, groupId, itemId, min, max, org, company, editable);

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", saved);
        out.put("message", id == 0 ? "Save Successfully" : "Update Successfully");         // (:233 / :271)
        return out;
    }

    // ------------------------------------------------------------------------------ helpers

    private Map<String, Boolean> requireView() {
        ctx.requireAccountingUser();
        Map<String, Boolean> r = rights.of(SCREEN_NAME);
        if (!Boolean.TRUE.equals(r.get("view")))
            throw new AccessDeniedException("You do not have the View right for Group Analysis Standards.");
        return r;
    }

    private static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    /** Architecture.Common.Conversion.ToInt - anything that is not a number is 0. */
    static int toInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) {
            try { return (int) Math.round(Double.parseDouble(String.valueOf(v).trim())); }
            catch (NumberFormatException e2) { return 0; }
        }
    }

    /** Conversion.ToDouble (0005 common:151) - Convert.ToDouble, 0 on any failure or on infinity. */
    static double toDouble(String s) {
        try {
            double d = Double.parseDouble(s.replace(",", "").trim());
            return Double.isInfinite(d) || Double.isNaN(d) ? 0.0 : d;
        } catch (NumberFormatException e) {
            return 0.0;
        }
    }

    /** Conversion.ToBool - "1" counts as true. */
    static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        if (v == null) return false;
        String s = String.valueOf(v).trim();
        return "true".equalsIgnoreCase(s) || "1".equals(s);
    }
}
