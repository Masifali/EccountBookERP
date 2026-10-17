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
import static com.mst.services.Inv4bDefinitionSupport.dateParam;
import static com.mst.services.Inv4bDefinitionSupport.dateTime;
import static com.mst.services.Inv4bDefinitionSupport.day;
import static com.mst.services.Inv4bDefinitionSupport.dbl;
import static com.mst.services.Inv4bDefinitionSupport.now;
import static com.mst.services.Inv4bDefinitionSupport.num;
import static com.mst.services.Inv4bDefinitionSupport.ok;
import static com.mst.services.Inv4bDefinitionSupport.str;

/**
 * Screen 96 "Customer Discount Policy" — Architecture.WinApp/CustomerDiscountPolicy.cs (base.Name
 * "CustomerDiscountPolicy", the rights key). Line refs ":NNN" are that file.
 *
 * <pre>
 *   InvfrmPriceSchedule_Load :436   rights, grid structure, GridFill, DiscountTypeFill, CustomerBind
 *   DiscountTypeFill :263           InvAllocateDiscItemToDiscType.GetDiscountTypes → Usp_GetDiscountTypes (Id / DiscountTypeCode)
 *   CustomerBind :287               CommonServices.GetSupplierustomerByCustomerGroupId("") →
 *                                   Sp_SupplierCustomer_GetAllMethod @Activity='GetSupplierustomerByCustomerGroupId' (Id / CompanyName)
 *   FillDiscountItems :303          InvAllocateDiscItemToDiscType.GetDiscountItemsFromAllocation → Usp_GetDiscountItemsFromAllocation
 *                                   (DiscountItemId / DiscountItemName; Cells[3] = DiscPerncentage → txtDiscountPercentage :348)
 *   GridFill :358                   BLL CustomerDiscountPolicy.FormHistory → Sp_CustomerDiscountPolicy_GetAllMethod @Activity='FormHistory'
 *   ReadById :233                   BLL GetByID → same procedure @Activity='ReadById'
 *   Insert :149                     BLL Save → Sp_CustomerDiscountPolicy_Insert (its @Id = 0 inserts, &gt; 0 updates), DiscountType "Percent"
 * </pre>
 */
@Service
public class Inv4bCustomerDiscountPolicyService {

    public static final String SCREEN = "CustomerDiscountPolicy";

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;

    public Inv4bCustomerDiscountPolicyService(JdbcTemplate jdbc, CurrentUserContext ctx, StoreScreenRights rights) {
        this.jdbc = jdbc; this.ctx = ctx; this.rights = rights;
    }

    /** InvfrmPriceSchedule_Load :436. */
    public Map<String, Object> init() {
        ctx.requireAccountingUser();
        Map<String, Object> out = lookups();
        out.put("rights", rights.of(SCREEN));
        out.put("history", history());
        return out;
    }

