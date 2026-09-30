package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ProductionMasterDataRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

/**
 * The three Production master-data forms - DefineProductionType.cs, DefineProductionPlanType.cs and
 * DefineProductionPlant.cs (Architecture.WinApp.Production) - the BLL side of each screen.
 *
 * The page runs the desktop's FormValidation with the desktop's messages and order; the same checks
 * are repeated here so the procedures are never reached with what the form would have refused.
 * Everything that decides whose record a plant is - organisation and company - comes from the session,
 * never from the request; the branch is the form's own combo (cmbbranch.Value), as on the desktop.
 *
 * Return value of each save is what the desktop's MessageBox shows in brackets:
 * "Record Save Successfully...[n]" / "Record Update Successfully...[n]" - the Insert's new Id, or the
 * record's own Id for an Update (DAL SetDate falls back to obj.Id when the procedure returns nothing).
 */
@Service
public class ProductionMasterDataService {

    /** The desktop screen the toolstrip button opens; the rights check names it (form :268). */
    public static final String WAREHOUSE_ALLOCATION_SCREEN = "frmWarehousesAllocationToPlant";

    @Autowired private ProductionMasterDataRepository repo;
    @Autowired private CurrentUserContext currentUserContext;

    // ------------------------------------------------------------------ Define Production Type

    /** DefineGridFill :76 - ProductionType.GetAll(new ProductionType()). */
    public List<Map<String, Object>> productionTypes() {
        currentUserContext.requireAccountingUser();
        return repo.productionTypes(0);
    }

    /** grdcountrydefine_DoubleClick :110 - GetAll(new ProductionType { Id = RecId }). */
    public List<Map<String, Object>> productionType(int id) {
        currentUserContext.requireAccountingUser();
        if (id <= 0) throw new IllegalArgumentException("Record not found.");
        return repo.productionTypes(id);
    }

    /** save() :52 (Id 0) and btnUpdate_Click :127 (Id = RecId). Code and Description are the same text. */
    public Map<String, Object> saveProductionType(Map<String, Object> body) {
        currentUserContext.requireAccountingUser();
        int id = asInt(body.get("id"));
        String description = text(body.get("description"));
        if (description.isEmpty()) throw new IllegalArgumentException("Description Field Required");
        if (id > 0 && repo.productionTypes(id).isEmpty()) throw new IllegalArgumentException("Record not found.");
        return result(repo.saveProductionType(id, description, description));
    }

    // ------------------------------------------------------------------ Define Plan Type

    /** DefineGridFill - ProductionPlanType.GetAll(new ProductionPlanType()). */
    public List<Map<String, Object>> planTypes() {
        currentUserContext.requireAccountingUser();
        return repo.planTypes(0);
    }

    /** grdcountrydefine_DoubleClick - GetAll(new ProductionPlanType { Id = RecId }). */
    public List<Map<String, Object>> planType(int id) {
        currentUserContext.requireAccountingUser();
        if (id <= 0) throw new IllegalArgumentException("Record not found.");
        return repo.planTypes(id);
    }

    /** save() / btnUpdate_Click - PlanTypeCode and PlanTypeDescription are the same text. */
    public Map<String, Object> savePlanType(Map<String, Object> body) {
        currentUserContext.requireAccountingUser();
        int id = asInt(body.get("id"));
        String description = text(body.get("description"));
        if (description.isEmpty()) throw new IllegalArgumentException("Description Field Required");
        if (id > 0 && repo.planTypes(id).isEmpty()) throw new IllegalArgumentException("Record not found.");
        return result(repo.savePlanType(id, description, description));
    }

    // ------------------------------------------------------------------ Define Plant

    /** DefineCountry_Load :215 - DefineGridFill (GetAll) and BranchFill (BrancheServiceBind). */
    public Map<String, Object> plantSetup() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        try { out.put("plants", repo.plants(u)); }
        catch (Exception e) { out.put("plantsError", msg(e)); }
        try { out.put("branches", repo.branches(u)); }
        catch (Exception e) { out.put("branchesError", msg(e)); }
        return out;
    }

    /** DefineGridFill :98 - InvProductionPlant.GetAll. */
    public List<Map<String, Object>> plants() {
        return repo.plants(currentUserContext.requireAccountingUser());
    }

    /** grdcountrydefine_DoubleClick :150 - InvProductionPlant.GetById(RecId), one of this company's own rows. */
    public List<Map<String, Object>> plant(int id) {
        UserAccount u = currentUserContext.requireAccountingUser();
        if (id <= 0 || !ownsPlant(u, id)) throw new IllegalArgumentException("Record not found.");
        return repo.plantById(id);
    }

    /**
     * save() :60 / btnUpdate_Click :168. Code and Description are sent as typed (Conversion.ToString of
     * .Text - the form trims only for the check); ActionId 0 and ProjectId 0 as the model leaves them;
     * OrganizationId / CompanyId from the session; BranchId from the combo.
     */
    public Map<String, Object> savePlant(Map<String, Object> body) {
        UserAccount u = currentUserContext.requireAccountingUser();
        int id = asInt(body.get("id"));
        String code = raw(body.get("code"));
        String description = raw(body.get("description"));
        int branchId = asInt(body.get("branchId"));
        if (code.trim().isEmpty()) throw new IllegalArgumentException("Code Field Required");
        if (description.trim().isEmpty()) throw new IllegalArgumentException("Description Field Required");
        if (branchId <= 0) throw new IllegalArgumentException("Branch Field Required");
        boolean known = false;
        for (Map<String, Object> b : repo.branches(u)) if (asInt(ci(b, "Id")) == branchId) { known = true; break; }
        if (!known) throw new IllegalArgumentException("Branch Field Required");
        if (id > 0 && !ownsPlant(u, id)) throw new IllegalArgumentException("Record not found.");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("@Id", id);
        m.put("@Code", code);
        m.put("@Description", description);
        m.put("@OrganizationId", u.getOrganizationId());
        m.put("@CompanyId", u.getCompanyId());
        m.put("@BranchId", branchId);
        m.put("@ProjectId", 0);
        m.put("@ActionId", 0);
        return result(repo.savePlant(m));
    }

    /**
     * btnWarehouseAllocation_Click :262 - Admin, or the View right of frmWarehousesAllocationToPlant;
     * otherwise "You Dont Have rights View Of This Form..". That form is not in this port yet, so the
     * answer also says so.
     */
    public Map<String, Object> warehouseAllocationRight() {
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean allowed = "Admin".equals(currentUserContext.currentRoleName())
                || repo.hasViewRight(u, WAREHOUSE_ALLOCATION_SCREEN);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("allowed", allowed);
        out.put("screen", WAREHOUSE_ALLOCATION_SCREEN);
        out.put("ported", false);
        return out;
    }

    private boolean ownsPlant(UserAccount u, int id) {
        for (Map<String, Object> r : repo.plants(u)) if (asInt(ci(r, "Id")) == id) return true;
        return false;
    }

    // ------------------------------------------------------------------ helpers

    private static Map<String, Object> result(int n) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", n);
        return out;
    }

    /** txtDescription.Text.Trim(). */
    private static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    /** Conversion.ToString(txt.Text) - as typed. */
    private static String raw(Object v) { return v == null ? "" : String.valueOf(v); }

    private static String msg(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e.getMessage()) : t.getMessage();
    }

    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    private static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) { return 0; }
    }
}
