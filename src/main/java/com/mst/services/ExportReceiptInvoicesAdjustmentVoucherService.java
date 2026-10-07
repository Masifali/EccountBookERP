package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import com.mst.services.desktopvoucher.DesktopVoucherSupport;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Collection;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

/**
 * 916 "Fcy Receipt Adjustment Voucher" - Architecture.WinApp.Account_Definition.AdjustmentVouchers.
 * frmExportReceiptInvoicesAdjustmentVoucher (form Name / ScreenName "frmExportReceiptInvoicesAdjustmentVoucher",
 * TransactionTypeId 3). ":NNN" = line in frmExportReceiptInvoicesAdjustmentVoucher.cs.
 *
 * Every read is the procedure the desktop's BLL VoucherInvoicesAdjustment (BLL 0653) calls, parameter for parameter:
 *   Vouchers_OutstandingFcyReceiptsForAdjustment  USP_Vouchers_OutstandingFcyReceiptsForAdjustment (org, company, AccountId / FcyId only when != 0)
 *   ExportInvoice_GetDataForAdjustment            USP_ExportInvoice_GetDataForAdjustment (org, company, SupplierCustomerId, FcyId)
 *   GetByVoucherHeadId                            USP_VoucherInvoicesAdjustment_ReadByVoucherHeadId (TransactionTypeId, VoucherHeadId, PaymentTypeId, SupplierCustomerId, PartyGlId)
 *   PartyDropDown                                 USP_VoucherInvoicesAdjustment_PartyDropDown (TransactionTypeId)
 *   FormHistory                                   USP_VoucherInvoicesAdjustment_FormHistory (guarded parameters, as the BLL)
 *
 * Save / Delete: the form's Insert() / btnDelete_Click validate and build the row list (:1180-1330), then
 * VoucherInvoicesAdjustment.Save (BLL 0653 :17). For TransactionTypeId 3 that Save re-reads the FCY bank receipt behind the voucher
 * (ExImFCBankReceipts.GetByID) and REBUILDS the receipt's accounting voucher through ExImFCBankReceipts.MakeVoucher(receipt, adjustments,
 * totalInvoiceAmount) (BLL 0464 :111-1292, with the per-invoice exchange-rate gain / loss lines), then DAL 0585 SetData deletes the
 * receipt's payment-term details (USP_FcyBankReceiptPaymentTermDetail_DeleteByHeaderId), runs USP_VoucherInvoicesAdjustment_InsertUpdateDelete
 * once per row and rewrites the receipt's VoucherHead / VoucherDetail / _H copies and USP_VoucherBalanceCheck - in one transaction.
 * That MakeVoucher adjustment path is not part of the web application (the Fcy Receipts port does not carry it either), and writing the
 * adjustment rows without it would leave the receipt voucher and the adjustments disagreeing, so this service runs every desktop
 * validation, in the desktop's order and with its texts, and then refuses with that reason; nothing is written.
 *
 * Rights: DesktopVoucherSupport.rights(ScreenName) - Save / Update / Delete / Print as Rightsobjects. Tenancy always from the session.
 */
@Service
public class ExportReceiptInvoicesAdjustmentVoucherService {
    public static final String SCREEN_NAME = "frmExportReceiptInvoicesAdjustmentVoucher";
    public static final int TRANSACTION_TYPE_ID = 3;
    private static final DateTimeFormatter STAMP = DateTimeFormatter.ofPattern("yyyy-MM-dd'T'HH:mm:ss");

    private final JdbcTemplate jdbc;
    private final CurrentUserContext context;
    private final DesktopVoucherSupport support;
    private final DesktopAttachmentStore attachmentStore;

    public ExportReceiptInvoicesAdjustmentVoucherService(JdbcTemplate jdbc, CurrentUserContext context, DesktopVoucherSupport support,
                                                         DesktopAttachmentStore attachmentStore) {
        this.jdbc = jdbc; this.context = context; this.support = support; this.attachmentStore = attachmentStore;
    }

