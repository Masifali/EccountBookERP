package com.mst.services;

import com.mst.repositories.LabAnalysisItemsRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Screen 156 "Item Analysis Parameter" (Lab, module 7) — desktop InvLabAnalysisItems.cs. Line
 * references (:n) are Architecture.WinApp.Lab/InvLabAnalysisItems.cs unless a BLL file is named.
 *
 * WHAT Insert() (:128-182) WRITES — nine assignments, then BLL Save:
 *     CompanyId / OrganizationId        = UserAccount                       (:151-152)
 *     AnalysisParameterCode             = txtdescription.Text               (:153)  <- the SAME box
 *     AnalysisParameterDescription      = txtdescription.Text               (:154)
 *     IsSub                             = ChkIsSub.Checked                  (:155)
 *     MasterParId                       = Conversion.ToInt(CmbMasterParameter.Value)   (:156; no selection = 0)
 *     MinValue / MaxValue               = Conversion.ToDouble(text)         (:157-158; "" = 0)
 *     ParentParameterId                 = IsSub ? ToInt(CmbParentParameter.Value) : 0  (:159-166)
 * The description is sent as typed (not trimmed). RecId > 0 routes to Sp_..._Update, else _Insert
 * (BLL 0402:17-21).
 *
 * RIGHTS. The desktop form never calls CommonServices.SetRightsValueInRightsObject — it has no
 * in-form Save/Update/Delete/Print check, and no Delete or Print button (toolstrip :876 = New,
 * Update, Save; the grid is AllowDelete = False, :258). The only gate is the menu. So the server
 * enforces the View right of ScreenName "InvLabAnalysisItems" (seed_screendef.txt id 156) on every
 * call and does not invent a Save/Update gate the desktop does not have.
 *
 * TENANCY. Organization and company come from the session only. A posted id is re-read against
 * them before the update, because Sp_InvLabAnalysisItems_Update filters on Id alone; on the desktop
 * RecId can only come from a row of the company-filtered grid (:284-285).
 */
@Service
public class LabAnalysisItemsService {

    /** seed_screendef.txt: (156, N'InvLabAnalysisItems', N'Item Analysis Parameter', 7, ...) */
    public static final int SCREEN_ID = 156;
    public static final String SCREEN_NAME = "InvLabAnalysisItems";

    private final LabAnalysisItemsRepository repository;
    private final CurrentUserContext currentUserContext;
    private final StoreScreenRights rights;

    public LabAnalysisItemsService(LabAnalysisItemsRepository repository,
                                   CurrentUserContext currentUserContext,
                                   StoreScreenRights rights) {
        this.repository = repository;
        this.currentUserContext = currentUserContext;
        this.rights = rights;
    }

    private Map<String, Boolean> requireView() {
        Map<String, Boolean> r = rights.of(SCREEN_NAME);
        if (!Boolean.TRUE.equals(r.get("view"))) {
            throw new AccessDeniedException("You do not have the View right for Item Analysis Parameter.");
        }
        return r;
    }

    /** The rights of this screen (View is required to get an answer at all). */
    public Map<String, Boolean> rights() {
        return requireView();
    }

