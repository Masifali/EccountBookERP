package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ProductionInputRepository;
import com.mst.repositories.ProductionOutputRepository;
import com.mst.repositories.W6BFeedManualRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.PlatformTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.HashMap;
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
 * Screen 929 "Production Manual Feed" - Architecture.WinApp.FeedMill.Production.frmProductionManualFeed (.cs 9,931 lines; logic lines 1-6,740, the rest is the designer).
 * Rights key base.Name "frmProductionManualFeed", DocumentTypeId 1122, schema [fed]. Page title "Production Manual".
 *
 * <pre>
 *   Route     /production/manual-feed      API /production/manual-feed/api
 *   Detail panel (Warehouse / Item / BOM No / UOM / Qty) adds rows to the Output grid; Input, Consumption and Over Head grids; Calculation summary;
 *   Load Input From BOM; "Update Input,Consumption.OH &amp; Output Rate"; history with Input / Output / Consumption / Overheads sub-grids.
 * </pre>
 *
 * SAVE (BLL 0431 Save + DAL 0488 SetData, one transaction): MakeVoucher (BLL, before the DAL) -> [fed].[USP_ProductionHeader_Insert|Update] -> every output row (LineId 1..n over
 * the whole list, removed rows last) -> every input row (LineId 1000+) -> over head rows -> consumption rows (LineId 2000+) -> Sp_InventoryStockEvalautionDetail_Update ->
 * Sp_InventoryTransactions_GetALLMethod -> USP_InventoryValidation per input row (and per consumption row) -> Sp_VoucherHead_Insert|Update -> Sp_VoucherDetail_Insert per line ->
 * USP_VoucherBalanceCheck; an empty voucher detail list throws "Voucher Detail list not found..." and rolls everything back, as the DAL does.
 *
 * KNOWN DIFFERENCES / GAPS
 * <ol>
 *  <li>Attachments (Attachment dialog, DMS copy / FTP, Proc_DMSAttachments_Insert, the "Attachment Folder Path" check the DAL makes) are not ported; the history NoOfAttachments link opens nothing.</li>
 *  <li>The shortcut-keys dialog, the "..." shift button (frmGenShift pop-up), the grid layout files (ctrlGrdBar1-7), the Formulas button (empty handler in the desktop) are not ported.</li>
 *  <li>Admin-only buttons "Auto Update" (btnRecordsUpdate) and "Auto Update Re-Processing" (btnReProcessing, AutoUpdatedRecords:6057) are not ported; the header AutoUpdate flag is always false.</li>
 *  <li>The desktop never clears lstRemoveRecordConsumption in FromReset (a removed consumption row of one record can be sent with the next save); the page clears all three removed lists on New.</li>
 *  <li>History: the form never sets ReportsParameters.EntryUser, so a user without the "Can View All Record" right sends @EntryUserId 0 (kept as is). The Shift Name filter combo is bound
 *      but never passed to the procedure by the desktop either.</li>
 *  <li>Numbers are double on the page and on the server (the desktop uses decimal); Math.Round / ToString are approximated (15 significant digits).</li>
 *  <li>Input rows loaded from a BOM in the multi-BOM path carry RateUomCode as the int cast the desktop does (hidden column, not saved).</li>
 *  <li>Nothing here has been run against the database or a browser.</li>
 * </ol>
 */
@Service
public class W6BFeedManualService {

    public static final String SCREEN = "frmProductionManualFeed";
    public static final int DOC_TYPE = 1122;

    private static final Set<Integer> ACCOUNT_TYPES = new LinkedHashSet<>(java.util.Arrays.asList(3, 6, 8, 11, 12, 13, 14, 20, 21));    // dtFillsForGrids:771

    private final Prod273Support db;
    private final W6BFeedManualRepository repo;
    private final ProductionInputRepository inRepo;
    private final PlatformTransactionManager tm;

    public W6BFeedManualService(Prod273Support db, W6BFeedManualRepository repo, ProductionInputRepository inRepo, PlatformTransactionManager tm) {
        this.db = db;
        this.repo = repo;
        this.inRepo = inRepo;
        this.tm = tm;
    }

    // ====================================================================================== helpers

    private static void need(boolean ok, String message) { if (!ok) throw new IllegalArgumentException(message); }

    private void right(String key, String message) {
        if (!db.has(SCREEN, key)) throw new IllegalStateException(message);
    }

    private static int userId(UserAccount u) { return u.getId() == null ? 0 : u.getId(); }

    private static double d(Object v) { return toDouble(v); }

    /** C# double.ToString() in a message (15 significant digits, no exponent). */
    private static String cs(double v) { return ProductionOutputRepository.csDouble(v); }

    private static Map<String, Object> m() { return new LinkedHashMap<>(); }

    private static Map<String, Object> pair(Object id, Object name) {
        Map<String, Object> o = m();
        o.put("Id", id);
        o.put("Name", name);
        return o;
    }

    // ====================================================================================== load (frmBillOfMaterial_Manufacturing_Load:463, btnRefresh_Click:2682)

    public Map<String, Object> lookups() {
        UserAccount u = db.user();
        Map<String, Object> out = m();
        out.put("rights", db.rights(SCREEN));
        out.put("branchId", db.branchId(u));
        out.put("docNo", docNo());
        out.put("dateTypes", Prod273Support.literal(Prod273Support.DATE_TYPES, "Parameters"));
        out.put("financialYearStart", db.financialYearStart(u, db.financialYearId()));
        out.putAll(refresh());
        return out;
    }

    /** The lists the form (re)loads on open and on Refresh. */
    public Map<String, Object> refresh() {
        UserAccount u = db.user();
        int branch = db.branchId(u);
        int fy = db.financialYearId();
        Map<String, Object> out = m();
        out.put("restrictSingleBom", db.configFlag(u, "RestrictSingleBomInOutPut"));                         // :510
        out.put("packingMaterialRequired", db.configFlag(u, "ProductionPackingMaterialRequired"));            // :512
        out.put("amountDecimals", toInt(db.config(u, "DefaultNoofDecimalPointsForAmount")));
        out.put("productionTypes", Prod273Support.project(repo.productionTypes(), "Id", "ProductionTypeDescription"));   // ProductionTypeFill:653
        out.put("shifts", Prod273Support.project(repo.shifts(u), "ShiftId", "ShiftName"));                                 // ShiftComboFill:685
        out.put("plants", Prod273Support.project(repo.plants(u), "Id", "Description"));                                   // PlantFeader:730
        out.put("warehouses", Prod273Support.project(repo.activeWarehouses(u), "Id", "WareHouseName"));                    // dtWare
        List<Map<String, Object>> items = new ArrayList<>();                                                              // dtItem = CommonServices.ReadAllItems
        for (Map<String, Object> r : db.itemsReadAll(u)) {
            Map<String, Object> o = m();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ItemCode", str(ci(r, "ItemCode")));
            items.add(o);
        }
        out.put("items", items);
        List<Map<String, Object>> cons = new ArrayList<>();                                                               // lstConsumptionitem = GetItemByItemTypeId("14,17")
        for (Map<String, Object> r : repo.consumptionItems(u)) {
            Map<String, Object> o = m();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ParentCategory", str(ci(r, "ParentCategory")));
            cons.add(o);
        }
        out.put("consumptionItems", cons);
        List<Map<String, Object>> accounts = new ArrayList<>();                                                           // dtAccounts (dtFillsForGrids:771)
        for (Map<String, Object> r : db.coaAllocation(u)) {
            if (ACCOUNT_TYPES.contains(toInt(ci(r, "AccountTypeId")))) {
                Map<String, Object> o = m();
                o.put("Id", toInt(ci(r, "Id")));
                o.put("AccountTitle", str(ci(r, "AccountTitle")));
                o.put("AccountCode", str(ci(r, "AccountCode")));
                accounts.add(o);
            }
        }
        out.put("accounts", accounts);
        out.put("uoms", db.allUoms(u));                                                                                   // dtuomGetAll = UOMSchedule.Getall
        out.put("bomItems", Prod273Support.project(repo.bomItems(u, branch, fy), "ItemId", "ItemName"));                    // ItemFillFromBom:807 (the Id is the ItemId)
        List<Map<String, Object>> bomData = new ArrayList<>();                                                            // dtBOMDataforCombo = GetAllBomNoAndItems
        for (Map<String, Object> r : repo.bomNoAndItems(u, branch, fy)) bomData.add(bomRow(r));
        out.put("bomData", bomData);
        return out;
    }

