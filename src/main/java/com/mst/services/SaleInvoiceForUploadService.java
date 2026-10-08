package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.SaleInvoiceForUploadRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import com.mst.services.datasync.SaleInvoiceTaxMapping;
import com.mst.services.datasync.TaxServerUploadTarget;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.*;
import java.util.stream.Collectors;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

/**
 * 956 "Sale Invoice (Upload)" - Architecture.WinApp.DataSyncing.frmPendingSaleInvoiceForUpload (Customer Sales, module 6).
 * ":NNN" = line in frmPendingSaleInvoiceForUpload.cs.
 *
 * The form lists the local sale invoices (types 95 and 99) that are not uploaded yet (USP_SaleInvoicePendingForUpload) and
 * "uploads" the checked ones: each is re-read (GetByID), mapped to a Sale Invoice Direct (99) of the tax company
 * (SaleInvoiceMapping), saved to the TAX SERVER database (BLL.TaxProject InvSaleInvoice.Save) and only then marked uploaded
 * locally (USP_InvSaleInvoice_UpdateInvoiceUploadStatus). The tax server connection is not known (see TaxServerUploadTarget),
 * so the upload runs every local step up to the tax-server call and stops there with that reason; nothing is written.
 *
 * Differences from frmPendingPurchaseInvoiceForUpload (diffed, all reproduced):
 *  - the first load also runs PendingDataDbCall(useBranchDt: true) and binds the grid (:143, :157);
 *  - the customer combo is filled once for the whole user (Usp_AllComboAgainstSaleInvoice, no branch filter, "95,99");
 *  - there is no CmbBranch_Leave and no digits-only key filter on the document-number boxes;
 *  - the first checked row of an invoice supplies DocNo without the "not 1104" test (:543);
 *  - Escape also closes the form (:615).
 *
 * Rights: the form checks none of its own; opening it needs the screen's View grant, which every call here requires.
 * Tenancy (organization / company / branch / financial year / user) always comes from the session.
 */
@Service
public class SaleInvoiceForUploadService {
    public static final int SCREEN_ID = 956;
    private static final Set<Integer> DOCUMENT_TYPES = Set.of(95, 99);
    private static final DateTimeFormatter STAMP = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss");

    /** dtGrid :400-436 - column name -> USP_SaleInvoicePendingForUpload column it is filled from (:441). */
    private static final String[][] GRID = {
            {"BranchId", "BranchId"}, {"BranchName", "BranchName"}, {"DocumentTypeId", "DocumentTypeId"},
            {"DocumentType", "DocumentType"}, {"Id", "Id"}, {"DocNo", "DocNo"}, {"DocDate", "DocDate"},
            {"SupplierCustomerId", "SupplierCustomerId"}, {"CustomerName", "CustomerName"}, {"PaymentTerm", "TermsDescription"},
            {"GpNo", "GpNo"}, {"VehicleNo", "VehicleNo"}, {"ItemId", "ItemId"}, {"ItemCode", "ItemCode"}, {"ItemName", "ItemName"},
            {"CropYear", "CropYear"}, {"JobLot", "JobLotCode"}, {"PackUom", "PackUomCode"}, {"ItemQty", "ItemQty"},
            {"NetBillWeight", "NetBillWeight"}, {"ItemRate", "ItemRate"}, {"RateUom", "RateUomCode"}, {"ItemAmount", "ItemAmount"},
            {"BillAmount", "BillAmount"}, {"EntryDate", "EntryDate"}, {"EntryUser", "EntryUserName"}, {"ModifyDate", "ModifyDate"},
            {"ModifyUser", "ModifyUserName"}, {"ApprovalStatus", "IsApproved"}, {"ApprovedDate", "ApprovedDate"},
            {"ApprovedUser", "ApprovedUserName"}, {"WareHouseName", "WareHouseName"}, {"CommissionAgent", "CommissionAgent"},
            {"CommAmount", "CommAmount"}, {"NoOfAttachments", "NoOfAttachments"}};

    private final SaleInvoiceForUploadRepository repo;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    private final DesktopAttachmentStore attachmentStore;

    public SaleInvoiceForUploadService(SaleInvoiceForUploadRepository repo, CurrentUserContext context, DesktopReportRights rights,
                                       DesktopAttachmentStore attachmentStore) {
        this.repo = repo; this.context = context; this.rights = rights; this.attachmentStore = attachmentStore;
    }

    public UserAccount user() {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
        return u;
    }

    // ================================================================================ load / refresh

