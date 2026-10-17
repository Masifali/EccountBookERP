package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.Engr795ProductionRepository;
import com.mst.repositories.ProductionInputRepository;
import com.mst.repositories.ProductionOutputRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.PlatformTransactionManager;
import org.springframework.transaction.support.TransactionTemplate;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

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
 * Screen 795 "Production (Casting)" - Architecture.WinApp.Mfg.frmProductionEngr (.cs 5,790 lines; logic lines 1-4,006, the rest is the designer).
 * Rights key base.Name "frmProductionEngr", DocumentTypeId 1670, schema [Mfg].
 *
 * <pre>
 *   Route     /production/engr-production      API /production/engr-production/api
 *   Four detail grids (Input, Output, Scrap, Over Head) + the Total/Gain-Loss summary, BOM load, work-order output loader, settlement voucher, history with four sub-grids.
 * </pre>
 *
 * SAVE (BLL 0366 Save + DAL 0319 SetData, one transaction): [Mfg].[USP_ProductionHeader_InsertAndUpdate] -> every output/scrap row (removed rows first, LineId 1..n
 * over the whole list) -> every input row (removed first, SortNo 1..n, LineId stays 0 as in the DAL) -> over head rows -> Sp_InventoryTransactions_GetALLMethod ->
 * Sp_InventoryStockEvalautionDetail_Update -> settled: voucher (MakeVoucher 0366:19) Sp_VoucherHead_Insert|Update + Sp_VoucherDetail_Insert + USP_VoucherBalanceCheck;
 * not settled: usp_VoucherDeleteByVoucherId when a voucher exists.
 *
 * KNOWN DIFFERENCES / GAPS
 * <ol>
 *  <li>Attachments (Attachment dialog, DMS copy, Proc_DMSAttachments_Insert, FormHelper.* attachment helpers) are not ported; AttachmentsValues / CustomAttachmentsValues
 *      are sent as "" as the desktop does when none are added. The history "NoOfAttachments" link opens nothing.</li>
 *  <li>SaveAs (history SaveAs button, Ctrl+F12), the shortcut-keys dialog and the desktop grid layout files (ctrlGrdBar, CommonServices.GetGridLayout) are not ported.</li>
 *  <li>Item.AllItemsBindForEngrWithModalOnly (dtitemOutput) is loaded by the desktop but nothing reads it, so the call is not made here.</li>
 *  <li>The input row F1 item picker of the desktop is bound to the SCRAP item list (dtitemScrap, grdInput_KeyDown:3533); kept as is.</li>
 *  <li>Loading a BOM whose input list is empty makes the desktop call grdInput.ClearStructure() (the grid goes blank); the page leaves the grid as it is.</li>
 *  <li>Numbers are double on the page and on the server (the desktop uses decimal in places); C# decimal.ToString() in the validation messages is approximated with the
 *      15-significant-digit form.</li>
 *  <li>Nothing here has been run against the database or a browser.</li>
 * </ol>
 */
@Service
public class Engr795ProductionService {

    public static final String SCREEN = "frmProductionEngr";
    public static final int DOC_TYPE = 1670;

    private final Prod273Support db;
    private final Engr795ProductionRepository repo;
    private final ProductionInputRepository inRepo;
    private final PlatformTransactionManager tm;

