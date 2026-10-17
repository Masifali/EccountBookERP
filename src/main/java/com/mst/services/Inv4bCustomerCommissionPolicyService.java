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
 * Screen 95 "Customer Commission Policy" — Architecture.WinApp.WholeSale.Commission/CustomerCommissionPolicy.cs
 * (base.Name "CustomerCommissionPolicy", the rights key). Line refs ":NNN" are that file.
 *
 * <pre>
 *   InvfrmPriceSchedule_Load :115   rights, CommissionTypeFill, CustomerBind, GridFill
 *   CommissionTypeFill :139         BLL CommissionType.FormHistory → USP_CommissionType_History (Id / CommissionTypeDescription)
 *   CustomerBind :180               CommonServices.GetSupplierustomerByCustomerGroupId("") →
 *                                   Sp_SupplierCustomer_GetAllMethod @Activity='GetSupplierustomerByCustomerGroupId'
 *                                   (Id / CompanyName) — binds cmbCustomer AND CmbIncentiveAccount
 *   cmbDiscountType_Leave :254      AllocateCommissionItemToCommissionType.CommissionItemsFromAllocationByCommissionTypeId →
 *                                   USP_CommissionItemsFromAllocationByCommissionTypeId (CommissionItemId / CommissionItemName,
 *                                   Cells[3] = CommissionPerncentage copied into txtCommissionPercentage :275)
 *   GridFill :457                   BLL CustomerCommissionPolicy.FormHistory → USP_CustomerCommissionPolicy_GetAllMethod
 *                                   @Activity='FormHistory', @PolicyTypeId=1
 *   ReadById :414                   BLL GetByID → same procedure @Activity='ReadById'
 *   Insert :343                     BLL Save → USP_CustomerCommissionPolicy_InsertAndUpdate (Id 0 inserts, else updates),
 *                                   CommissionType "Percent", PolicyTypeId 1
 * </pre>
 *
 * The desktop's "" CustomerGroupId reaches the procedure as an omitted @CustomerGroupIds (ADO.NET drops a null; an
 * empty string would match no group), i.e. every non-taxable party of the company.
 */
@Service
public class Inv4bCustomerCommissionPolicyService {

    public static final String SCREEN = "CustomerCommissionPolicy";
    /** Insert :386 — PolicyTypeId 1 = customer (2 = commission agent / broker). */
    public static final int POLICY_TYPE = 1;

    private final JdbcTemplate jdbc;
    private final CurrentUserContext ctx;
    private final StoreScreenRights rights;

    public Inv4bCustomerCommissionPolicyService(JdbcTemplate jdbc, CurrentUserContext ctx, StoreScreenRights rights) {
        this.jdbc = jdbc; this.ctx = ctx; this.rights = rights;
    }

    /** InvfrmPriceSchedule_Load :115. */
    public Map<String, Object> init() {
        ctx.requireAccountingUser();
        Map<String, Object> out = lookups();
        out.put("rights", rights.of(SCREEN));
        out.put("history", history());
        return out;
    }

