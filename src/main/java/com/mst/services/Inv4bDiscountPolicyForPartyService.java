package com.mst.services;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;
import static com.mst.services.Inv4bDefinitionSupport.bool;
import static com.mst.services.Inv4bDefinitionSupport.ci;
import static com.mst.services.Inv4bDefinitionSupport.dateParam;
import static com.mst.services.Inv4bDefinitionSupport.day;
import static com.mst.services.Inv4bDefinitionSupport.dbl;
import static com.mst.services.Inv4bDefinitionSupport.num;
import static com.mst.services.Inv4bDefinitionSupport.ok;
import static com.mst.services.Inv4bDefinitionSupport.str;

/**
 * Screen 98 "Discount Policy For Party" — Architecture.WinApp/DiscountPolicyForParty.cs (base.Name
 * "DiscountPolicyForParty", the rights key). Line refs ":NNN" are that file.
 *
 * <pre>
 *   InvfrmPriceSchedule_Load :279   rights, GridFill, DiscountTypeFill
 *   DiscountTypeFill :199           in-code table: Percent / Weight / Flat (all with Id 1 — :206-208); Rows[0] activated
 *   GridFill :238                   BLL DiscountPolicy.GetAll → Sp_DiscountPolicy_GetAllMethod @Activity='ReadAll'
 *   grd_DoubleClick :316            BLL DiscountPolicy.GetByID → same procedure @Activity='ReadById'
 *   Insert :128                     BLL DiscountPolicy.Save → Sp_DiscountPolicy_Insert (RId 0, SELECT @Id) / Sp_DiscountPolicy_Update
 *                                   DiscountType = cmbDiscountType.Text (:154)
 * </pre>
 *
 * There is no Edit column and no rights test on the grid: a double-click opens the row (:316).
 */
@Service
public class Inv4bDiscountPolicyForPartyService {

    public static final String SCREEN = "DiscountPolicyForParty";
    /** DiscountTypeFill :206-208 — the three texts the combo offers (the saved column is the text). */
    public static final List<String> DISCOUNT_TYPES = Arrays.asList("Percent", "Weight", "Flat");

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;

    public Inv4bDiscountPolicyForPartyService(JdbcTemplate jdbc, CurrentUserContext ctx, StoreScreenRights rights) {
        this.jdbc = jdbc; this.ctx = ctx; this.rights = rights;
    }

    /** InvfrmPriceSchedule_Load :279. */
    public Map<String, Object> init() {
        ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN));
        out.put("discountTypes", DISCOUNT_TYPES);
        out.put("history", history());
        return out;
    }

    /** GridFill :238 — Id, EffectedDate (ToShortDateString), PolicyName, DiscountType, DiscountRate, IsActive (:265). */
    public List<Map<String, Object>> history() {
        ctx.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_DiscountPolicy_GetAllMethod", params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "Activity", "ReadAll"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, "Id")));
            o.put("EffectedDate", day(ci(r, "EffectiveDate")));
            o.put("PolicyName", str(ci(r, "PolicyName")));
            o.put("DiscountType", str(ci(r, "DiscountType")));
            o.put("DiscountRate", dbl(ci(r, "DiscountRate")));
            o.put("IsActive", bool(ci(r, "IsActive")));
            out.add(o);
        }
        return out;
    }

    /** grd_DoubleClick :316 — GetByID. */
    public Map<String, Object> read(int id) {
        ctx.requireAccountingUser();
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_DiscountPolicy_GetAllMethod", params("Id", id, "Activity", "ReadById"));
        if (rows.isEmpty()) return null;
        Map<String, Object> r = rows.get(0);
        if (num(ci(r, "OrganizationId")) != ctx.currentOrganizationId() || num(ci(r, "CompanyId")) != ctx.currentCompanyId()) return null;
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", num(ci(r, "Id")));
        o.put("PolicyName", str(ci(r, "PolicyName")));
        o.put("EffectiveDate", day(ci(r, "EffectiveDate")));
        o.put("DiscountType", str(ci(r, "DiscountType")));
        o.put("DiscountRate", dbl(ci(r, "DiscountRate")));
        o.put("IsActive", bool(ci(r, "IsActive")));
        return o;
    }

    /** FormValidation :92. */
    public void validate(Map<String, Object> b) {
        if (str(b.get("PolicyName")).isEmpty()) throw new IllegalArgumentException("Policy Name Field is Required");
        if (str(b.get("DiscountType")).isEmpty()) throw new IllegalArgumentException("Discount Type Field is Required");
        if (str(b.get("DiscountRate")).isEmpty()) throw new IllegalArgumentException("Discount Rate Field is Required");
    }

    /** Insert :128 — Save (RId 0) or Update (RId &gt; 0, BtnEdit_Click :183 refuses RId 0). */
    public Map<String, Object> save(Map<String, Object> b) {
        ctx.requireAccountingUser();
        int rId = num(b.get("Id"));
        validate(b);
        if (rId == 0 && !rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (rId > 0 && !rights.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        if (rId > 0 && read(rId) == null) throw new IllegalArgumentException("Record not update because Id not found");
        Map<String, Object> p = params(
                "Id", rId,
                "PolicyName", str(b.get("PolicyName")),
                "EffectiveDate", dateParam(str(b.get("EffectiveDate"))),
                "DiscountType", str(b.get("DiscountType")),
                "DiscountRate", dbl(b.get("DiscountRate")),
                "IsActive", bool(b.get("IsActive")),
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId());
        int id = rId;
        if (rId == 0) id = DesktopProc.setProc(jdbc, "Sp_DiscountPolicy_Insert", p);      // SELECT @Id
        else DesktopProc.scalar(jdbc, "Sp_DiscountPolicy_Update", p);
        return ok(rId > 0 ? "Data Update Successfully...." : "Data Save Successfully....", id);
    }
}
