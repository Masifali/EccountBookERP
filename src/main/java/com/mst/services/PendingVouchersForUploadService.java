package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.PendingVouchersForUploadRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import com.mst.services.datasync.TaxServerUploadTarget;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collection;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

/**
 * 957 "Vouchers (Upload)" - Architecture.WinApp.DataSyncing.frmPendingVouchersForUpload (Accounts, module 2).
 * ":NNN" = line in frmPendingVouchersForUpload.cs.
 *
 * The form lists the local vouchers (document types 1-7, 9, 10, 26, 34, 35) that are not uploaded yet
 * (USP_Voucher_PendingForUpload), shows the lines of the selected one (VoucherHead.GetByID) and "uploads" the checked
 * ones: each is re-read (GetByID), mapped to a voucher of the tax company (VoucherMapping.MapToTaxProjectVoucher), saved
 * to the TAX SERVER database (BLL.TaxProject VoucherHead.Save) and only then marked uploaded locally
 * (USP_VoucherHead_UpdateVoucherUploadStatus). The tax server connection is not known (see TaxServerUploadTarget), so
 * the upload runs every local step up to the tax-server save and stops there with that reason; nothing is written.
 *
 * Rights: the form checks none of its own; opening it needs the screen's View grant, which every call here requires.
 * Tenancy (organization / company / financial year / user) always comes from the session. Web-only hardening, stated:
 * a voucher is only shown / uploaded when it belongs to the signed-in company and is one of the form's document types.
 */
@Service
public class PendingVouchersForUploadService {

    public static final int SCREEN_ID = 957;
    private static final Set<Integer> DOCUMENT_TYPES = new HashSet<Integer>(Arrays.asList(1, 2, 3, 4, 5, 6, 7, 9, 10, 26, 34, 35));
    private static final DateTimeFormatter STAMP = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss");

    /** dtGrid :229 - column -> USP_Voucher_PendingForUpload column it is filled from (:263). */
    private static final String[][] GRID = {
            {"Id", "Id"}, {"DocumentTypeId", "DocumentTypeId"}, {"VoucherDate", "VoucherDate"}, {"VoucherCode", "VoucherCode"},
            {"DocumentType", "DocumentTypeCode"}, {"AccountTitle", "AccountTitle"}, {"VoucherAmount", "VoucherAmount"},
            {"FcyCode", "Currencycode"}, {"ExchangeRate", "ExchangeCurrencyRate"}, {"FcyAmount", "FcAmount"}, {"Remarks", "Remarks"},
            {"EntryUser", "UserName"}, {"EntryDate", "EntryDate"}, {"ModifyUser", "ModifyUserName"}, {"ModifyDate", "ModifyDate"},
            {"ApprovedUser", "ApprovedUserName"}, {"ApprovedDate", "PostDate"}, {"NoOfAttachments", "NoOfAttachments"}};

    private final PendingVouchersForUploadRepository repo;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    private final DesktopAttachmentStore attachmentStore;

    public PendingVouchersForUploadService(PendingVouchersForUploadRepository repo, CurrentUserContext context,
                                           DesktopReportRights rights, DesktopAttachmentStore attachmentStore) {
        this.repo = repo; this.context = context; this.rights = rights; this.attachmentStore = attachmentStore;
    }

    public UserAccount user() {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
        return u;
    }

    // ================================================================================ load / refresh

    /**
     * InitializeComponentCustom :120 (From = ActiveYr.Start_Period, To = Now) and InitializeComponentMethod :137:
     * MultiCurrencyFeatureVisibilty = GetERPFeatureById(6), SubsidiaryAccountAllownOnVouchers = GetERPFeatureById(4),
     * ComboDBCall, StatusFill, ComboBind.
     */
    public Map<String, Object> init() {
        UserAccount u = user();
        Map<String, Object> out = lists(u);
        out.put("yearStart", repo.yearStart(u, context.currentFinancialYearId()));
        out.put("now", STAMP.format(LocalDateTime.now()));
        out.put("multiCurrency", repo.erpFeature(u, 6));
        out.put("subsidiary", repo.erpFeature(u, 4));
        return out;
    }

    /** btnRefresh_Click :207 - ComboBind(ComboDBCall()) + StatusFill(). */
    public Map<String, Object> refresh() { return lists(user()); }

