package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.ContraVoucherDto;
import com.mst.repositories.StoreIssuanceFinancialRepository;
import com.mst.repositories.StoreIssuanceRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.lang.reflect.Field;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.StoreIssuanceRepository.ci;
import static com.mst.repositories.StoreIssuanceRepository.clr;
import static com.mst.repositories.StoreIssuanceRepository.str;
import static com.mst.repositories.StoreIssuanceRepository.toDouble;
import static com.mst.repositories.StoreIssuanceRepository.toInt;

/**
 * Store Issuance (Financials) - desktop Architecture.WinApp.StoreManagement.StoreIssuanceFinancial (ScreenId 33,
 * ScreenName "StoreIssuanceFinancial", DocumentTypeId 451 "Store Issuance Financial Effects").
 *
 * The form never creates a document: it opens an existing Store Issuance (451) from the History tab (Edit) and its
 * only write is Update -> Insert() -> InvGsStoreIssuanceHeader.Save (BLL 0252 :108) -> DAL 0221 SetData. Because the
 * form sets ActionForVoucherId = 1, SetData skips the header / detail / stock writes for 451 (DAL :70 "DocumentTypeId
 * == 451 && ActionForVoucherId != 1"); what runs is: lock-date check, MakeVoucher when the configuration
 * "StoreIssuanceFinancialEffect" is true, USP_InventoryValidation for every detail, and the voucher head / details /
 * history rows (Sp_VoucherHead_Insert|Update, Sp_VoucherDetail_Insert, Sp_VoucherHead_H_Insert, Sp_VoucherDetail_H_Insert).
 *
 * The grid is read-only apart from Remarks (grdDetail_DesignTimeLayout / DetailGridSettings) and nothing of it other
 * than IssueQty/ItemRate/ids reaches the BLL, so the detail rows are re-read from the database by the document id
 * rather than trusted from the request. Tenancy, user and financial year come from the signed-in user.
 */
@Service
public class StoreIssuanceFinancialService {

    public static final int SCREEN_ID = 33;
    public static final String SCREEN_NAME = "StoreIssuanceFinancial";
    public static final int DOCUMENT_TYPE_ID = 451;
    private static final DateTimeFormatter STAMP = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss");

    private final StoreIssuanceFinancialRepository repo;
    private final StoreIssuanceRepository store;
    private final StoreScreenRights rights;
    private final DesktopReportRights reportRights;
    private final CurrentUserContext ctx;

    public StoreIssuanceFinancialService(StoreIssuanceFinancialRepository repo, StoreIssuanceRepository store,
                                         StoreScreenRights rights, DesktopReportRights reportRights, CurrentUserContext ctx) {
        this.repo = repo; this.store = store; this.rights = rights; this.reportRights = reportRights; this.ctx = ctx;
    }

    public UserAccount user() {
        UserAccount u = ctx.requireAccountingUser();
        reportRights.require(u, SCREEN_ID, "View");
        return u;
    }

    // ================================================================================ load (frmGSIssuance_Load :99)

    /** SetRightsValueInRightsObject(Name) -> btnUpdate.Enabled; GenerateDocNo; DetailAccountFill. */
    public Map<String, Object> setup() {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<String, Object>();
        out.put("rights", rights.of(SCREEN_NAME));
        out.put("docNo", docNo(u));
        out.put("accounts", accounts(u));
        out.put("today", LocalDate.now().toString());
        return out;
    }

    /** GenerateDocNo :143 - the text box is only set when the code is > 0. */
    public Map<String, Object> docNo() {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<String, Object>();
        out.put("docNo", docNo(u));
        return out;
    }

    private int docNo(UserAccount u) {
        return store.nextDocNo(u, ctx.currentFinancialYearId(), DOCUMENT_TYPE_ID);
    }

    /** DetailAccountFill :158 - "11,12,13,20,21", columns Id / AccountTitle. */
    private List<Map<String, Object>> accounts(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<Map<String, Object>>();
        for (Map<String, Object> r : repo.accountTitles(u, "11,12,13,20,21")) {
            Map<String, Object> m = new LinkedHashMap<String, Object>();
            m.put("Id", toInt(ci(r, "Id")));
            m.put("AccountTitle", str(ci(r, "AccountTitle")));
            out.add(m);
        }
        return out;
    }

    // ================================================================================ History (HistoryFill :552)

