package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.Pcc822ConversionRepository;
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
 * Pcc822 - Production hub screen 822 "Stock Conversion" - Architecture.WinApp.Pcc.frmConversion (.cs 8,723 lines), rights key base.Name "frmConversion",
 * DocumentTypeId 1867 (Insert:2776, Load:...). Schema [pcc]. BLL Architecture.BLL.pcc.Production.ConversionHeader (0362), DAL 0297 (SetData).
 *
 * <pre>
 *   Route     /production/pcc-conversion      API /production/pcc-conversion/api
 *   Not any of /production/stock-conversion (invfrmStockConversionProduction, doc 66), /party-processing/stock-conversion (screen 602, doc 176) or
 *   /packing-material/store-stock-conversion: those are different desktop classes and different procedures.
 * </pre>
 *
 * SAVE (BLL Save + DAL SetData, one transaction): header [pcc].[USP_ConversionHeader_Insert|Update] -> output rows -> input rows -> overhead rows -> wages rows
 * -> (ERP feature 5) FIFO stock evaluation rows via [pcc].[USP_GetStockByFifoMethod] -> Sp_InventoryStockEvalautionDetail_Update ->
 * Sp_InventoryTransactions_GetALLMethod -> USP_StockValidationConcrete for every input row -> Sp_VoucherHead_Insert|Update + Sp_VoucherDetail_Insert +
 * USP_VoucherBalanceCheck. The voucher is built first (MakeVoucher 0362:19), exactly as the desktop does.
 *
 * KNOWN DIFFERENCES / GAPS
 * <ol>
 *  <li>Attachments (Attachment dialog, DMS copy, Proc_DMSAttachments_Insert) are not ported.</li>
 *  <li>The Yes/No/Cancel "warehouse is not allocated to the plant" prompt (Insert:2860) is done by the page; the server only exposes
 *      /api/warehouse-allocated (dbo.USP_ValidateWareHouseAgainstPlantFromWareHouseAllocationToPlants).</li>
 *  <li>History EntryUserId is sent when the user lacks CanViewAllRecord (the desktop leaves it unset).</li>
 *  <li>Numbers are computed with double on the page and on the server (the desktop uses decimal); generated remarks print numbers without trailing zeros.</li>
 *  <li>Nothing here has been run against the database or a browser.</li>
 * </ol>
 */
@Service
public class Pcc822ConversionService {

    public static final String SCREEN = "frmConversion";
    public static final int DOC_TYPE = 1867;
    private static final int ERP_FEATURE_FIFO_CGS = 5;
    private static final Set<Integer> OVERHEAD_ACCOUNT_TYPES = new HashSet<>(java.util.Arrays.asList(3, 6, 8, 11, 12, 13, 14, 20, 21));

    private final Prod273Support db;
    private final Pcc822ConversionRepository repo;
    private final ProductionInputRepository inRepo;
    private final PlatformTransactionManager tm;

    public Pcc822ConversionService(Prod273Support db, Pcc822ConversionRepository repo, ProductionInputRepository inRepo, PlatformTransactionManager tm) {
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

    /** A cell as the JSON the page can read: dates as ISO text, decimals as double. */
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

    // ====================================================================================== load (frmBillOfMaterial_Manufacturing_Load)

    /** Load: FifoMethod:ERP feature 5, rights, DocumentNoFill, ProductionTypeFill, PlantFeader, ItemDetailFill, UOMFill / uomgetAll, dtFillsForGrids, Datetypefill, AllCombobind. */
    public Map<String, Object> lookups() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", db.rights(SCREEN));
        out.put("branchId", db.branchId(u));
        out.put("financialYearStart", db.financialYearStart(u, db.financialYearId()));
        out.put("fifo", inRepo.erpFeature(u, ERP_FEATURE_FIFO_CGS));
        out.put("amountDecimals", amountDecimals(u));
        out.put("rateDecimals", db.rateDecimals(u));
        out.put("docNo", docNo());
        out.put("dateTypes", Prod273Support.literal(Prod273Support.DATE_TYPES, "Parameters"));
        out.putAll(refresh());
        out.put("conversionNos", conversionNos());
        return out;
    }

