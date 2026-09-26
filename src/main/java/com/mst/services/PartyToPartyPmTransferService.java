package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.PartyToPartyPmTransferDto;
import com.mst.repositories.PartyToPartyPmTransferRepository;
import com.mst.security.CurrentUserContext;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;
import static com.mst.repositories.support.DesktopProc.params;

/**
 * Store Management (ModuleId 24) — screen 335 "Packing Material Transfer (Party to Party) (Not Use)".
 *
 * <ul>
 *   <li>Desktop form: {@code Architecture.WinApp.StoreManagement.PartyToPartyPackingMaterialTransfer}
 *       (PartyToPartyPackingMaterialTransfer.cs, designer at the bottom of the same file).</li>
 *   <li>ScreenDefinition.ScreenName (Id 335): {@code frmPackingMaterialTransferPartyToParty} — the key
 *       every dbo.ScreenRights row for this screen carries. The form's own {@code base.Name} (designer
 *       :2774) is {@code PartyToPartyPackingMaterialTransfer} — the same text as DocumentType 125's
 *       ScreenName — and that is what Form_Load:229 passes to SetRightsValueInRightsObject. See
 *       DEVIATION 1.</li>
 *   <li>DocumentTypeId 125 ("Pm-Trans", "Packing Material Transfer Party to Party").</li>
 *   <li>Page {@code /store/party-to-party-pm-transfer}, API {@code /api/store/party-to-party-pm-transfer}.</li>
 *   <li>BLL 0251 / DAL 0220 {@code *.PurchaseTrading.InvPmStockWithPartiesTransferHeader};
 *       Model 0196 InvPmStockWithPartiesTransferHeader, 0195 InvPmStockWithPartiesTransferDetail;
 *       CommonServices.P2PPMSlip419 (:8180), dtUomFromGloablUomScheduleByItemId (:2159),
 *       GetConfigurationByOrgCompandConfigDescription (:2385); DropDownBind.BindDDL;
 *       InfragisticsHelper.BindAndRetainSelection; GridEX_Helper.GridWrappingAndColumnSettings.</li>
 * </ul>
 *
 * SAVE PATH (DAL 0220 SetData:253, one SqlTransaction — here one Spring transaction):
 *   1. Sp_InvPmStockWithPartiesTransferHeader_Insert (Id == 0) / _Update (Id > 0) with every
 *      non-virtual property of Model 0196 in declaration order; ExecuteScalar > 0 becomes the Id,
 *      else the Id sent is kept (the update procedure returns no row).
 *   2. Sp_InvPmStockWithPartiesTransferDetail_Insert once per grid row, with every non-virtual
 *      property of Model 0195 in declaration order (the update procedure has already deleted the
 *      old detail rows).
 *   There is no stock posting, no inventory validation, no voucher and no date-lock in this DAL.
 *
 * ---------------------------------------------------------------------------------------------
 * DESKTOP BEHAVIOUR REPRODUCED (NOT CORRECTED)
 * ---------------------------------------------------------------------------------------------
 *  1. History "Entry Date" filter sends no date at all: FormHistorybind:663/674 tests rdpqtydoc
 *     twice, so the entry-date branch is unreachable and Entry Date behaves as "no date filter".
 *  2. After a NEW save the Print Preview slip is asked for RecId, which btnSave_Click:632 set to 0
 *     and Insert() never updates — USP_PmStockWithPartiesTransfer_SlipAndRegister @Id = 0 returns
 *     nothing and the desktop shows "Record Not Found For Dispaly" (spelling as in CommonServices
 *     :8203). After an UPDATE it prints the updated document. New/Reset never clears RecId, so the
 *     toolbar 219-Print keeps printing the last opened/updated document (page script).
 *  3. Doc No shown is GenerateCode; Sp_..._Insert regenerates DocNo itself (MAX+1), so the stored
 *     number can differ from the one shown. Update writes the opened document's DocNo back.
 *  4. IsApproved is always sent false, ApprovedDate = now, ApprovedUserId 0 (never set); the
 *     procedures ignore IsApproved/Approved* and the insert ignores ModifyDate/ModifyUserId. There is
 *     no approved-record check before update.
 *  5. Detail ItemRate 0, ItemAmount 0, SortNo 0 (never set). Detail Id is the row's Id on update,
 *     0 on insert (the procedure generates its own Id either way).
 *  6. Editing a grid row (double-click → Update) never changes the row's Item Condition: the
 *     double-click does not put the row's condition into the combo and btnUpdateDetail_Click:1059
 *     does not write ItemConditionId/ItemCondition back, yet the combo must still hold a condition
 *     to pass FormValidationDetail (page script).
 *  7. SupplierFrombind:386 binds "Supplier To" retaining "Supplier From"'s previous value (Refresh).
 *  8. The item search mode comes from configuration key "" (Form_Load:245) — normally absent, so
 *     "Name" is checked.
 *  9. The history grid has no total row: GridEX_Helper.GridColumnSettings formats and sums only
 *     float/double/decimal columns (IsColumnNumeric), so the int DocNo column stays unformatted and
 *     centred; the form grid and the history-detail grid total Qty (a double column) — page script.
 * 10. The grid has no row delete (Janus AllowDelete not set) and the form has no Delete button.
 * 11. btnNewHistory (Reset) puts From Date back to today − 3 regardless of the configured
 *     DefaultDaysToLessFromHistoryFromDate that Form_Load used.
 *
 * ---------------------------------------------------------------------------------------------
 * DEVIATIONS (WEB ONLY)
 * ---------------------------------------------------------------------------------------------
 *  1. Rights are read with ScreenName "frmPackingMaterialTransferPartyToParty" (the ScreenDefinition
 *     row that holds this screen's ScreenRights grants), not with base.Name
 *     "PartyToPartyPackingMaterialTransfer" which has no ScreenDefinition row — on the desktop only
 *     the Admin short-circuit would ever get Save/Update/Print.
 *  2. Rights are enforced server-side: Save right for a new document, Update right for an update,
 *     Print right for every slip (the desktop's history Print button and the after-save preview do
 *     not check Print).
 *  3. Doc No is never taken from the request (read-only box): generator on insert, stored header on
 *     update. A document is opened/printed/updated only when it is DocumentTypeId 125 of the
 *     user's organization and company; a missing one answers "Record Not Found" (the desktop's
 *     GetByID would fail with an index-out-of-range message).
 *  4. Each posted row is re-validated in FormValidationDetail order with its messages: the item must
 *     be an ItemTypeOfTypeId 14 item, the UOM one of that item's UOMs, Qty non-zero, the condition
 *     one of the offered conditions, both parties parties of the list (CustomerGroupId != 7), and
 *     the two parties different — a value already stored on the document being updated is accepted
 *     as the desktop would re-save it.
 *  5. A new header whose procedure returns no id is refused (the DAL would carry on with Id 0).
 *
 * NOT PORTED: attachments (btnattachment / Attachment form / FormHelper attachment helpers — the
 * DAL's attachment branch is therefore never taken), saved grid layouts and the ctrlGrdBar menus,
 * keyboard shortcuts and the ShortCut Keys popup, Enter-as-Tab, the Crystal layout of
 * 419-PartyToPartyPackingMaterialSlip.rpt (the procedure's rows are returned and printed as a table),
 * exact column widths (Constants.InventoryConstants values are not in the recovered source).
 */
