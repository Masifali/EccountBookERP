package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.Pcc597ConcreteProductionRepository;
import com.mst.repositories.ProductionInputRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.PlatformTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
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
import static com.mst.services.Prod273Support.ts;

/**
 * Pcc597 - Production hub screen 597 "Concrete Production" - Architecture.WinApp.Pcc.frmConcreteProduction (.cs 10,355 lines), rights key base.Name
 * "frmConcreteProduction", DocumentTypeId 1850 (Insert:3508, ReadById:3843, btnDelete_Click:3946, HistoryGridFill:4328). Schema [pcc].
 * BLL Architecture.BLL.pcc.ProductionHeader (0350, MakeVoucher + Save), DAL 0288 (SetData).
 *
 * <pre>
 *   Route     /production/pcc/concrete-production      API /production/pcc/concrete-production/api
 *   Siblings  /production/pcc-conversion (822, frmConversion 1867) and /production/pcc/stock-transfer-manual-concrete share the concrete FIFO costing.
 * </pre>
 *
 * SAVE (BLL Save + DAL SetData, one transaction): MakeVoucher (WIP account is mandatory) -> header [pcc].[USP_ProductionHeader_Insert|Update] -> output rows
 * (WorkOrderId = header's) -> input rows -> overhead rows -> wages rows -> (ERP feature 5) FIFO stock evaluation rows via [pcc].[USP_GetStockByFifoMethod] and the
 * "input amount = cgs amount" check -> Sp_InventoryStockEvalautionDetail_Update -> Sp_InventoryTransactions_GetALLMethod -> USP_InventoryValidation for every input row
 * -> Sp_VoucherHead_Insert|Update + Sp_VoucherDetail_Insert + USP_VoucherBalanceCheck. The voucher has no output (finished-goods) line, as in MakeVoucher.
 *
 * KNOWN DIFFERENCES / GAPS
 * <ol>
 *  <li>Attachments (Attachment dialog, DMS copy, Proc_DMSAttachments_Insert) are not ported.</li>
 *  <li>BtnLoadInputFromBom: the desktop's overhead Rows.Add passes 11 values for 12 columns (BomQty gets Qty, Qty gets Rate, Rate gets Amount, Amount gets the remarks
 *      text), which would throw on a decimal column. The intended mapping (BomQty = Qty, Qty, Rate, Amount, OhRemarks) is ported instead.</li>
 *  <li>The header's Item / Uom / Bom / Qty come from the output grid's CurrentRow on the desktop; the page sends the row that is selected (first row by default).</li>
 *  <li>Numbers are computed with double (the desktop uses decimal); generated remarks print numbers without trailing zeros.</li>
 *  <li>History sends EntryUserId when the user lacks CanViewAllRecord (as BLL FormHistory does).</li>
 *  <li>The alternative-item list uses 'GetAlternativeItemsByMainItemId', whose join in the dump reads "u.itemid = dr.itemid" (alias dr does not exist); if the live
 *      database has the same text the procedure error reaches the user unchanged.</li>
 *  <li>Nothing here has been run against the database or a browser.</li>
 * </ol>
 */
@Service
public class Pcc597ConcreteProductionService {

    public static final String SCREEN = "frmConcreteProduction";
    public static final int DOC_TYPE = 1850;
    private static final int ERP_FEATURE_FIFO_CGS = 5;
    private static final String CFG_WAGES_DEBIT = "ContractorWagesDebitTotExpenseAcOnProductionConcrete";
    private static final Set<Integer> OVERHEAD_ACCOUNT_TYPES = new HashSet<>(java.util.Arrays.asList(3, 6, 8, 11, 12, 13, 14, 20, 21));

    private final Prod273Support db;
    private final Pcc597ConcreteProductionRepository repo;
    private final ProductionInputRepository inRepo;
    private final PlatformTransactionManager tm;

    public Pcc597ConcreteProductionService(Prod273Support db, Pcc597ConcreteProductionRepository repo, ProductionInputRepository inRepo, PlatformTransactionManager tm) {
        this.db = db;
        this.repo = repo;
        this.inRepo = inRepo;
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

    /** .NET double.ToString(): shortest form, no trailing ".0". */
    static String net(double v) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return "0";
        if (v == Math.rint(v) && Math.abs(v) < 1e15) return String.valueOf((long) v);
        return BigDecimal.valueOf(v).stripTrailingZeros().toPlainString();
    }

    private static double round(double v, int places) {
        return BigDecimal.valueOf(v).setScale(places, RoundingMode.HALF_EVEN).doubleValue();
    }

    private static boolean empty(Object v) { return v == null || String.valueOf(v).trim().isEmpty(); }

    private static void need(boolean ok, String message) { if (!ok) throw new IllegalArgumentException(message); }

    private void right(String key, String message) {
        if (!db.has(SCREEN, key)) throw new IllegalStateException(message);
    }

