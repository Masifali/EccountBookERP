package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import com.mst.services.datasync.TaxServerUploadTarget;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Objects;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

/**
 * 917 "Export Voucher (Upload)" - Architecture.WinApp.DataSyncing.frmPendingExportVoucherForUpload (dbo.ScreenDefinition 917).
 * ":NNN" = line in frmPendingExportVoucherForUpload.cs.
 *
 * The form lists the local export vouchers (document type 27) that are not uploaded yet (USP_ExportVoucherPendingForUpload)
 * and "uploads" the checked ones: each is re-read (ExportVoucher.GetByIDForUpload), mapped to a tax-company export voucher
 * (ExportVoucherMapping.ConvertLocalObjectToTaxObject), saved to the TAX SERVER database (BLL.TaxProject.Export.ExportVoucher.Save)
 * and only then marked uploaded locally (USP_ExportVoucher_UpdateUploadStatus). The tax-server connection is not configured in
 * the web application (see TaxServerUploadTarget), so the upload runs every local step up to the tax-server save and stops there
 * with that reason; the upload status is never written.
 *
 * Rights: the form checks none of its own; every call here needs the screen's View grant. Tenancy always comes from the session.
 */
@Service
public class ExportVoucherForUploadService {
    public static final int SCREEN_ID = 917;
    private static final DateTimeFormatter STAMP = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss");

    /** dtGrid :257-292 - grid column -> USP_ExportVoucherPendingForUpload column it is filled from (:326). */
    private static final String[][] GRID = {
            {"DocumentTypeId", "DocumentTypeId"}, {"DocumentType", "DocumentType"}, {"Id", "Id"}, {"DocNo", "DocNo"},
            {"DocDate", "DocDate"}, {"SupplierCustomerId", "SupplierCustomerId"}, {"CustomerName", "CustomerName"},
            {"InvoiceNo", "InvoiceNo"}, {"CreditAccount", "CreditAccount"}, {"FcyCode", "FcyCode"},
            {"ExchangeRate", "ExchangeRate"}, {"FcyAmount", "FcyAmount"}, {"AddLessAmount", "AddLessAmount"},
            {"LcyAmount", "LcyAmount"}, {"ContractNo", "ContractNo"}, {"ItemName", "ItemName"},
            {"Warehouse", "WareHouseName"}, {"CropYear", "CropYear"}, {"JobLot", "JobLotDescription"},
            {"PackingType", "PackingType"}, {"ItemQty", "ItemQty"}, {"PackUom", "PackUomCode"},
            {"NetBillWeight", "NetBillWeight"}, {"ItemRate", "ItemRate"}, {"RateUom", "RateUomCode"},
            {"CostRate", "CostRate"}, {"FcyAmountDetail", "FcyAmountDetail"}, {"LcyAmountDetail", "LcyAmountDetail"},
            {"EntryDate", "EntryDate"}, {"EntryUser", "EntryUserName"}, {"ModifyDate", "ModifyDate"},
            {"ModifyUser", "ModifyUserName"}, {"ApprovalStatus", "IsApproved"}, {"ApprovedDate", "ApprovedDate"},
            {"ApprovedUser", "ApprovedUserName"}, {"NoOfAttachments", "NoOfAttachments"}};

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopReportRights rights;
    private final DesktopAttachmentStore attachmentStore;

    public ExportVoucherForUploadService(JdbcTemplate jdbc, CurrentUserContext context, DesktopReportRights rights,
                                         DesktopAttachmentStore attachmentStore) {
        this.jdbc = jdbc; this.context = context; this.rights = rights; this.attachmentStore = attachmentStore;
    }

    public UserAccount user() {
        UserAccount u = context.requireAccountingUser();
        rights.require(u, SCREEN_ID, "View");
        return u;
    }

    // ================================================================================ load / refresh