    /** ComboBind :178 - Activity "Account Header" rows -> cmbAccountTitleCpv, "DocumentType" rows -> CmbDocumentType; StatusFill :201. */
    private Map<String, Object> lists(UserAccount u) {
        List<Map<String, Object>> accounts = new ArrayList<Map<String, Object>>();
        List<Map<String, Object>> docTypes = new ArrayList<Map<String, Object>>();
        for (Map<String, Object> r : repo.combos(u)) {
            String type = str(ci(r, "ActivityType"));
            Map<String, Object> m = new LinkedHashMap<String, Object>();
            m.put("Id", toInt(ci(r, "Id")));
            m.put("Name", str(ci(r, "name")));
            if ("Account Header".equals(type)) accounts.add(m);
            else if ("DocumentType".equals(type)) docTypes.add(m);
        }
        List<Map<String, Object>> status = new ArrayList<Map<String, Object>>();
        status.add(statusRow(1, "Not Approved"));
        status.add(statusRow(2, "Approved"));
        status.add(statusRow(3, "All"));
        Map<String, Object> out = new LinkedHashMap<String, Object>();
        out.put("accounts", accounts);
        out.put("documentTypes", docTypes);
        out.put("statuses", status);
        return out;
    }

    private static Map<String, Object> statusRow(int id, String text) {
        Map<String, Object> m = new LinkedHashMap<String, Object>();
        m.put("Id", id);
        m.put("Status", text);
        return m;
    }

    // ================================================================================ search

