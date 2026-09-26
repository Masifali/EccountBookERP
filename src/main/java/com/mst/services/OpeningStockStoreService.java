package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.OpeningStockStoreDto;
import com.mst.repositories.OpeningStockStoreRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.regex.Pattern;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.clr;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Store Management (ModuleId 24) — screen 341 "Opening Stock Store".
 *
 * <ul>
 *   <li>Desktop form: {@code Architecture.WinApp.Inventory_Definition.frmStoreOpeningStockBalancing}
 *       (CommonServices.cs:3572-3573). The same class serves "frmPackingMaterialOpeningStockBalance"
 *       when BaseDocumentTypeId == 2; only the STORE path is ported here (ScreenName
 *       "frmStoreOpeningStockBalancing" → BaseDocumentTypeId 1, :302).</li>
 *   <li>ScreenName (base.Name, rights key): {@code frmStoreOpeningStockBalancing}.</li>
 *   <li>DocumentTypeId 39 (:242). BaseDocumentTypeId 1.</li>
 *   <li>Page {@code /store/opening-stock-store}, API {@code /api/store/opening-stock-store}.</li>
 *   <li>BLL 0253 / DAL 0222 / Model 0201 {@code *.PurchaseTrading.InvStockOpeningBalanceHeader};
 *       BLL 0583 Item.GetByID; BLL 0594 jobLot.GetByID; BLL 0379 GlobalServicesMethods; DAL 0205
 *       CommonServices.GetJobLotGlIdsandName; BLL 0136 GeneralReprots.StockopeningBalanceRegister;
 *       helpers DatatableHelper.GetAccountsFromGlobalByTypeIds, CommonBindings.ItemUomFromGlobalBind,
 *       CommonServices.dtUomFromGloablUomScheduleByItemId, FormHelper.ValidateControls.</li>
 * </ul>
 *
 * Not the same document as the existing InventoryOpening* port: that one is
 * {@code frmOpeningStockBlancing} (DocumentTypeId 40, crop/packing type/transaction type). Both write
 * the same table through the same procedures; this class reuses its stock-posting defaults
 * (InventoryOpeningDefaults, via StoreIssuanceRepository.postStock) and nothing else.
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED (NOT CORRECTED)
 * ---------------------------------------------------------------------------------------------
 *  1. History shows DocumentTypeId 39 for BOTH store and packing-material openings: FormHistory is
 *     called with a ReportsParameters whose BaseDocumentTypeId is never set (:867), so the BLL does
 *     not send it. The history grid's default filter is TODAY..TODAY: both history pickers have
 *     ShowCheckBox with Checked left at its default (true) and Value = the moment the form opened.
 *  2. The Doc No shown is GenerateCode at reset (GETDATE based); Sp_InvStockOpeningBalanceHeader_Insert
 *     regenerates DocNo from @DocDate, so the stored DocNo can differ from the one shown, while the
 *     voucher's VoucherCode and the "Save Successfully n" message use the number shown (BLL 0253:34).
 *  3. Update rewrites FinancialYearId with the CURRENT active year (:615) and BaseDocumentTypeId
 *     with 1; IsApproved is always sent false, so the procedure's approved-record refusal never fires.
 *  4. On insert the voucher's RefDocNoId is 0 (MakeVoucher runs before the id exists, BLL 0253:26);
 *     on update it is the record id. EntryUser is 0 on update, ModifyUser 0 on insert (BLL 0253:117-127).
 *  5. The voucher's debit account is the job lot's AccountId when > 0, else the item's PurchaseGLAC;
 *     the credit account is Stock Credit A/c. Detail lines carry ItemId 0 and JobLotId 0 (never set).
 *  6. SecondaryUomId 0 → saved as the Pack UOM with SecondaryUomQty = Qty; SecondaryUomItemRate 0 →
 *     saved as the Item Rate (:642-650). WeightKgs = Qty (:638). SecondaryUomQty / Rate go to
 *     DECIMAL(18,2) parameters (the database rounds them to 2 places).
 *  7. Secondary UOM / Qty / Rate are only required when Item Condition is 4 (:604).
 *  8. The page's rate/secondary calculations keep the desktop's STATIC flags (default "by secondary
 *     rate"): typing Qty before ever typing Item Rate recomputes Item Rate from Secondary Rate (0 when
 *     that is empty). That lives in the page script.
 *  9. Total Amount is Rate × Qty ÷ Rate-UOM Equivalent, formatted "###,##0.###" — the saved amount
 *     is that 3-decimal text (:1119).
 * 10. There is no Delete on this form; no date-lock check in its BLL.
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (WEB ONLY)
 * ---------------------------------------------------------------------------------------------
 *  1. Doc No is never taken from the request (read-only box): generator on insert, stored header on
 *     update.
 *  2. A document is only opened, printed or updated when it is DocumentTypeId 39 of the user's
 *     company AND not a packing-material opening (BaseDocumentTypeId 2 — the other screen's
 *     document, with its own rights). The desktop's ReadById would open it and saving would turn it
 *     into a store opening.
 *  3. Every lookup id posted must be in the list this screen offers (items of type 17, the item's
 *     warehouses/racks for the user's branch, job lots, item conditions, the item's UOMs, the stock
 *     credit accounts). The posted Amount must equal the desktop formula (or, on update, the stored
 *     amount the form was loaded with) — it is a computed read-only box.
 *  4. A save whose header procedure returns no id is refused (the DAL would continue with Id 0).
 *  5. The voucher-id lookup runs inside the save transaction (the desktop runs it on its own connection).
 *
 * ATTACHMENTS (btnAttachment → Attachment form; FormHelper.UpdateAttachmentsForObject; DAL 0222 :30-80;
 * FormHelper.DeleteAllAttachmentsFromDb), with the project's DesktopAttachmentStore:
 *  - AT.lst = the loaded attachments not removed + the new files. When it is not empty the save stores
 *    the new files, sets AttachmentsValues / CustomAttachmentsValues to the joined names, and after the
 *    header: Sp_DMSAttachments_GetAllMethod 'DeleteById' then Proc_DMSAttachments_Insert per file
 *    (RefAccountId = ItemId, RefDocumentTypeId 39, RefDocumentNo = id) — all in the save transaction.
 *  - When it is empty the header keeps the values loaded by ReadById (Insert:658 overwrites the helper's
 *    "" with the form fields) — reproduced; and on update with removals the DMS rows are deleted
 *    (DeleteAllAttachmentsFromDb). Deviation: that delete runs inside the save transaction with
 *    'DeleteById' (the desktop's DMSAttachments.RemoveByIdAndNames BLL was not available to read);
 *    removed files are not deleted physically (the store keeps objects that may be shared).
 *
 * NOT PORTED: saved grid layouts / ctrlGrdBar, keyboard
 * shortcuts and their popup, Enter-as-Tab, the Crystal layout of 416 (the procedure rows are
 * returned). HistoryDataDbCall's result (dtHistoryFromDb) is never used by the form — not called.
 * GetERPFeatureById(4) is read at Load and never used — not called.
 */
@Service
public class OpeningStockStoreService {

    public static final int DOCUMENT_TYPE_ID = 39;
    public static final int BASE_DOCUMENT_TYPE_ID = 1;                       // :304
    public static final String SCREEN_NAME = "frmStoreOpeningStockBalancing";
    /** ItemDtsFillFromGlobal:386 — BaseDocumentTypeId 1 → ItemTypeOfTypeId 17 only. */
    private static final Set<Integer> ITEM_TYPE_OF_TYPES = new HashSet<>(Arrays.asList(17));
    /** StockCreditAccountBindFromGlobal:570 — withoutTypeIds {2, 11, 15, 22}. */
    private static final Set<Integer> EXCLUDED_ACCOUNT_TYPES = new HashSet<>(Arrays.asList(2, 11, 15, 22));

    private final OpeningStockStoreRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;
    private final DesktopAttachmentStore store;

    public OpeningStockStoreService(OpeningStockStoreRepository repo, StoreScreenRights rights, CurrentUserContext ctx,
                                    DesktopAttachmentStore store) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
        this.store = store;
    }

    // =============================================================================== attachments

    /** FormHelper.LoadAttachmentsForObject(AT, Id, base.Name) — this document's rows only. */
    public List<Map<String, Object>> attachments(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (owned(u, id) == null) throw new IllegalArgumentException("Record not Found");
        return attachmentRows(u, id);
    }

    private List<Map<String, Object>> attachmentRows(UserAccount u, int id) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.attachments(SCREEN_NAME, id)) {
            if (toInt(ci(r, "OrganizationId")) != u.getOrganizationId() || toInt(ci(r, "CompanyId")) != u.getCompanyId()) continue;
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Attachment", str(ci(r, "Attachment")));
            o.put("UploadedFileCustomName", str(ci(r, "UploadedFileCustomName")));
            o.put("UploadedFileSizeMb", ci(r, "UploadedFileSizeMb"));
            o.put("EntryDate", stamp(ci(r, "EntryDate")));
            out.add(o);
        }
        return out;
    }

    public DesktopInventoryItemFileService.Download download(int id, int attachmentId) {
        UserAccount u = ctx.requireAccountingUser();
        if (owned(u, id) == null) throw new IllegalArgumentException("Record not Found");
        for (Map<String, Object> r : attachmentRows(u, id)) {
            if (toInt(r.get("Id")) != attachmentId) continue;
            String path = str(r.get("UploadedFileCustomName"));
            if (path.isBlank()) path = str(r.get("Attachment"));
            String name = basename(str(r.get("Attachment")).isBlank() ? path : str(r.get("Attachment")));
            return new DesktopInventoryItemFileService.Download(name, store.read(u, basename(path)), "application/octet-stream");
        }
        throw new IllegalArgumentException("Attachment not found");
    }

    private static String basename(String name) {
        String v = name.replace('\\', '/');
        v = v.substring(v.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(v);
        return v;
    }

    // ====================================================================================== load

    /** Form_Load + InitializeComponentMethod:298 — rights, Doc No, the global lists, history combos. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN_NAME));
        out.put("docNo", repo.generateCode(u, DOCUMENT_TYPE_ID, ctx.currentFinancialYearId()));
        out.putAll(globals(u));
        out.put("historyCombos", historyCombos(u));
        out.put("today", LocalDate.now().toString());
        return out;
    }

    /** btnRefresh_Click:811 — the six global services reloaded, then rebound. */
    public Map<String, Object> globals() {
        return globals(ctx.requireAccountingUser());
    }

    private Map<String, Object> globals(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("items", repo.items(u, ITEM_TYPE_OF_TYPES));
        out.put("uoms", repo.uoms(u));
        out.put("racks", repo.racks(u, branch(u)));
        out.put("jobLots", repo.jobLots(u));
        out.put("itemConditions", repo.itemConditions());
        out.put("stockCreditAccounts", repo.stockCreditAccounts(u, EXCLUDED_ACCOUNT_TYPES));
        return out;
    }

    /** FormReset:788 — DocumentNoDbCall. */
    public int nextDocNo() {
        UserAccount u = ctx.requireAccountingUser();
        return repo.generateCode(u, DOCUMENT_TYPE_ID, ctx.currentFinancialYearId());
    }

    /** BtnRefreshHistory_Click:999 / Form_Load:276 — HistoryComboBind(HistoryComboDbCall()). */
    public Map<String, Object> historyCombos() {
        return historyCombos(ctx.requireAccountingUser());
    }

    private Map<String, Object> historyCombos(UserAccount u) {
        List<Map<String, Object>> st = new ArrayList<>(), items = new ArrayList<>(), itemSt = new ArrayList<>();
        List<Map<String, Object>> rows = repo.historyDropDowns(u, DOCUMENT_TYPE_ID);
        for (Map<String, Object> r : rows) {
            String activity = str(r.get("Activity"));
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("Name", str(r.get("ReferenceName")));
            if ("StockAccount".equals(activity)) st.add(o);
            else if ("Item".equals(activity)) items.add(o);
            else if ("ItemStockAccount".equals(activity)) itemSt.add(o);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        /* :1035 — no rows at all: the combos are left as they are. */
        out.put("empty", rows.isEmpty());
        out.put("stockAccounts", st);
        out.put("items", items);
        out.put("itemStockAccounts", itemSt);
        return out;
    }

    // =================================================================================== history

    /** GridHistoryBind:861 — the dtHistoryGrid projection, in its column order. */
    public List<Map<String, Object>> history(String fromDate, String toDate, String fromDocNo, String toDocNo,
                                             int accountId, int itemStockAccountId, int itemId) {
        UserAccount u = ctx.requireAccountingUser();
        List<Map<String, Object>> rows = repo.formHistory(u, DOCUMENT_TYPE_ID, ctx.currentFinancialYearId(),
                StoreIssuanceService.dateOnly(fromDate), StoreIssuanceService.dateOnly(toDate),
                convInt(fromDocNo), convInt(toDocNo), accountId, itemId, itemStockAccountId);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("DocNo", toInt(r.get("DocNo")));
            o.put("DocDate", day(r.get("DocDate")));
            o.put("ItemName", str(r.get("ItemName")));
            o.put("Warehouse", str(r.get("WareHouseName")));
            o.put("RackName", str(r.get("RackName")));
            o.put("JobLot", str(r.get("JobLotDescription")));
            o.put("ItemCondition", str(r.get("ItemCondition")));
            o.put("ItemUOM", str(r.get("PackUom")));
            o.put("Qty", toDouble(r.get("Qty")));
            o.put("SecondaryUom", r.get("SecondaryUomCode"));
            o.put("SecondaryUomQty", r.get("SecondaryUomQty") == null ? null : toDouble(r.get("SecondaryUomQty")));
            o.put("PerItemWeight", r.get("PerItemSecondaryUomQty") == null ? null : toDouble(r.get("PerItemSecondaryUomQty")));
            o.put("SecondaryUomItemRate", r.get("SecondaryUomItemRate") == null ? null : toDouble(r.get("SecondaryUomItemRate")));
            o.put("ItemRate", toDouble(r.get("ItemRate")));
            o.put("UOM", str(r.get("RateUom")));
            o.put("totalAmount", toDouble(r.get("ItemAmount")));
            o.put("StockCreditAccount", str(r.get("AccountTitle")));
            o.put("Remarks", str(r.get("Remarks")));
            o.put("IsApproved", str(r.get("IsApproved")));
            o.put("Issued", toDouble(r.get("IssueWeight")));
            o.put("EntryDate", stamp(r.get("EntryDate")));
            o.put("EntryUser", str(r.get("EntryUser")));
            o.put("ModifyDate", stamp(r.get("ModifyDate")));
            o.put("ModifyUser", str(r.get("ModifyUser")));
            o.put("NoOfAttachments", toInt(r.get("NoOfAttachments")));
            out.add(o);
        }
        return out;
    }

    // ====================================================================================== read

    /** ReadById:708 — the values the form puts back, as the text the boxes receive (.ToString()). */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = owned(u, id);
        if (h == null) return null;
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", toInt(h.get("Id")));
        o.put("DocNo", toInt(h.get("DocNo")));
        o.put("DocDate", day(h.get("DocDate")));
        o.put("Remarks", str(h.get("Remarks")));
        o.put("ItemId", toInt(h.get("ItemId")));
        o.put("WarehouseId", toInt(h.get("WarehouseId")));
        o.put("RackId", toInt(h.get("RackId")));
        o.put("JobLotId", toInt(h.get("JobLotId")));
        o.put("ItemConditionId", toInt(h.get("ItemConditionId")));
        o.put("ItemUomSch", toInt(h.get("ItemUomSch")));
        o.put("Qty", net(h.get("Qty")));
        o.put("SecondaryUomId", toInt(h.get("SecondaryUomId")));
        o.put("SecondaryUomQty", net(h.get("SecondaryUomQty")));
        o.put("PerItemSecondaryUomQty", net(h.get("PerItemSecondaryUomQty")));
        o.put("SecondaryUomItemRate", net(h.get("SecondaryUomItemRate")));
        o.put("ItemRate", net(h.get("ItemRate")));
        o.put("RateUomSch", toInt(h.get("RateUomSch")));
        o.put("ItemAmount", net(h.get("ItemAmount")));
        o.put("StockCrGLAcId", toInt(h.get("StockCrGLAcId")));
        return o;
    }

    /** btnPrint_Click:1292 → StockOpeningBalanceSlipandRegister416(RECID, 39). */
    public List<Map<String, Object>> slip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "print")) throw new IllegalStateException("You do not have the Print right for this screen.");
        if (id == 0) throw new IllegalArgumentException("PrintId not found...");
        if (owned(u, id) == null) throw new IllegalArgumentException("No Record Found For Display");
        List<Map<String, Object>> rows = repo.register(u, String.valueOf(DOCUMENT_TYPE_ID), id);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                if (v instanceof byte[]) continue;                                   // CompLogoImage
                o.put(e.getKey(), v instanceof java.util.Date ? stamp(v) : v);
            }
            out.add(o);
        }
        return out;
    }

    // ====================================================================================== save

    /** btnSave_Click:679 / btnUpdate_Click:692 → Insert():583 → BLL 0253 Save → DAL 0222 SetData. */
    @Transactional
    public Map<String, Object> save(OpeningStockStoreDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        int recId = dto.Id == null ? 0 : dto.Id;
        if (recId < 0) throw new IllegalArgumentException("No Record Found To update");
        if (recId == 0 && !rights.has(SCREEN_NAME, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN_NAME, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");

        Map<String, Object> existing = null;
        if (recId != 0) {
            existing = owned(u, recId);
            if (existing == null) throw new IllegalArgumentException("Record not Found");
        }
        int fy = ctx.currentFinancialYearId();
        /* Deviation 1 — txtDocNo is ReadOnly: generator (new) / stored header (update). */
        int docNo = existing == null ? repo.generateCode(u, DOCUMENT_TYPE_ID, fy) : toInt(existing.get("DocNo"));

        int conditionId = i(dto.ItemConditionId);
        /* :589 — FormHelper.ValidateControls, same list, same order, same messages. */
        if (docNo == 0) fail("Document No must be a non-zero number");
        combo(dto.ItemId, "Item");
        combo(dto.WarehouseId, "Warehouse");
        combo(dto.RackId, "RackName");
        combo(dto.JobLotId, "Job Lot");
        combo(dto.ItemConditionId, "Item Condition");
        combo(dto.ItemUomSch, "UOM");
        number(dto.Qty, "Quantity");
        if (conditionId == 4) {                                                        // :604
            combo(dto.SecondaryUomId, "Secondary UOM");
            number(dto.SecondaryUomQty, "Secondary UOM Qty");
            number(dto.SecondaryUomRate, "Secondary UOM Rate");
        }
        number(dto.Rate, "Rate");
        combo(dto.RateUomSch, "Rate UOM");
        number(dto.Amount, "Amount");
        combo(dto.StockCrGLAcId, "Stock Credit Account");

        /* Deviation 3 — every id must be one the screen's lists offered. */
        final UserAccount uu = u;
        List<Map<String, Object>> items = repo.items(uu, ITEM_TYPE_OF_TYPES);
        Map<String, Object> item = find(items, i(dto.ItemId));
        if (item == null) fail("The selected Item is not in this screen's item list");
        List<Map<String, Object>> racks = repo.racks(uu, branch(uu));
        boolean whOk = false, rackOk = false;
        for (Map<String, Object> r : racks) {
            if (toInt(r.get("ItemId")) != i(dto.ItemId)) continue;
            if (toInt(r.get("WarehouseId")) == i(dto.WarehouseId)) {
                whOk = true;
                if (toInt(r.get("Id")) == i(dto.RackId)) rackOk = true;
            }
        }
        if (!whOk) fail("The selected Warehouse is not allocated to this item");
        if (!rackOk) fail("The selected Rack is not in this warehouse for this item");
        if (find(repo.jobLots(uu), i(dto.JobLotId)) == null) fail("The selected Job Lot is not in the list");
        if (find(repo.itemConditions(), conditionId) == null) fail("The selected Item Condition is not in the list");
        List<Map<String, Object>> itemUoms = new ArrayList<>();
        for (Map<String, Object> x : repo.uoms(uu)) if (toInt(x.get("ItemId")) == i(dto.ItemId)) itemUoms.add(x);
        if (find(itemUoms, i(dto.ItemUomSch)) == null) fail("The selected UOM is not assigned to this item");
        Map<String, Object> rateUom = find(itemUoms, i(dto.RateUomSch));
        if (rateUom == null) fail("The selected Rate UOM is not assigned to this item");
        if (i(dto.SecondaryUomId) != 0 && find(itemUoms, i(dto.SecondaryUomId)) == null) fail("The selected Secondary UOM is not assigned to this item");
        if (find(repo.stockCreditAccounts(uu, EXCLUDED_ACCOUNT_TYPES), i(dto.StockCrGLAcId)) == null) fail("The selected Stock Credit Account is not in the list");

        /* :611-654 — the values, converted as the form converts them. */
        double qty = convDouble(trim(dto.Qty));
        double rate = convDouble(dto.Rate);
        double amount = convDouble(dto.Amount);
        /* Deviation 3 — CalculateAmount:1113 is the only writer of txtAmount (ReadOnly). */
        double expected = (qty != 0d && rate != 0d) ? convDouble(net3(rate * (qty / toDouble(rateUom.get("Equivalent"))))) : 0d;
        boolean amountOk = Math.abs(expected - amount) < 0.0005
                || (existing != null && Math.abs(toDouble(existing.get("ItemAmount")) - amount) < 0.0005);
        if (!amountOk) fail("Amount must equal Rate x Qty / Rate UOM equivalent (" + net3(expected) + ")");

        int secondaryUomId = i(dto.SecondaryUomId);
        BigDecimal secondaryQty = convDecimal(dto.SecondaryUomQty);
        BigDecimal secondaryRate = convDecimal(dto.SecondaryUomRate);
        if (secondaryUomId == 0) {                                                     // :642
            secondaryUomId = i(dto.ItemUomSch);
            secondaryQty = convDecimal(dto.Qty);
        }
        if (secondaryRate.signum() == 0) secondaryRate = convDecimal(dto.Rate);       // :647

        Timestamp now = Timestamp.valueOf(LocalDateTime.now());
        Timestamp docDate = StoreIssuanceService.formDate(dto.DocDate, recId);
        int entryUser = recId == 0 ? u.getId() : 0;                                    // BLL 0253:124
        int modifyUser = recId == 0 ? 0 : u.getId();                                  // BLL 0253:117
        String remarks = dto.Remarks == null ? "" : dto.Remarks;

        /* Model 0201 InvStockOpeningBalanceHeader — non-virtual properties, declaration order. */
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("IsApproved", false);
        h.put("ApprovedDate", now);
        h.put("DocDate", docDate);
        h.put("EntryDate", now);
        h.put("ModifyDate", now);
        h.put("ItemAmount", amount);
        h.put("ItemRate", rate);
        h.put("Qty", qty);
        h.put("WeightKgs", qty);                                                       // :638
        h.put("PackingTypeId", 0);
        h.put("ApprovedUserId", u.getId());
        h.put("BranchesId", branch(u));
        h.put("CompanyId", u.getCompanyId());
        h.put("DocNo", docNo);
        h.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        h.put("EntryUserId", entryUser);
        h.put("Id", recId);
        h.put("ItemId", i(dto.ItemId));
        h.put("ItemUomSch", i(dto.ItemUomSch));
        h.put("JobLotId", i(dto.JobLotId));
        h.put("ModifyUserId", modifyUser);
        h.put("OrganizationId", u.getOrganizationId());
        h.put("FinancialYearId", fy);                                                   // D3
        h.put("ProjectsId", branch(u));                                                // :617
        h.put("RateUomSch", i(dto.RateUomSch));
        h.put("StockCrGLAcId", i(dto.StockCrGLAcId));
        h.put("RackId", i(dto.RackId));
        h.put("WarehouseId", i(dto.WarehouseId));
        h.put("ItemVariantId", 0);
        h.put("ProductionStageId", 0);
        h.put("CastingTypeId", 0);
        h.put("BaseDocumentTypeId", BASE_DOCUMENT_TYPE_ID);
        h.put("ItemConditionId", conditionId);
        h.put("BatchNo", null);
        h.put("CropYear", null);
        h.put("Remarks", remarks);
        h.put("TransactionType", null);
        /* :658 — "" after FormReset, the loaded values after ReadById (never from the request) ... */
        h.put("CustomAttachmentsValues", existing == null ? "" : str(existing.get("CustomAttachmentsValues")));
        h.put("AttachmentsValues", existing == null ? "" : str(existing.get("AttachmentsValues")));
        /* ... unless AT.lst is not empty: DAL 0222 :42-47 joins the list's names. */
        List<Map<String, Object>> atList = attachmentList(u, recId, dto);
        if (!atList.isEmpty()) {
            List<String> names = new ArrayList<>(), custom = new ArrayList<>();
            for (Map<String, Object> a : atList) { names.add(str(a.get("Attachment"))); custom.add(str(a.get("UploadedFileCustomName"))); }
            h.put("AttachmentsValues", String.join(",", names));
            h.put("CustomAttachmentsValues", String.join(",", custom));
        }
        h.put("SecondaryUomId", secondaryUomId);
        h.put("SecondaryUomQty", secondaryQty);
        h.put("SecondaryUomItemRate", secondaryRate);

        /* BLL 0253 MakeVoucher — built before SetData, so obj.Id is RECID here. */
        Map<String, Object> vh = OpeningStockStoreRepository.voucherHeadModel();
        vh.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        vh.put("DocumentTypeSrNo", recId);
        vh.put("RefDocNoId", recId);
        vh.put("VoucherCode", docNo);
        vh.put("VoucherDate", docDate);
        vh.put("Remarks", remarks);
        vh.put("RemarksOtherLingo", "");
        vh.put("ChequeDate", Timestamp.valueOf(LocalDate.now().atStartOfDay()));    // DateTime.Today
        vh.put("IncludeWHT", false);
        vh.put("BranchId", branch(u));
        vh.put("ProjectId", branch(u));
        vh.put("BillAmount", 0d);
        vh.put("ManualBillNo", "");                                                    // Conversion.ToString(null TransactionType)
        vh.put("DueDate", docDate);
        vh.put("DueDays", 0);
        vh.put("OrganizationId", u.getOrganizationId());
        vh.put("CompanyId", u.getCompanyId());
        vh.put("FinancialYearId", fy);
        vh.put("EntryUser", entryUser);
        vh.put("EntryDate", now);
        vh.put("ModifyDate", now);
        vh.put("ModifyUser", modifyUser);

        Map<String, Object> itemRow = repo.itemById(i(dto.ItemId));                   // BLL 0583 GetByID
        int debitAccount = 0;
        if (i(dto.JobLotId) > 0) debitAccount = toInt(ci(repo.jobLotById(i(dto.JobLotId)), "AccountId"));
        if (debitAccount <= 0) debitAccount = toInt(ci(itemRow, "PurchaseGLAC"));
        /* Item name from clsGlobalVariables.getGlobalAllItems (the item is in the type-17 list, checked above). */
        String comments = remarks + "   Item: " + str(item.get("ItemName")) + ",   Qty: " + clr(qty)
                + ",   Rate:" + clr(rate) + ",   Amount:" + clr(amount);

        List<Map<String, Object>> lines = new ArrayList<>();
        Map<String, Object> dr = OpeningStockStoreRepository.voucherDetailModel();
        dr.put("AccountId", debitAccount);
        dr.put("AgainstAccountId", i(dto.StockCrGLAcId));
        dr.put("Comments", comments);
        dr.put("DebitAmount", amount);
        dr.put("CreditAmount", 0d);
        dr.put("ItemRate", rate);
        dr.put("QtyIn", qty);
        dr.put("WeightIn", qty);
        dr.put("ItemAmount", amount);
        lines.add(dr);
        Map<String, Object> cr = OpeningStockStoreRepository.voucherDetailModel();
        cr.put("AccountId", i(dto.StockCrGLAcId));
        cr.put("AgainstAccountId", debitAccount);
        cr.put("Comments", comments);
        cr.put("CreditAmount", amount);
        cr.put("DebitAmount", 0d);
        cr.put("ItemRate", rate);
        cr.put("QtyIn", qty);
        cr.put("WeightIn", qty);
        cr.put("ItemAmount", amount);
        lines.add(cr);

        int id = persist(u, h, vh, lines);
        if (!atList.isEmpty()) {                                                       // DAL 0222 :48-80
            repo.deleteAttachments(SCREEN_NAME, id);
            for (Map<String, Object> a : atList) {
                repo.insertAttachment(u, SCREEN_NAME, i(dto.ItemId), DOCUMENT_TYPE_ID, id, str(a.get("Attachment")),
                        str(a.get("UploadedFileCustomName")), toDouble(a.get("UploadedFileSizeMb")));
            }
        } else if (recId > 0 && dto.removeAttachmentIds != null && !dto.removeAttachmentIds.isEmpty()) {
            repo.deleteAttachments(SCREEN_NAME, id);                                   // DeleteAllAttachmentsFromDb
        }
        Map<String, Object> ok = new LinkedHashMap<>();
        ok.put("success", true);
        ok.put("message", (recId > 0 ? "Update Successfully " : "Save Successfully ") + docNo);   // :663 / :668
        ok.put("id", id);
        return ok;
    }

    /**
     * DAL 0222 SetData, one transaction:
     *   header Insert|Update → id; [update] voucher id lookup; VoucherHead Insert|Update;
     *   VoucherDetail_Insert × 2; VoucherHead_H_Insert; VoucherDetail_H_Insert × 2;
     *   Sp_InventoryTransactions_GetALLMethod; Sp_InventoryStockEvalautionDetail_Update.
     */
    private int persist(UserAccount u, Map<String, Object> h, Map<String, Object> vh, List<Map<String, Object>> lines) {
        int recId = toInt(h.get("Id"));
        int num3 = repo.setProc(recId == 0 ? "Sp_InvStockOpeningBalanceHeader_Insert" : "Sp_InvStockOpeningBalanceHeader_Update", h);
        if (num3 > 0) h.put("Id", num3); else num3 = recId;
        if (num3 <= 0) throw new IllegalArgumentException("Save returned no document id.");    // Deviation 4 (400, not a rights failure)

        int num2 = 0;
        if (toInt(h.get("ModifyUserId")) > 0) {                                                 // :104
            num2 = repo.voucherHeadId(u, DOCUMENT_TYPE_ID, num3);
            if (num2 > 0) vh.put("Id", num2);
        }
        vh.put("DocumentTypeSrNo", num3);
        int num = repo.setProc(num2 == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", vh);
        if (num > 0) vh.put("Id", num);
        int voucherId = toInt(vh.get("Id"));
        for (Map<String, Object> d : lines) {
            d.put("VoucherHeadId", voucherId);
            repo.setProc("Sp_VoucherDetail_Insert", d);
        }
        int documentTypeIdRef = repo.setProc("Sp_VoucherHead_H_Insert", vh);
        for (Map<String, Object> d : lines) {
            d.put("VoucherHeadId", voucherId);
            d.put("DocumentTypeIdRef", documentTypeIdRef);
            repo.setProc("Sp_VoucherDetail_H_Insert", d);
        }
        repo.postStock(u, DOCUMENT_TYPE_ID, num3);
        return num3;
    }

    /** AT.lst — the loaded attachments not removed, then the new files (stored now). */
    private List<Map<String, Object>> attachmentList(UserAccount u, int recId, OpeningStockStoreDto dto) {
        List<Integer> removed = dto.removeAttachmentIds == null ? new ArrayList<>() : dto.removeAttachmentIds;
        List<com.mst.models.dto.InventoryPosItemRequest.Upload> files = dto.files == null ? new ArrayList<>() : dto.files;
        if (files.size() > 10) throw new IllegalArgumentException("At most ten attachments may be uploaded at once");
        List<Map<String, Object>> existing = recId > 0 ? attachmentRows(u, recId) : new ArrayList<>();
        for (Integer r : removed) {
            if (r == null || existing.stream().noneMatch(x -> toInt(x.get("Id")) == r)) throw new IllegalArgumentException("Attachment does not belong to this record");
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> x : existing) if (!removed.contains(toInt(x.get("Id")))) out.add(x);
        for (com.mst.models.dto.InventoryPosItemRequest.Upload f : files) {
            byte[] bytes = DesktopInventoryItemFileService.decode(f);
            String saved = store.store(u, f.name, bytes);
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Attachment", f.name);
            o.put("UploadedFileCustomName", saved);
            o.put("UploadedFileSizeMb", bytes.length / 1048576d);
            out.add(o);
        }
        return out;
    }

    // =================================================================================== helpers

    /** GetAllOrById(org, comp, 39, id) — and, Deviation 2, not a packing-material opening. */
    private Map<String, Object> owned(UserAccount u, int id) {
        if (id <= 0) return null;
        List<Map<String, Object>> rows = repo.getById(u, DOCUMENT_TYPE_ID, id);
        if (rows.isEmpty()) return null;
        Map<String, Object> h = rows.get(0);
        if (toInt(h.get("Id")) != id || toInt(h.get("DocumentTypeId")) != DOCUMENT_TYPE_ID) return null;
        if (toInt(h.get("CompanyId")) != u.getCompanyId() || toInt(h.get("OrganizationId")) != u.getOrganizationId()) return null;
        if (toInt(h.get("BaseDocumentTypeId")) == 2) return null;
        return h;
    }

    private static Map<String, Object> find(List<Map<String, Object>> rows, int id) {
        for (Map<String, Object> r : rows) if (toInt(r.get("Id")) == id) return r;
        return null;
    }

    private static void fail(String message) { throw new IllegalArgumentException(message); }

    /** ValidateControls, UltraCombo: {@code ActiveRow == null || ToInt(Value) == 0}. */
    private static void combo(Integer v, String field) {
        if (v == null || v == 0) fail(field + " field is required");
    }

    /** ValidateControls, TextBox + ValidType.Double: {@code !double.TryParse(text) || value == 0}. */
    private static void number(String text, String field) {
        Double d = tryParse(text);
        if (d == null || d == 0d) fail(field + " must be a non-zero number");
    }

    /** double.TryParse — NumberStyles.Float | AllowThousands, en-US. */
    private static final Pattern FLOAT = Pattern.compile("^[+-]?(\\d[\\d,]*)?(\\.\\d*)?([eE][+-]?\\d+)?$");
    private static final Pattern NUMBER = Pattern.compile("^[+-]?(\\d[\\d,]*)?(\\.\\d*)?$");

    static Double tryParse(String text) {
        if (text == null) return null;
        String t = text.trim();
        if (t.isEmpty() || !FLOAT.matcher(t).matches() || !t.matches(".*\\d.*")) return null;
        if (t.matches("^[+-]?,.*")) return null;
        try { return Double.parseDouble(t.replace(",", "")); } catch (NumberFormatException e) { return null; }
    }

    /** Conversion.ToDouble(text) — unparseable is 0. */
    static double convDouble(String text) { Double d = tryParse(text); return d == null ? 0d : d; }

    /** Conversion.ToDecimal(text) — decimal.Parse (NumberStyles.Number: no exponent); unparseable is 0. */
    static BigDecimal convDecimal(String text) {
        if (text == null) return BigDecimal.ZERO;
        String t = text.trim();
        if (t.isEmpty() || !NUMBER.matcher(t).matches() || !t.matches(".*\\d.*")) return BigDecimal.ZERO;
        try { return new BigDecimal(t.replace(",", "")); } catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }

    /** Conversion.ToInt(text) — FromDocNoHistory / ToDocNoHistory. */
    private static int convInt(String text) {
        if (text == null) return 0;
        try { return Integer.parseInt(text.trim()); } catch (NumberFormatException e) { return 0; }
    }

    /** .NET Framework {@code d.ToString("###,##0.###")}: 15 significant digits, then half away from zero. */
    static String net3(double d) {
        if (Double.isNaN(d) || Double.isInfinite(d)) return String.valueOf(d);
        BigDecimal b = new BigDecimal(d).round(new MathContext(15, RoundingMode.HALF_EVEN)).setScale(3, RoundingMode.HALF_UP);
        java.text.DecimalFormat f = new java.text.DecimalFormat("###,##0.###", java.text.DecimalFormatSymbols.getInstance(java.util.Locale.US));
        f.setRoundingMode(RoundingMode.HALF_UP);
        return f.format(b);
    }

    /** DataRow[x].ToString() — decimal keeps its scale, double prints as the CLR prints it. */
    private static String net(Object v) {
        if (v == null) return "";
        if (v instanceof BigDecimal) return ((BigDecimal) v).toPlainString();
        if (v instanceof Double || v instanceof Float) return clr(((Number) v).doubleValue());
        return String.valueOf(v);
    }

    private static String trim(String s) { return s == null ? null : s.trim(); }

    private static String day(Object v) {
        if (v == null) return null;
        String s = v instanceof java.util.Date && !(v instanceof java.sql.Date)
                ? new Timestamp(((java.util.Date) v).getTime()).toString() : String.valueOf(v);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    private static final java.time.format.DateTimeFormatter STAMP = java.time.format.DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");

    private static String stamp(Object v) {
        if (v == null) return null;
        if (v instanceof java.sql.Date) return v + " 00:00:00";
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().format(STAMP);
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().format(STAMP);
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).format(STAMP);
        return String.valueOf(v);
    }

    private static int branch(UserAccount u) { return u.getBranchesId() == null ? 0 : u.getBranchesId(); }
    private static int i(Integer v) { return v == null ? 0 : v; }
}
