package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.DeliveryChallanPreBillDto;
import com.mst.repositories.DeliveryChallanPreBillRepository;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.security.CurrentUserContext;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Store Management (ModuleId 24) — ScreenDefinition 960 "Delivery Challan Against PreBill".
 *
 *   ScreenName      frmDeliveryChallanAgainstPurchasePreBill (base.Name; the rights key)
 *   DocumentTypeId  148 (the loader reads pre-bills of DocumentTypeId 147)
 *   Route           /store/delivery-challan-prebill   API /api/store/delivery-challan-prebill
 *   Desktop         frmDeliveryChallanAgainstPurchasePreBill.cs (+ _Helper.cs), loader frmPendingPurchasePreBillLoader.cs
 *   BLL/DAL/Model   0313 BLL / 0253 DAL StoreManagement.DeliveryChallanHeader; 0314 BLL PurchasePreBillHeader;
 *                   0512 BLL DeliveryTerm; 0573 BLL InventoryItemsOther; Models 0436 DeliveryChallanDetail,
 *                   0438 DeliveryChallanExpenseDetail, 0439 DeliveryChallanHeader; CrystalReportPrint_Helper
 *                   .StoreDeliveryChallanSlip148 / StorePurchasePreBillSlip147.
 *
 * Saved rows are exactly the desktop's (same procedures, same parameters, same values), so GRN Store
 * (324), which reads pending challans through USP_DeliveryChallanHeader_PendingDataLoader, sees them
 * as it sees desktop-saved challans.
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 * ---------------------------------------------------------------------------------------------
 *  1. USP_DeliveryChallanDetail_Insert checks the pre-bill balance only when @PurchaseOrderHeaderId > 0,
 *     and the form never sets PurchaseOrderHeaderId/PurchaseOrderDetailId (always 0, Form.cs:998) — so
 *     the database never checks over-delivery; the only check is Insert():871 (Qty > PreBillQty - UsedQty).
 *  2. Every row needs a Purchase Demand (Insert():866-867, "Demand No is required ..."): a pre-bill line
 *     without a demand cannot be delivered from this screen.
 *  3. WareHouseId is never set (0); the procedure substitutes configuration 258 (virtual warehouse) and
 *     raises "Virtual Warehouse configuration is missing..." when it is absent — also for deleted rows.
 *  4. PartyBillDate of a line without one is saved as 1900-01-01 (Conversion.ToDateTime(DBNull), Form.cs:1018).
 *  5. Expense Qty is saved through Conversion.ToInt (Insert():898) — Convert.ToInt32(double), rounded half
 *     to even; Rate and Amount are saved unrounded.
 *  6. On insert the procedure recomputes DocNo as MAX(DocNo)+1 over ALL rows (deleted ones included),
 *     while GenerateCode skips ActionId 3 — the saved number can differ from the one shown (see deviation 4).
 *  7. ProjectsId = BranchesId (Insert():837); IsApproved false, ApprovedUserId/ApprovedDate = user/now are
 *     sent (the procedure ignores them); ReferenceNo and ApprovalRemarks are never sent.
 *  8. Update: the header procedure deletes every expense row of the document and the form re-inserts the
 *     grid's; detail rows are updated in place (ActionTypeId 2) and removed rows are soft-deleted
 *     (ActionTypeId 3) first, in the DAL's order: header, removed rows, grid rows, expenses — one transaction.
 *  9. VehicleNo/BiltyNo are header text boxes but are stored on every detail row; ReadById shows the first
 *     row's (Form.cs:959-964).
 * 10. History sends no DocumentTypeId (BLL FormHistory) and the "Bill To Party" filter is bound to the
 *     CityName list and sent as @CityId (HistoryComboBind:1136, HistoryGridFill:1323).
 * 11. The Delivery Term combo, when its current value is not in the list (e.g. at Form_Load), activates
 *     Rows[2] — the second term (BindAndRetainSelection(..., ActivateRow: true, 2), Form.cs:442). New/after
 *     save does NOT reset Delivery Term or Doc Date; City is set from "City Area" only on New/Refresh, not
 *     at Form_Load (GetConfigurationsFromGlobalAndBindValuesInColumns is not called from Load).
 * 12. Deleting checks nothing on the form side; the procedure refuses approved documents and documents
 *     used by a Gate Pass or a GRN.
 * 13. Doc Date: a new document saves the time of day; after Open then New the picker still holds the
 *     stored midnight date (FromReset:1033 leaves it), so the desktop saves midnight — the page sends
 *     DocDateFromOpened and the server reproduces that.
 * 14. Delete (DeleteById) soft-deletes header and detail rows only: its DeliveryChallanExpenseDetail rows
 *     stay, and USP_PurchasePreBillExpenseDetail_GetByHeaderIds excludes any pre-bill expense found there
 *     (NOT EXISTS, no ActionId filter) — those pre-bill expenses can never be loaded again.
 * 15. ReadByHeaderId_DeliveryChallanExpenseDetail INNER JOINs InventoryItemsOther: an expense row whose
 *     other item no longer exists is not read back, and the update's delete-all drops it for good.
 * 16. USP_DeliveryChallanHeader_InsertAndUpdate refuses an UPDATE (not only a delete) when a Gate Pass
 *     (GatePassGeneral) or a GRN (InvGrnDetail.DeliveryChallanId) uses the challan.
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (web only)
 * ---------------------------------------------------------------------------------------------
 *  1. SECURITY — detail rows: a new row (Id 0) is accepted only when its (PurchasePreBillHeaderId,
 *     PurchasePreBillDetailId) is a row of USP_PurchasePreBillHeader_PendingDataLoader for the session's
 *     organization/company/branch/financial year and DocumentTypeId 147 (no date/doc/item/party filters);
 *     item, uom, condition, demand ids, vendor/reference party, party bill no/date, PreBillQty and
 *     UsedQty come from that row, never from the request. The balance check of Insert():871 is made on
 *     those fresh values.
 *  2. SECURITY — a row with Id > 0 must be a live detail row of the stored document being updated (its
 *     values come from ReadByHeaderId_DeliveryChallanDetail); removed ids (lstRemoveRecord) must also be
 *     rows of that document and may not be kept at the same time; a pre-bill line may appear once.
 *  3. SECURITY — expense rows: a stored row (Id > 0) must belong to the document; a new row must come
 *     from USP_PurchasePreBillExpenseDetail_GetByHeaderIds for a pre-bill that is in the company-scoped
 *     pending loader (that procedure itself has no company filter). OtherItem/Qty/Rate/Amount come from
 *     the server row; only Remarks is taken from the page. The loader's expense fetch is validated the
 *     same way. Stored expense rows the page did not post are re-inserted unchanged (the desktop grid
 *     cannot drop them, and the update procedure deletes them all).
 *  4. Doc No is never taken from the request: GenerateCode on insert, the stored number on update. The
 *     success message shows the number the procedure actually stored (read back in the transaction).
 *  5. Open / update / delete / print only for a DocumentTypeId-148 document of the session's
 *     organization and company, and — without "CanView AllRecord" — only the user's own documents
 *     (the desktop reaches documents only through its history, which has that filter).
 *  6. Rights are checked on the server: Save (new), Update (Id > 0), Delete, Print.
 *  7. Print returns the slip procedure's rows and the expense sub-report rows; the Crystal layouts
 *     148_DeliveryChallanSlip.rpt / 147 pre-bill slip are out of scope.
 *
 * NOT PORTED: attachments (btnattachment, DMS rows, NoOfAttachments link), saved grid layouts (CtrlGrdBar),
 * ShortCut Keys popup and keyboard shortcuts, the Reference Party form button (not on the toolbar), the
 * FromDate/ToDate/OnlyPending entry used when another form opens this one.
 */
