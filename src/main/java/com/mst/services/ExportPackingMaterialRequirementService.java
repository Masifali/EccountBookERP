package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportPackingMaterialRequirementRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.ExportGdBreakUpRepository.ci;

/**
 * The BLL side of 219 PackingMaterialRequirement "Packing Material Requirement" (Architecture.WinApp.Export).
 *
 * The desktop form: Customer (Leave -> its sales contracts), Sales Contract (Leave -> the contract's items grid with a
 * "Load" button per row), Load copies the item / pack size / qty into the read-only Item boxes and re-filters the
 * packing grid; the operator picks a Packing Item (ValueChanged -> its UOM schedule), Pack Uom and Req Qty and adds
 * ("+") rows; the right-hand grid (grdMain) keeps every row of every item and is what Save sends.
 *
 * The desktop has no rights checks on this form; the web page requires View / Save by ScreenDefinition.Id 219.
 *
 * Desktop quirks reproduced:
 *  - Save never sets the model's Id (btnSave_Click only resets the static field), so every Save is an Insert; ReadById
 *    shows the Update button, which has no Click handler on the desktop (the page button does nothing either).
 *  - Save has no validation: an empty grid inserts a header with no detail rows. "Record Saved Successfully" is shown
 *    only when the new id > 0, and the form resets either way.
 *  - The header's ItemId / ItemUomId / ItemQty are never set (0).
 *  - ReadById fills both Qty and ReqQty of grdMain from the detail's ReqQty, and sets the Sales Contract combo by value
 *    without re-binding it (the customer is not set).
 * The add / Load / update-detail rules run on the page (they only touch the form's DataTables).
 */
@Service
public class ExportPackingMaterialRequirementService {

    public static final int SCREEN_ID = 219;

    @Autowired private ExportPackingMaterialRequirementRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    /** PackingMaterialRequirement_Load: ItemBind (type 14) and CustomerBind. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        out.put("permissions", perm);
        put(out, "pmItems", () -> idName(repo.pmItems(u), "Id", "ItemName"));
        put(out, "customers", () -> idName(repo.customers(u), "Id", "CompanyName"));
        return out;
    }

    /** CmbCustomerName_Leave -> SalesContractBind (the active financial year from the session). */
    public List<Map<String, Object>> salesContracts(int supplierCustomerId) {
        UserAccount u = user("View");
        return idName(repo.salesContracts(u, currentUserContext.currentFinancialYearId(), supplierCustomerId), "Id", "LcOrderNo");
    }

    /** CmbSalesContract_Leave -> SalesContractDetailByContractId: the procedure's columns as the grid shows them. */
    public List<Map<String, Object>> contractItems(int contractId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.itemsBySalesContract(u, contractId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ContractDetailId", asInt(ci(r, "ContractDetailId")));
            m.put("ItemId", asInt(ci(r, "ItemId")));
            m.put("ItemName", text(ci(r, "ItemName")));
            m.put("ItemCode", text(ci(r, "ItemCode")));
            m.put("ItemUomId", asInt(ci(r, "ItemUomId")));
            m.put("PackSize", text(ci(r, "PackSize")));
            m.put("InvPackingMaterialTypeId", asInt(ci(r, "InvPackingMaterialTypeId")));
            m.put("PackType", text(ci(r, "PackType")));
            m.put("CropYearId", asInt(ci(r, "CropYearId")));
            m.put("CropYear", text(ci(r, "CropYear")));
            m.put("Qty", asDouble(ci(r, "Qty")));
            m.put("MTon", asDouble(ci(r, "MTon")));
            m.put("RatePrice", asDouble(ci(r, "RatePrice")));
            m.put("IsApproved", ci(r, "IsApproved"));
            out.add(m);
        }
        return out;
    }

    /** cmbitem_ValueChanged -> bindRateUomAndItemPackUom (CommonServices.GetUomScheduleByItemId: Id, UOMCode). */
    public List<Map<String, Object>> uoms(int itemId) {
        return idName(repo.uomByItem(user("View"), itemId), "Id", "UOMCode");
    }