@Service
public class PartyToPartyPmTransferService {

    public static final int DOCUMENT_TYPE_ID = 125;
    /** ScreenDefinition 335 ScreenName — the rights key (see DEVIATION 1). */
    public static final String SCREEN_NAME = "frmPackingMaterialTransferPartyToParty";
    /** The form's base.Name (designer :2774). */
    public static final String FORM_NAME = "PartyToPartyPackingMaterialTransfer";
    /** ItemBind:293 — ItemTypeOfTypeId == 14. */
    public static final int ITEM_TYPE_OF_TYPE_ID = 14;

    private final PartyToPartyPmTransferRepository repo;
    private final StoreScreenRights rights;
    private final CurrentUserContext ctx;

    public PartyToPartyPmTransferService(PartyToPartyPmTransferRepository repo, StoreScreenRights rights,
                                         CurrentUserContext ctx) {
        this.repo = repo;
        this.rights = rights;
        this.ctx = ctx;
    }

    // ====================================================================================== load

    /** Form_Load:222 — rights, the global lists, GenerateCode, search mode, history From Date days. */
    public Map<String, Object> lookups() {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", rights.of(SCREEN_NAME));
        out.put("screenName", SCREEN_NAME);
        out.put("formName", FORM_NAME);
        /* :245 GetConfigurationByOrgCompandConfigDescription("") — the key is literally empty. */
        out.put("itemSearchByCode", toBool(repo.config(u, "")));
        out.putAll(globals(u));
        out.put("docNo", repo.generateCode(u, DOCUMENT_TYPE_ID));                                 // :276
        /* :279 */
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        out.put("today", LocalDate.now().toString());
        return out;
    }

