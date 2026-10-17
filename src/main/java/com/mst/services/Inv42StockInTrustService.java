package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;
import static com.mst.services.Inv42Support.day;
import static com.mst.services.Inv42Support.dateTime;
import static com.mst.services.Inv42Support.str;
import static com.mst.services.Inv42Support.toDouble;
import static com.mst.services.Inv42Support.toInt;

/**
 * Inventory hub, module 137 "Stock in Trust" — screen 963 "Stock In Trust", desktop
 * {@code Architecture.WinApp.Inventory_Reports.frmGatePassForStockInTrust}. Line refs ":NNN" are that file.
 *
 * <pre>
 *   frmGatePassForStockReseved_Load :118   ALlDropDown, IsAcceptFill, BranchesFill, Datetypefill
 *   ALlDropDown :141                        GatePassInward.GetDataForDropDownFromGPI(org, comp, null, BranchIds) → USP_GetDataForDropDownFromGPI
 *                                           (Activity "Supplier" / "GatePassType")
 *   BranchesFill :218                       GetBranchesAllocatedToUserFromInwardGatePass(org, comp, user, 51) → USP_GetBranchsAllocatedToUserFromGatePassInward
 *   GridFill :311                           GatePassInward.GatePassInward_StockReservedAsAmanat → Sp_GatePassInward_StockReservedAsAmanat
 *   grdfrm "Update" :778                    GatePassInward.GatePassInwardStatusUpdate → USP_GatePassInward_StatusUpdateForStockInTrust
 *   btnPrint_Click :540                     253_01-InvRptGatePassInwardRegisterA.rpt with the grid rows (POST /reports/print/grid)
 *   grdfrm GpSrNo link :507                 GatePassInwardSlipAndRegisterReport → 251-InvRptInwardGatePassSlip.rpt (existing route)
 * </pre>
 *
 * The "Select Branch First" test (:391), the "Please select vehicle status first..." test (:794) and the
 * "Record update successfully." message (:802) are the desktop's.
 */
@Service
public class Inv42StockInTrustService {

    public static final int SCREEN_ID = 963;
    public static final int GP_DOC_TYPE = 51;

    private final Inv42Support db;
    private final CurrentUserContext ctx;
    private final DesktopReportRights rights;

    public Inv42StockInTrustService(Inv42Support db, CurrentUserContext ctx, DesktopReportRights rights) {
        this.db = db;
        this.ctx = ctx;
        this.rights = rights;
    }

    /** Form_Load :118 — branches, status list, date types, the financial year start (Datetypefill default is "Financial Year"). */
    public Map<String, Object> init() {
        UserAccount u = ctx.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("branches", branches(u));
        out.put("userBranchId", toInt(u.getBranchesId()));
        out.put("financialYearStart", db.financialYearStart(u, ctx.currentFinancialYearId()));
        List<Map<String, Object>> statuses = new ArrayList<>();                       // IsAcceptFill :194
        String[] st = { "Accept As Reserved", "Accepted", "Rejected" };
        for (int i = 0; i < st.length; i++) { Map<String, Object> o = new LinkedHashMap<>(); o.put("Id", i + 1); o.put("Name", st[i]); statuses.add(o); }
        out.put("statuses", statuses);
        return out;
    }