    /** btnRefresh_Click:2616 - dtFillsForGrids, uomgetAll, PlantFeader, ItemDetailFill (items follow the production type on the page). */
    public Map<String, Object> refresh() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> types = new ArrayList<>();
        for (Map<String, Object> r : repo.productionTypes(u)) {
            int id = toInt(ci(r, "Id"));
            if (id == 45 || id == 46 || id == 47) types.add(pair(id, str(ci(r, "LookupName"))));          // ProductionTypeFill:846
        }
        out.put("productionTypes", types);
        out.put("plants", Prod273Support.project(repo.plants(u), "Id", "Description"));                  // PlantFeader:903
        out.put("allUoms", db.allUoms(u));                                                                // uomgetAll
        out.put("activeWarehouses", Prod273Support.project(repo.activeWarehouses(u), "Id", "WareHouseName")); // dtWareForOutPut:590
        out.put("contractors", Prod273Support.project(repo.contractors(u), "Id", "CompanyName"));         // dtContr:600
        List<Map<String, Object>> accounts = new ArrayList<>();                                           // dtAccounts:611
        for (Map<String, Object> r : db.coaAllocation(u)) {
            if (OVERHEAD_ACCOUNT_TYPES.contains(toInt(ci(r, "AccountTypeId")))) accounts.add(pair(toInt(ci(r, "Id")), str(ci(r, "AccountTitle"))));
        }
        out.put("overheadAccounts", accounts);
        return out;
    }

    private int amountDecimals(UserAccount u) {
        String s = db.config(u, "Default NoofDecimal Points For Amount");
        if (s.isEmpty()) return 2;
        int n = toInt(s);
        return n < 0 || n > 6 ? 2 : n;
    }

    /** DocumentNoFill:660. */
    public int docNo() {
        UserAccount u = db.user();
        return repo.generateCode(u, db.branchId(u), DOC_TYPE, db.financialYearId());
    }

    /** AllCombobind: pcc.USP_GetDataForDropDownFromConversionHeader 'ConversionNo'. */
    public List<Map<String, Object>> conversionNos() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.conversionNoDropDown(db.user())) out.add(pair(toInt(ci(r, "Id")), str(ci(r, "ReferenceName"))));
        return out;
    }

    /** ItemDetailFill:1010 / GetItemByItemTypeId. */
    public List<Map<String, Object>> items(String typeIds) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.itemsByType(db.user(), typeIds)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Name", str(ci(r, "ItemName")));
            o.put("Code", str(ci(r, "ItemCodeNew")));
            out.add(o);
        }
        return out;
    }

    /** AttributeVarientFill:688 - ItemAttributeVarient.GetAllForCombo(item). */
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

    /** CmbPlantName_Leave:960 - CommonServices.WareHouseAllocationToPlantsByProductionPlantId. */
    public List<Map<String, Object>> plantWarehouses(int plantId) {
        return Prod273Support.project(repo.plantWarehouses(db.user(), plantId), "Id", "WareHouseName");
    }

    /** ValidateWareHouseAgainstPlantFromWareHouseAllocationToPlants (Insert:2862). */
    public Map<String, Object> warehouseAllocated(int warehouseId, int plantId) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("allocated", repo.warehouseAllocatedToPlant(db.user(), warehouseId, plantId));
        return m;
    }

    /** Wages F1 (grdContractorWages_KeyDown:5088) - GetServiceActivityByItemId(1, item, docDate, activity, contractor). */
    public List<Map<String, Object>> serviceActivities(int itemId, String docDate, int activityId, int contractorId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.wagesActivities(db.user(), itemId, pickerValue(docDate), activityId, contractorId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "ContractorWagesRateScheduleId")));
            o.put("Name", str(ci(r, "WagesActivity")));
            o.put("Rate", toDouble(ci(r, "Rate")));
            o.put("Qty", toDouble(ci(r, "Qty")));
            out.add(o);
        }
        return out;
    }

    // ====================================================================================== stock / avg rate

    /**
     * The stock lookup the input grid runs after every pick (grdInputMaterial_KeyDown:4350): GetStockInHandAndAvgRateFromEvaluationConcrete, and with ERP
     * feature 5 on, GetAvgRateFromFIFOMethodConcrete (CommonServices) over [pcc].[USP_GetStockByFifoMethod].
     */
    public Map<String, Object> stock(int itemId, int warehouseId, int varientId, double itemQty, String docDate, int recId) {
        UserAccount u = db.user();
        Timestamp date = pickerValue(docDate);
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> rows = repo.stockInHand(u, itemId, date, warehouseId);
        out.put("found", !rows.isEmpty());
        out.put("qtyInHand", rows.isEmpty() ? 0d : toDouble(ci(rows.get(0), "QtyInHand")));
        out.put("avgRate", rows.isEmpty() ? 0d : toDouble(ci(rows.get(0), "AvgRate")));
        boolean fifo = inRepo.erpFeature(u, ERP_FEATURE_FIFO_CGS);
        out.put("fifo", fifo);
        out.put("fifoRate", (fifo && !rows.isEmpty()) ? fifoAvgRate(u, itemId, date, warehouseId, varientId, itemQty, recId) : 0d);
        return out;
    }

    /** CommonServices.GetAvgRateFromFIFOMethodConcrete (WinApp.Common/CommonServices.cs). ItemQty and NetWeight are the same number here. */
    private double fifoAvgRate(UserAccount u, int itemId, Timestamp date, int warehouseId, int varientId, double itemQty, int recId) {
        List<Map<String, Object>> layers = repo.fifoLayers(u, itemId, date, varientId, warehouseId, recId > 0 ? DOC_TYPE : 0, recId, null);
        if (layers.isEmpty()) return 0d;
        double availableWeight = 0, availableQtyTotal = 0;
        for (Map<String, Object> l : layers) {
            availableWeight += toDouble(ci(l, "BalWeight"));
            availableQtyTotal += toDouble(ci(l, "BalQty"));
        }
        double netWeight = itemQty;
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

    /** FormHistory (btnshow / HistoryGridFill) - [pcc].[USP_ConversionHeader_GetAllMethod] 'FormHistory'. */
    public List<Map<String, Object>> history(String fromDate, String toDate, int fromDocNo, int toDocNo, int conversionId) {
        UserAccount u = db.user();
        boolean viewAll = db.has(SCREEN, "viewAll");
        List<Map<String, Object>> rows = repo.formHistory(u, db.branchId(u), DOC_TYPE, db.financialYearId(), ts(fromDate), ts(toDate), fromDocNo, toDocNo,
                conversionId, viewAll, u.getId() == null ? 0 : u.getId());
        return json(rows);
    }

    /** ReadById:3155 - header + the four detail lists, already shaped as the page's grid rows (dtInput / dtOutPutByProduct / dtOverHead / dtContractorWages). */
    public Map<String, Object> read(int id) {
        List<Map<String, Object>> h = repo.read("ReadById", id);
        if (h.isEmpty()) throw new IllegalArgumentException("Record not found.");
        Map<String, Object> r = h.get(0);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", toInt(ci(r, "Id")));
        out.put("docDate", day(ci(r, "DocDate")));
        out.put("docNo", toInt(ci(r, "DocNo")));
        out.put("conversionNo", str(ci(r, "ConversionNo")));
        out.put("remarksHeader", str(ci(r, "RemarksHeader")));
        out.put("plantId", toInt(ci(r, "PlantId")));
        out.put("conversionTypeId", toInt(ci(r, "ConversionTypeId")));
        out.put("isApproved", Prod273Support.truthy(ci(r, "IsApproved")));

        List<Map<String, Object>> input = new ArrayList<>();
        for (Map<String, Object> d : repo.read("ReadByHeaderId_ConversionInputDetail", id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(d, "Id")));
            o.put("WarehouseId", toInt(ci(d, "WareHouseId")));
            o.put("WarehouseName", str(ci(d, "WareHouseName")));
            o.put("ItemId", toInt(ci(d, "ItemId")));
            o.put("ItemName", str(ci(d, "ItemName")));
            o.put("ItemUomId", toInt(ci(d, "ItemUomId")));
            o.put("Uom", toDouble(ci(d, "ItemUomEquivalent")));
            o.put("UomCode", str(ci(d, "ItemUomCode")));
            o.put("BalanceStock", toDouble(ci(d, "ItemQty")));                                           // ReadById:3203 puts ItemQty in both columns
            o.put("ItemQty", toDouble(ci(d, "ItemQty")));
            o.put("ItemAttributeVarientId", toInt(ci(d, "ItemAttributeVarientId")));
            o.put("AttributeVarient", str(ci(d, "ItemAttribute")));
            o.put("VarientEquivalent", toDouble(ci(d, "VarientEquivalent")));
            o.put("VarientQty", toDouble(ci(d, "VarientQty")));
            o.put("NetWeight", toDouble(ci(d, "NetWeight")));
            o.put("Rate", toDouble(ci(d, "Rate")));
            o.put("RateUomId", toInt(ci(d, "RateUomId")));
            o.put("RateUom", toDouble(ci(d, "RateUomEquivalent")));
            o.put("RateUomCode", str(ci(d, "RateUomCode")));
            o.put("Amount", toDouble(ci(d, "Amount")));
            input.add(o);
        }
        out.put("input", input);

        List<Map<String, Object>> output = new ArrayList<>();
        for (Map<String, Object> d : repo.read("ReadByHeaderId_ConversionOutPutDetail", id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(d, "Id")));
            o.put("WarehouseId", toInt(ci(d, "WareHouseId")));
            o.put("WarehouseName", str(ci(d, "WareHouseName")));
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
        for (Map<String, Object> d : repo.read("ReadByHeaderId_ConversionOverHeadsDetail", id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(d, "Id")));
            o.put("OverHeadTypeId", toInt(ci(d, "OverHeadTypeId")));
            o.put("OverHeadAccountId", toInt(ci(d, "OverHeadAccountId")));
            o.put("OverHeadAccountTitle", str(ci(d, "OverHeadAccountTitle")));
            o.put("PercentOfTotalInput", toDouble(ci(d, "PercentOfTotalInput")));
            o.put("Qty", toDouble(ci(d, "Qty")));
            o.put("Rate", toDouble(ci(d, "Rate")));
            o.put("Amount", toDouble(ci(d, "Amount")));
            o.put("OhRemarks", str(ci(d, "OhRemarks")));
            overhead.add(o);
        }
        out.put("overhead", overhead);

        List<Map<String, Object>> wages = new ArrayList<>();
        for (Map<String, Object> d : repo.read("ReadByHeaderId_ConversionWagesDetail", id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(d, "Id")));
            o.put("OutPutItemId", toInt(ci(d, "OutPutItemId")));
            o.put("OutPutItemName", str(ci(d, "OutPutItemName")));
            o.put("ContractorId", toInt(ci(d, "ContractorId")));
            o.put("ContractorName", str(ci(d, "ContractorName")));
            o.put("ContractorWagesRateScheduleId", toInt(ci(d, "ContractorWagesRateScheduleId")));
            o.put("ServiceActivity", str(ci(d, "ServiceActivity")));
            o.put("Qty", toDouble(ci(d, "Qty")));
            o.put("Rate", toDouble(ci(d, "Rate")));
            o.put("Amount", toDouble(ci(d, "Amount")));
            o.put("Remarks", str(ci(d, "Remarks")));
            wages.add(o);
        }
        out.put("wages", wages);
        return out;
    }

    /** btnDelete_Click:3267 - ConversionHeader.DeleteByID(UserAccount.ID, RecId). */
    public Map<String, Object> delete(int id) {
        right("delete", "You do not have the Delete right on Stock Conversion.");
        if (id <= 0) throw new IllegalArgumentException("No record found to Delete");
        UserAccount u = db.user();
        repo.deleteById(u.getId() == null ? 0 : u.getId(), id);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", "Delete Record Successfully");
        return m;
    }

    /** VoucherReprot:3330 - voucher head of the conversion (Sp_Vouchers_GetMethods ...ByReferenceDocumentTypeIdandRefEntryId). */
    public Map<String, Object> voucherHead(int id) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("voucherHeadId", inRepo.voucherHeadId(db.user(), DOC_TYPE, id));
        return m;
    }

    // ====================================================================================== save (Insert:2776 + BLL Save + DAL SetData)

    private static final class Ctx {
        UserAccount u;
        int org, company, branch, fy, userId;
        int headerId;
        int docNo;
        Timestamp docDate;
        boolean update;
        boolean fifo;
        List<Map<String, Object>> input = new ArrayList<>();      // maps keyed by the pcc detail model property names
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
        right(update ? "update" : "save", update ? "You do not have the Update right on Stock Conversion." : "You do not have the Save right on Stock Conversion.");

        /* FormValidation:1060 */
        String docNoText = str(body.get("docNo")).trim();
        need(!(docNoText.isEmpty() || docNoText.equals("0")), "Doc No Field Required");
        need(!str(body.get("conversionNo")).trim().isEmpty(), "Production No Field Required");
        need(toInt(body.get("conversionTypeId")) != 0, "Production Type Feild Required");
        need(toInt(body.get("plantId")) != 0, "Plant Name Feild Required");

        List<Map<String, Object>> inRows = list(body.get("input"));
        List<Map<String, Object>> outRows = list(body.get("output"));
        List<Map<String, Object>> ohRows = list(body.get("overhead"));
        List<Map<String, Object>> wgRows = list(body.get("wages"));
        need(!(inRows.isEmpty() || outRows.isEmpty()), "Grid Record Not Found...\nGrid (Input and OutPut) Should have atleast one record");

        isDuplicateRowExists(outRows);                                                                    // Insert:2802 -> :2663
        if (!wgRows.isEmpty() && toInt(wgRows.get(0).get("OutPutItemId")) > 0) checkBomHeaderIds(inRows, outRows, wgRows);   // :2806 -> :2685

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

        /* the page runs btnGenerateRatesOutput (UpdateRatesOutput) before Save; the header check is :2813 */
        double inputTotal = 0, ohTotal = 0, wgTotal = 0, outputTotal = 0;
        for (Map<String, Object> r : inRows) inputTotal += toDouble(r.get("Amount"));
        for (Map<String, Object> r : ohRows) ohTotal += toDouble(r.get("Amount"));
        for (Map<String, Object> r : wgRows) wgTotal += toDouble(r.get("Amount"));
        for (Map<String, Object> r : outRows) outputTotal += toDouble(r.get("Amount"));
        double netValue = inputTotal + ohTotal + wgTotal;
        if (round(netValue, 2) != round(outputTotal, 2) && Math.abs(round(netValue, 2) - round(outputTotal, 2)) > 0.0051) {
            throw new IllegalArgumentException("Net Vale " + String.format("%.2f", netValue) + " In Header and Total OutPut Amount " + String.format("%.2f", outputTotal) + " are not equal");
        }

        buildInput(c, inRows, list(body.get("removedInput")));
        buildOutput(c, outRows, list(body.get("removedOutput")));
        buildOverhead(c, ohRows);
        buildWages(c, wgRows);

        /* BLL Save:177 - the voucher is made before the user ids are adjusted */
        c.voucher = makeVoucher(c, body);

        Map<String, Object> header = new LinkedHashMap<>();
        header.put("Id", recId);
        header.put("DocumentTypeId", DOC_TYPE);
        header.put("DocNo", c.docNo);
        header.put("DocDate", c.docDate);
        header.put("ConversionNo", str(body.get("conversionNo")));
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
        header.put("ActionId", update ? 2 : 1);
        header.put("PlantId", toInt(body.get("plantId")));
        header.put("ConversionTypeId", toInt(body.get("conversionTypeId")));

        Integer saved = new TransactionTemplate(tm).execute(status -> setData(c, header));
        int id = saved == null ? 0 : saved;

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", id);
        out.put("docNo", c.docNo);
        out.put("message", (update ? "Data Update Successfully....  " : "Data Save Successfully....  ") + c.docNo);
        return out;
    }

    /** IsDuplicateRowExists:2656 - key item-variant-warehouse over every output row. */
    private void isDuplicateRowExists(List<Map<String, Object>> outRows) {
        Map<String, Integer> seen = new HashMap<>();
        for (int i = 0; i < outRows.size(); i++) {
            Map<String, Object> r = outRows.get(i);
            String key = toInt(r.get("ItemId")) + "-" + toInt(r.get("ItemAttributeVarientId")) + "-" + toInt(r.get("WarehouseId"));
            Integer prev = seen.get(key);
            if (prev != null) {
                throw new IllegalArgumentException("Duplicate Record not added to the grid.\nItem " + str(r.get("ItemName")) + " with  Variant " + str(r.get("AttributeVarient"))
                        + " and Warehouse " + str(r.get("WarehouseName")) + " exists in row No " + (i + 1) + " and Row No " + (prev + 1));
            }
            seen.put(key, i);
        }
    }

    /** CheckBomHeaderIds:2685 - output items vs wages items. The missing-in-output names are read from the INPUT table, as on the desktop. */
    private void checkBomHeaderIds(List<Map<String, Object>> inRows, List<Map<String, Object>> outRows, List<Map<String, Object>> wgRows) {
        Set<Integer> outIds = new LinkedHashSet<>();
        for (Map<String, Object> r : outRows) if (toInt(r.get("ItemId")) > 0) outIds.add(toInt(r.get("ItemId")));
        Set<Integer> wgIds = new LinkedHashSet<>();
        for (Map<String, Object> r : wgRows) if (toInt(r.get("OutPutItemId")) > 0) wgIds.add(toInt(r.get("OutPutItemId")));
        Set<Integer> missingInOutput = new LinkedHashSet<>(wgIds);
        missingInOutput.removeAll(outIds);
        Set<Integer> missingInWages = new LinkedHashSet<>(outIds);
        missingInWages.removeAll(wgIds);
        if (!missingInOutput.isEmpty()) {
            List<String> names = new ArrayList<>();
            for (Map<String, Object> r : inRows) if (missingInOutput.contains(toInt(r.get("ItemId")))) names.add(str(r.get("ItemName")));
            throw new IllegalArgumentException("ItemName " + String.join(", ", names) + " found in Wages Grid but not in Output Grid.");
        }
        if (!missingInWages.isEmpty()) {
            List<String> names = new ArrayList<>();
            for (Map<String, Object> r : outRows) if (missingInWages.contains(toInt(r.get("ItemId")))) names.add(str(r.get("ItemName")));
            throw new IllegalArgumentException("ItemName " + String.join(", ", names) + " found in Output Grid but not in Wages Grid.");
        }
    }

    // -------------------------------------------------------------------------------- row building with the desktop's validation order

    private void buildInput(Ctx c, List<Map<String, Object>> rows, List<Map<String, Object>> removed) {
        Map<Integer, List<Map<String, Object>>> uomCache = new HashMap<>();
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            int n = i + 1;
            if (toInt(r.get("ItemId")) == 0) continue;                                                     // Insert:2837
            Map<String, Object> d = new LinkedHashMap<>();
            int id = toInt(r.get("Id"));
            d.put("Id", id);
            d.put("ActionTypeId", id > 0 ? 2 : 1);
            need(!(empty(r.get("WarehouseId")) || toInt(r.get("WarehouseId")) == 0), "Warehouse Field Required in Grid Input Row No: " + n);
            d.put("WareHouseId", toInt(r.get("WarehouseId")));
            need(!empty(r.get("ItemId")), "Item Field Required in Grid Input Row No: " + n);
            int itemId = toInt(r.get("ItemId"));
            d.put("ItemId", itemId);
            need(!empty(r.get("ItemUomId")), "UOM  Field Required in Grid Input Row No: " + n);
            List<Map<String, Object>> uoms = uomCache.computeIfAbsent(itemId, k -> db.uomsByItem(c.u, k));
            if (!uoms.isEmpty()) {
                Integer match = null;
                for (Map<String, Object> m : uoms) if (toInt(ci(m, "Id")) == toInt(r.get("ItemUomId"))) { match = toInt(ci(m, "Id")); break; }
                need(match != null, "UOM in Input Grid Row No: " + n + " Is not Define Against Item " + itemId + ". Please Check...");
                d.put("ItemUomId", match);
            }
            need(!empty(r.get("ItemQty")), "ItemQty Field Required in Grid Input Row No: " + n);
            d.put("ItemQty", toDouble(r.get("ItemQty")));
            need(!empty(r.get("ItemAttributeVarientId")), "Varient Field Required in Grid Input Row No: " + n);
            d.put("ItemAttributeVarientId", toInt(r.get("ItemAttributeVarientId")));
            need(!empty(r.get("VarientEquivalent")), "Varient Equivalent Field Required in Grid Input Row No: " + n);
            d.put("VarientEquivalent", toDouble(r.get("VarientEquivalent")));
            d.put("VarientQty", toDouble(r.get("VarientQty")));
            need(!empty(r.get("NetWeight")), "NetWeight Field Required in Grid Input Row No: " + n);
            d.put("NetWeight", toDouble(r.get("NetWeight")));
            need(!empty(r.get("Rate")), "Rate Field Required in Grid Input Row No: " + n);
            d.put("Rate", toDouble(r.get("Rate")));
            need(!empty(r.get("RateUomId")), "Rate UOM Field Required in Grid Input Row No: " + n);
            if (!uoms.isEmpty()) {
                Integer match = null;
                for (Map<String, Object> m : uoms) if (toInt(ci(m, "Id")) == toInt(r.get("RateUomId"))) { match = toInt(ci(m, "Id")); break; }
                need(match != null, "RateUom in Input Grid Row No: " + n + " Is not Define Against Item " + itemId + ". Please Check...");
                d.put("RateUomId", match);
            }
            need(!empty(r.get("Amount")), "AmountField Required in Grid Input Row No: " + n);
            d.put("Amount", toDouble(r.get("Amount")));
            d.put("_ItemName", str(r.get("ItemName")));
            c.input.add(d);
        }
        for (Map<String, Object> r : removed) {                                                            // lstRemoveRecordInput: DeleteInputDetailRow:1525
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", toInt(r.get("Id")));
            d.put("WareHouseId", toInt(r.get("WarehouseId")));
            d.put("ItemId", toInt(r.get("ItemId")));
            d.put("ItemUomId", toInt(r.get("ItemUomId")));
            d.put("ItemQty", toDouble(r.get("ItemQty")));
            d.put("ItemAttributeVarientId", toInt(r.get("ItemAttributeVarientId")));
            d.put("VarientEquivalent", toDouble(r.get("VarientEquivalent")));
            d.put("VarientQty", toDouble(r.get("VarientQty")));
            d.put("NetWeight", toDouble(r.get("NetWeight")));
            d.put("Rate", toDouble(r.get("Rate")));
            d.put("RateUomId", toInt(r.get("RateUomId")));
            d.put("Amount", toDouble(r.get("Amount")));
            d.put("ActionTypeId", 3);
            c.input.add(d);
        }
    }

    private void buildOutput(Ctx c, List<Map<String, Object>> rows, List<Map<String, Object>> removed) {
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            int n = i + 1;
            need(toInt(r.get("ItemId")) != 0, "Item Field Required in Grid output Row No: " + n);        // Insert:2995 (reached when the row has no item)
            Map<String, Object> d = new LinkedHashMap<>();
            int id = toInt(r.get("Id"));
            d.put("Id", id);
            d.put("ActionTypeId", id > 0 ? 2 : 1);
            d.put("EntryTypeId", 1);
            need(!(empty(r.get("WarehouseId")) || toInt(r.get("WarehouseId")) == 0), "Warehouse Field Required in Grid OutPut Row No: " + n);
            d.put("WareHouseId", toInt(r.get("WarehouseId")));
            d.put("ItemId", toInt(r.get("ItemId")));
            need(!empty(r.get("ItemAttributeVarientId")), "ItemAttributeVarient Field Required in Grid OutPut Row No: " + n);
            d.put("ItemAttributeVarientId", toInt(r.get("ItemAttributeVarientId")));
            d.put("VarientEquivalent", toDouble(r.get("VarientEquivalent")));
            need(!empty(r.get("ItemQty")), "ItemQty Field Required in Grid OutPut Row No: " + n);
            d.put("ItemQty", toDouble(r.get("ItemQty")));
            need(!empty(r.get("VarientQty")), "VarientQty Field Required in Grid OutPut Row No: " + n);
            d.put("VarientQty", toDouble(r.get("VarientQty")));
            need(!(empty(r.get("Rate")) || toDouble(r.get("Rate")) == 0), "Rate Field Required in Grid Output Row No: " + n);
            d.put("Rate", toDouble(r.get("Rate")));
            need(!empty(r.get("Amount")), "AmountField Required in Grid Output Row No: " + n);
            d.put("Amount", toDouble(r.get("Amount")));
            d.put("ItemUomId", 0);
            d.put("NetWeight", toDouble(r.get("ItemQty")));
            d.put("RateUomId", 0);
            c.output.add(d);
        }
        for (Map<String, Object> r : removed) {                                                            // grdOutPutByProduct_KeyDown:4707
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", toInt(r.get("Id")));
            d.put("WareHouseId", toInt(r.get("WarehouseId")));
            d.put("ItemId", toInt(r.get("ItemId")));
            d.put("ItemAttributeVarientId", toInt(r.get("ItemAttributeVarientId")));
            d.put("ItemUomId", 0);
            d.put("ItemQty", toDouble(r.get("ItemQty")));
            d.put("VarientQty", toDouble(r.get("VarientQty")));
            d.put("VarientEquivalent", toDouble(r.get("VarientEquivalent")));
            d.put("NetWeight", 0d);
            d.put("Rate", toDouble(r.get("Rate")));
            d.put("RateUomId", 0);
            d.put("Amount", toDouble(r.get("Amount")));
            d.put("ActionTypeId", 3);
            c.output.add(d);
        }
    }

    private void buildOverhead(Ctx c, List<Map<String, Object>> rows) {
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            if (toInt(r.get("OverHeadAccountId")) <= 0) continue;                                          // Insert:3016
            int n = i + 1;
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Id", toInt(r.get("Id")));
            d.put("OverHeadTypeId", toInt(r.get("OverHeadAccountId")));
            d.put("OverHeadAccountId", toInt(r.get("OverHeadAccountId")));
            need(!(empty(r.get("Amount")) || toDouble(r.get("Amount")) == 0), "Amount Field Required in Grid Over Head Row No: " + n);
            d.put("PercentOfTotalInput", toDouble(r.get("PercentOfTotalInput")));
            d.put("Qty", toDouble(r.get("Qty")));
            d.put("Rate", toDouble(r.get("Rate")));
            d.put("Amount", toDouble(r.get("Amount")));
            d.put("OhRemarks", str(r.get("OhRemarks")));
            c.overhead.add(d);
        }
    }

    private void buildWages(Ctx c, List<Map<String, Object>> rows) {
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> r = rows.get(i);
            if (!(toInt(r.get("ContractorWagesRateScheduleId")) > 0 || toInt(r.get("OutPutItemId")) > 0)) continue;   // Insert:3041
            int n = i + 1;
            Map<String, Object> d = new LinkedHashMap<>();
            need(!(empty(r.get("OutPutItemId")) || toInt(r.get("OutPutItemId")) == 0), "OutPutItem Field Required in Grid Contractor Wages Row No: " + n);
            d.put("Id", toInt(r.get("Id")));
            d.put("ItemId", toInt(r.get("OutPutItemId")));
            need(!(empty(r.get("ContractorWagesRateScheduleId")) || toInt(r.get("ContractorWagesRateScheduleId")) == 0), "Service Activity Field Required in Grid Contractor Wages Row No: " + n);
            d.put("ContractorWagesRateScheduleId", toInt(r.get("ContractorWagesRateScheduleId")));
            need(!(empty(r.get("ContractorId")) || toInt(r.get("ContractorId")) == 0), "Contractor  Field Required in Grid Contractor Wages Row No: " + n);
            d.put("ContractorId", toInt(r.get("ContractorId")));
            need(!(empty(r.get("Amount")) || toDouble(r.get("Amount")) == 0), "Amount Field Required in Grid Contractor Wages Row No: " + n);
            d.put("Qty", toDouble(r.get("Qty")));
            d.put("Rate", toDouble(r.get("Rate")));
            d.put("Amount", toDouble(r.get("Amount")));
            d.put("Remarks", str(r.get("Remarks")));
            d.put("OutPutItemId", toInt(r.get("OutPutItemId")));
            c.wages.add(d);
        }
    }

    // -------------------------------------------------------------------------------- BLL MakeVoucher (0362:19)

    private Map<String, Object> makeVoucher(Ctx c, Map<String, Object> body) {
        List<Map<String, Object>> items = inRepo.itemGlAccounts(c.u);
        Map<String, Object> vh = ProductionInputRepository.voucherHeadDefaults();
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
        for (Map<String, Object> o : c.output) {
            if (toInt(o.get("ActionTypeId")) == 3) continue;
            need(!items.isEmpty(), "Item GL Account Not Found");
            Map<String, Object> gl = itemGl(items, toInt(o.get("ItemId")));
            if (gl == null) continue;
            String text = " ItemName: " + str(ci(gl, "ItemName")) + " Qty  " + net(toDouble(o.get("NetWeight"))) + " Rate  " + net(toDouble(o.get("Rate")));
            Map<String, Object> v = ProductionInputRepository.voucherDetailDefaults();
            v.put("LineId", lineId);
            v.put("AccountId", toInt(ci(gl, "PurchaseGLAC")));
            v.put("AgainstAccountId", toInt(ci(gl, "PurchaseGLAC")));
            v.put("Comments", text);
            v.put("DebitAmount", toDouble(o.get("Amount")));
            v.put("QtyIn", toDouble(o.get("ItemQty")));
            v.put("WeightIn", toDouble(o.get("NetWeight")));
            v.put("ItemRate", toDouble(o.get("Rate")));
            v.put("ItemAmount", toDouble(o.get("Amount")));
            c.voucherLines.add(v);
            total += toDouble(v.get("DebitAmount"));
            lineId++;
        }
        lineId = 1;
        for (Map<String, Object> o : c.input) {
            if (toInt(o.get("ActionTypeId")) == 3) continue;
            need(!items.isEmpty(), "Item GL Account Not Found");
            Map<String, Object> gl = itemGl(items, toInt(o.get("ItemId")));
            if (gl == null) continue;
            String text = " ItemName: " + str(ci(gl, "ItemName")) + " Qty  " + net(toDouble(o.get("NetWeight"))) + " Rate  " + net(toDouble(o.get("Rate")));
            Map<String, Object> v = ProductionInputRepository.voucherDetailDefaults();
            v.put("LineId", lineId);
            v.put("AccountId", toInt(ci(gl, "PurchaseGLAC")));
            v.put("AgainstAccountId", toInt(ci(gl, "PurchaseGLAC")));
            v.put("Comments", text);
            v.put("CreditAmount", toDouble(o.get("Amount")));
            v.put("QtyOut", toDouble(o.get("ItemQty")));
            v.put("WeightOut", toDouble(o.get("NetWeight")));
            v.put("ItemRate", toDouble(o.get("Rate")));
            v.put("ItemAmount", toDouble(o.get("Amount")));
            c.voucherLines.add(v);
            total += toDouble(v.get("CreditAmount"));
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
            Map<String, Object> v = ProductionInputRepository.voucherDetailDefaults();
            v.put("AccountId", toInt(o.get("OverHeadAccountId")));
            v.put("AgainstAccountId", toInt(o.get("OverHeadAccountId")));
            v.put("Comments", str(o.get("OhRemarks")));
            v.put("CreditAmount", toDouble(o.get("Amount")));
            v.put("QtyOut", toDouble(o.get("Qty")));
            v.put("WeightOut", toDouble(o.get("Qty")));
            v.put("ItemRate", toDouble(o.get("Rate")));
            v.put("ItemAmount", toDouble(o.get("Amount")));
            c.voucherLines.add(v);
        }
        if (!c.wages.isEmpty()) {
            List<Map<String, Object>> suppliers = repo.supplierGl(c.u);
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
                Map<String, Object> v = ProductionInputRepository.voucherDetailDefaults();
                v.put("AccountId", toInt(ci(sup, "GlAccountId")));
                v.put("AgainstAccountId", toInt(ci(sup, "GlAccountId")));
                v.put("Comments", str(o.get("Remarks")));
                v.put("CreditAmount", toDouble(o.get("Amount")));
                v.put("QtyIn", toDouble(o.get("Qty")));
                v.put("WeightIn", toDouble(o.get("Qty")));
                v.put("ItemRate", toDouble(o.get("Rate")));
                v.put("ItemAmount", toDouble(o.get("Amount")));
                v.put("SubsidiaryTypeId", 1);
                v.put("SubsidiaryAccountId", toInt(o.get("ContractorId")));
                v.put("SupplierCustomerId", toInt(o.get("ContractorId")));
                c.voucherLines.add(v);
            }
        }
        vh.put("VoucherAmount", total);
        return vh;
    }

    private static Map<String, Object> itemGl(List<Map<String, Object>> items, int itemId) {
        for (Map<String, Object> g : items) if (toInt(ci(g, "Id")) == itemId) return g;
        return null;
    }

    // -------------------------------------------------------------------------------- DAL SetData (0297:16)

    private Integer setData(Ctx c, Map<String, Object> header) {
        int num = repo.setHeader(!c.update, header);
        if (num > 0) c.headerId = num;
        else num = c.headerId;

        int line = 1;
        for (Map<String, Object> d : c.output) {
            d.put("ConversionHeaderId", c.headerId);
            d.put("LineId", line);
            d.put("Id", repo.setOutput(model(d, OUTPUT_PARAMS)));
            line++;
        }
        line = 1;
        for (Map<String, Object> d : c.input) {
            d.put("ConversionHeaderId", c.headerId);
            d.put("LineId", line);
            d.put("Id", repo.setInput(model(d, INPUT_PARAMS)));
            line++;
        }
        for (Map<String, Object> d : c.overhead) {
            d.put("ConversionHeaderId", c.headerId);
            d.put("Id", repo.setOverHead(model(d, OVERHEAD_PARAMS)));
        }
        for (Map<String, Object> d : c.wages) {
            d.put("ConversionHeaderId", c.headerId);
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
                    se.put("VarientId", toInt(d.get("ItemAttributeVarientId")));
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
                    se.put("PrdJobOrderNo", 0);
                    se.put("BranchesId", c.branch);
                    se.put("RefDocumentTypeId", DOC_TYPE);
                    se.put("EntryUser", c.update ? 0 : c.userId);
                    se.put("ModifyUser", c.update ? c.userId : 0);
                    se.put("CalcType", "Weight");
                    int lid = toInt(se.get("LineId"));
                    for (Map<String, Object> d : c.input) {
                        if (toInt(d.get("LineId")) == lid && lid > 0) {
                            se.put("RefDocIdNo", toInt(d.get("ConversionHeaderId")));
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

        for (Map<String, Object> d : c.input) {                                                            // every row, removed ones included (0297:150)
            LinkedHashMap<String, Object> p = ProductionInputRepository.params();
            p.put("@OrganizationId", c.org);
            p.put("@CompanyId", c.company);
            p.put("@DocumentTypeId", DOC_TYPE);
            p.put("@DocDate", c.docDate);
            p.put("@ItemId", toInt(d.get("ItemId")));
            p.put("@WarehouseId", toInt(d.get("WareHouseId")));
            if (toInt(d.get("ItemAttributeVarientId")) > 0) p.put("@VarientId", toInt(d.get("ItemAttributeVarientId")));
            p.put("@Qty", toDouble(d.get("ItemQty")));
            inRepo.command("dbo.USP_StockValidationConcrete", p);
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

    /** CommonServices.FIFOImplementionForConcrete (DAL 0205:1148). */
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
        List<Map<String, Object>> layers = repo.fifoLayers(c.u, toInt(d.get("ItemId")), c.docDate, 0, toInt(d.get("WareHouseId")),
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

    // -------------------------------------------------------------------------------- procedure parameter filters (procdure_index.csv, [pcc] schema)

    private static final Set<String> OUTPUT_PARAMS = setOf("Id", "EntryTypeId", "ConversionHeaderId", "WareHouseId", "ItemAttributeVarientId", "VarientEquivalent", "ItemId",
            "ItemUomId", "VarientQty", "ItemQty", "NetWeight", "Rate", "RateUomId", "Amount", "Remarks", "ActionTypeId", "LineId");
    private static final Set<String> INPUT_PARAMS = setOf("Id", "ConversionHeaderId", "ItemId", "ItemUomId", "ItemQty", "NetWeight", "Rate", "RateUomId", "Amount", "Remarks",
            "ActionTypeId", "LineId", "VarientEquivalent", "ItemAttributeVarientId", "VarientQty", "WareHouseId");
    private static final Set<String> OVERHEAD_PARAMS = setOf("Id", "ConversionHeaderId", "OverHeadTypeId", "OverHeadAccountId", "PercentOfTotalInput", "Qty", "Rate", "BomQty",
            "Amount", "OhRemarks");
    private static final Set<String> WAGES_PARAMS = setOf("Id", "ConversionHeaderId", "ContractorId", "ContractorWagesRateScheduleId", "ItemId", "Qty", "Rate", "Amount",
            "Remarks", "OutPutItemId");

    private static Set<String> setOf(String... a) { return new LinkedHashSet<>(java.util.Arrays.asList(a)); }

    /**
     * GenericProvider.SetProc over a detail model: value-type properties go out at their CLR value (0 when never set), reference-type ones only when set.
     * Every int / decimal property of the four detail models is listed in the set, so a missing key is sent as 0.
     */
    private static Map<String, Object> model(Map<String, Object> d, Set<String> declared) {
        Map<String, Object> p = new LinkedHashMap<>();
        for (String k : declared) {
            if (k.equals("Remarks") || k.equals("OhRemarks")) {
                if (d.get(k) != null) p.put(k, d.get(k));
            } else {
                Object v = d.get(k);
                p.put(k, v == null ? Integer.valueOf(0) : v);
            }
        }
        return p;
    }
}