    public List<Map<String, Object>> history() {
        UserAccount u = user();
        List<Map<String, Object>> out = new ArrayList<Map<String, Object>>();
        for (Map<String, Object> r : repo.historyForFinancialEffects(u, DOCUMENT_TYPE_ID, ctx.currentFinancialYearId())) {
            Map<String, Object> m = new LinkedHashMap<String, Object>();
            m.put("Id", toInt(ci(r, "Id")));
            m.put("DocDate", plain(ci(r, "DocDate")));
            m.put("DocNo", toInt(ci(r, "DocNo")));
            m.put("Remarks", str(ci(r, "Remarks")));
            m.put("ItemName", str(ci(r, "ItemName")));
            m.put("Equivalent", toDouble(ci(r, "Equivalent")));
            m.put("IssueQty", toDouble(ci(r, "IssueQty")));
            m.put("WareHouseName", str(ci(r, "WareHouseName")));
            m.put("DepartmentName", str(ci(r, "DepartmentName")));
            m.put("AssetName", str(ci(r, "AssetName")));
            m.put("EntryDate", plain(ci(r, "EntryDate")));
            m.put("EntryUser", str(ci(r, "EntryUser")));
            m.put("ModifyDate", plain(ci(r, "ModifyDate")));
            m.put("ModifyUser", str(ci(r, "ModifyUser")));
            out.add(m);
        }
        return out;
    }

    // ================================================================================ Edit (ReadById :395 + txtDocNo_TextChanged :466)

    /**
     * DataGridHistory_ColumnButtonClick "Edit" -> RECID = Id; ReadById(RECID) sets txtDocNo, whose TextChanged runs
     * ReadByDocNo (RECID = the Id found for that number, ReadById again), VoucherHeadIdGet and GetRefAccountId.
     */
    public Map<String, Object> load(int id) {
        UserAccount u = user();
        Map<String, Object> h = ownedHeader(u, id);
        int code = repoIdByDocNo(u, toInt(h.get("DocNo")));
        if (code > 0 && code != id) h = ownedHeader(u, code);
        int recId = toInt(h.get("Id"));
        int refDocType = toInt(h.get("RefDocumentTypeId"));
        List<Map<String, Object>> rows = new ArrayList<Map<String, Object>>();
        for (Map<String, Object> d : store.details(h)) rows.add(gridRow(d));
        Map<String, Object> out = new LinkedHashMap<String, Object>();
        out.put("Id", recId);
        out.put("DocNo", toInt(h.get("DocNo")));
        out.put("DocDate", plain(h.get("DocDate")));
        out.put("Remarks", str(h.get("Remarks")));
        out.put("RefDocNoId", toInt(h.get("RefDocNoId")));
        out.put("RefDocumentTypeId", refDocType);
        out.put("RequestType", refDocType == 85 ? "DeliveryOrder" : "DepartmentRequest");   // :408
        out.put("rows", rows);
        out.put("voucherHeadId", store.voucherHeadId(u, DOCUMENT_TYPE_ID, recId));            // VoucherHeadIdGet :193
        out.put("refAccountId", refAccountId(u, recId));                                      // GetRefAccountId :207
        return out;
    }

    private int repoIdByDocNo(UserAccount u, int docNo) { return repo.idByDocNo(u, DOCUMENT_TYPE_ID, docNo); }

    /** GetRefAccountId - every 451 voucher with DocumentTypeSrNo == RECID sets the combo; the last one wins. */
    private int refAccountId(UserAccount u, int recId) {
        int v = 0;
        for (Map<String, Object> r : repo.vouchersOfType(u, DOCUMENT_TYPE_ID)) {
            if (recId == toInt(ci(r, "DocumentTypeSrNo"))) v = toInt(ci(r, "RefAccountId"));
        }
        return v;
    }

