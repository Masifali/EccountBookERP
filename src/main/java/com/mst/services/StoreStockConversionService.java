package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.StockAdjustmentRepository;
import com.mst.repositories.StockConversionRepository;
import com.mst.repositories.StockTransferStoreRepository;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.repositories.StoreStockConversionRepository;
import com.mst.repositories.StoreStockTransferRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.StoreStockConversionRepository.DOC;

/**
 * Packing Material (ModuleId 54) - screen 502 "Store Stock Conversion".
 *
 *   Desktop form : Architecture.WinApp.Production.StoreStockConversion (ScreenName "StoreStockConversion")
 *   Loader       : Architecture.WinApp.Production.LoadavailableTransactionsForConversionStore
 *   DocTypeId 808, route /packing-material/store-stock-conversion
 *   BLL 0337 / DAL 0275 InvStockConversion (the 276 engine: {@link StockConversionRepository#save}),
 *   CommonServices.InvStockConversionPackingMaterial665 (665-InvStockConversionPackingMaterial-Summery.rpt).
 *
 * DAL SetData for 808: header, details (LineId 1..n), Sp_InventoryTransactions_GetALLMethod, then
 * Sp_InventoryStockEvalautionDetail_Update, USP_InventoryValidation per input ("1") row with
 * @ItemConditionId and @RackId, then the voucher (inputs credited, outputs debited, each at the job lot's
 * account or the item's PurchaseGLAC; no difference line because ConversionTypeId is 0).
 *
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 *  1. GetAvgRate passes the Item Condition COMBO itself to Conversion.ToInt, so the rate is read with
 *     ItemConditionId 0; the stock (AvailableStockGetByItem) uses the real condition, DocumentTypeId 0.
 *  2. Updating an INPUT row writes a column named "RckId" that does not exist: the row keeps the first
 *     five changes (entry type, item, warehouse) and the form shows
 *     "Column 'RckId' does not belong to table ." (the page reproduces this).
 *  3. Double-clicking a grid row copies the row's Remarks into the HEADER remarks box.
 *  4. Output rows are saved without a Job Lot / UOM check (only warehouse, rack, condition, rate, amount).
 *  5. The balance check compares the two summary boxes as text-to-number, i.e. rounded to the amount format.
 *  6. The rows' amounts and rates are the grid's values; the server checks them as Insert() does and
 *     does not recompute them (the desktop posts the grid).
 *
 * DEVIATIONS (web-only)
 *  A. A document is opened / updated / printed only when it is DocTypeId 808 of the user's organization and company.
 *  B. Grid layouts, attachments and the shortcut popup are not ported.
 */
@Service
public class StoreStockConversionService {

    public static final String SCREEN = "StoreStockConversion";

    private final StoreStockConversionRepository own;
    private final StockConversionRepository sc;
    private final StockTransferStoreRepository store;
    private final StockAdjustmentRepository adj;
    private final StoreStockTransferRepository transfer;
    private final StoreIssuanceRepository shared;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public StoreStockConversionService(StoreStockConversionRepository own, StockConversionRepository sc,
                                       StockTransferStoreRepository store, StockAdjustmentRepository adj,
                                       StoreStockTransferRepository transfer, StoreIssuanceRepository shared,
                                       StoreScreenRights rights, CurrentUserContext ctx) {
        this.own = own; this.sc = sc; this.store = store; this.adj = adj; this.transfer = transfer;
        this.shared = shared; this.rights = rights; this.ctx = ctx;
    }

    private int amountDigits(UserAccount u) {
        int n = toInt(shared.config(u, "Default NoofDecimal Points For Amount"));
        return n >= 1 && n <= 4 ? n : 2;
    }

    // ================================================================================= load

