package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.PurchaseDemandDto;
import com.mst.repositories.PurchaseDemandRepository;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.clr;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Screen 340 "Purchase Demand", Store Purchase (ModuleId 67).
 *
 *   Desktop form  Architecture.WinApp.StoreManagement/frmPurchaseDemand.cs
 *   ScreenName    "frmPurchaseDemand" (base.Tag, Form_Load:293). The same form also runs as
 *                 "frmPurchaseDemandPackingMaterial" (caption "Purchase Demand Packing Material",
 *                 ParentCategoryId 7, same DocumentTypeId 141). ONLY the "frmPurchaseDemand" path
 *                 (caption "Purchase Demand Store", ParentCategoryId 8) is ported here.
 *   DocumentTypeId 141 (Insert():768, GenerateDocNumber:386). The other number in the form, 64, is
 *                 the DocumentTypeId of "Purchase Invoice Store" (InvPurchaseInvoice rows with
 *                 DocumentTypeId 64 — see the approval-count procedure, "TypeID = 65, Description =
 *                 'Purchase Invoice Store' ... AND DocumentTypeId = 64"): UpdateLastPurchaseInfo:683
 *                 reads the item's last store purchase invoice from it.
 *   Route         /store/purchase-demand, API /api/store/purchase-demand
 *   BLL / DAL     BLL 0557 Architecture.BLL.Inventory.InvPurchaseDemandHeader, DAL 0410 (SetData,
 *                 GetAll), Models 1004 InvPurchaseDemandDetail / 1005 InvPurchaseDemandHeader;
 *                 BLL 0067 Department, 0223 FixedAssetsRegister, 0019 JobLotsAllocationToBranch,
 *                 0379 GlobalServicesMethods, 0583 Item. See PurchaseDemandRepository.
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED (not corrected)
 * ---------------------------------------------------------------------------------------------
 *  B1  Sp_InvPurchaseDemandHeader_Insert recomputes DocNo itself (MAX+1 per org/company/doc type/
 *      year/branch) and ignores the one sent; the saved number can differ from the one on screen.
 *      The success message carries no number, as on the desktop.
 *  B2  Sp_InvPurchaseDemandHeader_Update rewrites BranchesId with the SAVING user's branch and
 *      ParentCategoryId with 8 (Insert():766/778) — whoever updates the document moves it to their
 *      branch.
 *  B3  Every detail row is sent with ApprovedUserId = the saving user and ApprovedDate = now
 *      (FillDetailListCommonForInsertAndDelete:846-847); a new or edited row has ApprovedQty =
 *      RequiredQty (FillDetailRow:1025). The detail procedure only updates rows whose IsApproved is
 *      0, so approved rows are silently left unchanged.
 *  B4  "Required Qty" at Add only rejects an EMPTY box (:1411); 0 is accepted into the grid and is
 *      refused only at save, by FormHelper.ValidateField ("Required Qty is required in Detail Grid
 *      at row No: n").
 *  B5  The Uom is not checked at Add (FormValidationOfDetailPortion has no Uom check) — only at
 *      save ("Uom is required in Detail Grid at row No: n").
 *  B6  Editing a grid row (DoubleClick:1090) does NOT restore Item Condition, Outstanding Demand
 *      and PO Qty or Available Stock Qty; Update writes whatever the entry bar shows, so a row's
 *      condition can change unnoticed. Reproduced on the page.
 *  B7  A new row's UOM cell shows the UOM CODE (cmbUOM.Text); an opened row shows the
 *      UOMDescription the read procedure returns (FillDetailFromListCommonForReadById:927).
 *  B8  Duplicate check is by ItemId only (IsDuplicateItem:999), not item + condition.
 *  B9  The history grid (and its Edit / Slip buttons) is shown to every user; Edit opens the
 *      document even without the Update right (the Update button is then disabled). The detail
 *      grid's X / Edit buttons appear only with the Update right (gridsettings:1150), but a
 *      double-click edits a row regardless.
 *  B10 History aggregates the procedure's per-detail rows per document: RequiredQty summed,
 *      RequestBy distinct and joined with ", " (HistoryFill:1693-1705), first-seen order.
 *  B11 Save/Update needs a non-empty grid ("Grid Record not found"), checked after the confirm and
 *      after the "You have deleted some rows" warning, as in Insert():782-788.
 *  B12 btnRefreshHistory has no Click handler on the desktop; it is not offered.
 *  B13 Desktop keyboard path (not ported, recorded): frmPurchaseDemand_KeyDown tests UpdateMode,
 *      which is never set, so Ctrl+S always calls btnSave_Click — on an OPENED document it sets
 *      RECID = 0 and saves a COPY, and, when rows were deleted, USP_InvPurchaseDemand_DeleteDetailRows
 *      then deletes those rows from the ORIGINAL document. Ctrl+U never works.
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (web only)
 * ---------------------------------------------------------------------------------------------
 *  D1  History doc-no filter: BLL 0557 FormHistory sends @FromDocNo/@ToDocNo, which
 *      Sp_InvPurchaseDemand_GetAllMethod does not declare (it declares @DocNoFrom/@DocNoTo), so on
 *      the desktop any doc-no filter fails with error 8145 ("@FromDocNo is not a parameter for procedure ..."). Sent here under
 *      the procedure's own names so the filter works.
 *  D2  Doc No is read-only (designer ReadOnly = true) and never trusted from the request: the
 *      generator's number on insert (the procedure recomputes it anyway, B1), the stored number on
 *      update.
 *  D3  Open / update / print only when the header is DocumentTypeId 141 in the user's
 *      organization and company AND its ParentCategoryId is null, 0 or 8 — the rows this screen's
 *      own history can list. A Packing Material (7) demand is refused: updating it here would
 *      rewrite its ParentCategoryId to 8 (B2).
 *  D4  On update, every detail Id > 0 and every id in DetailIdsToDelete must belong to the stored
 *      document. Sp_InvPurchaseDemandDetail_Insert updates "WHERE Id = @Id" and
 *      USP_InvPurchaseDemand_DeleteDetailRows deletes by id with no header check, so a crafted
 *      request could otherwise move or delete another document's rows. On a new save the list of
 *      rows to delete is ignored (only reachable on the desktop through B13).
 *  D5  The row-by-row checks of Insert():795-801 and "Doc No Field Required" are re-run on the
 *      server, in the same order and with the same messages; the Save/Update rights are enforced.
 *  D6  DocDate: a new document carries today's time of day (the picker's), an opened one keeps the
 *      stored DocDate's time of day on whatever day is posted (a DateTimePicker keeps the time when
 *      the day is changed).
 *  D7  Deleting a saved grid row removes that one row. The desktop removes the DataTable row and
 *      then calls GridEXRow.Delete() on the same (now detached) grid row (DeleteDetailRow:981-982);
 *      what Janus does with the second call could not be verified.
 *  D9  Opening a document clears the deleted-rows list. The desktop's ReadById (:885) keeps
 *      DetailIdsToDelete, so an Update of the newly opened document would run
 *      USP_InvPurchaseDemand_DeleteDetailRows on rows of the PREVIOUS document (the procedure has no
 *      header check). D4 would refuse that save anyway; clearing the list avoids the dead end.
 *  D8  The Parent Category combo (disabled on the desktop) shows its single row selected. Its
 *      binder, InfragisticsHelper.BindAndRetainSelection, is not in the recovered source; with no
 *      selection the disabled, required combo would make every save impossible.
 *
 * NOT PORTED: attachments (btnAttachment, NoOfAttachments link, DeleteAllAttachmentsFromDb), saved
 * grid layouts (ctrlGrdBar1/2), keyboard shortcuts and the shortcut popup, "Define City",
 * Department define (button2) and Asset define (btnAssetRefDefine) pop-up forms. The slip returns
 * Sp_InvPurchaseDemand_Rpt's rows; the Crystal layout 454-InvPurchaseDemanSlip.rpt is out of scope.
 */
