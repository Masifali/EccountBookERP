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
import static com.mst.services.Inv4bDefinitionSupport.day;
import static com.mst.services.Inv4bDefinitionSupport.dbl;
import static com.mst.services.Inv4bDefinitionSupport.now;
import static com.mst.services.Inv4bDefinitionSupport.num;
import static com.mst.services.Inv4bDefinitionSupport.ok;
import static com.mst.services.Inv4bDefinitionSupport.str;

/**
 * Screen 94 "Commission Items" — Architecture.WinApp.WholeSale.Commission/CommissionItems.cs (base.Name
 * "CommissionItems", the rights key). Line refs ":NNN" are that file.
 *
 * <pre>
 *   InvDeffrmWarehouse_Load :299   rights (SetRightsValueInRightsObject), BindHistory
 *   BindHistory :100               BLL CommissionItems.FormHistory → USP_CommissionItems_FormHistory
 *   ReadById :273                  BLL CommissionItems.GetById   → USP_CommissionItems_ReadById @Id
 *   Insert :212                    BLL CommissionItems.Save      → USP_CommissionItems_Insert (RecId 0, returns @Id)
 *                                                               / USP_CommissionItems_Update (RecId &gt; 0)
 * </pre>
 *
 * Desktop behaviour reproduced: the model handed to FormHistory / Save carries only OrganizationId and
 * CompanyId (:108, :235) — IsActive and FinancialYearId are not set, so they are not sent and the
 * procedures use their NULL defaults. The "&gt; 100" check (:243) runs after FormValiadation and before
 * the procedure's own RAISERROR of the same meaning. Messages are verbatim (:87, :93, :245, :258, :262).
 */
@Service
public class Inv4bCommissionItemsService {

    public static final String SCREEN = "CommissionItems";

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;

    public Inv4bCommissionItemsService(JdbcTemplate jdbc, CurrentUserContext ctx, StoreScreenRights rights) {
        this.jdbc = jdbc; this.ctx = ctx; this.rights = rights;
    }

    /** InvDeffrmWarehouse_Load :299 — rights + BindHistory. */
    public Map<String, Object> init() {
        ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN));
        out.put("history", history());
        return out;
    }

    /** BindHistory :100 — table Id, CommissionItemName, Percentage, EntryUser, EntryDate (ToShortDateString), IsActive (:125). */
    public List<Map<String, Object>> history() {
        ctx.requireAccountingUser();
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "USP_CommissionItems_FormHistory", params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId()));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, "Id")));
            o.put("CommissionItemName", str(ci(r, "CommissionItemName")));
            o.put("Percentage", dbl(ci(r, "CommissionPerncentage")));
            o.put("EntryUser", str(ci(r, "EntryUser")));
            o.put("EntryDate", day(ci(r, "EntryDate")));
            o.put("IsActive", str(ci(r, "IsActive")));          // the procedure returns the text 'True' / 'False'
            out.add(o);
        }
        return out;
    }

    /** ReadById :273 — USP_CommissionItems_ReadById; null when the row does not exist (or is another company's). */
    public Map<String, Object> read(int id) {
        ctx.requireAccountingUser();
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "USP_CommissionItems_ReadById", params("Id", id));
        if (rows.isEmpty()) return null;
        Map<String, Object> r = rows.get(0);
        if (num(ci(r, "OrganizationId")) != ctx.currentOrganizationId() || num(ci(r, "CompanyId")) != ctx.currentCompanyId()) return null;
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", num(ci(r, "Id")));
        o.put("CommissionItemName", str(ci(r, "CommissionItemName")));
        o.put("CommissionPerncentage", dbl(ci(r, "CommissionPerncentage")));
        o.put("IsActive", bool(ci(r, "IsActive")));
        return o;
    }

    /**
     * Insert :212. Body: {Id, CommissionItemName (txtcode), CommissionPerncentage (txtDescription text), IsActive}.
     * The page asks "Are you sure to Save/Update?" after {@link #validate} and before this call.
     */
    public Map<String, Object> save(Map<String, Object> body) {
        ctx.requireAccountingUser();
        int recId = num(body.get("Id"));
        validate(body);
        if (recId == 0 && !rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId > 0 && !rights.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        if (recId > 0 && read(recId) == null) throw new IllegalArgumentException("Record Not Found");
        String pct = str(body.get("CommissionPerncentage"));
        double percentage = dbl(pct);                                   // Conversion.ToDouble(txtDescription.Text) :242
        if (percentage > 100.0) throw new IllegalArgumentException("Commission Perncentage % Should Not Be Greater Than 100");   // :245
        java.sql.Timestamp now = now();
        Map<String, Object> p = params(
                "Id", recId,
                "CommissionItemName", str(body.get("CommissionItemName")),
                "EntryDate", now,
                "EntryUserId", ctx.currentUserId(),
                "ModifyDate", now,
                "ModifyUserId", ctx.currentUserId(),
                "IsActive", bool(body.get("IsActive")),
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "CommissionPerncentage", percentage);
        int id;
        if (recId == 0) {
            id = DesktopProc.setProc(jdbc, "USP_CommissionItems_Insert", p);        // SELECT @Id
        } else {
            DesktopProc.scalar(jdbc, "USP_CommissionItems_Update", p);             // no result set
            id = recId;
        }
        /* :256-263 — "Save Successfully" only when the insert returned an id; "Update Successfully" always on update. */
        String msg = recId == 0 ? (id > 0 ? "Save Successfully" : "") : "Update Successfully";
        return ok(msg, id);
    }

    /** FormValiadation :83 — the two required-field messages, in order. */
    public void validate(Map<String, Object> body) {
        if (str(body.get("CommissionItemName")).isEmpty()) throw new IllegalArgumentException("Please Insert Commission Item Code");
        if (str(body.get("CommissionPerncentage")).isEmpty()) throw new IllegalArgumentException("Please Insert Commission %");
    }
}