@Service
public class DeliveryChallanPreBillService {

    private static final Logger LOG = LoggerFactory.getLogger(DeliveryChallanPreBillService.class);

    public static final int DOCUMENT_TYPE_ID = 148;
    public static final int PREBILL_DOCUMENT_TYPE_ID = 147;              // frmPendingPurchasePreBillLoader:99
    public static final String SCREEN_NAME = "frmDeliveryChallanAgainstPurchasePreBill";
    private static final Timestamp DATE_1900 = Timestamp.valueOf("1900-01-01 00:00:00");
    private static final DateTimeFormatter TS = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");

    private final DeliveryChallanPreBillRepository repo;
    private final StoreIssuanceRepository common;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;
    private final JdbcTemplate jdbc;

    public DeliveryChallanPreBillService(DeliveryChallanPreBillRepository repo, StoreIssuanceRepository common,
                                         StoreScreenRights rights, CurrentUserContext ctx, JdbcTemplate jdbc) {
        this.repo = repo;
        this.common = common;
        this.rights = rights;
        this.ctx = ctx;
        this.jdbc = jdbc;
    }

    // ============================================================================== lookups

    /** InitializeComponentMethod (Form.cs:319) and btnRefresh_Click (:1065). */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        int fy = ctx.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN_NAME));
        out.put("docNo", repo.generateCode(u, branch(u), fy, DOCUMENT_TYPE_ID));             // DocumentNoDbCall:410
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(common.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        out.put("cityAreaId", toInt(common.config(u, "City Area")));                          // :386
        out.put("financialYearStart", financialYearStart(u, fy));                             // cmbDateTypeHistory 5, loader New

        List<Map<String, Object>> terms = new ArrayList<>();                                  // DeliveryTerm:438
        for (Map<String, Object> r : repo.deliveryTerms()) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Description", str(ci(r, "Description")));
            terms.add(o);
        }
        out.put("deliveryTerms", terms);

        List<Map<String, Object>> cities = new ArrayList<>();                                 // CityDtFillFromGlobalAndBind:454
        for (Map<String, Object> r : repo.cities(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Description", str(ci(r, "CityName")));
            cities.add(o);
        }
        out.put("cities", cities);

        out.put("itemConditions", common.itemConditions());                                   // GridDtFill:467 (Id 4 dropped)

        List<Map<String, Object>> other = new ArrayList<>();                                  // OtherItemdtDbCall:480
        for (Map<String, Object> r : repo.otherItems(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("OtherItemName", str(ci(r, "OtherItemName")));
            other.add(o);
        }
        out.put("otherItems", other);

        List<Map<String, Object>> histCity = new ArrayList<>(), histItem = new ArrayList<>();  // HistoryComboBind:1136
        for (Map<String, Object> r : repo.historyDropDowns(u)) {
            String a = str(ci(r, "Activity"));
            List<Map<String, Object>> t = "CityName".equals(a) ? histCity : "Item".equals(a) ? histItem : null;
            if (t == null) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Name", str(ci(r, "ReferenceName")));
            t.add(o);
        }
        out.put("historyCities", histCity);
        out.put("historyItems", histItem);
        return out;
    }

    /** FromReset:1057 — DocumentNoDbCall. */
    public int docNo() {
        UserAccount u = ctx.requireAccountingUser();
        return repo.generateCode(u, branch(u), ctx.currentFinancialYearId(), DOCUMENT_TYPE_ID);
    }

    // =============================================================================== loader

    /** frmPendingPurchasePreBillLoader.ComboDbCall / CombosFill — "BillToPartyName" and "Item". */
    public Map<String, Object> loaderCombos() {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> party = new ArrayList<>(), item = new ArrayList<>();
        for (Map<String, Object> r : repo.preBillDropDowns(u)) {
            String a = str(ci(r, "Activity"));
            List<Map<String, Object>> t = "Item".equals(a) ? item : "BillToPartyName".equals(a) ? party : null;
            if (t == null) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Name", str(ci(r, "ReferenceName")));
            t.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("billToParties", party);
        out.put("items", item);
        return out;
    }

    private static final String[] PENDING_KEYS = {
            "RecordNo", "PurchasePreBillHeaderId", "DocumentTypeId", "DocNo", "DocDate", "BillToPartyId", "BillToPartyName",
            "ReferenceNo", "DiscountAmountHeader", "BillAmount", "DeliveryTermId", "DeliveryTerm", "CityId", "CityName",
            "RemarksHeader", "EntryUserId", "EntryDate", "EntryUserName", "ModifyUserId", "ModifyDate", "ModifyUserName",
            "ApprovedUserId", "ApprovedDate", "IsApproved", "ApprovalUserName", "DetailId", "PurchaseDemandHeaderId",
            "PurchaseDemandDetailId", "PurchaseOrderDetailId", "PurchaseOrderHeaderId", "WareHouseId", "WareHouseName",
            "ItemId", "ItemCode", "ItemName", "UomId", "Uom", "UomEquivalent", "ItemConditionId", "ItemCondition", "Qty",
            "Rate", "ItemAmountWithoutDiscount", "DiscountAmount", "ItemAmount", "ExpenseAmount", "ItemNetAmount",
            "VendorSupplierId", "VendorSupplierName", "ReferencePartyId", "ReferencePartyName", "PartyBillNo",
            "PartyBillDate", "VehicleNo", "BiltyNo", "RemarksDetail", "QtyUsedInChallan", "BalanceQty",
            "PurchaseDemandDocNo", "PurchaseDemandDocDate", "PurchaseDemandQty", "PurchaseOrderDocNo",
            "PurchaseOrderDocDate", "PurchaseOrderQty", "NoOfAttachments" };

    /**
     * PendingDataDbCall (loader:211). FromDate: the picker value (the first call uses Now-7); ToDate:
     * the picker value; doc range / item / bill-to party only when non-zero.
     */
    public List<Map<String, Object>> pending(String from, String to, String fromDocNo, String toDocNo, int itemId, int billToPartyId) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> rows = repo.pendingPreBills(u, branch(u), ctx.currentFinancialYearId(), PREBILL_DOCUMENT_TYPE_ID,
                StoreIssuanceService.pickerDate(from), StoreIssuanceService.pickerDate(to),
                intText(fromDocNo), intText(toDocNo), itemId, billToPartyId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(project(r, PENDING_KEYS));
        return out;
    }

    /** LoadExpData (Form.cs:2002) — only for pre-bills of the company-scoped pending loader (deviation 3). */
    public List<Map<String, Object>> loaderExpenses(String headerIds) {
        UserAccount u = ctx.requireAccountingUser();
        Set<Integer> ids = new LinkedHashSet<>();
        if (headerIds != null) {
            for (String s : headerIds.split(",")) {
                int id = intText(s);
                if (id > 0) ids.add(id);
            }
        }
        if (ids.isEmpty()) return new ArrayList<>();                                           // IsNullOrWhiteSpace(poIds)
        Set<Integer> allowed = new HashSet<>();
        for (Map<String, Object> r : allPending(u)) allowed.add(toInt(ci(r, "PurchasePreBillHeaderId")));
        for (Integer id : ids) {
            if (!allowed.contains(id)) throw new IllegalArgumentException("Pre-Bill " + id + " is not a pending Pre-Bill of this company, branch and year.");
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.preBillExpensesByHeaderIds(join(ids))) out.add(expenseSource(r));
        return out;
    }

    /** grd_LinkClicked DocNo (loader:337) — StorePurchasePreBillSlip147; the slip procedure is company/branch/year scoped. */
    public Map<String, Object> preBillSlip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (id == 0) throw new IllegalArgumentException("PrintId not found...");
        List<Map<String, Object>> rows = repo.preBillSlip(u, branch(u), ctx.currentFinancialYearId(), id);
        if (rows.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", plain(rows));
        out.put("expenses", plain(repo.preBillSlipExpenses(id)));
        return out;
    }

    // ============================================================================== history

    /** HistoryGridFill (Form.cs:1257). */
    public List<Map<String, Object>> history(String dateType, boolean fromChecked, String fromDate, boolean toChecked, String toDate,
                                             String fromDocNo, String toDocNo, int cityId, int itemId) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN_NAME, "viewAll");
        Timestamp f = fromChecked ? StoreIssuanceService.pickerDate(fromDate) : null;
        Timestamp t = toChecked ? StoreIssuanceService.pickerDate(toDate) : null;
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, branch(u), ctx.currentFinancialYearId(), viewAll, dateType, f, t,
                intText(fromDocNo), intText(toDocNo), cityId, itemId)) {
            Map<String, Object> o = new LinkedHashMap<>();                                    // dtHistoryGrid :1329
            o.put("Id", toInt(ci(r, "DeliveryChallanHeaderId")));
            o.put("DocumentTypeId", toInt(ci(r, "DocumentTypeId")));
            o.put("DocNo", toInt(ci(r, "DocNo")));
            o.put("DocDate", text(ci(r, "DocDate")));
            o.put("ReferenceNo", str(ci(r, "ReferenceNo")));
            o.put("DeliveryTermId", toInt(ci(r, "DeliveryTermId")));
            o.put("DeliveryTerm", str(ci(r, "DeliveryTerm")));
            o.put("CityId", toInt(ci(r, "CityId")));
            o.put("CityName", str(ci(r, "CityName")));
            o.put("RemarksHeader", str(ci(r, "RemarksHeader")));
            o.put("EntryDate", text(ci(r, "EntryDate")));
            o.put("EntryUserName", str(ci(r, "EntryUserName")));
            o.put("ModifyDate", text(ci(r, "ModifyDate")));
            o.put("ModifyUserName", str(ci(r, "ModifyUserName")));
            o.put("IsApproved", toBool(ci(r, "IsApproved")));
            o.put("ApprovedDate", text(ci(r, "ApprovedDate")));
            o.put("ApprovalUserName", str(ci(r, "ApprovalUserName")));
            o.put("NoOfAttachments", toInt(ci(r, "NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    // ================================================================================= read

    /** ReadById (Form.cs:940) / GetDetailGrdByHeadId (:1510) — header, grid rows (helper :46) and expense rows. */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = owned(u, id);
        if (h == null) return null;
        List<Map<String, Object>> details = repo.details(id);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : details) rows.add(gridRowFromStored(d));
        List<Map<String, Object>> exp = new ArrayList<>();
        for (Map<String, Object> e : repo.expenses(id)) {                                      // Form.cs:971
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(e, "DeliveryChallanExpenseDetailId")));
            o.put("PreBillId", toInt(ci(e, "PurchasePreBillHeaderId")));
            o.put("PreBillExpenseId", toInt(ci(e, "PurchasePreBillExpenseDetailId")));
            o.put("PreBillNo", toInt(ci(e, "PreBillNo")));
            o.put("ItemId", toInt(ci(e, "OtherItemId")));
            o.put("Qty", toDouble(ci(e, "Qty")));
            o.put("Rate", toDouble(ci(e, "Rate")));
            o.put("Amount", toDouble(ci(e, "Amount")));
            o.put("Remarks", str(ci(e, "Remarks")));
            exp.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", id);
        out.put("DocNo", toInt(ci(h, "DocNo")));
        out.put("DocDate", text(ci(h, "DocDate")));
        out.put("DeliveryTermId", toInt(ci(h, "DeliveryTermId")));
        out.put("CityId", toInt(ci(h, "CityId")));
        out.put("RemarksHeader", str(ci(h, "RemarksHeader")));
        out.put("VehicleNo", details.isEmpty() ? "" : str(ci(details.get(0), "VehicleNo")));   // :961
        out.put("BiltyNo", details.isEmpty() ? "" : str(ci(details.get(0), "BiltyNo")));
        out.put("rows", rows);
        out.put("expenses", exp);
        return out;
    }

    /** ShowPrint → StoreDeliveryChallanSlip148. */
    public Map<String, Object> slip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "print")) throw new IllegalStateException("you don't have print rights...");
        if (id == 0) throw new IllegalArgumentException("PrintId not found...");
        if (owned(u, id) == null) throw new IllegalArgumentException("No Record Found For Display");
        List<Map<String, Object>> rows = repo.slip(u, branch(u), ctx.currentFinancialYearId(), id);
        if (rows.isEmpty()) throw new IllegalArgumentException("No Record Found For Display");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", plain(rows));
        out.put("expenses", plain(repo.slipExpenses(id)));
        return out;
    }

    // =============================================================================== delete

    /** btnDelete_Click (Form.cs:797) → BLL DeleteByID in its own transaction. */
    @Transactional
    public Map<String, Object> delete(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "delete")) throw new IllegalStateException("You do not have the Delete right for this screen.");
        if (id <= 0) throw new IllegalArgumentException("No record found to Delete");
        if (owned(u, id) == null) throw new IllegalArgumentException("No record found to Delete");
        repo.deleteById(u.getId(), id);
        return ok("Delete Record Successfully", id);
    }

    // ================================================================================= save

    /**
     * btnsave_Click / btnupdate_Click → Insert() (Form.cs:821) → BLL Save → DAL SetData (one transaction):
     * USP_DeliveryChallanHeader_InsertAndUpdate, USP_DeliveryChallanDetail_Insert per detail (removed rows
     * first, then the grid rows), USP_DeliveryChallanExpenseDetail_Insert per expense row.
     */
    @Transactional
    public Map<String, Object> save(DeliveryChallanPreBillDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = i(dto.Id);
        if (recId == 0 && !rights.has(SCREEN_NAME, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN_NAME, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        if (recId < 0) throw new IllegalArgumentException("Record Not Update because RecId Not Found");   // btnupdate_Click:785
        Map<String, Object> stored = recId > 0 ? owned(u, recId) : null;
        if (recId > 0 && stored == null) throw new IllegalArgumentException("No Record Found");
        int fy = ctx.currentFinancialYearId();
        int branchId = branch(u);

        List<DeliveryChallanPreBillDto.Row> grid = dto.rows == null ? new ArrayList<>() : dto.rows;
        if (grid.isEmpty()) throw new IllegalArgumentException("Detail Record Not Found");            // :830
        /* deviation 4 — txtDocNo is disabled on the desktop; never from the request. */
        int docNo = stored != null ? toInt(ci(stored, "DocNo")) : repo.generateCode(u, branchId, fy, DOCUMENT_TYPE_ID);
        if (docNo == 0) throw new IllegalArgumentException("Doc No must be a non-zero number");        // ValidateControls :834
        String vehicleNo = dto.VehicleNo == null ? "" : dto.VehicleNo;
        String biltyNo = dto.BiltyNo == null ? "" : dto.BiltyNo;
        if (vehicleNo.trim().isEmpty()) throw new IllegalArgumentException("Vehicle No field is required");   // :835

        /* ---- server truth: the stored document and the company-scoped pending loader ---- */
        Map<Integer, Map<String, Object>> storedRows = new HashMap<>();
        if (recId > 0) for (Map<String, Object> d : repo.details(recId)) storedRows.put(toInt(ci(d, "DeliveryChallanDetailId")), d);
        List<Map<String, Object>> pendingRows = null;
        Map<String, Map<String, Object>> pendingByLine = new HashMap<>();
        Set<Integer> pendingHeaders = new HashSet<>();
        boolean needPending = false;
        for (DeliveryChallanPreBillDto.Row r : grid) if (recId == 0 || i(r.Id) <= 0) needPending = true;
        if (dto.expenses != null) for (DeliveryChallanPreBillDto.Exp e : dto.expenses) if (recId == 0 || i(e.Id) <= 0) needPending = true;
        if (needPending) {
            pendingRows = allPending(u);
            for (Map<String, Object> p : pendingRows) {
                int h = toInt(ci(p, "PurchasePreBillHeaderId"));
                pendingByLine.put(h + ":" + toInt(ci(p, "DetailId")), p);
                pendingHeaders.add(h);
            }
        }

        Map<String, Object> head = new LinkedHashMap<>();                                      // Model 0439 property order
        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        head.put("IsApproved", false);
        head.put("ApprovedDate", now);
        /* txtDocDate.Value: an opened document leaves the picker at the stored (midnight) DocDate, and
           FromReset:1033 never touches it — so after Open then New the desktop still saves midnight;
           otherwise it carries the time of day (formDate / pickerDate). */
        head.put("DocDate", recId > 0 ? StoreIssuanceService.formDate(dto.DocDate, recId)
                : (Boolean.TRUE.equals(dto.DocDateFromOpened) ? midnight(dto.DocDate) : StoreIssuanceService.formDate(dto.DocDate, 0)));
        head.put("EntryDate", now);
        head.put("ModifyDate", now);
        head.put("ActionId", recId == 0 ? 1 : 2);                                               // BLL Save
        head.put("ApprovedUserId", u.getId());
        head.put("BranchesId", branchId);
        head.put("CityId", i(dto.CityId));
        head.put("CompanyId", u.getCompanyId());
        head.put("DeliveryChallanHeaderId", recId);
        head.put("DeliveryTermId", i(dto.DeliveryTermId));
        head.put("DocNo", docNo);
        head.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        head.put("EntryUserId", u.getId());
        head.put("FinancialYearId", fy);
        head.put("ModifyUserId", u.getId());
        head.put("OrganizationId", u.getOrganizationId());
        head.put("ProjectsId", branchId);                                                       // :837
        head.put("ApprovalRemarks", null);                                                      // never set → not sent
        /* AttachmentsValues/CustomAttachmentsValues: "" after FromReset; ReadById's on update (null → not sent). */
        head.put("AttachmentsValues", stored != null ? ci(stored, "AttachmentsValues") : "");
        head.put("CustomAttachmentsValues", stored != null ? ci(stored, "CustomAttachmentsValues") : "");
        head.put("ReferenceNo", null);
        head.put("RemarksHeader", dto.RemarksHeader == null ? "" : dto.RemarksHeader);

        List<Map<String, Object>> details = new ArrayList<>();
        /* lstRemoveRecord (:852) — only when RecId > 0. */
        Set<Integer> removed = new LinkedHashSet<>();
        if (recId > 0 && dto.removedIds != null) {
            for (Integer rid : dto.removedIds) {
                int id = i(rid);
                if (id <= 0) continue;
                Map<String, Object> s = storedRows.get(id);
                if (s == null) throw new IllegalArgumentException("Removed detail row " + id + " is not a row of this document.");
                if (!removed.add(id)) continue;
                Map<String, Object> d = detailModel(storedSource(s), id, 3, toDouble(ci(s, "Qty")), str(ci(s, "RemarksDetail")), vehicleNo, biltyNo);
                details.add(d);
            }
        }

        Set<Integer> seenIds = new HashSet<>();
        Set<Integer> seenLines = new HashSet<>();
        for (int idx = 0; idx < grid.size(); idx++) {
            DeliveryChallanPreBillDto.Row r = grid.get(idx);
            int id = recId != 0 ? i(r.Id) : 0;                                                  // :859
            Source src;
            if (id > 0) {
                Map<String, Object> s = storedRows.get(id);
                if (s == null || removed.contains(id) || !seenIds.add(id))
                    throw new IllegalArgumentException("Row " + (idx + 1) + " is not a live row of this document.");
                src = storedSource(s);
            } else {
                Map<String, Object> p = pendingByLine.get(i(r.PurchasePreBillHeaderId) + ":" + i(r.PurchasePreBillDetailId));
                if (p == null) throw new IllegalArgumentException("Row " + (idx + 1)
                        + ": the Pre-Bill line is not pending for this company, branch and year (already delivered or not found).");
                src = pendingSource(p);
            }
            if (!seenLines.add(src.preBillDetailId))
                throw new IllegalArgumentException("Row " + (idx + 1) + ": the same Pre-Bill line is in the grid twice.");
            double qty = d(r.ThisQty);
            Map<String, Object> dm = detailModel(src, id, id <= 0 ? 1 : 2, qty, r.Remarks == null ? "" : r.Remarks, vehicleNo, biltyNo);
            validateField(src.demandHeaderId, "Demand No", idx);                                // :866
            validateField(src.demandDetailId, "Demand No", idx);
            validateField(src.itemId, "Item", idx);
            validateField(src.uomId, "Uom", idx);
            if (qty <= 0d) throw new IllegalArgumentException("This Qty is required in Detail Grid at row No: " + (idx + 1));
            if (qty > src.preBillQty - src.usedQty)                                             // :871
                throw new IllegalArgumentException("You can't add Qty more than Balance Qty in Detail Grid at row No: " + (idx + 1));
            details.add(dm);
        }

        /* ---- expenses (:879) ---- */
        Map<Integer, Map<String, Object>> storedExp = new LinkedHashMap<>();
        if (recId > 0) for (Map<String, Object> e : repo.expenses(recId)) storedExp.put(toInt(ci(e, "DeliveryChallanExpenseDetailId")), e);
        Map<Integer, Map<String, Object>> preBillExp = new HashMap<>();
        Set<Integer> wantHeaders = new LinkedHashSet<>();
        List<DeliveryChallanPreBillDto.Exp> expIn = dto.expenses == null ? new ArrayList<>() : dto.expenses;
        for (DeliveryChallanPreBillDto.Exp e : expIn) {
            if (recId > 0 && i(e.Id) > 0) continue;
            if (i(e.PreBillExpenseId) == 0 && i(e.ItemId) == 0) continue;
            if (!pendingHeaders.contains(i(e.PreBillId)))
                throw new IllegalArgumentException("Expense Pre-Bill " + i(e.PreBillId) + " is not a pending Pre-Bill of this company, branch and year.");
            wantHeaders.add(i(e.PreBillId));
        }
        if (!wantHeaders.isEmpty()) {
            for (Map<String, Object> x : repo.preBillExpensesByHeaderIds(join(wantHeaders)))
                preBillExp.put(toInt(ci(x, "PurchasePreBillExpenseDetailId")), x);
        }
        List<Map<String, Object>> expenses = new ArrayList<>();
        Set<Integer> usedStoredExp = new HashSet<>(), usedPreBillExp = new HashSet<>();
        for (int idx = 0; idx < expIn.size(); idx++) {
            DeliveryChallanPreBillDto.Exp e = expIn.get(idx);
            int storedId = recId > 0 ? i(e.Id) : 0;
            int otherItemId, preBillId, preBillExpId, expId;
            double qty, rate, amount;
            if (storedId > 0) {
                Map<String, Object> s = storedExp.get(storedId);
                if (s == null || !usedStoredExp.add(storedId))
                    throw new IllegalArgumentException("Expense row " + (idx + 1) + " is not a row of this document.");
                otherItemId = toInt(ci(s, "OtherItemId"));
                preBillId = toInt(ci(s, "PurchasePreBillHeaderId"));
                preBillExpId = toInt(ci(s, "PurchasePreBillExpenseDetailId"));
                qty = toDouble(ci(s, "Qty")); rate = toDouble(ci(s, "Rate")); amount = toDouble(ci(s, "Amount"));
                expId = storedId;
            } else {
                if (i(e.PreBillExpenseId) == 0 && i(e.ItemId) == 0) continue;                  // ItemId == 0 → skipped
                Map<String, Object> x = preBillExp.get(i(e.PreBillExpenseId));
                if (x == null || toInt(ci(x, "PurchasePreBillHeaderId")) != i(e.PreBillId) || !usedPreBillExp.add(i(e.PreBillExpenseId)))
                    throw new IllegalArgumentException("Expense row " + (idx + 1) + " is not an open expense of its Pre-Bill.");
                otherItemId = toInt(ci(x, "OtherItemId"));
                preBillId = toInt(ci(x, "PurchasePreBillHeaderId"));
                preBillExpId = toInt(ci(x, "PurchasePreBillExpenseDetailId"));
                qty = toDouble(ci(x, "Qty")); rate = toDouble(ci(x, "Rate")); amount = toDouble(ci(x, "Amount"));
                expId = 0;                                                                      // LoadExpData newRow["Id"] = 0
            }
            if (otherItemId == 0) continue;                                                     // :881
            if (amount == 0d) throw new IllegalArgumentException("Amount Field Required in Expense Grid Row no : " + (idx + 1));
            expenses.add(expenseModel(expId, preBillId, preBillExpId, otherItemId, qty, rate, amount, e.Remarks == null ? "" : e.Remarks));
        }
        int extraIdx = expIn.size();
        for (Map.Entry<Integer, Map<String, Object>> en : storedExp.entrySet()) {              // deviation 3
            if (usedStoredExp.contains(en.getKey())) continue;
            Map<String, Object> s = en.getValue();
            int rowNo = ++extraIdx;                                                             // after the posted rows, as in the grid
            if (toInt(ci(s, "OtherItemId")) == 0) continue;
            if (toDouble(ci(s, "Amount")) == 0d) throw new IllegalArgumentException("Amount Field Required in Expense Grid Row no : " + rowNo);
            expenses.add(expenseModel(en.getKey(), toInt(ci(s, "PurchasePreBillHeaderId")), toInt(ci(s, "PurchasePreBillExpenseDetailId")),
                    toInt(ci(s, "OtherItemId")), toDouble(ci(s, "Qty")), toDouble(ci(s, "Rate")), toDouble(ci(s, "Amount")), str(ci(s, "Remarks"))));
        }

        /* ---- DAL 0253 SetData ---- */
        if (details.isEmpty()) throw new IllegalArgumentException("Detail list not found");
        int num = repo.setProc(DeliveryChallanPreBillRepository.P_SAVE, head);
        int headerId = num > 0 ? num : recId;
        if (headerId <= 0) throw new IllegalArgumentException("Save returned no document id.");
        for (Map<String, Object> dm : details) {
            dm.put("DeliveryChallanHeaderId", headerId);
            repo.setProc(DeliveryChallanPreBillRepository.P_DETAIL, dm);
        }
        for (Map<String, Object> em : expenses) {
            em.put("DeliveryChallanHeaderId", headerId);
            repo.setProc(DeliveryChallanPreBillRepository.P_EXPENSE, em);
        }

        Map<String, Object> saved = repo.header(headerId);
        int savedNo = saved == null ? docNo : toInt(ci(saved, "DocNo"));
        return ok((recId > 0 ? "Data Update Successfully....  " : "Data Save Successfully....  ") + savedNo, headerId);
    }

    // ============================================================================== models

    /** One grid line's server-side values (FillDetailListCommonForInsertAndDelete :998). */
    private static final class Source {
        int demandHeaderId, demandDetailId, preBillHeaderId, preBillDetailId, itemId, uomId, itemConditionId;
        int vendorSupplierId, referencePartyId;
        double preBillQty, usedQty;
        String partyBillNo;
        Timestamp partyBillDate;
    }

    private static Source pendingSource(Map<String, Object> p) {                              // LoadInGridDetail :1937
        Source s = new Source();
        s.demandHeaderId = toInt(ci(p, "PurchaseDemandHeaderId"));
        s.demandDetailId = toInt(ci(p, "PurchaseDemandDetailId"));
        s.preBillHeaderId = toInt(ci(p, "PurchasePreBillHeaderId"));
        s.preBillDetailId = toInt(ci(p, "DetailId"));
        s.itemId = toInt(ci(p, "ItemId"));
        s.uomId = toInt(ci(p, "UomId"));
        s.itemConditionId = toInt(ci(p, "ItemConditionId"));
        s.vendorSupplierId = toInt(ci(p, "VendorSupplierId"));
        s.referencePartyId = toInt(ci(p, "ReferencePartyId"));
        s.preBillQty = toDouble(ci(p, "Qty"));
        s.usedQty = toDouble(ci(p, "QtyUsedInChallan"));
        s.partyBillNo = str(ci(p, "PartyBillNo"));
        s.partyBillDate = dateOr1900(ci(p, "PartyBillDate"));
        return s;
    }

    private static Source storedSource(Map<String, Object> d) {                               // helper FillDetailFromListCommonForReadById
        Source s = new Source();
        s.demandHeaderId = toInt(ci(d, "PurchaseDemandHeaderId"));
        s.demandDetailId = toInt(ci(d, "PurchaseDemandDetailId"));
        s.preBillHeaderId = toInt(ci(d, "PurchasePreBillHeaderId"));
        s.preBillDetailId = toInt(ci(d, "PurchasePreBillDetailId"));
        s.itemId = toInt(ci(d, "ItemId"));
        s.uomId = toInt(ci(d, "UomId"));
        s.itemConditionId = toInt(ci(d, "ItemConditionId"));
        s.vendorSupplierId = toInt(ci(d, "VendorSupplierId"));
        s.referencePartyId = toInt(ci(d, "ReferencePartyId"));
        s.preBillQty = toDouble(ci(d, "PurchasePreBillQty"));
        s.usedQty = toDouble(ci(d, "AlreadyUsedPreBillQtyInDeliveryChallan"));
        s.partyBillNo = str(ci(d, "PartyBillNo"));
        s.partyBillDate = dateOr1900(ci(d, "PartyBillDate"));
        return s;
    }

    /** Model 0436 DeliveryChallanDetail — every non-virtual property, in declaration order. */
    private static Map<String, Object> detailModel(Source s, int id, int actionTypeId, double qty, String remarks,
                                                   String vehicleNo, String biltyNo) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("PartyBillDate", s.partyBillDate);
        m.put("Qty", qty);
        m.put("ActionTypeId", actionTypeId);
        m.put("DeliveryChallanDetailId", id);
        m.put("DeliveryChallanHeaderId", 0);
        m.put("ItemId", s.itemId);
        m.put("PurchaseDemandDetailId", s.demandDetailId);
        m.put("PurchaseDemandHeaderId", s.demandHeaderId);
        m.put("PurchaseOrderDetailId", 0);                                                      // desktop note 1
        m.put("PurchaseOrderHeaderId", 0);
        m.put("PurchasePreBillDetailId", s.preBillDetailId);
        m.put("PurchasePreBillHeaderId", s.preBillHeaderId);
        m.put("ReferencePartyId", s.referencePartyId);
        m.put("UomId", s.uomId);
        m.put("VendorSupplierId", s.vendorSupplierId);
        m.put("WareHouseId", 0);                                                                // desktop note 3
        m.put("ItemConditionId", s.itemConditionId);
        m.put("BiltyNo", biltyNo);
        m.put("PartyBillNo", s.partyBillNo);
        m.put("RemarksDetail", remarks);
        m.put("VehicleNo", vehicleNo);
        return m;
    }

    /** Model 0438 DeliveryChallanExpenseDetail — declaration order. */
    private static Map<String, Object> expenseModel(int id, int preBillId, int preBillExpId, int otherItemId,
                                                    double qty, double rate, double amount, String remarks) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Amount", amount);
        m.put("Qty", (double) Math.rint(qty));                                                  // Conversion.ToInt :898 (half to even)
        m.put("Rate", rate);
        m.put("DeliveryChallanExpenseDetailId", id);
        m.put("DeliveryChallanHeaderId", 0);
        m.put("OtherItemId", otherItemId);
        m.put("PurchasePreBillExpenseDetailId", preBillExpId);
        m.put("PurchasePreBillHeaderId", preBillId);
        m.put("Remarks", remarks);
        return m;
    }

    /** helper FillDetailFromListCommonForReadById — one dtDetailGrid row (+ ItemCondition for the history grid). */
    private static Map<String, Object> gridRowFromStored(Map<String, Object> d) {
        Map<String, Object> o = new LinkedHashMap<>();
        double preBill = toDouble(ci(d, "PurchasePreBillQty")), used = toDouble(ci(d, "AlreadyUsedPreBillQtyInDeliveryChallan"));
        int vendor = toInt(ci(d, "VendorSupplierId"));
        o.put("Id", toInt(ci(d, "DeliveryChallanDetailId")));
        o.put("PurchaseDemandHeaderId", toInt(ci(d, "PurchaseDemandHeaderId")));
        o.put("PurchaseDemandDetailId", toInt(ci(d, "PurchaseDemandDetailId")));
        o.put("PurchaseDemandNo", toInt(ci(d, "PurchaseDemandDocNo")));
        o.put("PurchasePreBillHeaderId", toInt(ci(d, "PurchasePreBillHeaderId")));
        o.put("PurchasePreBillDetailId", toInt(ci(d, "PurchasePreBillDetailId")));
        o.put("PurchasePreBillNo", toInt(ci(d, "PurchasePreBillDocNo")));
        o.put("ItemId", toInt(ci(d, "ItemId")));
        o.put("ItemCode", str(ci(d, "ItemCode")));
        o.put("ItemName", str(ci(d, "ItemName")));
        o.put("UomId", toInt(ci(d, "UomId")));
        o.put("Uom", str(ci(d, "Uom")));
        o.put("UomEquivalent", toDouble(ci(d, "UomEquivalent")));
        o.put("ItemConditionId", toInt(ci(d, "ItemConditionId")));
        o.put("ItemCondition", str(ci(d, "ItemCondition")));
        o.put("TotalDemandQty", toDouble(ci(d, "PurchaseDemandQty")));
        o.put("PreBillQty", preBill);
        o.put("UsedQty", used);
        o.put("BalanceQty", preBill - used);
        o.put("ThisQty", toDouble(ci(d, "Qty")));
        o.put("PartyBillNo", str(ci(d, "PartyBillNo")));
        o.put("PartyBillDate", text(ci(d, "PartyBillDate")));
        o.put("VendorSupplierId", vendor);
        o.put("ReferencePartyId", toInt(ci(d, "ReferencePartyId")));
        o.put("VendorSupplier", vendor > 0 ? str(ci(d, "VendorSupplierName")) : str(ci(d, "ReferencePartyName")));
        o.put("Remarks", str(ci(d, "RemarksDetail")));
        return o;
    }

    /** A PurchasePreBillExpenseDetail_GetByHeaderIds row, as LoadExpData reads it. */
    private static Map<String, Object> expenseSource(Map<String, Object> r) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("PurchasePreBillExpenseDetailId", toInt(ci(r, "PurchasePreBillExpenseDetailId")));
        o.put("PurchasePreBillHeaderId", toInt(ci(r, "PurchasePreBillHeaderId")));
        o.put("PreBillNo", toInt(ci(r, "PreBillNo")));
        o.put("OtherItemId", toInt(ci(r, "OtherItemId")));
        o.put("OtherItemName", str(ci(r, "OtherItemName")));
        o.put("Qty", toDouble(ci(r, "Qty")));
        o.put("Rate", toDouble(ci(r, "Rate")));
        o.put("Amount", toDouble(ci(r, "Amount")));
        o.put("Remarks", str(ci(r, "Remarks")));
        return o;
    }

    // ============================================================================== helpers

    /** The unfiltered pending loader of this company/branch/year — the reference set for deviations 1 and 3. */
    private List<Map<String, Object>> allPending(UserAccount u) {
        return repo.pendingPreBills(u, branch(u), ctx.currentFinancialYearId(), PREBILL_DOCUMENT_TYPE_ID,
                null, null, 0, 0, 0, 0);
    }

    /** Deviation 5 — this screen's document of the session's organization/company (own only without CanView AllRecord). */
    private Map<String, Object> owned(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID) return null;
        if (toInt(ci(h, "CompanyId")) != i(u.getCompanyId())) return null;
        if (toInt(ci(h, "OrganizationId")) != i(u.getOrganizationId())) return null;
        if (!rights.has(SCREEN_NAME, "viewAll") && toInt(ci(h, "EntryUserId")) != i(u.getId())) return null;
        return h;
    }

    /** FormHelper.ValidateField on an int. */
    private static void validateField(int v, String field, int rowIndex) {
        if (v == 0) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    /** The picker at a stored DocDate — midnight of the chosen day. */
    private static Timestamp midnight(String yyyyMMdd) {
        LocalDate d = (yyyyMMdd == null || yyyyMMdd.trim().isEmpty()) ? LocalDate.now() : LocalDate.parse(yyyyMMdd.trim().substring(0, 10));
        return Timestamp.valueOf(d.atStartOfDay());
    }

    private static Timestamp dateOr1900(Object v) {
        if (v instanceof Timestamp) return (Timestamp) v;
        if (v instanceof java.sql.Date) return Timestamp.valueOf(((java.sql.Date) v).toLocalDate().atStartOfDay());
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime());
        if (v instanceof LocalDateTime) return Timestamp.valueOf((LocalDateTime) v);
        if (v instanceof LocalDate) return Timestamp.valueOf(((LocalDate) v).atStartOfDay());
        return DATE_1900;
    }

    static Object text(Object v) {
        if (v == null) return null;
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().format(TS);
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().atStartOfDay().format(TS);
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().format(TS);
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).format(TS);
        if (v instanceof LocalDate) return ((LocalDate) v).atStartOfDay().format(TS);
        if (v instanceof java.time.OffsetDateTime) return ((java.time.OffsetDateTime) v).toLocalDateTime().format(TS);
        return v;
    }

    private static Map<String, Object> project(Map<String, Object> r, String[] keys) {
        Map<String, Object> o = new LinkedHashMap<>();
        for (String k : keys) o.put(k, text(ci(r, k)));
        return o;
    }

    private static List<Map<String, Object>> plain(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                if (e.getValue() instanceof byte[]) continue;                                   // logo / rowversion
                o.put(e.getKey(), text(e.getValue()));
            }
            out.add(o);
        }
        return out;
    }

    private String financialYearStart(UserAccount u, int yearId) {
        try {
            for (Map<String, Object> r : jdbc.queryForList(
                    "EXEC dbo.Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId @OrganizationId=?, @CompanyId=?",
                    u.getOrganizationId(), u.getCompanyId())) {
                if (toInt(ci(r, "Id")) == yearId) {
                    Object v = ci(r, "Start_Period");
                    return v == null ? null : String.valueOf(v).substring(0, 10);
                }
            }
        } catch (Exception e) {
            LOG.warn("Could not read the financial year start", e);
        }
        return null;
    }

    /** Conversion.ToInt(text) — Convert.ToInt32(string): an integer or 0. */
    private static int intText(String s) {
        if (s == null) return 0;
        String t = s.trim();
        if (!t.matches("[-+]?\\d{1,10}")) return 0;
        try { return Integer.parseInt(t); } catch (NumberFormatException e) { return 0; }
    }

    private static String join(Set<Integer> ids) {
        StringBuilder sb = new StringBuilder();
        for (Integer id : ids) { if (sb.length() > 0) sb.append(','); sb.append(id); }
        return sb.toString();
    }

    private static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        return v != null && "true".equalsIgnoreCase(String.valueOf(v).trim());
    }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static int i(Integer v) { return v == null ? 0 : v; }
    private static double d(Double v) { return v == null ? 0d : v; }

    private static Map<String, Object> ok(String message, int id) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", message);
        m.put("id", id);
        return m;
    }
}