@Service
public class PurchaseDemandService {

    public static final int DOCUMENT_TYPE_ID = 141;
    /** DocumentTypeId of "Purchase Invoice Store" — UpdateLastPurchaseInfo:683. */
    public static final int LAST_PURCHASE_DOCUMENT_TYPE_ID = 64;
    public static final String SCREEN_NAME = "frmPurchaseDemand";
    /** Form_Load:302 — the Store path. */
    public static final int PARENT_CATEGORY_ID = 8;

    private static final DateTimeFormatter DD_MMM_YYYY = DateTimeFormatter.ofPattern("dd-MMM-yyyy", Locale.ENGLISH);
    private static final DateTimeFormatter ISO = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss");

    private final PurchaseDemandRepository repo;
    private final StoreIssuanceRepository store;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public PurchaseDemandService(PurchaseDemandRepository repo, StoreIssuanceRepository store,
                                 StoreScreenRights rights, CurrentUserContext ctx) {
        this.repo = repo;
        this.store = store;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ============================================================================== lookups

    /** Form_Load:286 and btnRefresh_Click:1490 — every combo, the doc no and the two configs. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        int fy = ctx.currentFinancialYearId();
        int branch = branch(u);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN_NAME));                                         // :305
        out.put("docNo", repo.nextDocNo(u, fy, branch, DOCUMENT_TYPE_ID));                 // GenerateDocNumber:369
        out.put("itemSearchByCode", StoreIssuanceService.toBool(store.config(u, "ItemSearchByCode")));   // :314
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(store.config(u, "DefaultDaysToLessFromHistoryFromDate"))); // :359
        out.put("parentCategoryId", PARENT_CATEGORY_ID);

        /* clsGlobalVariables.getGlobalAllItems. ParentCategoryBind:483 — ONE row, the first item of
           parent category 8; ItemDtsFillFromGlobal:508 — the items of that category (0 → 8). */
        List<Map<String, Object>> parents = new ArrayList<>();
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : repo.allItems(u)) {
            if (toInt(r.get("InventoryParentCategoriesId")) != PARENT_CATEGORY_ID) continue;
            if (parents.isEmpty()) {
                Map<String, Object> p = new LinkedHashMap<>();
                p.put("Id", PARENT_CATEGORY_ID);
                p.put("Description", str(r.get("InvParentCateDescription")));
                parents.add(p);
            }
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("ItemName", str(r.get("ItemName")));
            o.put("ItemCode", str(r.get("ItemCode")));
            items.add(o);
        }
        out.put("parentCategories", parents);
        out.put("items", items);