    /**
     * InitializeComponentCustom :118 (FromDate = ActiveYr.Start_Period, Todate = Now) and InitializeComponentMethod :137:
     * BranchImplemented = SaleInvoiceBranchWise, BranchesDbCall, PendingDataDbCall(useBranchDt: true) - EVERY branch in the
     * list, no other filter - ComboDBCall(); then BranchesBind, PendingDataGridBind, ComboBind.
     */
    public Map<String, Object> init() {
        UserAccount u = user();
        Map<String, Object> out = branchesState(u);
        @SuppressWarnings("unchecked")
        List<Map<String, Object>> branches = (List<Map<String, Object>>) out.get("branches");
        String all = branches.stream().map(r -> String.valueOf(num(r.get("BranchId")))).collect(Collectors.joining(","));
        String yearStart = repo.yearStart(u, context.currentFinancialYearId());
        LocalDateTime now = LocalDateTime.now();
        int fy = context.currentFinancialYearId();
        // PendingDataDbCall(true): FromDate = Start_Period, ToDate = Now, no doc-no / customer / account filter, BranchesIds = every row.
        List<Map<String, Object>> raw = repo.pending(u, fy != 0 ? fy : null, date(yearStart), java.sql.Date.valueOf(now.toLocalDate()),
                null, null, null, null, all.isEmpty() ? null : all);
        out.put("rows", gridRows(raw));
        out.put("customers", customerRows(repo.customers(u)));
        out.put("yearStart", yearStart);
        out.put("now", STAMP.format(now));
        int amount = num(repo.configuration(u, "Default NoofDecimal Points For Amount").trim());
        out.put("amountDecimals", amount >= 1 && amount <= 4 ? amount : 2);
        return out;
    }

    /** btnRefresh_Click :302 - BranchesDbCall + BranchesBind (the user's branch is ticked again) + ComboBind(ComboDBCall()). */
    public Map<String, Object> refresh() {
        UserAccount u = user();
        Map<String, Object> out = branchesState(u);
        out.put("customers", customerRows(repo.customers(u)));
        return out;
    }

    // ================================================================================ search

    /** BtnShow_Click :316 -> PendingDataDbCall :329 + PendingDataGridBind :399. */
    public Map<String, Object> rows(Map<String, Object> body) {
        UserAccount u = user();
        String branchIds = branchIds(u, ints(body.get("branchIds")));
        int fy = context.currentFinancialYearId();
        java.sql.Date from = date(body.get("fromDate")), to = date(body.get("toDate"));
        int fromNo = toInt(body.get("fromDocNo")), toNo = toInt(body.get("toDocNo"));
        int customer = toInt(body.get("supplierCustomerId"));
        int action = toInt(body.get("actionId"));
        if (action < 0 || action > 2) throw new IllegalArgumentException("Unknown account filter");
        List<Map<String, Object>> raw = repo.pending(u, fy != 0 ? fy : null, from, to,
                fromNo != 0 ? fromNo : null, toNo != 0 ? toNo : null, customer != 0 ? customer : null,
                action != 0 ? action : null, branchIds);
        return Map.of("rows", gridRows(raw));
    }