    /** invfrmStockConversionProduction_Load:309. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        boolean fifo = transfer.erpFeature(u, 5);
        boolean cgs = StoreIssuanceService.toBool(shared.config(u, "CGSEntryAllow"));
        out.put("rights", rights.of(SCREEN));
        out.put("fifo", fifo);
        out.put("cgsEntryAllow", cgs);
        out.put("docNo", own.nextCode(u, ctx.currentFinancialYearId()));
        List<Map<String, Object>> parents = new ArrayList<>();                         // ParentCategoryFill: 7, 8
        for (Map<String, Object> r : own.parentCategories()) {
            int id = toInt(ci(r, "Id"));
            if (id != 7 && id != 8) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", id);
            o.put("Name", str(ci(r, "InvParentCateDescription")));
            parents.add(o);
        }
        out.put("parentCategories", parents);
        out.put("inputItems", inputItems(u));
        out.put("outputItems", store.items(u));                                         // ItemFill: type 14 / 17
        out.put("conditions", store.conditions());
        out.put("jobLots", StockAdjustmentRepository.project(adj.jobLots(u, branch(u)), "Id", "JobLotDescription"));
        out.put("defaultJobLot", toInt(shared.config(u, "Job/Lot")));
        out.put("racks", store.racks(u, branch(u)));
        out.put("uoms", store.uoms(u, 0));
        out.put("packingMaterialDefaultWarehouse", toInt(shared.config(u, "PackingMaterialDefaultWarehouse")));
        out.put("defaultWarehouseForStoreFlow", toInt(shared.config(u, "DefaultWarehouseForStoreFlow")));
        out.put("amountDecimals", amountDigits(u));
        return out;
    }

    /** InputItems:608 - GetStoreAndPMItemsWithStockInHand. */
    private List<Map<String, Object>> inputItems(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : sc.storeAndPmItems(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ItemCode", str(ci(r, "ItemCode")));
            o.put("AvailableStock", toDouble(ci(r, "AvailableStock")));
            o.put("ParentCategoryId", toInt(ci(r, "InventoryParentCategoriesId")));
            out.add(o);
        }
        return out;
    }

    public int docNo() { return own.nextCode(ctx.requireAccountingUser(), ctx.currentFinancialYearId()); }

