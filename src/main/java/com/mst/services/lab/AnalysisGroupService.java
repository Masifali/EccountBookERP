package com.mst.services.lab;

import com.mst.repositories.lab.AnalysisGroupRepository;
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
 * Screen 157 "Analysis Group" (Lab, module 7) — desktop InvLabAnalysisGroup.cs. Line references
 * (:n) are Architecture.WinApp.Lab/InvLabAnalysisGroup.cs unless a BLL file is named.
 *
 * RIGHTS. The desktop form never calls CommonServices.SetRightsValueInRightsObject — it has no
 * in-form Save/Update/Delete/Print check and no Delete or Print button at all. The only gate is the
 * menu (the screen opens only for a user who holds it). So the server enforces the View right of
 * ScreenName "InvLabAnalysisGroup" (seed_screendef.txt id 157) on every call and does NOT invent a
 * Save/Update gate the desktop does not have. The flags are still returned to the page.
 *
 * TENANCY. OrganizationId / CompanyId come from the session only; BLL Save overwrites both from
 * clsGlobalVariables.UserAccount as well (0400:19-20).
 */
@Service
public class AnalysisGroupService {

    /** seed_screendef.txt: (157, N'InvLabAnalysisGroup', N'Analysis Group', 7, ...) */
    public static final int SCREEN_ID = 157;
    public static final String SCREEN_NAME = "InvLabAnalysisGroup";

    private final AnalysisGroupRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public AnalysisGroupService(AnalysisGroupRepository repo, StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
    }

    private Map<String, Boolean> requireView() {
        Map<String, Boolean> r = rights.of(SCREEN_NAME);
        if (!Boolean.TRUE.equals(r.get("view"))) {
            throw new AccessDeniedException("You do not have the View right for Analysis Group.");
        }
        return r;
    }

    /** InvLabAnalysisGroup_Load_1 (:283-297): GroupTypeFill() then gridfill(). */
    public Map<String, Object> init() {
        Map<String, Boolean> r = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", r);
        out.put("groupTypes", groupTypes());
        out.put("rows", gridRows());
        return out;
    }

    /**
     * GroupTypeFill (:299-326): dt(Id, Name) built from Id / InvParentCateDescription of
     * Item.InventoryParentCategories(Ids = "1,2,3,4,7,10,11"); bound value member "Id", display
     * member "Name", caption "Group Type" (:332).
     */
    private List<Map<String, Object>> groupTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.groupTypes()) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("Name", ci(r, "InvParentCateDescription"));
            out.add(m);
        }
        return out;
    }

    /** gridfill (:168-200): table(Id, GroupType, GroupCode, Description) projected from ReadAll (:190). */
    public List<Map<String, Object>> grid() {
        requireView();
        return gridRows();
    }

    private List<Map<String, Object>> gridRows() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.readAll(ctx.currentOrganizationId(), ctx.currentCompanyId())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", ci(r, "Id"));
            m.put("GroupType", ci(r, "InvParentCateDescription"));
            m.put("GroupCode", ci(r, "AnalysisGroupCode"));
            m.put("Description", ci(r, "AnalysisGroupDescription"));
            out.add(m);
        }
        return out;
    }

    /** grdfrm_DoubleClick (:225-252): GetAllOrById with Id; fills GroupType, AnalysisGroupCode, AnalysisGroupDescription. */
    public Map<String, Object> byId(int id) {
        requireView();
        List<Map<String, Object>> rows = id <= 0 ? new ArrayList<>()
                : repo.readById(ctx.currentOrganizationId(), ctx.currentCompanyId(), id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("found", !rows.isEmpty());                       // :239 — nothing happens when no row comes back
        if (!rows.isEmpty()) {
            Map<String, Object> r = rows.get(0);
            out.put("Id", ci(r, "Id"));
            out.put("GroupType", toInt(ci(r, "GroupType")));      // :241 Conversion.ToInt
            out.put("AnalysisGroupCode", str(ci(r, "AnalysisGroupCode")));
            out.put("AnalysisGroupDescription", str(ci(r, "AnalysisGroupDescription")));
        }
        return out;
    }

    /**
     * btnsave_Click (:104-133) when id == 0, btnupdate_Click (:135-166) otherwise.
     *
     * formvalidation (:81-102), in desktop order with the desktop's text:
     *   1. CmbGroupType.Text.Trim() == ""  -> "Please select Group type"
     *   2. txtcode.Text.Trim() == ""       -> "Please Insert Code"
     *   3. txtdescription.Text.Trim() == ""-> "Please Insert Description"
     * The "Are you sure to Save?/Update?" prompt (:115/:147) is a plain pre-save confirmation that
     * depends on no server state; the page asks it before posting.
     *
     * The text boxes are sent as typed (Conversion.ToString(txt.Text), :119-120 — not trimmed).
     * One procedure call, inside one transaction (DAL SetDate opens its own, 0356:14).
     */
    @Transactional
    public Map<String, Object> save(Integer id, Integer groupType, String code, String description) {
        requireView();
        int recId = id == null ? 0 : id;
        int type = groupType == null ? 0 : groupType;
        int org = ctx.currentOrganizationId();
        int company = ctx.currentCompanyId();

        /* The combo is LimitToList (:647): its text is non-empty only when a listed row is selected.
           A posted id that is not in the form's own list is therefore "no group type selected". */
        if (type <= 0 || !listed(type)) throw new IllegalArgumentException("Please select Group type");
        if (code == null || code.trim().isEmpty()) throw new IllegalArgumentException("Please Insert Code");
        if (description == null || description.trim().isEmpty()) throw new IllegalArgumentException("Please Insert Description");

        Map<String, Object> out = new LinkedHashMap<>();
        if (recId > 0) {
            /* RecId on the desktop can only come from a row of the company-filtered grid (:236-237).
               The update procedure filters on Id alone, so the posted id is proven to belong to this
               organization and company first. */
            if (repo.readById(org, company, recId).isEmpty()) {
                throw new AccessDeniedException("This Analysis Group does not belong to the current company.");
            }
            repo.update(company, recId, org, code, description, type);
            out.put("success", true);
            out.put("id", recId);
            out.put("message", "Update Successfully");            // :157
            return out;
        }
        Integer newId = repo.insert(company, org, code, description, type);
        if (newId == null || newId <= 0) {
            /* :115 "Save(...) > 0" — the desktop shows nothing and keeps the form when no id comes back. */
            throw new IllegalStateException("Analysis Group was not saved (the procedure returned no Id).");
        }
        out.put("success", true);
        out.put("id", newId);
        out.put("message", "Save Successfully");                  // :124
        return out;
    }

    private boolean listed(int groupType) {
        for (Map<String, Object> r : repo.groupTypes()) {
            if (toInt(ci(r, "Id")) == groupType) return true;
        }
        return false;
    }

    private static Object ci(Map<String, Object> row, String name) {
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    /** Conversion.ToInt — anything unparsable is 0. */
    private static int toInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        if (v == null) return 0;
        try { return (int) Double.parseDouble(String.valueOf(v).trim()); } catch (NumberFormatException e) { return 0; }
    }
}