    /** The 21 columns the form's dtgrddetail is filled with (:110-130, :428). */
    private static Map<String, Object> gridRow(Map<String, Object> d) {
        Map<String, Object> m = new LinkedHashMap<String, Object>();
        m.put("Id", toInt(ci(d, "Id")));
        m.put("ItemId", toInt(ci(d, "ItemId")));
        m.put("Item", str(ci(d, "ItemName")));
        m.put("WarehouseId", toInt(ci(d, "Warehouseid")));
        m.put("Warehouse", str(ci(d, "WareHouseName")));
        m.put("RackId", toInt(ci(d, "RackId")));
        m.put("RackName", str(ci(d, "rackName")));
        m.put("ItemConditionId", toInt(ci(d, "ItemConditionId")));
        m.put("ItemCondition", str(ci(d, "ItemCondition")));
        m.put("UnitId", toInt(ci(d, "ItemUomSchId")));
        m.put("Unit", toDouble(ci(d, "Equivalent")));
        m.put("IssueQty", toDouble(ci(d, "IssueQty")));
        m.put("ItemRate", toDouble(ci(d, "ItemRate")));
        m.put("ItemAmount", toDouble(ci(d, "ItemAmount")));
        m.put("DepartmentId", toInt(ci(d, "DepartmentId")));
        m.put("Department", str(ci(d, "DepartmentName")));
        m.put("AssetId", toInt(ci(d, "AssetsId")));
        m.put("Asset", str(ci(d, "AssetName")));
        m.put("Remarks", str(ci(d, "ReamarksDetail")));
        m.put("SupplierCustomerId", toInt(ci(d, "SupplierCustomerId")));
        m.put("SupplierName", str(ci(d, "CompanyName")));
        return m;
    }

    // ================================================================================ Update (btnUpdate_Click :336 -> Insert :193)

    @Transactional
    public Map<String, Object> update(Map<String, Object> body) {
        UserAccount u = user();
        int recId = toInt(body.get("Id"));
        if (recId == 0) throw new IllegalArgumentException("Record Not Found For Update");
        if (!rights.has(SCREEN_NAME, "update")) throw new IllegalStateException("You do not have the Update right for this screen.");
        String docNoText = str(body.get("DocNo")).trim();
        if (repoIdByDocNo(u, toInt(docNoText)) == 0) throw new IllegalArgumentException("Record Not Exist against this number");

        /* Insert() :193 */
        if (docNoText.isEmpty()) throw new IllegalArgumentException("document Number Field Required");
        Map<String, Object> h = ownedHeader(u, recId);
        Timestamp docDate = formDate(str(body.get("DocDate")));
        int accountId = toInt(body.get("AccountId"));
        if (accountId != 0) {
            boolean listed = false;
            for (Map<String, Object> a : accounts(u)) if (toInt(a.get("Id")) == accountId) { listed = true; break; }
            if (!listed) throw new IllegalArgumentException("Account Dr is not one of the accounts listed on this form");
        }
        String remarks = str(body.get("Remarks"));

        List<Map<String, Object>> loaded = store.details(h);
        if (loaded.isEmpty()) throw new IllegalArgumentException("Grid record not found");
        List<Map<String, Object>> details = new ArrayList<Map<String, Object>>();
        for (int idx = 0; idx < loaded.size(); idx++) {
            Map<String, Object> g = loaded.get(idx);
            double qty = toDouble(ci(g, "IssueQty"));
            if (!(qty > 0.0)) continue;
            double rate = toDouble(ci(g, "ItemRate"));
            Map<String, Object> d = new LinkedHashMap<String, Object>();
            d.put("Id", toInt(ci(g, "Id")));
            d.put("LineId", idx + 1);                                   // r.RowIndex + 1
            d.put("ItemId", toInt(ci(g, "ItemId")));
            d.put("Warehouseid", toInt(ci(g, "Warehouseid")));
            d.put("RackId", toInt(ci(g, "RackId")));
            d.put("ItemConditionId", toInt(ci(g, "ItemConditionId")));
            d.put("IssueQty", qty);
            d.put("ItemRate", rate);
            d.put("ItemAmount", qty * rate);
            d.put("DrAccountId", accountId);
            details.add(d);
            validate(d.get("Warehouseid"), "Warehouse", idx);
            validate(d.get("RackId"), "Rack Name", idx);
            validate(d.get("ItemConditionId"), "Item Condition", idx);
        }

        /* BLL 0252 Save :108 */
        Timestamp lock = store.dateLock(u);
        if (lock != null && !docDate.after(lock)) throw new IllegalArgumentException("Not Insert or Update record please check lock date");
        boolean effect = toBool(store.config(u, "StoreIssuanceFinancialEffect"));
        Map<String, Object> voucher = effect ? makeVoucher(u, h, docNoText, docDate, remarks, details) : null;

        /* DAL 0221 SetData: header / detail / stock writes are skipped (ActionForVoucherId == 1) */
        for (Map<String, Object> d : details) {
            store.inventoryValidation(u, DOCUMENT_TYPE_ID, docDate, toInt(d.get("ItemId")), toInt(d.get("ItemConditionId")),
                    toInt(d.get("Warehouseid")), toDouble(d.get("IssueQty")));
        }
        if (voucher != null) writeVoucher(u, recId, voucher, details);

        Map<String, Object> out = new LinkedHashMap<String, Object>();
        out.put("success", true);
        out.put("message", "Record Update SuccessFully" + toInt(docNoText));
        out.put("id", recId);
        out.put("voucherHeadId", store.voucherHeadId(u, DOCUMENT_TYPE_ID, recId));            // :317 VoucherHeadIdGet
        return out;
    }