        /* ItemConditionBindFromGlobal:555 — V_ItemCondition without Id 4, blank default row. */
        out.put("itemConditions", store.itemConditions());

        /* ItemUomFill:570 → dtUomFromGloablUomScheduleByItemId — the global UOM schedule. */
        List<Map<String, Object>> uoms = new ArrayList<>();
        for (Map<String, Object> r : repo.uomSchedule(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("ItemId", toInt(r.get("ItemId")));
            o.put("UOMCode", str(r.get("UOMCode")));
            uoms.add(o);
        }
        out.put("uoms", uoms);

        out.put("departments", project(repo.departments(u), "Id", "DepartmentName"));    // DepartmentFill:441
        out.put("assets", project(repo.fixedAssets(u), "Id", "AssetName"));               // FixedAssest:399
        out.put("jobLots", project(repo.jobLots(u, branch), "Id", "JobLotDescription"));  // combojoblotfill:584
        return out;
    }

    // ============================================================================ entry bar

    /** UpdateLastPurchaseInfo:667 — three texts, exactly as the boxes show them. */
    public Map<String, Object> lastPurchase(int itemId) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> r = repo.lastPurchase(u, branch(u), LAST_PURCHASE_DOCUMENT_TYPE_ID, itemId);
        Map<String, Object> out = new LinkedHashMap<>();
        if (!r.isEmpty()) {
            LocalDateTime d = toLocalDateTime(r.get(0).get("DocDate"));
            out.put("LastDate", (d == null ? LocalDate.of(1900, 1, 1) : d.toLocalDate()).format(DD_MMM_YYYY));
            out.put("LastQty", netString(r.get(0).get("ItemQty")));
            out.put("LastRate", netString(r.get(0).get("ItemRate")));
        } else {
            out.put("LastDate", "None");
            out.put("LastQty", "0");
            out.put("LastRate", "0");
        }
        return out;
    }

    /**
     * UpdateOutstandingDemandAndStockQty:705 and txtDocdate_ValueChanged:1537 — the two raw
     * numbers (the page applies "#,##.##" to the entry-bar boxes, the grid keeps them raw).
     */
    public Map<String, Object> outstanding(int itemId, String docDate, int itemConditionId, int recId) {
        UserAccount u = ctx.requireAccountingUser();
        Timestamp d = StoreIssuanceService.dateOnly(isBlank(docDate) ? LocalDate.now().toString() : docDate);
        List<Map<String, Object>> r = repo.outstandingDemandAndStock(u, itemId, d, itemConditionId, recId);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("OutstandingDemandAndPOQty", r.isEmpty() ? 0d : toDouble(r.get(0).get("OutstandingDemandAndPOQty")));
        out.put("AvaialableStockQty", r.isEmpty() ? 0d : toDouble(r.get(0).get("AvaialableStockQty")));
        return out;
    }

    /** txtBarcodeReader_KeyDown:1982 → GetItemIdByBarcode:2000. */
    public int itemIdByBarcode(String barcodeNo) {
        UserAccount u = ctx.requireAccountingUser();
        return repo.itemIdByBarcode(u, barcodeNo == null ? "" : barcodeNo.trim());
    }

    // ============================================================================== history

    /**
     * HistoryFill:1612. {@code dateType} is the checked radio: doc (default), entry, modify,
     * approved; a date is sent only when its picker is checked (the page sends it only then).
     */
    public List<Map<String, Object>> history(String dateType, String from, String to, int fromDocNo, int toDocNo) {
        UserAccount u = ctx.requireAccountingUser();
        boolean viewAll = rights.has(SCREEN_NAME, "viewAll");                           // :1622
        Timestamp f = isBlank(from) ? null : StoreIssuanceService.dateOnly(from);
        Timestamp t = isBlank(to) ? null : StoreIssuanceService.dateOnly(to);
        String k = dateType == null ? "doc" : dateType.trim().toLowerCase(Locale.ROOT);
        List<Map<String, Object>> rows = repo.formHistory(u, ctx.currentFinancialYearId(), branch(u), PARENT_CATEGORY_ID,
                viewAll, u.getId(),
                "doc".equals(k) ? f : null, "doc".equals(k) ? t : null,
                "entry".equals(k) ? f : null, "entry".equals(k) ? t : null,
                "modify".equals(k) ? f : null, "modify".equals(k) ? t : null,
                "approved".equals(k) ? f : null, "approved".equals(k) ? t : null,
                fromDocNo, toDocNo);

        /* :1693 — one row per Id, first-seen order; qty summed, requesters distinct. */
        Map<Integer, Map<String, Object>> byId = new LinkedHashMap<>();
        Map<Integer, Set<String>> requesters = new LinkedHashMap<>();
        for (Map<String, Object> r : rows) {
            int id = toInt(r.get("Id"));
            Map<String, Object> o = byId.get(id);
            if (o == null) {
                o = new LinkedHashMap<>();
                o.put("Id", id);
                o.put("RecordNo", toInt(r.get("RecordNo")));
                o.put("DocumentTypeId", toInt(r.get("DocumentTypeId")));
                o.put("DocDate", iso(r.get("DocDate")));
                o.put("DocNo", toInt(r.get("DocNo")));
                o.put("Status", str(r.get("Status")));
                o.put("RequiredQty", 0d);
                o.put("RequestBy", "");
                o.put("EntryUser", str(r.get("EntryUserName")));
                o.put("EntryDate", iso(r.get("EntryDate")));
                o.put("ModifyUser", str(r.get("ModifyUserName")));
                o.put("ModifyDate", iso(r.get("ModifyDate")));
                o.put("NoOfAttachments", toInt(r.get("NoOfAttachments")));
                o.put("RemarksHeader", str(r.get("RemarksHeader")));
                byId.put(id, o);
                requesters.put(id, new LinkedHashSet<>());
            }
            o.put("RequiredQty", toDouble(o.get("RequiredQty")) + toDouble(r.get("RequiredQty")));
            requesters.get(id).add(r.get("RequestBy") == null ? "" : String.valueOf(r.get("RequestBy")));
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map.Entry<Integer, Map<String, Object>> e : byId.entrySet()) {
            e.getValue().put("RequestBy", String.join(", ", requesters.get(e.getKey())));
            out.add(e.getValue());
        }
        return out;
    }

    // ================================================================================= read

    /** ReadById:885 / GetDetailGrdByHeadId:1852 — BLL GetByID. Null when not this screen's. */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = ownedHeader(u, id);
        if (h == null) return null;
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.details(id)) {
            /* FillDetailFromListCommonForReadById:916 — column order of dtgrddetail. */
            Map<String, Object> o = new LinkedHashMap<>();
            LocalDateTime lp = toLocalDateTime(d.get("LastPurchaseDate"));
            o.put("Id", toInt(d.get("Id")));
            o.put("Department", str(d.get("DepartmentName")));
            o.put("DepartmentId", toInt(d.get("DepartmentId")));
            o.put("RequistionBy", str(d.get("RequestBy")));
            o.put("ItemId", toInt(d.get("ItemId")));
            o.put("ItemCode", str(d.get("ItemCode")));
            o.put("ItemName", str(d.get("ItemName")));
            o.put("ItemUOM", str(d.get("UOMDescription")));                             // B7
            o.put("ItemUOMId", toInt(d.get("ItemSchuomId")));
            o.put("ItemConditionId", toInt(d.get("ItemConditionId")));
            o.put("ItemCondition", str(d.get("ItemCondition")));
            o.put("RequiredQty", toDouble(d.get("RequiredQty")));
            o.put("ApprovedQty", toDouble(d.get("ApprovedQty")));
            o.put("ApprovalStatus", toBool(d.get("IsApproved")));
            o.put("JobLot", str(d.get("JobLotDescription")));
            o.put("JobLotId", toInt(d.get("JobLotId")));
            o.put("AssetsRef", str(d.get("AssetName")));
            o.put("AssetsRefId", toInt(d.get("FixedAssetsRegisterId")));
            o.put("Remarks", str(d.get("RemarksDetail")));
            o.put("LastDate", nullDate(lp) ? "" : lp.toLocalDate().format(DD_MMM_YYYY));
            o.put("LastQty", toDouble(d.get("lastPurchaseQty")));
            o.put("LastRate", toDouble(d.get("LastPurchaseRate")));
            o.put("OutstandingDemandAndPOQty", toDouble(d.get("OutstandingDemandAndPOQty")));
            o.put("AvaialableStockQty", toDouble(d.get("AvaialableStockQty")));
            rows.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", id);
        out.put("DocNo", toInt(ci(h, "DocNo")));
        out.put("DocDate", iso(ci(h, "DocDate")));
        out.put("RemarksHeader", str(ci(h, "RemarksHeader")));
        out.put("Status", str(ci(h, "Status")));
        out.put("IsApproved", toBool(ci(h, "IsApproved")));
        out.put("rows", rows);
        return out;
    }

    /** PurchaseDemandSlip454 → CommonServices:8141 → Sp_InvPurchaseDemand_Rpt. */
    public List<Map<String, Object>> slip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (ownedHeader(u, id) == null) return null;
        return repo.slip(u, id);
    }

    // ================================================================================= save

    /** Insert():739 → BLL 0557 Save → DAL 0410 SetData, one transaction. */
    @Transactional
    public Map<String, Object> save(PurchaseDemandDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.Id == null ? 0 : dto.Id;
        if (recId == 0 && !rights.has(SCREEN_NAME, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN_NAME, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        Map<String, Object> stored = recId != 0 ? ownedHeader(u, recId) : null;
        if (recId != 0 && stored == null) throw new IllegalArgumentException("Record Not Found");

        int fy = ctx.currentFinancialYearId();
        int branch = branch(u);

        /* FormValidation:1376 — then D2: the number is the stored one or the generator's. */
        if (dto.DocNo == null || dto.DocNo == 0) throw new IllegalArgumentException("Doc No Field Required");
        int docNo = stored != null ? toInt(ci(stored, "DocNo")) : repo.nextDocNo(u, fy, branch, DOCUMENT_TYPE_ID);

        /* D4 — the rows this document owns. */
        Set<Integer> ownDetailIds = new HashSet<>();
        if (stored != null) for (Map<String, Object> d : repo.details(recId)) ownDetailIds.add(toInt(d.get("Id")));
        String detailIdsToDelete = "";
        if (stored != null && !isBlank(dto.DetailIdsToDelete)) {
            List<String> ids = new ArrayList<>();
            for (String s : dto.DetailIdsToDelete.split(",")) {
                if (isBlank(s)) continue;
                int id = toInt(s.trim());
                if (id <= 0 || !ownDetailIds.contains(id)) throw new IllegalArgumentException("Detail row " + s.trim() + " does not belong to this document.");
                ids.add(String.valueOf(id));
            }
            detailIdsToDelete = String.join(",", ids);                               // string.Join(",", HashSet)
        }

        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        Timestamp docDate = docDate(dto.DocDate, stored);

        /* Insert():764-781 — model property order of InvPurchaseDemandHeader (non-virtual only). */
        Map<String, Object> head = new LinkedHashMap<>();
        head.put("IsApproved", false);
        head.put("ApprovedDate", now);
        head.put("DocDate", docDate);
        head.put("EntryDate", now);
        head.put("ModifyDate", now);
        head.put("ApprovedUserId", 0);
        head.put("BranchesId", branch);
        head.put("CompanyId", u.getCompanyId());
        head.put("ParentCategoryId", PARENT_CATEGORY_ID);
        head.put("DocNo", docNo);
        head.put("EntryUser", u.getId());
        head.put("Id", recId);
        head.put("ModifyUser", u.getId());
        head.put("OrganizationId", u.getOrganizationId());
        head.put("ProjectsId", 0);                                                        // never set by the form
        head.put("FinancialYearId", fy);
        head.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        head.put("RemarksHeader", dto.RemarksHeader == null ? "" : dto.RemarksHeader);
        head.put("Status", "Open");

        List<PurchaseDemandDto.Row> all = dto.rows == null ? new ArrayList<>() : dto.rows;
        if (all.isEmpty()) throw new IllegalArgumentException("Grid Record not found");  // :787

        List<Map<String, Object>> details = new ArrayList<>();
        for (int idx = 0; idx < all.size(); idx++) {
            PurchaseDemandDto.Row r = all.get(idx);
            int detailId = recId != 0 ? i(r.Id) : 0;                                      // :793
            if (detailId > 0 && !ownDetailIds.contains(detailId)) {
                throw new IllegalArgumentException("Detail row " + detailId + " does not belong to this document.");   // D4
            }
            double requiredQty = d(r.RequiredQty);
            String requestBy = r.RequistionBy == null ? "" : r.RequistionBy;

            /* FillDetailListCommonForInsertAndDelete:831 — InvPurchaseDemandDetail property order. */
            Map<String, Object> pd = new LinkedHashMap<>();
            pd.put("IsApproved", r.ApprovalStatus != null && r.ApprovalStatus);
            pd.put("ApprovedDate", now);
            pd.put("LastPurchaseDate", lastPurchaseDate(r.LastDate));
            pd.put("lastPurchaseQty", d(r.LastQty));
            pd.put("LastPurchaseRate", d(r.LastRate));
            pd.put("RequiredQty", requiredQty);
            pd.put("ApprovedQty", d(r.ApprovedQty));
            pd.put("ApprovedUserId", u.getId());
            pd.put("DepartmentId", i(r.DepartmentId));
            pd.put("FixedAssetsRegisterId", i(r.AssetsRefId));
            pd.put("Id", detailId);
            pd.put("InvPurchaseDemandHeaderId", 0);
            pd.put("ItemId", i(r.ItemId));
            pd.put("ItemSchuomId", i(r.ItemUOMId));
            pd.put("JobLotId", i(r.JobLotId));
            pd.put("ItemConditionId", i(r.ItemConditionId));
            pd.put("RequestBy", requestBy);
            pd.put("RemarksDetail", r.Remarks == null ? "" : r.Remarks);

            /* :795-801, in this order. */
            validate(pd.get("DepartmentId"), "Department", idx);
            validate(requestBy, "Requistion By", idx);
            validate(pd.get("ItemId"), "Item", idx);
            validate(pd.get("ItemSchuomId"), "Uom", idx);
            validate(requiredQty, "Required Qty", idx);
            validate(pd.get("JobLotId"), "JobLot", idx);
            validate(pd.get("FixedAssetsRegisterId"), "Asset", idx);
            details.add(pd);
        }

        /* DAL 0410 SetData — header, details, then the deleted rows; attachments not ported. */
        String proc = recId == 0 ? "Sp_InvPurchaseDemandHeader_Insert" : "Sp_InvPurchaseDemandHeader_Update";
        int num = repo.setProc(proc, head);
        if (num > 0) head.put("Id", num); else num = recId;
        if (num <= 0) throw new IllegalStateException("Save returned no document id.");
        for (Map<String, Object> pd : details) {
            pd.put("InvPurchaseDemandHeaderId", num);
            repo.setProc("Sp_InvPurchaseDemandDetail_Insert", pd);
        }
        if (!detailIdsToDelete.isEmpty()) repo.deleteDetailRows(num, detailIdsToDelete);

        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", recId == 0 ? "Record Save Successfully" : "Record Update Successfully");
        m.put("id", num);
        return m;
    }

    // ============================================================================== helpers

    /** D3 — this screen's document, in the user's organization and company, parent category 8. */
    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        if (id <= 0) return null;
        Map<String, Object> h = repo.header(id);
        if (h == null) return null;
        if (toInt(ci(h, "DocumentTypeId")) != DOCUMENT_TYPE_ID) return null;
        if (toInt(ci(h, "CompanyId")) != u.getCompanyId()) return null;
        if (toInt(ci(h, "OrganizationId")) != u.getOrganizationId()) return null;
        int parent = toInt(ci(h, "ParentCategoryId"));
        if (parent != 0 && parent != PARENT_CATEGORY_ID) return null;
        return h;
    }

    /** D6 — txtDocdate.Value at save time. */
    private static Timestamp docDate(String posted, Map<String, Object> stored) {
        LocalDate day = isBlank(posted) ? LocalDate.now() : LocalDate.parse(posted.trim().substring(0, 10));
        if (stored != null) {
            LocalDateTime s = toLocalDateTime(ci(stored, "DocDate"));
            LocalTime t = s == null ? LocalTime.MIDNIGHT : s.toLocalTime();
            return Timestamp.valueOf(LocalDateTime.of(day, t));
        }
        return Timestamp.valueOf(LocalDateTime.of(day, LocalTime.now().withNano(0)));
    }

    /** Conversion.ToDateTime(cell text) — an unparseable or empty text is 1900-01-01. */
    private static Timestamp lastPurchaseDate(String text) {
        if (!isBlank(text)) {
            try {
                return Timestamp.valueOf(LocalDate.parse(text.trim(), DD_MMM_YYYY).atStartOfDay());
            } catch (Exception ignored) { /* falls through */ }
        }
        return Timestamp.valueOf(LocalDate.of(1900, 1, 1).atStartOfDay());
    }

    /** Conversion.CheckDateTimeNull — null, 0001-01-01 and 1900-01-01 count as "no date". */
    private static boolean nullDate(LocalDateTime d) {
        if (d == null) return true;
        LocalDate x = d.toLocalDate();
        return x.equals(LocalDate.of(1900, 1, 1)) || x.getYear() <= 1;
    }

    /** FormHelper.ValidateField:503. */
    private static void validate(Object v, String field, int rowIndex) {
        boolean bad = v == null
                || (v instanceof Integer && (Integer) v == 0)
                || (v instanceof Double && (Double) v <= 0d)
                || (v instanceof String && ((String) v).trim().isEmpty());
        if (bad) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    /** Conversion.ToString of a numeric cell: decimals keep their scale, doubles print as the CLR does. */
    private static String netString(Object v) {
        if (v == null) return "";
        if (v instanceof BigDecimal) return ((BigDecimal) v).toPlainString();
        if (v instanceof Double || v instanceof Float) return clr(((Number) v).doubleValue());
        return String.valueOf(v);
    }

    private static LocalDateTime toLocalDateTime(Object v) {
        if (v == null) return null;
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().atStartOfDay();
        if (v instanceof LocalDateTime) return (LocalDateTime) v;
        if (v instanceof LocalDate) return ((LocalDate) v).atStartOfDay();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime();
        String s = String.valueOf(v).trim();
        try {
            if (s.length() >= 19) return LocalDateTime.parse(s.substring(0, 19).replace(' ', 'T'));
            if (s.length() >= 10) return LocalDate.parse(s.substring(0, 10)).atStartOfDay();
        } catch (Exception ignored) { /* not a date */ }
        return null;
    }

    private static String iso(Object v) {
        LocalDateTime d = toLocalDateTime(v);
        return d == null ? null : d.format(ISO);
    }

    private static List<Map<String, Object>> project(List<Map<String, Object>> rows, String id, String name) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get(id)));
            o.put("Name", str(r.get(name)));
            out.add(o);
        }
        return out;
    }

    private static boolean toBool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        return v != null && "true".equalsIgnoreCase(String.valueOf(v).trim());
    }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static int i(Integer v) { return v == null ? 0 : v; }
    private static double d(Double v) { return v == null ? 0d : v; }
    private static boolean isBlank(String s) { return s == null || s.trim().isEmpty(); }
}