    /** btnRefresh_Click:829 — UomSchedule, SupplierCustomerLists, AllItems, ItemConditions reloaded. */
    public Map<String, Object> globals() {
        return globals(ctx.requireAccountingUser());
    }

    private Map<String, Object> globals(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("items", repo.items(u, ITEM_TYPE_OF_TYPE_ID));                                    // ItemBind:288
        out.put("itemConditions", repo.itemConditions());                                         // :328
        out.put("parties", repo.parties(u));                                                      // SupplierFrombind:366
        out.put("uoms", repo.uoms(u));                                                            // cmbitem_Leave:399
        return out;
    }

    /** Reset:802 — GenerateCode. */
    public int nextDocNo() {
        return repo.generateCode(ctx.requireAccountingUser(), DOCUMENT_TYPE_ID);
    }

    // =================================================================================== history

    /**
     * FormHistorybind:653. mode: "doc" | "entry" | "modify" | "approved" (the four radios);
     * fromDate / toDate are sent only when their picker is checked.
     */
    public List<Map<String, Object>> history(String mode, String fromDate, String toDate,
                                             String fromDocNo, String toDocNo) {
        UserAccount u = ctx.requireAccountingUser();
        Timestamp from = StoreIssuanceService.dateOnly(fromDate), to = StoreIssuanceService.dateOnly(toDate);
        Timestamp dFrom = null, dTo = null, mFrom = null, mTo = null, aFrom = null, aTo = null;
        String m = mode == null ? "doc" : mode;
        if ("doc".equals(m)) { dFrom = from; dTo = to; }                                          // :663
        /* :674 "else if (rdpqtydoc.Checked)" — the Entry Date radio never reaches its branch. */
        else if ("modify".equals(m)) { mFrom = from; mTo = to; }                                  // :685
        else if ("approved".equals(m)) { aFrom = from; aTo = to; }                                // :696
        List<Map<String, Object>> rows = repo.formHistory(u, DOCUMENT_TYPE_ID,
                toInt(fromDocNo), toInt(toDocNo),                                                   // :707-708
                dFrom, dTo, null, null, mFrom, mTo, aFrom, aTo);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {                                                      // :724-727
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(r.get("Id")));
            o.put("DocNo", toInt(r.get("DocNo")));
            o.put("DocDate", day(r.get("DocDate")));
            o.put("EntryUser", str(r.get("EntryUser")));
            o.put("EntryDate", stamp(r.get("EntryDate")));
            o.put("ModifyUser", str(r.get("ModifyUser")));
            o.put("ModifyDate", stamp(r.get("ModifyDate")));
            o.put("ApprovedUser", str(r.get("ApprovedUser")));
            o.put("ApprovedDate", stamp(r.get("ApprovedDate")));
            o.put("Remarks", str(r.get("RemarksHeader")));
            out.add(o);
        }
        return out;
    }

    // ====================================================================================== read

    /**
     * ReadById:858 / BindHistoryDetailGrid:1005 — BLL GetByID: the header row and its detail rows,
     * each detail row in the form grid's columns (:876).
     */
    public Map<String, Object> load(int id) {
        UserAccount u = ctx.requireAccountingUser();
        Map<String, Object> h = owned(u, id);
        if (h == null) return null;
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", toInt(h.get("Id")));
        o.put("DocNo", toInt(h.get("DocNo")));
        o.put("DocDate", day(h.get("DocDate")));
        o.put("RemarksHeader", str(h.get("RemarksHeader")));
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.details(toInt(h.get("Id")))) {
            Map<String, Object> r = new LinkedHashMap<>();
            r.put("Id", toInt(d.get("Id")));
            r.put("ItemId", toInt(d.get("ItemId")));
            r.put("ItemCode", str(d.get("ItemCode")));
            r.put("ItemName", str(d.get("ItemName")));
            r.put("PackUomId", toInt(d.get("ItemUomId")));
            r.put("PackUom", str(d.get("UOMCode")));
            r.put("ItemConditionId", toInt(d.get("ItemConditionId")));
            r.put("ItemCondition", str(d.get("ItemCondition")));
            r.put("Qty", toDouble(d.get("ItemQty")));
            r.put("Remarks", str(d.get("RemarksDetail")));
            r.put("SupplierFromId", toInt(d.get("SupplierIdFrom")));
            r.put("SupplierFrom", str(d.get("SupplierFrom")));
            r.put("SupplierToId", toInt(d.get("SupplierIdTo")));
            r.put("SupplierTo", str(d.get("SupplierTo")));
            rows.add(r);
        }
        o.put("rows", rows);
        return o;
    }

    /** btnprint_Click:1423 / history Print:1155 / Insert:619 → CommonServices.P2PPMSlip419(PrintId). */
    public List<Map<String, Object>> slip(int id) {
        UserAccount u = ctx.requireAccountingUser();
        if (!rights.has(SCREEN_NAME, "print")) throw new IllegalStateException("You do not have the Print right for this screen.");
        if (id != 0 && owned(u, id) == null) throw new IllegalArgumentException("Record Not Found For Dispaly");
        List<Map<String, Object>> rows = repo.slip(u, id);
        if (rows.isEmpty()) throw new IllegalArgumentException("Record Not Found For Dispaly");         // :8203
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) {
                Object v = e.getValue();
                if (v instanceof byte[]) continue;                                               // CompLogoImage
                o.put(e.getKey(), v instanceof java.util.Date ? stamp(v) : v);
            }
            out.add(o);
        }
        return out;
    }

    // ====================================================================================== save

    /** btnSave_Click:628 / btnUpdate_Click:641 → Insert():546 → BLL 0251 Save → DAL 0220 SetData. */
    @Transactional
    public Map<String, Object> save(PartyToPartyPmTransferDto dto) {
        UserAccount u = ctx.requireAccountingUser();
        if (dto == null) throw new IllegalArgumentException("Nothing to save.");
        int recId = dto.Id == null ? 0 : dto.Id;
        if (recId == 0 && !rights.has(SCREEN_NAME, "save")) throw new IllegalStateException("You do not have the Save right for this screen.");
        if (recId != 0 && !rights.has(SCREEN_NAME, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");

        List<PartyToPartyPmTransferDto.Row> rows = dto.rows == null ? new ArrayList<>() : dto.rows;
        if (rows.isEmpty()) throw new IllegalArgumentException("Please Insert Any Record In Grid");     // :554

        Map<String, Object> stored = null;
        List<Map<String, Object>> storedRows = new ArrayList<>();
        int docNo;
        if (recId > 0) {
            stored = owned(u, recId);
            if (stored == null) throw new IllegalArgumentException("Record Not Found");
            storedRows = repo.details(recId);
            docNo = toInt(stored.get("DocNo"));                                                    // txtVoucherCode (ReadById:870)
        } else {
            docNo = repo.generateCode(u, DOCUMENT_TYPE_ID);                                          // txtVoucherCode (Reset:802)
        }
        if (docNo == 0) throw new IllegalArgumentException("DocNo field is required");               // FormValidation:490

        List<BigDecimal> qtys = validateRows(u, rows, storedRows);

        Timestamp now = Timestamp.valueOf(LocalDateTime.now().withNano(0));
        /* :575-586 — Model 0196 non-virtual properties in declaration order. */
        Map<String, Object> head = params(
                "IsApproved", false,                                                                 // :578
                "ApprovedDate", now,                                                                 // :586
                "DocDate", StoreIssuanceService.formDate(dto.DocDate, recId),                        // :576
                "EntryDate", now,                                                                    // :582
                "ModifyDate", now,                                                                   // :584
                "ApprovedUserId", 0,                                                                 // never set
                "CompanyId", u.getCompanyId(),
                "DocNo", docNo,                                                                      // :575
                "DocumentTypeId", DOCUMENT_TYPE_ID,                                                  // :579
                "EntryUserId", u.getId(),
                "Id", recId,                                                                         // :569
                "ModifyUserId", u.getId(),
                "OrganizationId", u.getOrganizationId(),
                "RemarksHeader", trim(dto.RemarksHeader));                                          // :577
        int ret = repo.setProc(recId > 0 ? PartyToPartyPmTransferRepository.P_UPDATE : PartyToPartyPmTransferRepository.P_INSERT, head);
        int headerId = ret > 0 ? ret : recId;                                                        // DAL :282-289
        if (headerId <= 0) throw new IllegalStateException("The document could not be saved (no id returned).");

        for (int i = 0; i < rows.size(); i++) {                                                      // :589-601, DAL :290-294
            PartyToPartyPmTransferDto.Row r = rows.get(i);
            Map<String, Object> det = params(
                    "ItemQty", qtys.get(i),                                                          // :597 ToDecimal(Qty)
                    "ItemRate", BigDecimal.ZERO,
                    "ItemAmount", 0d,
                    "Id", recId > 0 ? nz(r.Id) : 0,                                                  // :592
                    "InvPmStockWithPartiesTransferHeaderId", headerId,                               // DAL :292
                    "ItemId", nz(r.ItemId),
                    "ItemUomId", nz(r.PackUomId),
                    "SortNo", 0,
                    "SupplierIdFrom", nz(r.SupplierFromId),
                    "SupplierIdTo", nz(r.SupplierToId),
                    "ItemConditionId", nz(r.ItemConditionId),
                    "RemarksDetail", r.Remarks == null ? "" : r.Remarks);                           // :598 ToString(cell)
            repo.setProc(PartyToPartyPmTransferRepository.P_DETAIL_INSERT, det);
        }

        Map<String, Object> out = new LinkedHashMap<>();
        out.put("id", headerId);
        out.put("recId", recId);
        out.put("message", recId > 0 ? "Record Updated Successfully" : "Record Saved Successfully");   // :609 / :614
        return out;
    }

    /**
     * FormValidationDetail:499, per posted row, in the desktop's order and with its messages (see
     * DEVIATION 4). Returns each row's Qty as Conversion.ToDecimal of the cell.
     */
    private List<BigDecimal> validateRows(UserAccount u, List<PartyToPartyPmTransferDto.Row> rows,
                                          List<Map<String, Object>> storedRows) {
        Set<Integer> items = ids(repo.items(u, ITEM_TYPE_OF_TYPE_ID), "Id");
        Set<Integer> conditions = ids(repo.itemConditions(), "Id");
        Set<Integer> parties = ids(repo.parties(u), "Id");
        Set<String> itemUoms = new HashSet<>();
        for (Map<String, Object> m : repo.uoms(u)) itemUoms.add(toInt(m.get("ItemId")) + ":" + toInt(m.get("Id")));

        Set<Integer> sItems = new HashSet<>(), sConds = new HashSet<>(), sParties = new HashSet<>();
        Set<String> sItemUoms = new HashSet<>(), sPairs = new HashSet<>();
        for (Map<String, Object> d : storedRows) {
            sItems.add(toInt(d.get("ItemId")));
            sConds.add(toInt(d.get("ItemConditionId")));
            sParties.add(toInt(d.get("SupplierIdFrom")));
            sParties.add(toInt(d.get("SupplierIdTo")));
            sItemUoms.add(toInt(d.get("ItemId")) + ":" + toInt(d.get("ItemUomId")));
            sPairs.add(toInt(d.get("SupplierIdFrom")) + ":" + toInt(d.get("SupplierIdTo")));
        }

        List<BigDecimal> out = new ArrayList<>();
        for (PartyToPartyPmTransferDto.Row r : rows) {
            if (r == null) throw new IllegalArgumentException("ItemName Field is Required");
            int item = nz(r.ItemId), uom = nz(r.PackUomId), cond = nz(r.ItemConditionId);
            int from = nz(r.SupplierFromId), to = nz(r.SupplierToId);
            if (!(item != 0 && items.contains(item)) && !(item != 0 && sItems.contains(item)))
                throw new IllegalArgumentException("ItemName Field is Required");
            String iu = item + ":" + uom;
            if (!itemUoms.contains(iu) && !sItemUoms.contains(iu))
                throw new IllegalArgumentException("ItemUOM Field is Required");
            BigDecimal qty = decimal(r.Qty);
            if (qty.signum() == 0) throw new IllegalArgumentException("ItemQty Field is Required");
            if (!(cond != 0 && conditions.contains(cond)) && !sConds.contains(cond))
                throw new IllegalArgumentException("Item Condition Field is Required");
            if (!(from != 0 && parties.contains(from)) && !(from != 0 && sParties.contains(from)))
                throw new IllegalArgumentException("SupplierFrom value Field is Required");
            if (!(to != 0 && parties.contains(to)) && !(to != 0 && sParties.contains(to)))
                throw new IllegalArgumentException("SupplierTo Field is Required");
            if (from == to && !sPairs.contains(from + ":" + to))
                throw new IllegalArgumentException("Supplier From && Supplier To can't be same");
            out.add(qty);
        }
        return out;
    }

    // ================================================================================== helpers

    /** GetByID, limited to DocumentTypeId 125 of the user's organization and company. */
    private Map<String, Object> owned(UserAccount u, int id) {
        if (id <= 0) return null;
        List<Map<String, Object>> rows = repo.readById(id);
        if (rows.isEmpty()) return null;
        Map<String, Object> h = rows.get(0);
        if (toInt(h.get("DocumentTypeId")) != DOCUMENT_TYPE_ID) return null;
        if (toInt(h.get("CompanyId")) != nz(u.getCompanyId())) return null;
        if (toInt(h.get("OrganizationId")) != nz(u.getOrganizationId())) return null;
        return h;
    }

    private static Set<Integer> ids(List<Map<String, Object>> rows, String key) {
        Set<Integer> s = new HashSet<>();
        for (Map<String, Object> r : rows) s.add(toInt(r.get(key)));
        return s;
    }

    /** Conversion.ToDecimal of the Qty cell (a double column filled from txtItemQty.Text); 0 when unparseable. */
    private static BigDecimal decimal(String text) {
        if (text == null) return BigDecimal.ZERO;
        String t = text.trim().replace(",", "");
        if (t.isEmpty()) return BigDecimal.ZERO;
        try { return new BigDecimal(t); } catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }

    /** Conversion.ToBool — "true"/"1" (any case) is true, anything else false. */
    private static boolean toBool(String v) {
        if (v == null) return false;
        String t = v.trim();
        return "1".equals(t) || "true".equalsIgnoreCase(t);
    }

    private static String trim(String s) { return s == null ? "" : s.trim(); }

    private static int nz(Integer v) { return v == null ? 0 : v; }

    /** A DATE column as "yyyy-MM-dd". */
    private static String day(Object v) {
        if (v == null) return null;
        String s = String.valueOf(v);
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }

    /** A DATETIME column as "yyyy-MM-ddTHH:mm:ss" (null stays null). */
    private static String stamp(Object v) {
        if (v == null) return null;
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().withNano(0).toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().withNano(0).toString();
        return String.valueOf(v).replace(' ', 'T');
    }
}
