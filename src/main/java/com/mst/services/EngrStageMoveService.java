package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.EngrStageMoveRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.PlatformTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.Prod273Support.ci;
import static com.mst.services.Prod273Support.day;
import static com.mst.services.Prod273Support.list;
import static com.mst.services.Prod273Support.pickerValue;
import static com.mst.services.Prod273Support.stamp;
import static com.mst.services.Prod273Support.str;
import static com.mst.services.Prod273Support.toDouble;
import static com.mst.services.Prod273Support.toInt;
import static com.mst.services.Prod273Support.truthy;

/**
 * Screens 807 and 846 "Production Move (Stage Shift)" - Architecture.WinApp.Mfg.frmStockTransferManual_Engr (.cs 6,831 lines; logic lines 1-3,515, the rest is
 * the designer). Rights key base.Name "frmStockTransferManual_Engr", DocumentTypeId 1663, schema [Mfg].
 *
 * <pre>
 *   Route     /production/engr-stage-move      API /production/engr-stage-move/api
 *   One page serves both menu ids (807 and 846 are the same desktop class).
 *   Not the Store screen 806 (/store/stock-transfer-manual, class frmStockTransferManual, DocumentTypeId 806): that form posts through
 *   dbo.Sp_InvStockTransferHeader_* (DAL 0412) with FIFO / vouchers / packing / crop year; this one posts through
 *   [Mfg].[USP_InvStockTransferHeader_InsertAndUpdate] / [Mfg].[USP_InvStockTransferDetail_Insert] (DAL 0316) with production stages, work orders,
 *   work stations, casting type and batch, and has no accounting. It is not a superset of 806 and 806 is not a subset of it.
 * </pre>
 *
 * SAVE (BLL 0363 Save + DAL 0316 SetData, one transaction): header insert / update -> every detail row (removed rows first, ActionTypeId 3; LineId 1..n over
 * the whole list) -> Sp_InventoryTransactions_GetALLMethod -> Sp_InventoryStockEvalautionDetail_Update -> USP_InventoryValidation for every detail
 * (DocumentTypeId 1663 / 1665 only).
 *
 * KNOWN DIFFERENCES / GAPS
 * <ol>
 *  <li>Attachments (Attachment dialog, DMS copy, Proc_DMSAttachments_Insert) are not ported; AttachmentsValues is sent as "" as the desktop does when none are added.</li>
 *  <li>SaveAs (Ctrl+F12), the shortcut-keys dialog and the desktop grid layout files (ctrlGrdBar) are not ported.</li>
 *  <li>The loader dialog (frmLoadAvailableStockEngr) fills its Item combo from the wrong table rows when a category is chosen (it reads dtAvailableStock.Rows[i] with an index
 *      taken from the filtered table); the page uses the filtered rows.</li>
 *  <li>The desktop adds a new detail row from the entry panel with the HEADER warehouse-to / production-stage combos (btnplus_Click:2060) but updates it with the DETAIL combos
 *      (btnUpdateDetail_Click:2088); both are kept as is.</li>
 *  <li>History date type 4 sets From = today and To = 1 Jan (cmbperemeter_ValueChanged:3045) - copied as is, so it lists nothing.</li>
 *  <li>Numbers are double on the page and on the server (the desktop uses decimal).</li>
 *  <li>Nothing here has been run against the database or a browser.</li>
 * </ol>
 */
@Service
public class EngrStageMoveService {

    public static final String SCREEN = "frmStockTransferManual_Engr";
    public static final int DOC_TYPE = 1663;

    private final Prod273Support db;
    private final EngrStageMoveRepository repo;
    private final PlatformTransactionManager tm;

    public EngrStageMoveService(Prod273Support db, EngrStageMoveRepository repo, PlatformTransactionManager tm) {
        this.db = db;
        this.repo = repo;
        this.tm = tm;
    }

    // ====================================================================================== helpers