    // ================================================================================ voucher (BLL 0252 MakeVoucher :20, DAL 0221 :171)

    private Map<String, Object> makeVoucher(UserAccount u, Map<String, Object> h, String docNoText, Timestamp docDate,
                                            String remarks, List<Map<String, Object>> details) {
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        vh.DocumentTypeId = DOCUMENT_TYPE_ID;
        vh.DocumentTypeSrNo = toInt(h.get("Id")) ;               // obj.Id = RECID
        vh.RefAccountId = toInt(h.get("RefDocNoId"));
        vh.VoucherCode = toInt(docNoText);
        vh.VoucherDate = docDate.toString();
        String now = Timestamp.valueOf(LocalDateTime.now()).toString();
        vh.ChequeDate = Timestamp.valueOf(LocalDate.now().atStartOfDay()).toString();
        vh.DueDate = now; vh.EntryDate = now; vh.ModifyDate = now;
        vh.Remarks = remarks;
        vh.RemarksOtherLingo = "";
        vh.IncludeWHT = false;
        vh.BranchId = u.getBranchesId() == null ? 0 : u.getBranchesId();
        vh.ProjectId = 0;
        vh.DueDays = 0;
        vh.OrganizationId = u.getOrganizationId();
        vh.CompanyId = u.getCompanyId();
        vh.FinancialYearId = ctx.currentFinancialYearId();
        vh.EntryUser = u.getId();
        vh.ModifyUser = u.getId();

        List<Map<String, Object>> gl = store.itemGlAccounts(u);
        List<ContraVoucherDto.Detail> lines = new ArrayList<ContraVoucherDto.Detail>();
        int branch = u.getBranchesId() == null ? 0 : u.getBranchesId();
        for (Map<String, Object> item : details) {
            vh.RefAccountId = toInt(item.get("DrAccountId"));             // (451 ? item.DrAccountId : 0)
            if (gl.isEmpty()) throw new IllegalArgumentException("Item Record Not found");
            Map<String, Object> g = null;
            int itemId = toInt(item.get("ItemId"));
            for (Map<String, Object> x : gl) if (toInt(x.get("Id")) == itemId) { g = x; break; }
            if (g == null) throw new IllegalArgumentException("Item Purchase GL Account Not found");
            double qty = toDouble(item.get("IssueQty"));
            double rate = toDouble(item.get("ItemRate"));
            double amount = toDouble(item.get("ItemAmount"));
            int drAc = toInt(item.get("DrAccountId"));
            int cogs = toInt(g.get("COGSGLAC"));
            int purchase = toInt(g.get("PurchaseGLAC"));
            String comments = remarks + "  Qty: " + clr(qty) + "   " + str(g.get("ItemName")) + "   Rate:" + clr(rate);

            ContraVoucherDto.Detail dr = new ContraVoucherDto.Detail();
            dr.LineId = toInt(item.get("LineId"));
            dr.AccountId = drAc > 0 ? drAc : cogs;
            dr.AgainstAccountId = purchase;
            dr.Comments = comments;
            dr.DebitAmount = (double) (float) amount;
            dr.CreditAmount = 0d;
            dr.ItemId = itemId; dr.QtyOut = qty; dr.ItemRate = rate; dr.ItemAmount = amount; dr.BranchesId = branch;
            lines.add(dr);

            ContraVoucherDto.Detail cr = new ContraVoucherDto.Detail();
            cr.LineId = toInt(item.get("LineId"));
            cr.AccountId = purchase;
            cr.AgainstAccountId = drAc > 0 ? drAc : cogs;
            cr.Comments = comments;
            cr.CreditAmount = (double) (float) amount;
            cr.DebitAmount = 0d;
            cr.ItemId = itemId; cr.QtyOut = qty; cr.ItemRate = rate; cr.ItemAmount = amount; cr.BranchesId = branch;
            lines.add(cr);
        }
        Map<String, Object> v = new LinkedHashMap<String, Object>();
        v.put("head", vh);
        v.put("lines", lines);
        return v;
    }