    /** InitializeComponentCustom :72 (FromDate = ActiveYr.Start_Period, Todate = Now) and InitializeComponentMethod :87 (ComboDBCall + ComboBind). */
    public Map<String, Object> init() {
        UserAccount u = user();
        Map<String, Object> out = new LinkedHashMap<>(combos(u));
        out.put("yearStart", yearStart(u, context.currentFinancialYearId()));
        out.put("now", STAMP.format(LocalDateTime.now()));
        int amount = toInt(configuration(u, "Default NoofDecimal Points For Amount").trim());
        out.put("amountDecimals", amount >= 1 && amount <= 4 ? amount : 2);
        return out;
    }

    /** btnRefresh_Click :145 - ComboBind(ComboDBCall()). */
    public Map<String, Object> refresh() { return combos(user()); }

    /**
     * ComboDBCall :106 - ExportVoucher.GetDataForDropDownFromExportInvoiceVoucher(org, company, DocumentTypeIds "27", NotUploaded 1) and
     * ComboBind :122 which splits the rows by ActivityType into Customer (Buyer Name), ContractNo (Contract #) and Currency (Currency).
     */
    private Map<String, Object> combos(UserAccount u) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "[dbo].[USP_GetDataForDropDownFromExportInvoiceVoucher]", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "NotUploaded", 1, "DocumentTypeIds", "27"));
        List<Map<String, Object>> buyers = new ArrayList<>(), contracts = new ArrayList<>(), currencies = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            String type = str(r.get("ActivityType"));
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", toInt(r.get("Id")));
            m.put("name", str(r.get("name")));
            if ("ContractNo".equals(type)) contracts.add(m);
            else if ("Customer".equals(type)) buyers.add(m);
            else if ("Currency".equals(type)) currencies.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("buyers", buyers);
        out.put("contracts", contracts);
        out.put("currencies", currencies);
        return out;
    }

    // ================================================================================ search

    /**
     * BtnShow_Click :152 -> PendingDataDbCall :163 + PendingDataGridBind :188. Guards as the BLL (ExportVoucherPendingForUpload):
     * FinancialYearId / DocNoFrom / DocNoTo / SupplierCustomerId / ContractId / CurrencyId only when != 0, both dates always set.
     */
    public Map<String, Object> rows(Map<String, Object> body) {
        UserAccount u = user();
        int fy = context.currentFinancialYearId();
        java.sql.Date from = date(body.get("fromDate")), to = date(body.get("toDate"));
        int fromNo = toInt(body.get("fromDocNo")), toNo = toInt(body.get("toDocNo"));
        int buyer = toInt(body.get("supplierCustomerId")), contract = toInt(body.get("contractId")), currency = toInt(body.get("currencyId"));
        Map<String, Object> p = DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "FinancialYearId", fy != 0 ? fy : null, "FromDate", from, "ToDate", to,
                "DocNoFrom", fromNo != 0 ? fromNo : null, "DocNoTo", toNo != 0 ? toNo : null,
                "SupplierCustomerId", buyer != 0 ? buyer : null, "ContractId", contract != 0 ? contract : null,
                "CurrencyId", currency != 0 ? currency : null);
        List<Map<String, Object>> raw = DesktopProc.rows(jdbc, "USP_ExportVoucherPendingForUpload", p);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Map<String, Object> row = new LinkedHashMap<>();
            for (String[] c : GRID) row.put(c[0], plain(r.get(c[1])));
            rows.add(row);
        }
        Map<String, Object> out = new HashMap<>();
        out.put("rows", rows);
        return out;
    }

    // ================================================================================ upload

    /**
     * BtnUploadInvoices_Click :264. The page checks "Check any row first" and asks "Are you sure to Save?" first; this runs the
     * loop over the distinct checked voucher ids in grid order (:272). For each: GetByIDForUpload (:278 - header, details, payment
     * terms, other charges, commission), the detail check (:281-283, "... is not found in database"), the IsUploaded check (:284),
     * the mapping's tax company setup read (ExportVoucherMapping.ConvertLocalObjectToTaxObject), then the tax-server Save (:288),
     * which cannot run from the web, so it refuses there and UpdateUploadStatus (:289) is never reached.
     */
    public Map<String, Object> upload(Map<String, Object> body) {
        UserAccount u = user();
        List<Map<String, Object>> checked = new ArrayList<>();
        Object list = body.get("rows");
        if (list instanceof Collection) {
            for (Object o : (Collection<?>) list) {
                if (o instanceof Map) {
                    Map<String, Object> x = new HashMap<>();
                    for (Map.Entry<?, ?> e : ((Map<?, ?>) o).entrySet()) x.put(String.valueOf(e.getKey()), e.getValue());
                    checked.add(x);
                }
            }
        }
        if (checked.isEmpty()) throw new IllegalArgumentException("Check any row first");

        LinkedHashSet<Integer> ids = new LinkedHashSet<>();
        for (Map<String, Object> r : checked) ids.add(toInt(r.get("Id")));
        int uploaded = 0;
        for (int voucherId : ids) {
            // :274-275 - DocNo of the first checked row of this voucher (0 when none).
            int docNo = 0;
            for (Map<String, Object> r : checked) if (toInt(r.get("Id")) == voucherId) { docNo = toInt(r.get("DocNo")); break; }
            List<Map<String, Object>> headers = DesktopProc.rows(jdbc, "Sp_ExportVoucher_GetAllMethod", DesktopProc.params("Id", voucherId, "Activity", "ReadById"));
            // GetByIDForUpload returns GetDataForUpload(...)[0]: no row -> the List indexer's exception text.
            if (headers.isEmpty())
                throw new IllegalArgumentException("Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index");
            Map<String, Object> h = headers.get(0);
            if (toInt(h.get("OrganizationId")) != toInt(u.getOrganizationId()) || toInt(h.get("CompanyId")) != toInt(u.getCompanyId()))
                throw new AccessDeniedException("The selected voucher does not belong to this company");
            // DAL 0508 GetDataForUpload :352 - the four child reads that follow the header read.
            List<Map<String, Object>> details = DesktopProc.rows(jdbc, "Sp_ExportVoucher_GetAllMethod", DesktopProc.params("Id", voucherId, "Activity", "ReadDetailByHeaderId"));
            int invoiceId = toInt(h.get("ExImInvoiceId"));
            DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", DesktopProc.params("Id", invoiceId, "Activity", "ReadExImInvoicePaymentTermsDetailByHeaderId"));
            DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", DesktopProc.params("Id", invoiceId, "Activity", "ExImInvoiceOtherChargesDetail_ReadByHeaderId"));
            DesktopProc.rows(jdbc, "Sp_ExImInvoice_GetAllMethod", DesktopProc.params("Id", invoiceId, "Activity", "EximInvoiceCommissionInfoByReadByHeaderId"));
            if (details.isEmpty())
                throw new IllegalArgumentException("Selected voucher : " + docNo + " is not found in database");
            if (bool(h.get("IsUploaded")))
                throw new IllegalArgumentException("Selected Voucher : " + docNo + " is already uploaded");
            // ExportVoucherMapping :47 - InvSaleInvoice.GetTaxCompanySetupByOrgAndCompany(Obj.OrganizationId, Obj.CompanyId).
            List<Map<String, Object>> setup = DesktopProc.rows(jdbc, "USP_GetTaxCompanySetupByOrgAndCompany",
                    DesktopProc.params("OrganizationId", toInt(h.get("OrganizationId")), "CompanyId", toInt(h.get("CompanyId"))));
            if (setup.isEmpty())
                throw new IllegalArgumentException("Error converting sale invoice: Tax company setup not found for the given company.");
            // :288 - BLL.TaxProject.Export.ExportVoucher.Save(mapped, org, company) writes to the tax server: refused, status not written.
            throw new IllegalArgumentException(TaxServerUploadTarget.notConfigured("Selected Voucher : " + docNo));
        }
        Map<String, Object> out = new HashMap<>();
        out.put("message", "Record Uploaded Successfully");
        out.put("uploaded", uploaded);
        return out;
    }

    // ================================================================================ attachments (NoOfAttachments link)

    /** grd_LinkClicked :218 - CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId); other tenants dropped. */
    public List<Map<String, Object>> attachments(int id, int documentTypeId) {
        UserAccount u = user();
        List<Map<String, Object>> out = new ArrayList<>();
        if (id <= 0 || documentTypeId <= 0) return out;
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_DMSAttachments_GetAllMethod", DesktopProc.params(
                "RefDocumentTypeId", documentTypeId, "Id", id, "Activity", "ReadAttachmentsbyRefDocumentTypeId"));
        for (Map<String, Object> r : rows) {
            if (toInt(r.get("OrganizationId")) != toInt(u.getOrganizationId()) || toInt(r.get("CompanyId")) != toInt(u.getCompanyId())) continue;
            Map<String, Object> a = new LinkedHashMap<>();
            a.put("Id", toInt(r.get("Id")));
            a.put("AttachmentName", str(r.get("Attachment")));
            a.put("CustomName", str(r.get("UploadedFileCustomName")));
            a.put("EntryDate", plain(r.get("EntryDate")));
            out.add(a);
        }
        return out;
    }

    public static final class Download {
        private final String name; private final byte[] bytes;
        Download(String name, byte[] bytes) { this.name = name; this.bytes = bytes; }
        public String name() { return name; }
        public byte[] bytes() { return bytes; }
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

    private String configuration(UserAccount u, String description) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Sp_ConfigrationsAllocation_GetAllMethod", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "ConfigDescription", description, "Activity", "GetConfigurationByOrgCompandConfigDescription"));
        return rows.isEmpty() ? "" : Objects.toString(rows.get(0).get("ConfigKey"), "");
    }

    /** clsGlobalVariables.ActiveYr.Start_Period of the signed-in financial year. */
    private String yearStart(UserAccount u, int financialYearId) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
        for (Map<String, Object> r : rows) {
            Object id = r.get("Id");
            if (id instanceof Number && ((Number) id).intValue() == financialYearId && r.get("Start_Period") != null)
                return String.valueOf(r.get("Start_Period")).substring(0, 10);
        }
        return "";
    }

    private static java.sql.Date date(Object v) {
        String s = v == null ? "" : String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.length() > 10 ? s.substring(0, 10) : s)); }
        catch (RuntimeException e) { throw new IllegalArgumentException("Invalid date: " + s); }
    }

    private static String basename(String name) {
        String v = name == null ? "" : name.replace('\\', '/');
        v = v.substring(v.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(v);
        return v;
    }

    private static boolean bool(Object v) {
        if (v instanceof Boolean) return (Boolean) v;
        if (v instanceof Number) return ((Number) v).intValue() != 0;
        String s = v == null ? "" : String.valueOf(v).trim();
        if (s.equalsIgnoreCase("true")) return true;
        try { return Integer.parseInt(s) != 0; } catch (NumberFormatException e) { return false; }
    }

    /** Conversion.ToInt: blank / non-numeric -> 0. */
    private static int toInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        String s = v == null ? "" : String.valueOf(v).trim();
        if (s.isEmpty()) return 0;
        try { return (int) Math.rint(Double.parseDouble(s)); } catch (NumberFormatException e) { return 0; }
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    private static Object plain(Object v) {
        if (v instanceof java.sql.Timestamp) return STAMP.format(((java.sql.Timestamp) v).toLocalDateTime());
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString() + "T00:00:00";
        if (v instanceof LocalDateTime) return STAMP.format((LocalDateTime) v);
        return v;
    }
}