    /** FormHelper.ValidateField(value, field, rowIndex) for an int property: only 0 fails. */
    private static void fieldInt(int v, String field, int rowNo) {
        if (v == 0) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + rowNo);
    }

    /** FormHelper.ValidateField for a decimal property: <= 0 fails. */
    private static void fieldDec(double v, String field, int rowNo) {
        if (v <= 0) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + rowNo);
    }

    private static void fieldStr(String v, String field, int rowNo) {
        if (v == null || v.trim().isEmpty()) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + rowNo);
    }

    private boolean wagesDebitConfig(UserAccount u) { return db.configFlag(u, CFG_WAGES_DEBIT); }

    // ====================================================================================== load (frmConcreteProduction_Load, .cs 520-600)

    /** Load: FifoMethod = ERP feature 5, rights, config, DocumentNoFill, ProductionTypeFill, PlantFeader, dtFillsForGrids, WIP/Wages fills, Datetypefill, AllCombobind. */
    public Map<String, Object> lookups() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", db.rights(SCREEN));
        out.put("branchId", db.branchId(u));
        out.put("financialYearStart", db.financialYearStart(u, db.financialYearId()));
        out.put("fifo", inRepo.erpFeature(u, ERP_FEATURE_FIFO_CGS));
        out.put("wagesDebitExpense", wagesDebitConfig(u));                                                 // .cs 544
        out.put("amountDecimals", amountDecimals(u));
        out.put("rateDecimals", db.rateDecimals(u));
        out.put("docNo", docNo());
        out.put("dateTypes", Prod273Support.literal(Prod273Support.DATE_TYPES, "Parameters"));
        out.putAll(refresh());
        out.put("productionNos", productionNos());
        return out;
    }

    /** btnRefresh_Click (.cs 3300): PlantBind, WIP warehouse / account, wages account (+ every lookup of the load). */
    public Map<String, Object> refresh() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> types = new ArrayList<>();
        for (Map<String, Object> r : repo.productionTypes(u)) {
            int id = toInt(ci(r, "Id"));
            if (id == 45 || id == 46 || id == 47) types.add(pair(id, str(ci(r, "LookupName"))));          // ProductionTypeFill:1176
        }
        out.put("productionTypes", types);
        out.put("plants", Prod273Support.project(repo.plants(u), "Id", "Description"));                  // PlantFeaderDbCall:1199
        out.put("wipWarehouses", Prod273Support.project(repo.warehousesByType(u, 5), "Id", "WareHouseName")); // WIPWarehouseFill:1227
        out.put("wipAccounts", Prod273Support.project(repo.accountsByType(u, "4,12"), "Id", "AccountTitle"));  // WIPAccountFill:1259
        out.put("wagesAccounts", Prod273Support.project(repo.accountsByType(u, "11,12,13,14,20,21"), "Id", "AccountTitle")); // WagesExpenseAccountFill:1291
        List<Map<String, Object>> accounts = new ArrayList<>();                                           // dtAccounts (overhead accounts)
        for (Map<String, Object> r : db.coaAllocation(u)) {
            if (OVERHEAD_ACCOUNT_TYPES.contains(toInt(ci(r, "AccountTypeId")))) accounts.add(pair(toInt(ci(r, "Id")), str(ci(r, "AccountTitle"))));
        }
        out.put("overheadAccounts", accounts);
        out.put("allUoms", db.allUoms(u));
        out.put("jobLots", Prod273Support.project(repo.jobLots(u), "Id", "JobLotDescription"));          // dtFillsForGrids:715
        out.put("contractors", Prod273Support.project(repo.contractors(u), "Id", "CompanyName"));         // dtContr
        out.put("refParties", Prod273Support.project(repo.refParties(u), "Id", "ReferencePartyName"));    // RefPartydtDbCall:1019
        List<Map<String, Object>> customers = new ArrayList<>();                                          // SupplierDtFillFromGlobal:997
        for (Map<String, Object> r : repo.customers(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Name", str(ci(r, "CompanyName")));
            o.put("Code", str(ci(r, "PartyCode")));
            o.put("City", str(ci(r, "CityName")));
            customers.add(o);
        }
        out.put("customers", customers);
        out.put("bomNos", bomNos());
        return out;
    }

    private int amountDecimals(UserAccount u) {
        String s = db.config(u, "Default NoofDecimal Points For Amount");
        if (s.isEmpty()) return 2;
        int n = toInt(s);
        return n < 0 || n > 6 ? 2 : n;
    }

    /** DocumentNoFill:756. */
    public int docNo() {
        UserAccount u = db.user();
        return repo.generateCode(u, db.branchId(u), DOC_TYPE, db.financialYearId());
    }

    /** AllCombobind: pcc.USP_GetDataForDropDownFromProductionHeader, rows with Activity "ProductionNo" (Id, ReferenceName). */
    public List<Map<String, Object>> productionNos() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.productionNoDropDown(db.user())) {
            String act = str(ci(r, "Activity"));
            if (!act.isEmpty() && !act.equalsIgnoreCase("ProductionNo")) continue;
            out.add(pair(toInt(ci(r, "Id")), str(ci(r, "ReferenceName"))));
        }
        return out;
    }

    /** ItemdtFillFromBomDbCall:803 - GetAllBomItems for the selected production type (DocumentTypeId 1851). */
    public List<Map<String, Object>> bomItems(int typeId) {
        UserAccount u = db.user();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.bomItems(u, db.branchId(u), db.financialYearId(), typeId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "ItemId")));
            o.put("Name", str(ci(r, "ItemName")));
            o.put("Code", str(ci(r, "ItemCode")));
            o.put("ItemTypeOfTypeId", toInt(ci(r, "ItemTypeOfTypeId")));
            out.add(o);
        }
        return out;
    }

    /** BOMNoAndItemFill:924 - dtBOMDataforCombo (GetAllBomNoAndItems, DocumentTypeId 1851). */
    public List<Map<String, Object>> bomNos() {
        UserAccount u = db.user();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.bomNosAndItems(u, db.branchId(u), db.financialYearId())) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Name", str(ci(r, "BomName")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ItemUomId", toInt(ci(r, "ItemUomId")));
            o.put("PackUom", str(ci(r, "PackUom")));
            o.put("PackUomEquivalent", toDouble(ci(r, "PackUomEquivalent")));
            o.put("BomBatchQty", toDouble(ci(r, "BomBatchQty")));
            out.add(o);
        }
        return out;
    }

    /** AttributeVarientFill:859 - ItemAttributeVarient.GetAllForCombo(item). */
    public List<Map<String, Object>> attributes(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.attributes(db.user(), itemId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "ItemAttributeVarientId")));
            o.put("Name", str(ci(r, "ItemAttribute")));
            o.put("Equivalent", toDouble(ci(r, "VarientEquivalent")));
            out.add(o);
        }
        return out;
    }

    /** CmbPlantName_Leave:1362 - CommonServices.WareHouseAllocationToPlantsByProductionPlantId (dtWareForInput / dtWareForOutPut). */
    public List<Map<String, Object>> plantWarehouses(int plantId) {
        return Prod273Support.project(repo.plantWarehouses(db.user(), plantId), "Id", "WareHouseName");
    }

    /** grdOutPutByProduct F1 on SaleOrder - SaleOrder.GetSaleOrderNo. */
    public List<Map<String, Object>> saleOrders(int customerId, int itemId) {
        return json(repo.saleOrders(db.user(), customerId, itemId));
    }

    /** grdInputMaterial_KeyDown F1 on ItemName - alternative items of a BOM input. */
    public List<Map<String, Object>> alternativeItems(int itemId, int bomHeaderId) {
        return json(repo.alternativeItems(db.user(), itemId, bomHeaderId));
    }

    /** WorkOrderDbCall:1079 - PendingWorkOrderForProduction (RecId = record being edited, OrderDetailIds = the detail ids of that record). */
    public List<Map<String, Object>> workOrders(int recId, String detailIds) {
        UserAccount u = db.user();
        return json(repo.pendingWorkOrders(u, db.branchId(u), db.financialYearId(), recId, detailIds));
    }

    /** Wages F1 - GetServiceActivityByItemId(1, item, docDate, activity, contractor). */
    public List<Map<String, Object>> serviceActivities(int itemId, String docDate, int activityId, int contractorId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.wagesActivities(itemId, pickerValue(docDate), activityId, contractorId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "ContractorWagesRateScheduleId")));
            o.put("Name", str(ci(r, "WagesActivity")));
            o.put("Rate", toDouble(ci(r, "Rate")));
            o.put("Qty", toDouble(ci(r, "Qty")));
            out.add(o);
        }
        return out;
    }

    // ====================================================================================== BtnLoadInputFromBom (FillInputDetailForMultiBom:1602)

    /**
     * One BOM as the three grids' rows: BomHeader.ReadById (input list 'ReadByHeaderId_BomInputDetail', overhead list 'ReadByHeaderId_BomFohDetail') and
     * BomHeader_BomWagesDetailWithUpdatedRatesByHeaderId. Input rows with MainItemId != 0 (alternative items) are skipped (:1681). Warehouse = first plant warehouse
     * with Id > 0 (:1638), job lot = first job lot (:1651). The page de-duplicates against BomInputId / BomOverHeadId / BomWagesId (:1690, :1721, :1750).
     */
    public Map<String, Object> bomLoad(int bomId, int plantId, int outItemId, String outItemName) {
        UserAccount u = db.user();
        int warehouseId = 0;
        String warehouseName = "";
        for (Map<String, Object> w : repo.plantWarehouses(u, plantId)) {
            if (toInt(ci(w, "Id")) > 0) { warehouseId = toInt(ci(w, "Id")); warehouseName = str(ci(w, "WareHouseName")); break; }
        }
        int jobLotId = 0;
        String jobLotName = "";
        List<Map<String, Object>> lots = repo.jobLots(u);
        if (!lots.isEmpty()) { jobLotId = toInt(ci(lots.get(0), "Id")); jobLotName = str(ci(lots.get(0), "JobLotDescription")); }

        List<Map<String, Object>> input = new ArrayList<>();
        for (Map<String, Object> d : repo.bomRead("ReadByHeaderId_BomInputDetail", bomId)) {
            if (toInt(ci(d, "MainItemId")) != 0) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", 0);
            o.put("BomHeaderId", toInt(ci(d, "BomHeaderId")));
            o.put("BomNo", str(ci(d, "BomHeaderNo")));
            o.put("BomInputId", toInt(ci(d, "Id")));
            o.put("WarehouseId", warehouseId);
            o.put("WarehouseName", warehouseName);
            o.put("JobLotId", jobLotId);
            o.put("JobLot", jobLotName);
            o.put("ItemId", toInt(ci(d, "ItemId")));
            o.put("ItemName", str(ci(d, "ItemName")));
            o.put("ItemUomId", toInt(ci(d, "ItemUomId")));
            o.put("Uom", toDouble(ci(d, "PackUomEquivalent")));
            o.put("UomCode", str(ci(d, "PackUom")));
            o.put("BalanceStock", 0d);
            o.put("BomQty", toDouble(ci(d, "ItemQty")));
            o.put("ItemQty", toDouble(ci(d, "ItemQty")));
            o.put("NetWeight", toDouble(ci(d, "WeightKg")));
            o.put("BomRate", toDouble(ci(d, "Rate")));
            o.put("Rate", 0d);
            o.put("RateUomId", toInt(ci(d, "RateUomId")));
            o.put("RateUom", toDouble(ci(d, "RateUomEquivalent")));
            o.put("RateUomCode", str(ci(d, "RateUom")));
            o.put("Amount", toDouble(ci(d, "Amount")));
            o.put("BomMinQty", toDouble(ci(d, "MinQty")));
            o.put("BomMaxQty", toDouble(ci(d, "MaxQty")));
            o.put("MinQty", toDouble(ci(d, "MinQty")));
            o.put("MaxQty", toDouble(ci(d, "MaxQty")));
            o.put("MainItemId", toInt(ci(d, "ItemId")));
            input.add(o);
        }
        List<Map<String, Object>> overhead = new ArrayList<>();
        for (Map<String, Object> d : repo.bomRead("ReadByHeaderId_BomFohDetail", bomId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", 0);
            o.put("BomHeaderId", toInt(ci(d, "BomHeaderId")));
            o.put("BomNo", str(ci(d, "BomHeaderNo")));
            o.put("BomOverHeadId", toInt(ci(d, "Id")));
            o.put("OverHeadTypeId", toInt(ci(d, "OverHeadTypeId")));
            o.put("OverHeadAccountId", toInt(ci(d, "GLAccountId")));
            o.put("OverHeadAccountTitle", str(ci(d, "OverHeadAccountTitle")));
            o.put("PercentOfTotalInput", toDouble(ci(d, "PercentOfTotalInput")));
            o.put("BomQty", toDouble(ci(d, "Qty")));                                                      // intended mapping, see class note 2
            o.put("Qty", toDouble(ci(d, "Qty")));
            o.put("Rate", toDouble(ci(d, "Rate")));
            o.put("Amount", toDouble(ci(d, "Amount")));
            o.put("OhRemarks", str(ci(d, "OhRemarks")));
            overhead.add(o);
        }
        List<Map<String, Object>> wages = new ArrayList<>();
        for (Map<String, Object> d : repo.bomWagesWithUpdatedRates(bomId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", 0);
            o.put("BomHeaderId", toInt(ci(d, "BomHeaderId")));
            o.put("BomNo", str(ci(d, "BomHeaderNo")));
            o.put("BomWagesId", toInt(ci(d, "Id")));
            o.put("OutPutItemId", outItemId);
            o.put("OutPutItemName", outItemName == null ? "" : outItemName);
            o.put("ContractorId", 0);
            o.put("ContractorName", "");
            o.put("ContractorWagesRateScheduleId", toInt(ci(d, "ContractorWagesRateScheduleId")));
            o.put("ServiceActivity", str(ci(d, "ServiceActivity")));
            o.put("BomQty", toDouble(ci(d, "ItemQty")));
            o.put("Qty", toDouble(ci(d, "ItemQty")));
            o.put("Rate", toDouble(ci(d, "Rate")));
            o.put("Amount", toDouble(ci(d, "Amount")));
            o.put("Remarks", str(ci(d, "Remarks")));
            wages.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("input", input);
        out.put("overhead", overhead);
        out.put("wages", wages);
        return out;
    }

    // ====================================================================================== stock / avg rate

    /**
     * UpdateInputCurrentStockAndAvgRate:5880 for one input row: GetStockInHandAndAvgRateFromEvaluationConcrete -> QtyInHand (BalanceStock); rate is the stock's AvgRate, or
     * with ERP feature 5 on, GetAvgRateFromFIFOMethodConcrete (CommonServices) over [pcc].[USP_GetStockByFifoMethod] (JobLotId, DocumentTypeId 1850 + Id when editing).
     */
    public Map<String, Object> stock(int itemId, int warehouseId, int jobLotId, double itemQty, double netWeight, String docDate, int recId) {
        UserAccount u = db.user();
        Timestamp date = pickerValue(docDate);
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> rows = repo.stockInHand(u, itemId, date, warehouseId);
        boolean fifo = inRepo.erpFeature(u, ERP_FEATURE_FIFO_CGS);
        out.put("found", !rows.isEmpty());
        out.put("fifo", fifo);
        out.put("balanceStock", rows.isEmpty() ? 0d : toDouble(ci(rows.get(0), "QtyInHand")));
        double rate = 0d;
        if (!rows.isEmpty()) rate = fifo ? fifoAvgRate(u, itemId, date, warehouseId, jobLotId, itemQty, netWeight, recId) : toDouble(ci(rows.get(0), "AvgRate"));
        out.put("rate", rate);
        return out;
    }

    /** CommonServices.GetAvgRateFromFIFOMethodConcrete (WinApp.Common/CommonServices.cs). */
    private double fifoAvgRate(UserAccount u, int itemId, Timestamp date, int warehouseId, int jobLotId, double itemQty, double netWeight, int recId) {
        List<Map<String, Object>> layers = repo.fifoLayers(u, itemId, date, warehouseId, jobLotId, recId > 0 ? DOC_TYPE : 0, recId, null);
        if (layers.isEmpty()) return 0d;
        double availableWeight = 0, availableQtyTotal = 0;
        for (Map<String, Object> l : layers) {
            availableWeight += toDouble(ci(l, "BalWeight"));
            availableQtyTotal += toDouble(ci(l, "BalQty"));
        }
        if (!(itemQty <= availableQtyTotal && netWeight <= availableWeight)) return 0d;
        double consumedQty = 0, consumed = 0, totalCgs = 0;
        for (Map<String, Object> l : layers) {
            double available = toDouble(ci(l, "BalWeight"));
            double availableQty = toDouble(ci(l, "BalQty"));
            double rate = toDouble(ci(l, "AvgRate"));
            if (availableQty <= itemQty - consumedQty) {
                consumed += available;
                consumedQty += availableQty;
                totalCgs += availableQty * rate;
            } else if (availableQty >= itemQty - consumedQty) {
                double outQty = itemQty - consumedQty;
                double outWeight = netWeight - consumed;
                totalCgs += (itemQty - consumedQty) * rate;
                consumedQty += outQty;
                consumed += outWeight;
            }
            if (itemQty == consumedQty) break;
        }
        return (totalCgs > 0 && itemQty > 0) ? totalCgs / itemQty : 0d;
    }

    // ====================================================================================== history / read / delete

    /** HistoryGridFill:4328 - [pcc].[USP_ProductionHeader_GetAllMethod] 'FormHistory'. referred: 1 = Referred, 2 = Not Referred, 0 = All. */
    public List<Map<String, Object>> history(String fromDate, String toDate, int fromDocNo, int toDocNo, int productionId, int referred) {
        UserAccount u = db.user();
        boolean viewAll = db.has(SCREEN, "viewAll");
        List<Map<String, Object>> rows = repo.formHistory(u, db.branchId(u), DOC_TYPE, db.financialYearId(), ts(fromDate), ts(toDate), fromDocNo, toDocNo,
                productionId, viewAll, u.getId() == null ? 0 : u.getId(), referred);
        return json(rows);
    }

    /** ReadById:3843 - header + the four detail lists, shaped as the grids' rows (Fill*FromListCommonForReadById:4049-4118). */
    public Map<String, Object> read(int id) {
        List<Map<String, Object>> h = repo.read("ReadById", id);
        if (h.isEmpty()) throw new IllegalArgumentException("No Record Found");
        Map<String, Object> r = h.get(0);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", toInt(ci(r, "Id")));
        out.put("docDate", day(ci(r, "DocDate")));
        out.put("docNo", toInt(ci(r, "DocNo")));
        out.put("workOrderId", toInt(ci(r, "WorkOrderId")));
        out.put("productionTypeId", toInt(ci(r, "ProductionTypeId")));
        out.put("plantId", toInt(ci(r, "PlantId")));
        out.put("productionNo", str(ci(r, "ProductionNo")));
        out.put("remarksHeader", str(ci(r, "RemarksHeader")));
        out.put("itemId", toInt(ci(r, "ItemId")));
        out.put("bomHeaderId", toInt(ci(r, "BomHeaderId")));
        out.put("itemUomId", toInt(ci(r, "ItemUomId")));
        out.put("itemQty", toDouble(ci(r, "ItemQty")));
        out.put("wipWarehouseId", toInt(ci(r, "WIPWarehouseId")));
        out.put("wipAccountId", toInt(ci(r, "WIPGLAccountID")));
        out.put("wagesExpenseAccountId", toInt(ci(r, "WagesExpenseAccountId")));
        out.put("isApproved", Prod273Support.truthy(ci(r, "IsApproved")));

        List<Map<String, Object>> input = new ArrayList<>();
        for (Map<String, Object> d : repo.read("ReadByHeaderId_ProductionInputDetail", id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(d, "Id")));
            o.put("BomHeaderId", toInt(ci(d, "BomHeaderId")));
            o.put("BomNo", str(ci(d, "BomHeaderNo")));
            o.put("BomInputId", toInt(ci(d, "BomInputId")));
            o.put("WarehouseId", toInt(ci(d, "WareHouseId")));
            o.put("WarehouseName", str(ci(d, "WareHouseName")));
            o.put("JobLotId", toInt(ci(d, "JobLotId")));
            o.put("JobLot", str(ci(d, "JobLotDescription")));
            o.put("ItemId", toInt(ci(d, "ItemId")));
            o.put("ItemName", str(ci(d, "ItemName")));
            o.put("ItemUomId", toInt(ci(d, "ItemUomId")));
            o.put("Uom", toDouble(ci(d, "ItemUomEquivalent")));
            o.put("UomCode", str(ci(d, "ItemUomCode")));
            o.put("BalanceStock", toDouble(ci(d, "ItemQty")));                                           // :4079 puts ItemQty in the BalanceStock column
            o.put("BomQty", toDouble(ci(d, "BomQty")));
            o.put("ItemQty", toDouble(ci(d, "ItemQty")));
            o.put("NetWeight", toDouble(ci(d, "NetWeight")));
            o.put("BomRate", toDouble(ci(d, "BomRate")));
            o.put("Rate", toDouble(ci(d, "Rate")));
            o.put("RateUomId", toInt(ci(d, "RateUomId")));
            o.put("RateUom", toDouble(ci(d, "RateUomEquivalent")));
            o.put("RateUomCode", str(ci(d, "RateUomCode")));
            o.put("Amount", toDouble(ci(d, "Amount")));
            o.put("BomMinQty", toDouble(ci(d, "BomMinQty")));
            o.put("BomMaxQty", toDouble(ci(d, "BomMaxQty")));
            o.put("MinQty", toDouble(ci(d, "MinQty")));
            o.put("MaxQty", toDouble(ci(d, "MaxQty")));
            o.put("MainItemId", toInt(ci(d, "BomMainItemId")));
            input.add(o);
        }
        out.put("input", input);

        List<Map<String, Object>> output = new ArrayList<>();
        for (Map<String, Object> d : repo.read("ReadByHeaderId_ProductionOutPutDetail", id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(d, "Id")));
            o.put("BomHeaderId", toInt(ci(d, "BOMHeaderId")));
            o.put("BomNo", str(ci(d, "BOMHeaderNo")));
            o.put("WorkOrderId", toInt(ci(d, "WorkOrderId")));
            o.put("WorkOrderDetailId", toInt(ci(d, "WorkOrderDetailId")));
            o.put("WorkOrderNo", str(ci(d, "WorkOrderNo")));
            o.put("EntryTypeId", toInt(ci(d, "EntryTypeId")));
            o.put("EntryType", str(ci(d, "EntryType")));
            o.put("CustomerTypeId", toInt(ci(d, "CustomerTypeId")));
            o.put("CustomerType", str(ci(d, "CustomerType")));
            o.put("CustomerId", toInt(ci(d, "CustomerId")));
            o.put("CustomerName", str(ci(d, "CustomerName")));
            o.put("ReferenceNo", str(ci(d, "ReferenceNo")));
            o.put("SaleOrderId", toInt(ci(d, "SaleOrderId")));
            o.put("SaleOrderDetailId", toInt(ci(d, "SaleOrderDetailId")));
            o.put("SaleOrder", toInt(ci(d, "SaleOrderNo")));
            o.put("WarehouseId", toInt(ci(d, "WareHouseId")));
            o.put("WarehouseName", str(ci(d, "WareHouseName")));
            o.put("JobLotId", toInt(ci(d, "JobLotId")));
            o.put("JobLot", str(ci(d, "JobLotDescription")));
            o.put("ItemId", toInt(ci(d, "ItemId")));
            o.put("ItemName", str(ci(d, "ItemName")));
            o.put("ItemAttributeVarientId", toInt(ci(d, "ItemAttributeVarientId")));
            o.put("AttributeVarient", str(ci(d, "ItemAttribute")));
            o.put("VarientEquivalent", toDouble(ci(d, "VarientEquivalent")));
            o.put("ItemUomId", toInt(ci(d, "ItemUomId")));
            o.put("Uom", toDouble(ci(d, "ItemUomEquivalent")));
            o.put("UomCode", str(ci(d, "ItemUomCode")));
            o.put("ItemQty", toDouble(ci(d, "ItemQty")));
            o.put("VarientQty", toDouble(ci(d, "VarientQty")));
            o.put("NetWeight", toDouble(ci(d, "NetWeight")));
            o.put("Rate", toDouble(ci(d, "Rate")));
            o.put("RateUomId", toInt(ci(d, "RateUomId")));
            o.put("RateUom", toDouble(ci(d, "RateUomEquivalent")));
            o.put("RateUomCode", str(ci(d, "RateUomCode")));
            o.put("Amount", toDouble(ci(d, "Amount")));
            output.add(o);
        }
        out.put("output", output);

        List<Map<String, Object>> overhead = new ArrayList<>();
        for (Map<String, Object> d : repo.read("ReadByHeaderId_ProductionOverHeadsDetail", id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(d, "Id")));
            o.put("BomHeaderId", toInt(ci(d, "BomHeaderId")));
            o.put("BomNo", str(ci(d, "BomHeaderNo")));
            o.put("BomOverHeadId", toInt(ci(d, "BomOverHeadId")));
            o.put("OverHeadTypeId", toInt(ci(d, "OverHeadTypeId")));
            o.put("OverHeadAccountId", toInt(ci(d, "OverHeadAccountId")));
            o.put("OverHeadAccountTitle", str(ci(d, "OverHeadAccountTitle")));
            o.put("PercentOfTotalInput", toDouble(ci(d, "PercentOfTotalInput")));
            o.put("BomQty", toDouble(ci(d, "BomQty")));
            o.put("Qty", toDouble(ci(d, "Qty")));
            o.put("Rate", toDouble(ci(d, "Rate")));
            o.put("Amount", toDouble(ci(d, "Amount")));
            o.put("OhRemarks", str(ci(d, "OhRemarks")));
            overhead.add(o);
        }
        out.put("overhead", overhead);

        List<Map<String, Object>> wages = new ArrayList<>();
        for (Map<String, Object> d : repo.read("ReadByHeaderId_ProductionWagesDetail", id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(d, "Id")));
            o.put("BomHeaderId", toInt(ci(d, "BomHeaderId")));
            o.put("BomNo", str(ci(d, "BomHeaderNo")));
            o.put("BomWagesId", toInt(ci(d, "BomWagesId")));
            o.put("OutPutItemId", toInt(ci(d, "OutPutItemId")));
            o.put("OutPutItemName", str(ci(d, "OutPutItemName")));
            o.put("ContractorId", toInt(ci(d, "ContractorId")));
            o.put("ContractorName", str(ci(d, "ContractorName")));
            o.put("ContractorWagesRateScheduleId", toInt(ci(d, "ContractorWagesRateScheduleId")));
            o.put("ServiceActivity", str(ci(d, "ServiceActivity")));
            o.put("BomQty", toDouble(ci(d, "BomQty")));
            o.put("Qty", toDouble(ci(d, "Qty")));
            o.put("Rate", toDouble(ci(d, "Rate")));
            o.put("Amount", toDouble(ci(d, "Amount")));
            o.put("Remarks", str(ci(d, "Remarks")));
            wages.add(o);
        }
        out.put("wages", wages);
        return out;
    }

    /** btnDelete_Click:3946 - ProductionHeader.DeleteByID(UserAccount.ID, RecId); the procedure refuses approved / released / FIFO-referred documents itself. */
    public Map<String, Object> delete(int id) {
        right("delete", "You do not have the Delete right on Concrete Production.");
        if (id <= 0) throw new IllegalArgumentException("No record found to Delete");
        UserAccount u = db.user();
        repo.deleteById(u.getId() == null ? 0 : u.getId(), id);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", "Delete Record Seccessfully");
        return m;
    }

    /** VoucherReprot:4168 - voucher head of the production (DocumentTypeId 1850). */
    public Map<String, Object> voucherHead(int id) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("voucherHeadId", inRepo.voucherHeadId(db.user(), DOC_TYPE, id));
        return m;
    }

    // ====================================================================================== save (Insert:3508 + BLL Save + DAL SetData)

    private static final class Ctx {
        UserAccount u;
        int org, company, branch, fy, userId;
        int headerId;
        int docNo;
        int workOrderId;
        int wipAccountId, wagesExpenseAccountId, headerItemId;
        boolean wagesDebitExpense;
        Timestamp docDate;
        boolean update;
        boolean fifo;
        List<Map<String, Object>> input = new ArrayList<>();
        List<Map<String, Object>> output = new ArrayList<>();
        List<Map<String, Object>> overhead = new ArrayList<>();
        List<Map<String, Object>> wages = new ArrayList<>();
        Map<String, Object> voucher;
        List<Map<String, Object>> voucherLines = new ArrayList<>();
    }

    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = db.user();
        int recId = toInt(body.get("id"));
        boolean update = recId > 0;
        right(update ? "update" : "save", update ? "You do not have the Update right on Concrete Production." : "You do not have the Save right on Concrete Production.");
        boolean wagesDebit = wagesDebitConfig(u);

        /* FormValidation:1789 */
        String docNoText = str(body.get("docNo")).trim();
        need(!(docNoText.isEmpty() || docNoText.equals("0")), "Doc No Field Required");
        int productionTypeId = toInt(body.get("productionTypeId"));
        need(productionTypeId != 0, "Production Type Feild Required");
        need(!str(body.get("productionNo")).trim().isEmpty(), "Production No Field Required");
        need(toInt(body.get("plantId")) != 0, "Plant Name Feild Required");
        need(toInt(body.get("wipWarehouseId")) != 0, "WIPWarehouse Field Required");
        need(toInt(body.get("wipAccountId")) != 0, "WIPAccount Field Required");
        if (wagesDebit) need(toInt(body.get("wagesExpenseAccountId")) != 0, "Wages Expense Account Field Required");

        List<Map<String, Object>> inRows = list(body.get("input"));
        List<Map<String, Object>> outRows = list(body.get("output"));
        List<Map<String, Object>> ohRows = list(body.get("overhead"));
        List<Map<String, Object>> wgRows = list(body.get("wages"));
        need(!(inRows.isEmpty() || outRows.isEmpty()), "Grid Record Not Found...\nGrid (Input and OutPut) Should have atleast one record");   // Insert:3516

        validateItemTypeOfType(u, outRows, productionTypeId);                                              // :3527 -> :5783
        isDuplicateRowExists(outRows);                                                                    // :3528 -> :3435
        checkBomHeaderIds(outRows, inRows, "Output", "Input", 0, true);                                   // :3529 -> :3468
        checkBomHeaderIds(inRows, ohRows, "InPut", "Over Head", 1, false);                                // :3530
        checkBomHeaderIds(inRows, wgRows, "InPut", "Wages", 1, false);                                    // :3531

        /* btnGenerateRatesOutput_Click:5837 -> UpdateRatesOutput:6126 then the header net check :3533 */
        updateRatesOutput(outRows, inRows, ohRows, wgRows, wagesDebit);
        double inputTotal = 0, ohTotal = 0, wgTotal = 0, manual = 0, outputTotal = 0;
        for (Map<String, Object> r : inRows) inputTotal += toDouble(r.get("Amount"));
        for (Map<String, Object> r : ohRows) ohTotal += toDouble(r.get("Amount"));
        for (Map<String, Object> r : wgRows) wgTotal += toDouble(r.get("Amount"));
        for (Map<String, Object> r : outRows) {
            outputTotal += toDouble(r.get("Amount"));
            if (toInt(r.get("EntryTypeId")) != 1) manual += toDouble(r.get("Amount"));
        }
        double netValue = inputTotal + ohTotal + wgTotal - manual;                                         // Calculation:3407
        double compare = wagesDebit ? netValue - wgTotal : netValue;
        if (Math.abs(round(compare, 2) - round(outputTotal, 2)) > 0.0051) {
            throw new IllegalArgumentException("Net Vale " + net(round(compare, 2)) + " In Header and Total OutPut Amount " + net(round(outputTotal, 2)) + " are not equal");
        }

        Ctx c = new Ctx();
        c.u = u;
        c.org = u.getOrganizationId();
        c.company = u.getCompanyId();
        c.branch = db.branchId(u);
        c.fy = db.financialYearId();
        c.userId = u.getId() == null ? 0 : u.getId();
        c.headerId = recId;
        c.update = update;
        c.docNo = toInt(docNoText);
        c.docDate = pickerValue(body.get("docDate"));
        c.fifo = inRepo.erpFeature(u, ERP_FEATURE_FIFO_CGS);
        c.workOrderId = toInt(body.get("workOrderId"));
        c.wipAccountId = toInt(body.get("wipAccountId"));
        c.wagesExpenseAccountId = toInt(body.get("wagesExpenseAccountId"));
        c.wagesDebitExpense = wagesDebit;

        /* header item / uom / bom / qty = grdOutPutByProduct.CurrentRow (:3556); the page sends the selected row index */
        int cur = toInt(body.get("currentOutputIndex"));
        if (cur < 0 || cur >= outRows.size()) cur = 0;
        Map<String, Object> curRow = outRows.get(cur);
        c.headerItemId = toInt(curRow.get("ItemId"));

        boolean workOrderDetailExists = false;
        for (Map<String, Object> r : outRows) if (toInt(r.get("WorkOrderDetailId")) > 0) { workOrderDetailExists = true; break; }
        if (!workOrderDetailExists) c.workOrderId = 0;                                                    // :3639

        buildInput(c, inRows, list(body.get("removedInput")));
        buildOutput(c, outRows, list(body.get("removedOutput")));
        buildOverhead(c, ohRows);
        buildWages(c, wgRows);

        /* BLL Save:177 - the voucher is made before the user ids are adjusted */
        c.voucher = makeVoucher(c);

        Map<String, Object> header = new LinkedHashMap<>();
        header.put("Id", recId);
        header.put("DocumentTypeId", DOC_TYPE);
        header.put("DocNo", c.docNo);
        header.put("DocDate", c.docDate);
        header.put("WorkOrderId", c.workOrderId);
        header.put("BomHeaderId", toInt(curRow.get("BomHeaderId")));
        header.put("ItemId", toInt(curRow.get("ItemId")));
        header.put("ItemUomId", toInt(curRow.get("ItemUomId")));
        header.put("ItemQty", toDouble(curRow.get("ItemQty")));
        header.put("ProductionNo", str(body.get("productionNo")));
        header.put("RemarksHeader", str(body.get("remarksHeader")));
        header.put("OrganizationId", c.org);
        header.put("CompanyId", c.company);
        header.put("BranchesId", c.branch);
        header.put("ProjectsId", c.branch);
        header.put("FinancialYearId", c.fy);
        header.put("EntryUserId", update ? 0 : c.userId);
        header.put("EntryDate", Prod273Support.now());
        header.put("ModifyUserId", update ? c.userId : 0);
        header.put("ModifyDate", Prod273Support.now());
        header.put("IsApproved", false);
        header.put("ApprovedUserId", c.userId);
        header.put("ApprovedDate", Prod273Support.now());
        header.put("ProductionTypeId", productionTypeId);
        header.put("ActionId", update ? 2 : 1);
        header.put("WIPWarehouseId", toInt(body.get("wipWarehouseId")));
        header.put("WIPGLAccountID", c.wipAccountId);
        header.put("PlantId", toInt(body.get("plantId")));
        header.put("WagesExpenseAccountId", c.wagesExpenseAccountId);

        Integer saved = new TransactionTemplate(tm).execute(status -> setData(c, header));
        int id = saved == null ? 0 : saved;

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("docNo", c.docNo);
        out.put("message", (update ? "Data Update Successfully....  " : "Data Save Successfully....  ") + c.docNo);
        return out;
    }

    /** ValidateItemTypeOfType:5783 - every output item must have ItemTypeOfTypeId = the production type (item list = USP_Item_AllItemsWithModal). */
    private void validateItemTypeOfType(UserAccount u, List<Map<String, Object>> outRows, int productionTypeId) {
        Set<Integer> valid = new HashSet<>();
        for (Map<String, Object> i : repo.allItems(u)) {
            if (toInt(ci(i, "Id")) != 0 && toInt(ci(i, "ItemTypeOfTypeId")) == productionTypeId) valid.add(toInt(ci(i, "Id")));
        }
        String typeName = "";
        for (Map<String, Object> t : repo.productionTypes(u)) if (toInt(ci(t, "Id")) == productionTypeId) typeName = str(ci(t, "LookupName"));
        for (Map<String, Object> r : outRows) {
            int itemId = toInt(r.get("ItemId"));
            if (itemId > 0 && !valid.contains(itemId)) {
                throw new IllegalArgumentException("Item '" + str(r.get("ItemName")) + "' in Output Grid is not valid for Type '" + typeName + "' in ItemList.");
            }
        }
    }

    /** IsDuplicateRowExists:3435 - key bom-item-variant-warehouse-customer-referenceNo over every output row. */
    private void isDuplicateRowExists(List<Map<String, Object>> outRows) {
        Map<String, Integer> seen = new HashMap<>();
        for (int i = 0; i < outRows.size(); i++) {
            Map<String, Object> r = outRows.get(i);
            String key = toInt(r.get("BomHeaderId")) + "-" + toInt(r.get("ItemId")) + "-" + toInt(r.get("ItemAttributeVarientId")) + "-" + toInt(r.get("WarehouseId"))
                    + "-" + toInt(r.get("CustomerId")) + "-" + str(r.get("ReferenceNo"));
            Integer prev = seen.get(key);
            if (prev != null) {
                throw new IllegalArgumentException("Duplicate Record not added to the grid.\nItem " + str(r.get("ItemName")) + " with BOM No " + str(r.get("BomNo"))
                        + ", Variant " + str(r.get("AttributeVarient")) + ", Warehouse " + str(r.get("WarehouseName")) + ", Customer " + str(r.get("CustomerName"))
                        + ", Reference No " + str(r.get("ReferenceNo")) + " exists in row No " + (i + 1) + " and Row No " + (prev + 1));
            }
            seen.put(key, i);
        }
    }

    /**
     * CheckBomHeaderIds(dtOutput, dtInput, Target, Source, CheckSpecific):3468. The first table is the "target" grid, the second the "source". Source ids missing in the
     * target throw "BomHeaderNo ... found in Source Grid but not in Target Grid." (names from the source rows, distinct); when CheckSpecific is 0 the reverse check follows
     * (names from the target rows, not distinct).
     */
    private void checkBomHeaderIds(List<Map<String, Object>> target, List<Map<String, Object>> source, String targetName, String sourceName, int checkSpecific, boolean both) {
        Set<Integer> targetIds = new LinkedHashSet<>();
        for (Map<String, Object> r : target) if (toInt(r.get("BomHeaderId")) > 0) targetIds.add(toInt(r.get("BomHeaderId")));
        Set<Integer> sourceIds = new LinkedHashSet<>();
        for (Map<String, Object> r : source) if (toInt(r.get("BomHeaderId")) > 0) sourceIds.add(toInt(r.get("BomHeaderId")));
        Set<Integer> missingInTarget = new LinkedHashSet<>(sourceIds);
        missingInTarget.removeAll(targetIds);
        if (!missingInTarget.isEmpty()) {
            Set<String> names = new LinkedHashSet<>();
            for (Map<String, Object> r : source) if (missingInTarget.contains(toInt(r.get("BomHeaderId")))) names.add(str(r.get("BomNo")));
            throw new IllegalArgumentException("BomHeaderNo " + String.join(", ", names) + " found in " + sourceName + " Grid but not in " + targetName + " Grid.");
        }
        if (checkSpecific != 0 && checkSpecific != 2) return;
        Set<Integer> missingInSource = new LinkedHashSet<>(targetIds);
        missingInSource.removeAll(sourceIds);
        if (!missingInSource.isEmpty()) {
            List<String> names = new ArrayList<>();
            for (Map<String, Object> r : target) if (missingInSource.contains(toInt(r.get("BomHeaderId")))) names.add(str(r.get("BomNo")));
            throw new IllegalArgumentException("BomHeaderNo " + String.join(", ", names) + " found in " + targetName + " Grid but not in " + sourceName + " Grid.");
        }
    }

    /**
     * UpdateRatesOutput:6126 - per BOM: net = input amount + overhead amount (+ wages amount when the wages are not debited to the expense account); every auto-rate
     * output row of that BOM gets Amount = net / total VarientQty * row VarientQty and Rate = Amount / VarientQty.
     */
    private void updateRatesOutput(List<Map<String, Object>> outRows, List<Map<String, Object>> inRows, List<Map<String, Object>> ohRows,
                                   List<Map<String, Object>> wgRows, boolean wagesDebit) {
        if (outRows.size() == 1 && toInt(outRows.get(0).get("BomHeaderId")) > 0 && toInt(outRows.get(0).get("ItemId")) == 0) {
            throw new IllegalArgumentException("Output Grid Record not Filled Completely");
        }
        Map<Integer, Double> inputByBom = new LinkedHashMap<>();
        for (Map<String, Object> r : inRows) inputByBom.merge(toInt(r.get("BomHeaderId")), toDouble(r.get("Amount")), Double::sum);
        for (Map.Entry<Integer, Double> e : inputByBom.entrySet()) {
            int bomId = e.getKey();
            double oh = 0, wg = 0;
            for (Map<String, Object> r : ohRows) if (toInt(r.get("BomHeaderId")) == bomId) oh += toDouble(r.get("Amount"));
            for (Map<String, Object> r : wgRows) if (toInt(r.get("BomHeaderId")) == bomId) wg += toDouble(r.get("Amount"));
            double netValue = wagesDebit ? e.getValue() + oh : e.getValue() + oh + wg;
            List<Map<String, Object>> rows = new ArrayList<>();
            double totalVarientQty = 0;
            for (Map<String, Object> r : outRows) {
                if (toInt(r.get("BomHeaderId")) == bomId && toInt(r.get("EntryTypeId")) == 1) { rows.add(r); totalVarientQty += toDouble(r.get("VarientQty")); }
            }
            for (Map<String, Object> r : rows) {
                double q = toDouble(r.get("VarientQty"));
                double amount = totalVarientQty > 0 ? netValue / totalVarientQty * q : 0;
                r.put("Amount", amount);
                r.put("Rate", q > 0 ? amount / q : 0);
            }
        }
    }

    // -------------------------------------------------------------------------------- row building with the desktop's validation order

    /** FillInPutDetailListCommonForInsertAndDelete:4008 + the ValidateField calls of Insert:3580. Removed rows are added first (:3573). */
    private void buildInput(Ctx c, List<Map<String, Object>> rows, List<Map<String, Object>> removed) {
        if (c.update) {
            for (Map<String, Object> r : removed) c.input.add(inputDetail(c, r, 0, true));
        }
        Map<Integer, List<Map<String, Object>>> uomCache = new HashMap<>();
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            int n = i + 1;
            if (toInt(r.get("ItemId")) == 0) continue;                                                     // :3583
            Map<String, Object> d = inputDetail(c, r, n, false);
            int id = c.update ? toInt(r.get("Id")) : 0;
            d.put("Id", id);
            d.put("ActionTypeId", id <= 0 ? 1 : 2);
            int itemId = toInt(r.get("ItemId"));
            List<Map<String, Object>> uoms = uomCache.computeIfAbsent(itemId, k -> db.uomsByItem(c.u, k));
            String uomMsg = null, rateUomMsg = null;
            int itemUomId = 0, rateUomId = 0;
            if (!uoms.isEmpty()) {
                for (Map<String, Object> m : uoms) if (toInt(ci(m, "Id")) == toInt(r.get("ItemUomId"))) { itemUomId = toInt(ci(m, "Id")); break; }
                if (itemUomId == 0) uomMsg = "UOM in Input Grid Row No: " + n + " Is not Define Against Item " + itemId + ". Please Check...";
                for (Map<String, Object> m : uoms) if (toInt(ci(m, "Id")) == toInt(r.get("RateUomId"))) { rateUomId = toInt(ci(m, "Id")); break; }
                if (rateUomId == 0) rateUomMsg = "RateUom in Input Grid Row No: " + n + " Is not Define Against Item " + itemId + ". Please Check...";
            }
            d.put("ItemUomId", itemUomId);
            d.put("RateUomId", rateUomId);
            fieldInt(toInt(d.get("WareHouseId")), "Warehouse", n);
            fieldInt(toInt(d.get("JobLotId")), "JobLotId", n);
            fieldInt(toInt(d.get("ItemId")), "Item", n);
            if (itemUomId == 0) { if (uomMsg != null) throw new IllegalArgumentException(uomMsg); fieldInt(0, "ItemUom", n); }
            fieldDec(toDouble(d.get("ItemQty")), "Qty", n);
            fieldDec(toDouble(d.get("Rate")), "Rate", n);
            if (rateUomId == 0) { if (rateUomMsg != null) throw new IllegalArgumentException(rateUomMsg); fieldInt(0, "RateUom", n); }
            fieldDec(toDouble(d.get("Amount")), "Amount", n);
            d.put("_ItemName", str(r.get("ItemName")));
            c.input.add(d);
        }
    }

    private Map<String, Object> inputDetail(Ctx c, Map<String, Object> r, int n, boolean removed) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("Id", toInt(r.get("Id")));
        d.put("BomInputId", toInt(r.get("BomInputId")));
        d.put("BomHeaderId", toInt(r.get("BomHeaderId")));
        d.put("WareHouseId", toInt(r.get("WarehouseId")));
        d.put("JobLotId", toInt(r.get("JobLotId")));
        d.put("ItemId", toInt(r.get("ItemId")));
        d.put("ItemUomId", toInt(r.get("ItemUomId")));
        d.put("ItemQty", toDouble(r.get("ItemQty")));
        d.put("MinQty", toDouble(r.get("MinQty")));
        d.put("MaxQty", toDouble(r.get("MaxQty")));
        d.put("NetWeight", toDouble(r.get("NetWeight")));
        d.put("Rate", toDouble(r.get("Rate")));
        d.put("RateUomId", toInt(r.get("RateUomId")));
        d.put("Amount", toDouble(r.get("Amount")));
        d.put("Remarks", "");
        if (removed) d.put("ActionTypeId", 3);
        return d;
    }

    /** FillOutPutDetailListCommonForInsertAndDelete:3968 + the ValidateField calls of Insert:3597. */
    private void buildOutput(Ctx c, List<Map<String, Object>> rows, List<Map<String, Object>> removed) {
        if (c.update) {
            for (Map<String, Object> r : removed) {
                Map<String, Object> d = outputDetail(c, r);
                d.put("ActionTypeId", 3);
                c.output.add(d);
            }
        }
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            int n = i + 1;
            need(toInt(r.get("ItemId")) != 0, "Item Field Required in Grid output Row No: " + n);        // :3626
            Map<String, Object> d = outputDetail(c, r);
            int id = c.update ? toInt(r.get("Id")) : 0;
            d.put("Id", id);
            d.put("ActionTypeId", id <= 0 ? 1 : 2);
            fieldInt(toInt(d.get("BOMHeaderId")), "Bom No", n);
            if (toInt(r.get("WorkOrderId")) > 0) fieldStr(str(d.get("CustomerName")), "Customer", n);
            fieldInt(toInt(d.get("WareHouseId")), "Warehouse", n);
            fieldInt(toInt(d.get("JobLotId")), "JobLotId", n);
            fieldInt(toInt(d.get("ItemId")), "Item", n);
            fieldInt(toInt(d.get("ItemAttributeVarientId")), "Item Attribute Varient", n);
            fieldDec(toDouble(d.get("ItemQty")), "Qty", n);
            fieldDec(toDouble(d.get("VarientQty")), "VarientQty", n);
            fieldDec(toDouble(d.get("Rate")), "Rate", n);
            fieldDec(toDouble(d.get("Amount")), "Amount", n);
            c.output.add(d);
        }
    }

    private Map<String, Object> outputDetail(Ctx c, Map<String, Object> r) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("Id", toInt(r.get("Id")));
        d.put("BOMHeaderId", toInt(r.get("BomHeaderId")));
        d.put("WorkOrderDetailId", toInt(r.get("WorkOrderDetailId")));
        d.put("EntryTypeId", 1);                                                                           // :3974
        d.put("CustomerTypeId", toInt(r.get("CustomerTypeId")));
        d.put("CustomerId", toInt(r.get("CustomerId")));
        d.put("CustomerName", str(r.get("CustomerName")));
        d.put("ReferenceNo", str(r.get("ReferenceNo")));
        d.put("SaleOrderId", toInt(r.get("SaleOrderId")));
        d.put("SaleOrderDetailId", toInt(r.get("SaleOrderDetailId")));
        d.put("WareHouseId", toInt(r.get("WarehouseId")));
        d.put("JobLotId", toInt(r.get("JobLotId")));
        d.put("ItemId", toInt(r.get("ItemId")));
        d.put("ItemAttributeVarientId", toInt(r.get("ItemAttributeVarientId")));
        d.put("VarientEquivalent", toDouble(r.get("VarientEquivalent")));
        d.put("ItemUomId", toInt(r.get("ItemUomId")));
        d.put("ItemQty", toDouble(r.get("ItemQty")));
        d.put("VarientQty", toDouble(r.get("VarientQty")));
        d.put("NetWeight", toDouble(r.get("NetWeight")));
        d.put("Rate", toDouble(r.get("Rate")));
        d.put("RateUomId", toInt(r.get("RateUomId")));
        d.put("Amount", toDouble(r.get("Amount")));
        d.put("Remarks", "");
        return d;
    }

    /** Insert:3647 - rows with OverHeadAccountId > 0. */
    private void buildOverhead(Ctx c, List<Map<String, Object>> rows) {
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            if (toInt(r.get("OverHeadAccountId")) <= 0) continue;
            int n = i + 1;
            need(!(empty(r.get("Amount")) || toDouble(r.get("Amount")) == 0), "Amount Field Required in Grid Over Head Row No: " + n);
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", toInt(r.get("Id")));
            d.put("BomOverHeadId", toInt(r.get("BomOverHeadId")));
            d.put("OverHeadTypeId", toInt(r.get("OverHeadAccountId")));
            d.put("OverHeadAccountId", toInt(r.get("OverHeadAccountId")));
            d.put("PercentOfTotalInput", toDouble(r.get("PercentOfTotalInput")));
            d.put("Qty", toDouble(r.get("Qty")));
            d.put("BomQty", toDouble(r.get("BomQty")));
            d.put("Rate", toDouble(r.get("Rate")));
            d.put("Amount", toDouble(r.get("Amount")));
            d.put("OhRemarks", str(r.get("OhRemarks")));
            d.put("BomHeaderId", toInt(r.get("BomHeaderId")));
            c.overhead.add(d);
        }
    }

    /** Insert:3667 - rows with ContractorWagesRateScheduleId > 0; ItemId = OutPutItemId. */
    private void buildWages(Ctx c, List<Map<String, Object>> rows) {
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            if (toInt(r.get("ContractorWagesRateScheduleId")) <= 0) continue;
            int n = i + 1;
            need(!(empty(r.get("ContractorId")) || toInt(r.get("ContractorId")) == 0), "Contractor  Field Required in Grid Contractor Wages Row No: " + n);
            need(!(empty(r.get("Amount")) || toDouble(r.get("Amount")) == 0), "Amount Field Required in Grid Contractor Wages Row No: " + n);
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", toInt(r.get("Id")));
            d.put("BomWagesId", toInt(r.get("BomWagesId")));
            d.put("ContractorWagesRateScheduleId", toInt(r.get("ContractorWagesRateScheduleId")));
            d.put("ItemId", toInt(r.get("OutPutItemId")));
            d.put("ContractorId", toInt(r.get("ContractorId")));
            d.put("Qty", toDouble(r.get("Qty")));
            d.put("BomQty", toDouble(r.get("BomQty")));
            d.put("Rate", toDouble(r.get("Rate")));
            d.put("Amount", toDouble(r.get("Amount")));
            d.put("Remarks", str(r.get("Remarks")));
            d.put("BomHeaderId", toInt(r.get("BomHeaderId")));
            d.put("OutPutItemId", toInt(r.get("OutPutItemId")));
            c.wages.add(d);
        }
    }

    // -------------------------------------------------------------------------------- BLL MakeVoucher (0350:19)

    private Map<String, Object> makeVoucher(Ctx c) {
        need(c.wipAccountId != 0, "WIP AccountId Not Found");
        List<Map<String, Object>> items = inRepo.itemGlAccounts(c.u);
        Map<String, Object> vh = ProductionInputRepository.voucherHeadDefaults();
        if (!items.isEmpty()) {
            Map<String, Object> hg = itemGl(items, c.headerItemId);
            if (hg != null) {
                vh.put("RefAccountId", toInt(ci(hg, "PurchaseGLAC")));
                vh.put("AgainstAccountId", toInt(ci(hg, "PurchaseGLAC")));
            }
        }
        vh.put("DocumentTypeSrNo", c.headerId);
        vh.put("VoucherDate", c.docDate);
        vh.put("VoucherCode", c.docNo);
        vh.put("DocumentTypeId", DOC_TYPE);
        vh.put("EntryDate", LocalDateTime.now());
        vh.put("ModifyDate", LocalDateTime.now());
        vh.put("EntryUser", c.userId);
        vh.put("ModifyUser", c.userId);
        vh.put("OrganizationId", c.org);
        vh.put("CompanyId", c.company);
        vh.put("FinancialYearId", c.fy);
        vh.put("BranchId", c.branch);
        vh.put("IsApproved", false);
        double total = 0;
        int lineId = 1;
        for (Map<String, Object> o : c.input) {
            if (toInt(o.get("ActionTypeId")) == 3) continue;
            need(!items.isEmpty(), "Item GL Account Not Found");
            Map<String, Object> gl = itemGl(items, toInt(o.get("ItemId")));
            if (gl == null) continue;
            String text = "ItemName: " + str(ci(gl, "ItemName")) + " Qty  " + net(toDouble(o.get("ItemQty"))) + " Rate  " + net(toDouble(o.get("Rate")));
            Map<String, Object> dr = ProductionInputRepository.voucherDetailDefaults();
            dr.put("LineId", lineId);
            dr.put("AccountId", c.wipAccountId);
            dr.put("AgainstAccountId", toInt(ci(gl, "PurchaseGLAC")));
            dr.put("Comments", text);
            dr.put("DebitAmount", toDouble(o.get("Amount")));
            dr.put("QtyOut", toDouble(o.get("ItemQty")));
            dr.put("WeightOut", toDouble(o.get("NetWeight")));
            dr.put("ItemRate", toDouble(o.get("Rate")));
            dr.put("ItemAmount", toDouble(o.get("Amount")));
            dr.put("JobLotId", toInt(o.get("JobLotId")));
            c.voucherLines.add(dr);
            Map<String, Object> cr = ProductionInputRepository.voucherDetailDefaults();
            cr.put("LineId", lineId);
            cr.put("AccountId", toInt(ci(gl, "PurchaseGLAC")));
            cr.put("AgainstAccountId", c.wipAccountId);
            cr.put("Comments", text);
            cr.put("CreditAmount", toDouble(o.get("Amount")));
            cr.put("QtyOut", toDouble(o.get("ItemQty")));
            cr.put("WeightOut", toDouble(o.get("NetWeight")));
            cr.put("ItemRate", toDouble(o.get("Rate")));
            cr.put("ItemAmount", toDouble(o.get("Amount")));
            cr.put("JobLotId", toInt(o.get("JobLotId")));
            c.voucherLines.add(cr);
            total += toDouble(cr.get("CreditAmount"));
            lineId++;
        }
        for (Map<String, Object> o : c.overhead) {
            if (str(o.get("OhRemarks")).isEmpty()) {
                String s = "OverHeads ";
                if (toDouble(o.get("Qty")) > 0) s += "Qty " + net(toDouble(o.get("Qty")));
                if (toDouble(o.get("Rate")) > 0) s += " Rate " + net(toDouble(o.get("Rate")));
                if (toDouble(o.get("Amount")) > 0) s += " Amount " + net(toDouble(o.get("Amount")));
                o.put("OhRemarks", s);
            }
            Map<String, Object> dr = ProductionInputRepository.voucherDetailDefaults();
            dr.put("AccountId", c.wipAccountId);
            dr.put("AgainstAccountId", toInt(o.get("OverHeadAccountId")));
            dr.put("Comments", str(o.get("OhRemarks")));
            dr.put("DebitAmount", toDouble(o.get("Amount")));
            dr.put("QtyIn", toDouble(o.get("Qty")));
            dr.put("WeightIn", toDouble(o.get("Qty")));
            dr.put("ItemRate", toDouble(o.get("Rate")));
            dr.put("ItemAmount", toDouble(o.get("Amount")));
            total += toDouble(dr.get("DebitAmount"));
            c.voucherLines.add(dr);
            Map<String, Object> cr = ProductionInputRepository.voucherDetailDefaults();
            cr.put("AccountId", toInt(o.get("OverHeadAccountId")));
            cr.put("AgainstAccountId", c.wipAccountId);
            cr.put("Comments", str(o.get("OhRemarks")));
            cr.put("CreditAmount", toDouble(o.get("Amount")));
            cr.put("QtyIn", toDouble(o.get("Qty")));
            cr.put("WeightIn", toDouble(o.get("Qty")));
            cr.put("ItemRate", toDouble(o.get("Rate")));
            cr.put("ItemAmount", toDouble(o.get("Amount")));
            c.voucherLines.add(cr);
        }
        if (!c.wages.isEmpty()) {
            List<Map<String, Object>> suppliers = repo.supplierGl(c.u);
            int debitAccount = c.wagesDebitExpense ? c.wagesExpenseAccountId : c.wipAccountId;
            for (Map<String, Object> o : c.wages) {
                if (str(o.get("Remarks")).isEmpty()) {
                    String s = "Wages ";
                    if (toDouble(o.get("Qty")) > 0) s += "Qty " + net(toDouble(o.get("Qty")));
                    if (toDouble(o.get("Rate")) > 0) s += " Rate " + net(toDouble(o.get("Rate")));
                    if (toDouble(o.get("Amount")) > 0) s += " Amount " + net(toDouble(o.get("Amount")));
                    o.put("Remarks", s);
                }
                Map<String, Object> sup = null;
                for (Map<String, Object> s : suppliers) if (toInt(ci(s, "Id")) == toInt(o.get("ContractorId"))) { sup = s; break; }
                if (sup == null) throw new IllegalArgumentException("Contractor GlAccountId Not Found");
                int contractorGl = toInt(ci(sup, "GlAccountId"));
                Map<String, Object> dr = ProductionInputRepository.voucherDetailDefaults();
                dr.put("AccountId", debitAccount);
                dr.put("AgainstAccountId", contractorGl);
                dr.put("Comments", str(o.get("Remarks")));
                dr.put("DebitAmount", toDouble(o.get("Amount")));
                dr.put("QtyIn", toDouble(o.get("Qty")));
                dr.put("WeightIn", toDouble(o.get("Qty")));
                dr.put("ItemRate", toDouble(o.get("Rate")));
                dr.put("ItemAmount", toDouble(o.get("Amount")));
                total += toDouble(dr.get("DebitAmount"));
                c.voucherLines.add(dr);
                Map<String, Object> cr = ProductionInputRepository.voucherDetailDefaults();
                cr.put("AccountId", contractorGl);
                cr.put("AgainstAccountId", debitAccount);
                cr.put("Comments", str(o.get("Remarks")));
                cr.put("CreditAmount", toDouble(o.get("Amount")));
                cr.put("QtyIn", toDouble(o.get("Qty")));
                cr.put("WeightIn", toDouble(o.get("Qty")));
                cr.put("ItemRate", toDouble(o.get("Rate")));
                cr.put("ItemAmount", toDouble(o.get("Amount")));
                cr.put("SubsidiaryTypeId", 1);
                cr.put("SubsidiaryAccountId", toInt(o.get("ContractorId")));
                cr.put("SupplierCustomerId", toInt(o.get("ContractorId")));
                c.voucherLines.add(cr);
            }
        }
        vh.put("VoucherAmount", total);
        return vh;
    }

    private static Map<String, Object> itemGl(List<Map<String, Object>> items, int itemId) {
        for (Map<String, Object> g : items) if (toInt(ci(g, "Id")) == itemId) return g;
        return null;
    }

    // -------------------------------------------------------------------------------- DAL SetData (0288:16)

    private Integer setData(Ctx c, Map<String, Object> header) {
        int num = repo.setHeader(!c.update, header);
        if (num > 0) c.headerId = num;
        else num = c.headerId;

        int line = 1;
        for (Map<String, Object> d : c.output) {
            d.put("ProductionHeaderId", c.headerId);
            d.put("WorkOrderId", c.workOrderId);
            d.put("LineId", line);
            d.put("Id", repo.setOutput(model(d, OUTPUT_PARAMS)));
            line++;
        }
        line = 1;
        for (Map<String, Object> d : c.input) {
            d.put("ProductionHeaderId", c.headerId);
            d.put("LineId", line);
            d.put("Id", repo.setInput(model(d, INPUT_PARAMS)));
            line++;
        }
        for (Map<String, Object> d : c.overhead) {
            d.put("ProductionHeaderId", c.headerId);
            d.put("Id", repo.setOverHead(model(d, OVERHEAD_PARAMS)));
        }
        for (Map<String, Object> d : c.wages) {
            d.put("ProductionHeaderId", c.headerId);
            d.put("Id", repo.setWages(model(d, WAGES_PARAMS)));
        }
        /* AttachmentsList (Proc_DMSAttachments_Insert) is not ported. */

        if (c.fifo) {
            List<Map<String, Object>> itemGl = inRepo.itemGlAccounts(c.u);
            List<Map<String, Object>> evaluations = new ArrayList<>();
            for (Map<String, Object> d : c.input) {
                if (toInt(d.get("ActionTypeId")) == 3) continue;
                Map<String, Object> gl = itemGl(itemGl, toInt(d.get("ItemId")));
                if (gl == null) continue;
                for (Map<String, Object> se : fifoImplementation(c, d, str(ci(gl, "ItemName")), evaluations)) {
                    se.put("ItemUom", toInt(d.get("ItemUomId")));
                    evaluations.add(se);
                }
            }
            double inputAmount = 0, cgs = 0;
            for (Map<String, Object> d : c.input) if (toInt(d.get("ActionTypeId")) != 3) inputAmount += toDouble(d.get("Amount"));
            for (Map<String, Object> se : evaluations) cgs += toDouble(se.get("CgsAmount"));
            if (round(inputAmount, 4) != round(cgs, 4)) {
                throw new IllegalArgumentException("Total input amount and total cgs amount against fifo not match.input amount is " + net(round(inputAmount, 4)) + " cgs amount is " + net(cgs));
            }
            if (!evaluations.isEmpty()) {
                if (c.update) {
                    LinkedHashMap<String, Object> p = ProductionInputRepository.params();
                    p.put("@OrganizationId", c.org);
                    p.put("@CompanyId", c.company);
                    p.put("@RefDocumentTypeId", DOC_TYPE);
                    p.put("@RefDocIdNo", c.headerId);
                    inRepo.command("[dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId]", p);
                }
                for (Map<String, Object> se : evaluations) {
                    se.put("OrganizationId", c.org);
                    se.put("CompanyId", c.company);
                    se.put("DocDate", c.docDate);
                    se.put("DocCodeNo", c.docNo);
                    se.put("SupplierCustomerId", 0);
                    se.put("PrdJobOrderNo", c.workOrderId);
                    se.put("BranchesId", c.branch);
                    se.put("RefDocumentTypeId", DOC_TYPE);
                    se.put("EntryUser", c.update ? 0 : c.userId);
                    se.put("ModifyUser", c.update ? c.userId : 0);
                    se.put("CalcType", "Weight");
                    int lid = toInt(se.get("LineId"));
                    for (Map<String, Object> d : c.input) {
                        if (toInt(d.get("LineId")) == lid && lid > 0) {
                            se.put("RefDocIdNo", toInt(d.get("ProductionHeaderId")));
                            se.put("RefDocSubIdNo", toInt(d.get("Id")));
                            break;
                        }
                    }
                    inRepo.setProc("USP_InventoryStockEvalautionDetail_Insert", se, ProductionInputRepository.P_STOCK_EVALUATION_INSERT);
                }
            }
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

        for (Map<String, Object> d : c.input) {                                                            // every row, removed ones included (0288 SetData)
            LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@OrganizationId", c.org);
            p.put("@CompanyId", c.company);
            p.put("@DocumentTypeId", DOC_TYPE);
            p.put("@DocDate", c.docDate);
            p.put("@ItemId", toInt(d.get("ItemId")));
            p.put("@WarehouseId", toInt(d.get("WareHouseId")));
            p.put("@JobLotId", toInt(d.get("JobLotId")));
            p.put("@PackUomId", toInt(d.get("ItemUomId")));
            p.put("@NetWeight", toDouble(d.get("NetWeight")));
            inRepo.command("USP_InventoryValidation", p);
        }

        /* voucher */
        int existing = inRepo.voucherHeadId(c.u, DOC_TYPE, c.headerId);
        Map<String, Object> vh = c.voucher;
        if (existing > 0) vh.put("Id", existing);
        vh.put("DocumentTypeSrNo", c.headerId);
        int vr = inRepo.setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh, ProductionInputRepository.P_VOUCHER_HEAD);
        int voucherId = vr > 0 ? vr : toInt(vh.get("Id"));
        vh.put("Id", voucherId);
        if (!c.voucherLines.isEmpty()) {
            for (Map<String, Object> v : c.voucherLines) {
                v.put("VoucherHeadId", voucherId);
                int lid = toInt(v.get("LineId"));
                for (Map<String, Object> d : c.input) {
                    if (toInt(d.get("LineId")) == lid && lid > 0 && toDouble(v.get("CreditAmount")) > 0) { v.put("RefDocSubIdNo", toInt(d.get("Id"))); break; }
                }
                for (Map<String, Object> d : c.output) {
                    if (toInt(d.get("LineId")) == lid && lid > 0 && toDouble(v.get("DebitAmount")) > 0) { v.put("RefDocSubIdNo", toInt(d.get("Id"))); break; }
                }
                inRepo.setProc("Sp_VoucherDetail_Insert", v, ProductionInputRepository.P_VOUCHER_DETAIL);
            }
            if (voucherId > 0) {
                LinkedHashMap<String, Object> p = ProductionInputRepository.params();
                p.put("@OrganizationId", c.org);
                p.put("@CompanyId", c.company);
                p.put("@Id", voucherId);
                inRepo.command("USP_VoucherBalanceCheck", p);
            }
        }
        return num;
    }

    /** CommonServices.FIFOImplementionForConcrete (DAL 0205): no @VarientId, no @JobLotId (the DAL never sets them), ItemQty drives the layer walk. */
    private List<Map<String, Object>> fifoImplementation(Ctx c, Map<String, Object> d, String itemName, List<Map<String, Object>> reserved) {
        String xml = null;
        if (!reserved.isEmpty()) {
            StringBuilder sb = new StringBuilder("<ArrayOfFIFOStockEvaluation xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
            for (Map<String, Object> r : reserved) {
                sb.append("<FIFOStockEvaluation><RefDocumentTypeId>").append(toInt(r.get("RefRefDocumentTypeId"))).append("</RefDocumentTypeId>")
                        .append("<RefDocIdNo>").append(toInt(r.get("RefRefDocIdNo"))).append("</RefDocIdNo>")
                        .append("<RefDocSubIdNo>").append(toInt(r.get("RefRefDocSubIdNo"))).append("</RefDocSubIdNo>")
                        .append("<ReserveQty>").append(net(toDouble(r.get("QtyOut")))).append("</ReserveQty>")
                        .append("<ReserveWeight>").append(net(toDouble(r.get("StockWeightOut")))).append("</ReserveWeight></FIFOStockEvaluation>");
            }
            xml = sb.append("</ArrayOfFIFOStockEvaluation>").toString();
        }
        List<Map<String, Object>> layers = repo.fifoLayers(c.u, toInt(d.get("ItemId")), c.docDate, toInt(d.get("WareHouseId")), 0,
                c.update ? DOC_TYPE : 0, c.update ? c.headerId : 0, xml);
        if (layers.isEmpty()) throw new IllegalArgumentException("Stock Not Found this Item " + itemName + " against FIFO Method .....");
        double available = 0;
        for (Map<String, Object> l : layers) available += toDouble(ci(l, "NetBalQty"));
        double itemQty = toDouble(d.get("ItemQty"));
        if (!(itemQty <= available)) {
            throw new IllegalArgumentException("Qty available is " + net(available) + " and row Qty is " + net(itemQty) + " this item " + itemName + " against FIFO....");
        }
        List<Map<String, Object>> out = new ArrayList<>();
        double consumed = 0;
        for (Map<String, Object> l : layers) {
            double avgRate = toDouble(ci(l, "AvgRate"));
            if (avgRate <= 0) throw new IllegalArgumentException("Rate Not Found this Item " + itemName + " against FIFO Method");
            double balQty = toDouble(ci(l, "NetBalQty"));
            Map<String, Object> se = ProductionInputRepository.stockEvaluationDefaults();
            se.put("Id", toInt(ci(l, "Id")));
            se.put("LineId", toInt(d.get("LineId")));
            se.put("ItemId", toInt(d.get("ItemId")));
            se.put("WarehouseId", toInt(d.get("WareHouseId")));
            se.put("JobLotId", 0);
            se.put("InvPackingTypeId", 0);
            se.put("VarientId", 0);
            se.put("CropBatch", "");
            se.put("RefRefDocumentTypeId", toInt(ci(l, "RefDocumentTypeId")));
            se.put("RefRefDocIdNo", toInt(ci(l, "RefDocIdNo")));
            se.put("RefRefDocSubIdNo", toInt(ci(l, "RefDocSubIdNo")));
            se.put("CgsRate", avgRate);
            se.put("RateUom", toInt(ci(l, "RateUomId")));
            if (balQty <= itemQty - consumed) {
                se.put("QtyOut", balQty);
                se.put("BillWeightOut", balQty);
                se.put("StockWeightOut", balQty);
                se.put("CgsAmount", balQty * avgRate);
                consumed += balQty;
                out.add(se);
            } else if (balQty >= itemQty - consumed) {
                double q = itemQty - consumed;
                se.put("QtyOut", q);
                se.put("BillWeightOut", q);
                se.put("StockWeightOut", q);
                se.put("CgsAmount", q * avgRate);
                consumed += q;
                out.add(se);
            }
            if (itemQty == consumed) break;
        }
        return out;
    }

    // -------------------------------------------------------------------------------- procedure parameter filters (procdure.utf8.sql, [pcc] schema)

    private static final Set<String> OUTPUT_PARAMS = setOf("Id", "ProductionHeaderId", "WorkOrderId", "BOMHeaderId", "EntryTypeId", "SaleOrderId", "WareHouseId", "JobLotId",
            "ItemAttributeVarientId", "VarientEquivalent", "ItemId", "ItemUomId", "VarientQty", "ItemQty", "NetWeight", "Rate", "RateUomId", "Amount", "Remarks", "ActionTypeId",
            "LineId", "WorkOrderDetailId", "CustomerId", "SaleOrderDetailId", "CustomerName", "CustomerTypeId", "ReferenceNo");
    private static final Set<String> INPUT_PARAMS = setOf("Id", "ProductionHeaderId", "BomInputId", "WareHouseId", "JobLotId", "ItemId", "ItemUomId", "ItemQty", "NetWeight",
            "Rate", "RateUomId", "Amount", "Remarks", "ActionTypeId", "LineId", "BomHeaderId", "MinQty", "MaxQty");
    private static final Set<String> OVERHEAD_PARAMS = setOf("Id", "ProductionHeaderId", "BomOverHeadId", "OverHeadTypeId", "OverHeadAccountId", "PercentOfTotalInput", "Qty",
            "Rate", "BomQty", "Amount", "OhRemarks", "BomHeaderId");
    private static final Set<String> WAGES_PARAMS = setOf("Id", "ProductionHeaderId", "BomWagesId", "ContractorId", "ContractorWagesRateScheduleId", "ItemId", "Qty", "Rate",
            "Amount", "Remarks", "BomQty", "BomHeaderId", "OutPutItemId");
    private static final Set<String> STRING_PARAMS = setOf("Remarks", "OhRemarks", "CustomerName", "ReferenceNo");

    private static Set<String> setOf(String... a) { return new LinkedHashSet<>(java.util.Arrays.asList(a)); }

    /**
     * GenericProvider.SetProc over a detail model: value-type properties go out at their CLR value (0 when never set), reference-type (string) ones only when set.
     * Every int / decimal property of the four detail models is listed in the set, so a missing key is sent as 0.
     */
    private static Map<String, Object> model(Map<String, Object> d, Set<String> declared) {
        Map<String, Object> p = new LinkedHashMap<>();
        for (String k : declared) {
            if (STRING_PARAMS.contains(k)) {
                if (d.get(k) != null) p.put(k, d.get(k));
            } else {
                Object v = d.get(k);
                p.put(k, v == null ? Integer.valueOf(0) : v);
            }
        }
        return p;
    }
}
