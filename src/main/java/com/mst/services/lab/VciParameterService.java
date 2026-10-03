package com.mst.services.lab;

import com.mst.repositories.lab.VciParameterRepository;
import com.mst.security.CurrentUserContext;
import com.mst.services.StoreScreenRights;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Screen 809 "VCI Parameter" (Lab, module 7) — desktop Architecture.WinApp.Lookups/ExImVCIParameter.cs.
 * Line references (:n) are that file.
 *
 * RIGHTS. The form never calls CommonServices.SetRightsValueInRightsObject and has no Delete or Print
 * button; the only gate is the menu. The server therefore enforces the View right of ScreenName
 * "ExImVCIParameter" on every call and invents no Save/Update gate.
 *
 * TENANCY. OrganizationId / CompanyId / user id come from the session only (:149-154 take them from
 * UserAccount).
 */
@Service
public class VciParameterService {

    public static final int SCREEN_ID = 809;
    public static final String SCREEN_NAME = "ExImVCIParameter";

    private final VciParameterRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public VciParameterService(VciParameterRepository repo, StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
    }

    private Map<String, Boolean> requireView() {
        Map<String, Boolean> r = rights.of(SCREEN_NAME);
        if (!Boolean.TRUE.equals(r.get("view"))) {
            throw new AccessDeniedException("You do not have the View right for VCI Parameter.");
        }
        return r;
    }

    /** ExImVCIParameter_Load (:75-92): ParameterCategoryfill() then FormHistory() (category value 0 = all). */
    public Map<String, Object> init() {
        Map<String, Boolean> r = requireView();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", r);
        out.put("categories", categoryRows());
        out.put("rows", historyRows(0));
        return out;
    }

    /** ParameterCategoryfill (:94-129) / btnRefresh_Click (:400-410). */
    public List<Map<String, Object>> categories() {
        requireView();
        return categoryRows();
    }

    /** The two columns BindDDLNew keeps (DropDownBind.cs): value member "Id", display member "VciCategoryDescription". */
    private List<Map<String, Object>> categoryRows() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.categories(ctx.currentOrganizationId(), ctx.currentCompanyId())) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(ci(r, "Id")));
            m.put("VciCategoryDescription", str(ci(r, "VciCategoryDescription")));
            out.add(m);
        }
        return out;
    }

    /** FormHistory (:251-286). */
    public List<Map<String, Object>> history(int categoryId) {
        requireView();
        return historyRows(categoryId);
    }

    /** dt(Id, CategoryDescription, ParameterDescription) built at :263-271. */
    private List<Map<String, Object>> historyRows(int categoryId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.getAll(ctx.currentOrganizationId(), ctx.currentCompanyId(), Math.max(categoryId, 0))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(ci(r, "VCIParameterId")));
            m.put("CategoryDescription", str(ci(r, "VciCategoryDescription")));
            m.put("ParameterDescription", str(ci(r, "VciParameterDescription")));
            out.add(m);
        }
        return out;
    }

    /**
     * Insert() (:131-177) — btnsave_Click (:179, RecId forced to 0) and btnUpdate_Click (:192).
     *
     * FormValidation (:206-220), desktop order and text:
     *   1. txtDescription.Text == ""                       -> "Description Field is Required"  (not trimmed)
     *   2. no active combo row or Conversion.ToInt(Value)==0 -> "VCI Category Field is Required"
     * The "Are you sure to Save? / Update?" prompt (:142/:148) depends on no server state; the page asks it.
     * The description is sent as typed (Conversion.ToString(txtDescription.Text), :155).
     */
    @Transactional
    public Map<String, Object> save(Integer id, Integer categoryId, String description) {
        requireView();
        int recId = id == null ? 0 : id;
        int category = categoryId == null ? 0 : categoryId;
        int org = ctx.currentOrganizationId();
        int company = ctx.currentCompanyId();
        int user = ctx.currentUserId();

        if (description == null || description.isEmpty()) throw new IllegalArgumentException("Description Field is Required");
        /* The combo is LimitToList (:540): a value that is not in the form's own list cannot be selected. */
        if (category <= 0 || !categoryListed(org, company, category)) throw new IllegalArgumentException("VCI Category Field is Required");

        Timestamp now = new Timestamp(System.currentTimeMillis());   // :151-152 / BLL 0037 DateTime.Now
        Map<String, Object> out = new LinkedHashMap<>();
        if (recId > 0) {
            /* RecId on the desktop can only come from a row of the company-filtered grid (:330). The update
               procedure filters on Id alone, so the posted id is proven to belong to this company first. */
            if (!parameterListed(org, company, recId)) {
                throw new AccessDeniedException("This VCI Parameter does not belong to the current company.");
            }
            repo.update(now, company, user, category, recId, org, description);
            out.put("success", true);
            out.put("id", recId);
            out.put("message", "Record Update Successfully");      // :160
            return out;
        }
        Integer newId = repo.insert(now, company, user, category, org, description);
        out.put("success", true);
        out.put("id", newId == null ? 0 : newId);
        out.put("message", "Record Save Successfully");            // :164
        return out;
    }

    private boolean categoryListed(int org, int company, int category) {
        for (Map<String, Object> r : repo.categories(org, company)) {
            if (toInt(ci(r, "Id")) == category) return true;
        }
        return false;
    }

    private boolean parameterListed(int org, int company, int id) {
        for (Map<String, Object> r : repo.getAll(org, company, 0)) {
            if (toInt(ci(r, "VCIParameterId")) == id) return true;
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
