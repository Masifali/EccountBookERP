package com.mst.services;

import com.mst.repositories.LabAnalysisItemsRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * Screen 156 - Item Analysis Parameter. Desktop: <code>InvLabAnalysisItems.cs</code>.
 *
 * The form is small enough to port whole: formvalidation(), Insert(), gridfill(),
 * grdfrm_DoubleClick(), MasterParameters(), refresh(), ChkIsSub_CheckedChanged().
 *
 * ---------------------------------------------------------------------------------------------
 * WHAT THE DESKTOP WRITES, AND WHY EVERY FIELD IS ALWAYS SENT
 * ---------------------------------------------------------------------------------------------
 * Insert() (:128-183) assigns nine properties before calling Save, with no conditionals except
 * one - ParentParameterId, which has an explicit else that sets 0:
 *
 *     obj.CompanyId                     = UserAccount.CompanyId
 *     obj.OrganizationId                = UserAccount.OrganizationId
 *     obj.AnalysisParameterCode         = txtdescription.Text      <- the SAME box as below
 *     obj.AnalysisParameterDescription  = txtdescription.Text
 *     obj.IsSub                         = ChkIsSub.Checked
 *     obj.MasterParId                   = Conversion.ToInt(CmbMasterParameter.Value)
 *     obj.MinValue                      = Conversion.ToDouble(txtMinValue.Text)
 *     obj.MaxValue                      = Conversion.ToDouble(txtMaxValue.Text)
 *     obj.ParentParameterId             = IsSub ? cmb : 0
 *
 * Code and Description both take txtdescription - there is one textbox on the form and it fills
 * both columns. That is deliberate, not a transcription slip, and it is reproduced: writing a
 * generated code into AnalysisParameterCode would diverge from every row the desktop has written.
 *
 * Conversion.ToInt(null) is 0 and Conversion.ToDouble("") is 0, so an untouched combo or an empty
 * numeric box still sends 0 - never NULL. Each is sent unconditionally here for the same reason.
 *
 * ---------------------------------------------------------------------------------------------
 * SAVE vs UPDATE
 * ---------------------------------------------------------------------------------------------
 * The desktop has one Insert() for both. btnsave_Click sets RecId = 0 first; btnupdate_Click does
 * not. RecId > 0 sets obj.Id and the BLL routes to Sp_..._Update, otherwise Sp_..._Insert. The
 * web keeps that shape: an id of 0 (or absent) inserts, anything else updates.
 *
 * RecId is NOT taken from the client. A posted id decides which row gets overwritten, so it is
 * re-read against this organization and company before any update - otherwise a crafted request
 * could rewrite another tenant's parameter. The desktop cannot do that because RecId can only be
 * set by double-clicking a row in a grid that was already filtered to the user's own company.
 */
@Service
public class LabAnalysisItemsService {

    private final LabAnalysisItemsRepository repository;
    private final CurrentUserContext currentUserContext;

    public LabAnalysisItemsService(LabAnalysisItemsRepository repository,
                                   CurrentUserContext currentUserContext) {
        this.repository = repository;
        this.currentUserContext = currentUserContext;
    }

