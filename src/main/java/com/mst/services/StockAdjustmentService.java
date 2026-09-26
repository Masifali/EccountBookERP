package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.StockAdjustmentDto;
import com.mst.repositories.StockAdjustmentRepository;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.lang.reflect.Field;
import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.StockAdjustmentRepository.DOCUMENT_TYPE_ID;
import static com.mst.repositories.StockAdjustmentRepository.project;
import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.clr;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Store Management (ModuleId 24) — screen 320 "Stock Adjustment".
 *
 *   Desktop form : Architecture.WinApp.StoreManagement.frmStockAdjustment (ScreenName "frmStockAdjustment")
 *   Loader       : Architecture.WinApp.Production.LoadavailableTransactionsForIssuance (btnLoadAvailableStock_Click:2335)
 *   DocumentTypeId 70, route /store/stock-adjustment, API /api/store/stock-adjustment
 *   BLL 0546 / DAL 0401 Architecture.*.Inventory.InvStockAdjustment, Models 0977 / 0978,
 *   DAL 0205 CommonServices (FIFOImplemention, GetEqvilentByItemIdAndUomScheduleId), BLL 0056
 *   GetAvgRatesAndStockInHand, WinApp CommonServices (GetAvgRateFromFIFOMethod:2787, AvgRateOnlyForCGS:2719,
 *   GetStockInHandFromInventoryTrasactions:2687, StockAdjustmentSlipAndRegister409:8583, VoucherReport_118:5647).
 *
 * LoadStockForAdjustment.cs is NOT opened by this form (no reference to it anywhere in the recovered
 * desktop source except its own file) — the Loader button opens LoadavailableTransactionsForIssuance.
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 * ---------------------------------------------------------------------------------------------
 *  1. FormHistory sends @DocNoFrom/@DocNoTo, but Sp_InvStockAdjustment_GetAllMethod 'FormHistory'
 *     never filters on them — the history Doc No boxes have no effect.
 *  2. Sp_InvStockAdjustment_Insert recomputes DocNo itself (MAX+1); the number shown on the form is
 *     informational. On update the stored header's DocNo is sent (ReadOnly box, never trusted).
 *  3. FIFO costing (feature 5, Loss, no loader rows) calls FIFOImplemention with CropYear = the
 *     detail's virtual CropYear, which Insert() never sets, and no CropYearId — FIFO ignores the crop
 *     year; CropBatch is written as "" (Conversion.ToString(null)).
 *  4. In the FIFO voucher lines the credit line's ItemAmount is item6.AmountOut, which is still 0 when
 *     the line is built (DAL 0401:151, AmountOut is only assigned later at :203).
 *  5. VoucherAmount / BillAmount are summed only by MakeVoucher; FIFO-built lines do not add to them.
 *  6. MakeVoucher numbers voucher lines 1..n over items that HAVE a GL row, while details are numbered
 *     1..n over all rows; RefDocSubIdNo is matched by LineId, so a skipped item shifts the match.
 *  7. MakeVoucher's "loader row" flag is sticky: once one row with refs is seen, later rows count too.
 *  8. EntryUser and ModifyUser of the voucher head are the signed-in user on insert AND update
 *     (MakeVoucher runs before the BLL zeroes EnteryUserId / ModifyUserId).
 *  9. Financial effects on and no voucher line built (items without GL, or FIFO Loss without GL) →
 *     "VoucherDetail List Not Found" and the whole save rolls back.
 * 10. GetAvgRate (entry bar) lets the average-rate feature (1/2/19) override a FIFO rate; the
 *     "Generate Rate" button (AvgRateUpdateOnDocDateChange) prefers FIFO (else-if). Reproduced per call.
 * 11. Generate Rate stops at the first loader row (break, not continue).
 * 12. FIFO weight check uses Math.Round(sum, 2) (banker's rounding) and the loop ends on exact double
 *     equality of consumed weight — reproduced as written.
 * 13. No DateLock check: this BLL has none (other store documents do).
 * 14. Loader: when the user has grant rows for "LoadavailableTransactionsForIssuance" but none named
 *     "Rate", its Form_Load throws on newList[0]; combos stay empty, no auto-search, FromDate stays
 *     today, and Load fails with "Input array is longer than the number of columns in this table."
 *     (dtIssuance has no columns) — the page reproduces this state.
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (web-only)
 * ---------------------------------------------------------------------------------------------
 *  A. FIFO reads (USP_GetStockByFifoMethod, equivalents) run on the save's own transaction/connection;
 *     the desktop opens a separate connection for them while its transaction is open.
 *  B. @FIFOXML is built as {@code <ArrayOfFIFOStockEvaluation><FIFOStockEvaluation>…} with the five
 *     elements the procedure reads (RefDocumentTypeId, RefDocIdNo, RefDocSubIdNo, ReserveQty,
 *     ReserveWeight). The desktop's SerializationSurrogate is not in the recovered source, so the
 *     remaining (unread) elements are not reproduced.
 *  C. A document is opened / updated / printed only when it is DocumentTypeId 70 of the user's
 *     organization and company; a header insert that returns no id is refused.
 *  D. DocNo is generated server-side at save (see 2); the desktop sends the number shown at reset.
 *  E. Attachments (DMS), saved grid layouts, shortcut-key popup: not ported. Crystal layouts
 *     409-InvStockAdjustmentSlip.rpt / 118-AcRptVoucherSlip.rpt: the procedure rows are returned.
 */
@Service
public class StockAdjustmentService {

    public static final String SCREEN = "frmStockAdjustment";
    private static final String LOADER_SCREEN = "LoadavailableTransactionsForIssuance";
    private static final int FEATURE_FIFO = 5;

    private final StockAdjustmentRepository repo;
    private final StoreIssuanceRepository shared;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public StockAdjustmentService(StockAdjustmentRepository repo, StoreIssuanceRepository shared,
                                  StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.shared = shared;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ===================================================================================== load

    /** frmStockAdjustment_Load:274 — rights, features, doc no and every combo the form binds. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        int fy = ctx.currentFinancialYearId();
        Set<Integer> f = repo.erpFeatures(u);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN));
        out.put("fifoMethod", f.contains(FEATURE_FIFO));                                  // :298
        out.put("avgFeatureMethod", f.contains(1) || f.contains(2) || f.contains(19));    // :299
        out.put("docNo", repo.nextDocNo(u, fy));                                          // DocumentNoFill:357
        out.put("amountRoundDigits", toInt(shared.config(u, "Default NoofDecimal Points For Amount")));
        out.putAll(refreshLists(u));
        out.put("cropYears", project(repo.cropYears(u), "Id", "CropYear"));                // CropYearFill:465
        out.put("entryTypes", project(repo.entryTypes(), "Id", "Type"));                  // EntryTypeBind:637
        out.put("historyTypes", historyTypes());                                          // HistoryCombosFill:1440
        return out;
    }

    /** Reset():815 → DocumentNoFill:357. */
    public int docNo() {
        return repo.nextDocNo(ctx.requireAccountingUser(), ctx.currentFinancialYearId());
    }

    /** btnRefresh_Click:875 — packing types, warehouses, job lots, items (crop year is not refreshed). */
    public Map<String, Object> refresh() {
        return refreshLists(ctx.requireAccountingUser());
    }

    private Map<String, Object> refreshLists(UserAccount u) {
        int branch = branch(u);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("warehouses", project(repo.warehouses(u, branch), "Id", "WareHouseName"));     // :385
        out.put("jobLots", project(repo.jobLots(u, branch), "Id", "JobLotDescription"));       // :425
        out.put("packingTypes", project(repo.packingTypes(), "Id", "PackTypeDesc"));           // :604
        List<Map<String, Object>> items = new ArrayList<>();                                   // ItemBind:499
        for (Map<String, Object> r : repo.items(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("ItemName", str(r.get("ItemName")));
            o.put("ItemCode", str(r.get("ItemCodeNew")));
            items.add(o);
        }
        out.put("items", items);
        return out;
    }

    public List<Map<String, Object>> historyTypes() {
        return project(repo.historyAdjustmentTypes(ctx.requireAccountingUser()), "Id", "ReferenceName");
    }

    /** bindRateUomAndItemPackUom:581 → CommonServices.GetUomScheduleByItemId:5239 (Id, UOMCode, Equivalent...). */
    public List<Map<String, Object>> uoms(int itemId) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.uomSchedule(u, itemId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("UOMCode", str(r.get("UOMCode")));
            o.put("Equivalent", toDouble(r.get("Equivalent")));
            o.put("QtyEquivalent", toDouble(r.get("QtyEquivalent")));
            o.put("BaseRateUom", r.get("BaseRateUom"));
            out.add(o);
        }
        return out;
    }

    /** CmbEntryType_Leave:670 — 1 → "10,12", anything else (blank included) → "11,12,13,20,21". */
    public List<Map<String, Object>> accounts(int entryTypeId) {
        UserAccount u = ctx.requireAccountingUser();
        String ids = entryTypeId == 1 ? "10,12" : "11,12,13,20,21";
        return project(repo.accountsByTypeIds(u, ids), "Id", "AccountTitle");
    }

    // ============================================================================ stock / rates

    /** AvailableStock:2282 — the value, and whether the label shows (Balance > 0). */
    public Map<String, Object> stockBalance(int recId, int itemId, String docDate, int jobLotId, int warehouseId, String cropYear) {
        UserAccount u = ctx.requireAccountingUser();
        double b = repo.stockInHand(u, itemId, StoreIssuanceService.formDate(docDate, recId), jobLotId, warehouseId,
                cropYear == null ? "" : cropYear);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("balance", b);
        out.put("visible", b > 0d);
        return out;
    }

    /**
     * The rate lookups of GetAvgRate:2146 (mode "entry") and AvgRateUpdateOnDocDateChange:2414
     * (mode "grid"). Returns {rate, computed}: computed=false when neither feature is on.
     */
    public Map<String, Object> rate(String mode, int recId, int itemId, String docDate, int packUomId, int warehouseId,
                                    int packingTypeId, int jobLotId, int cropYearId, double qty, double weight) {
        UserAccount u = ctx.requireAccountingUser();
        Set<Integer> f = repo.erpFeatures(u);
        boolean fifo = f.contains(FEATURE_FIFO);
        boolean avg = f.contains(1) || f.contains(2) || f.contains(19);
        Timestamp d = StoreIssuanceService.formDate(docDate, recId);
        double rate = 0d;
        boolean computed = false;
        boolean grid = "grid".equals(mode);
        if (fifo) {
            rate = round(fifoAverageRate(u, itemId, d, packUomId, warehouseId, packingTypeId, jobLotId, cropYearId,
                    qty, weight, recId), 3);
            computed = true;
        }
        if (avg && (!grid || !fifo)) {                    // entry: `if`, overrides FIFO; grid: `else if`
            rate = repo.avgRateOnlyForCgs(u, itemId, d, DOCUMENT_TYPE_ID, recId, jobLotId, cropYearId, warehouseId);
            computed = true;
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rate", rate);
        out.put("computed", computed);
        return out;
    }

    /** CommonServices.GetAvgRateFromFIFOMethod:2787 — BalWeight / BalQty (not the Net columns). */
    private double fifoAverageRate(UserAccount u, int itemId, Timestamp docDate, int packUomId, int warehouseId,
                                   int packingTypeId, int jobLotId, int cropYearId, double qty, double weight, int recId) {
        List<Map<String, Object>> rows = repo.stockByFifo(u, itemId, docDate, packUomId, warehouseId, jobLotId,
                packingTypeId, cropYearId, null, recId > 0 ? DOCUMENT_TYPE_ID : 0, recId > 0 ? recId : 0, null);
        if (rows.isEmpty()) return 0d;
        double totalWt = 0d;
        for (Map<String, Object> r : rows) totalWt += toDouble(r.get("BalWeight"));
        double reqQty = qty, reqWt = weight, total = 0d;
        if (weight <= totalWt) {
            double consumed = 0d, consumedQty = 0d;
            for (Map<String, Object> r : rows) {
                double avail = toDouble(r.get("BalWeight")), availQty = toDouble(r.get("BalQty"));
                double avgRate = toDouble(r.get("AvgRate"));
                if (avail <= reqWt - consumed && availQty <= reqQty - consumedQty) {
                    consumed += avail;
                    consumedQty += availQty;
                    total += avail * avgRate;
                } else if (avail >= reqWt - consumed && availQty >= reqQty - consumedQty) {
                    double outQty = reqQty - consumedQty, outWt = reqWt - consumed;
                    total += outWt * avgRate;
                    consumedQty += outQty;
                    consumed += outWt;
                }
                if (reqWt == consumed && reqQty == consumedQty) break;
            }
            if (total > 0d && reqWt > 0d) return total / reqWt;
        }
        return 0d;
    }

    // ================================================================================= history

    /** gridhistoryfill:1492 — the twelve columns the desktop copies. */
    public List<Map<String, Object>> history(String from, String to, int fromDocNo, int toDocNo, int adjustmentTypeId) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN, "viewAll");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, ctx.currentFinancialYearId(), viewAll, u.getId(),
                isBlank(from) ? null : StoreIssuanceService.pickerDate(from),
                isBlank(to) ? null : StoreIssuanceService.pickerDate(to), fromDocNo, toDocNo, adjustmentTypeId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String k : new String[] { "RecordNo", "Id", "VoucherHeadId", "DocDate", "DocNo", "RemarksHeader",
                    "EntryType", "EnteryDate", "EntryUser", "ModifyDate", "ModifyUser", "NoOfAttachments" }) {
                o.put(k, r.get(k));
            }
            out.add(o);
        }
        return out;
    }

    // ==================================================================================== read

    /** ReadById:1370 / GridDetailBind:1680 — header plus detail rows in the grid's column names. */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) return null;
        int type = toInt(ci(h, "AdjustmentTypeId"));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.details(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(d.get("Id")));
            o.put("RefDocumentTypeId", toInt(d.get("RefDocumentTypeId")));
            o.put("RefDocNoId", toInt(d.get("RefDocNoId")));
            o.put("RefDocSubIdNo", toInt(d.get("RefDocSubIdNo")));
            o.put("RefDocumentType", str(d.get("RefDocumentType")));
            o.put("RefDocDate", d.get("RefDocDate"));
            o.put("RefDocNo", toInt(d.get("RefDocNo")));
            o.put("WareHouseId", toInt(d.get("WarehouseId")));
            o.put("WareHouse", str(d.get("WareHouseName")));
            o.put("ItemId", toInt(d.get("ItemId")));
            o.put("Item", str(d.get("ItemName")));
            o.put("ItemCode", str(d.get("ItemCode")));
            o.put("CropYearId", toInt(d.get("CropYearId")));
            o.put("CropYear", str(d.get("CropYear")));
            o.put("JobLotId", toInt(d.get("JobLotId")));
            o.put("JobLot", str(d.get("JobLot")));
            o.put("PackingTypeId", toInt(d.get("PackingTypeId")));
            o.put("PackingType", str(d.get("PackingType")));
            o.put("ItemUOMId", toInt(d.get("PackUomId")));
            o.put("ItemUOM", str(d.get("PackUom")));
            o.put("ItemUOMEquivalent", toDouble(d.get("PackUomEquivalent")));
            o.put("ItemQty", toDouble(d.get("Qty")));
            o.put("Weight", toDouble(d.get("NetWeight")));
            o.put("ItemRate", toDouble(d.get("ItemRate")));
            o.put("RateUomId", toInt(d.get("RateUomId")));
            o.put("RateUom", str(d.get("RateUom")));
            o.put("RateUomEquivalent", toDouble(d.get("RateUomEquivalent")));
            o.put("ItemAmount", toDouble(d.get("Amount")));
            /* :1392 — Loss shows DebitAccountId, anything else CreditAccountId. */
            o.put("AccountId", type != 2 ? toInt(d.get("CreditAccountId")) : toInt(d.get("DebitAccountId")));
            o.put("AccountTitle", str(d.get("AccountTitle")));
            o.put("Comments", str(d.get("RemarksDetail")));
            rows.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(ci(h, "Id")));
        out.put("DocNo", toInt(ci(h, "DocNo")));
        out.put("DocDate", ci(h, "DocDate"));
        out.put("RemarksHeader", str(ci(h, "RemarksHeader")));
        out.put("AdjustmentTypeId", type);
        out.put("rows", rows);
        return out;
    }

    /** GenerateReport:1798 → StockAdjustmentSlipAndRegister409 (@Id only). */
    public List<Map<String, Object>> slip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (id == 0) throw new IllegalArgumentException("No Record Found For Display");
        if (ownedHeader(u, id) == null) return null;
        return repo.slip(u, id);
    }

    /** btnVoucher118_Click / history "Voucher" → VoucherReport_118(VoucherHeadId). */
    public List<Map<String, Object>> voucherSlip(int voucherHeadId) {
        UserAccount u = ctx.requireAccountingUser();
        if (voucherHeadId == 0) throw new IllegalArgumentException("VoucherId Not Found");
        return repo.voucherSlip(u, voucherHeadId);
    }

    // ================================================================================== loader

    /** LoadavailableTransactionsForIssuance.LoadInvoices_Load:128 + StockComboFill:197. */
    public Map<String, Object> loaderLookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> grants = repo.userRights(u, LOADER_SCREEN, ctx.currentRoleName());
        boolean hasRate = false;
        for (Map<String, Object> g : grants) if ("Rate".equals(str(g.get("RightName")))) hasRate = true;
        if (!grants.isEmpty() && !hasRate) {                                  // :149 newList[0] on an empty list
            out.put("failed", "Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
            return out;
        }
        out.put("failed", null);
        out.put("fromDate", repo.financialYearStart(u, ctx.currentFinancialYearId()));      // :153
        String[] kinds = { "ParentCategories", "ItemCategories", "ItemTypes", "JobLot", "CropYear", "Warehouse",
                "DocumentType", "Supplier_Customer", "Items" };
        Map<String, List<Map<String, Object>>> lists = new LinkedHashMap<>();
        for (String k : kinds) lists.put(k, new ArrayList<>());
        for (Map<String, Object> r : repo.loaderDropDowns(u, branch(u))) {
            List<Map<String, Object>> t = lists.get(str(r.get("ActivityType")));
            if (t == null) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id"));
            o.put("Name", str(r.get("name")));
            t.add(o);
        }
        out.putAll(lists);
        return out;
    }

    /** PendingInventoryTransactionsForIssuanceLoad:319 — rows projected as dtcol (GrnNo 0 for 112 / 80). */
    public List<Map<String, Object>> loaderSearch(String from, String to, int parentCategoryId, int itemCategoryId,
                                                  int itemTypeId, int jobLotId, String cropYear, int warehouseId,
                                                  int refDocumentTypeId, int supplierCustomerId, int itemId) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.availableTransactions(u, branch(u), StoreIssuanceService.dateOnly(from),
                StoreIssuanceService.dateOnly(to), supplierCustomerId, warehouseId, itemId, refDocumentTypeId, jobLotId,
                parentCategoryId, itemCategoryId, itemTypeId, cropYear)) {
            Map<String, Object> o = new LinkedHashMap<>();
            int rdt = toInt(r.get("RefDocumentTypeId"));
            for (String k : new String[] { "RefDocumentTypeId", "RefDocIdNo", "RefDocSubIdNo", "RefDocumentType",
                    "DocDate", "DocCodeNo", "ManualNo", "GrnNo", "SupplierCustomerId", "SupplierCustomerName",
                    "VehicleNo", "GpNo", "WarehouseId", "WareHouseCode", "RefWarehouse", "ItemId", "ItemName",
                    "ItemCode", "CropYearId", "CropBatch", "JobLotId", "JobLotCode", "InvPackingTypeId", "PackingType",
                    "ItemUom", "PackUom", "PackSize", "QtyIn", "QtyOut", "QtyBalance", "WeightIn", "WeightOut",
                    "WeightBalance", "ReserveWeight", "StockWeightOut", "AVgRate", "RateUom", "Equivalent",
                    "RateUomId", "ItemAmount", "BiltyNo" }) {
                o.put(k, r.get(k));
            }
            if (rdt == 112 || rdt == 80) o.put("GrnNo", 0);
            o.put("Remarks", r.get("TranRemarks"));
            out.add(o);
        }
        return out;
    }

    // ==================================================================================== save

    /** btnsave_Click:1341 / btnupdate_Click:1354 → Insert():1195 → BLL 0546 Save → DAL 0401 SetData. */
    @Transactional
    public Map<String, Object> save(StockAdjustmentDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.Id == null ? 0 : dto.Id;
        if (recId == 0 && !rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        Map<String, Object> existing = null;
        if (recId != 0) {
            existing = ownedHeader(u, recId);
            if (existing == null) throw new IllegalArgumentException("Record Not Update because RecId Not Found");
        }
        List<StockAdjustmentDto.Row> rows = dto.rows == null ? new ArrayList<>() : dto.rows;
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");             // :1205
        int fy = ctx.currentFinancialYearId();
        int docNo = existing != null ? toInt(ci(existing, "DocNo")) : repo.nextDocNo(u, fy);
        if (docNo == 0) throw new IllegalArgumentException("DocNo Field is Required");                // :715
        int type = i(dto.AdjustmentTypeId);
        boolean typeInList = false;
        for (Map<String, Object> t : repo.entryTypes()) if (toInt(t.get("Id")) == type) typeInList = true;
        if (type == 0 || !typeInList) throw new IllegalArgumentException("Entry Type Field is Required"); // :721

        Timestamp docDate = StoreIssuanceService.formDate(dto.DocDate, recId);
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        Map<String, Object> h = new LinkedHashMap<>();                      // Model 0977, declaration order
        h.put("IsApproved", false);
        h.put("ApprovedDate", now);
        h.put("DocDate", docDate);
        h.put("EnteryDate", now);
        h.put("ModifyDate", now);
        h.put("AdjustmentTypeId", type);
        h.put("ApprovalUserId", u.getId());
        h.put("BranchesId", branch(u));
        h.put("CompanyId", u.getCompanyId());
        h.put("DocNo", docNo);
        h.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        h.put("EnteryUserId", u.getId());
        h.put("FinancialYearId", fy);
        h.put("Id", recId);
        h.put("ModifyUserId", u.getId());
        h.put("OrganizationId", u.getOrganizationId());
        h.put("ProjectsId", 0);
        h.put("RemarksHeader", dto.Remarks == null ? "" : dto.Remarks);

        List<Map<String, Object>> details = new ArrayList<>();
        List<String> rateUoms = new ArrayList<>();
        for (int idx = 0; idx < rows.size(); idx++) {                       // :1242
            StockAdjustmentDto.Row r = rows.get(idx);
            int n = idx + 1;
            if (i(r.WareHouseId) == 0) throw new IllegalArgumentException("Warehouse Not found In Grid Row No : " + n);
            if (i(r.ItemId) == 0) throw new IllegalArgumentException("Item Not found In Grid Row No : " + n);
            if (i(r.CropYearId) == 0) throw new IllegalArgumentException("CropYear Not found In Grid Row No : " + n);
            if (i(r.JobLotId) == 0) throw new IllegalArgumentException("JobLot Not found In Grid Row No : " + n);
            if (i(r.PackingTypeId) == 0) throw new IllegalArgumentException("PackingType Not found In Grid Row No : " + n);
            if (i(r.ItemUOMId) == 0) throw new IllegalArgumentException("ItemUOM Not found In Grid Row No : " + n);
            if (type != 2 && d(r.ItemQty) == 0d) throw new IllegalArgumentException("ItemQty Not found In Grid Row No : " + n);
            if (d(r.Weight) == 0d) throw new IllegalArgumentException("Weight Not found In Grid Row No : " + n);
            if (i(r.RateUomId) == 0) throw new IllegalArgumentException("RateUom Not found In Grid Row No : " + n);
            if (d(r.ItemAmount) == 0d) throw new IllegalArgumentException("ItemAmount Not found In Grid Row No : " + n);
            if (i(r.AccountId) == 0) throw new IllegalArgumentException("Account Not found In Grid Row No : " + n);
            Map<String, Object> m = new LinkedHashMap<>();                  // Model 0978, declaration order
            m.put("ItemRate", dec(d(r.ItemRate)));
            m.put("NetWeight", dec(d(r.Weight)));
            m.put("Qty", dec(d(r.ItemQty)));
            m.put("Amount", d(r.ItemAmount));
            m.put("CreditAccountId", type == 2 ? 0 : i(r.AccountId));      // :1305
            m.put("CropYearId", i(r.CropYearId));
            m.put("DebitAccountId", type == 2 ? i(r.AccountId) : 0);
            m.put("Id", i(r.Id));
            m.put("RefDocumentTypeId", i(r.RefDocumentTypeId));
            m.put("RefDocNoId", i(r.RefDocNoId));
            m.put("RefDocSubIdNo", i(r.RefDocSubIdNo));
            m.put("InvStockAdjustmentId", 0);
            m.put("ItemId", i(r.ItemId));
            m.put("JobLotId", i(r.JobLotId));
            m.put("PackingTypeId", i(r.PackingTypeId));
            m.put("PackUomId", i(r.ItemUOMId));
            m.put("RateUomId", i(r.RateUomId));
            m.put("WarehouseId", i(r.WareHouseId));
            m.put("LineId", 0);
            m.put("ItemConditionId", 0);
            m.put("RackId", 0);
            m.put("RemarksDetail", r.Comments == null ? "" : r.Comments);
            details.add(m);
            rateUoms.add(r.RateUom == null ? "" : r.RateUom);               // vd.RateUom (virtual) — MakeVoucher text only
        }

        /* BLL Save. */
        boolean inactive = StoreIssuanceService.toBool(shared.config(u, "InventoryFinancialsEffectsInActive"));
        boolean fifo = repo.erpFeatures(u).contains(FEATURE_FIFO);
        Voucher voucher = inactive ? null : makeVoucher(u, h, details, fifo);
        if (recId == 0) h.put("ModifyUserId", 0); else h.put("EnteryUserId", 0);
        int id = setData(u, h, details, recId == 0 ? "Sp_InvStockAdjustment_Insert" : "Sp_InvStockAdjustment_Update",
                inactive, fifo, voucher);
        Map<String, Object> ok = new LinkedHashMap<>();
        ok.put("success", true);
        ok.put("message", (recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + docNo);
        ok.put("id", id);
        return ok;
    }

    private static final class Voucher {
        ContraVoucherDto.Head head;
        List<ContraVoucherDto.Detail> lines = new ArrayList<>();
    }

    /** BLL 0546 MakeVoucher. */
    private Voucher makeVoucher(UserAccount u, Map<String, Object> h, List<Map<String, Object>> details, boolean fifo) {
        Voucher v = new Voucher();
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        String now = Timestamp.valueOf(LocalDateTime.now()).toString();
        vh.DocumentTypeId = DOCUMENT_TYPE_ID;
        vh.DocumentTypeSrNo = toInt(h.get("Id"));
        vh.RefDocNoId = toInt(h.get("Id"));
        vh.VoucherCode = toInt(h.get("DocNo"));
        vh.VoucherDate = String.valueOf(h.get("DocDate"));
        vh.Remarks = str(h.get("RemarksHeader"));
        vh.RemarksOtherLingo = "";
        vh.ChequeDate = Timestamp.valueOf(LocalDate.now().atStartOfDay()).toString();
        vh.IncludeWHT = false;
        vh.BranchId = toInt(h.get("BranchesId"));
        vh.ProjectId = toInt(h.get("ProjectsId"));
        vh.ManualBillNo = String.valueOf(toInt(h.get("DocNo")));
        vh.DueDate = now;
        vh.OrganizationId = u.getOrganizationId();
        vh.CompanyId = u.getCompanyId();
        vh.FinancialYearId = toInt(h.get("FinancialYearId"));
        vh.EntryUser = toInt(h.get("EnteryUserId"));                // D8: still the user here
        vh.EntryDate = now;
        vh.ModifyDate = now;
        vh.ModifyUser = toInt(h.get("ModifyUserId"));
        v.head = vh;
        List<Map<String, Object>> gl = shared.itemGlAccounts(u);
        if (details.isEmpty()) return v;
        int type = toInt(h.get("AdjustmentTypeId"));
        int branch = toInt(h.get("BranchesId"));
        double sum = 0d;
        boolean loaderRow = false;
        int line = 1;
        for (Map<String, Object> item : details) {
            if (gl.isEmpty()) throw new IllegalArgumentException("Item Record Not found");
            Map<String, Object> g = find(gl, toInt(item.get("ItemId")));
            if (g == null) continue;
            if (toInt(item.get("RefDocumentTypeId")) > 0 && toInt(item.get("RefDocNoId")) > 0 && toInt(item.get("RefDocSubIdNo")) > 0) {
                loaderRow = true;                                          // D7 sticky
            }
            int purchase = toInt(g.get("PurchaseGLAC"));
            double amount = toDouble(item.get("Amount"));
            double qty = toDouble(item.get("Qty")), wt = toDouble(item.get("NetWeight")), rate = toDouble(item.get("ItemRate"));
            if (type == 1) {
                int cr = toInt(item.get("CreditAccountId"));
                ContraVoucherDto.Detail a = line(line, purchase, cr, branch, toInt(item.get("ItemId")), rate, amount);
                a.DebitAmount = amount; a.QtyIn = qty; a.WeightIn = wt;
                ContraVoucherDto.Detail b = line(line, cr, purchase, branch, toInt(item.get("ItemId")), rate, amount);
                b.CreditAmount = amount; b.QtyIn = qty; b.WeightIn = wt;
                v.lines.add(a); v.lines.add(b);
                line++;
                sum += b.CreditAmount;
            }
            if (type == 2 && (!fifo || loaderRow)) {
                int dr = toInt(item.get("DebitAccountId"));
                ContraVoucherDto.Detail a = line(line, dr, purchase, branch, toInt(item.get("ItemId")), rate, amount);
                a.DebitAmount = amount; a.QtyOut = qty; a.WeightOut = wt;
                ContraVoucherDto.Detail b = line(line, purchase, dr, branch, toInt(item.get("ItemId")), rate, amount);
                b.CreditAmount = amount; b.QtyOut = qty; b.WeightOut = wt;
                v.lines.add(a); v.lines.add(b);
                sum += b.CreditAmount;
                line++;
            }
        }
        vh.VoucherAmount = sum;
        vh.BillAmount = sum;
        return v;
    }

    private static ContraVoucherDto.Detail line(int lineId, int account, int against, int branch, int itemId,
                                                double rate, double amount) {
        ContraVoucherDto.Detail l = new ContraVoucherDto.Detail();
        l.LineId = lineId;
        l.AccountId = account;
        l.AgainstAccountId = against;
        l.Comments = "";
        l.ItemId = itemId;
        l.ItemRate = rate;
        l.ItemAmount = amount;
        l.BranchesId = branch;
        return l;
    }

    /** DAL 0401 SetData — one transaction (the caller's @Transactional). */
    private int setData(UserAccount u, Map<String, Object> h, List<Map<String, Object>> details, String proc,
                        boolean inactive, boolean fifo, Voucher voucher) {
        int recId = toInt(h.get("Id"));
        int num3 = repo.setProc(proc, h);
        if (num3 > 0) h.put("Id", num3); else num3 = recId;
        if (num3 <= 0) throw new IllegalStateException("Save returned no document id.");     // deviation C
        int id = num3;
        int line = 1;
        boolean loaderRows = false;
        for (Map<String, Object> d : details) {
            if (toInt(d.get("RefDocumentTypeId")) > 0 && toInt(d.get("RefDocNoId")) > 0 && toInt(d.get("RefDocSubIdNo")) > 0) loaderRows = true;
            d.put("InvStockAdjustmentId", id);
            d.put("LineId", line);
            d.put("Id", repo.setProc("Sp_InvStockAdjustmentDetail_Insert", d));
            line++;
        }
        int type = toInt(h.get("AdjustmentTypeId"));
        int modifyUser = toInt(h.get("ModifyUserId"));
        Timestamp docDate = (Timestamp) h.get("DocDate");
        if (fifo && type == 2 && !loaderRows) {                             // DocumentTypeId 70 != 215
            List<Map<String, Object>> gl = shared.itemGlAccounts(u);
            List<Map<String, Object>> evals = new ArrayList<>();
            for (Map<String, Object> d : details) {
                Map<String, Object> g = find(gl, toInt(d.get("ItemId")));
                if (g == null) continue;
                String name = str(g.get("ItemName"));
                for (Map<String, Object> e : fifoImplementation(u, d, docDate, modifyUser > 0 ? id : 0, name, evals)) {
                    String text = "ItemQty: " + clr(toDouble(e.get("QtyOut"))) + ", Item: " + name
                            + ", Net Weight: " + clr(toDouble(e.get("StockWeightOut")))
                            + ", CGS Rate: " + clr(toDouble(e.get("CgsRate")));
                    if (!inactive) {
                        int dr = toInt(d.get("DebitAccountId")), purchase = toInt(g.get("PurchaseGLAC"));
                        ContraVoucherDto.Detail a = new ContraVoucherDto.Detail();
                        a.LineId = toInt(d.get("LineId")); a.IsCGS = 1; a.AccountId = dr; a.AgainstAccountId = purchase;
                        a.Comments = text; a.DebitAmount = toDouble(e.get("CgsAmount")); a.ItemId = toInt(d.get("ItemId"));
                        a.QtyOut = toDouble(e.get("QtyOut")); a.WeightOut = toDouble(e.get("StockWeightOut"));
                        a.ItemCgsRate = toDouble(e.get("CgsRate")); a.JobLotId = toInt(d.get("JobLotId"));
                        a.BranchesId = toInt(h.get("BranchesId"));
                        voucher.lines.add(a);
                        ContraVoucherDto.Detail b = new ContraVoucherDto.Detail();
                        b.LineId = a.LineId; b.IsCGS = 1; b.AccountId = purchase; b.AgainstAccountId = dr;
                        b.Comments = text; b.CreditAmount = toDouble(e.get("CgsAmount")); b.ItemId = a.ItemId;
                        b.QtyOut = a.QtyOut; b.WeightOut = a.WeightOut; b.ItemCgsRate = a.ItemCgsRate;
                        b.ItemAmount = toDouble(e.get("AmountOut"));             // D4: still 0 here
                        b.JobLotId = a.JobLotId; b.BranchesId = a.BranchesId;
                        voucher.lines.add(b);
                    }
                    evals.add(e);
                }
            }
            if (!evals.isEmpty()) {
                if (modifyUser > 0) repo.reverseInventoryQty(u, id);
                for (Map<String, Object> e : evals) {
                    e.put("OrganizationId", u.getOrganizationId());
                    e.put("CompanyId", u.getCompanyId());
                    e.put("DocDate", docDate);
                    e.put("DocCodeNo", toInt(h.get("DocNo")));
                    e.put("SupplierCustomerId", 0);
                    e.put("BranchesId", toInt(h.get("BranchesId")));
                    e.put("RefDocumentTypeId", DOCUMENT_TYPE_ID);
                    e.put("EntryUser", toInt(h.get("EnteryUserId")));
                    e.put("ModifyUser", modifyUser);
                    e.put("CalcType", "Weight");
                    e.put("AmountOut", e.get("CgsAmount"));
                    for (Map<String, Object> d : details) {
                        int ln = toInt(d.get("LineId"));
                        if (ln == toInt(e.get("LineId")) && ln > 0) {
                            e.put("RefDocIdNo", toInt(d.get("InvStockAdjustmentId")));
                            e.put("RefDocSubIdNo", toInt(d.get("Id")));
                            e.put("ItemConditionId", toInt(d.get("ItemConditionId")));
                            break;
                        }
                    }
                    repo.setProc("USP_InventoryStockEvalautionDetail_Insert", e);
                }
            }
        } else {
            repo.evaluationUpdate(u, id);
        }
        repo.transactionsRecalc(u, id);
        if (type == 2) for (Map<String, Object> d : details) repo.inventoryValidation(u, docDate, d);
        if (!inactive) writeVoucher(u, id, h, details, voucher, type);
        return id;
    }

    /** DAL 0205 FIFOImplemention:725 for one detail; {@code reserved} is the list built so far. */
    private List<Map<String, Object>> fifoImplementation(UserAccount u, Map<String, Object> d, Timestamp docDate,
                                                         int recId, String name, List<Map<String, Object>> reserved) {
        String xml = null;
        if (!reserved.isEmpty()) {
            StringBuilder sb = new StringBuilder("<ArrayOfFIFOStockEvaluation>");
            for (Map<String, Object> r : reserved) {
                sb.append("<FIFOStockEvaluation><RefDocumentTypeId>").append(toInt(r.get("RefRefDocumentTypeId")))
                  .append("</RefDocumentTypeId><RefDocIdNo>").append(toInt(r.get("RefRefDocIdNo")))
                  .append("</RefDocIdNo><RefDocSubIdNo>").append(toInt(r.get("RefRefDocSubIdNo")))
                  .append("</RefDocSubIdNo><ReserveWeight>").append(xmlNum(toDouble(r.get("StockWeightOut"))))
                  .append("</ReserveWeight><ReserveQty>").append(xmlNum(toDouble(r.get("QtyOut"))))
                  .append("</ReserveQty></FIFOStockEvaluation>");
            }
            xml = sb.append("\n</ArrayOfFIFOStockEvaluation>").toString();
        }
        int itemId = toInt(d.get("ItemId"));
        List<Map<String, Object>> rows = repo.stockByFifo(u, itemId, docDate, toInt(d.get("PackUomId")),
                toInt(d.get("WarehouseId")), toInt(d.get("JobLotId")), toInt(d.get("PackingTypeId")),
                0, null, recId > 0 ? DOCUMENT_TYPE_ID : 0, recId, xml);           // D3: no crop year
        if (rows.isEmpty()) throw new IllegalArgumentException("Stock Not Found this Item " + name + " against FIFO Method .....");
        double sumW = 0d;
        for (Map<String, Object> r : rows) sumW += toDouble(r.get("NetBalWeight"));
        double itemQty = toDouble(d.get("Qty")), netWeight = toDouble(d.get("NetWeight"));
        if (!(netWeight <= new BigDecimal(sumW).setScale(2, RoundingMode.HALF_EVEN).doubleValue())) {
            throw new IllegalArgumentException("Weight available is " + clr(sumW) + " and row Weight is " + clr(netWeight)
                    + " this item " + name + " against FIFO....");
        }
        List<Map<String, Object>> out = new ArrayList<>();
        double num2 = netWeight, num3 = itemQty, num5 = 0d, num7 = 0d;
        for (Map<String, Object> r : rows) {
            double avg = toDouble(r.get("AvgRate"));
            if (avg <= 0d) throw new IllegalArgumentException("Rate Not Found this Item " + name + " against FIFO Method");
            int rateUom = toInt(r.get("RateUomId"));
            if (rateUom == 0) throw new IllegalArgumentException("RateUomId not found  this " + name + " against FIFO Method");
            double num6 = toDouble(r.get("NetBalWeight")), num4 = toDouble(r.get("NetBalQty"));
            double eq = repo.equivalent(u, itemId, rateUom);
            if (eq == 0d) throw new IllegalArgumentException("RateUom Not Found");
            Map<String, Object> e = null;
            if (num6 <= num2 - num7) {
                num7 += num6;
                num5 += num4;
                e = fifoRow(r, d, rateUom, num4, num6, avg * eq, eq);
            } else if (num6 >= num2 - num7) {
                double q = num3 - num5, w = num2 - num7;
                e = fifoRow(r, d, rateUom, q, w, avg * eq, eq);
                num5 += q;
                num7 += w;
            }
            if (e != null) out.add(e);
            if (netWeight == num7) break;
        }
        return out;
    }

    private static Map<String, Object> fifoRow(Map<String, Object> r, Map<String, Object> d, int rateUom,
                                               double qtyOut, double weightOut, double cgsRate, double eq) {
        Map<String, Object> e = StockAdjustmentRepository.evaluationModel();
        e.put("Id", toInt(r.get("Id")));
        e.put("LineId", toInt(d.get("LineId")));
        e.put("ItemId", toInt(d.get("ItemId")));
        e.put("WarehouseId", toInt(d.get("WarehouseId")));
        e.put("RateUom", rateUom);
        e.put("JobLotId", toInt(d.get("JobLotId")));
        e.put("InvPackingTypeId", toInt(d.get("PackingTypeId")));
        e.put("ItemUom", toInt(d.get("PackUomId")));
        e.put("CropBatch", "");
        e.put("RefRefDocumentTypeId", toInt(r.get("RefDocumentTypeId")));
        e.put("RefRefDocIdNo", toInt(r.get("RefDocIdNo")));
        e.put("RefRefDocSubIdNo", toInt(r.get("RefDocSubIdNo")));
        e.put("QtyOut", qtyOut);
        e.put("BillWeightOut", weightOut);
        e.put("StockWeightOut", weightOut);
        e.put("CgsRate", cgsRate);
        e.put("CgsAmount", weightOut / eq * cgsRate);
        return e;
    }

    /** DAL 0401 :258-300 — voucher head insert/update, details, H mirrors. */
    private void writeVoucher(UserAccount u, int id, Map<String, Object> h, List<Map<String, Object>> details,
                              Voucher v, int type) {
        ContraVoucherDto.Head vh = v.head;
        int existing = shared.voucherHeadId(u, DOCUMENT_TYPE_ID, id);
        if (existing > 0) vh.Id = existing;
        vh.DocumentTypeSrNo = id;
        vh.RefDocNoId = id;
        int num2 = shared.setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", model(vh));
        if (num2 > 0) vh.Id = num2;
        if (v.lines.isEmpty()) throw new IllegalArgumentException("VoucherDetail List Not Found");
        for (ContraVoucherDto.Detail l : v.lines) {
            l.VoucherHeadId = vh.Id;
            l.BranchesId = toInt(h.get("BranchesId"));
            for (Map<String, Object> d : details) {
                int ln = toInt(d.get("LineId"));
                if (ln == i(l.LineId) && ln > 0) { l.RefDocSubIdNo = toInt(d.get("Id")); break; }
            }
            l.IsCGS = type == 2 ? 1 : 0;
            shared.setProc("Sp_VoucherDetail_Insert", model(l));
        }
        int ref = shared.setProc("Sp_VoucherHead_H_Insert", model(vh));
        for (ContraVoucherDto.Detail l : v.lines) {
            l.VoucherHeadId = vh.Id;
            l.DocumentTypeIdRef = ref;
            shared.setProc("Sp_VoucherDetail_H_Insert", model(l));
        }
    }

    // ================================================================================= helpers

    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID) return null;
        if (toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId()) return null;
        return h;
    }

    private static Map<String, Object> find(List<Map<String, Object>> gl, int itemId) {
        for (Map<String, Object> x : gl) if (toInt(x.get("Id")) == itemId) return x;
        return null;
    }

    /** Public fields in declaration order (as StoreIssuanceService.model) — dates to Timestamp. */
    private static Map<String, Object> model(Object o) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Field f : o.getClass().getFields()) {
            if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
            if (List.class.isAssignableFrom(f.getType())) continue;
            try {
                Object v = f.get(o);
                String n = f.getName();
                if (v != null && (n.equals("VoucherDate") || n.equals("ChequeDate") || n.equals("DueDate")
                        || n.equals("EntryDate") || n.equals("ModifyDate") || n.equals("PostDate")
                        || n.equals("DCheqDate") || n.equals("GpDate"))) {
                    String s = String.valueOf(v);
                    v = Timestamp.valueOf(s.length() == 10 ? s + " 00:00:00" : s);
                }
                m.put(n, v);
            } catch (IllegalAccessException ignored) { }
        }
        return m;
    }

    /** Conversion.ToDecimal(double) — 15 significant digits. */
    private static BigDecimal dec(double v) {
        return v == 0d ? BigDecimal.ZERO : new BigDecimal(v).round(new MathContext(15));
    }

    private static String xmlNum(double v) { return clr(v); }

    private static double round(double v, int p) {
        if (Double.isNaN(v) || Double.isInfinite(v)) return 0d;
        return new BigDecimal(v).setScale(p, RoundingMode.HALF_EVEN).doubleValue();
    }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static int i(Integer v) { return v == null ? 0 : v; }
    private static double d(Double v) { return v == null ? 0d : v; }
    private static boolean isBlank(String s) { return s == null || s.trim().isEmpty(); }
}