    /** BranchesFill :218 — USP_GetBranchsAllocatedToUserFromGatePassInward (BranchId, BranchName), DocumentTypeId 51. */
    private List<Map<String, Object>> branches(UserAccount u) {
        return Inv42Support.project(db.rows("USP_GetBranchsAllocatedToUserFromGatePassInward", params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "UserId", u.getId(), "DocumentTypeId", GP_DOC_TYPE)),
                "BranchId", "BranchName");
    }

    /** ALlDropDown :141 — USP_GetDataForDropDownFromGPI with the checked branch ids (",1,2" as the desktop builds it). */
    public Map<String, List<Map<String, Object>>> dropDowns(String branchIds) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, List<Map<String, Object>>> by = new LinkedHashMap<>();
        by.put("Supplier", new ArrayList<>());
        by.put("GatePassType", new ArrayList<>());
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        String ids = csv(branchIds);
        if (!ids.isEmpty()) p.put("BranchesIds", ids);
        for (Map<String, Object> r : db.rows("USP_GetDataForDropDownFromGPI", p)) {
            List<Map<String, Object>> t = by.get(str(r.get("Activity")));
            if (t == null) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("ReferenceName")));
            t.add(o);
        }
        return by;
    }

    /** GridFill :311 — Sp_GatePassInward_StockReservedAsAmanat; "Select Branch First" without a branch (:391). */
    public List<Map<String, Object>> show(Map<String, Object> f) {
        UserAccount u = ctx.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
        String ids = csv(str(f.get("branchIds")));
        if (ids.isEmpty()) throw new IllegalArgumentException("Select Branch First");
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        Timestamp from = Inv42Support.ts(f.get("fromDate")), to = Inv42Support.ts(f.get("toDate"));
        if (from != null) p.put("FromDate", from);
        if (to != null) p.put("ToDate", to);
        if (toInt(f.get("fromDocNo")) != 0) p.put("FromDocNo", toInt(f.get("fromDocNo")));
        if (toInt(f.get("toDocNo")) != 0) p.put("ToDocNo", toInt(f.get("toDocNo")));
        if (toInt(f.get("supplierCustomerId")) != 0) p.put("SupplierCustomerId", toInt(f.get("supplierCustomerId")));
        if (!str(f.get("status")).trim().isEmpty()) p.put("Status", str(f.get("status")).trim());
        if (!str(f.get("gatepassType")).trim().isEmpty()) p.put("GatepassType", str(f.get("gatepassType")).trim());
        p.put("BranchesIds", ids);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : db.rows("Sp_GatePassInward_StockReservedAsAmanat", p)) {
            Map<String, Object> o = new LinkedHashMap<>();           // dtTable :347-374 (the raw row for the .rpt is kept too)
            o.put("Id", toInt(r.get("Id")));
            o.put("BranchName", str(r.get("BranchName")));
            o.put("OrderType", str(r.get("OrderType")));
            o.put("GpSrNo", toInt(r.get("GpSrNo")));
            o.put("GpDate", day(r.get("GpDate")));
            o.put("GatePassType", str(r.get("GatepassType")));
            o.put("VehicleType", str(r.get("VehicleType")));
            o.put("VehicleNo", str(r.get("VehicleNo")));
            o.put("BiltyNo", str(r.get("BiltyNo")));
            o.put("OrderNo", str(r.get("OrderNo")));
            o.put("SupplierName", str(r.get("CompanyName")));
            o.put("ItemName", str(r.get("VarietyName")));
            o.put("ItemQty", toDouble(r.get("ItemQty")));
            o.put("SupplierFirstWeight", toDouble(r.get("SupplierFirstWeight")));
            o.put("SupplierSecondWeight", toDouble(r.get("SupplierSecondWeight")));
            o.put("SupplierWeight", toDouble(r.get("SupplierWeight")));
            o.put("FactoryWeight", toDouble(r.get("FactoryWeight")));
            o.put("DifferenceWeight", toDouble(r.get("DifferenceWeight")));
            o.put("Freight", toDouble(r.get("Freight")));
            o.put("NetPaid", toDouble(r.get("NetPaid")));
            o.put("Status", str(r.get("Status")));
            o.put("StatusUpdate", "");
            o.put("CityName", str(r.get("Description")));
            o.put("EntryUser", str(r.get("UserName")));
            o.put("Remarks", str(r.get("OtherRemarks")));
            o.put("InTime", dateTime(r.get("InTime")));
            o.put("OutTime", dateTime(r.get("OutTime")));
            o.put("NoOfAttachments", toInt(r.get("NoOfAttachments")));
            Map<String, Object> raw = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                if (v instanceof byte[]) continue;                  // CompLogoImage
                raw.put(e.getKey(), v instanceof java.util.Date ? dateTime(v) : v);
            }
            o.put("raw", raw);
            out.add(o);
        }
        return out;
    }

    /** grdfrm "Update" :778 — GatePassInward.GatePassInwardStatusUpdate(EntryUser, Id, ReqType = status text). */
    @Transactional
    public Map<String, Object> updateStatus(int id, String status) {
        UserAccount u = ctx.requireAccountingUser();
        rights.require(u, SCREEN_ID, "Update");
        if (id <= 0) throw new IllegalArgumentException("Record Not Found");
        if (str(status).trim().isEmpty()) throw new IllegalArgumentException("Please select vehicle status first...");
        Integer n = db.jdbc().queryForObject("SELECT COUNT(1) FROM dbo.GatePassInward WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?",
                Integer.class, id, u.getOrganizationId(), u.getCompanyId());
        if (n == null || n == 0) throw new IllegalArgumentException("Record Not Found");
        db.scalar("USP_GatePassInward_StatusUpdateForStockInTrust", params("Id", id, "UserId", u.getId(), "Status", status.trim()));
        return Inv42Support.ok("Record update successfully.", id, null);
    }

    /** ",1,2" → "1,2" with only the digits kept. */
    private static String csv(String s) {
        StringBuilder sb = new StringBuilder();
        for (String part : str(s).split(",")) {
            String t = part.trim();
            if (t.isEmpty() || !t.matches("\\d+")) continue;
            if (sb.length() > 0) sb.append(',');
            sb.append(t);
        }
        return sb.toString();
    }
}
