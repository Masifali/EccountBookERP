package com.mst.services;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;
import static com.mst.services.Inv4bDefinitionSupport.bool;
import static com.mst.services.Inv4bDefinitionSupport.ci;
import static com.mst.services.Inv4bDefinitionSupport.dbl;
import static com.mst.services.Inv4bDefinitionSupport.now;
import static com.mst.services.Inv4bDefinitionSupport.num;
import static com.mst.services.Inv4bDefinitionSupport.ok;
import static com.mst.services.Inv4bDefinitionSupport.str;

/**
 * Screen 114 "Define Item UOM" — Architecture.WinApp.Inventory_Definition/InvDeffrmItemItemUom.cs (base.Name
 * "InvDeffrmItemItemUom"). Line refs ":NNN" are that file.
 *
 * <pre>
 *   InvDeffrmItemItemUom_Load :79   ParentUOMFill, gridfill (no rights are read by this form)
 *   ParentUOMFill :90               BLL UOM.GetAllParentUOM → Sp_UOM_GetAllMethod @Activity='GetAllParentUOM' (Id / Description)
 *   gridfill :258                   BLL UOM.Getall → Sp_UOM_GetAllMethod @Activity='ReadByOrganizationCompanyId'
 *   grdfrm_DoubleClick :318         BLL UOM.GetByID → Sp_UOM_GetAllMethod @Activity='ReadById'
 *   Insert :182                     BLL UOM.Save → Sp_UOM_Insert (RecId 0, SELECT @Id) / Sp_UOM_Update;
 *                                   UOMDescription = UOMSymbol = UOMCode (:208-209)
 * </pre>
 *
 * Desktop behaviour reproduced: the ReadByOrganizationCompanyId branch filters by OrganizationId only (the
 * CompanyId test is commented out in the procedure), so the grid lists every company's UOMs of the organization.
 */
@Service
public class Inv4bDefineItemUomService {

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;

    public Inv4bDefineItemUomService(JdbcTemplate jdbc, CurrentUserContext ctx) { this.jdbc = jdbc; this.ctx = ctx; }

    /** InvDeffrmItemItemUom_Load :79. */
    public Map<String, Object> init() {
        ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> parents = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_UOM_GetAllMethod", params("Activity", "GetAllParentUOM"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, "Id")));
            o.put("Name", str(ci(r, "Description")));
            parents.add(o);
        }
        out.put("parentUoms", parents);
        out.put("history", history());
        return out;
    }

    /** gridfill :258 — Id, UomCode, Equivalent, ParentUOMId, ParentUOM, Status, QtyEquivalent (:283). */
    public List<Map<String, Object>> history() {
        ctx.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_UOM_GetAllMethod", params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "Activity", "ReadByOrganizationCompanyId"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, "Id")));
            o.put("UomCode", str(ci(r, "UOMCode")));
            o.put("Equivalent", dbl(ci(r, "Equivalent")));
            o.put("ParentUOMId", num(ci(r, "ParentUOMId")));
            o.put("ParentUOM", str(ci(r, "ParentUomCode")));
            o.put("Status", bool(ci(r, "UOMStatus")));
            o.put("QtyEquivalent", dbl(ci(r, "QtyEquivalent")));
            out.add(o);
        }
        return out;
    }

    /** grdfrm_DoubleClick :318 — GetByID; null when the row does not exist or is another organization's. */
    public Map<String, Object> read(int id) {
        ctx.requireAccountingUser();
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_UOM_GetAllMethod", params("Id", id, "Activity", "ReadById"));
        if (rows.isEmpty()) return null;
        Map<String, Object> r = rows.get(0);
        if (num(ci(r, "OrganizationId")) != ctx.currentOrganizationId()) return null;
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", num(ci(r, "Id")));
        o.put("UOMCode", str(ci(r, "UOMCode")));
        o.put("Equivalent", dbl(ci(r, "Equivalent")));
        o.put("UOMStatus", bool(ci(r, "UOMStatus")));
        o.put("ParentUOMId", num(ci(r, "ParentUOMId")));
        o.put("QtyEquivalent", dbl(ci(r, "QtyEquivalent")));
        return o;
    }

    /** formvalidation :106 — the three messages in order (Equivalent / QtyEquivalent are tested as ToDouble == 0). */
    public void validate(Map<String, Object> b) {
        if (str(b.get("UOMCode")).isEmpty()) throw new IllegalArgumentException("UomCode field is required");
        if (dbl(b.get("Equivalent")) == 0.0) throw new IllegalArgumentException("Equivalent field is required");
        if (dbl(b.get("QtyEquivalent")) == 0.0) throw new IllegalArgumentException("QtyEquivalent field is required");
    }

    /** Insert :182. */
    public Map<String, Object> save(Map<String, Object> b) {
        ctx.requireAccountingUser();
        int recId = num(b.get("Id"));
        validate(b);
        if (recId > 0 && read(recId) == null) throw new IllegalArgumentException("Record Not Found");
        String code = str(b.get("UOMCode"));
        java.sql.Timestamp now = now();
        Map<String, Object> p = params(
                "CompanyId", ctx.currentCompanyId(),
                "EntryDate", now,
                "EntryUser", ctx.currentUserId(),
                "Id", recId,
                "ModifyDate", now,
                "ModifyUser", ctx.currentUserId(),
                "OrganizationId", ctx.currentOrganizationId(),
                "ParentUOMId", num(b.get("ParentUOMId")),
                "UOMCode", code,
                "UOMDescription", code,                                   // :208
                "UOMStatus", bool(b.get("UOMStatus")),
                "UOMSymbol", code,                                        // :209
                "Equivalent", dbl(b.get("Equivalent")),
                "QtyEquivalent", dbl(b.get("QtyEquivalent")));
        int id = recId;
        if (recId == 0) id = DesktopProc.setProc(jdbc, "Sp_UOM_Insert", p);      // SELECT @Id
        else DesktopProc.scalar(jdbc, "Sp_UOM_Update", p);
        return ok(recId > 0 ? "Record Update Successfully." : "Record Save Successfully..", id);
    }
}
