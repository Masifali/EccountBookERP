package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.models.dto.StoreStockTransferDto;
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
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.clr;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Store Management (ModuleId 24) — screen 339 "Stock Transfer".
 *
 * <ul>
 *   <li>ScreenName (base.Name): {@code frmStockTransfer} — rights key and attachment screen name.</li>
 *   <li>DocumentTypeId: 68 (DocumentNoFill:668, Insert:2013, attachments :2127).</li>
 *   <li>Route /store/stock-transfer, API /api/store/stock-transfer.</li>
 *   <li>Desktop form: Architecture.WinApp.StoreManagement/frmStockTransfer.cs (6,278 lines; logic :337-3694),
 *       loader dialog Architecture.WinApp.Production/LoadavailableTransactionsForIssuance.cs.</li>
 *   <li>BLL 0559 InvStockTransferHeader, DAL 0412 InvStockTransferHeader, Models 1009 / 1008 / 0979
 *       (header / detail / expense), plus the combo BLLs listed on {@link StoreStockTransferRepository}.</li>
 * </ul>
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 * ---------------------------------------------------------------------------------------------
 *  1. GenerateCode sends Activity 'GenrateCode' without BranchesId (company-wide max+1), but
 *     Sp_InvStockTransferHeader_Insert recomputes DocNo per branch. The "Data Save Successfully....  N"
 *     message shows the generator's number, which can differ from the stored one.
 *  2. Transfer Type and Gate Pass combos are disabled (Load:365-366); the only way to set them is the
 *     "Pending Entries" grid's Load button (GetDataFromGPGrid:1521). Inward rows cannot be added with
 *     "+" ("You Can't Add Direct Record", btnplus_Click:1631).
 *  3. FormValidation:1071 — for MoveOrder with no gate pass it returns true early, so the Ticket No
 *     check is skipped entirely.
 *  4. btnplus_Click:1648 stores RateUOMId 0 and RateUOM 40 and the Amount box's "#,#" text (rounded to
 *     a whole number) as ItemAmount; ItemAmountCalculation:3085 (only on "Update" of a row) recomputes
 *     every row's ItemAmount unrounded as NetWeight / RateUOM * ItemRate.
 *  5. GetAvgRate:3319 fills Avg Rate even when StockTransferFinancialEffectIsActive is off (the box is
 *     only hidden), so ItemRate/ItemAmount are saved anyway; the rate is AvgRateOnlyForCGS * 40.
 *  6. WeightCalculation:3118 — when EbUnit is "0" (not empty) EbTotal's box keeps its previous text while
 *     the net weight is computed with EbTotal 0.
 *  7. "+" (split, Inward only) copies the row's ItemArray including its Id, so splitting a saved row
 *     posts two rows with the same detail Id; Sp_InvStockTransferDetail_Insert updates that one row twice.
 *  8. Loader rows (LoadDataDetailfromPurchaseInvoivce:3429) are added with FlagForSplitAndDelete true and
 *     so can never be deleted; every manual row is removed when a loader row arrives; a re-loaded
 *     reference adds to QTY and BalWeight but not to NetWeight/GrossWeight.
 *  9. MakeVoucher (BLL 0559:80) is built before the DAL's FIFO rewrite of ItemRate/ItemAmount and before
 *     the header id exists; USP_VoucherBalanceCheck is called with @Id = the stock transfer id, not the
 *     voucher head id (DAL 0412).
 * 10. FIFO (ERP feature 5) with no loader rows and TransferType Outward on a single-branch transfer
 *     builds no evaluation rows, so the DAL always fails with "CGS Amount or StockWeight cannot be equal
 *     to zero.this Item … against FIFO Method".
 * 11. History (gridhistoryfill:2805) shows ModifyDate as the ENTRY date when a modify date exists; the
 *     history detail grid (grdhistory_SelectionChanged:2948) puts CropYear under "PackingType" and the
 *     packing type under "CropYear".
 * 12. History's Reset / Refresh buttons (btnNewHistory, btnRefreshHistory) have no Click handler in the
 *     designer — they do nothing; they are not rendered.
 * 13. AvgRateRecalculateOnDocDateChange:3642 — once one Inward row carries transfer ids the NewRateGet
 *     flag stays false for every later row.
 * 14. No DateLock check: this BLL, unlike the issuance family, never checks the lock date.
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (web only, with reason)
 * ---------------------------------------------------------------------------------------------
 *  1. DocNo is the generator's on insert and the stored header's on update; never taken from the page.
 *  2. On update GatePassId, WbTicketId, TransferType, RefDocumentTypeId and NoOfBranches come from the
 *     stored header (the desktop's controls hold exactly those after ReadById and cannot be changed).
 *  3. The Outward "item does not exist in Delivery Order" check uses the delivery-order items of the
 *     posted gate pass, read on the server; the desktop uses whatever list the form last loaded, which
 *     after ReadById can be stale.
 *  4. A document is only opened, updated, printed or deleted when it belongs to the user's company and
 *     is DocumentTypeId 68; a header procedure that returns no id aborts the save.
 *  5. Rights (Save/Update/Delete) are re-checked on the server.
 *  6. A posted detail Id is accepted only when it is one of the saved document's own rows (0 on a new
 *     document): Sp_InvStockTransferDetail_Insert UPDATEs by Id alone, with no header/company check.
 *     The loader references (TransferDocumentTypeId / TransferId / TransferDetailId) are still taken from
 *     the page as the desktop takes them from its loader grid; they are not re-validated on the server.
 *
 * NOT PORTED: attachments (btnAttachment, AttachmentsList in SetData), saved grid layouts (ctrlGrdBar*),
 * shortcut-key popup, the post-save Wages Bill dialog (frmwagesBillHeader, WagesCompulsoryOnStockTransfer),
 * the Crystal layout 406-InvStockTransferSlip.rpt (the slip procedure's rows are returned instead).
 */
@Service
public class StoreStockTransferService {

    public static final int DOC = 68;
    public static final String SCREEN = "frmStockTransfer";
    private static final int FIFO_FEATURE = 5;

    private final StoreStockTransferRepository repo;
    private final StoreIssuanceRepository shared;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public StoreStockTransferService(StoreStockTransferRepository repo, StoreIssuanceRepository shared,
                                     StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.shared = shared;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ================================================================================= load

    /** frmStockTransfer_Load:357. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN));
        out.put("fifo", repo.erpFeature(u, FIFO_FEATURE));                                        // :364
        out.put("financialEffect", toBool(shared.config(u, "StockTransferFinancialEffectIsActive"))); // :378
        out.put("wagesCompulsory", toBool(shared.config(u, "WagesCompulsoryOnStockTransfer")));      // :379
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(shared.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        out.put("docNo", repo.generateCode(u, DOC, fy));                                           // DocumentNoFill:654
        out.put("warehouses", project(repo.activeWarehouses(u), "Id", "WareHouseName"));           // WareHouseFill:612
        out.put("packingTypes", project(repo.packingTypes(), "Id", "PackTypeDesc"));               // GetPackingType:774
        out.put("cropYears", project(repo.cropYears(u), "Id", "CropYear"));                        // GetCropYear:805
        out.put("jobLots", project(repo.jobLots(u), "Id", "JobLotDescription"));                   // GetJobLot:834
        out.put("items", items(u));                                                                // ItemDetailFill:682
        out.put("accounts", accounts(u));                                                          // AccountsFill:940
        out.put("historyTransferTypes", historyTransferTypes(u));                                  // HistoryComboFill:2669
        out.put("pending", repo.pending(u, fy, branch(u)));                                        // :1403
        out.put("financialYearStart", repo.financialYearStart(u, fy));
        return out;
    }

    /** DocumentNoFill:654 — called by Reset:2318 as well. */
    public int docNo() {
        UserAccount u = ctx.requireAccountingUser();
        return repo.generateCode(u, DOC, ctx.currentFinancialYearId());
    }

    public List<Map<String, Object>> items() { return items(ctx.requireAccountingUser()); }

    private List<Map<String, Object>> items(UserAccount u) {
        return project(repo.readAllItems(u), "Id", "ItemName");
    }

    /** AccountsFill:951 — AccountTypeId not in 2, 11, 12, 15. */
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

    public List<Map<String, Object>> pending() {
        UserAccount u = ctx.requireAccountingUser();
        return repo.pending(u, ctx.currentFinancialYearId(), branch(u));
    }

    public Map<String, Object> reloadCombos() {                     // btnRefresh_Click:2361
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("items", items(u));
        out.put("warehouses", project(repo.activeWarehouses(u), "Id", "WareHouseName"));
        out.put("packingTypes", project(repo.packingTypes(), "Id", "PackTypeDesc"));
        out.put("cropYears", project(repo.cropYears(u), "Id", "CropYear"));
        out.put("jobLots", project(repo.jobLots(u), "Id", "JobLotDescription"));
        return out;
    }

    // ================================================================================= header combos

    /** Inward → the form's RefDocumentTypeId, Outward → 91, anything else → the model default 0. */
    private static int gateDocType(String transferType, int refDocumentTypeId) {
        if ("Inward".equals(transferType)) return refDocumentTypeId;
        if ("Outward".equals(transferType)) return 91;
        return 0;
    }

    public List<Map<String, Object>> gatePasses(String transferType, int refDocumentTypeId) {       // :470
        UserAccount u = ctx.requireAccountingUser();
        return repo.gatePasses(u, ctx.currentFinancialYearId(), branch(u), gateDocType(transferType, refDocumentTypeId));
    }

    public List<Map<String, Object>> tickets(int gatePassId, String transferType, int refDocumentTypeId) { // :519
        UserAccount u = ctx.requireAccountingUser();
        return repo.tickets(u, ctx.currentFinancialYearId(), gatePassId, gateDocType(transferType, refDocumentTypeId));
    }

    public List<Map<String, Object>> moveOrderTickets() {                                             // :895
        UserAccount u = ctx.requireAccountingUser();
        return repo.moveOrderTickets(u, ctx.currentFinancialYearId(), branch(u));
    }

    /** cmbTicketNo_Leave:1054 — "0" and "" when the ticket has no row. */
    public Map<String, Object> ticketWeight(int ticketId) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> r = repo.netWeightByTicket(u, ticketId);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("NetWbWeight", r == null ? "0" : clr(toDouble(r.get("NetWbWeight"))));      // Conversion.ToString(double)
        out.put("WorkingReportNo", r == null ? "" : str(r.get("WorkingReportNo")));
        return out;
    }

    public List<Map<String, Object>> itemsFromDeliveryOrder(int gatePassId) {                           // :571
        return repo.itemsFromDeliveryOrder(ctx.requireAccountingUser(), gatePassId);
    }

    public List<Map<String, Object>> branchData(int gatePassId) {                                       // :3482
        UserAccount u = ctx.requireAccountingUser();
        return repo.branchData(u, branch(u), gatePassId);
    }

    // ================================================================================= entry bar

    public List<Map<String, Object>> uoms(int itemId) {                                                 // :734
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.uomsByItem(ctx.requireAccountingUser(), itemId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Equivalent", toDouble(r.get("Equivalent")));
            out.add(o);
        }
        return out;
    }

    /** AvailableStockGetByItem:3220 — ToDate = DocDate.Value. */
    public String availableStock(int recId, int itemId, int warehouseId, int jobLotId, String cropYear, String docDate) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> r = repo.weightCurrStock(u, itemId, StoreIssuanceService.formDate(docDate, recId),
                warehouseId, jobLotId, cropYear == null ? "" : cropYear.trim());
        return r.isEmpty() ? "0" : str(r.get(0).get("AvailableStock"));
    }

    /**
     * GetAvgRate:3319 (cropYearId, no crop text) and AvgRateRecalculateOnDocDateChange:3673 (crop text,
     * no id) — AvgRateOnlyForCGS(item, DocDate.Value, 68, Id, jobLot, …, warehouseFrom) * 40.
     * Returns 0 under FIFO (the form never asks then).
     */
    public double avgRate(int recId, int itemId, String docDate, int jobLotId, int cropYearId, String cropYear, int warehouseId) {
        UserAccount u = ctx.requireAccountingUser();
        if (repo.erpFeature(u, FIFO_FEATURE)) return 0d;
        return repo.avgRateOnlyForCgs(u, itemId, StoreIssuanceService.formDate(docDate, recId), DOC, recId,
                jobLotId, cropYearId, cropYear, warehouseId) * 40.0;
    }

    // ================================================================================= loader dialog

    /** StockComboFill:197 — BranchesIds = the user's branch as text. */
    public Map<String, Object> loaderLookups() {
        UserAccount u = ctx.requireAccountingUser();
        String[] kinds = { "ParentCategories", "ItemCategories", "ItemTypes", "JobLot", "CropYear", "Warehouse",
                "DocumentType", "Supplier_Customer", "Items" };
        Map<String, Object> out = new LinkedHashMap<>();
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
        out.putAll(by);
        out.put("fromDate", repo.financialYearStart(u, ctx.currentFinancialYearId()));
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
            if (rdt == 112 || rdt == 80) o.put("GrnNo", 0);                      // :398
            o.put("Remarks", r.get("TranRemarks"));
            out.add(o);
        }
        return out;
    }

    // ================================================================================= history

    public List<Map<String, Object>> history(String dateType, String from, String to, int fromDocNo, int toDocNo,
                                             String transferType) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN, "viewAll");
        Timestamp f = isBlank(from) ? null : StoreIssuanceService.pickerDate(from);
        Timestamp t = isBlank(to) ? null : StoreIssuanceService.pickerDate(to);
        String k = dateType == null ? "doc" : dateType;
        List<Map<String, Object>> rows = repo.formHistory(u, DOC, viewAll, ctx.currentFinancialYearId(), branch(u), u.getId(),
                "doc".equals(k) ? f : null, "doc".equals(k) ? t : null,
                "entry".equals(k) ? f : null, "entry".equals(k) ? t : null,
                "modify".equals(k) ? f : null, "modify".equals(k) ? t : null,
                "approved".equals(k) ? f : null, "approved".equals(k) ? t : null,
                fromDocNo, toDocNo, transferType == null ? "" : transferType);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {                                     // gridhistoryfill:2805
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("DocNo", toInt(r.get("DocNo")));
            o.put("DocDate", r.get("DocDate"));
            o.put("TransferType", r.get("TransferType"));
            o.put("WorkingReportNo", r.get("WorkingReportNo"));
            o.put("GatePassId", r.get("GatePassId"));
            o.put("WbTicketId", r.get("WbTicketId"));
            o.put("GpSrNo", r.get("GpSrNo"));
            o.put("TicketNo", r.get("TicketNo"));
            o.put("WbNetWeight", toDouble(r.get("WbNetWeight")));
            o.put("OtherWeight", toDouble(r.get("OtherWeight")));
            o.put("DiffWeight", toDouble(r.get("DiffWeight")));
            o.put("EntryDate", r.get("EntryDate"));
            o.put("EntryUser", r.get("EntryUserName"));
            o.put("ModifyDate", r.get("ModifyDate") == null ? "" : r.get("EntryDate"));   // D11
            o.put("ModifyUser", r.get("ModifyUserName"));
            o.put("NoOfAttachments", r.get("NoOfAttachments"));
            o.put("RemarksHeader", r.get("RemarksHeader"));
            out.add(o);
        }
        return out;
    }

    /** grdhistory_SelectionChanged:2910 — GetByID's details, columns as the desktop fills them (D11). */
    public List<Map<String, Object>> historyDetail(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (owned(u, id) == null) return null;
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.details(id)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("FromWarehouse", str(d.get("FromWareHouseName")));
            o.put("ToWarehouse", str(d.get("ToWareHouseName")));
            o.put("ItemName", str(d.get("ItemName")));
            o.put("PackingType", str(d.get("CropYear")));
            o.put("CropYear", str(d.get("PackTypeDesc")));
            o.put("JobLotFrom", str(d.get("JobLotDescription")));
            o.put("JobLotTo", str(d.get("JobLotTo")));
            o.put("Qty", toDouble(d.get("Qty")));
            o.put("PackUom", str(d.get("PackUom")));
            o.put("GrossWeight", toDouble(d.get("GrossWeight")));
            o.put("EbUnit", toDouble(d.get("EbUnit")));
            o.put("EbTotal", toDouble(d.get("EbTotal")));
            o.put("AddLessWt", toDouble(d.get("AdLsWeight")));
            o.put("StockWeight", toDouble(d.get("NetWeight")));
            o.put("ItemRate", toDouble(d.get("ItemRate")));
            o.put("ItemAmount", toDouble(d.get("ItemAmount")));
            o.put("ExpenseAmount", toDouble(d.get("ExpenseAmount")));
            o.put("RemarksDetail", str(d.get("RemarksDetail")));
            out.add(o);
        }
        return out;
    }

    // ================================================================================= read

    /** ReadById:2186. */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = owned(u, id);
        if (h == null) return null;
        int refDocType = toInt(h.get("RefDocumentTypeId"));
        int branches = toInt(h.get("NoOfBranches"));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", toInt(h.get("Id")));
        out.put("RefDocumentTypeId", refDocType);
        out.put("DocDate", h.get("DocDate"));
        out.put("DocNo", toInt(h.get("DocNo")));
        out.put("NoOfBranches", branches);
        out.put("RemarksHeader", str(h.get("RemarksHeader")));
        out.put("GatePassId", toInt(h.get("GatePassId")));
        out.put("GpSrNo", toInt(h.get("GpSrNo")));
        out.put("WbTicketId", toInt(h.get("WbTicketId")));
        out.put("TicketNo", toInt(h.get("TicketNo")));
        out.put("WbNetWeight", clr(toDouble(h.get("WbNetWeight"))));                // :2226 Conversion.ToString(double)
        out.put("OtherWeight", clr(toDouble(h.get("OtherWeight"))));
        out.put("TransferType", str(h.get("TransferType")));
        out.put("WorkingReportNo", str(h.get("WorkingReportNo")));
        out.put("IsApproved", toBool(str(h.get("IsApproved"))));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.details(id)) {                          // :2239
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("FlagForSplitAndDelete", refDocType == 51 && branches > 1);
            o.put("Id", toInt(d.get("Id")));
            o.put("RefDocumentTypeId", toInt(d.get("RefDocumentTypeId")));
            o.put("RefDocNoId", toInt(d.get("RefDocNoId")));
            o.put("RefDocSubIdNo", toInt(d.get("RefDocSubIdNo")));
            o.put("ItemId", toInt(d.get("ItemId")));
            o.put("ItemName", str(d.get("ItemName")));
            o.put("PackTypeId", toInt(d.get("InvPackingTypeId")));
            o.put("PackType", str(d.get("PackTypeDesc")));
            o.put("CropYear", str(d.get("CropYear")));
            o.put("JobLotId", toInt(d.get("JobLotId")));
            o.put("JobLot", str(d.get("JobLotDescription")));
            o.put("JobLotIdTo", toInt(d.get("JobLotIdTo")));
            o.put("JobLotTo", str(d.get("JobLotTo")));
            o.put("QTY", toDouble(d.get("Qty")));
            o.put("PackUOMId", toInt(d.get("PackUomId")));
            o.put("PackUOM", str(clrOrBlank(d.get("Equivalent"))));
            o.put("GrossWeight", toDouble(d.get("GrossWeight")));
            o.put("EbUnit", toDouble(d.get("EbUnit")));
            o.put("EbTotal", toDouble(d.get("EbTotal")));
            o.put("AdLsWeight", toDouble(d.get("AdLsWeight")));
            o.put("NetWeight", toDouble(d.get("NetWeight")));
            o.put("BalQty", toDouble(d.get("Qty")));
            o.put("BalWeight", toDouble(d.get("NetWeight")));
            o.put("WareHouseFromId", toInt(d.get("FromWarehouseId")));
            o.put("WareHouseFrom", str(d.get("FromWareHouseName")));
            o.put("WareHouseToId", toInt(d.get("ToWarehouseId")));
            o.put("WareHouseTo", str(d.get("ToWareHouseName")));
            o.put("Remarks", str(d.get("RemarksDetail")));
            o.put("ItemRate", toDouble(d.get("ItemRate")));
            o.put("RateUOMId", toInt(d.get("RateUomId")));
            o.put("RateUOM", toDouble(d.get("RateUom")));
            o.put("ItemAmount", toDouble(d.get("ItemAmount")));
            o.put("Expense", toDouble(d.get("ExpenseAmount")));
            o.put("TransferDocumentTypeId", toInt(d.get("TransferDocumentTypeId")));
            o.put("TransferId", toInt(d.get("TransferId")));
            o.put("TransferDetailId", toInt(d.get("TransferDetailId")));
            rows.add(o);
        }
        out.put("rows", rows);
        List<Map<String, Object>> exp = new ArrayList<>();
        for (Map<String, Object> e : repo.expenses(id)) {                        // :2247
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Account", toInt(e.get("AccountId")));
            o.put("Percentage", toDouble(e.get("Percentage")));
            o.put("Qty", toDouble(e.get("Qty")));
            o.put("Rate", toDouble(e.get("Rate")));
            o.put("Amount", toDouble(e.get("Amount")));
            o.put("Remarks", str(e.get("Remarks")));
            exp.add(o);
        }
        out.put("expenses", exp);
        return out;
    }

    /** CommonServices.StockTransferSlip406. */
    public List<Map<String, Object>> slip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (owned(u, id) == null) return null;
        return repo.slip(u, id);
    }

    // ================================================================================= delete

    /** btnDelete_Click:3594 → BLL 0581 InvPurchaseInvoice.RemoveByID. */
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

    /** btnsave_Click / btnupdate_Click → Insert():1958 → BLL 0559 Save → DAL 0412 SetData. */
    @Transactional
    public Map<String, Object> save(StoreStockTransferDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.Id == null ? 0 : dto.Id;
        if (recId == 0 && !rights.has(SCREEN, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> existing = null;
        if (recId != 0) {
            existing = owned(u, recId);
            if (existing == null) throw new IllegalArgumentException("Record Not Found");
            if (toBool(str(existing.get("IsApproved")))) throw new IllegalArgumentException("Approved Record Not Update"); // :2171
        }
        /* Deviation 2 — the stored header wins on update. */
        String transferType = existing != null ? str(existing.get("TransferType")) : (dto.TransferType == null ? "" : dto.TransferType);
        int gatePassId = existing != null ? toInt(existing.get("GatePassId")) : i(dto.GatePassId);
        int ticketId = existing != null ? toInt(existing.get("WbTicketId")) : i(dto.WbTicketId);
        int refDocType = existing != null ? toInt(existing.get("RefDocumentTypeId")) : i(dto.RefDocumentTypeId);
        int noOfBranches = existing != null ? toInt(existing.get("NoOfBranches")) : i(dto.NoOfBranches);
        int docNo = existing != null ? toInt(existing.get("DocNo")) : repo.generateCode(u, DOC, fy);   // Deviation 1
        boolean financialEffect = toBool(shared.config(u, "StockTransferFinancialEffectIsActive"));

        /* FormValidation:1071 */
        if (docNo == 0) throw new IllegalArgumentException("DocNo Field is Required");
        if (!"Inward".equals(transferType) && !"Outward".equals(transferType) && !"MoveOrder".equals(transferType)) {
            throw new IllegalArgumentException("Transfer Type Field is Required");
        }
        boolean skipTicket = false;
        if (gatePassId == 0) {
            if (!"MoveOrder".equals(transferType)) throw new IllegalArgumentException("GatePassNo Field is Required");
            skipTicket = true;                                                          // D3
        }
        if (!skipTicket && ticketId == 0) throw new IllegalArgumentException("TicketNo Field is Required");
        if (dto.rows == null || dto.rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");

        /* :1991 — other charges */
        List<StoreStockTransferDto.Expense> exps = dto.expenses == null ? new ArrayList<>() : dto.expenses;
        for (StoreStockTransferDto.Expense r : exps) {
            if (!(d(r.Amount) > 0.0)) continue;
            if (i(r.Account) == 0) throw new IllegalArgumentException("Please Select an Account Against OtherCharges Amount First");
            if (r.Remarks == null || r.Remarks.isEmpty()) throw new IllegalArgumentException("Grid Charge to Product Remarks required Please Check");
        }

        Timestamp docDate = StoreIssuanceService.formDate(dto.DocDate, recId);
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        Map<String, Object> sh = new LinkedHashMap<>();                                // Model 1009 order
        sh.put("Id", recId);
        sh.put("DocNo", docNo);
        sh.put("DocDate", docDate);
        sh.put("GatePassId", gatePassId);
        sh.put("WbTicketId", ticketId);
        sh.put("WbNetWeight", d(dto.WbNetWeight));
        sh.put("OtherWeight", d(dto.OtherWeight));
        sh.put("RemarksHeader", dto.RemarksHeader == null ? "" : dto.RemarksHeader);
        sh.put("TransferType", transferType);
        sh.put("EntryDate", now);
        sh.put("EntryUser", u.getId());
        sh.put("ModifyDate", now);
        sh.put("ModifyUser", u.getId());
        sh.put("IsApproved", false);
        sh.put("ApprovedUserId", 0);
        sh.put("ApprovedDate", null);
        sh.put("TransitWarehouseId", 0);
        sh.put("RefDocumentTypeId", refDocType);
        sh.put("OrganizationId", u.getOrganizationId());
        sh.put("CompanyId", u.getCompanyId());
        sh.put("BranchesId", branch(u));
        sh.put("ProjectsId", 0);
        sh.put("DocumentTypeId", DOC);
        sh.put("FinancialYearId", fy);
        sh.put("NoOfBranches", noOfBranches);
        sh.put("FromBranchId", 0);
        sh.put("ToBranchId", 0);

        /* :2033 detail loop */
        List<Map<String, Object>> doItems = null;
        List<Map<String, Object>> details = new ArrayList<>();
        /* Deviation 6 — Sp_InvStockTransferDetail_Insert UPDATEs "WHERE Id = @Id" with no header or
           company check, so a posted detail Id is only accepted when it is one of THIS document's
           rows (the desktop grid can only hold ids ReadById put there; a new document has none). */
        java.util.Set<Integer> ownDetailIds = new java.util.HashSet<>();
        if (recId != 0) for (Map<String, Object> d0 : repo.details(recId)) ownDetailIds.add(toInt(ci(d0, "Id")));
        double grossWeight = 0.0;
        for (int idx = 0; idx < dto.rows.size(); idx++) {
            StoreStockTransferDto.Row r = dto.rows.get(idx);
            Map<String, Object> vd = detailModel();
            int detailId = recId != 0 ? i(r.Id) : 0;
            if (detailId > 0 && !ownDetailIds.contains(detailId)) {
                throw new IllegalArgumentException("Detail row " + detailId + " does not belong to this document.");
            }
            vd.put("Id", detailId);
            vd.put("RefDocumentTypeId", i(r.RefDocumentTypeId));
            vd.put("RefDocNoId", i(r.RefDocNoId));
            vd.put("RefDocSubIdNo", i(r.RefDocSubIdNo));
            if (i(r.RefDocumentTypeId) > 0 && d(r.NetWeight) > d(r.BalWeight)) {
                throw new IllegalArgumentException("NetWeight cannot be greater than BalWeight Please Check!");
            }
            int itemId = i(r.ItemId);
            vd.put("ItemId", itemId);
            if ("Outward".equals(transferType)) {                                  // Deviation 3
                if (doItems == null) doItems = repo.itemsFromDeliveryOrder(u, gatePassId);
                boolean found = false;
                for (Map<String, Object> x : doItems) if (toInt(x.get("ItemId")) == itemId) found = true;
                if (!doItems.isEmpty() && !found) {
                    throw new IllegalArgumentException("Item " + str(r.ItemName) + " in Detail Grid Row No : " + (idx + 1) + " does not Exists in Delivery Order");
                }
            }
            vd.put("InvPackingTypeId", i(r.PackTypeId));
            vd.put("CropYear", r.CropYear == null ? "" : r.CropYear);
            vd.put("JobLotId", i(r.JobLotId));
            vd.put("JobLotIdTo", i(r.JobLotIdTo));
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
            vd.put("ToWarehouseId", i(r.WareHouseToId));
            vd.put("FromWarehouseId", i(r.WareHouseFromId));
            vd.put("TransferDocumentTypeId", i(r.TransferDocumentTypeId));
            vd.put("TransferId", i(r.TransferId));
            vd.put("TransferDetailId", i(r.TransferDetailId));
            if (noOfBranches > 1 && "Inward".equals(transferType) && i(r.WareHouseToId) == 0) {
                throw new IllegalArgumentException("WarehouseTo Field is Required.");
            }
            if ("MoveOrder".equals(transferType) || refDocType == 52) {
                if (i(r.WareHouseFromId) == 0) throw new IllegalArgumentException("FromWarehouse Field is Required.");
                if (i(r.WareHouseToId) == 0) throw new IllegalArgumentException("WarehouseTo Field is Required.");
            }
            vd.put("RemarksDetail", r.Remarks == null ? "" : r.Remarks);
            if (financialEffect) {
                if (d(r.ItemRate) <= 0.0) throw new IllegalArgumentException("AvgRate field required");
                if (d(r.ItemAmount) <= 0.0) throw new IllegalArgumentException("Amount field required");
            }
            vd.put("_ItemName", str(r.ItemName));
            details.add(vd);
        }
        /* :2100 expenses with an account */
        List<Map<String, Object>> expenses = new ArrayList<>();
        for (StoreStockTransferDto.Expense r : exps) {
            if (i(r.Account) == 0) continue;
            Map<String, Object> pf = new LinkedHashMap<>();                            // Model 0979 order
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
        if (d(dto.WbNetWeight) != grossWeight) throw new IllegalArgumentException("GrossWeight and WeighBridge Weight Not Match"); // :2115

        /* BLL Save:167 */
        Voucher voucher = makeVoucher(u, sh, details, expenses);
        if (recId == 0) sh.put("ModifyUser", 0); else sh.put("EntryUser", 0);
        int id = setData(u, sh, details, expenses, voucher, recId == 0 ? "Sp_InvStockTransferHeader_Insert" : "Sp_InvStockTransferHeader_Update");

        Map<String, Object> out = ok(recId > 0 ? "Data Update Successfully....  " + docNo : "Data Save Successfully....  " + docNo, id);
        return out;
    }

    /** BLL 0559 MakeVoucher:80. */
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
        vh.ManualBillNo = "0";                         // Conversion.ToString(obj.WorkingReportNo) — never set by the form
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
        List<Map<String, Object>> gl = shared.itemGlAccounts(u);
        if (!details.isEmpty() && !expenses.isEmpty()) {
            double total = 0.0;
            for (Map<String, Object> item : details) {
                if (gl.isEmpty()) throw new IllegalArgumentException("Item Record Not found");
                Map<String, Object> g = null;
                for (Map<String, Object> x : gl) if (toInt(x.get("Id")) == toInt(item.get("ItemId"))) { g = x; break; }
                if (g == null) continue;
                String c = "Item: " + str(g.get("ItemName")) + ",   Qty: " + clr(toDouble(item.get("Qty")))
                        + ",   Weight: " + clr(toDouble(item.get("NetWeight"))) + ",   Rate:" + clr(toDouble(item.get("ItemRate")))
                        + "  Item Amount: " + clr(toDouble(item.get("ItemAmount"))) + "  ExpenseAmount: " + clr(toDouble(item.get("ExpenseAmount")));
                ContraVoucherDto.Detail l = new ContraVoucherDto.Detail();
                l.AccountId = toInt(g.get("PurchaseGLAC"));
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

    /** DAL 0412 SetData — one transaction (the caller's @Transactional). */
    private int setData(UserAccount u, Map<String, Object> obj, List<Map<String, Object>> details,
                        List<Map<String, Object>> expenses, Voucher v, String procName) {
        {
            ContraVoucherDto.Head voucher = v.head;
            int recId = toInt(obj.get("Id"));
            int num = repo.setProc(procName, obj);
            if (num > 0) obj.put("Id", num); else num = recId;
            if (num <= 0) throw new IllegalStateException("Save returned no document id.");   // Deviation 4
            String transferType = str(obj.get("TransferType"));
            int modifyUser = toInt(obj.get("ModifyUser"));

            boolean flag = false;
            int line = 0;
            for (Map<String, Object> d : details) {
                d.put("LineId", ++line);
                if (toInt(d.get("RefDocumentTypeId")) > 0 && toInt(d.get("RefDocNoId")) > 0 && toInt(d.get("RefDocSubIdNo")) > 0) flag = true;
            }
            boolean flag3 = repo.erpFeature(u, FIFO_FEATURE);                   // DocumentTypeId != 807
            boolean fifoPath = flag3 && !flag && !"Inward".equals(transferType);
            List<Map<String, Object>> source = new ArrayList<>();
            List<Map<String, Object>> evalList = new ArrayList<>();
            if (flag3 && !flag) {
                source = shared.itemGlAccounts(u);
                if (!"Inward".equals(transferType)) {
                    for (Map<String, Object> item3 : details) {
                        String itemName = null;
                        for (Map<String, Object> x : source) if (toInt(x.get("Id")) == toInt(item3.get("ItemId"))) { itemName = str(x.get("ItemName")); break; }
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
                                evalList.add(item5);
                            }
                        }
                    }
                }
            }
            String text = "";
            for (Map<String, Object> item2 : details) {
                if (fifoPath) {
                    for (Map<String, Object> x : source) if (toInt(x.get("Id")) == toInt(item2.get("ItemId"))) { text = str(x.get("ItemName")); break; }
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
                item2.put("Id", repo.setProc("Sp_InvStockTransferDetail_Insert", sendable(item2)));
            }
            for (Map<String, Object> e : expenses) {
                e.put("InvStockTransferHeaderId", num);
                repo.setProc("Sp_InvStockTransferExpense_Insert", e);
            }
            if (fifoPath) {
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
                repo.evaluationUpdate(u, DOC, num);
            }
            if (!expenses.isEmpty() && !flag3 && !toBool(shared.config(u, "InventoryFinancialsEffectsInActive"))) {
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
                        "CompanyId", u.getCompanyId(), "Id", num));                                  // D9
                int documentTypeIdRef = repo.setProc("Sp_VoucherHead_H_Insert", model(voucher));
                for (ContraVoucherDto.Detail l : lines) {
                    l.VoucherHeadId = voucher.Id;
                    l.DocumentTypeIdRef = documentTypeIdRef;
                    repo.setProc("Sp_VoucherDetail_H_Insert", model(l));
                }
            }
            repo.inventoryTransactions(u, DOC, num);
            for (Map<String, Object> d : details) {
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
    }

    /** DAL 0205 FIFOImplemention:725 for one detail. */
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
        for (Map<String, Object> r : list3) value2 += toDouble(r.get("NetBalWeight"));
        double itemQty = toDouble(item3.get("Qty")), netWeight = toDouble(item3.get("NetWeight"));
        if (!(netWeight <= BigDecimal.valueOf(value2).setScale(2, RoundingMode.HALF_EVEN).doubleValue())) {
            throw new IllegalArgumentException("Weight available is " + clr(value2) + " and row Weight is " + clr(netWeight)
                    + " this item " + name + " against FIFO....");
        }
        List<Map<String, Object>> out = new ArrayList<>();
        double num2 = netWeight, num3 = itemQty, num5 = 0, num7 = 0;
        for (Map<String, Object> s : list3) {
            if (toDouble(s.get("AvgRate")) <= 0.0) throw new IllegalArgumentException("Rate Not Found this Item " + name + " against FIFO Method");
            int rateUom = toInt(s.get("RateUomId"));
            if (rateUom == 0) throw new IllegalArgumentException("RateUomId not found  this " + name + " against FIFO Method");
            double num6 = toDouble(s.get("NetBalWeight")), num4 = toDouble(s.get("NetBalQty"));
            double eq = repo.equivalentByItemAndSchedule(u, itemId, rateUom);
            if (eq == 0.0) throw new IllegalArgumentException("RateUom Not Found");
            Map<String, Object> e = evalModel();
            e.put("Id", toInt(s.get("Id")));
            e.put("LineId", toInt(item3.get("LineId")));
            e.put("ItemId", itemId);
            e.put("WarehouseId", wh);
            e.put("RateUom", rateUom);
            e.put("JobLotId", jobLot);
            e.put("InvPackingTypeId", packing);
            e.put("ItemUom", packUom);
            e.put("CropBatch", crop);
            e.put("RefRefDocumentTypeId", toInt(s.get("RefDocumentTypeId")));
            e.put("RefRefDocIdNo", toInt(s.get("RefDocIdNo")));
            e.put("RefRefDocSubIdNo", toInt(s.get("RefDocSubIdNo")));
            double cgsRate = toDouble(s.get("AvgRate")) * eq;
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

    /** The detail map without the service's private "_" keys. */
    private static Map<String, Object> sendable(Map<String, Object> d) {
        Map<String, Object> m = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : d.entrySet()) if (!e.getKey().startsWith("_")) m.put(e.getKey(), e.getValue());
        return m;
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
        if (toInt(h.get("DocumentTypeId")) != DOC) return null;
        if (toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId()) return null;
        return h;
    }

    private static List<Map<String, Object>> project(List<Map<String, Object>> rows, String id, String name) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get(id)));
            o.put(name, str(r.get(name)));
            out.add(o);
        }
        return out;
    }

    private static Object clrOrBlank(Object v) { return v == null ? "" : clr(toDouble(v)); }
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
