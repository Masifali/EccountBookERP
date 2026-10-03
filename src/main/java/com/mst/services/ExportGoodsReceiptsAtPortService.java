package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportGoodsReceiptsAtPortRepository;
import com.mst.repositories.SaleInvoiceRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportShipmentFormsSupport.*;

/**
 * Architecture.WinApp.Export.ExImGoodsReceiptsAtPort "Goods Receipts At Port" (ClientSize 944 x 676, no ScreenDefinition
 * row - rights by the form name "ExImGoodsReceiptsAtPort", see ExportShipmentFormsSupport) and its loader dialog
 * GetForwardingDataForGoodsReceiptsAsPort.
 *
 * Desktop behaviour reproduced, not corrected:
 *  Q1  Save in UpdateMode sends header Id 1, so the BLL picks USP_ExImGoodsReceiptsAtPort_Update for EVERY row, also rows
 *      added from the loader while editing (their Id 0 updates nothing). SortNo is only set when NOT UpdateMode, so every
 *      updated row is written back with SortNo = 0 (the int default) - the document loses its grouping in the history.
 *  Q2  New rows all get SortNo = Max(SortNo of the history table) + 1. The desktop computes it from the history grid it
 *      loaded (branch-scoped, Sp_ExImGoodsReceiptsAtPort_GetAllMetohd); the service re-reads the same procedure at save
 *      time, which is the same set unless another user saved in between.
 *  Q3  No validation: an empty grid "saves" (no procedure call) and still says "Record Saved Successfully".
 *  Q4  PackEquivalent = NetWeight / NoOfBags (loader) or WeightReceived / QtyReceived (history) with no zero check - the
 *      page keeps the resulting Infinity / NaN exactly as a .NET double would.
 *  Q5  The loader's From/To dates are shown but never sent; FromDate starts at ActiveYr.Start_Period.
 */
@Service
public class ExportGoodsReceiptsAtPortService {

    public static final String SCREEN_NAME = "ExImGoodsReceiptsAtPort";

    @Autowired private ExportGoodsReceiptsAtPortRepository repo;
    @Autowired private SaleInvoiceRepository rightsRepo;
    @Autowired private CurrentUserContext ctx;

    private int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }

    /** ExImGoodsReceiptsAtPort_Load: rights (btnsave / btnupdate Enabled) and BindHistory. */
    public Map<String, Object> setup() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Boolean> r = rights(rightsRepo, ctx, u, SCREEN_NAME);
        out.put("rights", r);
        put(out, "history", () -> historyRows(u));
        return out;
    }

    public List<Map<String, Object>> history() { return historyRows(ctx.requireAccountingUser()); }

    /** BindHistory - dthistory columns in the desktop's order (DocDate / GpDate / EntryDate / ModifyDate as dates). */
    private List<Map<String, Object>> historyRows(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(u, branch(u))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("SortNo", asInt(ci(r, "SortNo")));
            m.put("ForwardingId", asInt(ci(r, "ExImForwardingId")));
            m.put("ForwardingDetailId", asInt(ci(r, "ExImForwardingPackingDetailId")));
            m.put("DocNo", asInt(ci(r, "DocNo")));
            m.put("DocDate", iso(ci(r, "DocDate")));
            m.put("CustomerName", text(ci(r, "CustomerName")));
            m.put("GpNo", asInt(ci(r, "GpNo")));
            m.put("GpDate", iso(ci(r, "GpDate")));
            m.put("VehicleNo", text(ci(r, "VehicleNo")));
            m.put("BiltyNo", text(ci(r, "BiltyNo")));
            m.put("WarehouseId", asInt(ci(r, "WarehouseId")));
            m.put("Warehouse", text(ci(r, "WareHouseName")));
            m.put("QtyReceived", asDouble(ci(r, "QtyReceived")));
            m.put("WeightReceived", asDouble(ci(r, "WeightReceived")));
            m.put("Remarks", raw(ci(r, "RemarksDetail")));
            m.put("EntryUser", text(ci(r, "EntryUser")));
            m.put("EntryDate", iso(ci(r, "EnteryDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUser")));
            m.put("ModifyDate", iso(ci(r, "ModifyDate")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= loader (GetForwardingDataForGoodsReceiptsAsPort)

    /** LoadInvoices_Load: PartyNameFill (group "7"), CurrencyFill, ItemFill. */
    public Map<String, Object> loaderCombos() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "parties", () -> project(repo.partiesByGroup(u, "7"), "Id", "CompanyName"));
        put(out, "currencies", () -> project(repo.currencies(u), "Id", "CurrencyName"));
        put(out, "items", () -> project(repo.itemsCombo(u), "Id", "ItemName"));
        return out;
    }

    /** ExportPreInvoicesLoad - every row of the procedure (the page builds the master grid distinct by Id and the detail grid). */
    public List<Map<String, Object>> loaderRows(int partyId, int itemId, int currencyId) {
        UserAccount u = ctx.requireAccountingUser();
        return plain(repo.forwardingData(u, ctx.currentFinancialYearId(), branch(u), partyId, itemId, currencyId));
    }

    private static List<Map<String, Object>> project(List<Map<String, Object>> rows, String id, String name) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, id)));
            m.put("Name", text(ci(r, name)));
            out.add(m);
        }
        return out;
    }

    // ================================================================= save

    /**
     * btnsave_Click / btnupdate_Click. Save right for a new document, Update right in UpdateMode (the buttons'
     * Enabled states). Model properties in the model's order (GenericProvider.SetProc sends all 15).
     */
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = ctx.requireAccountingUser();
        boolean update = asBool(body.get("update"));
        Map<String, Boolean> r = rights(rightsRepo, ctx, u, SCREEN_NAME);
        if (update) require(r, "Update", "You do not have the Update right for this screen.");
        else require(r, "Save", "You do not have the Save right for this screen.");

        int sortNo = 0;
        if (!update) {                                                   // Q2
            int max = 0;
            for (Map<String, Object> h : repo.history(u, branch(u))) max = Math.max(max, asInt(ci(h, "SortNo")));
            sortNo = max + 1;
        }
        Timestamp now = now();
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> row : list(body.get("rows"))) {
            Map<String, Object> vd = new LinkedHashMap<>();
            vd.put("EnteryDate", now);
            vd.put("ModifyDate", now);
            vd.put("QtyReceived", dec(row.get("QtyReceived")));
            vd.put("WeightReceived", dec(row.get("WeightReceived")));
            vd.put("BranchId", branch(u));
            vd.put("CompanyId", u.getCompanyId());
            vd.put("EnteryUserId", u.getId());
            vd.put("ModifyUserId", u.getId());
            vd.put("ExImForwardingId", asInt(row.get("ForwardingId")));
            vd.put("ExImForwardingPackingDetailId", asInt(row.get("ForwardingDetailId")));
            vd.put("Id", asInt(row.get("Id")));
            vd.put("OrganizationId", u.getOrganizationId());
            vd.put("SortNo", update ? 0 : sortNo);                       // Q1
            vd.put("WarehouseId", asInt(row.get("WarehouseId")));
            vd.put("RemarksDetail", raw(row.get("RemarksDetail")));
            items.add(vd);
        }
        int id = repo.save(update ? "USP_ExImGoodsReceiptsAtPort_Update" : "USP_ExImGoodsReceiptsAtPort_Insert", items);
        return ok(update ? "Record Updated Successfully" : "Record Saved Successfully", id);
    }
}
