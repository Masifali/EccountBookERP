package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.StockTransferManualDto;
import com.mst.repositories.StockTransferManualRepository;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.repositories.StoreStockTransferRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.lang.reflect.Field;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.clr;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Store Management (ModuleId 24) — screen 332 "Stock Transfer Manual".
 *
 * <ul>
 *   <li>ScreenName (base.Name): {@code frmStockTransferManual} — rights key (designer :2572).</li>
 *   <li>DocumentTypeId: 806 (Insert:967, DocumentNoFill:1279, gridhistoryfill:1790, slip 407).</li>
 *   <li>Route /store/stock-transfer-manual, API /api/store/stock-transfer-manual.</li>
 *   <li>Desktop form: Architecture.WinApp.StoreManagement/frmStockTransferManual.cs (5,595 lines; logic :352-2942,
 *       designer :2953-), loader dialog Architecture.WinApp.Production/LoadavailableTransactionsForIssuance.cs.</li>
 *   <li>BLL 0559 InvStockTransferHeader (GenerateCode, FormHistory, GetByID, AllComboAgainstStockTransfer,
 *       StockTransferSlipandRegister, GetRecordsById, Save/MakeVoucher); DAL 0412 InvStockTransferHeader
 *       (SetData, GetData); Models 1009 / 1008 / 0979 (header / detail / expense); BLL 0025
 *       WarehousesAllocationToBranch, BLL 0019 JobLotsAllocationToBranch; BLL 0579 InvPackingType.Getall,
 *       0571 InvCropYear.Getall, 0583 Item.ReadAllItems, 0610 UOMSchedule.SearchByObject, 0648 COAAllocation.GetAll,
 *       0580 InvSaleInvoice.GetWeightCurrStockByItem, 0056 GetAvgRatesAndStockInHand.AvgRateOnlyForCGS;
 *       BLL 0581 InvPurchaseInvoice.RemoveByID → DAL 0434 (Sp_InvoicesVouchersandStocksDelete); DAL 0205
 *       CommonServices (FIFOImplemention, GetItemGlIdsandItemName, GetUomScheduleIdByItemIdAndEquivalent,
 *       GetERPFeaturesByCompanyId, GetVoucherHeadId); loader: BLL 0125 / 0574 (same calls as screen 339).</li>
 *   <li>Shared reads are made through {@link StoreStockTransferRepository} and {@link StoreIssuanceRepository}
 *       (unchanged); reads only this form makes are on {@link StockTransferManualRepository}.</li>
 * </ul>
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 * ---------------------------------------------------------------------------------------------
 *  1. GenerateCode (DocumentNoFill:1275) sends no BranchesId (company-wide max+1) while
 *     Sp_InvStockTransferHeader_Insert recomputes DocNo per branch; "Data Save Successfully....  N" shows the
 *     generator's number, which can differ from the stored one.
 *  2. Insert:969 saves ProjectsId = the user's BranchesId; NoOfBranches, FromBranchId, ToBranchId,
 *     RefDocumentTypeId, TransitWarehouseId and OtherWeight are always 0.
 *  3. Gate Pass# and Ticket No# are free TextBoxes (digits only); their numbers are saved as GatePassId /
 *     WbTicketId without any lookup (Insert:976-977), and ReadById:1160 shows the stored ids back.
 *  4. "Factory Weight" is recomputed as the grid's GrossWeight total, formatted "#,##0.###", after "+",
 *     row Update, row delete and every close of the Loader (btnLoadInvoices_Click:2350), but it stays
 *     editable; the save compares it to the unrounded GrossWeight total and stops with "GrossWeight and
 *     WeighBridge Weight Not Match" — after every detail and other-charges check (Insert:1072).
 *  5. btnplus_Click:898 stores RateUOMId 0, RateUOM 40, ItemAmount = the Amount box's "#,#" text (rounded);
 *     ItemAmountCalculation:2240 (row Update only) recomputes every row as NetWeight / RateUOM * ItemRate.
 *  6. GetAvgRate:2290 always fills Avg Rate (AvgRateOnlyForCGS * 40), FIFO or not, and whether or not
 *     StockTransferFinancialEffectIsActive shows the box, so ItemRate / ItemAmount are saved anyway. Avg Rate
 *     stays editable (designer :1093, no ReadOnly).
 *  7. WeightCalculation:2095 — when EbUnit is "0" (not empty) EbTotal's box keeps its previous text while the
 *     net weight is computed with EbTotal 0.
 *  8. Loader rows (LoadDataDetailfromPurchaseInvoivce:2358) remove every manual row; a re-loaded reference
 *     adds to QTY and BalWeight but not to NetWeight / GrossWeight; "+" refuses once a loader row exists.
 *     Editing a loader row disables WareHouse From / Item / Pack UOM / Crop Year / Job Lot From, and only
 *     New / Reset (or editing a manual row) enables them again. The Loader drops manual rows from the grid
 *     without adding their ids to DetailRowsRemoveIds (:2378-2380, unlike the X button :2741), so on an opened
 *     document those rows stay in the database after saving (reproduced).
 *  9. btnUpdateDetail_Click:805 empties the Item Name list when Trans.Type is "Outward"; only Refresh
 *     (ItemDetailFill) fills it again.
 * 10. The history's Transfer Type combo is filled from AllComboAgainstStockTransfer with DocumentTypeIds "68"
 *     (the other stock-transfer screen), and its text is sent as the TransferType filter. FormHistory gets no
 *     BranchesId (gridhistoryfill:1786 never sets it).
 * 11. History "TicketNo" shows WbTicketId and "GPNo" the GatePassId (gridhistoryfill:1866); WBWeight uses the
 *     format "#,##0,.##" — the comma before the point divides by 1000 — while its total uses "#,##0.##".
 * 12. MakeVoucher (BLL 0559:80) is built before the header id exists and before the DAL's FIFO rewrite of
 *     ItemRate / ItemAmount; USP_VoucherBalanceCheck gets @Id = the stock transfer id (DAL 0412:317).
 * 13. The DAL's detail-row removal (USP_InvStockTransferDetailRowsDeleteByIds, 806 only) runs after
 *     Sp_InventoryTransactions_GetALLMethod and before USP_InventoryValidation (DAL 0412:333).
 * 14. DocDate_ValueChanged is empty: the avg rates are only re-read by "Generate Rate"
 *     (AvgRateRecalculateOnDocDateChange:2892), which stops at the first loader row, skips under FIFO,
 *     and leaves a row unchanged when the rate is 0.
 * 15. Delete goes through InvPurchaseInvoice.RemoveByID (Sp_InvoicesVouchersandStocksDelete, 806).
 * 16. No DateLock check: this BLL never checks the lock date.
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (web only, with reason)
 * ---------------------------------------------------------------------------------------------
 *  1. DocNo is the generator's on insert and the stored header's on update; never taken from the page.
 *  2. A document is only opened, updated, printed or deleted when it belongs to the user's organization /
 *     company and is DocumentTypeId 806; a header procedure that returns no id aborts the save.
 *  3. Rights (Save / Update / Delete) and IsApproved are re-checked on the server from the stored header.
 *  4. Posted detail Ids: on insert every row Id is 0; on update a row Id must be one of the saved document's
 *     own detail rows (Sp_InvStockTransferDetail_Insert UPDATEs by Id alone).
 *  5. DetailRowsRemoveIds is reduced to the saved document's own detail ids (the procedure already filters by
 *     header, so the effect is the same) and is ignored on insert.
 *  6. Loader references (RefDocumentTypeId / RefDocNoId / RefDocSubIdNo) are re-validated: the triple must be
 *     an InventoryStockEvalautionDetail row of the user's organization / company (the table the loader lists).
 *  7. The BalWeight / NetWeight check uses the posted BalWeight, as the desktop uses its grid cell.
 *
 * NOT PORTED: attachments (btnAttachment, AttachmentsList / DeleteAttachmentsList, DeleteAllAttachmentsFromDb,
 * the history's NoOfAttachments link), saved grid layouts (ctrlGrdBar*), keyboard shortcuts, the post-save Wages
 * Bill dialog (frmwagesBillHeader — WagesCompulsoryOnStockTransfer && WagesRefDocumentsStatus 806), the hidden
 * Branch / Project combos (cmbbranch1 / combproject1, Visible = false, never read by the save), the Crystal
 * layout 407-InvStockTransferSlip.rpt (the slip procedure's rows are returned instead).
 */
@Service
public class StockTransferManualService {

    public static final int DOC = 806;
    public static final String SCREEN = "frmStockTransferManual";
    private static final int FIFO_FEATURE = 5;
    private static final int MULTI_BRANCH_FEATURE = 11;

    private final StockTransferManualRepository own;
    private final StoreStockTransferRepository repo;
    private final StoreIssuanceRepository shared;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public StockTransferManualService(StockTransferManualRepository own, StoreStockTransferRepository repo,
                                      StoreIssuanceRepository shared, StoreScreenRights rights, CurrentUserContext ctx) {
        this.own = own;
        this.repo = repo;
        this.shared = shared;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ================================================================================= load

    /** PurchsaeOrder_Load:372. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN));                                                        // :376
        out.put("financialEffect", toBool(shared.config(u, "StockTransferFinancialEffectIsActive")));  // :386
        out.put("wagesCompulsory", toBool(shared.config(u, "WagesCompulsoryOnStockTransfer")));        // :387
        out.put("fifo", repo.erpFeature(u, FIFO_FEATURE));                                           // :396
        out.put("multiBranch", repo.erpFeature(u, MULTI_BRANCH_FEATURE));                            // :397
        out.put("historyTransferTypes", historyTransferTypes(u));                                    // HistoryComboFill:1732
        out.put("accounts", accounts(u));                                                            // AccountsFill:620
        out.put("docNo", repo.generateCode(u, DOC, fy));                                             // DocumentNoFill:1265
        out.putAll(combos(u));                                                                       // WareHouseFill … ItemDetailFill
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(shared.config(u, "DefaultDaysToLessFromHistoryFromDate"))); // :506
        return out;
    }

    /** WareHouseFill:1213, GetPackingType:1336, GetCropYear:1354, GetJobLot:1389, ItemDetailFill:1293 — also btnRefresh_Click:1441. */
    public Map<String, Object> combos() { return combos(ctx.requireAccountingUser()); }

    private Map<String, Object> combos(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("warehouses", project(own.warehousesAllocatedToBranch(u, branch(u)), "Id", "WareHouseName"));
        out.put("packingTypes", project(repo.packingTypes(), "Id", "PackTypeDesc"));
        out.put("cropYears", project(repo.cropYears(u), "Id", "CropYear"));
        out.put("jobLots", project(own.jobLotsAllocatedToBranch(u, branch(u)), "Id", "JobLotDescription"));
        out.put("items", project(repo.readAllItems(u), "Id", "ItemName"));
        return out;
    }

    /** DocumentNoFill:1265 — Reset:1506 calls it too. */
    public int docNo() {
        UserAccount u = ctx.requireAccountingUser();
        return repo.generateCode(u, DOC, ctx.currentFinancialYearId());
    }

    /** AccountsFill:631 — AccountTypeId not in 2, 11, 12, 15. */
    private List<Map<String, Object>> accounts(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.coaAllocation(u)) {
            int t = toInt(r.get("AccountTypeId"));
            if (t == 2 || t == 11 || t == 12 || t == 15) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("AccountTitle", str(r.get("AccountTitle")));
            out.add(o);
        }
        return out;
    }

    /** HistoryComboFill:1732 — DocumentTypeIds "68" (D10); btnRefreshHistory_Click:2840 re-reads it. */
    public List<Map<String, Object>> historyTransferTypes() { return historyTransferTypes(ctx.requireAccountingUser()); }

    private List<Map<String, Object>> historyTransferTypes(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.allCombo(u, "68")) {
            if (!"TransferType".equals(str(r.get("Activity")))) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("TransferType", str(r.get("ReferenceName")));
            out.add(o);
        }
        return out;
    }

    // ================================================================================= entry bar

    /** bindRateUomAndItemPackUom:1310 — Id / Equivalent. */
    public List<Map<String, Object>> uoms(int itemId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.uomsByItem(ctx.requireAccountingUser(), itemId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Equivalent", toDouble(r.get("Equivalent")));
            out.add(o);
        }
        return out;
    }

    /** AvailableStockGetByItem:2052 — ToDate = DocDate.Value; "0" when no row. */
    public String availableStock(int recId, int itemId, int warehouseId, int jobLotId, String cropYear, String docDate) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> r = repo.weightCurrStock(u, itemId, StoreIssuanceService.formDate(docDate, recId),
                warehouseId, jobLotId, cropYear == null ? "" : cropYear.trim());
        return r.isEmpty() ? "0" : str(r.get(0).get("AvailableStock"));
    }

    /**
     * GetAvgRate:2294 (CropYearId, no crop text) and AvgRateRecalculateOnDocDateChange:2915 (crop text, id 0):
     * AvgRateOnlyForCGS(item, DocDate.Value, 806, RecId, jobLot, …, warehouseFrom) * 40. No FIFO test here —
     * GetAvgRate never makes one (D6); the Generate Rate caller checks FIFO itself.
     */
    public double avgRate(int recId, int itemId, String docDate, int jobLotId, int cropYearId, String cropYear, int warehouseId) {
        UserAccount u = ctx.requireAccountingUser();
        return repo.avgRateOnlyForCgs(u, itemId, StoreIssuanceService.formDate(docDate, recId), DOC, recId,
                jobLotId, cropYearId, cropYear, warehouseId) * 40.0;
    }

    // ================================================================================= loader dialog

    /** LoadavailableTransactionsForIssuance.StockComboFill:197 — BranchesIds = the user's branch as text. */
    public Map<String, Object> loaderLookups() {
        UserAccount u = ctx.requireAccountingUser();
        String[] kinds = { "ParentCategories", "ItemCategories", "ItemTypes", "JobLot", "CropYear", "Warehouse",
                "DocumentType", "Supplier_Customer", "Items" };
        Map<String, List<Map<String, Object>>> by = new LinkedHashMap<>();
        for (String k : kinds) by.put(k, new ArrayList<>());
        for (Map<String, Object> r : repo.loaderDropDowns(u, String.valueOf(branch(u)))) {
            List<Map<String, Object>> t = by.get(str(r.get("ActivityType")));
            if (t == null) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("Id"));
            o.put("name", str(r.get("name")));
            t.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>(by);
        out.put("fromDate", repo.financialYearStart(u, ctx.currentFinancialYearId()));   // FromDate = ActiveYr.Start_Period
        return out;
    }

    /** PendingInventoryTransactionsForIssuanceLoad:319 — both pickers carry their time of day. */
    public List<Map<String, Object>> loaderSearch(String from, String to, int parent, int category, int type, int jobLot,
                                                  String cropYear, int warehouseId, int refDocType, int supplierId, int itemId) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.loaderAvailable(u, branch(u), StoreIssuanceService.pickerDate(from),
                StoreIssuanceService.pickerDate(to), supplierId, warehouseId, itemId, refDocType, jobLot, parent,
                category, type, cropYear)) {
            Map<String, Object> o = new LinkedHashMap<>(r);
            int rdt = toInt(r.get("RefDocumentTypeId"));
            if (rdt == 112 || rdt == 80) o.put("GrnNo", 0);                                 // :398
            o.put("Remarks", r.get("TranRemarks"));
            out.add(o);
        }
        return out;
    }

    // ================================================================================= history

    /** gridhistoryfill:1780 — no BranchesId, NoOfRecords 0 (omitted). */
    public List<Map<String, Object>> history(String dateType, String from, String to, int fromDocNo, int toDocNo,
                                             String transferType) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN, "viewAll");
        Timestamp f = isBlank(from) ? null : StoreIssuanceService.pickerDate(from);
        Timestamp t = isBlank(to) ? null : StoreIssuanceService.pickerDate(to);
        String k = dateType == null ? "doc" : dateType;
        List<Map<String, Object>> rows = repo.formHistory(u, DOC, viewAll, ctx.currentFinancialYearId(), 0, u.getId(),
                "doc".equals(k) ? f : null, "doc".equals(k) ? t : null,
                "entry".equals(k) ? f : null, "entry".equals(k) ? t : null,
                "modify".equals(k) ? f : null, "modify".equals(k) ? t : null,
                "approved".equals(k) ? f : null, "approved".equals(k) ? t : null,
                fromDocNo, toDocNo, transferType == null ? "" : transferType);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {                                                  // :1866
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("DocDate", ci(r, "DocDate"));
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("TransType", ci(r, "TransferType"));
            o.put("GPNo", str(ci(r, "GatePassId")));
            o.put("TicketNo", str(ci(r, "WBTicketId")));
            o.put("WBWeight", toDouble(ci(r, "WbNetWeight")));
            o.put("Remarks", ci(r, "RemarksHeader"));
            o.put("FromBranchName", ci(r, "FromBranchName"));
            o.put("ToBranchName", ci(r, "ToBranchName"));
            o.put("EntryDate", ci(r, "EntryDate"));
            o.put("EntryUser", ci(r, "EntryUserName"));
            o.put("ModifyDate", ci(r, "ModifyDate"));
            o.put("ModifyUser", ci(r, "ModifyUserName"));
            o.put("NoOfAttachments", ci(r, "NoOfAttachments"));
            out.add(o);
        }
        return out;
    }

    /** grdhistory_SelectionChanged:2609 — GetByID's details (15 columns). */
    public List<Map<String, Object>> historyDetail(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (owned(u, id) == null) return null;
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.details(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("FromWarehouse", str(ci(d, "FromWareHouseName")));
            o.put("ToWarehouse", str(ci(d, "ToWareHouseName")));
            o.put("JobLotFrom", str(ci(d, "JobLotDescription")));
            o.put("JobLotTo", str(ci(d, "JobLotTo")));
            o.put("CropYear", str(ci(d, "CropYear")));
            o.put("ItemName", str(ci(d, "ItemName")));
            o.put("PackingType", str(ci(d, "PackTypeDesc")));
            o.put("Qty", toDouble(ci(d, "Qty")));
            o.put("PackUom", str(ci(d, "PackUom")));
            o.put("GrossWeight", toDouble(ci(d, "GrossWeight")));
            o.put("EbUnit", toDouble(ci(d, "EbUnit")));
            o.put("EbTotal", toDouble(ci(d, "EbTotal")));
            o.put("AddLessWt", toDouble(ci(d, "AdLsWeight")));
            o.put("StockWeight", toDouble(ci(d, "NetWeight")));
            o.put("RemarksDetail", str(ci(d, "RemarksDetail")));
            out.add(o);
        }
        return out;
    }

    // ================================================================================= read

    /** ReadById:1143. */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = owned(u, id);
        if (h == null) return null;
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(ci(h, "Id")));
        out.put("BranchesId", toInt(ci(h, "BranchesId")));
        out.put("ProjectsId", toInt(ci(h, "ProjectsId")));
        out.put("DocDate", ci(h, "DocDate"));
        out.put("DocNo", toInt(ci(h, "DocNo")));
        out.put("RemarksHeader", str(ci(h, "RemarksHeader")));
        out.put("WbNetWeight", clr(toDouble(ci(h, "WbNetWeight"))));                 // :1159 Conversion.ToString(double)
        out.put("GatePassId", String.valueOf(toInt(ci(h, "GatePassId"))));           // :1160
        out.put("WbTicketId", String.valueOf(toInt(ci(h, "WbTicketId"))));           // :1161
        out.put("TransferType", str(ci(h, "TransferType")));
        out.put("WorkingReportNo", String.valueOf(toInt(ci(h, "WorkingReportNo")))); // virtual int → "0" when none
        out.put("IsApproved", toBool(str(ci(h, "IsApproved"))));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.details(id)) {                               // :1166
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(d, "Id")));
            o.put("RefDocumentTypeId", toInt(ci(d, "RefDocumentTypeId")));
            o.put("RefDocNoId", toInt(ci(d, "RefDocNoId")));
            o.put("RefDocSubIdNo", toInt(ci(d, "RefDocSubIdNo")));
            o.put("ItemId", toInt(ci(d, "ItemId")));
            o.put("ItemName", str(ci(d, "ItemName")));
            o.put("PackTypeId", toInt(ci(d, "InvPackingTypeId")));
            o.put("PackType", str(ci(d, "PackTypeDesc")));
            o.put("CropYear", str(ci(d, "CropYear")));
            o.put("JobLotId", toInt(ci(d, "JobLotId")));
            o.put("JobLot", str(ci(d, "JobLotDescription")));
            o.put("JobLotIdTo", toInt(ci(d, "JobLotIdTo")));
            o.put("JobLotTo", str(ci(d, "JobLotTo")));
            o.put("QTY", toDouble(ci(d, "Qty")));
            o.put("PackUOMId", toInt(ci(d, "PackUomId")));
            o.put("PackUOM", ci(d, "Equivalent") == null ? "" : clr(toDouble(ci(d, "Equivalent"))));
            o.put("GrossWeight", toDouble(ci(d, "GrossWeight")));
            o.put("EbUnit", toDouble(ci(d, "EbUnit")));
            o.put("EbTotal", toDouble(ci(d, "EbTotal")));
            o.put("AdLsWeight", toDouble(ci(d, "AdLsWeight")));
            o.put("NetWeight", toDouble(ci(d, "NetWeight")));
            o.put("BalQty", toDouble(ci(d, "Qty")));                                   // :1168 Qty again
            o.put("BalWeight", toDouble(ci(d, "NetWeight")));                          //       NetWeight again
            o.put("WareHouseFromId", toInt(ci(d, "FromWarehouseId")));
            o.put("WareHouseFrom", str(ci(d, "FromWareHouseName")));
            o.put("WareHouseToId", toInt(ci(d, "ToWarehouseId")));
            o.put("WareHouseTo", str(ci(d, "ToWareHouseName")));
            o.put("Remarks", str(ci(d, "RemarksDetail")));
            o.put("ItemRate", toDouble(ci(d, "ItemRate")));
            o.put("RateUOMId", toInt(ci(d, "RateUomId")));
            o.put("RateUOM", toDouble(ci(d, "RateUom")));
            o.put("ItemAmount", toDouble(ci(d, "ItemAmount")));
            o.put("Expense", toDouble(ci(d, "ExpenseAmount")));
            rows.add(o);
        }
        out.put("rows", rows);
        List<Map<String, Object>> exp = new ArrayList<>();
        for (Map<String, Object> e : repo.expenses(id)) {                              // :1174
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Account", toInt(ci(e, "AccountId")));
            o.put("Percentage", toDouble(ci(e, "Percentage")));
            o.put("Qty", toDouble(ci(e, "Qty")));
            o.put("Rate", toDouble(ci(e, "Rate")));
            o.put("Amount", toDouble(ci(e, "Amount")));
            o.put("Remarks", str(ci(e, "Remarks")));
            exp.add(o);
        }
        out.put("expenses", exp);
        return out;
    }

    /** CommonServices.StockTransferSlip407:7737 — null when not the user's document. */
    public List<Map<String, Object>> slip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (owned(u, id) == null) return null;
        return own.slip407(u, DOC, id);
    }

    // ================================================================================= delete

    /** btnDelete_Click:2852 → BLL 0581 InvPurchaseInvoice.RemoveByID (DocumentTypeId 806, EntryUser = user). */
    @Transactional
    public Map<String, Object> delete(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN, "delete")) throw new IllegalStateException("You do not have the Delete right for this screen.");
        if (id <= 0) throw new IllegalArgumentException("RecordId Not Found.....");
        if (owned(u, id) == null) throw new IllegalArgumentException("RecordId Not Found.....");
        shared.removeInvoiceVoucherAndStock(u, DOC, id, u.getId());
        return ok("Delete Record Successfully", id);
    }

    // ================================================================================= save

    /** btnsave_Click:1111 / btnupdate_Click:1124 → Insert():912 → BLL 0559 Save → DAL 0412 SetData. */
    @Transactional
    public Map<String, Object> save(StockTransferManualDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.Id == null ? 0 : dto.Id;
        if (recId == 0 && !rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> existing = null;
        if (recId != 0) {
            existing = owned(u, recId);
            if (existing == null) throw new IllegalArgumentException("Record Not Found");
            if (toBool(str(ci(existing, "IsApproved")))) throw new IllegalArgumentException("Approved Record Not Update"); // :1130
        }
        int docNo = existing != null ? toInt(ci(existing, "DocNo")) : repo.generateCode(u, DOC, fy);     // Deviation 1
        String transferType = dto.TransferType == null ? "" : dto.TransferType;
        boolean financialEffect = toBool(shared.config(u, "StockTransferFinancialEffectIsActive"));

        /* FormValidation:517 */
        if (docNo == 0) throw new IllegalArgumentException("DocNo Field is Required");
        if (!"Inward".equals(transferType) && !"Outward".equals(transferType) && !"MoveOrder".equals(transferType)) {
            throw new IllegalArgumentException("Transfer Type Field is Required");
        }
        if (dto.rows == null || dto.rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");   // :926

        /* :945 — other charges */
        List<StockTransferManualDto.Expense> exps = dto.expenses == null ? new ArrayList<>() : dto.expenses;
        for (StockTransferManualDto.Expense r : exps) {
            if (!(d(r.Amount) > 0.0)) continue;
            if (i(r.Account) == 0) throw new IllegalArgumentException("Please Select an Account Against OtherCharges Amount First");
            if (r.Remarks == null || r.Remarks.isEmpty()) throw new IllegalArgumentException("Grid Charge to Product Remarks required Please Check");
        }

        Timestamp docDate = StoreIssuanceService.formDate(dto.DocDate, recId);
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        Map<String, Object> sh = new LinkedHashMap<>();                                // Model 1009, declaration order
        sh.put("Id", recId);
        sh.put("DocNo", docNo);
        sh.put("DocDate", docDate);                                                    // :970
        sh.put("GatePassId", textInt(dto.GatePass));                                   // :976
        sh.put("WbTicketId", textInt(dto.TicketNo));                                   // :977
        double wbNetWeight = textDouble(dto.WbNetWeight);
        sh.put("WbNetWeight", wbNetWeight);                                            // :978
        sh.put("OtherWeight", 0d);
        sh.put("RemarksHeader", dto.RemarksHeader == null ? "" : dto.RemarksHeader);  // :975
        sh.put("TransferType", transferType);                                          // :979
        sh.put("EntryDate", now);                                                      // BLL Save:171
        sh.put("EntryUser", u.getId());
        sh.put("ModifyDate", now);
        sh.put("ModifyUser", u.getId());
        sh.put("IsApproved", false);
        sh.put("ApprovedUserId", 0);
        sh.put("ApprovedDate", null);
        sh.put("TransitWarehouseId", 0);
        sh.put("RefDocumentTypeId", 0);
        sh.put("OrganizationId", u.getOrganizationId());
        sh.put("CompanyId", u.getCompanyId());
        sh.put("BranchesId", branch(u));                                               // :968
        sh.put("ProjectsId", branch(u));                                               // :969 (D2)
        sh.put("DocumentTypeId", DOC);
        sh.put("FinancialYearId", fy);
        sh.put("NoOfBranches", 0);
        sh.put("FromBranchId", 0);
        sh.put("ToBranchId", 0);

        /* Deviation 4 / 5 — the saved document's own detail ids. */
        Set<Integer> ownDetailIds = new HashSet<>();
        if (recId != 0) for (Map<String, Object> d0 : repo.details(recId)) ownDetailIds.add(toInt(ci(d0, "Id")));
        String removeIds = "";
        if (recId != 0 && dto.DetailRowsRemoveIds != null) {
            Set<Integer> postedIds = new HashSet<>();                                  // an id still posted as a row is not removed
            if (dto.rows != null) for (StockTransferManualDto.Row r0 : dto.rows) if (r0 != null) postedIds.add(i(r0.Id));
            StringBuilder sb = new StringBuilder();
            for (String tok : dto.DetailRowsRemoveIds.split(",")) {
                int v = textInt(tok);
                if (v > 0 && ownDetailIds.contains(v) && !postedIds.contains(v)) sb.append(',').append(v);
            }
            removeIds = sb.toString();
        }

        /* :991 detail loop */
        List<Map<String, Object>> details = new ArrayList<>();
        double grossWeight = 0.0;
        for (StockTransferManualDto.Row r : dto.rows) {
            Map<String, Object> vd = detailModel();
            int detailId = recId != 0 ? i(r.Id) : 0;
            if (detailId != 0 && !ownDetailIds.contains(detailId)) {                   // negative ids rejected too
                throw new IllegalArgumentException("Detail row " + detailId + " does not belong to this document.");
            }
            vd.put("Id", detailId);
            int rdt = i(r.RefDocumentTypeId), rno = i(r.RefDocNoId), rsub = i(r.RefDocSubIdNo);
            vd.put("RefDocumentTypeId", rdt);
            vd.put("RefDocNoId", rno);
            vd.put("RefDocSubIdNo", rsub);
            if (rdt > 0 && d(r.NetWeight) > d(r.BalWeight)) {
                throw new IllegalArgumentException("NetWeight cannot be greater than BalWeight Please Check!");
            }
            if (i(r.ItemId) == 0) throw new IllegalArgumentException("Item Name Field Required In Detail Grid...");
            vd.put("ItemId", i(r.ItemId));
            if (i(r.JobLotId) == 0) throw new IllegalArgumentException("JobLot From Field Required In Detail Grid...");
            vd.put("JobLotId", i(r.JobLotId));
            if (i(r.JobLotIdTo) == 0) throw new IllegalArgumentException("JobLot To Field Required In Detail Grid...");
            vd.put("JobLotIdTo", i(r.JobLotIdTo));
            if (i(r.WareHouseFromId) == 0) throw new IllegalArgumentException("WareHouseFrom  Field Required In Detail Grid...");
            vd.put("FromWarehouseId", i(r.WareHouseFromId));
            if (i(r.WareHouseToId) == 0) throw new IllegalArgumentException("WareHouseTo Field Required In Detail Grid...");
            vd.put("ToWarehouseId", i(r.WareHouseToId));
            vd.put("InvPackingTypeId", i(r.PackTypeId));
            vd.put("CropYear", r.CropYear == null ? "" : r.CropYear);
            vd.put("Qty", d(r.QTY));
            vd.put("PackUomId", i(r.PackUOMId));
            vd.put("GrossWeight", d(r.GrossWeight));
            grossWeight += d(r.GrossWeight);
            vd.put("AdLsWeight", d(r.AdLsWeight));
            vd.put("EbUnit", d(r.EbUnit));
            vd.put("EbTotal", d(r.EbTotal));
            vd.put("NetWeight", d(r.NetWeight));
            vd.put("ItemRate", d(r.ItemRate));
            vd.put("RateUomId", i(r.RateUOMId));
            vd.put("ItemAmount", d(r.ItemAmount));
            vd.put("ExpenseAmount", d(r.Expense));
            vd.put("RemarksDetail", r.Remarks == null ? "" : r.Remarks);
            if (financialEffect) {                                                      // :1043
                if (d(r.ItemRate) <= 0.0) throw new IllegalArgumentException("AvgRate field required");
                if (d(r.ItemAmount) <= 0.0) throw new IllegalArgumentException("Amount field required");
            }
            /* Deviation 6 */
            if ((rdt > 0 || rno > 0 || rsub > 0) && !own.loaderReferenceExists(u, rdt, rno, rsub)) {
                throw new IllegalArgumentException("Loader reference " + rdt + "/" + rno + "/" + rsub + " was not found for this company.");
            }
            details.add(vd);
        }
        /* :1057 expenses with an account */
        List<Map<String, Object>> expenses = new ArrayList<>();
        for (StockTransferManualDto.Expense r : exps) {
            if (i(r.Account) == 0) continue;
            Map<String, Object> pf = new LinkedHashMap<>();                             // Model 0979 order
            pf.put("Amount", d(r.Amount));
            pf.put("Percentage", d(r.Percentage));
            pf.put("Qty", d(r.Qty));
            pf.put("Rate", d(r.Rate));
            pf.put("AccountId", i(r.Account));
            pf.put("Id", 0);
            pf.put("InvStockTransferHeaderId", 0);
            pf.put("Remarks", r.Remarks == null ? "" : r.Remarks);
            expenses.add(pf);
        }
        if (wbNetWeight != grossWeight) throw new IllegalArgumentException("GrossWeight and WeighBridge Weight Not Match"); // :1072 (D4)

        /* BLL Save:167 */
        Voucher voucher = makeVoucher(u, sh, details, expenses);
        String proc;
        if (recId == 0) {
            sh.put("ModifyUser", 0);
            proc = "Sp_InvStockTransferHeader_Insert";
        } else {
            sh.put("EntryUser", 0);
            own.recordDate(recId);                                                      // PreviousDate = GetRecordsById(Id)
            proc = "Sp_InvStockTransferHeader_Update";
        }
        int id = setData(u, sh, details, expenses, voucher, removeIds, proc);
        return ok(recId > 0 ? "Data Update Successfully....  " + docNo : "Data Save Successfully....  " + docNo, id);
    }

    /** BLL 0559 MakeVoucher:80 (same code as screen 339's port). */
    private Voucher makeVoucher(UserAccount u, Map<String, Object> obj, List<Map<String, Object>> details,
                                List<Map<String, Object>> expenses) {
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        vh.DocumentTypeId = DOC;
        vh.DocumentTypeSrNo = toInt(obj.get("Id"));
        vh.RefDocNoId = toInt(obj.get("Id"));
        vh.VoucherCode = toInt(obj.get("DocNo"));
        vh.VoucherDate = String.valueOf(obj.get("DocDate"));
        vh.Remarks = str(obj.get("RemarksHeader"));
        vh.RemarksOtherLingo = "";
        vh.ChequeDate = Timestamp.valueOf(LocalDate.now().atStartOfDay()).toString();
        vh.IncludeWHT = false;
        vh.BranchId = toInt(obj.get("BranchesId"));
        vh.ProjectId = toInt(obj.get("ProjectsId"));
        vh.ManualBillNo = "0";                         // Conversion.ToString(obj.WorkingReportNo) — never set by Insert()
        String now = Timestamp.valueOf(LocalDateTime.now()).toString();
        vh.DueDate = now;
        vh.OrganizationId = u.getOrganizationId();
        vh.CompanyId = u.getCompanyId();
        vh.FinancialYearId = toInt(obj.get("FinancialYearId"));
        vh.EntryUser = toInt(obj.get("EntryUser"));
        vh.EntryDate = now;
        vh.ModifyDate = now;
        vh.ModifyUser = toInt(obj.get("ModifyUser"));
        List<ContraVoucherDto.Detail> lines = new ArrayList<>();
        List<Map<String, Object>> gl = shared.itemGlAccounts(u);          // CommonServies.GetItemListForFinancialEffects
        if (!details.isEmpty() && !expenses.isEmpty()) {
            double total = 0.0;
            for (Map<String, Object> item : details) {
                if (gl.isEmpty()) throw new IllegalArgumentException("Item Record Not found");
                Map<String, Object> g = null;
                for (Map<String, Object> x : gl) if (toInt(ci(x, "Id")) == toInt(item.get("ItemId"))) { g = x; break; }
                if (g == null) continue;
                String c = "Item: " + str(ci(g, "ItemName")) + ",   Qty: " + clr(toDouble(item.get("Qty")))
                        + ",   Weight: " + clr(toDouble(item.get("NetWeight"))) + ",   Rate:" + clr(toDouble(item.get("ItemRate")))
                        + "  Item Amount: " + clr(toDouble(item.get("ItemAmount"))) + "  ExpenseAmount: " + clr(toDouble(item.get("ExpenseAmount")));
                ContraVoucherDto.Detail l = new ContraVoucherDto.Detail();
                l.AccountId = toInt(ci(g, "PurchaseGLAC"));
                l.AgainstAccountId = l.AccountId;
                l.Comments = c;
                l.DebitAmount = toDouble(item.get("ExpenseAmount"));
                l.ItemId = toInt(item.get("ItemId"));
                l.QtyIn = toDouble(item.get("Qty"));
                l.ItemRate = toDouble(item.get("ItemRate"));
                l.WeightIn = toDouble(item.get("NetWeight"));
                l.ItemAmount = toDouble(item.get("ItemAmount"));
                l.BranchesId = toInt(obj.get("BranchesId"));
                lines.add(l);
            }
            for (Map<String, Object> e : expenses) {
                ContraVoucherDto.Detail l = new ContraVoucherDto.Detail();
                l.AccountId = toInt(e.get("AccountId"));
                l.AgainstAccountId = l.AccountId;
                l.Comments = str(e.get("Remarks"));
                l.QtyIn = toDouble(e.get("Qty"));
                l.ItemRate = toDouble(e.get("Rate"));
                l.CreditAmount = toDouble(e.get("Amount"));
                l.BranchesId = toInt(obj.get("BranchesId"));
                total += toDouble(e.get("Amount"));
                lines.add(l);
            }
            vh.VoucherAmount = total;
            vh.BillAmount = total;
        }
        Voucher v = new Voucher();
        v.head = vh;
        v.lines = lines;
        return v;
    }

    /** VoucherHead with its voucherDetailList. */
    private static final class Voucher {
        ContraVoucherDto.Head head;
        List<ContraVoucherDto.Detail> lines;
    }

    /** DAL 0412 SetData:18 — one transaction (the caller's @Transactional), calls in the DAL's order. */
    private int setData(UserAccount u, Map<String, Object> obj, List<Map<String, Object>> details,
                        List<Map<String, Object>> expenses, Voucher v, String removeIds, String procName) {
        ContraVoucherDto.Head voucher = v.head;
        int recId = toInt(obj.get("Id"));
        int num = repo.setProc(procName, obj);                                           // :51
        if (num > 0) obj.put("Id", num); else num = recId;
        if (num <= 0) throw new IllegalStateException("Save returned no document id.");   // Deviation 2
        String transferType = str(obj.get("TransferType"));
        int modifyUser = toInt(obj.get("ModifyUser"));

        boolean flag = false;                                                            // :61
        int line = 0;
        for (Map<String, Object> d : details) {
            d.put("LineId", ++line);
            if (toInt(d.get("RefDocumentTypeId")) > 0 && toInt(d.get("RefDocNoId")) > 0 && toInt(d.get("RefDocSubIdNo")) > 0) flag = true;
        }
        boolean flag3 = repo.erpFeature(u, FIFO_FEATURE);                                // :70 DocumentTypeId != 807
        boolean fifoPath = flag3 && !flag && !"Inward".equals(transferType);
        List<Map<String, Object>> source = new ArrayList<>();
        List<Map<String, Object>> evalList = new ArrayList<>();
        if (flag3 && !flag) {                                                            // :74
            source = shared.itemGlAccounts(u);
            if (!"Inward".equals(transferType)) {
                for (Map<String, Object> item3 : details) {
                    String itemName = null;
                    for (Map<String, Object> x : source) if (toInt(ci(x, "Id")) == toInt(item3.get("ItemId"))) { itemName = str(ci(x, "ItemName")); break; }
                    List<Map<String, Object>> got = fifo(u, (Timestamp) obj.get("DocDate"), item3, modifyUser > 0 ? num : 0,
                            modifyUser > 0, itemName, evalList);
                    for (Map<String, Object> item5 : got) {
                        Map<String, Object> in = evalModel();
                        in.put("LineId", item5.get("LineId"));
                        in.put("ItemId", item5.get("ItemId"));
                        in.put("WarehouseId", toInt(item3.get("ToWarehouseId")));
                        in.put("RefWarehouseId", toInt(item3.get("FromWarehouseId")));
                        in.put("RateUom", item5.get("RateUom"));
                        in.put("JobLotId", toInt(item3.get("JobLotIdTo")));
                        in.put("InvPackingTypeId", item5.get("InvPackingTypeId"));
                        in.put("ItemUom", item5.get("ItemUom"));
                        in.put("CropBatch", str(item5.get("CropBatch")));
                        in.put("RefRefDocumentTypeId", 0);
                        in.put("RefRefDocIdNo", 0);
                        in.put("RefRefDocSubIdNo", 0);
                        in.put("QtyIn", toDouble(item5.get("QtyOut")));
                        in.put("BillWeightIn", toDouble(item5.get("BillWeightOut")));
                        in.put("StockWeightIn", toDouble(item5.get("StockWeightOut")));
                        in.put("ItemRate", toDouble(item5.get("CgsRate")));
                        in.put("AmountIn", toDouble(item5.get("CgsAmount")));
                        item5.put("RefWarehouseId", toInt(item3.get("ToWarehouseId")));
                        if ("MoveOrder".equals(transferType)) {
                            evalList.add(in);
                            evalList.add(item5);
                        } else if ("Outward".equals(transferType) && toInt(obj.get("NoOfBranches")) > 1) {
                            evalList.add(item5);                                        // never: NoOfBranches is 0 here
                        }
                    }
                }
            }
        }
        String text = "";
        for (Map<String, Object> item2 : details) {                                     // :144
            if (fifoPath) {
                for (Map<String, Object> x : source) if (toInt(ci(x, "Id")) == toInt(item2.get("ItemId"))) { text = str(ci(x, "ItemName")); break; }
                double amt = 0, wt = 0;
                for (Map<String, Object> x : evalList) {
                    if (toInt(x.get("ItemId")) == toInt(item2.get("ItemId")) && toInt(x.get("WarehouseId")) == toInt(item2.get("FromWarehouseId"))
                            && toInt(x.get("JobLotId")) == toInt(item2.get("JobLotId")) && toInt(x.get("InvPackingTypeId")) == toInt(item2.get("InvPackingTypeId"))
                            && toInt(x.get("ItemUom")) == toInt(item2.get("PackUomId")) && str(x.get("CropBatch")).equals(str(item2.get("CropYear")))
                            && toDouble(x.get("BillWeightOut")) > 0.0) {
                        amt += toDouble(x.get("CgsAmount"));
                        wt += toDouble(x.get("StockWeightOut"));
                    }
                }
                if (!(amt > 0.0) || !(wt > 0.0)) {
                    throw new IllegalArgumentException("CGS Amount or StockWeight cannot be equal to zero.this Item " + text + " against FIFO Method");
                }
                double rate = amt / wt;
                item2.put("ItemRate", rate);
                item2.put("ItemAmount", rate * toDouble(item2.get("NetWeight")));
                int rateUom = repo.uomScheduleIdByEquivalent(u, toInt(item2.get("ItemId")), 1.0);
                item2.put("RateUomId", rateUom);
                if (rateUom <= 0) throw new IllegalArgumentException("1KG Rate ScheduleId Not Found this Item " + text + " against FIFO Method");
            }
            item2.put("InvStockTransferHeaderId", num);
            item2.put("Id", repo.setProc("Sp_InvStockTransferDetail_Insert", item2));  // :175
        }
        for (Map<String, Object> e : expenses) {                                         // :178
            e.put("InvStockTransferHeaderId", num);
            repo.setProc("Sp_InvStockTransferExpense_Insert", e);
        }
        if (fifoPath) {                                                                  // :216
            if (evalList.isEmpty()) throw new IllegalArgumentException("InventoryStockEvalautionDetailslist not found against FIFO....");
            if (modifyUser > 0) {
                repo.exec("[dbo].[USP_InventoryQtyReverseAndDeleteByReferenceId]", params(
                        "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                        "RefDocumentTypeId", DOC, "RefDocIdNo", num));
            }
            for (Map<String, Object> item : evalList) {
                item.put("OrganizationId", u.getOrganizationId());
                item.put("CompanyId", u.getCompanyId());
                item.put("DocDate", obj.get("DocDate"));
                item.put("DocCodeNo", toInt(obj.get("DocNo")));
                item.put("BranchesId", toInt(obj.get("BranchesId")));
                item.put("RefDocumentTypeId", DOC);
                item.put("EntryUser", toInt(obj.get("EntryUser")));
                item.put("ModifyUser", modifyUser);
                item.put("CalcType", "Weight");
                if (!(toDouble(item.get("BillWeightIn")) > 0.0)) item.put("ItemRate", toDouble(item.get("CgsRate")));
                item.put("AmountOut", toDouble(item.get("CgsAmount")));
                int lid = toInt(item.get("LineId"));
                for (Map<String, Object> dd : details) {
                    if (lid > 0 && toInt(dd.get("LineId")) == lid) {
                        item.put("RefDocIdNo", toInt(dd.get("InvStockTransferHeaderId")));
                        item.put("RefDocSubIdNo", toInt(dd.get("Id")));
                        break;
                    }
                }
                repo.setProc("USP_InventoryStockEvalautionDetail_Insert", item);
            }
        } else {
            repo.evaluationUpdate(u, DOC, num);                                          // :266
        }
        if (!expenses.isEmpty() && !flag3 && !toBool(shared.config(u, "InventoryFinancialsEffectsInActive"))) { // :273
            int existing = shared.voucherHeadId(u, DOC, num);
            if (existing > 0) voucher.Id = existing;
            voucher.DocumentTypeSrNo = num;
            voucher.RefDocNoId = num;
            int num3 = repo.setProc(existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", model(voucher));
            if (num3 > 0) voucher.Id = num3;
            List<ContraVoucherDto.Detail> lines = v.lines;
            if (lines == null || lines.isEmpty()) throw new IllegalArgumentException("VoucherDetail List Not Found");
            for (ContraVoucherDto.Detail l : lines) {
                l.VoucherHeadId = voucher.Id;
                repo.setProc("Sp_VoucherDetail_Insert", model(l));
            }
            repo.exec("USP_VoucherBalanceCheck", params("OrganizationId", u.getOrganizationId(),
                    "CompanyId", u.getCompanyId(), "Id", num));                                  // D12
            int documentTypeIdRef = repo.setProc("Sp_VoucherHead_H_Insert", model(voucher));
            for (ContraVoucherDto.Detail l : lines) {
                l.VoucherHeadId = voucher.Id;
                l.DocumentTypeIdRef = documentTypeIdRef;
                repo.setProc("Sp_VoucherDetail_H_Insert", model(l));
            }
        }
        repo.inventoryTransactions(u, DOC, num);                                         // :327
        if (removeIds != null && !removeIds.isEmpty()) own.detailRowsDelete(u, DOC, num, removeIds); // :333 (D13)
        for (Map<String, Object> d : details) {                                          // :347
            repo.exec("USP_InventoryValidation", params(
                    "OrganizationId", u.getOrganizationId(),
                    "CompanyId", u.getCompanyId(),
                    "DocumentTypeId", DOC,
                    "DocDate", obj.get("DocDate"),
                    "ItemId", toInt(d.get("ItemId")),
                    "WarehouseId", toInt(d.get("FromWarehouseId")),
                    "JobLotId", toInt(d.get("JobLotId")),
                    "CropYear", d.get("CropYear"),
                    "InvPackingTypeId", toInt(d.get("InvPackingTypeId")),
                    "PackUomId", toInt(d.get("PackUomId")),
                    "RefDocumentTypeId", toInt(d.get("RefDocumentTypeId")),
                    "RefDocNoId", toInt(d.get("RefDocNoId")),
                    "RefDocSubIdNo", toInt(d.get("RefDocSubIdNo")),
                    "NetWeight", toDouble(d.get("NetWeight")),
                    "ItemConditionId", 0));
        }
        return num;
    }

    /** DAL 0205 FIFOImplemention:725 for one detail (stockUOM / JobLot / CropYear / PackingType sent: 806 != 807). */
    private List<Map<String, Object>> fifo(UserAccount u, Timestamp docDate, Map<String, Object> item3, int id,
                                           boolean update, String itemName, List<Map<String, Object>> reserve) {
        String name = itemName == null ? "" : itemName;
        int itemId = toInt(item3.get("ItemId"));
        Map<String, Object> p = params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ItemId", itemId, "DocDate", docDate);
        int packUom = toInt(item3.get("PackUomId")), wh = toInt(item3.get("FromWarehouseId")),
                jobLot = toInt(item3.get("JobLotId")), packing = toInt(item3.get("InvPackingTypeId"));
        String crop = str(item3.get("CropYear"));
        if (packUom != 0) p.put("PackUomId", packUom);
        if (wh != 0) p.put("WarehouseId", wh);
        if (jobLot != 0) p.put("JobLotId", jobLot);
        if (packing != 0) p.put("PackingTypeId", packing);
        if (!crop.isEmpty()) p.put("CropYear", crop);
        if (update) p.put("DocumentTypeId", DOC);
        if (update && id != 0) p.put("Id", id);
        if (!reserve.isEmpty()) p.put("FIFOXML", fifoXml(reserve));
        List<Map<String, Object>> list3 = repo.stockByFifo(p);
        if (list3.isEmpty()) throw new IllegalArgumentException("Stock Not Found this Item " + name + " against FIFO Method .....");
        double value2 = 0;
        for (Map<String, Object> r : list3) value2 += toDouble(ci(r, "NetBalWeight"));
        double itemQty = toDouble(item3.get("Qty")), netWeight = toDouble(item3.get("NetWeight"));
        if (!(netWeight <= BigDecimal.valueOf(value2).setScale(2, RoundingMode.HALF_EVEN).doubleValue())) {
            throw new IllegalArgumentException("Weight available is " + clr(value2) + " and row Weight is " + clr(netWeight)
                    + " this item " + name + " against FIFO....");
        }
        List<Map<String, Object>> out = new ArrayList<>();
        double num2 = netWeight, num3 = itemQty, num5 = 0, num7 = 0;
        for (Map<String, Object> s : list3) {
            if (toDouble(ci(s, "AvgRate")) <= 0.0) throw new IllegalArgumentException("Rate Not Found this Item " + name + " against FIFO Method");
            int rateUom = toInt(ci(s, "RateUomId"));
            if (rateUom == 0) throw new IllegalArgumentException("RateUomId not found  this " + name + " against FIFO Method");
            double num6 = toDouble(ci(s, "NetBalWeight")), num4 = toDouble(ci(s, "NetBalQty"));
            double eq = repo.equivalentByItemAndSchedule(u, itemId, rateUom);
            if (eq == 0.0) throw new IllegalArgumentException("RateUom Not Found");
            Map<String, Object> e = evalModel();
            e.put("Id", toInt(ci(s, "Id")));
            e.put("LineId", toInt(item3.get("LineId")));
            e.put("ItemId", itemId);
            e.put("WarehouseId", wh);
            e.put("RateUom", rateUom);
            e.put("JobLotId", jobLot);
            e.put("InvPackingTypeId", packing);
            e.put("ItemUom", packUom);
            e.put("CropBatch", crop);
            e.put("RefRefDocumentTypeId", toInt(ci(s, "RefDocumentTypeId")));
            e.put("RefRefDocIdNo", toInt(ci(s, "RefDocIdNo")));
            e.put("RefRefDocSubIdNo", toInt(ci(s, "RefDocSubIdNo")));
            double cgsRate = toDouble(ci(s, "AvgRate")) * eq;
            if (num6 <= num2 - num7) {
                num7 += num6;
                num5 += num4;
                e.put("QtyOut", num4);
                e.put("BillWeightOut", num6);
                e.put("StockWeightOut", num6);
                e.put("CgsRate", cgsRate);
                e.put("CgsAmount", num6 / eq * cgsRate);
                out.add(e);
            } else if (num6 >= num2 - num7) {
                double q = num3 - num5, w = num2 - num7;
                e.put("QtyOut", q);
                e.put("BillWeightOut", w);
                e.put("StockWeightOut", w);
                e.put("CgsRate", cgsRate);
                e.put("CgsAmount", w / eq * cgsRate);
                num5 += q;
                num7 += w;
                out.add(e);
            }
            if (netWeight == num7) break;
        }
        return out;
    }

    /** Conversion.ConvertListToXmlSerializer&lt;FIFOStockEvaluation&gt; — the proc reads the five reservation fields. */
    private static String fifoXml(List<Map<String, Object>> reserve) {
        StringBuilder sb = new StringBuilder("<ArrayOfFIFOStockEvaluation>");
        for (Map<String, Object> r : reserve) {
            sb.append("\n<FIFOStockEvaluation>")
              .append("<Id>0</Id><DocDate>0001-01-01T00:00:00</DocDate><ItemId>0</ItemId><WarehouseId>0</WarehouseId>")
              .append("<ItemUomId>0</ItemUomId><RateUomId>0</RateUomId>")
              .append("<RefDocumentTypeId>").append(toInt(r.get("RefRefDocumentTypeId"))).append("</RefDocumentTypeId>")
              .append("<RefDocIdNo>").append(toInt(r.get("RefRefDocIdNo"))).append("</RefDocIdNo>")
              .append("<RefDocSubIdNo>").append(toInt(r.get("RefRefDocSubIdNo"))).append("</RefDocSubIdNo>")
              .append("<QtyIn>0</QtyIn><QtyOut>0</QtyOut><BalQty>0</BalQty><WeightIn>0</WeightIn><WeightOut>0</WeightOut>")
              .append("<BalWeight>0</BalWeight><AmountIn>0</AmountIn><AmountOut>0</AmountOut><BalAmount>0</BalAmount><AvgRate>0</AvgRate>")
              .append("<ReserveWeight>").append(plain(toDouble(r.get("StockWeightOut")))).append("</ReserveWeight>")
              .append("<ReserveQty>").append(plain(toDouble(r.get("QtyOut")))).append("</ReserveQty>")
              .append("<NetBalQty>0</NetBalQty><NetBalWeight>0</NetBalWeight></FIFOStockEvaluation>");
        }
        return sb.append("\n</ArrayOfFIFOStockEvaluation>").toString();
    }

    private static String plain(double v) {
        if (v == 0d) return "0";
        return BigDecimal.valueOf(v).stripTrailingZeros().toPlainString();
    }

    /** Model 1023 InventoryStockEvalautionDetail — non-virtual properties with their CLR defaults (nulls omitted). */
    private static Map<String, Object> evalModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("IsApproved", false);
        p.put("DocDate", null);
        for (String k : new String[] { "AmountIn", "AmountOut", "BillWeightIn", "BillWeightOut", "CgsRate", "ExpenseAmountIn",
                "ItemRate", "QtyIn", "QtyOut", "StockWeightIn", "StockWeightOut", "CgsAmount" }) p.put(k, 0d);
        for (String k : new String[] { "BranchesId", "CompanyId", "DocCodeNo", "GpNoDcNo", "Id", "InvPackingTypeId", "ItemId",
                "ItemUom", "JobLotId", "OrderNo", "OrganizationId", "PrdJobOrderNo", "ProjectsId", "RateUom", "RefDocIdNo",
                "RefDocSubIdNo", "RefDocumentTypeId", "OtherDocumentTypeId", "OtherDocNoId", "OtherSubDocNoId",
                "SupplierCustomerId", "WarehouseId", "RefWarehouseId", "CityId", "LineId", "RefRefDocumentTypeId",
                "RefRefDocIdNo", "RefRefDocSubIdNo", "EntryUser", "ModifyUser", "InvoiceId", "InvoiceDetailId",
                "VarientId", "ItemConditionId" }) p.put(k, 0);
        for (String k : new String[] { "CalcType", "CropBatch", "TranRemarks", "VehicleNo", "BiltyNo" }) p.put(k, null);
        return p;
    }

    /** Model 1008 InvStockTransferDetail — non-virtual properties, declaration order, CLR defaults. */
    private static Map<String, Object> detailModel() {
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("Id", 0);
        p.put("InvStockTransferHeaderId", 0);
        p.put("ItemId", 0);
        p.put("InvPackingTypeId", 0);
        p.put("CropYearId", 0);
        p.put("CropYear", null);
        p.put("JobLotId", 0);
        p.put("JobLotIdTo", 0);
        p.put("PackUomId", 0);
        p.put("Qty", 0d);
        p.put("GrossWeight", 0d);
        p.put("AdLsWeight", 0d);
        p.put("NetWeight", 0d);
        p.put("EbUnit", 0d);
        p.put("EbTotal", 0d);
        p.put("ItemRate", 0d);
        p.put("RateUomId", 0);
        p.put("ItemAmount", 0d);
        p.put("ExpenseAmount", 0d);
        p.put("FromWarehouseId", 0);
        p.put("ToWarehouseId", 0);
        p.put("RefDocumentTypeId", 0);
        p.put("RefDocNoId", 0);
        p.put("RefDocSubIdNo", 0);
        p.put("TransferDocumentTypeId", 0);
        p.put("TransferDetailId", 0);
        p.put("TransferId", 0);
        p.put("RefDocInvoiceId", 0);
        p.put("LineId", 0);
        p.put("ItemConditionId", 0);
        p.put("RackId", 0);
        p.put("ToRackId", 0);
        p.put("RemarksDetail", null);
        return p;
    }

    /** Public fields in declaration order — as StoreIssuanceService.model. */
    private static Map<String, Object> model(Object o) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Field f : o.getClass().getFields()) {
            if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
            if (List.class.isAssignableFrom(f.getType())) continue;
            try {
                Object v = f.get(o);
                if (v != null && ("VoucherDate".equals(f.getName()) || "ChequeDate".equals(f.getName())
                        || "DueDate".equals(f.getName()) || "EntryDate".equals(f.getName())
                        || "ModifyDate".equals(f.getName()) || "PostDate".equals(f.getName())
                        || "DCheqDate".equals(f.getName()) || "GpDate".equals(f.getName()))) {
                    v = Timestamp.valueOf(String.valueOf(v).length() == 10 ? v + " 00:00:00" : String.valueOf(v));
                }
                m.put(f.getName(), v);
            } catch (IllegalAccessException ignored) { }
        }
        return m;
    }

    // ================================================================================= helpers

    private Map<String, Object> owned(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOC) return null;
        if (toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId()) return null;
        return h;
    }

    private static List<Map<String, Object>> project(List<Map<String, Object>> rows, String id, String name) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, id)));
            o.put(name, str(ci(r, name)));
            out.add(o);
        }
        return out;
    }

    /** Conversion.ToInt(string) — 0 when the text is not an integer. */
    private static int textInt(String s) {
        if (s == null) return 0;
        try { return Integer.parseInt(s.trim()); } catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDouble(string) — thousands separators allowed, 0 when not a number. */
    private static double textDouble(String s) {
        if (s == null) return 0d;
        String t = s.replace(",", "").trim();
        if (t.isEmpty()) return 0d;
        try { return Double.parseDouble(t); } catch (NumberFormatException e) { return 0d; }
    }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static int i(Integer v) { return v == null ? 0 : v; }
    private static double d(Double v) { return v == null ? 0d : v; }
    private static boolean isBlank(String s) { return s == null || s.trim().isEmpty(); }
    private static boolean toBool(String v) {
        if (v == null) return false;
        String t = v.trim();
        return "true".equalsIgnoreCase(t) || "1".equals(t);
    }
    private static Map<String, Object> ok(String message, int id) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", message);
        m.put("id", id);
        return m;
    }
}