    private UserAccount user() { return context.requireAccountingUser(); }

    // ================================================================================ load

    /**
     * InitializeComponentCustom :104 + InitializeComponentMethod :146: Rightsobjects of the screen, DefaultDaysToLessFromHistoryFromDate,
     * the unfiltered pending list (Vouchers_OutstandingFcyReceiptsForAdjustment(org, company, 0, 0)), the active year's start (DateType 5).
     */
    public Map<String, Object> load() {
        UserAccount u = user();
        Map<String, Boolean> r = support.rights(SCREEN_NAME);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("canSave", r.get("save")); out.put("canUpdate", r.get("update")); out.put("canDelete", r.get("delete"));
        out.put("canPrint", r.get("print")); out.put("canViewAll", r.get("canViewAllRecord"));
        int days = toInt(support.config("DefaultDaysToLessFromHistoryFromDate"));
        out.put("defaultDays", days);
        out.put("now", STAMP.format(LocalDateTime.now()));
        out.put("yearStart", yearStart(u, context.currentFinancialYearId()));
        out.put("pending", pending());
        return out;
    }

    /** Vouchers_OutstandingFcyReceiptsForAdjustment(org, company, 0, 0) - Reset() :1126 and the first load :166. */
    public List<Map<String, Object>> pending() {
        UserAccount u = user();
        List<Map<String, Object>> raw = DesktopProc.rows(jdbc, "[dbo].[USP_Vouchers_OutstandingFcyReceiptsForAdjustment]",
                DesktopProc.params("OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
        return plain(raw);
    }

    /** LoadInvoiceData :738 - ExportInvoice_GetDataForAdjustment(org, company, supplierId, FcyId); the four parameters are always sent. */
    public List<Map<String, Object>> invoices(int supplierCustomerId, int fcyId) {
        UserAccount u = user();
        return plain(DesktopProc.rows(jdbc, "[dbo].[USP_ExportInvoice_GetDataForAdjustment]", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "SupplierCustomerId", supplierCustomerId, "FcyId", fcyId)));
    }

    /** ReadById :1280 / GenerateSlip :1744 - VoucherInvoicesAdjustment.GetByVoucherHeadId(TransactionTypeId 3, ...). */
    public List<Map<String, Object>> byVoucherHead(int voucherHeadId, int paymentTypeId, int supplierCustomerId, int partyGlId) {
        requireOwnVoucher(voucherHeadId);
        return plain(DesktopProc.rows(jdbc, "[dbo].[USP_VoucherInvoicesAdjustment_ReadByVoucherHeadId]", DesktopProc.params(
                "TransactionTypeId", TRANSACTION_TYPE_ID, "VoucherHeadId", voucherHeadId, "PaymentTypeId", paymentTypeId,
                "SupplierCustomerId", supplierCustomerId, "PartyGlId", partyGlId)));
    }

    // ================================================================================ history

    /** HistoryComboBind :1343 - VoucherInvoicesAdjustment.PartyDropDown(TransactionTypeId). */
    public List<Map<String, Object>> historyParties() {
        user();
        return plain(DesktopProc.rows(jdbc, "[dbo].[USP_VoucherInvoicesAdjustment_PartyDropDown]",
                DesktopProc.params("TransactionTypeId", TRANSACTION_TYPE_ID)));
    }

    /**
     * FillHistory :1429 -> VoucherInvoicesAdjustment.FormHistory (BLL 0653 :60). Parameters as the BLL builds them: TransactionTypeId always;
     * the dates the selected radio chooses (Voucher / Entry / Modify) when their check boxes are ticked; VoucherNoFrom / VoucherNoTo when != 0;
     * SupplierCustomerId + PartyGlId when both != 0. (PI.CanViewAllRecord / EntryUser are set by the form but the BLL never sends them.)
     * Rows are reshaped as :1491-1511 do: AdjustmentAmount = AdvanceAmount for PaymentTypeId 1, account / invoice columns renamed.
     */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        user();
        String dateType = str(b.get("dateType"));
        java.sql.Date from = date(b.get("fromDate")), to = date(b.get("toDate"));
        int fromNo = toInt(b.get("fromNo")), toNo = toInt(b.get("toNo"));
        int sc = toInt(b.get("supplierCustomerId")), gl = toInt(b.get("glAccountId"));
        Map<String, Object> p = DesktopProc.params("TransactionTypeId", TRANSACTION_TYPE_ID);
        if ("entry".equals(dateType)) { p.put("EntryFromDate", from); p.put("EntryToDate", to); }
        else if ("modify".equals(dateType)) { p.put("ModifyFromDate", from); p.put("ModifyToDate", to); }
        else { p.put("VoucherDateFrom", from); p.put("VoucherDateTo", to); }
        if (fromNo != 0) p.put("VoucherNoFrom", fromNo);
        if (toNo != 0) p.put("VoucherNoTo", toNo);
        if (sc != 0 && gl != 0) { p.put("SupplierCustomerId", sc); p.put("PartyGlId", gl); }
        List<Map<String, Object>> raw = DesktopProc.rows(jdbc, "[dbo].[USP_VoucherInvoicesAdjustment_FormHistory]", p);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : raw) {
            Object adj = toInt(r.get("PaymentTypeId")) == 1 ? r.get("AdvanceAmount") : r.get("AdjustmentAmount");
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("TransactionTypeId", plain(r.get("TransactionTypeId")));
            m.put("VoucherHeadId", plain(r.get("VoucherHeadId")));
            m.put("VoucherCode", plain(r.get("VoucherCode")));
            m.put("VoucherDate", plain(r.get("VoucherDate")));
            m.put("VoucherAmount", plain(r.get("VoucherAmount")));
            m.put("PartyGlId", plain(r.get("PartyGlId")));
            m.put("AccountCode", plain(r.get("PartyGlAccountCode")));
            m.put("AccountTitle", plain(r.get("PartyGlAccountTitle")));
            m.put("PaymentTypeId", plain(r.get("PaymentTypeId")));
            m.put("PaymentType", plain(r.get("PaymentType")));
            m.put("RefDocumentTypeId", plain(r.get("RefDocumentTypeId")));
            m.put("RefDocumentType", plain(r.get("RefDocumentType")));
            m.put("InvoiceNos", plain(r.get("RefDocNos")));
            m.put("InvoiceDates", plain(r.get("RefDocDates")));
            m.put("AdjustmentAmount", plain(adj));
            m.put("SupplierCustomerId", plain(r.get("SupplierCustomerId")));
            m.put("PartyName", plain(r.get("PartyName")));
            m.put("EntryDate", plain(r.get("EntryDate")));
            m.put("EntryUserName", plain(r.get("EntryUserName")));
            m.put("ModifyDate", plain(r.get("ModifyDate")));
            m.put("ModifyUserName", plain(r.get("ModifyUserName")));
            m.put("VoucherRemarks", str(r.get("Remarks")));
            out.add(m);
        }
        return out;
    }

    // ================================================================================ save / delete

    /**
     * Insert() :1163 for btnSave_Click :1304 (RecId 0) and btnUpdate_Click :1465 (RecId > 0). The page asks "Are you sure to Save?" /
     * "Are you sure to Update?" first (skipped for the auto-utilize loop). Every check below is the desktop's, in its order.
     */
    public Map<String, Object> save(Map<String, Object> b) {
        UserAccount u = user();
        int recId = toInt(b.get("recId"));
        Map<String, Boolean> r = support.rights(SCREEN_NAME);
        if (recId > 0) {
            // btnUpdate_Click :1465
            if (!Boolean.TRUE.equals(r.get("update"))) throw new AccessDeniedException("The user does not have Update rights for this screen");
            if (Boolean.TRUE.equals(b.get("approved"))) throw new IllegalArgumentException("Record Not Update because Record has approved");
        } else if (!Boolean.TRUE.equals(r.get("save"))) {
            throw new AccessDeniedException("The user does not have Save rights for this screen");
        }
        List<Map<String, Object>> lines = maps(b.get("lines"));
        if (lines.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        // FormValidation :1070
        int voucherHeadId = toInt(b.get("voucherHeadId"));
        if (voucherHeadId == 0) throw new IllegalArgumentException("Voucher No Field is Required");
        String amountText = str(b.get("voucherAmount")).trim();
        if (amountText.isEmpty() || "0".equals(amountText)) throw new IllegalArgumentException("VoucherAmount Field is Required");
        int paymentTypeId = toInt(b.get("paymentTypeId"));
        if (paymentTypeId == 0) throw new IllegalArgumentException("Payment Type Field is Required");
        int supplierCustomerId = toInt(b.get("supplierCustomerId"));
        if (supplierCustomerId == 0) throw new IllegalArgumentException("Party Field is Required");
        requireOwnVoucher(voucherHeadId);

        BigDecimal total = BigDecimal.ZERO;
        int count = 0, rowNo = 0;
        for (Map<String, Object> l : lines) {
            rowNo++;
            if (toInt(l.get("Id")) > 0 || dec(l.get("AdjustmentAmount")).signum() > 0) {
                // :1214-1232 - the row's validations (RowIndex + 1 is the grid row number).
                if (toInt(l.get("RefDocumentTypeId")) == 0) throw new IllegalArgumentException("RefDocumentType Required in Detail Grid And row No: " + rowNo);
                if (toInt(l.get("ExportVoucherId")) == 0) throw new IllegalArgumentException("RefDocNo Required in Detail Grid And row No: " + rowNo);
                if (dec(l.get("InvoiceAmount")).signum() == 0) throw new IllegalArgumentException("InvoiceAmount Required in Detail Grid And row No: " + rowNo);
                // detail.AdvanceAmount is always 0m on this form, so PaymentTypeId 1 rows fail here, exactly as on the desktop.
                if (paymentTypeId == 1) throw new IllegalArgumentException("AdjustmentAmount Required in Detail Grid And row No: " + rowNo);
                if (dec(l.get("AdjustmentAmount")).signum() == 0) throw new IllegalArgumentException("AdjustmentAmount Required in Detail Grid And row No: " + rowNo);
                total = total.add(dec(l.get("AdjustmentAmount")));
                count++;
            }
        }
        if (count == 0) throw new IllegalArgumentException("At least enter one row in detail ");
        BigDecimal voucherAmount = dec(amountText);
        BigDecimal t0 = total.setScale(0, RoundingMode.HALF_EVEN), v0 = voucherAmount.setScale(0, RoundingMode.HALF_EVEN);
        if (paymentTypeId == 1 && t0.compareTo(v0) > 0)
            throw new IllegalArgumentException("Total adjustment Amount " + t0.toPlainString() + " In Grid cannot be greater then to voucher amount " + v0.toPlainString());
        if (paymentTypeId != 1 && t0.compareTo(v0) != 0)
            throw new IllegalArgumentException("Total adjustment Amount " + t0.toPlainString() + " In Grid Should be equal to voucher amount " + v0.toPlainString());
        // VoucherInvoicesAdjustment.Save (BLL 0653 :17) - see the class comment.
        throw new IllegalArgumentException(regenerationRefusal("The adjustment of voucher " + str(b.get("voucherCode")), u));
    }

    /** btnDelete_Click :1389 - "Record Id not found for deletion..." then the same VoucherInvoicesAdjustment.Save (ActionTypeId 3 rows). */
    public Map<String, Object> delete(Map<String, Object> b) {
        UserAccount u = user();
        if (!Boolean.TRUE.equals(support.rights(SCREEN_NAME).get("delete")))
            throw new AccessDeniedException("The user does not have Delete rights for this screen");
        int recId = toInt(b.get("recId"));
        if (recId == 0) throw new IllegalArgumentException("Record Id not found for deletion...");
        requireOwnVoucher(toInt(b.get("voucherHeadId")));
        throw new IllegalArgumentException(regenerationRefusal("The deletion of the adjustment of voucher " + str(b.get("voucherCode")), u));
    }

    private static String regenerationRefusal(String what, UserAccount u) {
        return what + " was not saved. On the desktop this Save rebuilds the Fcy receipt's accounting voucher with the invoice-wise exchange-rate "
                + "gain / loss lines (ExImFCBankReceipts.MakeVoucher with the adjustments) in the same transaction as the adjustment rows. That "
                + "rebuild is not available in the web application, and the adjustment rows alone would not match the receipt voucher, so nothing was written.";
    }

    // ================================================================================ attachments (NoOfAttachments link)

    /** grdPendingOrders_LinkClicked :606 - CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId); other tenants dropped. */
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

    private static String basename(String name) {
        String v = name == null ? "" : name.replace('\\', '/');
        v = v.substring(v.lastIndexOf('/') + 1);
        DesktopAttachmentStore.validateName(v);
        return v;
    }

    // ================================================================================ helpers

    private void requireOwnVoucher(int voucherHeadId) {
        if (voucherHeadId <= 0) return;
        UserAccount u = user();
        List<Integer> hit = jdbc.queryForList("SELECT 1 FROM dbo.VoucherHead WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?",
                Integer.class, voucherHeadId, u.getOrganizationId(), u.getCompanyId());
        if (hit.isEmpty()) throw new AccessDeniedException("The selected voucher does not belong to this company");
    }

    private String yearStart(UserAccount u, int financialYearId) {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, "Proc_FinancialYear_ReadActiveByOrganizationIdNCompanyId", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId()));
        for (Map<String, Object> row : rows) {
            Object id = row.get("Id");
            if (id instanceof Number && ((Number) id).intValue() == financialYearId && row.get("Start_Period") != null)
                return String.valueOf(row.get("Start_Period")).substring(0, 10);
        }
        return "";
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> maps(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof Collection) for (Object o : (Collection<?>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    private static java.sql.Date date(Object v) {
        String s = v == null ? "" : String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        try { return java.sql.Date.valueOf(LocalDate.parse(s.length() > 10 ? s.substring(0, 10) : s)); }
        catch (RuntimeException e) { throw new IllegalArgumentException("Invalid date: " + s); }
    }

    /** Conversion.ToDecimal: blank / non-numeric -> 0. */
    private static BigDecimal dec(Object v) {
        if (v instanceof BigDecimal) return (BigDecimal) v;
        if (v instanceof Number) return new BigDecimal(String.valueOf(v));
        String s = v == null ? "" : String.valueOf(v).trim().replace(",", "");
        if (s.isEmpty()) return BigDecimal.ZERO;
        try { return new BigDecimal(s); } catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }

    /** Conversion.ToInt: blank / non-numeric -> 0. */
    private static int toInt(Object v) {
        if (v instanceof Number) return ((Number) v).intValue();
        String s = v == null ? "" : String.valueOf(v).trim();
        if (s.isEmpty()) return 0;
        try { return (int) Math.rint(Double.parseDouble(s)); } catch (NumberFormatException e) { return 0; }
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    private static List<Map<String, Object>> plain(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new HashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) m.put(e.getKey(), plain(e.getValue()));
            out.add(m);
        }
        return out;
    }

    private static Object plain(Object v) {
        if (v instanceof java.sql.Timestamp) return STAMP.format(((java.sql.Timestamp) v).toLocalDateTime());
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString() + "T00:00:00";
        if (v instanceof LocalDateTime) return STAMP.format((LocalDateTime) v);
        if (v instanceof BigDecimal) return ((BigDecimal) v).doubleValue();
        return v;
    }
}
