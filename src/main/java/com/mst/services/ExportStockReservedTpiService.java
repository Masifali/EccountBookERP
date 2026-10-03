package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportStockReservedTpiRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;
import static com.mst.services.ExportQSupport.*;

/**
 * BLL side of frmStockReservedAgainstThirdPartyInspection (Architecture.WinApp.Export) "Stock Reserved Against
 * third Party Analysis" - /export/stock-reserved-against-tpi[?trackingId=]. No ScreenDefinition row; it opens
 * from 857 Third Party Inspection / 858 Lab Against Third Party Inspection, whose rights gate it ("View" for
 * every action - the desktop form has no rights code). Tenancy, branch, user and financial year come from the
 * session.
 */
@Service
public class ExportStockReservedTpiService {

    public static final int[] PARENTS = {857, 858};
    /** RefDocumentTypeId the form writes on every reserved row (the lab pre-production lot inspection). */
    public static final int REF_DOCUMENT_TYPE_ID = 204;

    @Autowired private ExportStockReservedTpiRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user() {
        UserAccount u = currentUserContext.requireAccountingUser();
        requireAny(rights, u, PARENTS, "View");
        return u;
    }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }

    /**
     * InitializeComponentCustom (FromDate = ActiveYr.Start_Period) + InitializeComponentMethod (ComboDbCall ->
     * StockComboFill, TrackingNoDbCall -> TackingNoBind). The two reads run in one Task on the desktop; a failure
     * of either shows "Error occurred during database call." and binds nothing.
     */
    public Map<String, Object> setup() {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>();
        try {
            Map<String, Object> fy = repo.financialYear(u, currentUserContext.currentFinancialYearId());
            out.put("yearStart", fy == null ? "" : iso(ci(fy, "Start_Period")));
        } catch (Exception e) { out.put("yearStart", ""); }
        try {
            List<Map<String, Object>> combos = repo.dropDownsAndLists(u);
            List<Map<String, Object>> tracking = repo.trackingNos(u);
            splitCombos(out, combos);
            out.put("trackingNos", project(tracking, "Id", "Id", "name", "LotRefNo"));
        } catch (Exception e) {
            out.put("setupError", "Error occurred during database call.");
        }
        return out;
    }

    /** btnRefresh_Click: StockComboFill(ComboDbCall()) + TackingNoBind(TrackingNoDbCall()); errors as ex.Message. */
    public Map<String, Object> refresh() {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>();
        splitCombos(out, repo.dropDownsAndLists(u));
        out.put("trackingNos", project(repo.trackingNos(u), "Id", "Id", "name", "LotRefNo"));
        return out;
    }

    /** StockComboFill: one Id/name list per ActivityType (Stock_Account is collected but bound to nothing). */
    private static void splitCombos(Map<String, Object> out, List<Map<String, Object>> dt) {
        String[][] map = {
                {"ParentCategories", "parentCategories"}, {"JobLot", "jobLots"}, {"CropYear", "cropYears"},
                {"Warehouse", "warehouses"}, {"DocumentType", "documentTypes"}, {"Supplier_Customer", "parties"},
                {"Items", "items"}};
        for (String[] m : map) {
            List<Map<String, Object>> rows = new ArrayList<>();
            for (Map<String, Object> r : dt) {
                if (!m[0].equals(text(ci(r, "ActivityType")))) continue;
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", ci(r, "Id"));
                o.put("name", text(ci(r, "name")));
                rows.add(o);
            }
            out.put(m[1], rows);
        }
    }

    /**
     * PendingTransactions (btnShow_Click "Search", Ctrl+S, also after Reset): ReportsParameters from the
     * filters - BranchesId (session), FromDate / ToDate, party, item, ref document type, job lot, warehouse,
     * parent category (combo values), CropYear = the crop-year combo's TEXT - each sent only when set, as BLL 0574
     * does. Rows are projected into dtGrid's 41 columns; GrnNo is 0 for RefDocumentTypeId 112 and 80.
     */
    public List<Map<String, Object>> search(Map<String, Object> body) {
        UserAccount u = user();
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId());
        if (branch(u) != 0) p.put("BranchesId", branch(u));
        LocalDate from = asDate(body.get("fromDate")), to = asDate(body.get("toDate"));
        p.put("DateFrom", java.sql.Date.valueOf(from == null ? LocalDate.now() : from));
        p.put("DateTo", java.sql.Date.valueOf(to == null ? LocalDate.now() : to));
        putIf(p, "SupplierCustomerId", asInt(body.get("supplierId")));
        putIf(p, "WarehouseId", asInt(body.get("warehouseId")));
        putIf(p, "ItemId", asInt(body.get("itemId")));
        putIf(p, "ReferenceDocumentTypeId", asInt(body.get("refDocumentTypeId")));
        putIf(p, "JobLotId", asInt(body.get("jobLotId")));
        putIf(p, "InventoryParentCategories", asInt(body.get("parentCategoryId")));
        String crop = raw(body.get("cropYearText"));
        if (!crop.isEmpty()) p.put("CropYear", crop);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.availableTransactions(p)) {
            int refType = asInt(ci(r, "RefDocumentTypeId"));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("RefDocumentTypeId", refType);
            m.put("RefDocIdNo", asInt(ci(r, "RefDocIdNo")));
            m.put("RefDocSubIdNo", asInt(ci(r, "RefDocSubIdNo")));
            m.put("RefDocumentType", text(ci(r, "RefDocumentType")));
            m.put("DocDate", isoDateTime(ci(r, "DocDate")));
            m.put("DocCodeNo", asInt(ci(r, "DocCodeNo")));
            m.put("ManualNo", text(ci(r, "ManualNo")));
            m.put("GrnNo", (refType == 112 || refType == 80) ? 0 : asInt(ci(r, "GrnNo")));
            m.put("SupplierCustomerId", asInt(ci(r, "SupplierCustomerId")));
            m.put("PartyName", text(ci(r, "SupplierCustomerName")));
            m.put("GpNo", text(ci(r, "GpNo")));
            m.put("VehicleNo", text(ci(r, "VehicleNo")));
            m.put("BiltyNo", text(ci(r, "BiltyNo")));
            m.put("Remarks", text(ci(r, "TranRemarks")));
            m.put("WarehouseId", asInt(ci(r, "WarehouseId")));
            m.put("WareHouse", text(ci(r, "WareHouseCode")));
            m.put("ItemId", asInt(ci(r, "ItemId")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("ItemCode", text(ci(r, "ItemCode")));
            m.put("CropYearId", asInt(ci(r, "CropYearId")));
            m.put("CropYear", text(ci(r, "CropBatch")));
            m.put("JobLotId", asInt(ci(r, "JobLotId")));
            m.put("JobLot", text(ci(r, "JobLotCode")));
            m.put("InvPackingTypeId", asInt(ci(r, "InvPackingTypeId")));
            m.put("PackingType", text(ci(r, "PackingType")));
            m.put("ItemUom", text(ci(r, "ItemUom")));
            m.put("PackUom", text(ci(r, "PackUom")));
            m.put("PackEquivalent", asDouble(ci(r, "PackSize")));
            m.put("ItemQty", asDouble(ci(r, "QtyBalance")));
            m.put("NetWeight", asDouble(ci(r, "WeightBalance")));
            m.put("QtyIn", asDouble(ci(r, "QtyIn")));
            m.put("QtyOut", asDouble(ci(r, "QtyOut")));
            m.put("BalanceQty", asDouble(ci(r, "QtyBalance")));
            m.put("WeightIn", asDouble(ci(r, "WeightIn")));
            m.put("WeightOut", asDouble(ci(r, "WeightOut")));
            m.put("BalanceWeight", asDouble(ci(r, "WeightBalance")));
            m.put("AVgRate", asDouble(ci(r, "AVgRate")));
            m.put("RateUomId", asInt(ci(r, "RateUomId")));
            m.put("RateUom", text(ci(r, "RateUom")));
            m.put("RateEquivalent", asDouble(ci(r, "Equivalent")));
            m.put("ItemAmount", asDouble(ci(r, "ItemAmount")));
            out.add(m);
        }
        return out;
    }

    private static void putIf(Map<String, Object> p, String k, int v) { if (v != 0) p.put(k, v); }

    /**
     * btnsave_Click: no checked row -> "Please select at least one record to save."; no third party analysis ->
     * "Please select a third party analysis."; per checked row the three quantity checks (desktop texts);
     * one InventoryStockReserved per row (ActionId 1, Id 0, RefDocumentTypeId 204, RefDocIdNo = the analysis,
     * RefRef* = the stock transaction, ProjectsId = BranchesId, IsApproved false, Approved* = now / user);
     * InventoryStockReserved.SaveList; "Allocated Successfully".
     */
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = user();
        List<Map<String, Object>> rows = list(body.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Please select at least one record to save.");
        int trackingId = asInt(body.get("trackingId"));
        if (trackingId == 0) throw new IllegalArgumentException("Please select a third party analysis.");
        int fy = currentUserContext.currentFinancialYearId();
        Timestamp now = now();
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            double qtyOut = asDouble(r.get("ItemQty"));
            double weightOut = asDouble(r.get("NetWeight"));
            if (qtyOut <= 0.0 && weightOut <= 0.0) throw new IllegalArgumentException("ItemQty or NetWeight must be greater than zero for selected records.");
            if (qtyOut > asDouble(r.get("BalanceQty"))) throw new IllegalArgumentException("ItemQty cannot be greater than BalanceQty. Please check!");
            if (weightOut > asDouble(r.get("BalanceWeight"))) throw new IllegalArgumentException("NetWeight cannot be greater than BalanceWeight. Please check!");
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("IsApproved", false);
            d.put("ApprovedDate", now);
            d.put("DocDate", ts(r.get("DocDate")));
            d.put("EntryDate", now);
            d.put("ModifyDate", now);
            d.put("Amount", asDouble(r.get("ItemAmount")));
            d.put("NetWeight", weightOut);
            d.put("ItemRate", asDouble(r.get("AVgRate")));
            d.put("ItemQty", qtyOut);
            d.put("Id", 0);
            d.put("RefDocIdNo", trackingId);
            d.put("RefDocSubIdNo", 0);
            d.put("RefDocumentTypeId", REF_DOCUMENT_TYPE_ID);
            d.put("RefRefDocIdNo", asInt(r.get("RefDocIdNo")));
            d.put("RefRefDocSubIdNo", asInt(r.get("RefDocSubIdNo")));
            d.put("RefRefDocumentTypeId", asInt(r.get("RefDocumentTypeId")));
            d.put("DocNo", asInt(r.get("DocCodeNo")));
            d.put("SupplierCustomerId", asInt(r.get("SupplierCustomerId")));
            d.put("TranRemarks", raw(r.get("Remarks")));
            d.put("VehicleNo", raw(r.get("VehicleNo")));
            d.put("BiltyNo", raw(r.get("BiltyNo")));
            d.put("GpNo", asInt(r.get("GpNo")));
            d.put("WarehouseId", asInt(r.get("WarehouseId")));
            d.put("ItemId", asInt(r.get("ItemId")));
            d.put("CropYearId", asInt(r.get("CropYearId")));
            d.put("JobLotId", asInt(r.get("JobLotId")));
            d.put("PackingTypeId", asInt(r.get("InvPackingTypeId")));
            d.put("ItemUomId", asInt(r.get("ItemUom")));
            d.put("RateUomId", asInt(r.get("RateUomId")));
            d.put("OrganizationId", u.getOrganizationId());
            d.put("CompanyId", u.getCompanyId());
            d.put("BranchesId", branch(u));
            d.put("FinancialYearId", fy);
            d.put("ProjectsId", branch(u));
            d.put("EntryUserId", u.getId());
            d.put("ModifyUserId", u.getId());
            d.put("ApprovedUserId", u.getId());
            d.put("ActionId", 1);
            items.add(d);
        }
        int id = repo.saveList(items);
        return saved(id, "Allocated Successfully");
    }
}