    /**
     * gridfill() (:214-251). The desktop projects the procedure's rows into a seven-column table
     * (:228-234, :237): Id, AnalysisParameter, ParentParameter, MasterParId, MasterParameter,
     * MinValue, MaxValue — Id and MasterParId hidden (:259-260).
     */
    public List<Map<String, Object>> grid() {
        requireView();
        int org = currentUserContext.currentOrganizationId();
        int company = currentUserContext.currentCompanyId();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repository.readAll(org, company)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", ci(r, "Id"));
            m.put("analysisParameter", ci(r, "AnalysisParameterDescription"));
            m.put("parentParameter", ci(r, "ParentParameter"));
            m.put("masterParId", ci(r, "MasterParId"));
            m.put("masterParameter", ci(r, "MasterParameterName"));
            m.put("minValue", ci(r, "MinValue"));
            m.put("maxValue", ci(r, "MaxValue"));
            out.add(m);
        }
        return out;
    }

    /**
     * The Parent Parameter combo. The desktop does not query for it: gridfill() binds it from the
     * grid's own table — DDL.BindDDLNew(table, CmbParentParameter, "Id", "AnalysisParameter",
     * "Parent Parameter", false) (:241) — so the list is exactly the grid (the row itself included).
     */
    public List<Map<String, Object>> parentParameters() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : grid()) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", r.get("id"));
            m.put("description", r.get("analysisParameter"));
            out.add(m);
        }
        return out;
    }

    /** MasterParameters() (:305-319) — usp_getLabMasterParms, bound "Id" / "MasterParameterName", caption "Master Parameter". */
    public List<Map<String, Object>> masterParameters() {
        requireView();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repository.masterParameters()) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", ci(r, "Id"));
            m.put("description", ci(r, "MasterParameterName"));
            out.add(m);
        }
        return out;
    }

    /** grdfrm_DoubleClick (:273-303) — GetAllOrById with Id; the six fields the form fills (:290-295). */
    public Map<String, Object> readById(int id) {
        requireView();
        if (id <= 0) return null;
        List<Map<String, Object>> rows = repository.readById(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(), id);
        if (rows.isEmpty()) {
            return null;
        }
        Map<String, Object> r = rows.get(0);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("id", ci(r, "Id"));
        m.put("description", ci(r, "AnalysisParameterDescription"));
        m.put("parentParameterId", ci(r, "ParentParameterId"));
        m.put("masterParId", ci(r, "MasterParId"));
        m.put("isSub", toBool(ci(r, "IsSub")));
        m.put("minValue", ci(r, "MinValue"));
        m.put("maxValue", ci(r, "MaxValue"));
        return m;
    }

    /**
     * Insert() (:128-182) — one entry point for Save (RecId = 0, :188) and Update.
     *
     * formvalidation (:88-103), in desktop order with the desktop's text:
     *   1. txtdescription.Text.Trim() == ""                      -> "Please Insert Description"
     *   2. ChkIsSub.Checked && CmbParentParameter.ActiveRow==null -> "Parent Parameter Field is Required"
     * The "Are you sure to Save?/Update?" prompt (:141/:147) depends on no server state; the page
     * asks it before posting. One procedure call in one transaction (DAL SetDate, 0358:14).
     */
    @Transactional
    public Map<String, Object> save(Integer id, String description, boolean isSub,
                                    Integer parentParameterId, Integer masterParId,
                                    Double minValue, Double maxValue) {
        requireView();

        int posted = parentParameterId == null ? 0 : parentParameterId;
        int master = masterParId == null ? 0 : masterParId;               // :156 ToInt(null) == 0
        double min = minValue == null ? 0d : minValue;                    // :157 ToDouble("") == 0
        double max = maxValue == null ? 0d : maxValue;                    // :158

        if (description == null || description.trim().isEmpty()) {
            throw new IllegalArgumentException("Please Insert Description");
        }
        if (isSub && posted <= 0) {
            throw new IllegalArgumentException("Parent Parameter Field is Required");
        }
        int parent = isSub ? posted : 0;                                  // :159-166

        int org = currentUserContext.currentOrganizationId();
        int company = currentUserContext.currentCompanyId();
        Map<String, Object> response = new LinkedHashMap<>();

        if (id != null && id > 0) {
            if (repository.readById(org, company, id).isEmpty()) {
                throw new AccessDeniedException("This Analysis Parameter does not belong to the current company.");
            }
            repository.update(id, description, description, company, org, parent, isSub, master, min, max);
            response.put("success", true);
            response.put("id", id);
            response.put("message", "Update Successfully");               // :174
            return response;
        }

        Integer newId = repository.insert(description, description, company, org, parent, isSub, master, min, max);
        response.put("success", true);
        response.put("id", newId);
        response.put("message", "Save Successfully");                     // :170 (shown whatever Save returned)
        return response;
    }

    private static Object ci(Map<String, Object> row, String name) {
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) {
            if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        }
        return null;
    }

    /** Conversion.ToBool — true / non-zero / "1" / "true". */
    private static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        if (v == null) return false;
        String s = String.valueOf(v).trim();
        return "1".equals(s) || "true".equalsIgnoreCase(s);
    }
}