    @SuppressWarnings("unchecked")
    private void writeVoucher(UserAccount u, int docId, Map<String, Object> voucher, List<Map<String, Object>> details) {
        ContraVoucherDto.Head vh = (ContraVoucherDto.Head) voucher.get("head");
        List<ContraVoucherDto.Detail> lines = (List<ContraVoucherDto.Detail>) voucher.get("lines");
        int existingId = store.voucherHeadId(u, DOCUMENT_TYPE_ID, docId);
        vh.Id = existingId;
        vh.DocumentTypeSrNo = docId;
        int num2 = store.setProc(existingId == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", model(vh));
        if (num2 > 0) vh.Id = num2;
        if (lines.isEmpty()) throw new IllegalArgumentException("VoucherDetail List Not Found");
        for (ContraVoucherDto.Detail l : lines) {
            for (Map<String, Object> d : details) {
                if (toInt(d.get("LineId")) == (l.LineId == null ? 0 : l.LineId)) { l.RefDocSubIdNo = toInt(d.get("Id")); break; }
            }
            l.VoucherHeadId = vh.Id;
            store.setProc("Sp_VoucherDetail_Insert", model(l));
        }
        int documentTypeIdRef = store.setProc("Sp_VoucherHead_H_Insert", model(vh));
        for (ContraVoucherDto.Detail l : lines) {
            l.VoucherHeadId = vh.Id;
            l.DocumentTypeIdRef = documentTypeIdRef;
            store.setProc("Sp_VoucherDetail_H_Insert", model(l));
        }
    }

    private static Map<String, Object> model(Object o) {
        Map<String, Object> m = new LinkedHashMap<String, Object>();
        for (Field f : o.getClass().getFields()) {
            if (java.lang.reflect.Modifier.isStatic(f.getModifiers())) continue;
            if (List.class.isAssignableFrom(f.getType())) continue;
            try {
                Object v = f.get(o);
                String n = f.getName();
                if (v != null && ("VoucherDate".equals(n) || "ChequeDate".equals(n) || "DueDate".equals(n) || "EntryDate".equals(n)
                        || "ModifyDate".equals(n) || "PostDate".equals(n) || "DCheqDate".equals(n) || "GpDate".equals(n))) {
                    String s = String.valueOf(v);
                    v = Timestamp.valueOf(s.length() == 10 ? s + " 00:00:00" : s);
                }
                m.put(n, v);
            } catch (IllegalAccessException ignored) { }
        }
        return m;
    }

    // ================================================================================ helpers

    /** GetByID(...)[0] of a 451 document of this company; no row -> the List indexer's text. */
    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        Map<String, Object> h = id > 0 ? store.header(id) : null;
        if (h == null)
            throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        if (toInt(h.get("DocumentTypeId")) != DOCUMENT_TYPE_ID
                || (h.containsKey("CompanyId") && toInt(h.get("CompanyId")) != toInt(u.getCompanyId()))
                || (h.containsKey("OrganizationId") && toInt(h.get("OrganizationId")) != toInt(u.getOrganizationId())))
            throw new AccessDeniedException("The selected document is not a Store Issuance of this company");
        return h;
    }

    /** FormHelper.ValidateField - 0 / blank is "required". */
    private static void validate(Object v, String field, int rowIndex) {
        boolean bad = v == null || (v instanceof Integer && (Integer) v == 0)
                || (v instanceof Double && (Double) v <= 0d) || (v instanceof String && ((String) v).trim().isEmpty());
        if (bad) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    /** txtDocdate.Value of an opened document: the date at midnight (ReadById assigns a DATE column). */
    private static Timestamp formDate(String yyyyMMdd) {
        LocalDate d = (yyyyMMdd == null || yyyyMMdd.trim().length() < 10) ? LocalDate.now() : LocalDate.parse(yyyyMMdd.trim().substring(0, 10));
        return Timestamp.valueOf(d.atStartOfDay());
    }

    private static boolean toBool(String v) {
        if (v == null) return false;
        String t = v.trim();
        return "true".equalsIgnoreCase(t) || "1".equals(t);
    }

    private static Object plain(Object v) {
        if (v instanceof Timestamp) return STAMP.format(((Timestamp) v).toLocalDateTime());
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString() + "T00:00:00";
        if (v instanceof LocalDateTime) return STAMP.format((LocalDateTime) v);
        return v;
    }
}