    /**
     * gridfill() (:214-252). The desktop projects the procedure's columns into a seven-column
     * table and shows five of them - Id and MasterParId are present but hidden (grdfrmSetting,
     * :253-271), because the double-click needs Id and the combo needs MasterParId. The same
     * seven are returned here; the page hides the same two.
     *
     * ParentParameter and MasterParameterName are resolved by the procedure's own LEFT JOINs, so
     * a row whose parent or master no longer exists shows blank rather than disappearing.
     */
    public List<Map<String, Object>> grid() {
        int org = currentUserContext.currentOrganizationId();
        int company = currentUserContext.currentCompanyId();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repository.readAll(org, company)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", r.get("Id"));
            m.put("analysisParameter", r.get("AnalysisParameterDescription"));
            m.put("parentParameter", r.get("ParentParameter"));
            m.put("masterParId", r.get("MasterParId"));
            m.put("masterParameter", r.get("MasterParameterName"));
            m.put("minValue", r.get("MinValue"));
            m.put("maxValue", r.get("MaxValue"));
            out.add(m);
        }
        return out;
    }

    /**
     * The Parent Parameter combo. The desktop does NOT query for it - gridfill() binds the combo
     * from the very table it just built (DDL.BindDDLNew(table, CmbParentParameter, "Id",
     * "AnalysisParameter", ...), :246-249), and only when that table has rows. So the parent list
     * is exactly the grid: every existing parameter, itself included. Reproduced by serving the
     * same rows rather than issuing a second read.
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

    /** MasterParameters() (:305-320) - usp_getLabMasterParms, bound Id / MasterParameterName. */
    public List<Map<String, Object>> masterParameters() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repository.masterParameters()) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("id", r.get("Id"));
            m.put("description", r.get("MasterParameterName"));
            out.add(m);
        }
        return out;
    }

    /**
     * grdfrm_DoubleClick (:273-304) - loads one row into the entry fields and flips Save to
     * Update. Tenancy is part of the read, exactly as the desktop's model carries
     * OrganizationId/CompanyId into GetAllOrById.
     */
    public Map<String, Object> readById(int id) {
        List<Map<String, Object>> rows = repository.readById(
                currentUserContext.currentOrganizationId(),
                currentUserContext.currentCompanyId(), id);
        if (rows.isEmpty()) {
            return null;
        }
        Map<String, Object> r = rows.get(0);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("id", r.get("Id"));
        m.put("description", r.get("AnalysisParameterDescription"));
        m.put("parentParameterId", r.get("ParentParameterId"));
        m.put("masterParId", r.get("MasterParId"));
        m.put("isSub", r.get("IsSub"));
        m.put("minValue", r.get("MinValue"));
        m.put("maxValue", r.get("MaxValue"));
        return m;
    }

    /**
     * formvalidation() (:88-103) - both refusals, in the desktop's order and wording.
     *
     * The second one is the reason IsSub matters at save time: the desktop only demands a parent
     * when Is Sub is ticked, and when it is not ticked it stores 0 rather than whatever the combo
     * happened to hold. Validating on the server as well as in the page is what stops a posted
     * body from writing a sub-parameter with no parent.
     */
    private String validate(String description, boolean isSub, int parentParameterId) {
        if (description == null || description.trim().isEmpty()) {
            return "Please Insert Description";
        }
        if (isSub && parentParameterId <= 0) {
            return "Parent Parameter Field is Required";
        }
        return null;
    }

    /** Insert() (:128-183) - one entry point for both save and update, as the desktop has. */
    @Transactional
    public Map<String, Object> save(Integer id, String description, boolean isSub,
                                    Integer parentParameterId, Integer masterParId,
                                    Double minValue, Double maxValue) {
        Map<String, Object> response = new LinkedHashMap<>();

        int parent = isSub ? (parentParameterId == null ? 0 : parentParameterId) : 0;   // :158-165
        int master = masterParId == null ? 0 : masterParId;                             // ToInt(null) == 0
        double min = minValue == null ? 0d : minValue;                                  // ToDouble("") == 0
        double max = maxValue == null ? 0d : maxValue;

        String refusal = validate(description, isSub, parent);
        if (refusal != null) {
            response.put("success", false);
            response.put("message", refusal);
            return response;
        }

        String text = description.trim();
        int org = currentUserContext.currentOrganizationId();
        int company = currentUserContext.currentCompanyId();

        if (id != null && id > 0) {
            /* The posted id is only honoured once it is proven to belong to this tenant. */
            if (repository.readById(org, company, id).isEmpty()) {
                response.put("success", false);
                response.put("message", "That analysis parameter does not belong to this company.");
                return response;
            }
            repository.update(id, text, text, company, org, parent, isSub, master, min, max);
            response.put("success", true);
            response.put("id", id);
            response.put("message", "Update Successfully");      // :172, desktop wording
            return response;
        }

        Integer newId = repository.insert(text, text, company, org, parent, isSub, master, min, max);
        if (newId == null || newId <= 0) {
            throw new IllegalStateException("Analysis parameter save returned no Id.");
        }
        response.put("success", true);
        response.put("id", newId);
        response.put("message", "Save Successfully");            // :168, desktop wording
        return response;
    }
}