    /** tabControl1_SelectedIndexChanged (History) -> HistoryBind: Id hidden, LcOrderNo, EntryDate, UserName. */
    public List<Map<String, Object>> history() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(user("View"))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("LcOrderNo", text(ci(r, "LcOrderNo")));
            m.put("EntryDate", ExportEformRegistrationService.iso(ci(r, "EntryDate")));
            m.put("UserName", text(ci(r, "UserName")));
            out.add(m);
        }
        return out;
    }

    /**
     * grdhistory_DoubleClick -> ReadById: GetByID(Id)[0] ("Index was out of range..." when missing), the contract's
     * items grid, and grdMain rows (ItemId, ItemName, PackUomId, PackUom, Qty = ReqQty, PmItemId, PackingItem, PmUomId,
     * PmPackUom, ReqQty).
     */
    public Map<String, Object> readById(int id) {
        UserAccount u = user("View");
        List<Map<String, Object>> h = repo.header(id);
        if (h.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        int contractId = asInt(ci(h.get(0), "ExImLcOrderId"));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", id);
        out.put("ExImLcOrderId", contractId);
        out.put("contractItems", contractItems(contractId));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.detail(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("ItemId", asInt(ci(d, "ItemId")));
            m.put("ItemName", text(ci(d, "ItemName")));
            m.put("PackUomId", asInt(ci(d, "ItemUomId")));
            m.put("PackUom", text(ci(d, "PackUom")));
            m.put("Qty", asDouble(ci(d, "ReqQty")));
            m.put("PmItemId", asInt(ci(d, "ItemIdPm")));
            m.put("PackingItem", text(ci(d, "ItemNamePm")));
            m.put("PmUomId", asInt(ci(d, "ItemUomIdPm")));
            m.put("PmPackUom", text(ci(d, "PackUomPm")));
            m.put("ReqQty", asDouble(ci(d, "ReqQty")));
            rows.add(m);
        }
        out.put("rows", rows);
        return out;
    }

    /**
     * Insert(): (the confirm runs on the page) header + one detail per grdMain row with SortNo = row index + 1,
     * ExImLcOrderMrpHeader.Save (BLL stamps EntryDate / ModifyDate = now).
     */
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = user("Save");
        int contractId = asInt(body.get("salesContractId"));
        List<Map<String, Object>> rows = list(body.get("rows"));
        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> hdr = new LinkedHashMap<>();
        hdr.put("ItemQty", BigDecimal.ZERO);
        hdr.put("CompanyId", u.getCompanyId());
        hdr.put("ExImLcOrderId", contractId);
        hdr.put("Id", 0);
        hdr.put("ItemId", 0);
        hdr.put("ItemUomId", 0);
        hdr.put("OrganizationId", u.getOrganizationId());
        hdr.put("EntryUser", u.getId());
        hdr.put("ModifyUser", u.getId());
        hdr.put("EntryDate", now);
        hdr.put("ModifyDate", now);
        List<Map<String, Object>> details = new ArrayList<>();
        int i = 0;
        for (Map<String, Object> r : rows) {
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("ReqQty", BigDecimal.valueOf(asDouble(r.get("ReqQty"))));
            d.put("ExImLcOrderId", contractId);
            d.put("ExImLcOrderMrpHeaderId", 0);
            d.put("Id", 0);
            d.put("ItemId", asInt(r.get("ItemId")));
            d.put("ItemIdPm", asInt(r.get("PmItemId")));
            d.put("ItemUomId", asInt(r.get("PackUomId")));
            d.put("ItemUomIdPm", asInt(r.get("PmUomId")));
            d.put("SortNo", ++i);
            details.add(d);
        }
        int result = repo.save(hdr, details);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", result);
        out.put("message", result > 0 ? "Record Saved Successfully" : "");
        return out;
    }

    // ------------------------------------------------------------------ helpers

    private interface Loader { Object load() throws Exception; }

    private static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) {
            Throwable t = e; while (t.getCause() != null && t.getCause() != t) t = t.getCause();
            out.put(key, new ArrayList<>()); out.put(key + "Error", t.getMessage());
        }
    }

    private static List<Map<String, Object>> idName(List<Map<String, Object>> rows, String id, String name) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, id)));
            m.put("name", text(ci(r, name)));
            out.add(m);
        }
        return out;
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    private static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    private static int asInt(Object v) { return ExportEformRegistrationService.asInt(v); }

    private static double asDouble(Object v) { return ExportEformRegistrationService.asDouble(v); }
}