    /** btnRefresh_Click:1999 - the global lists again. */
    public Map<String, Object> refresh() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("inputItems", inputItems(u));
        out.put("outputItems", store.items(u));
        out.put("conditions", store.conditions());
        out.put("jobLots", StockAdjustmentRepository.project(adj.jobLots(u, branch(u)), "Id", "JobLotDescription"));
        out.put("racks", store.racks(u, branch(u)));
        out.put("uoms", store.uoms(u, 0));
        return out;
    }

    // ============================================================================ stock / rate

    /**
     * AvailableStockGetByItem:962 (QtyInHand with condition, rack, warehouse; DocumentTypeId 0) and
     * GetAvgRate:913 (AvgRate with ItemConditionId 0 - D1; DocumentTypeId 808 when editing).
     */
    public Map<String, Object> stock(int recId, int itemId, int conditionId, int warehouseId, int rackId, String docDate) {
        UserAccount u = ctx.requireAccountingUser();
        if (recId > 0 && owned(u, recId) == null) recId = 0;
        Timestamp d = StoreIssuanceService.formDate(docDate, recId);
        Map<String, Object> s = store.avgRateQtyAndStock(u, itemId, d, conditionId, recId, 0, warehouseId, rackId);
        Map<String, Object> r = store.avgRateQtyAndStock(u, itemId, d, 0, recId, recId > 0 ? DOC : 0, 0, 0);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("stock", s == null ? 0d : round(toDouble(ci(s, "QtyInHand")), 2));
        out.put("avgRate", r == null ? 0d : round(toDouble(ci(r, "AvgRate")), 3));
        return out;
    }

    /** AvgRateUpdateOnDocDateChange:2454 - one rate per input row (ItemId, ItemConditionId). */
    public List<Double> rates(int recId, String docDate, List<Map<String, Object>> rows) {
        UserAccount u = ctx.requireAccountingUser();
        if (recId > 0 && owned(u, recId) == null) recId = 0;
        Timestamp d = StoreIssuanceService.formDate(docDate, recId);
        List<Double> out = new ArrayList<>();
        for (Map<String, Object> row : rows == null ? List.<Map<String, Object>>of() : rows) {
            Map<String, Object> r = store.avgRateQtyAndStock(u, toInt(row.get("ItemId")), d, toInt(row.get("ItemConditionId")),
                    recId, recId > 0 ? DOC : 0, 0, 0);
            out.add(r == null ? 0d : round(toDouble(ci(r, "AvgRate")), 3));
        }
        return out;
    }

    // ================================================================================= history

    /** BindGridHeaderistory:397 - Doc / Entry / Modify date radio. */
    public List<Map<String, Object>> history(String dateType, String from, String to, int fromDocNo, int toDocNo) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN, "viewAll");
        String f = blank(from) ? null : from.substring(0, 10), t = blank(to) ? null : to.substring(0, 10);
        String k = dateType == null ? "doc" : dateType;
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : own.history(u, ctx.currentFinancialYearId(), viewAll, u.getId(),
                "doc".equals(k) ? f : null, "doc".equals(k) ? t : null,
                "entry".equals(k) ? f : null, "entry".equals(k) ? t : null,
                "modify".equals(k) ? f : null, "modify".equals(k) ? t : null, fromDocNo, toDocNo)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("DocumentTypeId", toInt(ci(r, "DocumentTypeId")));
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("DocDate", ci(r, "DocDate"));
            o.put("ProductionNo", str(ci(r, "ProductionNo")));
            o.put("Remarks", str(ci(r, "Remarks")));
            o.put("EntryDate", ci(r, "EntryDate"));
            o.put("EntryUser", str(ci(r, "EntryUser")));
            o.put("ModifyDate", ci(r, "ModifyDate"));
            o.put("ModifyUser", str(ci(r, "ModifyUser")));
            o.put("ApprovedDate", ci(r, "PostDate"));
            o.put("ApprovedUser", str(ci(r, "ApprovedUser")));
            o.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    // ==================================================================================== read

    /** ReadById:1325 - header plus the two grids (EntryType "1" input, anything else output). */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = owned(u, id);
        if (h == null) return null;
        List<Map<String, Object>> in = new ArrayList<>(), outRows = new ArrayList<>();
        for (Map<String, Object> d : sc.details(id, DOC)) {
            Map<String, Object> o = new LinkedHashMap<>();
            String et = str(ci(d, "EntryType"));
            o.put("Id", toInt(ci(d, "Id")));
            o.put("RefDocumentTypeId", toInt(ci(d, "RefDocumentTypeId")));
            o.put("RefDocNoId", toInt(ci(d, "RefDocNoId")));
            o.put("RefDocSubId", toInt(ci(d, "RefDocSubId")));
            o.put("EntryType", et);
            o.put("ItemId", toInt(ci(d, "ItemId")));
            o.put("ItemName", str(ci(d, "ItemName")));
            o.put("WareHouseId", toInt(ci(d, "WarehouseId")));
            o.put("WareHouse", str(ci(d, "WareHouseName")));
            o.put("RackId", toInt(ci(d, "RackId")));
            o.put("RackName", str(ci(d, "rackName")));
            o.put("ItemConditionId", toInt(ci(d, "ItemConditionId")));
            o.put("ItemCondition", str(ci(d, "ItemCondition")));
            o.put("ItemUOMId", toInt(ci(d, "ItemUomId")));
            o.put("UOM", str(ci(d, "UomCode")));
            o.put("JobLotId", toInt(ci(d, "JobLotId")));
            o.put("JobLot", str(ci(d, "JobLotDescription")));
            o.put("Quantity", toDouble(ci(d, "Qty")));
            o.put("Rate", toDouble(ci(d, "Rate")));
            o.put("Amount", toDouble(ci(d, "Amount")));
            o.put("Remarks", str(ci(d, "Remarks")));
            o.put("BalQty", 0d);
            o.put("LineId", toInt(ci(d, "SortNo")));
            if ("1".equals(et)) in.add(o); else outRows.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(ci(h, "Id")));
        out.put("DocSrNo", toInt(ci(h, "DocSrNo")));
        out.put("ProductionNo", str(ci(h, "ProductionNo")));
        out.put("DocDate", ci(h, "DocDate"));
        out.put("Remarks", str(ci(h, "Remarks")));
        out.put("parentCategoryId", toInt(ci(h, "parentCategoryId")));
        out.put("input", in);
        out.put("output", outRows);
        return out;
    }

    public boolean owns(int id) { return owned(ctx.requireAccountingUser(), id) != null; }

    // ================================================================================== loader

    /** LoadInvoices_Load:475 - the drop-down rows and the year start (FromDate). */
    public Map<String, Object> loaderLookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : own.loaderDropDowns(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("ActivityType", str(ci(r, "ActivityType")));
            o.put("Id", toInt(ci(r, "Id")));
            o.put("name", str(ci(r, "name")));
            o.put("InventoryParentCategoriesId", str(ci(r, "InventoryParentCategoriesId")));
            rows.add(o);
        }
        out.put("rows", rows);
        out.put("fromDate", adj.financialYearStart(u, ctx.currentFinancialYearId()));
        return out;
    }

    /** PendingInventoryTransactions:265 - the raw procedure rows (the form hands dtData's columns back). */
    public List<Map<String, Object>> loaderSearch(String from, String to, int parentCategoryId, int itemCategoryId,
                                                  int itemTypeId, int warehouseId, int itemId, int itemConditionId) {
        UserAccount u = ctx.requireAccountingUser();
        return own.availableStock(u, blank(from) ? null : from.substring(0, 10), blank(to) ? null : to.substring(0, 10),
                parentCategoryId, itemCategoryId, itemTypeId, warehouseId, itemId, itemConditionId);
    }

    // ==================================================================================== save

    /** btnsave_Click:1060 / btnUpdate_Click:1263 → Insert():1073 → BLL 0337 Save → DAL 0275 SetData. */
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = toInt(body.get("Id"));
        if (recId == 0 && !rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        Map<String, Object> existing = null;
        if (recId != 0) {
            existing = owned(u, recId);
            if (existing == null) throw new IllegalArgumentException("RecId not found");
        }
        int fy = ctx.currentFinancialYearId();
        int docSrNo = existing != null ? toInt(ci(existing, "DocSrNo")) : own.nextCode(u, fy);
        if (docSrNo == 0) throw new IllegalArgumentException("document Number Field Required");            // :1389
        String productionNo = str(body.get("ProductionNo"));
        if (productionNo.trim().isEmpty()) throw new IllegalArgumentException("Production NO. Field Required");  // :1395
        List<Map<String, Object>> input = list(body.get("input")), output = list(body.get("output"));
        if (input.isEmpty()) throw new IllegalArgumentException("At Least 1 Entry Of Input is Required in Input Grid");
        if (output.isEmpty()) throw new IllegalArgumentException("At Least 1 Entry Of Output is Required in Output Grid");
        int dp = amountDigits(u);
        double inAmt = 0d, outAmt = 0d;
        for (Map<String, Object> r : input) inAmt += num(r.get("Amount"));
        for (Map<String, Object> r : output) outAmt += num(r.get("Amount"));
        if (round(inAmt, dp) != round(outAmt, dp))                                                            // :1110
            throw new IllegalArgumentException("Total Input Amount Should Be Equal To Total Output Amount");

        LocalDateTime now = LocalDateTime.now();
        int userId = u.getId() == null ? 0 : u.getId();
        StockConversionRepository.SaveModel m = new StockConversionRepository.SaveModel();
        Map<String, Object> h = m.header;
        h.put("Id", recId);
        h.put("DocTypeId", DOC);
        h.put("DocSrNo", docSrNo);
        h.put("DocDate", StoreIssuanceService.formDate(str(body.get("DocDate")), recId));
        h.put("ProductionNo", productionNo);
        h.put("DocManualRef", "");
        h.put("Remarks", str(body.get("Remarks")));
        h.put("EntryDate", now);
        h.put("EntryUser", userId);
        h.put("ModifyDate", now);
        h.put("ModifyUser", userId);
        h.put("PostDate", null);                                   // DateTime? never set by this form
        h.put("PostUser", 0);
        h.put("PostState", false);
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("BranchedId", branch(u));
        h.put("ProjectsId", 0);
        h.put("parentCategoryId", toInt(body.get("parentCategoryId")));
        h.put("FinancialYearId", fy);
        h.put("GainLossId", 0);
        h.put("EBDepartmentId", 0);
        h.put("ConversionTypeId", 0);
        h.put("DifferenceAccountId", 0);
        /* BLL 0337 Save:15 - Id == 0 → ActionId 1, ModifyUser 0; else ActionId 2, EntryUser 0. */
        if (recId == 0) { h.put("ActionId", 1); h.put("ModifyUser", 0); }
        else { h.put("ActionId", 2); h.put("EntryUser", 0); }

        int n = 0;
        for (Map<String, Object> r : input) {                                                                 // :1132
            n++;
            if (toInt(r.get("ItemId")) == 0) continue;
            if (toInt(r.get("WareHouseId")) == 0) throw new IllegalArgumentException("Warehouse require in Input grid row#" + n);
            if (toInt(r.get("RackId")) == 0) throw new IllegalArgumentException("Rack Name require in Input grid row#" + n);
            if (toInt(r.get("ItemUOMId")) == 0) throw new IllegalArgumentException("Uom require in Input grid row#" + n);
            if (toInt(r.get("ItemConditionId")) == 0) throw new IllegalArgumentException("Item Condition require in Input grid row#" + n);
            if (toInt(r.get("JobLotId")) == 0) throw new IllegalArgumentException("Job Lot require in Input grid row#" + n);
            if (num(r.get("Quantity")) == 0d) throw new IllegalArgumentException("Qty field Required");
            if (num(r.get("Rate")) == 0d) throw new IllegalArgumentException("ItemRate field Required In Row#" + n + "In InPut Grid");
            if (num(r.get("Amount")) == 0d) throw new IllegalArgumentException("Amount field Required In Row#" + n + "In InPut Grid");
            m.details.add(detail(r, str(r.get("EntryType")).isEmpty() ? "1" : str(r.get("EntryType"))));
        }
        n = 0;
        for (Map<String, Object> r : output) {                                                                // :1194
            n++;
            if (toInt(r.get("ItemId")) == 0) continue;
            if (toInt(r.get("WareHouseId")) == 0) throw new IllegalArgumentException("Warehouse require in output grid row#" + n);
            if (toInt(r.get("RackId")) == 0) throw new IllegalArgumentException("Rack Name require in output grid row#" + n);
            if (toInt(r.get("ItemConditionId")) == 0) throw new IllegalArgumentException("Item Condition require in output grid row#" + n);
            if (num(r.get("Rate")) == 0d) throw new IllegalArgumentException("ItemRate field Required In Row#" + n + "In Output Grid");
            if (num(r.get("Amount")) == 0d) throw new IllegalArgumentException("Amount field Required In Row#" + n + "In Output Grid");
            m.details.add(detail(r, str(r.get("EntryType")).isEmpty() ? "2" : str(r.get("EntryType"))));
        }

        int id = sc.save(m);
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("id", id);
        res.put("message", recId == 0 ? "Save Successfully" : "Update Successfully");
        return res;
    }

    /** InvStockConversionDetail as Insert() fills it; every other property keeps its CLR default. */
    private static Map<String, Object> detail(Map<String, Object> r, String entryType) {
        Map<String, Object> d = new LinkedHashMap<>();
        double qty = num(r.get("Quantity"));
        int uom = toInt(r.get("ItemUOMId"));
        d.put("Id", toInt(r.get("Id")));
        d.put("InvStockConversionId", 0);
        d.put("EntryType", entryType);
        d.put("WarehouseId", toInt(r.get("WareHouseId")));
        d.put("ItemId", toInt(r.get("ItemId")));
        d.put("ItemUomId", uom);
        d.put("CropBatch", null);
        d.put("JobLotId", toInt(r.get("JobLotId")));
        d.put("PackingtypeId", 0);
        d.put("Qty", qty);
        d.put("PackUnit", 0);
        d.put("Weight", qty);                                     // d.Weight = d.Qty
        d.put("Rate", num(r.get("Rate")));
        d.put("RateUOMId", uom);                                  // d.RateUOMId = d.ItemUomId
        d.put("Amount", num(r.get("Amount")));
        d.put("ProjectId", 0);
        d.put("VoucherHeadId", 0);
        d.put("Remarks", str(r.get("Remarks")));
        d.put("ExpenseAmount", 0d);
        d.put("PackingMaterialAmount", 0d);
        d.put("Moisture", 0d);
        d.put("MoistureSlabId", 0);
        d.put("RefDocumentTypeId", toInt(r.get("RefDocumentTypeId")));
        d.put("RefDocNoId", toInt(r.get("RefDocNoId")));
        d.put("RefDocSubId", toInt(r.get("RefDocSubId")));
        d.put("LineId", 0);
        d.put("WagesAmount", 0d);
        d.put("ItemPmCost", 0d);
        d.put("ItemOhCost", 0d);
        d.put("ItemConditionId", toInt(r.get("ItemConditionId")));
        d.put("RackId", toInt(r.get("RackId")));
        d.put("SortNo", toInt(r.get("LineId")));                 // d.SortNo = grid LineId
        d.put("labIPmActivityLogId", 0);
        d.put("IsOnHold", false);
        return d;
    }

    // ================================================================================= helpers

    private Map<String, Object> owned(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = sc.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "DocTypeId")) != DOC) return null;
        if (toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId()) return null;
        return h;
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> list(Object o) {
        return o instanceof List ? (List<Map<String, Object>>) o : new ArrayList<>();
    }

    private static double num(Object v) {
        double d = toDouble(v);
        if (!Double.isFinite(d)) throw new IllegalArgumentException("Invalid number");
        return d;
    }

    private static double round(double v, int p) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return 0d;
        return new BigDecimal(v).setScale(p, RoundingMode.HALF_EVEN).doubleValue();
    }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static boolean blank(String s) { return s == null || s.trim().isEmpty(); }
}
