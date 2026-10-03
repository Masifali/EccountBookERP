package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.PurchaseInvoiceForUploadRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import com.mst.services.datasync.PurchaseInvoiceTaxMapping;
import com.mst.services.datasync.TaxServerUploadTarget;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.*;
import java.util.stream.Collectors;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

/**
 * 910 "Purchase Invoice For Upload" - Architecture.WinApp.DataSyncing.frmPendingPurchaseInvoiceForUpload (Supplier
 * Purchases, module 5). ":NNN" = line in frmPendingPurchaseInvoiceForUpload.cs.
 *
 * The form lists the local purchase invoices (types 56, 57, 138, 245, 702) that are not uploaded yet
 * (USP_PurchaseInvoicePendingForUpload) and "uploads" the checked ones: each is re-read (GetByID), mapped to a
 * Purchase Invoice Direct (57) of the tax company (PurchaseInvoiceMapping), saved to the TAX SERVER database
 * (BLL.TaxProject InvPurchaseInvoice.Save) and only then marked uploaded locally (USP_InvPurchaseInvoice_UpdateUploadStatus).
 * The tax server connection is not known (see TaxServerUploadTarget), so the upload runs every local step up to the
 * tax-server save and stops there with that reason; nothing is written.
 *
 * Rights: the form checks none of its own; opening it needs the screen's View grant, which every call here requires.
 * Tenancy (organization / company / branch / financial year / user) always comes from the session.
 */
@Service
public class PurchaseInvoiceForUploadService {
    public static final int SCREEN_ID = 910;
    private static final Set<Integer> DOCUMENT_TYPES = Set.of(56, 57, 138, 245, 702);
    private static final DateTimeFormatter STAMP = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss");

    /** dtGrid :405-441 - column name -> USP_PurchaseInvoicePendingForUpload column it is filled from (:446). */
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

    private final PurchaseInvoiceForUploadRepository repo;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    private final TaxServerUploadTarget taxServer;
    private final DesktopAttachmentStore attachmentStore;

    public PurchaseInvoiceForUploadService(PurchaseInvoiceForUploadRepository repo, CurrentUserContext context, DesktopReportRights rights,
                                           TaxServerUploadTarget taxServer, DesktopAttachmentStore attachmentStore) {
        this.repo = repo; this.context = context; this.rights = rights; this.taxServer = taxServer; this.attachmentStore = attachmentStore;
    }

    public UserAccount user() {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
        return u;
    }

    // ================================================================================ load / refresh

    /**
     * InitializeComponentCustom :118 (FromDate = ActiveYr.Start_Period, Todate = Now) and InitializeComponentMethod :137:
     * BranchImplemented = PurchaseInvoiceBranchWise, BranchesDbCall, ComboDBCall(useBranchDt: true) - the first party
     * list is for EVERY branch in the list, not only the ticked one - then BranchesBind / ComboBind.
     */
    public Map<String, Object> init() {
        UserAccount u = user();
        Map<String, Object> out = branchesState(u);
        @SuppressWarnings("unchecked")
        List<Map<String, Object>> branches = (List<Map<String, Object>>) out.get("branches");
        String all = branches.stream().map(r -> String.valueOf(num(r.get("BranchId")))).collect(Collectors.joining(","));
        out.put("suppliers", supplierRows(repo.suppliers(u, all)));
        out.put("yearStart", repo.yearStart(u, context.currentFinancialYearId()));
        out.put("now", STAMP.format(LocalDateTime.now()));
        int amount = num(repo.configuration(u, "Default NoofDecimal Points For Amount").trim());
        out.put("amountDecimals", amount >= 1 && amount <= 4 ? amount : 2);
        return out;
    }

    /** btnRefresh_Click :302 - BranchesDbCall + BranchesBind (the user's branch is ticked again); the page then reloads the parties. */
    public Map<String, Object> refresh() {
        return branchesState(user());
    }

    /** ComboDBCall() :229 with the ticked branches (CmbBranch_Leave :707, btnRefresh_Click :308). */
    public List<Map<String, Object>> suppliers(List<Integer> branchIds) {
        UserAccount u = user();
        return supplierRows(repo.suppliers(u, branchIds(u, branchIds)));
    }

    // ================================================================================ search