    /** BtnNew_Click :476 — DiscountTypeFill + CustomerBind (GridFill is /api/history). */
    public Map<String, Object> lookups() {
        ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("discountTypes", project(DesktopProc.rows(jdbc, "Usp_GetDiscountTypes", params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId())), "Id", "DiscountTypeCode"));
        out.put("customers", customers());
        return out;
    }

    List<Map<String, Object>> customers() {
        return project(DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "Activity", "GetSupplierustomerByCustomerGroupId")), "Id", "CompanyName");
    }

    /** FillDiscountItems :303 — value DiscountItemId, text DiscountItemName, DiscPerncentage. */
    public List<Map<String, Object>> discountItems(int discountTypeId) {
        ctx.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Usp_GetDiscountItemsFromAllocation", params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "DiscountTypeId", discountTypeId))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, "DiscountItemId")));
            o.put("Name", str(ci(r, "DiscountItemName")));
            o.put("Percentage", dbl(ci(r, "DiscPerncentage")));
            out.add(o);
        }
        return out;
    }

    /** GridFill :358 — dtGrid columns of :448-460 (EffectedDate ToShortDateString :376). */
    public List<Map<String, Object>> history() {
        ctx.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_CustomerDiscountPolicy_GetAllMethod", params(
                "OrgnaizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "Activity", "FormHistory"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, "Id")));
            o.put("EffectedDate", day(ci(r, "EffectiveDate")));
            o.put("CustomerId", num(ci(r, "CustomerId")));
            o.put("CustomerName", str(ci(r, "CustomerName")));
            o.put("DiscountType", str(ci(r, "DiscountTypeCode")));
            o.put("DiscountItem", str(ci(r, "DiscountItemName")));
            o.put("DiscountPct", dbl(ci(r, "DiscountRate")));          // "Discount%"
            o.put("ItemPct", dbl(ci(r, "DiscPerncentage")));           // "Item%" (hidden)
            o.put("IsActive", bool(ci(r, "IsActive")));
            o.put("EntryUser", str(ci(r, "EntryUser")));
            o.put("EntryDate", dateTime(ci(r, "EntryDate")));
            o.put("ModifyUser", str(ci(r, "ModifyUser")));
            o.put("ModifyDate", dateTime(ci(r, "ModifyDate")));
            out.add(o);
        }
        return out;
    }

    /** ReadById :233. */
    public Map<String, Object> read(int id) {
        ctx.requireAccountingUser();
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_CustomerDiscountPolicy_GetAllMethod", params("Id", id, "Activity", "ReadById"));
        if (rows.isEmpty()) return null;
        Map<String, Object> r = rows.get(0);
        if (num(ci(r, "OrganizationId")) != ctx.currentOrganizationId() || num(ci(r, "CompanyId")) != ctx.currentCompanyId()) return null;
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", num(ci(r, "Id")));
        o.put("CustomerId", num(ci(r, "CustomerId")));
        o.put("EffectiveDate", day(ci(r, "EffectiveDate")));
        o.put("DiscountTypeId", num(ci(r, "DiscountTypeId")));
        o.put("DiscountRate", dbl(ci(r, "DiscountRate")));
        o.put("DiscountItemId", num(ci(r, "DiscountItemId")));
        o.put("IsActive", bool(ci(r, "IsActive")));
        return o;
    }

    /** FormValidation :113 — ActiveRow == null tests become "no value"; the rate test is on the TEXT being empty. */
    public void validate(Map<String, Object> b) {
        if (num(b.get("CustomerId")) == 0) throw new IllegalArgumentException("Customer Name Field is Required");
        if (num(b.get("DiscountTypeId")) == 0) throw new IllegalArgumentException("Discount Type Field is Required");
        if (str(b.get("DiscountRate")).isEmpty()) throw new IllegalArgumentException("Discount Rate Field is Required");
    }

    /** Insert :149. */
    public Map<String, Object> save(Map<String, Object> b) {
        ctx.requireAccountingUser();
        int rId = num(b.get("Id"));
        validate(b);
        if (rId == 0 && !rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (rId > 0 && !rights.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        if (rId > 0 && read(rId) == null) throw new IllegalArgumentException("Record not update because Id not found");
        boolean known = false;
        for (Map<String, Object> c : customers()) if (num(c.get("Id")) == num(b.get("CustomerId"))) { known = true; break; }
        if (!known) throw new IllegalArgumentException("Customer Name Field is Required");
        java.sql.Timestamp now = now();
        Map<String, Object> p = params(
                "Id", rId,
                "CustomerId", num(b.get("CustomerId")),
                "EffectiveDate", dateParam(str(b.get("EffectiveDate"))),
                "DiscountType", "Percent",                                         // :185
                "DiscountRate", dbl(b.get("DiscountRate")),
                "IsActive", bool(b.get("IsActive")),
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "DiscountItemId", num(b.get("DiscountItemId")),
                "DiscountTypeId", num(b.get("DiscountTypeId")),
                "EntryUserId", ctx.currentUserId(),
                "EntryDate", now,
                "ModifyUserId", ctx.currentUserId(),
                "ModifyDate", now);
        int id = rId;
        if (rId == 0) id = DesktopProc.setProc(jdbc, "Sp_CustomerDiscountPolicy_Insert", p);      // SELECT @Id
        else DesktopProc.scalar(jdbc, "Sp_CustomerDiscountPolicy_Insert", p);
        return ok(rId > 0 ? "Data Update Successfully...." : "Data Save Successfully....", id);
    }

    private static List<Map<String, Object>> project(List<Map<String, Object>> rows, String idCol, String nameCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, idCol)));
            o.put("Name", str(ci(r, nameCol)));
            out.add(o);
        }
        return out;
    }
}