    /**
     * BtnShow_Click :222 -> VoucherGridFill :229. FinancialYearId / UserId from the session; the date pair goes to the
     * parameter pair of the ticked radio button (Doc / Entry / Modify / Approved date), each date only when its own check
     * box is ticked; DocNo / account only when non-zero; IsApproved from the status text ("All" omits it, an empty or
     * unknown text sends false, as the model's default bool does); DocumentTypeIds = the combo value as text; ActionId
     * 0 / 1 / 2 from the radio group (0 omitted by the BLL).
     */
    public Map<String, Object> rows(Map<String, Object> body) {
        UserAccount u = user();
        Map<String, Object> f = new LinkedHashMap<String, Object>();
        String dateType = str(body.get("dateType"));
        java.sql.Date from = date(body.get("fromDate")), to = date(body.get("toDate"));
        String[] names;
        if ("entry".equals(dateType)) names = new String[] { "EntryFromDate", "EntryToDate" };
        else if ("modify".equals(dateType)) names = new String[] { "ModifyFromDate", "ModifyToDate" };
        else if ("approved".equals(dateType)) names = new String[] { "ApprovedFromDate", "ApprovedToDate" };
        else names = new String[] { "FromDate", "ToDate" };
        if (from != null) f.put(names[0], from);
        if (to != null) f.put(names[1], to);
        int fromNo = toInt(body.get("fromDocNo")), toNo = toInt(body.get("toDocNo"));
        if (fromNo != 0) f.put("DocNoFrom", fromNo);
        if (toNo != 0) f.put("DocNoTo", toNo);
        int account = toInt(body.get("accountId"));
        if (account != 0) f.put("AccountId", account);
        String status = str(body.get("status"));
        if (!"All".equals(status)) f.put("IsApproved", "Approved".equals(status));
        int docType = toInt(body.get("documentTypeId"));
        if (docType != 0) {
            if (!DOCUMENT_TYPES.contains(docType)) throw new AccessDeniedException("The selected document type is not listed on this form");
            f.put("DocumentTypeIds", String.valueOf(docType));
        }
        int action = toInt(body.get("actionId"));
        if (action < 0 || action > 2) throw new IllegalArgumentException("Unknown account filter");
        if (action != 0) f.put("ActionId", action);

        List<Map<String, Object>> raw = repo.pending(u, context.currentFinancialYearId(), f);
        List<Map<String, Object>> rows = new ArrayList<Map<String, Object>>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> row = new LinkedHashMap<String, Object>();
            for (String[] c : GRID) row.put(c[0], plain(ci(r, c[1])));
            rows.add(row);
        }
        Map<String, Object> out = new LinkedHashMap<String, Object>();
        out.put("rows", rows);
        if (!raw.isEmpty()) {                                              // :276 - read from the first row
            out.put("totalVouchers", toInt(ci(raw.get(0), "TotalVouchers")));
            out.put("approvedVouchers", toInt(ci(raw.get(0), "TotalApprovedVoucher")));
            out.put("unApprovedVouchers", toInt(ci(raw.get(0), "TotalUnApprovedVoucher")));
        }
        return out;
    }

    // ================================================================================ selected record

    /**
     * DataGridHistory_SelectionChanged :445 -> VoucherDetailByHeaderId :456: VoucherHead.GetByID(Id), lines ordered by
     * the line Id; a voucher without lines clears the grid. GetByID returns the first row of ReadByID, so an unknown id
     * is the List indexer's exception.
     */
    public Map<String, Object> voucher(int id) {
        UserAccount u = user();
        Map<String, Object> h = ownedHeader(u, id);
        List<Map<String, Object>> lines = repo.details(id);
        List<Map<String, Object>> sorted = new ArrayList<Map<String, Object>>(lines);
        sorted.sort((a, b) -> Integer.compare(toInt(ci(a, "Id")), toInt(ci(b, "Id"))));
        List<Map<String, Object>> rows = new ArrayList<Map<String, Object>>();
        for (Map<String, Object> d : sorted) {
            double debit = toDouble(ci(d, "DebitAmount")), credit = toDouble(ci(d, "CreditAmount"));
            double fcy = toDouble(ci(d, "DCurrencyAmount"));
            Map<String, Object> m = new LinkedHashMap<String, Object>();
            m.put("AccountCode", str(ci(d, "AccountCode")));
            m.put("AccountTitle", str(ci(d, "AccountTitle")));
            m.put("SubsidiaryAccount", str(ci(d, "SubsidiaryAccountTitle")));
            m.put("JobLot", str(ci(d, "JobLotDescription")));
            m.put("Remarks", str(ci(d, "Comments")));
            m.put("DebitAmount", debit);
            m.put("CreditAmount", credit);
            m.put("DebitFcyAmount", debit > 0.0 ? fcy : 0.0);              // :476
            m.put("CreditFcyAmount", credit > 0.0 ? fcy : 0.0);
            m.put("ChequeDate", plain(ci(d, "DCheqDate")));
            m.put("ChequeNo", str(ci(d, "CheqNoDetail")));
            rows.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<String, Object>();
        out.put("Id", toInt(h.get("Id")));
        out.put("rows", rows);
        return out;
    }

    // ================================================================================ upload

    /**
     * BtnUploadVouchers_Click :550. The page checks "Check any row first" and asks "Are you sure to Upload Selected
     * vouchers?" first; this runs the loop over the distinct checked voucher ids in grid order: GetByID, "... is not
     * found in database" (no voucher / no lines), "... is already uploaded", the mapping (including its tax company setup
     * read) and the tax-server Save - which cannot run from the web, so it refuses there and the local status write
     * (UpdateVoucherUploadStatus) is never reached, exactly as when Save throws on the desktop.
     */
    public Map<String, Object> upload(Map<String, Object> body) {
        UserAccount u = user();
        List<Map<String, Object>> checked = new ArrayList<Map<String, Object>>();
        Object rowsObj = body.get("rows");
        if (rowsObj instanceof Collection) {
            for (Object o : (Collection<?>) rowsObj) {
                if (o instanceof Map) {
                    Map<String, Object> x = new HashMap<String, Object>();
                    for (Map.Entry<?, ?> e : ((Map<?, ?>) o).entrySet()) x.put(String.valueOf(e.getKey()), e.getValue());
                    checked.add(x);
                }
            }
        }
        if (checked.isEmpty()) throw new IllegalArgumentException("Check any row first");

        LinkedHashSet<Integer> ids = new LinkedHashSet<Integer>();
        for (Map<String, Object> r : checked) ids.add(toInt(r.get("Id")));
        for (int voucherId : ids) {
            int docNo = 0;                                                  // :562 - VoucherCode of the first checked row of this voucher
            for (Map<String, Object> r : checked) {
                if (toInt(r.get("Id")) == voucherId) { docNo = toInt(r.get("VoucherCode")); break; }
            }
            Map<String, Object> h = ownedHeader(u, voucherId);
            List<Map<String, Object>> lines = repo.details(voucherId);
            if (lines.isEmpty()) throw new IllegalArgumentException("Selected Voucher : " + docNo + " is not found in database");
            if (bool(ci(h, "IsUploaded"))) throw new IllegalArgumentException("Selected Voucher : " + docNo + " is already uploaded");
            if (repo.taxCompanySetup(toInt(ci(h, "OrganizationId")), toInt(ci(h, "CompanyId"))).isEmpty())
                throw new IllegalArgumentException("Tax company setup not found for the given company.");   // VoucherMapping :57
            throw new IllegalArgumentException(TaxServerUploadTarget.notConfigured("Selected Voucher : " + docNo));   // :578 Save(...)
        }
        throw new IllegalStateException("Nothing to upload");
    }

    // ================================================================================ attachments (NoOfAttachments link)

    /** DataGridHistory_LinkClicked :431 - CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId); other tenants dropped. */
    public List<Map<String, Object>> attachments(int id, int documentTypeId) {
        UserAccount u = user();
        List<Map<String, Object>> out = new ArrayList<Map<String, Object>>();
        if (id <= 0 || documentTypeId <= 0) return out;
        for (Map<String, Object> r : repo.attachments(id, documentTypeId)) {
            if (toInt(ci(r, "OrganizationId")) != toInt(u.getOrganizationId()) || toInt(ci(r, "CompanyId")) != toInt(u.getCompanyId())) continue;
            Map<String, Object> a = new LinkedHashMap<String, Object>();
            a.put("Id", toInt(ci(r, "Id")));
            a.put("AttachmentName", str(ci(r, "Attachment")));
            a.put("CustomName", str(ci(r, "UploadedFileCustomName")));
            a.put("EntryDate", plain(ci(r, "EntryDate")));
            out.add(a);
        }
        return out;
    }

    public static final class Download {
        public final String name; public final byte[] bytes;
        Download(String name, byte[] bytes) { this.name = name; this.bytes = bytes; }
    }

    public Download attachment(int id, int documentTypeId, int attachmentId) {
        UserAccount u = user();
        for (Map<String, Object> a : attachments(id, documentTypeId)) {
            if (toInt(a.get("Id")) != attachmentId) continue;
            String stored = basename(str(a.get("CustomName")).trim().isEmpty() ? str(a.get("AttachmentName")) : str(a.get("CustomName")));
            String name = str(a.get("AttachmentName")).trim().isEmpty() ? stored : basename(str(a.get("AttachmentName")));
            return new Download(name, attachmentStore.read(u, stored));
        }
        throw new IllegalArgumentException("Attachment not found");
    }

    // ================================================================================ helpers

    /** GetByID(...)[0] - no row -> the List indexer's exception text; another company / document type is refused. */
    private Map<String, Object> ownedHeader(UserAccount u, int id) {
        List<Map<String, Object>> headers = id > 0 ? repo.header(id) : new ArrayList<Map<String, Object>>();
        if (headers.isEmpty())
            throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
        Map<String, Object> h = headers.get(0);
        if (toInt(ci(h, "OrganizationId")) != toInt(u.getOrganizationId()) || toInt(ci(h, "CompanyId")) != toInt(u.getCompanyId()))
            throw new AccessDeniedException("The selected voucher does not belong to this company");
        if (!DOCUMENT_TYPES.contains(toInt(ci(h, "DocumentTypeId"))))
            throw new AccessDeniedException("The selected document is not a voucher listed on this form");
        return h;
    }

    private static String basename(String name) {
        String v = name == null ? "" : name.replace('\\', '/');
        v = v.substring(v.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(v);
        return v;
    }

    private static java.sql.Date date(Object v) {
        String s = v == null ? "" : String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.length() > 10 ? s.substring(0, 10) : s)); }
        catch (RuntimeException e) { throw new IllegalArgumentException("Invalid date: " + s); }
    }

    private static Object ci(Map<String, Object> row, String name) {
        if (row == null) return null;
        if (row.containsKey(name)) return row.get(name);
        for (Map.Entry<String, Object> e : row.entrySet()) if (e.getKey() != null && e.getKey().equalsIgnoreCase(name)) return e.getValue();
        return null;
    }

    private static boolean bool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        return v != null && "true".equalsIgnoreCase(String.valueOf(v).trim());
    }

    /** Conversion.ToInt: blank / non-numeric -> 0. */
    private static int toInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        String s = v == null ? "" : String.valueOf(v).trim();
        if (s.isEmpty()) return 0;
        try { return (int) Math.rint(Double.parseDouble(s)); } catch (NumberFormatException e) { return 0; }
    }

    private static double toDouble(Object v) {
        if (v instanceof Number) return ((Number) v).doubleValue();
        String s = v == null ? "" : String.valueOf(v).trim();
        if (s.isEmpty()) return 0d;
        try { return Double.parseDouble(s.replace(",", "")); } catch (NumberFormatException e) { return 0d; }
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    private static Object plain(Object v) {
        if (v instanceof Timestamp) return STAMP.format(((Timestamp) v).toLocalDateTime());
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString() + "T00:00:00";
        if (v instanceof LocalDateTime) return STAMP.format((LocalDateTime) v);
        return v;
    }
}