    private static Map<String, Object> bomRow(Map<String, Object> r) {
        Map<String, Object> o = m();
        o.put("Id", toInt(ci(r, "Id")));
        o.put("Name", str(ci(r, "BomName")));
        o.put("ItemId", toInt(ci(r, "ItemId")));
        o.put("ItemUomId", toInt(ci(r, "ItemUomId")));
        o.put("PackUom", str(ci(r, "PackUom")));
        o.put("PackUomEquivalent", d(ci(r, "PackUomEquivalent")));
        o.put("BomBatchQty", d(ci(r, "BomBatchQty")));
        return o;
    }

    /** DocumentNoFill:628. */
    public String docNo() {
        UserAccount u = db.user();
        List<Map<String, Object>> r = repo.generateCode(u, db.financialYearId());
        return r.isEmpty() ? "0" : String.valueOf(toInt(ci(r.get(0), "DocNo")));
    }

    /** BOMNobyItemFill:923 - BomHeader.GetBomNoByItemId. */
    public List<Map<String, Object>> bomsByItem(int itemId) {
        UserAccount u = db.user();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.bomsByItem(u, db.branchId(u), db.financialYearId(), itemId)) out.add(bomRow(r));
        return out;
    }

    /**
     * FillInputDetailForMultiBom:1185 - the input rows of the BOMs on the output grid (dtInput column order). {@code outputBomIds} are the BomHeaderId cells of the output grid in row order.
     * RestrictSingleBomInOutPut: getBomdataBySingleBomId of the first row; otherwise GetBomdataByIds with the ",id" string the desktop builds (its String.Contains check is kept).
     */
    public Map<String, Object> inputsFromBom(List<Integer> outputBomIds) {
        UserAccount u = db.user();
        boolean single = db.configFlag(u, "RestrictSingleBomInOutPut");
        int warehouseId = 0;
        String warehouseName = "";
        for (Map<String, Object> w : repo.activeWarehouses(u)) {                                                          // first dtWare row with Id > 0
            if (toInt(ci(w, "Id")) > 0) { warehouseId = toInt(ci(w, "Id")); warehouseName = str(ci(w, "WareHouseName")); break; }
        }
        List<Map<String, Object>> rows = new ArrayList<>();
        if (single) {
            int bomId = outputBomIds.isEmpty() ? 0 : outputBomIds.get(0);
            for (Map<String, Object> r : repo.bomDataBySingle(u, bomId)) {
                double qty = d(ci(r, "ItemQty"));
                double eq = d(ci(r, "PUomEquivalent"));
                Map<String, Object> o = blankInput();
                o.put("BomHeaderId", toInt(ci(r, "BomHeaderId")));
                o.put("BomNo", str(ci(r, "BomHeaderNo")));
                o.put("BomInputId", toInt(ci(r, "BomInputId")));
                o.put("WarehouseId", warehouseId);
                o.put("WarehouseName", warehouseName);
                o.put("ItemId", toInt(ci(r, "ItemId")));
                o.put("ItemName", str(ci(r, "ItemName")));
                o.put("ItemUomId", toInt(ci(r, "ItemUomId")));
                o.put("Uom", eq);
                o.put("UomCode", str(ci(r, "PackUom")));
                o.put("BomQty", qty);
                o.put("ItemQty", qty);
                o.put("NetWeight", qty * eq);
                boolean hasRate = toInt(ci(r, "RateUomId")) > 0;
                o.put("RateUomId", hasRate ? toInt(ci(r, "RateUomId")) : toInt(ci(r, "ItemUomId")));
                o.put("RateUom", hasRate ? toInt(ci(r, "RateEquivalent")) : toInt(ci(r, "PUomEquivalent")));          // Conversion.ToInt(...) in the desktop
                o.put("RateUomCode", String.valueOf(hasRate ? toInt(ci(r, "RateUom")) : toInt(ci(r, "PackUom"))));
                rows.add(o);
            }
        } else {
            StringBuilder ids = new StringBuilder();
            for (Integer b : outputBomIds) {
                if (b != null && b > 0 && !ids.toString().contains(String.valueOf(b))) ids.append(",").append(b);
            }
            for (Map<String, Object> r : repo.bomDataByIds(u, ids.toString())) {
                Map<String, Object> o = blankInput();
                o.put("WarehouseId", warehouseId);
                o.put("WarehouseName", warehouseName);
                o.put("ItemId", toInt(ci(r, "ItemId")));
                o.put("ItemName", str(ci(r, "ItemName")));
                o.put("ItemUomId", toInt(ci(r, "ItemUomId")));
                o.put("Uom", d(ci(r, "PUomEquivalent")));
                o.put("UomCode", str(ci(r, "PackUom")));
                boolean hasRate = toInt(ci(r, "RateUomId")) > 0;
                o.put("RateUomId", hasRate ? toInt(ci(r, "RateUomId")) : toInt(ci(r, "ItemUomId")));
                o.put("RateUom", hasRate ? toInt(ci(r, "RateEquivalent")) : toInt(ci(r, "PUomEquivalent")));
                o.put("RateUomCode", String.valueOf(hasRate ? toInt(ci(r, "RateUom")) : toInt(ci(r, "PackUom"))));
                rows.add(o);
            }
        }
        Map<String, Object> out = m();
        out.put("single", single);
        out.put("rows", rows);
        return out;
    }

    private static Map<String, Object> blankInput() {
        Map<String, Object> o = m();
        o.put("Id", 0);
        o.put("BomHeaderId", 0);
        o.put("BomNo", "");
        o.put("BomInputId", 0);
        o.put("WarehouseId", 0);
        o.put("WarehouseName", "");
        o.put("ItemId", 0);
        o.put("ItemName", "");
        o.put("ItemUomId", 0);
        o.put("Uom", 0d);
        o.put("UomCode", "");
        o.put("BalanceStock", 0d);
        o.put("BomQty", 0d);
        o.put("ItemQty", 0d);
        o.put("PrctOfTotalQty", 0d);
        o.put("NetWeight", 0d);
        o.put("BomRate", 0d);
        o.put("Rate", 0d);
        o.put("RateUomId", 0);
        o.put("RateUom", 0d);
        o.put("RateUomCode", "");
        o.put("Amount", 0d);
        o.put("BomMinQty", 0d);
        o.put("BomMaxQty", 0d);
        return o;
    }

    /**
     * Stock in hand and average rate of items on a document date (grdInputMaterial_KeyDown:4600 / UpdateRowStockAndRate:5799): per distinct item the
     * stock procedure (Organization and Company swapped, as the form builds it) and fed.usp_getAvgRateByItemId (DocumentTypeId 1122, RecId).
     */
    public List<Map<String, Object>> itemRates(Map<String, Object> body) {
        UserAccount u = db.user();
        Timestamp docDate = pickerValue(body.get("docDate"));
        int recId = toInt(body.get("recId"));
        List<Map<String, Object>> out = new ArrayList<>();
        Set<Integer> seen = new LinkedHashSet<>();
        Object ids = body.get("itemIds");
        if (ids instanceof List) {
            for (Object o : (List<?>) ids) {
                int itemId = toInt(o);
                if (itemId <= 0 || !seen.add(itemId)) continue;
                Map<String, Object> r = m();
                r.put("itemId", itemId);
                r.put("stock", repo.stockInHand(u, itemId, docDate));
                r.put("rate", repo.avgRateByItemId(u, itemId, docDate, recId));
                out.add(r);
            }
        }
        return out;
    }

    // ====================================================================================== history / read / delete

    /** AllCombobind:3570 - Production No / Production Type / Shift Name drop-downs of the history filters. */
    public Map<String, Object> historyLookups() {
        List<Map<String, Object>> no = new ArrayList<>(), type = new ArrayList<>(), shift = new ArrayList<>();
        for (Map<String, Object> r : repo.historyDropDowns(db.user())) {
            String activity = str(ci(r, "Activity"));
            Map<String, Object> o = pair(toInt(ci(r, "Id")), str(ci(r, "ReferenceName")));
            if ("ProductionNo".equals(activity)) no.add(o);
            else if ("ProductionType".equals(activity)) type.add(o);
            else if ("ShiftName".equals(activity)) shift.add(o);
        }
        Map<String, Object> out = m();
        out.put("productionNo", no);
        out.put("productionType", type);
        out.put("shift", shift);
        return out;
    }

    /** HistoryGridFill:3781 - the 17 columns of the desktop history table. */
    public List<Map<String, Object>> history(String fromDate, String toDate, int fromDocNo, int toDocNo, int productionHeaderId, int productTypeId) {
        UserAccount u = db.user();
        boolean viewAll = db.has(SCREEN, "viewAll");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, db.branchId(u), db.financialYearId(), Prod273Support.ts(fromDate) == null ? null : pickerValue(fromDate),
                Prod273Support.ts(toDate) == null ? null : pickerValue(toDate), fromDocNo, toDocNo, productionHeaderId, viewAll, productTypeId)) {
            Map<String, Object> o = m();
            o.put("RecordNo", toInt(ci(r, "RecordNo")));
            o.put("Id", toInt(ci(r, "Id")));
            o.put("DocumentTypeId", toInt(ci(r, "DocumentTypeId")));
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("DocDate", day(ci(r, "DocDate")));
            o.put("ProductionType", str(ci(r, "ProductionTypeDescription")));
            o.put("ShiftName", str(ci(r, "ShiftName")));
            o.put("ProductionNo", str(ci(r, "ProductionNo")));
            o.put("PlantName", str(ci(r, "PlantName")));
            o.put("RemarksHeader", str(ci(r, "RemarksHeader")));
            o.put("EntryDate", stamp(ci(r, "EntryDate")));
            o.put("EntryUserName", str(ci(r, "EntryUserName")));
            o.put("ModifyDate", stamp(ci(r, "ModifyDate")));
            o.put("ModifyUserName", str(ci(r, "ModifyUserName")));
            o.put("IsApproved", truthy(ci(r, "IsApproved")));
            o.put("ApprovedDate", stamp(ci(r, "ApprovedDate")));
            o.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    /** ReadById:3334 / DatagridHistoryDetailbind:4084 - the header and the four detail lists, shaped as the page's grid rows (dtInput / dtOutPutByProduct / dtOverHead / dtConsupmtionDetail). */
    public Map<String, Object> read(int id) {
        List<Map<String, Object>> hs = repo.readHeader(id);
        if (hs.isEmpty()) throw new IllegalArgumentException("Record not found");
        Map<String, Object> h = hs.get(0);
        Map<String, Object> out = m();
        out.put("id", toInt(ci(h, "Id")));
        out.put("docNo", toInt(ci(h, "DocNo")));
        out.put("docDate", day(ci(h, "DocDate")));
        out.put("productionTypeId", toInt(ci(h, "ProductionTypeId")));
        out.put("shiftId", toInt(ci(h, "ShiftId")));
        out.put("plantId", toInt(ci(h, "PlantId")));
        out.put("productionNo", str(ci(h, "ProductionNo")));
        out.put("remarks", str(ci(h, "RemarksHeader")));
        out.put("isApproved", truthy(ci(h, "IsApproved")));

        List<Map<String, Object>> output = new ArrayList<>();
        for (Map<String, Object> r : repo.readOutputs(id)) {
            Map<String, Object> o = m();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("BomHeaderId", toInt(ci(r, "BOMHeaderId")));
            o.put("BomNo", str(ci(r, "BOMHeaderNo")));
            o.put("WarehouseId", toInt(ci(r, "WareHouseId")));
            o.put("WarehouseName", str(ci(r, "WareHouseName")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ItemUomId", toInt(ci(r, "ItemUomId")));
            o.put("Uom", d(ci(r, "ItemUomEquivalent")));
            o.put("UomCode", str(ci(r, "ItemUomCode")));
            o.put("ConsumptionQty", d(ci(r, "ConsumptionQty")));
            o.put("ItemQty", d(ci(r, "ItemQty")));
            o.put("NetWeight", d(ci(r, "NetWeight")));
            o.put("Rate", d(ci(r, "Rate")));
            o.put("RateUomId", toInt(ci(r, "RateUomId")));
            o.put("RateUom", d(ci(r, "RateUomEquivalent")));
            o.put("RateUomCode", str(ci(r, "RateUomCode")));
            o.put("Amount", d(ci(r, "Amount")));
            o.put("BomQty", d(ci(r, "BomQty")));
            output.add(o);
        }
        out.put("output", output);

        List<Map<String, Object>> input = new ArrayList<>();
        for (Map<String, Object> r : repo.readInputs(id)) {
            Map<String, Object> o = m();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("BomHeaderId", toInt(ci(r, "BomHeaderId")));
            o.put("BomNo", str(ci(r, "BomHeaderNo")));
            o.put("BomInputId", toInt(ci(r, "BomInputId")));
            o.put("WarehouseId", toInt(ci(r, "WareHouseId")));
            o.put("WarehouseName", str(ci(r, "WareHouseName")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ItemUomId", toInt(ci(r, "ItemUomId")));
            o.put("Uom", d(ci(r, "ItemUomEquivalent")));
            o.put("UomCode", str(ci(r, "ItemUomCode")));
            o.put("BalanceStock", d(ci(r, "ItemQty")));                                                      // ReadById:3401 puts ItemQty in the Stock column
            o.put("BomQty", d(ci(r, "BomQty")));
            o.put("ItemQty", d(ci(r, "ItemQty")));
            o.put("PrctOfTotalQty", d(ci(r, "PrctOfTotalQty")));
            o.put("NetWeight", d(ci(r, "NetWeight")));
            o.put("BomRate", d(ci(r, "BomRate")));
            o.put("Rate", d(ci(r, "Rate")));
            o.put("RateUomId", toInt(ci(r, "RateUomId")));
            o.put("RateUom", d(ci(r, "RateUomEquivalent")));
            o.put("RateUomCode", str(ci(r, "RateUomCode")));
            o.put("Amount", d(ci(r, "Amount")));
            o.put("BomMinQty", d(ci(r, "BomMinQty")));
            o.put("BomMaxQty", d(ci(r, "BomMaxQty")));
            input.add(o);
        }
        out.put("input", input);

        List<Map<String, Object>> oh = new ArrayList<>();
        for (Map<String, Object> r : repo.readOverheads(id)) {
            Map<String, Object> o = m();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("BomHeaderId", toInt(ci(r, "BomHeaderId")));
            o.put("BomNo", str(ci(r, "BomHeaderNo")));
            o.put("BomOverHeadId", toInt(ci(r, "BomOverHeadId")));
            o.put("OverHeadTypeId", toInt(ci(r, "OverHeadTypeId")));
            o.put("OverHeadAccountId", toInt(ci(r, "OverHeadAccountId")));
            o.put("OverHeadAccount", str(ci(r, "OverHeadAccountTitle")));
            o.put("PercentOfTotalInput", d(ci(r, "PercentOfTotalInput")));
            o.put("BomQty", d(ci(r, "BomQty")));
            o.put("Qty", d(ci(r, "Qty")));
            o.put("Rate", d(ci(r, "Rate")));
            o.put("BomAmount", d(ci(r, "BomAmount")));
            o.put("Amount", d(ci(r, "Amount")));
            o.put("OhRemarks", str(ci(r, "OhRemarks")));
            oh.add(o);
        }
        out.put("overhead", oh);

        List<Map<String, Object>> con = new ArrayList<>();
        for (Map<String, Object> r : repo.readConsumptions(id)) {
            Map<String, Object> o = m();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("BomHeaderId", toInt(ci(r, "BomHeaderId")));
            o.put("BomNo", str(ci(r, "BomHeaderNo")));
            o.put("BomConsumptionId", toInt(ci(r, "BomConsumptionId")));
            o.put("WarehouseId", toInt(ci(r, "WareHouseId")));
            o.put("WarehouseName", str(ci(r, "WareHouseName")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ParentCategory", str(ci(r, "ParentCategory")));
            o.put("ItemUomId", toInt(ci(r, "ItemUomId")));
            o.put("Uom", d(ci(r, "ItemUomEquivalent")));
            o.put("UomCode", str(ci(r, "ItemUomCode")));
            o.put("BalanceStock", d(ci(r, "ItemQty")));
            o.put("BomQty", d(ci(r, "BomQty")));
            o.put("ItemQty", d(ci(r, "ItemQty")));
            o.put("NetWeight", d(ci(r, "NetWeight")));
            o.put("BomRate", d(ci(r, "BomRate")));
            o.put("Rate", d(ci(r, "Rate")));
            o.put("RateUomId", toInt(ci(r, "RateUomId")));
            o.put("RateUom", d(ci(r, "RateUomEquivalent")));
            o.put("RateUomCode", str(ci(r, "RateUomCode")));
            o.put("Amount", d(ci(r, "Amount")));
            o.put("OutPutItemId", toInt(ci(r, "OutPutItemId")));
            con.add(o);
        }
        out.put("consumption", con);
        return out;
    }

    /** VoucherReprot:3558 - CommonServices.VoucherHeadIdGet(Id, 1122). */
    public Map<String, Object> voucherHead(int id) {
        Map<String, Object> o = m();
        o.put("voucherHeadId", inRepo.voucherHeadId(db.user(), DOC_TYPE, id));
        return o;
    }

    /** btnDelete_Click:3452 - BLL 0431 DeleteByID(UserAccount.ID, RecId). */
    public Map<String, Object> delete(int id) {
        right("delete", "You do not have the Delete right on Production Manual.");                          // btnDelete.Enabled = DoHaveCanDelete (:475)
        need(id > 0, "No record found to Delete");
        UserAccount u = db.user();
        new TransactionTemplate(tm).execute(st -> { repo.deleteById(userId(u), id); return null; });
        Map<String, Object> o = m();
        o.put("success", true);
        o.put("message", "Delete Record Successfully");
        return o;
    }

    // ====================================================================================== save (Insert:2809)

    /** Working state of one Insert() call. */
    private static final class Ctx {
        UserAccount u;
        int org, company, branch, fy, uid, recId, headerId, docNo;
        boolean update;
        Timestamp docDate;
        final List<Map<String, Object>> output = new ArrayList<>();       // ProductionOutPutDetailList (model property names)
        final List<Map<String, Object>> input = new ArrayList<>();        // ProductionInputDetailList
        final List<Map<String, Object>> overhead = new ArrayList<>();     // ProductionOverHeadsDetailList
        final List<Map<String, Object>> consumption = new ArrayList<>();  // ProductionConsumptionDetailList
        Map<String, Object> voucher;
        final List<Map<String, Object>> voucherLines = new ArrayList<>();
        final Map<Integer, List<Map<String, Object>>> uomCache = new HashMap<>();
    }

    private List<Map<String, Object>> uomSchedule(Ctx c, int itemId) {
        return c.uomCache.computeIfAbsent(itemId, k -> db.uomsByItem(c.u, k));
    }

    /** CommonServices.GetUomScheduleByItemId + dt.Select("Id='x'"): the UOM must be in the item's schedule; 0 when the item has no schedule rows (the model property is left unset). */
    private int scheduled(Ctx c, int itemId, int uomId, String missing) {
        List<Map<String, Object>> rows = uomSchedule(c, itemId);
        if (rows.isEmpty()) return 0;
        for (Map<String, Object> r : rows) if (toInt(ci(r, "Id")) == uomId) return uomId;
        throw new IllegalArgumentException(missing);
    }

    private static Double parseDecimal(String s) {
        String t = s == null ? "" : s.trim().replace(",", "");
        if (t.isEmpty()) return null;
        try { return Double.valueOf(t); } catch (NumberFormatException e) { return null; }
    }

    public Map<String, Object> save(Map<String, Object> body) {
        Ctx c = new Ctx();
        c.u = db.user();
        c.recId = toInt(body.get("id"));
        c.update = c.recId > 0;
        right(c.update ? "update" : "save", c.update ? "You do not have the Update right on Production Manual." : "You do not have the Save right on Production Manual.");

        /* FormValidation:1321 */
        String docNoText = str(body.get("docNo")).trim();
        need(!(docNoText.isEmpty() || docNoText.equals("0")), "Doc No Field Required");
        int productionTypeId = toInt(body.get("productionTypeId")), plantId = toInt(body.get("plantId")), shiftId = toInt(body.get("shiftId"));
        need(!(plantId == 0 || productionTypeId == 0), "Production Type Field Required");                   // :1329 tests the plant text with the production type
        need(shiftId != 0, "Shift Field Required");
        String productionNo = str(body.get("productionNo"));
        need(!productionNo.trim().isEmpty(), "Production No Field Required");
        need(plantId != 0, "Plant Name Field Required");

        List<Map<String, Object>> inRows = list(body.get("input")), outRows = list(body.get("output")), ohRows = list(body.get("overhead")), conRows = list(body.get("consumption"));
        need(!inRows.isEmpty() && !outRows.isEmpty(), "Grid Record Not Found...\nGrid (Input and OutPut) Should have atleast one record");

        boolean restrictSingle = db.configFlag(c.u, "RestrictSingleBomInOutPut");
        boolean packingRequired = db.configFlag(c.u, "ProductionPackingMaterialRequired");

        /* IsDuplicateRowExists:2739 */
        if (restrictSingle) {
            Set<Integer> bomIds = new LinkedHashSet<>();
            for (Map<String, Object> r : outRows) bomIds.add(toInt(r.get("BomHeaderId")));
            need(bomIds.size() <= 1, "OutPut have More than One BOM");
        }
        Map<String, Integer> unique = new LinkedHashMap<>();
        int rowIndex = 0;
        for (Map<String, Object> r : outRows) {
            String key = toInt(r.get("BomHeaderId")) + "-" + toInt(r.get("ItemId")) + "-" + toInt(r.get("ItemUomId")) + "-" + toInt(r.get("WarehouseId"));
            if (unique.containsKey(key)) {
                throw new IllegalArgumentException("Duplicate Record not added to the grid.\nItem " + str(r.get("ItemName")) + " with BOM No " + str(r.get("BomNo")) + " , Uom "
                        + str(r.get("UomCode")) + " and Warehouse " + str(r.get("WarehouseName")) + " exists in row No " + (rowIndex + 1) + " and Row No " + (unique.get(key) + 1));
            }
            unique.put(key, rowIndex);
            rowIndex++;
        }

        /* Net value against the output total (:2852) */
        double sumOutput = 0;
        for (Map<String, Object> r : outRows) sumOutput += d(r.get("Amount"));
        Double parsed = parseDecimal(str(body.get("totalOutputAutoRated")));
        need(parsed != null, "Invalid input for Total Output Auto Rated Amount.");
        BigDecimal header = BigDecimal.valueOf(parsed).setScale(0, RoundingMode.HALF_EVEN);
        BigDecimal rounded = BigDecimal.valueOf(sumOutput).setScale(0, RoundingMode.HALF_UP);
        if (rounded.subtract(header).compareTo(BigDecimal.ONE) > 0) {
            throw new IllegalArgumentException("Net Value " + header.toPlainString() + " in Header and Total Output Amount " + rounded.toPlainString() + " are not equal");
        }

        c.org = c.u.getOrganizationId();
        c.company = c.u.getCompanyId();
        c.branch = db.branchId(c.u);
        c.fy = db.financialYearId();
        c.uid = userId(c.u);
        c.docNo = toInt(docNoText);
        c.docDate = pickerValue(body.get("docDate"));

        /* input: grid rows with an item (LineId counter = rows done), removed rows last when updating */
        int lineNo = 0;
        for (Map<String, Object> r : inRows) {
            if (toInt(r.get("ItemId")) == 0) continue;
            Map<String, Object> det = new LinkedHashMap<>();
            int rid = c.recId > 0 ? toInt(r.get("Id")) : 0;
            det.put("Id", rid);
            det.put("ActionTypeId", rid > 0 ? 2 : 1);
            det.put("BomInputId", toInt(r.get("BomInputId")));
            det.put("BomHeaderId", toInt(r.get("BomHeaderId")));
            String at = " in Grid Input Row No: " + (lineNo + 1);
            need(toInt(r.get("WarehouseId")) != 0, "Warehouse Field Required" + at);
            det.put("WareHouseId", toInt(r.get("WarehouseId")));
            int itemId = toInt(r.get("ItemId"));
            det.put("ItemId", itemId);
            need(toInt(r.get("ItemUomId")) != 0, "UOM  Field Required" + at);
            det.put("ItemUomId", scheduled(c, itemId, toInt(r.get("ItemUomId")), "UOM in Input Grid Row No: " + (lineNo + 1) + " Is not Define Against Item " + itemId + ". Please Check..."));
            need(d(r.get("ItemQty")) != 0, "ItemQty Field Required" + at);
            det.put("ItemQty", d(r.get("ItemQty")));
            need(d(r.get("NetWeight")) != 0, "NetWeight Field Required" + at);
            det.put("NetWeight", d(r.get("NetWeight")));
            need(d(r.get("Rate")) != 0, "Rate Field Required" + at);
            det.put("Rate", d(r.get("Rate")));
            need(toInt(r.get("RateUomId")) != 0, "Rate UOM Field Required" + at);
            det.put("RateUomId", scheduled(c, itemId, toInt(r.get("RateUomId")), "RateUom in Input Grid Row No: " + (lineNo + 1) + " Is not Define Against Item " + itemId + ". Please Check..."));
            need(d(r.get("Amount")) != 0, "Amount Field Required" + at);
            det.put("Amount", d(r.get("Amount")));
            det.put("PrctOfTotalQty", d(r.get("PrctOfTotalQty")));
            c.input.add(det);
            lineNo++;
        }
        if (c.recId > 0) for (Map<String, Object> r : list(body.get("removedInput"))) c.input.add(removedLine(r, "BomInputId", "BomInputId"));

        /* output: every grid row, removed rows last */
        for (int i = 0; i < outRows.size(); i++) {
            Map<String, Object> r = outRows.get(i);
            int no = i + 1;
            need(toInt(r.get("ItemId")) != 0, "Item Field Required in Grid output Row No: " + no);              // :3143 (the blank placeholder row fails here too)
            Map<String, Object> det = new LinkedHashMap<>();
            int rid = c.recId > 0 ? toInt(r.get("Id")) : 0;
            det.put("Id", rid);
            det.put("ActionTypeId", rid > 0 ? 2 : 1);
            det.put("EntryTypeId", 1);
            det.put("BOMHeaderId", toInt(r.get("BomHeaderId")));
            if (productionTypeId != 4 && productionTypeId != 5) {
                need(toInt(r.get("BomHeaderId")) != 0, "BomHeader Field Required in Grid OutPut Row No: " + no);
            }
            need(toInt(r.get("WarehouseId")) != 0, "Warehouse Field Required in Grid OutPut Row No: " + no);
            det.put("WareHouseId", toInt(r.get("WarehouseId")));
            int itemId = toInt(r.get("ItemId"));
            det.put("ItemId", itemId);
            need(d(r.get("ItemQty")) != 0, "ItemQty Field Required in Grid OutPut Row No: " + no);
            det.put("ItemQty", d(r.get("ItemQty")));
            if (restrictSingle) {
                need(d(r.get("ConsumptionQty")) != 0, "ConsumptionQty Field Required in Grid OutPut Row No: " + no);
                det.put("ConsumptionQty", d(r.get("ConsumptionQty")));
            } else {
                det.put("ConsumptionQty", d(r.get("ItemQty")));
            }
            need(d(r.get("Rate")) != 0, "Rate Field Required in Grid Output Row No: " + no);
            det.put("Rate", d(r.get("Rate")));
            need(!(str(r.get("Amount")).trim().isEmpty() || d(r.get("Rate")) == 0), "AmountField Required in Grid Output Row No: " + no);   // :3199 tests the Rate cell
            det.put("Amount", d(r.get("Amount")));
            need(toInt(r.get("ItemUomId")) != 0, "UOM  Field Required in Grid OutPut Row No: " + no);
            det.put("ItemUomId", scheduled(c, itemId, toInt(r.get("ItemUomId")), "UOM in Output Grid Row No: " + no + " Is not Define Against Item " + itemId + ". Please Check..."));
            need(d(r.get("NetWeight")) != 0, "NetWeight Field Required in Grid OutPut Row No: " + no);
            det.put("NetWeight", d(r.get("NetWeight")));
            need(toInt(r.get("RateUomId")) != 0, "Rate UOM Field Required in Grid Output Row No: " + no);
            det.put("RateUomId", scheduled(c, itemId, toInt(r.get("RateUomId")), "RateUom in output Grid Row No: " + no + " Is not Define Against Item " + itemId + ". Please Check..."));
            c.output.add(det);
        }
        for (Map<String, Object> r : list(body.get("removedOutput"))) {                                       // DeleteOutput (grdOutPutByProduct_ColumnButtonClick:2310)
            Map<String, Object> det = new LinkedHashMap<>();
            det.put("Id", toInt(r.get("Id")));
            det.put("WareHouseId", toInt(r.get("WarehouseId")));
            det.put("ItemId", toInt(r.get("ItemId")));
            det.put("ItemUomId", 0);
            det.put("ItemQty", d(r.get("ItemQty")));
            det.put("NetWeight", 0d);
            det.put("Rate", d(r.get("Rate")));
            det.put("RateUomId", 0);
            det.put("Amount", 0d);
            det.put("ActionTypeId", 3);
            c.output.add(det);
        }

        /* over head: only rows with an account */
        for (int i = 0; i < ohRows.size(); i++) {
            Map<String, Object> r = ohRows.get(i);
            if (toInt(r.get("OverHeadAccountId")) <= 0) continue;
            Map<String, Object> det = new LinkedHashMap<>();
            det.put("Id", c.recId > 0 ? toInt(r.get("Id")) : 0);
            det.put("BomOverHeadId", toInt(r.get("BomOverHeadId")));
            det.put("OverHeadTypeId", toInt(r.get("OverHeadAccountId")));
            det.put("OverHeadAccountId", toInt(r.get("OverHeadAccountId")));
            need(d(r.get("Amount")) != 0, "Amount Field Required in Grid Over Head Row No: " + (i + 1));
            det.put("PercentOfTotalInput", d(r.get("PercentOfTotalInput")));
            det.put("Qty", d(r.get("Qty")));
            det.put("BomQty", d(r.get("BomQty")));
            det.put("Rate", d(r.get("Rate")));
            det.put("Amount", d(r.get("Amount")));
            det.put("OhRemarks", str(r.get("OhRemarks")));
            det.put("BomHeaderId", toInt(r.get("BomHeaderId")));
            c.overhead.add(det);
        }

        /* ProductionPackingMaterialRequired (:3299) */
        if (packingRequired) {
            boolean any = false;
            for (Map<String, Object> r : conRows) if ("Packing Material".equals(str(r.get("ParentCategory")))) any = true;
            need(!conRows.isEmpty() && any, "Consumption grid don't have entry of Packing Material. Please Check!");
        }

        /* consumption */
        for (int i = 0; i < conRows.size(); i++) {
            Map<String, Object> r = conRows.get(i);
            if (toInt(r.get("ItemId")) == 0) continue;
            int no = i + 1;
            Map<String, Object> det = new LinkedHashMap<>();
            int rid = c.recId > 0 ? toInt(r.get("Id")) : 0;
            det.put("Id", rid);
            det.put("ActionTypeId", rid > 0 ? 2 : 1);
            det.put("BomConsumptionId", toInt(r.get("BomConsumptionId")));
            det.put("BomHeaderId", toInt(r.get("BomHeaderId")));
            need(toInt(r.get("WarehouseId")) != 0, "Warehouse Field Required in Grid Consumption Row No: " + no);
            det.put("WareHouseId", toInt(r.get("WarehouseId")));
            int itemId = toInt(r.get("ItemId"));
            det.put("ItemId", itemId);
            need(toInt(r.get("ItemUomId")) != 0, "UOM  Field Required in Grid Consumption Row No: " + no);
            det.put("ItemUomId", scheduled(c, itemId, toInt(r.get("ItemUomId")), "UOM in Consumption Grid Row No: " + no + " Is not Define Against Item " + itemId + ". Please Check..."));
            need(d(r.get("ItemQty")) != 0, "ItemQty Field Required in Grid Consumption Row No: " + no);
            det.put("ItemQty", d(r.get("ItemQty")));
            need(d(r.get("NetWeight")) != 0, "NetWeight Field Required in Grid Consumption Row No: " + no);
            det.put("NetWeight", d(r.get("NetWeight")));
            need(d(r.get("Rate")) != 0, "Rate Field Required in Grid Consumption Row No: " + no);
            det.put("Rate", d(r.get("Rate")));
            need(toInt(r.get("RateUomId")) != 0, "Rate UOM Field Required in Grid Consumption Row No: " + no);
            det.put("RateUomId", scheduled(c, itemId, toInt(r.get("RateUomId")), "RateUom in Consumption Grid Row No: " + no + " Is not Define Against Item " + itemId + ". Please Check..."));
            need(d(r.get("Amount")) != 0, "AmountField Required in Grid Consumption Row No: " + no);
            det.put("Amount", d(r.get("Amount")));
            det.put("OutPutItemId", toInt(r.get("OutPutItemId")));
            c.consumption.add(det);
        }
        for (Map<String, Object> r : list(body.get("removedConsumption"))) c.consumption.add(removedLine(r, "BomConsumptionId", "BomConsumptionId"));

        /* attachments: not ported (AttachmentsList / DeleteAttachmentsList stay empty) */
        if (c.update) {
            need(!repo.readHeader(c.recId).isEmpty(), "Record not update because Id not found");
        }

        Timestamp now = Prod273Support.now();
        Map<String, Object> h = new LinkedHashMap<>();                                                        // Insert:2832 + BLL 0431 Save:1
        h.put("Id", c.update ? c.recId : 0);
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("DocNo", c.docNo);
        h.put("DocDate", c.docDate);
        h.put("WorkOrderId", 0);
        h.put("ProductionNo", productionNo);
        h.put("RemarksHeader", str(body.get("remarks")));
        h.put("OrganizationId", c.org);
        h.put("CompanyId", c.company);
        h.put("BranchesId", c.branch);
        h.put("ProjectsId", c.branch);                                                                        // ProjectsId = UserAccount.BranchesId (:2832)
        h.put("FinancialYearId", c.fy);
        h.put("EntryUserId", c.update ? 0 : c.uid);                                                           // Save: update sets EntryUserId = 0, insert sets ModifyUserId = 0
        h.put("EntryDate", now);
        h.put("ModifyUserId", c.update ? c.uid : 0);
        h.put("ModifyDate", now);
        h.put("IsApproved", false);
        h.put("ApprovedUserId", c.uid);
        h.put("ApprovedDate", now);
        h.put("ProductionTypeId", productionTypeId);
        h.put("ActionId", c.update ? 2 : 1);
        h.put("WIPWarehouseId", 0);
        h.put("WIPGLAccountID", 0);
        h.put("PlantId", plantId);
        h.put("AutoUpdate", false);
        h.put("ShiftId", shiftId);
        h.put("SupplierCustomerId", 0);
        h.put("IsUploaded", false);

        c.voucher = makeVoucher(c, now);                                                                      // BLL Save: MakeVoucher before the DAL (obj.Id still 0 on insert)

        int newId = new TransactionTemplate(tm).execute(st -> setData(c, h));
        return Prod273Support.ok((c.update ? "Data Update Successfully....  " : "Data Save Successfully....  ") + c.docNo, newId, c.docNo);
    }

    /** The removed-row objects the delete buttons build (ActionTypeId 3). */
    private static Map<String, Object> removedLine(Map<String, Object> r, String bomKey, String bomProp) {
        Map<String, Object> det = new LinkedHashMap<>();
        det.put("Id", toInt(r.get("Id")));
        det.put(bomProp, toInt(r.get(bomKey)));
        det.put("WareHouseId", toInt(r.get("WarehouseId")));
        det.put("ItemId", toInt(r.get("ItemId")));
        det.put("ItemUomId", toInt(r.get("ItemUomId")));
        det.put("ItemQty", d(r.get("ItemQty")));
        det.put("NetWeight", d(r.get("NetWeight")));
        det.put("Rate", d(r.get("Rate")));
        det.put("RateUomId", toInt(r.get("RateUomId")));
        det.put("Amount", d(r.get("Amount")));
        det.put("ActionTypeId", 3);
        return det;
    }

    // -------------------------------------------------------------------------------- BLL 0431 MakeVoucher (:17)

    private static Map<String, Object> gl(List<Map<String, Object>> items, int itemId) {
        for (Map<String, Object> g : items) if (toInt(ci(g, "Id")) == itemId) return g;
        return null;
    }

    private Map<String, Object> makeVoucher(Ctx c, Timestamp now) {
        List<Map<String, Object>> items = inRepo.itemGlAccounts(c.u);
        Map<String, Object> vh = ProductionInputRepository.voucherHeadDefaults();
        vh.put("DocumentTypeSrNo", c.update ? c.recId : 0);
        vh.put("VoucherDate", c.docDate);
        vh.put("VoucherCode", c.docNo);
        vh.put("DocumentTypeId", DOC_TYPE);
        vh.put("EntryDate", now);
        vh.put("ModifyDate", now);
        vh.put("EntryUser", c.update ? 0 : c.uid);
        vh.put("ModifyUser", c.update ? c.uid : 0);
        vh.put("OrganizationId", c.org);
        vh.put("CompanyId", c.company);
        vh.put("FinancialYearId", c.fy);
        vh.put("BranchId", c.branch);
        vh.put("IsApproved", false);
        double total = 0;
        int lineId = 1;
        for (Map<String, Object> o : c.output) {
            if (toInt(o.get("ActionTypeId")) == 3) continue;
            need(!items.isEmpty(), "Item GL Account Not Found");
            Map<String, Object> g = gl(items, toInt(o.get("ItemId")));
            if (g == null) continue;                                                                           // not in the list: skipped, no error (the desktop does the same)
            String text = " ItemName: " + str(ci(g, "ItemName")) + " Qty  " + cs(d(o.get("NetWeight"))) + " Rate  " + cs(d(o.get("Rate")));    // Remarks is null in the desktop: "" + " ItemName: "
            Map<String, Object> v = ProductionInputRepository.voucherDetailDefaults();
            v.put("LineId", lineId);
            v.put("AccountId", toInt(ci(g, "PurchaseGLAC")));
            v.put("AgainstAccountId", toInt(ci(g, "PurchaseGLAC")));
            v.put("Comments", text);
            v.put("DebitAmount", d(o.get("Amount")));
            v.put("QtyIn", d(o.get("ItemQty")));
            v.put("WeightIn", d(o.get("NetWeight")));
            v.put("ItemRate", d(o.get("Rate")));
            v.put("ItemAmount", d(o.get("Amount")));
            c.voucherLines.add(v);
            total += d(v.get("DebitAmount"));
            lineId++;
        }
        lineId = 1000;
        for (Map<String, Object> o : c.input) {
            if (toInt(o.get("ActionTypeId")) == 3) continue;
            need(!items.isEmpty(), "Item GL Account Not Found");
            Map<String, Object> g = gl(items, toInt(o.get("ItemId")));
            if (g == null) continue;
            String text = " ItemName: " + str(ci(g, "ItemName")) + " Qty  " + cs(d(o.get("NetWeight"))) + " Rate  " + cs(d(o.get("Rate")));
            Map<String, Object> v = ProductionInputRepository.voucherDetailDefaults();
            v.put("LineId", lineId);
            v.put("AccountId", toInt(ci(g, "PurchaseGLAC")));
            v.put("AgainstAccountId", toInt(ci(g, "PurchaseGLAC")));
            v.put("Comments", text);
            v.put("CreditAmount", d(o.get("Amount")));
            v.put("QtyOut", d(o.get("ItemQty")));
            v.put("WeightOut", d(o.get("NetWeight")));
            v.put("ItemRate", d(o.get("Rate")));
            v.put("ItemAmount", d(o.get("Amount")));
            c.voucherLines.add(v);
            total += d(v.get("CreditAmount"));
            lineId++;
        }
        for (Map<String, Object> o : c.overhead) {                                                             // LineId stays 0; the amount is not added to the voucher total (BLL :153)
            String remarks = str(o.get("OhRemarks"));
            if (remarks.isEmpty()) {
                remarks = "OverHeads ";
                if (d(o.get("Qty")) > 0) remarks = remarks + "Qty " + cs(d(o.get("Qty")));
                if (d(o.get("Rate")) > 0) remarks = remarks + " Rate " + cs(d(o.get("Rate")));
                if (d(o.get("Amount")) > 0) remarks = remarks + " Amount " + cs(d(o.get("Amount")));
                o.put("OhRemarks", remarks);                                                                   // the desktop writes it back into the row it saves
            }
            Map<String, Object> v = ProductionInputRepository.voucherDetailDefaults();
            v.put("AccountId", toInt(o.get("OverHeadAccountId")));
            v.put("AgainstAccountId", toInt(o.get("OverHeadAccountId")));
            v.put("Comments", remarks);
            v.put("CreditAmount", d(o.get("Amount")));
            v.put("QtyOut", d(o.get("Qty")));
            v.put("WeightOut", d(o.get("Qty")));
            v.put("ItemRate", d(o.get("Rate")));
            v.put("ItemAmount", d(o.get("Amount")));
            c.voucherLines.add(v);
        }
        lineId = 2000;
        for (Map<String, Object> o : c.consumption) {
            if (toInt(o.get("ActionTypeId")) == 3) continue;
            need(!items.isEmpty(), "Item GL Account Not Found");
            Map<String, Object> g = gl(items, toInt(o.get("ItemId")));
            if (g == null) continue;
            String text = " ItemName: " + str(ci(g, "ItemName")) + " Qty  " + cs(d(o.get("ItemQty"))) + " Rate  " + cs(d(o.get("Rate")));
            Map<String, Object> v = ProductionInputRepository.voucherDetailDefaults();
            v.put("LineId", lineId);
            v.put("AccountId", toInt(ci(g, "PurchaseGLAC")));
            v.put("AgainstAccountId", toInt(ci(g, "PurchaseGLAC")));
            v.put("Comments", text);
            v.put("CreditAmount", d(o.get("Amount")));
            v.put("QtyOut", d(o.get("ItemQty")));
            v.put("WeightOut", d(o.get("NetWeight")));
            v.put("ItemRate", d(o.get("Rate")));
            v.put("ItemAmount", d(o.get("Amount")));
            c.voucherLines.add(v);
            total += d(v.get("CreditAmount"));
            lineId++;
        }
        vh.put("VoucherAmount", total);
        return vh;
    }

    // -------------------------------------------------------------------------------- DAL 0488 SetData

    private int setData(Ctx c, Map<String, Object> header) {
        int num = repo.setHeader(c.update, model(header, HEADER_PARAMS));
        if (num > 0) c.headerId = num; else { num = c.recId; c.headerId = c.recId; }

        int line = 1;
        for (Map<String, Object> det : c.output) {
            det.put("ProductionHeaderId", c.headerId);
            det.put("WorkOrderId", 0);
            det.put("LineId", line);
            det.put("Id", repo.setOutput(model(det, OUTPUT_PARAMS)));
            line++;
        }
        line = 1000;
        for (Map<String, Object> det : c.input) {
            det.put("ProductionHeaderId", c.headerId);
            det.put("LineId", line);
            det.put("Id", repo.setInput(model(det, INPUT_PARAMS)));
            line++;
        }
        for (Map<String, Object> det : c.overhead) {
            det.put("ProductionHeaderId", c.headerId);
            det.put("Id", repo.setOverhead(model(det, OVERHEAD_PARAMS)));
        }
        line = 2000;
        for (Map<String, Object> det : c.consumption) {
            det.put("LineId", line);
            det.put("ProductionHeaderId", c.headerId);
            det.put("Id", repo.setConsumption(model(det, CONSUMPTION_PARAMS)));
            line++;
        }

        Map<String, Object> se = ProductionInputRepository.stockEvaluationDefaults();
        se.put("OrganizationId", c.org);
        se.put("CompanyId", c.company);
        se.put("RefDocumentTypeId", DOC_TYPE);
        se.put("RefDocIdNo", c.headerId);
        inRepo.setProc("Sp_InventoryStockEvalautionDetail_Update", se, ProductionInputRepository.P_STOCK_EVALUATION_UPDATE);

        Map<String, Object> it = ProductionInputRepository.inventoryTransactionsDefaults();
        it.put("OrganizationId", c.org);
        it.put("CompanyId", c.company);
        it.put("RefDocumentTypeId", DOC_TYPE);
        it.put("RefDocIdNo", num);
        inRepo.setProc("Sp_InventoryTransactions_GetALLMethod", it, ProductionInputRepository.P_INV_TRANSACTIONS);

        for (Map<String, Object> det : c.input) {                                                              // USP_InventoryValidation per input row (AutoUpdate false)
            java.util.LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@OrganizationId", c.org);
            p.put("@CompanyId", c.company);
            p.put("@DocumentTypeId", DOC_TYPE);
            p.put("@DocDate", c.docDate);
            p.put("@ItemId", toInt(det.get("ItemId")));
            p.put("@WarehouseId", toInt(det.get("WareHouseId")));
            p.put("@NetWeight", d(det.get("NetWeight")));
            p.put("@AutoUpdate", false);
            inRepo.command("USP_InventoryValidation", p);
        }
        for (Map<String, Object> det : c.consumption) {                                                        // ... and per consumption row (!AutoUpdate)
            java.util.LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@OrganizationId", c.org);
            p.put("@CompanyId", c.company);
            p.put("@DocumentTypeId", DOC_TYPE);
            p.put("@DocDate", c.docDate);
            p.put("@ItemId", toInt(det.get("ItemId")));
            p.put("@WarehouseId", toInt(det.get("WareHouseId")));
            p.put("@NetWeight", d(det.get("NetWeight")));
            inRepo.command("USP_InventoryValidation", p);
        }

        int existing = inRepo.voucherHeadId(c.u, DOC_TYPE, c.headerId);
        Map<String, Object> vh = c.voucher;
        if (existing > 0) vh.put("Id", existing);
        vh.put("DocumentTypeSrNo", c.headerId);
        int vr = inRepo.setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh, ProductionInputRepository.P_VOUCHER_HEAD);
        int voucherId = vr > 0 ? vr : toInt(vh.get("Id"));
        vh.put("Id", voucherId);
        if (c.voucherLines.isEmpty()) throw new IllegalArgumentException("Voucher Detail list not found...");
        for (Map<String, Object> v : c.voucherLines) {
            int lid = toInt(v.get("LineId"));
            for (Map<String, Object> o : c.output) {                                                           // x.LineId == item.LineId, both > 0, DebitAmount > 0
                if (toInt(o.get("LineId")) == lid && lid > 0 && d(v.get("DebitAmount")) > 0) { v.put("RefDocSubIdNo", toInt(o.get("Id"))); break; }
            }
            for (Map<String, Object> o : c.input) {
                if (toInt(o.get("LineId")) == lid && lid > 0 && d(v.get("CreditAmount")) > 0) { v.put("RefDocSubIdNo", toInt(o.get("Id"))); break; }
            }
            for (Map<String, Object> o : c.consumption) {
                if (toInt(o.get("LineId")) == lid && lid > 0 && d(v.get("CreditAmount")) > 0) { v.put("RefDocSubIdNo", toInt(o.get("Id"))); break; }
            }
            v.put("VoucherHeadId", voucherId);
            inRepo.setProc("Sp_VoucherDetail_Insert", v, ProductionInputRepository.P_VOUCHER_DETAIL);
        }
        if (voucherId > 0) {
            java.util.LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@OrganizationId", c.org);
            p.put("@CompanyId", c.company);
            p.put("@Id", voucherId);
            inRepo.command("USP_VoucherBalanceCheck", p);
        }
        /* AttachmentsList / DeleteAttachmentsList (Proc_DMSAttachments_Insert, file copy) are not ported. */
        return num;
    }

    // -------------------------------------------------------------------------------- model -> declared parameters

    private static final Set<String> HEADER_PARAMS = names("Id", "DocumentTypeId", "DocNo", "DocDate", "WorkOrderId", "ProductionNo", "RemarksHeader", "OrganizationId", "CompanyId",
            "BranchesId", "ProjectsId", "FinancialYearId", "EntryUserId", "EntryDate", "ModifyUserId", "ModifyDate", "IsApproved", "ApprovedUserId", "ApprovedDate", "ProductionTypeId",
            "ActionId", "WIPWarehouseId", "WIPGLAccountID", "PlantId", "AutoUpdate", "ShiftId", "SupplierCustomerId", "IsUploaded");
    private static final Set<String> OUTPUT_PARAMS = names("Id", "ProductionHeaderId", "WorkOrderId", "BOMHeaderId", "EntryTypeId", "SaleOrderId", "WareHouseId", "ItemId", "ItemUomId",
            "ItemQty", "NetWeight", "Rate", "RateUomId", "Amount", "Remarks", "ActionTypeId", "LineId", "ConsumptionQty", "JobLotId", "PriceTypeId");
    private static final Set<String> INPUT_PARAMS = names("Id", "ProductionHeaderId", "BomInputId", "WareHouseId", "ItemId", "ItemUomId", "ItemQty", "PrctOfTotalQty", "NetWeight", "Rate",
            "RateUomId", "Amount", "Remarks", "ActionTypeId", "LineId", "BomHeaderId");
    private static final Set<String> OVERHEAD_PARAMS = names("Id", "ProductionHeaderId", "BomOverHeadId", "OverHeadTypeId", "OverHeadAccountId", "PercentOfTotalInput", "Qty", "Rate",
            "BomQty", "Amount", "OhRemarks", "BomHeaderId");
    private static final Set<String> CONSUMPTION_PARAMS = names("Id", "ProductionHeaderId", "BomConsumptionId", "WareHouseId", "ItemId", "ItemUomId", "ItemQty", "NetWeight", "Rate",
            "RateUomId", "Amount", "Remarks", "ActionTypeId", "LineId", "BomHeaderId", "OutPutItemId");
    private static final Set<String> STRING_PARAMS = names("Remarks", "OhRemarks", "ProductionNo", "RemarksHeader");

    private static Set<String> names(String... n) { return new LinkedHashSet<>(java.util.Arrays.asList(n)); }

    /**
     * GenericProvider.SetProc reflects over the model: every value-type property is sent (a CLR default when never set), reference types (the strings) only when not null.
     * The procedures declare fewer parameters than the models have properties, so the model's properties are filtered to the declared ones, with CLR defaults filled in.
     */
    private static Map<String, Object> model(Map<String, Object> det, Set<String> declared) {
        Map<String, Object> p = new LinkedHashMap<>();
        for (String k : declared) {
            Object v = det.get(k);
            if (v == null) {
                if (STRING_PARAMS.contains(k)) continue;
                v = 0;
            }
            p.put(k, v instanceof BigDecimal ? ((BigDecimal) v).doubleValue() : v);
        }
        return p;
    }
}