    /** BtnShow_Click :316 -> PendingDataDbCall :329 + PendingDataGridBind :399. */
    public Map<String, Object> rows(Map<String, Object> body) {
        UserAccount u = user();
        String branchIds = branchIds(u, ints(body.get("branchIds")));
        int fy = context.currentFinancialYearId();
        java.sql.Date from = date(body.get("fromDate")), to = date(body.get("toDate"));
        int fromNo = toInt(body.get("fromDocNo")), toNo = toInt(body.get("toDocNo"));
        int supplier = toInt(body.get("supplierCustomerId"));
        int action = toInt(body.get("actionId"));
        if (action < 0 || action > 2) throw new IllegalArgumentException("Unknown account filter");
        List<Map<String, Object>> raw = repo.pending(u, fy != 0 ? fy : null, from, to,
                fromNo != 0 ? fromNo : null, toNo != 0 ? toNo : null, supplier != 0 ? supplier : null,
                action != 0 ? action : null, branchIds);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> row = new LinkedHashMap<>();
            for (String[] c : GRID) row.put(c[0], plain(r.get(c[1])));
            rows.add(row);
        }
        return Map.of("rows", rows);
    }

    // ================================================================================ upload

    /**
     * BtnUploadInvoices_Click :548. The page checks "Check any row first" and asks "Are you sure to Save?" first; this
     * runs the loop over the distinct checked invoice ids in grid order (:560). For each: GetByID (:566), the detail
     * check (:569-571, "... is not found in database"), the IsUploaded check (:572), the mapping (:576, including the
     * TaxCompanySetup read), FreightAmount = 0 / TransportAccountId = 0 (:577-578), then the tax-server Save (:579) -
     * which cannot run from the web, so it refuses there and UpdateInvoiceUploadStatus (:580) is never reached.
     */
    public Map<String, Object> upload(Map<String, Object> body) {
        UserAccount u = user();
        List<Map<String, Object>> checked = new ArrayList<>();
        if (body.get("rows") instanceof Collection<?> c)
            for (Object o : c) if (o instanceof Map<?, ?> m) { Map<String, Object> x = new HashMap<>(); m.forEach((k, v) -> x.put(String.valueOf(k), v)); checked.add(x); }
        if (checked.isEmpty()) throw new IllegalArgumentException("Check any row first");

        LinkedHashSet<Integer> ids = new LinkedHashSet<>();
        for (Map<String, Object> r : checked) ids.add(toInt(r.get("Id")));
        int uploaded = 0;
        for (int invoiceId : ids) {
            // :563-564 - DocNo of the first checked row of this invoice whose DocumentTypeId is not 1104 (0 when none).
            int docNo = checked.stream().filter(r -> toInt(r.get("Id")) == invoiceId && toInt(r.get("DocumentTypeId")) != 1104)
                    .findFirst().map(r -> toInt(r.get("DocNo"))).orElse(0);
            List<Map<String, Object>> headers = repo.header(invoiceId);
            // GetByID returns GetDate(...)[0]: no row -> the List indexer's exception, whose text the BLL rethrows.
            if (headers.isEmpty())
                throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
            Map<String, Object> h = headers.get(0);
            if (num(h.get("OrganizationId")) != num(u.getOrganizationId()) || num(h.get("CompanyId")) != num(u.getCompanyId()))
                throw new AccessDeniedException("The selected invoice does not belong to this company");
            int documentTypeId = num(h.get("DocumentTypeId"));
            if (!DOCUMENT_TYPES.contains(documentTypeId))
                throw new AccessDeniedException("The selected document is not a purchase invoice pending for upload");
            List<Map<String, Object>> details = repo.details(invoiceId, documentTypeId);
            if (details != null && details.isEmpty())
                throw new IllegalArgumentException("Selected Invoice : " + docNo + " is not found in database");
            if (bool(h.get("IsUploaded")))
                throw new IllegalArgumentException("Selected Invoice : " + docNo + " is already uploaded");
            PurchaseInvoiceTaxMapping.TaxPurchaseInvoice mapped = PurchaseInvoiceTaxMapping.convert(h, details, repo.emptyBags(invoiceId),
                    repo.taxCompanySetup(num(h.get("OrganizationId")), num(h.get("CompanyId"))), 0, 0.0);
            mapped.header().put("FreightAmount", 0.0);
            mapped.header().put("TransportAccountId", 0);
            taxServer.savePurchaseInvoice(mapped, docNo);                 // :579 - refuses (tax server not configured)
            repo.updateUploadStatus(u.getId(), invoiceId, true);           // :580 - reached only after a successful Save
            uploaded++;
        }
        return Map.of("message", "Record Uploaded Successfully", "uploaded", uploaded);
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
        // GlobalVariables_Helper.GetConfigValueFromGlobal("PurchaseInvoiceBranchWise") through Conversion.ToBool.
        boolean branchImplemented = toBool(repo.configuration(u, "PurchaseInvoiceBranchWise"));
        int current = num(u.getBranchesId());
        String currentName = repo.branchName(u, current);
        List<Map<String, Object>> branches = new ArrayList<>();
        if (branchImplemented) {
            branches.add(branch(current, currentName));                        // dtBranch.Rows.Add(BranchesId, BranchName)
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
     * The ticked branches as ComboDBCall / PendingDataDbCall build them: string.Join(",", ids) (:266, :375). Nothing
     * ticked -> "Select branch first" (:251-255, :360-364). Every id must be in the form's own branch list.
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

    /** ComboBind :279 - value member Id, display member ReferenceName (Activity "Supplier" rows only). */
    private static List<Map<String, Object>> supplierRows(List<Map<String, Object>> rows) {
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