    public Engr795ProductionService(Prod273Support db, Engr795ProductionRepository repo, ProductionInputRepository inRepo, PlatformTransactionManager tm) {
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

    private static void need(boolean ok, String message) { if (!ok) throw new IllegalArgumentException(message); }

    private void right(String key, String message) {
        if (!db.has(SCREEN, key)) throw new IllegalStateException(message);
    }

    private static int userId(UserAccount u) { return u.getId() == null ? 0 : u.getId(); }

    private static double d(Object v) { return toDouble(v); }

    /** C# decimal / double.ToString() in a message (15 significant digits, no exponent). */
    private static String cs(double v) { return ProductionOutputRepository.csDouble(v); }

    private static Object num(Object v) {
        if (v instanceof BigDecimal) return ((BigDecimal) v).doubleValue();
        return v;
    }

    private static Map<String, Object> m() { return new LinkedHashMap<>(); }

    // ====================================================================================== load (InitializeComponentMethod:454, BtnRefresh_Click:2361)

    public Map<String, Object> lookups() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
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
        Map<String, Object> out = new LinkedHashMap<>();
        int inputGroup = toInt(db.config(u, "DefaultProductionInputItemCustomGroupId"));
        int scrapGroup = toInt(db.config(u, "DefaultProductionScrapItemCustomGroupId"));
        int overheadGroup = toInt(db.config(u, "DefaultProductionOverHeadCustomGroupId"));
        int castingWarehouse = toInt(db.config(u, "DefaultCastingWarehouse"));
        int stageId = toInt(db.config(u, "DefaultProductionStageId"));
        out.put("defaultCastingWarehouseId", castingWarehouse);
        out.put("amountDecimals", toInt(db.config(u, "DefaultNoofDecimalPointsForAmount")));
        out.put("defaultProductionStageId", stageId);
        List<Map<String, Object>> all = repo.allLookups();
        String stageName = "";
        List<Map<String, Object>> casting = new ArrayList<>();
        for (Map<String, Object> r : all) {
            if (toInt(ci(r, "Id")) == stageId) { stageName = str(ci(r, "LookupName")); }
            if (toInt(ci(r, "LookUpTypeId")) == 1) casting.add(pair(toInt(ci(r, "Id")), str(ci(r, "LookupName"))));     // BindCastingType:632
        }
        out.put("defaultProductionStageName", stageName);
        out.put("castingTypes", casting);
        List<Map<String, Object>> heat = new ArrayList<>();
        for (Map<String, Object> r : repo.heatCodes()) heat.add(pair(toInt(ci(r, "Id")), str(ci(r, "type"))));
        out.put("heatCodes", heat);
        out.put("jobLots", Prod273Support.project(repo.jobLots(u, branch), "Id", "JobLotDescription"));
        boolean branchFeature = repo.erpFeature(u, 11);
        out.put("warehouses", Prod273Support.project(repo.warehouses(u, branchFeature ? branch : 0), "Id", "WareHouseName"));
        out.put("uoms", db.allUoms(u));                                                                  // dtAllUom = UOMSchedule.Getall (ItemId, Id, UOMCode, Equivalent)
        List<Map<String, Object>> inputItems = new ArrayList<>(), scrapItems = new ArrayList<>();
        for (Map<String, Object> r : repo.itemsWithCustomGroup(u, branch)) {                              // BindItemInputScrap:822
            Map<String, Object> o = m();
            o.put("Id", toInt(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("ItemCode", str(ci(r, "ItemCode")));
            int g = toInt(ci(r, "CustomGroupId"));
            if (inputGroup == g) inputItems.add(o);
            if (scrapGroup == g) scrapItems.add(o);
        }
        out.put("inputItems", inputItems);
        out.put("scrapItems", scrapItems);
        List<Map<String, Object>> accounts = new ArrayList<>();
        for (Map<String, Object> r : repo.accountsWithCustomGroup(u)) {                                   // BindOverHeadAccounts:937
            if (truthy(ci(r, "ExistsInCustomGroup")) && overheadGroup == toInt(ci(r, "CustomGroupId"))) {
                Map<String, Object> o = m();
                o.put("Id", toInt(ci(r, "ChartOfAccountId")));
                o.put("AccountTitle", str(ci(r, "AccountTitle")));
                o.put("AccountCode", str(ci(r, "AccountCode")));
                accounts.add(o);
            }
        }
        out.put("overheadAccounts", accounts);
        out.put("boms", boms(0));
        return out;
    }

    /** GenerateCode:667. */
    public String docNo() {
        UserAccount u = db.user();
        List<Map<String, Object>> r = repo.generateCode(u, db.branchId(u), DOC_TYPE, db.financialYearId());
        return r.isEmpty() ? "0" : String.valueOf(toInt(ci(r.get(0), "DocNo")));
    }

    /** GetAllBomHeader:742 - every BOM with its CastingTypeId (the page filters by the casting type, BindBomHeader:755). */
    public List<Map<String, Object>> boms(int recId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.boms(db.user(), recId)) {
            Map<String, Object> o = m();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("BomName", str(ci(r, "BomName")));
            o.put("EffectedDate", day(ci(r, "EffectedDate")));
            o.put("CastingTypeId", toInt(ci(r, "CastingTypeId")));
            out.add(o);
        }
        return out;
    }

    /** FillInputDetailForMultiBom:1043 - the BOM's input list shaped as input grid rows (WarehouseId is chosen by the page). */
    public List<Map<String, Object>> bomInputs(int bomId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.bomInputs(bomId)) {
            Map<String, Object> o = m();
            o.put("Id", 0);
            o.put("BomHeaderId", toInt(ci(r, "BomHeaderId")));
            o.put("BomInputDetailId", toInt(ci(r, "BomInputDetailId")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("PackUomId", toInt(ci(r, "ItemUomId")));
            o.put("PackUom", str(ci(r, "PackUom")));
            o.put("PackEquivalent", d(ci(r, "PackUomEquivalent")));
            o.put("NetWeight", d(ci(r, "WeightKg")));
            o.put("Rate", d(ci(r, "Rate")));
            o.put("RateUomId", toInt(ci(r, "RateUomId")));
            o.put("RateUom", str(ci(r, "RateUom")));
            o.put("RateEquivalent", d(ci(r, "RateUomEquivalent")));
            o.put("Amount", d(ci(r, "Amount")));
            out.add(o);
        }
        return out;
    }

    /** BtnLoadOutPut_Click:3943 / grdOutPut_KeyDown F1:3427 - ProductionHeader.WorkOrderSemiFinishForProductionOutput. */
    public List<Map<String, Object>> workOrders(int castingTypeId, int recId) {
        need(castingTypeId != 0, "Casting type Field Required");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.workOrderSemiFinish(db.user(), castingTypeId, recId)) {
            Map<String, Object> o = m();
            for (String k : new String[] { "WorkOrderId", "workOrderItemDetailId", "ItemId", "UomId", "ItemVariantId", "CastingTypeId" }) o.put(k, toInt(ci(r, k)));
            for (String k : new String[] { "WorkOrderNo", "ItemName", "ItemCode", "ItemUom", "ModelDescription", "CastingType" }) o.put(k, str(ci(r, k)));
            for (String k : new String[] { "ItemWeight", "ItemEquivalent", "Qty", "UsedQtyInProduction", "BalQty" }) o.put(k, d(ci(r, k)));
            out.add(o);
        }
        return out;
    }

    // ====================================================================================== history / read / delete

    /** HistoryGridFill:2615 - the 21 columns of the desktop history table. */
    public List<Map<String, Object>> history(String fromDate, String toDate, int fromDocNo, int toDocNo) {
        UserAccount u = db.user();
        boolean viewAll = db.has(SCREEN, "viewAll");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, db.branchId(u), DOC_TYPE, db.financialYearId(), pickerValue(fromDate), pickerValue(toDate),
                fromDocNo, toDocNo, viewAll, userId(u))) {
            Map<String, Object> o = m();
            Object in = ci(r, "inTime"), outT = ci(r, "outTime");
            o.put("Id", toInt(ci(r, "Id")));
            o.put("DocDate", day(ci(r, "DocDate")));
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("DocumentTypeId", toInt(ci(r, "DocumentTypeId")));
            o.put("ProductionNo", str(ci(r, "ProductionNo")));
            o.put("BatchCode", str(ci(r, "batchCode")));
            o.put("heatCodeId", str(ci(r, "heatCodeId")));
            o.put("heatCode", str(ci(r, "heatCode")));
            o.put("InTime", stamp(in));
            o.put("OutTime", stamp(outT));
            o.put("TotalTime", (in instanceof Timestamp && outT instanceof Timestamp) ? timeSpan((Timestamp) in, (Timestamp) outT) : "");
            o.put("CastingType", str(ci(r, "CastingType")));
            o.put("BomHeaderId", toInt(ci(r, "bomHeaderId")));
            o.put("BomNo", str(ci(r, "BomNo")));
            o.put("IsSettled", str(ci(r, "IsSettled")));
            o.put("SettlementStatus", toInt(ci(r, "IsSettled")) == 1 ? "Settled" : "Not Settled");
            o.put("RemarksHeader", str(ci(r, "RemarksHeader")));
            o.put("EntryDate", stamp(ci(r, "EntryDate")));
            o.put("EntryUser", str(ci(r, "EntryUserName")));
            o.put("ModifyDate", stamp(ci(r, "ModifyDate")));
            o.put("ModifyUser", str(ci(r, "ModifyUserName")));
            o.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    /** System.TimeSpan.ToString() of out - in. */
    static String timeSpan(Timestamp from, Timestamp to) {
        long ms = to.getTime() - from.getTime();
        boolean neg = ms < 0;
        ms = Math.abs(ms);
        long days = ms / 86400000L, h = (ms / 3600000L) % 24, mi = (ms / 60000L) % 60, s = (ms / 1000L) % 60, f = ms % 1000L;
        String t = String.format("%02d:%02d:%02d", h, mi, s) + (f != 0 ? String.format(".%03d0000", f) : "");
        return (neg ? "-" : "") + (days > 0 ? days + "." : "") + t;
    }

    /** ReadById:2168 / BindAllDetailHistories:2876 - the header and the four detail lists, shaped as the page's grid rows. */
    public Map<String, Object> read(int id) {
        List<Map<String, Object>> hs = repo.readHeader(id);
        if (hs.isEmpty()) throw new IllegalArgumentException("Record not found");
        Map<String, Object> h = hs.get(0);
        Map<String, Object> out = m();
        out.put("id", toInt(ci(h, "ProductionHeaderId")));
        out.put("docNo", toInt(ci(h, "DocNo")));
        out.put("docDate", day(ci(h, "DocDate")));
        out.put("productionNo", str(ci(h, "ProductionNo")));
        out.put("jobLotId", toInt(ci(h, "JobLotId")));
        out.put("heatCode", toInt(ci(h, "heatCode")));
        out.put("batchCode", str(ci(h, "batchCode")));
        out.put("inTime", stamp(ci(h, "inTime")));
        out.put("outTime", stamp(ci(h, "outTime")));
        out.put("castingTypeId", toInt(ci(h, "CastingTypeId")));
        out.put("bomHeaderId", toInt(ci(h, "BomHeaderId")));
        out.put("remarks", str(ci(h, "RemarksHeader")));
        out.put("isSettled", toInt(ci(h, "IsSettled")) == 1);
        out.put("isApproved", truthy(ci(h, "IsApproved")));

        List<Map<String, Object>> output = new ArrayList<>(), scrap = new ArrayList<>();
        for (Map<String, Object> r : repo.readOutputs(id)) {
            Map<String, Object> o = m();
            o.put("Id", toInt(ci(r, "ProductionOutPutDetailId")));
            o.put("WarehouseId", toInt(ci(r, "WareHouseId")));
            o.put("WarehouseName", str(ci(r, "WareHouseName")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("PackUomId", toInt(ci(r, "ItemUomId")));
            o.put("PackUom", str(ci(r, "PackUom")));
            o.put("PackEquivalent", d(ci(r, "PackEquivalent")));
            o.put("NetWeight", d(ci(r, "NetWeight")));
            o.put("Rate", d(ci(r, "ItemRate")));
            o.put("RateUomId", toInt(ci(r, "RateUomId")));
            o.put("RateUom", str(ci(r, "RateUom")));
            o.put("RateEquivalent", d(ci(r, "RateEquivalent")));
            o.put("Amount", d(ci(r, "Amount")));
            if (toInt(ci(r, "OutPutTypeId")) == 1) {
                o.put("WorkOrderId", toInt(ci(r, "WorkOrderId")));
                o.put("workOrderItemDetailId", toInt(ci(r, "workOrderItemDetailId")));
                o.put("WorkOrderNo", str(ci(r, "WorkOrderNo")));
                o.put("ModalId", toInt(ci(r, "ItemVariantId")));
                o.put("ModalDescription", str(ci(r, "VarientDescription")));
                o.put("ProcessStatusId", toInt(ci(r, "ProcessStatusId")));
                o.put("ProcessStatus", str(ci(r, "ProcessStatus")));
                o.put("BalQty", d(ci(r, "WorkOrderBalQty")));
                o.put("WeightPerQty", d(ci(r, "ItemWeight")));
                o.put("CastingWeight", d(ci(r, "NetWeight")) * d(ci(r, "ItemWeight")));
                output.add(o);
            } else {
                scrap.add(o);
            }
        }
        out.put("output", output);
        out.put("scrap", scrap);

        List<Map<String, Object>> input = new ArrayList<>();
        for (Map<String, Object> r : repo.readInputs(id)) {
            Map<String, Object> o = m();
            o.put("Id", toInt(ci(r, "productionInputDetailId")));
            o.put("BomHeaderId", toInt(ci(r, "BomHeaderId")));
            o.put("BomInputDetailId", toInt(ci(r, "BomInputDetailId")));
            o.put("WarehouseId", toInt(ci(r, "WareHouseId")));
            o.put("WarehouseName", str(ci(r, "WareHouseName")));
            o.put("ItemId", toInt(ci(r, "ItemId")));
            o.put("ItemName", str(ci(r, "ItemName")));
            o.put("PackUomId", toInt(ci(r, "ItemUomId")));
            o.put("PackUom", str(ci(r, "PackUom")));
            o.put("PackEquivalent", d(ci(r, "PackEquivalent")));
            o.put("NetWeight", d(ci(r, "Weight")));
            o.put("Rate", d(ci(r, "ItemRate")));
            o.put("RateUomId", toInt(ci(r, "RateUomId")));
            o.put("RateUom", str(ci(r, "RateUom")));
            o.put("RateEquivalent", d(ci(r, "RateEquivalent")));
            o.put("Amount", d(ci(r, "Amount")));
            input.add(o);
        }
        out.put("input", input);

        List<Map<String, Object>> oh = new ArrayList<>();
        for (Map<String, Object> r : repo.readOverheads(id)) {
            Map<String, Object> o = m();
            o.put("Id", toInt(ci(r, "ProductionOverHeadsDetailId")));
            o.put("OverHeadAccountId", toInt(ci(r, "OverHeadAccountId")));
            o.put("OverHeadAccount", str(ci(r, "OverHeadAccountTitle")));
            o.put("PercentOfTotalInput", d(ci(r, "PercentOfTotalInput")));
            o.put("Qty", d(ci(r, "Qty")));
            o.put("Rate", d(ci(r, "Rate")));
            o.put("Amount", d(ci(r, "Amount")));
            o.put("OhRemarks", str(ci(r, "Remarks")));
            oh.add(o);
        }
        out.put("overhead", oh);
        return out;
    }

    /** VoucherReprot:3176 - CommonServices.VoucherHeadIdGet(Id, 1670). */
    public Map<String, Object> voucherHead(int id) {
        Map<String, Object> o = m();
        o.put("voucherHeadId", inRepo.voucherHeadId(db.user(), DOC_TYPE, id));
        return o;
    }

    /** BtnDelete_Click:2259 - BLL 0366 DeleteByID. */
    public Map<String, Object> delete(int id) {
        right("delete", "You do not have the Delete right on Production (Casting).");                       // BtnDelete.Enabled = DoHaveCanDelete (:500)
        need(id > 0, "No record found to Delete");
        UserAccount u = db.user();
        new TransactionTemplate(tm).execute(st -> { repo.deleteById(u, DOC_TYPE, userId(u), id); return null; });
        Map<String, Object> o = m();
        o.put("success", true);
        o.put("message", "Delete Record Successfully");
        return o;
    }

    // ====================================================================================== save (Insert:1820)

    /** Working state of one Insert() call. */
    private static final class Ctx {
        UserAccount u;
        int org, company, branch, fy, uid, recId, headerId, docNo;
        boolean update, settled;
        Timestamp docDate;
        final List<Map<String, Object>> output = new ArrayList<>();       // ProductionOutPutDetailslist (model property names)
        final List<Map<String, Object>> input = new ArrayList<>();        // ProductionInputDetailslist
        final List<Map<String, Object>> overhead = new ArrayList<>();     // ProductionOverHeadsDetailList
        Map<String, Object> voucher;
        final List<Map<String, Object>> voucherLines = new ArrayList<>();
    }

    public Map<String, Object> save(Map<String, Object> body) {
        Ctx c = new Ctx();
        c.u = db.user();
        c.recId = toInt(body.get("id"));
        c.update = c.recId > 0;
        right(c.update ? "update" : "save", c.update ? "You do not have the Update right on Production (Casting)." : "You do not have the Save right on Production (Casting).");

        /* FormValidation:980 */
        String docNoText = str(body.get("docNo")).trim();
        need(!(toInt(docNoText) == 0 || docNoText.isEmpty() || docNoText.equals("0")), "DocNo Field is Required");
        String productionNo = str(body.get("productionNo")).trim();
        need(!(productionNo.isEmpty() || productionNo.equals("0")), "Production No Field is Required");
        String batchCode = str(body.get("batchCode")).trim();
        int jobLotId = toInt(body.get("jobLotId"));
        need(!(batchCode.isEmpty() || batchCode.equals("0")), "Batch Code Field is Required");
        need(jobLotId != 0, "Batch Code Field is Required");
        int heatCode = toInt(body.get("heatCode"));
        need(heatCode != 0, "Heat Code Field is Required");
        Timestamp inTime = Prod273Support.ts(body.get("inTime")), outTime = Prod273Support.ts(body.get("outTime"));
        need(inTime != null && outTime != null && inTime.before(outTime), "In time cannot be equal to or greater than out time.");

        List<Map<String, Object>> outRows = list(body.get("output")), scrapRows = list(body.get("scrap")), inRows = list(body.get("input")), ohRows = list(body.get("overhead"));
        need(!inRows.isEmpty(), "Grid Record Not Found,Input Grid Should have a row");                    // :1838 (grdInput.RowCount == 0)

        c.org = c.u.getOrganizationId();
        c.company = c.u.getCompanyId();
        c.branch = db.branchId(c.u);
        c.fy = db.financialYearId();
        c.uid = userId(c.u);
        c.docNo = toInt(docNoText);
        c.docDate = pickerValue(body.get("docDate"));
        c.settled = truthy(body.get("settled"));
        int castingTypeId = toInt(body.get("castingTypeId"));
        String castingTypeText = str(body.get("castingTypeText"));

        if (c.settled) {                                                                                      // :1856
            double sumOutput = 0, sumInput = 0, sumOh = 0, sumScrap = 0;
            for (Map<String, Object> r : outRows) sumOutput += d(r.get("Amount"));
            for (Map<String, Object> r : inRows) sumInput += d(r.get("Amount"));
            for (Map<String, Object> r : ohRows) sumOh += d(r.get("Amount"));
            for (Map<String, Object> r : scrapRows) sumScrap += d(r.get("Amount"));
            double autoRated = sumInput + sumOh - sumScrap;                                                   // Calculation(): summary row Id 5
            if (Math.rint(autoRated * 100) / 100 != Math.rint(sumOutput * 100) / 100) {
                throw new IllegalArgumentException("Net Vale " + cs(autoRated) + " In Header and Total OutPut Amount " + cs(sumOutput) + " are not equal");
            }
        }

        /* output + scrap: removed rows first (RecId > 0), then the grid rows */
        if (c.update) {
            for (Map<String, Object> r : list(body.get("removedOutput"))) c.output.add(removedOutput(r));
        }
        int rowNo = 0;
        for (Map<String, Object> r : outRows) {
            rowNo++;
            Map<String, Object> det = outputDetail(r, c.recId != 0 ? toInt(r.get("Id")) : 0, 1);
            String at = " In Row #" + rowNo + " In OutPut Grid";
            if (toInt(det.get("WorkOrderId")) == 0) throw new IllegalArgumentException("WorkOrder Required" + at);
            if (toInt(det.get("WareHouseId")) == 0) throw new IllegalArgumentException("WareHouse Required" + at);
            if (toInt(det.get("ItemId")) == 0) throw new IllegalArgumentException("Item Required" + at);
            if (toInt(det.get("ItemUomId")) == 0) throw new IllegalArgumentException("Uom Required" + at);
            if (toInt(det.get("ProcessStatusId")) == 0) throw new IllegalArgumentException("Production Stage Required" + at);
            if (d(det.get("NetWeight")) == 0) throw new IllegalArgumentException("Qty Required" + at);
            if (d(det.get("ItemRate")) == 0) throw new IllegalArgumentException("Rate Required" + at);
            if (toInt(det.get("RateUomId")) == 0) throw new IllegalArgumentException("RateUom Required" + at);
            if (d(det.get("Amount")) == 0) throw new IllegalArgumentException("Amount Required" + at);
            double others = 0;
            for (Map<String, Object> r2 : outRows) {
                if (toInt(r2.get("WorkOrderId")) > 0 && toInt(r2.get("Id")) != toInt(det.get("ProductionOutPutDetailId")) && toInt(r2.get("WorkOrderId")) == toInt(det.get("WorkOrderId"))) {
                    others += d(r2.get("NetWeight"));
                }
            }
            double toVerify = d(r.get("BalQty")) - others;
            if (toInt(det.get("WorkOrderId")) > 0 && d(det.get("NetWeight")) > toVerify) {
                throw new IllegalArgumentException("Qty '" + cs(d(det.get("NetWeight"))) + "' Can not Greater than Bal Qty '" + cs(toVerify) + "' of Selected WorkOder "
                        + "against CastingType '" + castingTypeText + "'" + at.replace(" In Row #", " In Row # "));
            }
            c.output.add(det);
        }
        rowNo = 0;
        for (Map<String, Object> r : scrapRows) {
            rowNo++;
            if (toInt(r.get("ItemId")) <= 0) continue;                                                        // :1972
            Map<String, Object> det = outputDetail(r, c.recId != 0 ? toInt(r.get("Id")) : 0, 2);
            det.put("WorkOrderId", 0);
            det.put("workOrderItemDetailId", 0);
            det.put("ItemVariantId", 0);
            det.put("ProcessStatusId", 0);
            String at = " In Row #" + rowNo + " In Scrap Grid";
            if (toInt(det.get("WareHouseId")) == 0) throw new IllegalArgumentException("WareHouse Required" + at);
            if (toInt(det.get("ItemId")) == 0) throw new IllegalArgumentException("Item Required" + at);
            if (toInt(det.get("ItemUomId")) == 0) throw new IllegalArgumentException("Uom Required" + at);
            if (d(det.get("NetWeight")) == 0) throw new IllegalArgumentException("Weight Required" + at);
            if (d(det.get("ItemRate")) == 0) throw new IllegalArgumentException("Rate Required" + at);
            if (toInt(det.get("RateUomId")) == 0) throw new IllegalArgumentException("RateUom Required" + at);
            if (d(det.get("Amount")) == 0) throw new IllegalArgumentException("Amount Required" + at);
            c.output.add(det);
        }

        /* input: removed rows first, then the grid rows */
        if (c.update) {
            for (Map<String, Object> r : list(body.get("removedInput"))) c.input.add(removedInput(r, heatCode, inTime, outTime));
        }
        rowNo = 0;
        for (Map<String, Object> r : inRows) {
            rowNo++;
            Map<String, Object> det = new LinkedHashMap<>();
            int rid = c.recId != 0 ? toInt(r.get("Id")) : 0;
            det.put("productionInputDetailId", rid);
            det.put("ActionTypeId", rid <= 0 ? 1 : 2);
            det.put("BomHeaderId", toInt(r.get("BomHeaderId")));
            det.put("BomInputDetailId", toInt(r.get("BomInputDetailId")));
            det.put("WareHouseId", toInt(r.get("WarehouseId")));
            det.put("ItemId", toInt(r.get("ItemId")));
            det.put("ItemUomId", toInt(r.get("PackUomId")));
            det.put("Qty", d(r.get("NetWeight")));
            det.put("Weight", d(r.get("NetWeight")));
            det.put("ItemRate", d(r.get("Rate")));
            det.put("RateUomId", toInt(r.get("RateUomId")));
            det.put("Amount", d(r.get("Amount")));
            det.put("HeatCode", heatCode);
            det.put("timeStart", inTime);
            det.put("timeOut", outTime);
            det.put("RemarksDetail", "");
            String at = " In Row #" + rowNo + " In Input Grid";
            if (toInt(det.get("WareHouseId")) == 0) throw new IllegalArgumentException("WareHouse Required" + at);
            if (toInt(det.get("ItemId")) == 0) throw new IllegalArgumentException("Item Required" + at);
            if (toInt(det.get("ItemUomId")) == 0) throw new IllegalArgumentException("Uom Required" + at);
            if (d(det.get("Weight")) == 0) throw new IllegalArgumentException("Weight Required" + at);
            if (d(det.get("ItemRate")) == 0) throw new IllegalArgumentException("Rate Required" + at);
            if (toInt(det.get("RateUomId")) == 0) throw new IllegalArgumentException("RateUom Required" + at);
            if (d(det.get("Amount")) == 0) throw new IllegalArgumentException("Amount Required" + at);
            c.input.add(det);
        }
        need(!c.input.isEmpty(), "Grid Record Not Found,Input Grid Should have a row");                    // :2080

        /* over head: only rows with an account */
        rowNo = 0;
        Map<Integer, String> accountTitles = null;
        for (Map<String, Object> r : ohRows) {
            rowNo++;
            if (toInt(r.get("OverHeadAccountId")) <= 0) continue;
            Map<String, Object> det = new LinkedHashMap<>();
            det.put("ProductionOverHeadsDetailId", toInt(r.get("Id")));
            det.put("OverHeadAccountId", toInt(r.get("OverHeadAccountId")));
            det.put("PercentOfTotalInput", d(r.get("PercentOfTotalInput")));
            det.put("Qty", d(r.get("Qty")));
            det.put("Rate", d(r.get("Rate")));
            det.put("Amount", d(r.get("Amount")));
            det.put("Remarks", str(r.get("OhRemarks")));
            if (d(det.get("Amount")) == 0) throw new IllegalArgumentException("Amount Field Required in Grid Over Head Row No: " + rowNo);
            if (accountTitles == null) {
                accountTitles = new LinkedHashMap<>();
                for (Map<String, Object> a : repo.accountsWithCustomGroup(c.u)) accountTitles.put(toInt(ci(a, "ChartOfAccountId")), str(ci(a, "AccountTitle")));
            }
            det.put("OverHeadAccountTitle", accountTitles.getOrDefault(toInt(det.get("OverHeadAccountId")), ""));
            c.overhead.add(det);
        }

        /* attachments: not ported (AttachmentsList / DeleteAttachmentsList stay empty, AttachmentsValues "") */
        if (c.update) {                                                                                       // btnUpdate has no approved check on this form; ReadById shows Update only
            List<Map<String, Object>> hs = repo.readHeader(c.recId);
            need(!hs.isEmpty(), "Record not update because Id not found");
        }

        Timestamp now = Prod273Support.now();
        Map<String, Object> h = new LinkedHashMap<>();                                                        // BLL 0366 Save:151 + Insert:1867
        h.put("ProductionHeaderId", c.update ? c.recId : 0);
        h.put("DocumentTypeId", DOC_TYPE);
        h.put("DocNo", c.docNo);
        h.put("DocDate", c.docDate);
        h.put("RemarksHeader", str(body.get("remarks")).trim());
        h.put("ProductionNo", productionNo);
        h.put("OrganizationId", c.org);
        h.put("CompanyId", c.company);
        h.put("BranchesId", c.branch);
        h.put("ProjectsId", 0);
        h.put("FinancialYearId", c.fy);
        h.put("EntryUserId", c.uid);
        h.put("EntryDate", now);
        h.put("ModifyUserId", c.uid);
        h.put("ModifyDate", now);
        h.put("IsApproved", false);
        h.put("ApprovedUserId", c.uid);
        h.put("ApprovedDate", now);
        h.put("ActionId", c.update ? 2 : 1);
        h.put("AttachmentsValues", "");
        h.put("CustomAttachmentsValues", "");
        h.put("batchCode", batchCode);
        h.put("heatCode", heatCode);
        h.put("inTime", inTime);
        h.put("outTime", outTime);
        h.put("bomHeaderId", toInt(body.get("bomHeaderId")));
        h.put("CastingTypeId", castingTypeId);
        h.put("IsSettled", c.settled ? 1 : 0);
        h.put("JobLotId", jobLotId);

        if (c.settled) c.voucher = makeVoucher(c, h);                                                         // BLL Save:155 (before the DAL, ProductionHeaderId is still 0 on insert)

        int newId = new TransactionTemplate(tm).execute(st -> setData(c, h));
        return Prod273Support.ok((c.update ? "Record Update Successfully [" : "Record Save Successfully [") + c.docNo + "]", newId, c.docNo);
    }

    private Map<String, Object> outputDetail(Map<String, Object> r, int id, int outPutTypeId) {
        Map<String, Object> det = new LinkedHashMap<>();
        det.put("ProductionOutPutDetailId", id);
        det.put("ActionTypeId", id <= 0 ? 1 : 2);
        det.put("OutPutTypeId", outPutTypeId);
        det.put("SaleOrderId", 0);
        det.put("WorkOrderId", toInt(r.get("WorkOrderId")));
        det.put("workOrderItemDetailId", toInt(r.get("workOrderItemDetailId")));
        det.put("WareHouseId", toInt(r.get("WarehouseId")));
        det.put("ItemId", toInt(r.get("ItemId")));
        det.put("JobLotId", 0);
        det.put("ItemUomId", toInt(r.get("PackUomId")));
        det.put("ItemVariantId", toInt(r.get("ModalId")));
        det.put("ProcessStatusId", toInt(r.get("ProcessStatusId")));
        det.put("ItemQty", d(r.get("NetWeight")));
        det.put("NetWeight", d(r.get("NetWeight")));
        det.put("ItemRate", d(r.get("Rate")));
        det.put("RateUomId", toInt(r.get("RateUomId")));
        det.put("Amount", d(r.get("Amount")));
        det.put("RemarksDetail", "");
        return det;
    }

    /** DeleteOutputDetailRow:1575 / DeleteScrapDetailRow:1781 - the removed detail object (ActionTypeId 3). */
    private Map<String, Object> removedOutput(Map<String, Object> r) {
        int type = toInt(r.get("OutPutTypeId")) == 2 ? 2 : 1;
        Map<String, Object> det = outputDetail(r, toInt(r.get("Id")), type);
        det.put("ActionTypeId", 3);
        if (type == 2) {
            det.put("WorkOrderId", 0);
            det.put("workOrderItemDetailId", 0);
            det.put("ItemVariantId", 0);
            det.put("ProcessStatusId", 0);
        }
        return det;
    }

    /** DeleteInputDetailRow:1225. */
    private Map<String, Object> removedInput(Map<String, Object> r, int heatCode, Timestamp inTime, Timestamp outTime) {
        Map<String, Object> det = new LinkedHashMap<>();
        det.put("productionInputDetailId", toInt(r.get("Id")));
        det.put("BomHeaderId", toInt(r.get("BomHeaderId")));
        det.put("BomInputDetailId", toInt(r.get("BomInputDetailId")));
        det.put("WareHouseId", toInt(r.get("WarehouseId")));
        det.put("ItemId", toInt(r.get("ItemId")));
        det.put("ItemUomId", toInt(r.get("PackUomId")));
        det.put("Qty", d(r.get("NetWeight")));
        det.put("Weight", d(r.get("NetWeight")));
        det.put("ItemRate", d(r.get("Rate")));
        det.put("RateUomId", toInt(r.get("RateUomId")));
        det.put("Amount", d(r.get("Amount")));
        det.put("RemarksDetail", "");
        det.put("ActionTypeId", 3);
        det.put("HeatCode", heatCode);
        det.put("timeStart", inTime);
        det.put("timeOut", outTime);
        return det;
    }

    // -------------------------------------------------------------------------------- BLL 0366 MakeVoucher (:19)

    private static Map<String, Object> gl(List<Map<String, Object>> items, int itemId) {
        for (Map<String, Object> g : items) if (toInt(ci(g, "Id")) == itemId) return g;
        return null;
    }

    private Map<String, Object> makeVoucher(Ctx c, Map<String, Object> header) {
        List<Map<String, Object>> items = inRepo.itemGlAccounts(c.u);
        Map<String, Object> vh = ProductionInputRepository.voucherHeadDefaults();
        vh.put("DocumentTypeSrNo", c.headerId);
        vh.put("VoucherDate", c.docDate);
        vh.put("VoucherCode", c.docNo);
        vh.put("DocumentTypeId", DOC_TYPE);
        vh.put("EntryDate", Prod273Support.now());
        vh.put("ModifyDate", Prod273Support.now());
        vh.put("EntryUser", c.uid);
        vh.put("ModifyUser", c.uid);
        vh.put("OrganizationId", c.org);
        vh.put("CompanyId", c.company);
        vh.put("FinancialYearId", c.fy);
        vh.put("BranchId", c.branch);
        vh.put("IsApproved", false);
        double total = 0;
        if (!c.output.isEmpty()) {
            int lineId = 1;
            for (Map<String, Object> o : c.output) {
                if (toInt(o.get("ActionTypeId")) == 3) continue;
                need(!items.isEmpty(), "Items record not found for accounts...");
                Map<String, Object> g = gl(items, toInt(o.get("ItemId")));
                need(g != null, "Item GL Account Not Found Against this ItemId " + toInt(o.get("ItemId")));
                String text = "ItemName: " + str(ci(g, "ItemName")) + " Qty  " + cs(d(o.get("ItemQty"))) + " Rate  " + cs(d(o.get("ItemRate")));
                Map<String, Object> v = ProductionInputRepository.voucherDetailDefaults();
                v.put("LineId", lineId);
                v.put("AccountId", toInt(ci(g, "PurchaseGLAC")));
                v.put("AgainstAccountId", toInt(ci(g, "PurchaseGLAC")));
                v.put("Comments", text);
                v.put("DebitAmount", d(o.get("Amount")));
                v.put("QtyIn", d(o.get("ItemQty")));
                v.put("WeightIn", d(o.get("NetWeight")));
                v.put("ItemRate", d(o.get("ItemRate")));
                v.put("ItemAmount", d(o.get("Amount")));
                v.put("JobLotId", toInt(o.get("JobLotId")));
                c.voucherLines.add(v);
                lineId++;
            }
            lineId = 1;
            for (Map<String, Object> o : c.input) {
                if (toInt(o.get("ActionTypeId")) == 3) continue;
                need(!items.isEmpty(), "Items record not found for accounts...");
                Map<String, Object> g = gl(items, toInt(o.get("ItemId")));
                need(g != null, "Item GL Account Not Found Against this ItemId " + toInt(o.get("ItemId")));
                String text = "ItemName: " + str(ci(g, "ItemName")) + " Qty  " + cs(d(o.get("Qty"))) + " Rate  " + cs(d(o.get("ItemRate")));
                Map<String, Object> v = ProductionInputRepository.voucherDetailDefaults();
                v.put("LineId", lineId);
                v.put("AccountId", toInt(ci(g, "PurchaseGLAC")));
                v.put("AgainstAccountId", toInt(ci(g, "PurchaseGLAC")));
                v.put("Comments", text);
                v.put("CreditAmount", d(o.get("Amount")));
                v.put("QtyOut", d(o.get("Qty")));
                v.put("WeightOut", d(o.get("Weight")));
                v.put("ItemRate", d(o.get("ItemRate")));
                v.put("ItemAmount", d(o.get("Amount")));
                c.voucherLines.add(v);
                total += d(v.get("CreditAmount"));
                lineId++;
            }
            for (Map<String, Object> o : c.overhead) {
                String text;
                if (str(o.get("Remarks")).isEmpty()) {
                    text = "Account: " + str(o.get("OverHeadAccountTitle"));
                    if (d(o.get("Qty")) > 0) text += " Qty " + cs(d(o.get("Qty")));
                    if (d(o.get("Rate")) > 0) text += " Rate " + cs(d(o.get("Rate")));
                    if (d(o.get("Amount")) > 0) text += " Amount " + cs(d(o.get("Amount")));
                } else {
                    text = str(o.get("Remarks"));
                }
                Map<String, Object> v = ProductionInputRepository.voucherDetailDefaults();
                v.put("LineId", lineId);                                                                      // num2 is not advanced inside the over head loop (0366:116)
                v.put("AccountId", toInt(o.get("OverHeadAccountId")));
                v.put("AgainstAccountId", toInt(o.get("OverHeadAccountId")));
                v.put("Comments", text);
                v.put("CreditAmount", d(o.get("Amount")));
                c.voucherLines.add(v);
                total += d(v.get("CreditAmount"));
            }
        }
        vh.put("VoucherAmount", total);
        return vh;
    }

    // -------------------------------------------------------------------------------- DAL 0319 SetData

    private int setData(Ctx c, Map<String, Object> header) {
        int num = repo.setHeader(header);
        if (num > 0) c.headerId = num; else { num = c.recId; c.headerId = c.recId; }

        int line = 1;
        for (Map<String, Object> det : c.output) {
            det.put("ProductionHeaderId", c.headerId);
            det.put("LineId", line);
            det.put("ProductionOutPutDetailId", repo.setOutput(model(det, OUTPUT_PARAMS)));
            line++;
        }
        line = 1;
        for (Map<String, Object> det : c.input) {
            det.put("ProductionHeaderId", c.headerId);
            det.put("SortNo", line);
            det.put("productionInputDetailId", repo.setInput(model(det, INPUT_PARAMS)));
            line++;
        }
        for (Map<String, Object> det : c.overhead) {
            det.put("ProductionHeaderId", c.headerId);
            det.put("ProductionOverHeadsDetailId", repo.setOverhead(model(det, OVERHEAD_PARAMS)));
        }
        /* AttachmentsList (Proc_DMSAttachments_Insert) is not ported. */

        Map<String, Object> it = ProductionInputRepository.inventoryTransactionsDefaults();
        it.put("OrganizationId", c.org);
        it.put("CompanyId", c.company);
        it.put("RefDocumentTypeId", DOC_TYPE);
        it.put("RefDocIdNo", num);
        inRepo.setProc("Sp_InventoryTransactions_GetALLMethod", it, ProductionInputRepository.P_INV_TRANSACTIONS);

        Map<String, Object> se = ProductionInputRepository.stockEvaluationDefaults();
        se.put("OrganizationId", c.org);
        se.put("CompanyId", c.company);
        se.put("RefDocumentTypeId", DOC_TYPE);
        se.put("RefDocIdNo", c.headerId);
        inRepo.setProc("Sp_InventoryStockEvalautionDetail_Update", se, ProductionInputRepository.P_STOCK_EVALUATION_UPDATE);

        int existing = inRepo.voucherHeadId(c.u, DOC_TYPE, c.headerId);
        if (c.settled) {
            Map<String, Object> vh = c.voucher;
            if (existing > 0) vh.put("Id", existing);
            vh.put("DocumentTypeSrNo", c.headerId);
            int vr = inRepo.setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh, ProductionInputRepository.P_VOUCHER_HEAD);
            int voucherId = vr > 0 ? vr : toInt(vh.get("Id"));
            vh.put("Id", voucherId);
            if (!c.voucherLines.isEmpty()) {
                for (Map<String, Object> v : c.voucherLines) {
                    int lid = toInt(v.get("LineId"));
                    for (Map<String, Object> o : c.output) {                                                  // x.LineId == item2.LineId && both > 0 && DebitAmount > 0
                        if (toInt(o.get("LineId")) == lid && lid > 0 && d(v.get("DebitAmount")) > 0) { v.put("RefDocSubIdNo", toInt(o.get("ProductionOutPutDetailId"))); break; }
                    }
                    for (Map<String, Object> o : c.input) {                                                   // input LineId stays 0 in the DAL, so this never matches (kept)
                        if (toInt(o.get("LineId")) == lid && lid > 0 && d(v.get("CreditAmount")) > 0) { v.put("RefDocSubIdNo", toInt(o.get("productionInputDetailId"))); break; }
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
            }
        } else if (existing > 0) {
            repo.voucherDeleteByVoucherId(existing);
        }
        return num;
    }

    // -------------------------------------------------------------------------------- model -> declared parameters

    private static final java.util.Set<String> OUTPUT_PARAMS = names("ProductionOutPutDetailId", "ProductionHeaderId", "BomHeaderId", "BomByProductDetailId", "WorkOrderId", "SaleOrderId",
            "OutPutTypeId", "WareHouseId", "ItemId", "JobLotId", "PackingTypeId", "ItemUomId", "ItemQty", "BomQty", "NetWeight", "ItemRate", "RateUomId", "Amount", "ActionTypeId",
            "LineId", "RemarksDetail", "ProcessStatusId", "ItemVariantId", "workOrderItemDetailId");
    private static final java.util.Set<String> INPUT_PARAMS = names("productionInputDetailId", "ProductionHeaderId", "BomHeaderId", "BomInputDetailId", "HeatCode", "timeStart", "timeOut",
            "WareHouseId", "ItemId", "ItemUomId", "BomQty", "Qty", "Weight", "ItemRate", "RateUomId", "Amount", "SortNo", "ActionTypeId", "LineId", "RemarksDetail");
    private static final java.util.Set<String> OVERHEAD_PARAMS = names("ProductionOverHeadsDetailId", "ProductionHeaderId", "BomHeaderId", "BomOverHeadId", "OverHeadAccountId", "BomQty",
            "Qty", "Rate", "Amount", "Remarks", "PercentOfTotalInput");

    private static java.util.Set<String> names(String... n) { return new java.util.LinkedHashSet<>(java.util.Arrays.asList(n)); }

    /**
     * GenericProvider.SetProc reflects over the model: every value-type property is sent (a CLR default when never set), reference types only when not null.
     * The procedures declare more parameters than that, so the model's properties are filtered to the declared ones, with their CLR defaults filled in.
     */
    private static Map<String, Object> model(Map<String, Object> det, java.util.Set<String> declared) {
        Map<String, Object> p = new LinkedHashMap<>();
        for (String k : declared) {
            Object v = det.get(k);
            if (v == null) {
                if (k.equals("RemarksDetail") || k.equals("Remarks")) v = "";
                else if (k.equals("timeStart") || k.equals("timeOut")) v = null;
                else v = 0;
            }
            p.put(k, v instanceof BigDecimal ? ((BigDecimal) v).doubleValue() : v);
        }
        return p;
    }
}