    private static List<Map<String, Object>> gridRows(List<Map<String, Object>> raw) {
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> row = new LinkedHashMap<>();
            for (String[] c : GRID) row.put(c[0], plain(r.get(c[1])));
            rows.add(row);
        }
        return rows;
    }

    // ================================================================================ upload

    /**
     * BtnUploadInvoices_Click :525. The page checks "Check any row first" and asks "Are you sure to Save?" first; this runs
     * the loop over the distinct checked invoice ids in grid order (:537). For each: DocNo of its first checked row (:539),
     * GetByID (:542), the detail check (:547, "... is not found in database"), the IsUploaded check (:549), the mapping (:553,
     * including the TaxCompanySetup read and - when the invoice has no tax account - the tax server's schedule read),
     * FreightAmount = 0 / TransporterId = 0 (:554-555), then the tax-server Save (:556) - which cannot run from the web, so it
     * refuses there and UpdateInvoiceUploadStatus (:557) is never reached.
     */
    public Map<String, Object> upload(Map<String, Object> body) {
        UserAccount u = user();
        List<Map<String, Object>> checked = new ArrayList<>();
        if (body.get("rows") instanceof Collection<?> c)
            for (Object o : c) if (o instanceof Map<?, ?> m) { Map<String, Object> x = new HashMap<>(); m.forEach((k, v) -> x.put(String.valueOf(k), v)); checked.add(x); }
        if (checked.isEmpty()) throw new IllegalArgumentException("Check any row first");

        LinkedHashSet<Integer> ids = new LinkedHashSet<>();
        for (Map<String, Object> r : checked) ids.add(toInt(r.get("Id")));
        for (int invoiceId : ids) {
            int docNo = checked.stream().filter(r -> toInt(r.get("Id")) == invoiceId).findFirst().map(r -> toInt(r.get("DocNo"))).orElse(0);
            List<Map<String, Object>> headers = repo.header(invoiceId);
            // GetByID returns GetData(...)[0]: no row -> the List indexer's exception, whose text the BLL rethrows.
            if (headers.isEmpty())
                throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
            Map<String, Object> h = headers.get(0);
            if (num(h.get("OrganizationId")) != num(u.getOrganizationId()) || num(h.get("CompanyId")) != num(u.getCompanyId()))
                throw new AccessDeniedException("The selected invoice does not belong to this company");
            int documentTypeId = num(h.get("DocumentTypeId"));
            if (!DOCUMENT_TYPES.contains(documentTypeId))
                throw new AccessDeniedException("The selected document is not a sale invoice pending for upload");
            List<Map<String, Object>> details = repo.details(invoiceId, documentTypeId);
            if (details.isEmpty())
                throw new IllegalArgumentException("Selected Invoice : " + docNo + " is not found in database");
            if (bool(h.get("IsUploaded")))
                throw new IllegalArgumentException("Selected Invoice : " + docNo + " is already uploaded");
            List<Map<String, Object>> setup = repo.taxCompanySetup(num(h.get("OrganizationId")), num(h.get("CompanyId")));
            SaleInvoiceTaxMapping.requireTaxSetup(setup);
            if (SaleInvoiceTaxMapping.needsTaxServerSchedule(h))
                throw new IllegalArgumentException("Error converting sale invoice: " + TaxServerUploadTarget.notConfigured("Selected Invoice : " + docNo));
            SaleInvoiceTaxMapping.TaxSaleInvoice mapped = SaleInvoiceTaxMapping.convert(h, details,
                    repo.children(invoiceId, "InvSaleInvoicePaymentTerm_ReadBySaleInvoiceID"), setup);
            mapped.header().put("FreightAmount", 0.0);
            mapped.header().put("TransporterId", 0);
            // :556 BLL.TaxProject InvSaleInvoice.Save(obj2) - the tax server database; refuses here, and :557 is never reached.
            throw new IllegalArgumentException(TaxServerUploadTarget.notConfigured("Selected Invoice : " + docNo));
        }
        // Every pass ends at the tax-server step above, which refuses; the status write-back (:557) is never reached.
        throw new IllegalStateException(TaxServerUploadTarget.notConfigured("The selected invoices"));
    }

    // ================================================================================ attachments (NoOfAttachments link)

    /** grd_LinkClicked :514 - CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId); other tenants dropped. */
    public List<Map<String, Object>> attachments(int id, int documentTypeId) {
        UserAccount u = user();
        List<Map<String, Object>> out = new ArrayList<>();
        if (id <= 0 || documentTypeId <= 0) return out;
        for (var r : repo.attachments(id, documentTypeId)) {
            if (num(r.get("OrganizationId")) != num(u.getOrganizationId()) || num(r.get("CompanyId")) != num(u.getCompanyId())) continue;
            Map<String, Object> a = new LinkedHashMap<>();
            a.put("Id", num(r.get("Id")));
            a.put("AttachmentName", str(r.get("Attachment")));
            a.put("CustomName", str(r.get("UploadedFileCustomName")));
            a.put("EntryDate", plain(r.get("EntryDate")));
            out.add(a);
        }
        return out;
    }

    public record Download(String name, byte[] bytes) { }

    public Download attachment(int id, int documentTypeId, int attachmentId) {
        UserAccount u = user();
        for (var a : attachments(id, documentTypeId)) {
            if (num(a.get("Id")) != attachmentId) continue;
            String stored = basename(str(a.get("CustomName")).isBlank() ? str(a.get("AttachmentName")) : str(a.get("CustomName")));
            String name = str(a.get("AttachmentName")).isBlank() ? stored : basename(str(a.get("AttachmentName")));
            return new Download(name, attachmentStore.read(u, stored));
        }
        throw new IllegalArgumentException("Attachment not found");
    }

    // ================================================================================ helpers

    /** BranchesDbCall :162 + BranchesBind :187. */
    private Map<String, Object> branchesState(UserAccount u) {
        // GlobalVariables_Helper.GetConfigValueFromGlobal("SaleInvoiceBranchWise") through Conversion.ToBool.
        boolean branchImplemented = toBool(repo.configuration(u, "SaleInvoiceBranchWise"));
        int current = num(u.getBranchesId());
        List<Map<String, Object>> branches = new ArrayList<>();
        if (branchImplemented) {
            branches.add(branch(current, repo.branchName(u, current)));        // dtBranch.Rows.Add(BranchesId, BranchName)
        } else {
            for (var r : repo.branchesAllocated(u)) branches.add(branch(num(r.get("BranchId")), str(r.get("BranchName"))));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("branchImplemented", branchImplemented);
        out.put("branches", branches);
        // BranchesBind :210 - CmbBranch.Text = UserAccount.BranchName (ticks the user's branch); :211-220 fall back to the
        // first row when that leaves the combo empty.
        int ticked = branches.stream().anyMatch(r -> num(r.get("BranchId")) == current) ? current
                : branches.isEmpty() ? 0 : num(branches.get(0).get("BranchId"));
        out.put("branchId", ticked);
        return out;
    }

    private static Map<String, Object> branch(int id, String name) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("BranchId", id);
        m.put("BranchName", name);
        return m;
    }

    /**
     * The ticked branches as PendingDataDbCall builds them: string.Join(",", ids) (:352). Nothing ticked -> "Select branch
     * first" (:337-341). Every id must be in the form's own branch list.
     */
    private String branchIds(UserAccount u, List<Integer> ids) {
        if (ids == null || ids.isEmpty()) throw new IllegalArgumentException("Select branch first");
        @SuppressWarnings("unchecked")
        List<Map<String, Object>> allowedRows = (List<Map<String, Object>>) branchesState(u).get("branches");
        Set<Integer> allowed = allowedRows.stream().map(r -> num(r.get("BranchId"))).collect(Collectors.toSet());
        if (ids.stream().anyMatch(id -> id == null || !allowed.contains(id)))
            throw new AccessDeniedException("The selected branch is not allocated to this user");
        return ids.stream().distinct().map(String::valueOf).collect(Collectors.joining(","));
    }

    /** ComboBind :258 - value member Id, display member ReferenceName (Activity "Supplier" rows only). */
    private static List<Map<String, Object>> customerRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (var r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", num(r.get("Id")));
            m.put("ReferenceName", str(r.get("ReferenceName")));
            out.add(m);
        }
        return out;
    }

    private static java.sql.Date date(Object v) {
        String s = v == null ? "" : String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.length() > 10 ? s.substring(0, 10) : s)); }
        catch (RuntimeException e) { throw new IllegalArgumentException("Invalid date: " + s); }
    }

    private static List<Integer> ints(Object value) {
        List<Integer> out = new ArrayList<>();
        if (value instanceof Collection<?> c) for (Object o : c) { int n = toInt(o); if (n != 0) out.add(n); }
        return out;
    }

    private static String basename(String name) {
        String v = name == null ? "" : name.replace('\\', '/');
        v = v.substring(v.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(v);
        return v;
    }

    /** Conversion.ToBool: Convert.ToBoolean(text), else Convert.ToBoolean(Convert.ToInt32(text)), else false. */
    private static boolean toBool(String v) {
        String s = v == null ? "" : v.trim();
        if (s.equalsIgnoreCase("true")) return true;
        if (s.equalsIgnoreCase("false") || s.isEmpty()) return false;
        try { return Integer.parseInt(s) != 0; } catch (NumberFormatException e) { return false; }
    }

    private static boolean bool(Object v) {
        if (v instanceof Boolean b) return b;
        if (v instanceof Number n) return n.intValue() != 0;
        return v != null && toBool(String.valueOf(v));
    }

    /** Conversion.ToInt: blank / non-numeric -> 0. */
    private static int toInt(Object v) {
        if (v instanceof Number n) return n.intValue();
        String s = v == null ? "" : String.valueOf(v).trim();
        if (s.isEmpty()) return 0;
        try { return (int) Math.rint(Double.parseDouble(s)); } catch (NumberFormatException e) { return 0; }
    }

    private static int num(Object v) { return toInt(v); }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    private static Object plain(Object v) {
        if (v instanceof java.sql.Timestamp t) return STAMP.format(t.toLocalDateTime());
        if (v instanceof java.sql.Date d) return d.toLocalDate().toString() + "T00:00:00";
        if (v instanceof LocalDateTime t) return STAMP.format(t);
        return v;
    }
}