    /** BtnRefresh_Click :625 — CommissionTypeFill + CustomerBind. */
    public Map<String, Object> lookups() {
        ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("commissionTypes", project(DesktopProc.rows(jdbc, "USP_CommissionType_History", params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId())), "Id", "CommissionTypeDescription"));
        out.put("customers", customers());
        return out;
    }

    /** CommonServices.GetSupplierustomerByCustomerGroupId("") — Id / CompanyName. */
    List<Map<String, Object>> customers() {
        return project(DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", params(
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "Activity", "GetSupplierustomerByCustomerGroupId")), "Id", "CompanyName");
    }

    /** CommissionItemsByCommissionTypeId :228 — rows for CmbCommissionItem (value CommissionItemId, text CommissionItemName, rate). */
    public List<Map<String, Object>> commissionItems(int commissionTypeId) {
        ctx.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_CommissionItemsFromAllocationByCommissionTypeId", params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "CommissionTypeId", commissionTypeId))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, "CommissionItemId")));
            o.put("Name", str(ci(r, "CommissionItemName")));
            o.put("Percentage", dbl(ci(r, "CommissionPerncentage")));
            out.add(o);
        }
        return out;
    }

    /** GridFill :457 — the dt columns of :470-487 in order. */
    public List<Map<String, Object>> history() {
        ctx.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "USP_CustomerCommissionPolicy_GetAllMethod", params(
                "OrgnaizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "PolicyTypeId", POLICY_TYPE, "Activity", "FormHistory"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", num(ci(r, "Id")));
            o.put("CustomerId", num(ci(r, "CustomerId")));
            o.put("CustomerName", str(ci(r, "CustomerName")));
            o.put("EffectiveDate", day(ci(r, "EffectiveDate")));
            o.put("CommissionType", str(ci(r, "CommissionType")));
            o.put("CommissionRate", dbl(ci(r, "CommissionRate")));
            o.put("IsActive", bool(ci(r, "IsActive")));
            o.put("CommissionItemsId", num(ci(r, "CommissionItemsId")));
            o.put("CommissionItemName", str(ci(r, "CommissionItemName")));
            o.put("CommissionTypeId", num(ci(r, "CommissionTypeId")));
            o.put("CommissionTypeCode", str(ci(r, "CommissionTypeCode")));
            o.put("CommissionPerncentage", dbl(ci(r, "CommissionPerncentage")));
            o.put("IncentiveAccount", str(ci(r, "IncentiveParty")));
            o.put("EntryDate", dateTime(ci(r, "EntryDate")));
            o.put("EntryUserName", str(ci(r, "EntryUserName")));
            o.put("ModifyDate", dateTime(ci(r, "ModifyDate")));
            o.put("ModifyUserName", str(ci(r, "ModifyUserName")));
            out.add(o);
        }
        return out;
    }

    /** ReadById :414 — @Activity='ReadById'; null when missing or another company's. */
    public Map<String, Object> read(int id) {
        ctx.requireAccountingUser();
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "USP_CustomerCommissionPolicy_GetAllMethod", params("Id", id, "Activity", "ReadById"));
        if (rows.isEmpty()) return null;
        Map<String, Object> r = rows.get(0);
        if (num(ci(r, "OrganizationId")) != ctx.currentOrganizationId() || num(ci(r, "CompanyId")) != ctx.currentCompanyId()) return null;
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", num(ci(r, "Id")));
        o.put("CustomerId", num(ci(r, "CustomerId")));
        o.put("CustomerIncentivePartyId", num(ci(r, "CustomerIncentivePartyId")));
        o.put("EffectiveDate", day(ci(r, "EffectiveDate")));
        o.put("CommissionTypeId", num(ci(r, "CommissionTypeId")));
        o.put("CommissionRate", dbl(ci(r, "CommissionRate")));
        o.put("CommissionItemsId", num(ci(r, "CommissionItemsId")));
        o.put("IsActive", bool(ci(r, "IsActive")));
        return o;
    }

    /** FormValidation :301 — the four messages in order. Body ids are the combo values (0 = nothing selected). */
    public void validate(Map<String, Object> b) {
        if (num(b.get("CustomerId")) == 0) throw new IllegalArgumentException("Customer Name Field is Required");
        if (num(b.get("CommissionTypeId")) == 0) throw new IllegalArgumentException("Commission Type Field is Required");
        if (dbl(b.get("CommissionRate")) == 0.0) throw new IllegalArgumentException("Commission Rate Field is Required");
        if (num(b.get("CustomerIncentivePartyId")) == 0) throw new IllegalArgumentException("Incentive Account Field is Required");
    }

    /** Insert :343 (BtnSave sets RId 0; BtnEdit :441 needs RId &gt; 0). */
    public Map<String, Object> save(Map<String, Object> b) {
        ctx.requireAccountingUser();
        int rId = num(b.get("Id"));
        validate(b);
        if (rId == 0 && !rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (rId > 0 && !rights.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        if (rId > 0 && read(rId) == null) throw new IllegalArgumentException("Record not update because Id not found");
        /* the posted party / type ids must be rows the page was offered (tenancy; the desktop combos cannot hold others) */
        requireId(customers(), num(b.get("CustomerId")), "Customer Name Field is Required");
        requireId(customers(), num(b.get("CustomerIncentivePartyId")), "Incentive Account Field is Required");
        java.sql.Timestamp now = now();
        Map<String, Object> p = params(
                "Id", rId,
                "CustomerId", num(b.get("CustomerId")),
                "EffectiveDate", dateParam(str(b.get("EffectiveDate"))),
                "CommissionType", "Percent",                                       // :378
                "CommissionRate", dbl(b.get("CommissionRate")),
                "IsActive", bool(b.get("IsActive")),
                "OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(),
                "CommissionItemsId", num(b.get("CommissionItemsId")),
                "CommissionTypeId", num(b.get("CommissionTypeId")),
                "EntryUserId", ctx.currentUserId(),
                "EntryDate", now,
                "ModifyUserId", ctx.currentUserId(),
                "ModifyDate", now,
                "CustomerIncentivePartyId", num(b.get("CustomerIncentivePartyId")),
                "PolicyTypeId", POLICY_TYPE);
        int id = rId;
        if (rId == 0) id = DesktopProc.setProc(jdbc, "USP_CustomerCommissionPolicy_InsertAndUpdate", p);     // SELECT @Id
        else DesktopProc.scalar(jdbc, "USP_CustomerCommissionPolicy_InsertAndUpdate", p);
        return ok(rId > 0 ? "Data Update Successfully...." : "Data Save Successfully....", id);
    }

    private static void requireId(List<Map<String, Object>> rows, int id, String message) {
        for (Map<String, Object> r : rows) if (num(r.get("Id")) == id) return;
        throw new IllegalArgumentException(message);
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