    private static Map<String, Object> pair(Object id, Object name) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        o.put("Name", name);
        return o;
    }

    private static Object cell(Object v) {
        if (v instanceof Timestamp || v instanceof java.sql.Date || v instanceof java.util.Date) return stamp(v);
        if (v instanceof BigDecimal) return ((BigDecimal) v).doubleValue();
        return v;
    }

    private static List<Map<String, Object>> json(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) o.put(e.getKey(), cell(e.getValue()));
            out.add(o);
        }
        return out;
    }

    private static void need(boolean ok, String message) { if (!ok) throw new IllegalArgumentException(message); }

    private void right(String key, String message) {
        if (!db.has(SCREEN, key)) throw new IllegalStateException(message);
    }

    private int userId(UserAccount u) { return u.getId() == null ? 0 : u.getId(); }

    // ====================================================================================== load (InitializeComponentMethod:465)

    public Map<String, Object> lookups() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", db.rights(SCREEN));
        out.put("branchId", db.branchId(u));
        out.put("financialYearStart", db.financialYearStart(u, db.financialYearId()));
        out.put("docNo", docNo());
        out.put("dateTypes", Prod273Support.literal(Prod273Support.DATE_TYPES, "Parameters"));
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(db.config(u, "DefaultDaysToLessFromHistoryFromDate")));   // :471
        out.putAll(refresh());
        return out;
    }

    /** btnRefresh_Click:1660 - BindTransferType, WareHouseFill, ProductionStageComboFill, WorkStationFill, WorkOrderFill. */
    public Map<String, Object> refresh() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> types = new ArrayList<>();
        for (Map<String, Object> r : repo.allLookups()) {                                              // BindTransferType:573 - LookUpTypeId == 4 && Id != 9
            if (toInt(ci(r, "LookUpTypeId")) == 4 && toInt(ci(r, "Id")) != 9) types.add(pair(toInt(ci(r, "Id")), str(ci(r, "LookupName"))));
        }
        out.put("transferTypes", types);
        out.put("warehouses", Prod273Support.project(repo.warehouses(u), "Id", "WareHouseName"));
        List<Map<String, Object>> stages = new ArrayList<>();
        for (Map<String, Object> r : repo.lookupsByType(3)) stages.add(pair(toInt(ci(r, "Id")), str(ci(r, "LookupName"))));   // ProductionStageComboFill:1142
        out.put("productionStages", stages);
        out.put("workStations", Prod273Support.project(repo.workStations(u, db.branchId(u)), "Id", "WorkStationName"));
        out.put("workOrders", Prod273Support.project(repo.workOrders(u), "Id", "WorkOrderNo"));
        return out;
    }

    /** DocumentNoDBCall:531. */
    public String docNo() {
        UserAccount u = db.user();
        List<Map<String, Object>> r = repo.generateCode(u, db.branchId(u), DOC_TYPE, db.financialYearId());
        return r.isEmpty() ? "0" : String.valueOf(toInt(ci(r.get(0), "DocNo")));
    }

    // ====================================================================================== transfer type / stock

    /** GetTransferTypeData:1356 - panel visibility (types 6, 8, 10) and the allocated warehouses / production stages. */
    public Map<String, Object> transferType(int transferTypeId) {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("showPanel", transferTypeId == 6 || transferTypeId == 8 || transferTypeId == 10);
        List<Map<String, Object>> dt = repo.typeAllocation(u, transferTypeId);
        out.put("found", !dt.isEmpty());
        if (!dt.isEmpty()) {
            out.put("fromWarehouseId", toInt(ci(dt.get(0), "FromWarehouseId")));
            out.put("toWarehouseId", toInt(ci(dt.get(0), "ToWarehouseId")));
            out.put("fromStageId", toInt(ci(dt.get(0), "FromProductionStageId")));
            out.put("toStageId", toInt(ci(dt.get(0), "ToProductionStageId")));
        }
        return out;
    }

    /** CmbTransferType_Leave:1422 / :1442 - GetAvailableQtyByWarehouseAndProductionStage, plus SampleModaldBCall (ItemModel_GetByItemIds) for the BindCombos step. */
    public Map<String, Object> availableStock(int warehouseId, int stageId, String docDate) {
        UserAccount u = db.user();
        List<Map<String, Object>> rows = json(repo.availableStock(u, warehouseId, stageId, pickerValue(docDate)));
        Set<Integer> ids = new LinkedHashSet<>();
        for (Map<String, Object> r : rows) ids.add(toInt(ci(r, "ItemId")));
        StringBuilder sb = new StringBuilder();
        for (Integer id : ids) sb.append(id).append(',');                                                // Aggregate("", current + id + ",")
        List<Map<String, Object>> models = new ArrayList<>();
        if (!rows.isEmpty()) {
            for (Map<String, Object> r : repo.itemModels(u, sb.toString())) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", toInt(ci(r, "itemModalId")));
                o.put("SampleNo", str(ci(r, "ItemSampleNo")));
                o.put("ItemModal", str(ci(r, "ItemModal")));
                o.put("ItemCategoryId", toInt(ci(r, "ItemCategoryId")));
                o.put("ItemId", toInt(ci(r, "ItemId")));
                models.add(o);
            }
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("stock", rows);
        out.put("models", models);
        return out;
    }

    /** ItemUomBind:959 - ItemAttributeVarient.GetAllForCombo. */
    public List<Map<String, Object>> attributes(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.attributes(db.user(), itemId)) out.add(pair(toInt(ci(r, "ItemAttributeVarientId")), str(ci(r, "ItemAttribute"))));
        return out;
    }

    /** GetCgsAvgRate:2514 - CommonServices.AvgRateOnlyForCGS(ItemId, DocDate, 1663, RecId, 0, 0, null, WarehouseId). */
    public Map<String, Object> avgRate(int itemId, int warehouseId, String docDate, int recId) {
        double rate = 0d;
        if (itemId > 0) {
            UserAccount u = db.user();
            rate = db.avgRateOnlyForCgs(u, itemId, pickerValue(docDate), DOC_TYPE, recId, 0, 0, null, warehouseId, 0);
        }
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("avgRate", rate);
        return o;
    }

    // ====================================================================================== history / read / delete

    /** gridhistoryfill:3105 - BLL 0363 FormHistory. */
    public List<Map<String, Object>> history(String fromDate, String toDate, int fromDocNo, int toDocNo) {
        UserAccount u = db.user();
        boolean viewAll = db.has(SCREEN, "viewAll");
        return json(repo.formHistory(u, db.branchId(u), DOC_TYPE, db.financialYearId(), pickerValue(fromDate), pickerValue(toDate), fromDocNo, toDocNo, viewAll, userId(u)));
    }

    /** ReadById:2400 - header + the detail rows shaped as the page's grid rows (the desktop table with 26 columns). */
    public Map<String, Object> read(int id) {
        List<Map<String, Object>> hs = repo.readHeader(id);
        if (hs.isEmpty()) throw new IllegalArgumentException("Record not found");
        Map<String, Object> h = hs.get(0);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", toInt(ci(h, "Id")));
        out.put("docDate", day(ci(h, "DocDate")));
        out.put("docNo", toInt(ci(h, "DocNo")));
        out.put("transferTypeId", toInt(ci(h, "TransferTypeId")));
        out.put("remarksHeader", str(ci(h, "RemarksHeader")));
        out.put("isApproved", truthy(ci(h, "IsApproved")));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.readDetails(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            double qty = toDouble(ci(d, "ItemQty"));
            o.put("Id", toInt(ci(d, "Id")));
            o.put("ItemId", toInt(ci(d, "ItemId")));
            o.put("ItemName", str(ci(d, "ItemName")));
            o.put("UomId", toInt(ci(d, "ItemUomId")));
            o.put("Uom", str(ci(d, "ItemUom")));
            o.put("VariantId", toInt(ci(d, "ItemVariantId")));
            o.put("VariantDescription", str(ci(d, "VariantDescription")));
            o.put("JobLotId", toInt(ci(d, "JobLotId")));
            o.put("BatchNo", str(ci(d, "BatchDescription")));
            o.put("CastingTypeId", toInt(ci(d, "CastingTypeId")));
            o.put("CastingType", str(ci(d, "CastingType")));
            o.put("WareHouseFromId", toInt(ci(d, "WarehouseFromId")));
            o.put("WareHouseToId", toInt(ci(d, "WarehouseToId")));
            o.put("ProductionStageFromId", toInt(ci(d, "ProductionStageFromId")));
            o.put("ProductionStageToId", toInt(ci(d, "ProductionStageToId")));
            o.put("WorkStationFromId", toInt(ci(d, "WorkStationFromId")));
            o.put("WorkStationToId", toInt(ci(d, "WorkStationToId")));
            o.put("AvailableQty", qty);                                                                   // :2423 - both columns get ItemQty
            o.put("TransferQty", qty);
            o.put("AvgRate", toDouble(ci(d, "ItemRate")));
            o.put("RateUomId", toInt(ci(d, "RateUomId")));
            o.put("RateUom", str(ci(d, "RateUom")));
            o.put("RateEquivalent", toDouble(ci(d, "RateUomEquivalent")));
            o.put("Amount", toDouble(ci(d, "ItemAmount")));
            o.put("WorkOrderId", toInt(ci(d, "WorkOrderId")));
            o.put("Remarks", str(ci(d, "RemarksDetail")));
            /* display names the desktop gets from its value lists / the history sub-grid */
            o.put("WarehouseFrom", str(ci(d, "WarehouseFrom")));
            o.put("WarehouseTo", str(ci(d, "WarehouseTo")));
            o.put("ProductionStageFrom", str(ci(d, "ProductionStageFrom")));
            o.put("ProductionStageTo", str(ci(d, "ProductionStageTo")));
            o.put("WorkStationFrom", str(ci(d, "WorkStationFrom")));
            o.put("WorkStationTo", str(ci(d, "WorkStationTo")));
            o.put("WorkOrderNo", str(ci(d, "WorkOrderNo")));
            rows.add(o);
        }
        out.put("rows", rows);
        return out;
    }

    /** btnDelete_Click:2445 - BLL 0363 DeleteByID(UserAccount.ID, RecId). */
    public Map<String, Object> delete(int id) {
        right("update", "You do not have the Update right on Production Move (Stage Shift).");     // btnDelete.Enabled = DoHaveUpdateRights (:488)
        need(id > 0, "RecId not Found");
        UserAccount u = db.user();
        new TransactionTemplate(tm).execute(st -> { repo.deleteById(userId(u), id); return null; });
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", "Delete Record Successfully");
        return m;
    }

    // ====================================================================================== save (Insert:2199)

    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = db.user();
        int recId = toInt(body.get("id"));
        boolean update = recId > 0;
        right(update ? "update" : "save", update ? "You do not have the Update right on Production Move (Stage Shift)." : "You do not have the Save right on Production Move (Stage Shift).");

        /* FormValidation:1578 */
        String docNoText = str(body.get("docNo")).trim();
        need(!(docNoText.isEmpty() || docNoText.equals("0")), "DocNo Field is Required");
        int transferTypeId = toInt(body.get("transferTypeId"));
        need(transferTypeId != 0, "TransferType Field is Required");

        List<Map<String, Object>> rows = list(body.get("rows"));
        need(!rows.isEmpty(), "Grid Record Not Found");                                                      // :2211

        if (update) {                                                                                         // btnupdate_Click:2385
            List<Map<String, Object>> hs = repo.readHeader(recId);
            if (!hs.isEmpty() && truthy(ci(hs.get(0), "IsApproved"))) throw new IllegalArgumentException("Approved Record Not Update");
        }

        int org = u.getOrganizationId(), company = u.getCompanyId(), branch = db.branchId(u), fy = db.financialYearId(), uid = userId(u);
        Timestamp now = Prod273Support.now();
        Timestamp docDate = pickerValue(body.get("docDate"));

        List<Map<String, Object>> details = new ArrayList<>();
        if (update) {                                                                                         // :2247 - removed rows first
            for (Map<String, Object> r : list(body.get("removed"))) {
                Map<String, Object> d = new LinkedHashMap<>();
                d.put("Id", toInt(r.get("Id")));
                d.put("ItemId", toInt(r.get("ItemId")));
                d.put("ItemUomId", toInt(r.get("UomId")));
                d.put("ItemVariantId", toInt(r.get("VariantId")));
                d.put("JobLotId", toInt(r.get("JobLotId")));
                d.put("BatchDescription", str(r.get("BatchNo")));
                d.put("WorkOrderId", toInt(r.get("WorkOrderId")));
                d.put("WarehouseFromId", toInt(r.get("WareHouseFromId")));
                d.put("WarehouseToId", toInt(r.get("WareHouseToId")));
                d.put("ProductionStageFromId", toInt(r.get("ProductionStageFromId")));
                d.put("ProductionStageToId", toInt(r.get("ProductionStageToId")));
                d.put("WorkStationFromId", toInt(r.get("WorkStationFromId")));
                d.put("WorkStationToId", toInt(r.get("WorkStationToId")));
                d.put("ItemQty", toDouble(r.get("TransferQty")));
                d.put("NetWeight", toDouble(r.get("TransferQty")));
                d.put("ItemRate", toDouble(r.get("AvgRate")));
                d.put("RateUomId", toInt(r.get("RateUomId")));
                d.put("ItemAmount", toDouble(r.get("Amount")));
                d.put("CastingTypeId", toInt(r.get("CastingTypeId")));
                d.put("RemarksDetail", str(r.get("Remarks")));
                d.put("ActionTypeId", 3);
                details.add(d);
            }
        }
        int rowNo = 0;
        for (Map<String, Object> r : rows) {
            rowNo++;
            Map<String, Object> d = new LinkedHashMap<>();
            int rid = recId != 0 ? toInt(r.get("Id")) : 0;
            d.put("Id", rid);
            d.put("ActionTypeId", rid <= 0 ? 1 : 2);
            d.put("ItemId", toInt(r.get("ItemId")));
            d.put("ItemUomId", toInt(r.get("UomId")));
            d.put("ItemVariantId", toInt(r.get("VariantId")));
            d.put("BatchDescription", str(r.get("BatchNo")));
            d.put("WorkOrderId", toInt(r.get("WorkOrderId")));
            double transferQty = toDouble(r.get("TransferQty"));
            if (transferTypeId != 6 && !update && transferQty > toDouble(r.get("AvailableQty"))) {            // :2265 (btnsave visible = a new record)
                throw new IllegalArgumentException("Item Quantity cannot greater than Available Qty in Detail in Row No: " + rowNo);
            }
            d.put("JobLotId", toInt(r.get("JobLotId")));
            d.put("CastingTypeId", toInt(r.get("CastingTypeId")));
            d.put("WarehouseFromId", toInt(r.get("WareHouseFromId")));
            d.put("WarehouseToId", toInt(r.get("WareHouseToId")));
            d.put("ProductionStageFromId", toInt(r.get("ProductionStageFromId")));
            d.put("ProductionStageToId", toInt(r.get("ProductionStageToId")));
            d.put("WorkStationFromId", toInt(r.get("WorkStationFromId")));
            d.put("WorkStationToId", toInt(r.get("WorkStationToId")));
            d.put("ItemQty", transferQty);
            d.put("NetWeight", transferQty);
            d.put("ItemRate", toDouble(r.get("AvgRate")));
            d.put("RateUomId", toInt(r.get("RateUomId")));
            d.put("ItemAmount", toDouble(r.get("Amount")));
            d.put("RemarksDetail", str(r.get("Remarks")));
            String at = " in Detail in Row No: " + rowNo;
            need(toInt(d.get("ItemId")) > 0, "Item field is required" + at);
            need(toInt(d.get("ItemUomId")) > 0, "Uom field is required" + at);
            need(!str(d.get("BatchDescription")).isEmpty(), "Batch No field is required" + at);
            need(toInt(d.get("WarehouseFromId")) > 0, "Warehouse From field is required" + at);
            need(toInt(d.get("WarehouseToId")) > 0, "Warehouse To field is required" + at);
            need(toInt(d.get("WarehouseFromId")) != toInt(d.get("WarehouseToId")), "Warehouse Stage From And To Cannot be same" + at);
            need(toInt(d.get("ProductionStageFromId")) > 0, "Production Stage From field is required" + at);
            need(toInt(d.get("ProductionStageToId")) > 0, "Production Stage To field is required" + at);
            need(!(transferTypeId != 8 && toInt(d.get("ProductionStageFromId")) == toInt(d.get("ProductionStageToId"))), "Production Stage From And To Cannot be same" + at);
            need(transferQty > 0, "Item Quantity field is required" + at);
            need(toDouble(d.get("ItemRate")) > 0, "Item Rate field is required" + at);
            need(toInt(d.get("RateUomId")) > 0, "RateUom field is required" + at);
            need(toDouble(d.get("ItemAmount")) > 0, "Amount field is required" + at);
            details.add(d);
        }
        need(!details.isEmpty(), "At least enter value/quantity in one of the rows of detail ");

        /* BLL 0363 Save:17 */
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", recId);
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("DocNo", toInt(docNoText));
        h.put("DocDate", docDate);
        h.put("TransferTypeId", transferTypeId);
        h.put("AttachmentsValues", "");
        h.put("CustomAttachmentsValues", "");
        h.put("RemarksHeader", str(body.get("remarksHeader")));
        h.put("EntryDate", now);
        h.put("EntryUserId", update ? 0 : uid);
        h.put("ModifyDate", now);
        h.put("ModifyUserId", update ? uid : 0);
        h.put("IsApproved", false);
        h.put("ApprovedUserId", uid);
        h.put("ApprovedDate", now);
        h.put("OrganizationId", org);
        h.put("CompanyId", company);
        h.put("BranchesId", branch);
        h.put("ProjectsId", branch);
        h.put("FinancialYearId", fy);
        h.put("ActionId", update ? 2 : 1);

        final List<Map<String, Object>> toSave = details;
        int newId = new TransactionTemplate(tm).execute(st -> setData(u, h, recId, docDate, toSave));

        Map<String, Object> out = Prod273Support.ok((update ? "Data Update Successfully....  " : "Data Save Successfully....  ") + toInt(docNoText), newId, toInt(docNoText));
        return out;
    }

    /** DAL 0316 SetData:18 - in the DAL's order. */
    private int setData(UserAccount u, Map<String, Object> header, int recId, Timestamp docDate, List<Map<String, Object>> details) {
        int num = repo.setHeader(header);                                                                   // :55
        int headerId = num > 0 ? num : recId;
        int lineId = 1;
        for (Map<String, Object> d : details) {                                                             // :60
            d.put("LineId", lineId);
            d.put("InvStockTransferHeaderId", headerId);
            d.put("IsApproved", false);
            d.put("RejectionReasonTypeId", 0);
            d.put("TeamWorkerNameId", 0);
            d.put("Id", repo.setDetail(d));
            lineId++;
        }
        repo.inventoryTransactions(u, DOC_TYPE, headerId);                                                  // :70
        repo.stockEvaluationUpdate(u, DOC_TYPE, headerId);                                                  // :77
        for (Map<String, Object> d : details) {                                                             // :82 - DocumentTypeId 1663 / 1665
            repo.inventoryValidation(u, DOC_TYPE, docDate, toInt(d.get("ItemId")), toInt(d.get("WarehouseFromId")), toInt(d.get("ItemVariantId")),
                    toInt(d.get("ProductionStageFromId")), toInt(d.get("WorkOrderId")), toInt(d.get("WorkStationFromId")), str(d.get("BatchDescription")),
                    toDouble(d.get("ItemQty")));
        }
        return headerId;
    }
}
