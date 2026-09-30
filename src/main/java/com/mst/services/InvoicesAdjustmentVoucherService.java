package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.models.dto.InvoicesAdjustmentVoucherDto;
import com.mst.repositories.AccountsGroupDSupport;
import com.mst.repositories.support.DesktopProc;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.AccountsGroupDSupport.toInt;

/**
 * Screens 861 "Invoices Adjustment Voucher" and 863 "Receipt Invoices Adjustment Voucher":
 *
 * <pre>
 *  kind "payment"  frmInvoicesAdjustmentVoucher         TransactionTypeId 1  invoices: USP_PurchaseInvoice_GetDataForAdjustment
 *  kind "receipt"  frmReceiptInvoicesAdjustmentVoucher  TransactionTypeId 2  invoices: USP_SaleInvoice_GetDataForAdjustment
 * </pre>
 *
 * Both forms are the same code apart from those two lines, the advance PaymentTypeId their
 * Insert() splits on (1 on 861, 2 on 863) and their titles/prints. Their Save, Update and Delete
 * all go through BLL 0653 VoucherInvoicesAdjustment.Save → DAL 0585 SetData, which calls
 * USP_VoucherInvoicesAdjustment_InsertUpdateDelete once per list item inside ONE transaction,
 * with GenericProvider.SetProc sending every non-virtual property of the model (model 1193) —
 * so ExchangeRate, PaymentTermId, FcyId and ExImInvoicePaymentTermsDetailId go as 0, never NULL,
 * and DocumentTypeId (not a model property) is never sent.
 *
 * Kept as the desktop has it:
 *  - 863's Insert() treats PaymentTypeId 2 as the advance (AdvanceAmount) but its total check,
 *    ReadById and history still read PaymentTypeId 1 as the advance.
 *  - The Detail grid has no Delete column, so a saved row cannot be removed on its own; only the
 *    toolbar Delete removes a voucher's rows (ActionTypeId 3 for every saved row).
 *  - History sends no user filter: VoucherInvoicesAdjustment.FormHistory never adds @EntryUser.
 */
@Service
public class InvoicesAdjustmentVoucherService {

    @Autowired private AccountsGroupDSupport s;

    public static class Refusal extends RuntimeException { public Refusal(String m) { super(m); } }

    static final class Kind {
        final int transactionTypeId; final String screen; final String invoiceProc; final int advanceTypeForSave;
        Kind(int t, String s, String p, int a) { transactionTypeId = t; screen = s; invoiceProc = p; advanceTypeForSave = a; }
    }

    static Kind kind(String kind) {
        if ("payment".equals(kind)) return new Kind(1, "frmInvoicesAdjustmentVoucher", "[dbo].[USP_PurchaseInvoice_GetDataForAdjustment]", 1);
        if ("receipt".equals(kind)) return new Kind(2, "frmReceiptInvoicesAdjustmentVoucher", "[dbo].[USP_SaleInvoice_GetDataForAdjustment]", 2);
        throw new Refusal("Unknown adjustment voucher kind");
    }

    // ================================================================================== load

    /** InitializeComponentMethod: rights, DefaultDaysToLessFromHistoryFromDate, feature 4, pending. */
    public Map<String, Object> init(String kindName) {
        Kind k = kind(kindName);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", s.rights(k.screen));
        out.put("defaultDaysToLessFromHistoryFromDate", toInt(s.config("DefaultDaysToLessFromHistoryFromDate")));
        out.put("subsidiaryFeature", s.feature(4));
        out.put("amountDecimals", toInt(s.config("Default NoofDecimal Points For Amount")));
        out.put("financialYearStart", s.yearStart());
        out.put("pending", pending(kindName));
        return out;
    }

    /** Vouchers_OutstandingForAdjustment(org, comp, TransactionTypeId, 0) — @AccountId never sent by these forms. */
    public List<Map<String, Object>> pending(String kindName) {
        Kind k = kind(kindName);
        UserAccount u = s.user();
        return DesktopProc.rows(s.jdbc(), "[dbo].[USP_Vouchers_OutstandingForAdjustment]", DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "TransTypeId", k.transactionTypeId));
    }

    /** LoadInvoiceData: Purchase/SaleInvoice_GetDataForAdjustment(org, comp, SupplierCustomerId) — three parameters. */
    public List<Map<String, Object>> invoices(String kindName, int supplierCustomerId) {
        Kind k = kind(kindName);
        UserAccount u = s.user();
        return DesktopProc.rows(s.jdbc(), k.invoiceProc, DesktopProc.params(
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "SupplierCustomerId", supplierCustomerId));
    }

    /** GetByVoucherHeadId — all five parameters always sent (ReadById and GenerateSlip). */
    public List<Map<String, Object>> byVoucherHead(String kindName, int voucherHeadId, int paymentTypeId,
                                                   int supplierCustomerId, int partyGlId) {
        Kind k = kind(kindName);
        return DesktopProc.rows(s.jdbc(), "[dbo].[USP_VoucherInvoicesAdjustment_ReadByVoucherHeadId]", DesktopProc.params(
                "TransactionTypeId", k.transactionTypeId, "VoucherHeadId", voucherHeadId, "PaymentTypeId", paymentTypeId,
                "SupplierCustomerId", supplierCustomerId, "PartyGlId", partyGlId));
    }

    /** HistoryComboBind → PartyDropDown(TransactionTypeId). */
    public List<Map<String, Object>> parties(String kindName) {
        Kind k = kind(kindName);
        return DesktopProc.rows(s.jdbc(), "[dbo].[USP_VoucherInvoicesAdjustment_PartyDropDown]",
                DesktopProc.params("TransactionTypeId", k.transactionTypeId));
    }

    /**
     * FillHistory → FormHistory: each date only when its box is ticked, under the chosen radio;
     * doc numbers when non-zero; the party pair only when both are non-zero.
     */
    public List<Map<String, Object>> history(String kindName, String dateType, String from, String to,
                                             int fromDocNo, int toDocNo, int supplierCustomerId, int glAccountId) {
        Kind k = kind(kindName);
        Map<String, Object> p = DesktopProc.params("TransactionTypeId", k.transactionTypeId);
        String fromKey, toKey;
        if ("entry".equals(dateType)) { fromKey = "EntryFromDate"; toKey = "EntryToDate"; }
        else if ("modify".equals(dateType)) { fromKey = "ModifyFromDate"; toKey = "ModifyToDate"; }
        else { fromKey = "VoucherDateFrom"; toKey = "VoucherDateTo"; }
        if (from != null && !from.trim().isEmpty()) p.put(fromKey, from.trim());
        if (to != null && !to.trim().isEmpty()) p.put(toKey, to.trim());
        if (fromDocNo != 0) p.put("VoucherNoFrom", fromDocNo);
        if (toDocNo != 0) p.put("VoucherNoTo", toDocNo);
        if (supplierCustomerId != 0 && glAccountId != 0) {
            p.put("SupplierCustomerId", supplierCustomerId);
            p.put("PartyGlId", glAccountId);
        }
        return DesktopProc.rows(s.jdbc(), "[dbo].[USP_VoucherInvoicesAdjustment_FormHistory]", p);
    }

    // =============================================================================== save

    /** BtnSave_Click / btnUpdate_Click → Insert(Auto) (:1009). */
    @Transactional
    public Map<String, Object> save(String kindName, InvoicesAdjustmentVoucherDto dto) {
        Kind k = kind(kindName);
        int recId = dto.recId == null ? 0 : dto.recId;
        s.requireRight(k.screen, recId > 0 ? "canUpdate" : "canSave",
                recId > 0 ? "You do not have the Update right for this screen." : "You do not have the Save right for this screen.");
        if (dto.lines == null || dto.lines.isEmpty()) throw new Refusal("Grid Record Not Found");
        validateHeader(dto);
        UserAccount u = s.user();
        String now = now();
        BigDecimal voucherAmount = dec(dto.voucherAmountText);
        List<Map<String, Object>> list = new ArrayList<>();
        BigDecimal total = BigDecimal.ZERO;
        for (int i = 0; i < dto.lines.size(); i++) {
            InvoicesAdjustmentVoucherDto.Line r = dto.lines.get(i);
            BigDecimal adj = dec(r.adjustmentAmount);
            int rowId = r.id == null ? 0 : r.id;
            if (!(rowId > 0 || adj.compareTo(BigDecimal.ZERO) > 0)) continue;
            int id = recId != 0 ? rowId : 0;
            Map<String, Object> d = model(k, u, now, dto, voucherAmount);
            d.put("Id", id);
            d.put("ActionTypeId", id <= 0 ? 1 : 2);
            d.put("RefDocumentTypeId", nz(r.refDocumentTypeId));
            d.put("RefDocNoId", nz(r.invoiceId));
            BigDecimal inv = dec(r.invoiceAmount);
            d.put("InvoiceAmount", inv);
            boolean advance = nz(dto.paymentTypeId) == k.advanceTypeForSave;
            d.put("AdvanceAmount", advance ? adj : BigDecimal.ZERO);
            d.put("AdjustmentAmount", advance ? BigDecimal.ZERO : adj);
            total = total.add(adj);
            d.put("Remarks", r.remarks == null ? "" : r.remarks);
            int rowNo = i + 1;
            if (nz(r.refDocumentTypeId) == 0) throw new Refusal("RefDocumentType Required in Detail Grid And row No: " + rowNo);
            if (nz(r.invoiceId) == 0) throw new Refusal("RefDocNo Required in Detail Grid And row No: " + rowNo);
            if (inv.compareTo(BigDecimal.ZERO) == 0) throw new Refusal("InvoiceAmount Required in Detail Grid And row No: " + rowNo);
            if (adj.compareTo(BigDecimal.ZERO) == 0) throw new Refusal("AdjustmentAmount Required in Detail Grid And row No: " + rowNo);
            list.add(d);
        }
        if (list.isEmpty()) throw new Refusal("At least enter one row in detail ");
        // The total checks read PaymentTypeId 1 on BOTH forms (Math.Round(decimal, 0) = half-to-even).
        BigDecimal t0 = total.setScale(0, RoundingMode.HALF_EVEN);
        BigDecimal v0 = voucherAmount.setScale(0, RoundingMode.HALF_EVEN);
        if (nz(dto.paymentTypeId) == 1 && t0.compareTo(v0) > 0) {
            throw new Refusal("Total adjustment Amount " + t0.toPlainString() + " In Grid cannot be greater then to voucher amount " + v0.toPlainString());
        }
        if (nz(dto.paymentTypeId) != 1 && t0.compareTo(v0) != 0) {
            throw new Refusal("Total adjustment Amount " + t0.toPlainString() + " In Grid Should be equal to voucher amount " + v0.toPlainString());
        }
        for (Map<String, Object> d : list) DesktopProc.setProc(s.jdbc(), "USP_VoucherInvoicesAdjustment_InsertUpdateDelete", d);
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("voucherHeadId", nz(dto.voucherHeadId));
        res.put("message", (recId == 0 ? "Record Saved Successfully [" : "Record Update Successfully [") + toInt(dto.voucherCode) + "] ");
        return res;
    }

    /** FormValidation (:907) — the header checks, in the desktop's order. */
    private static void validateHeader(InvoicesAdjustmentVoucherDto dto) {
        if (nz(dto.voucherHeadId) == 0) throw new Refusal("Voucher No Field is Required");
        String amt = dto.voucherAmountText == null ? "" : dto.voucherAmountText.trim();
        if (amt.isEmpty() || "0".equals(amt)) throw new Refusal("VoucherAmount Field is Required");
        if (nz(dto.paymentTypeId) == 0) throw new Refusal("Payment Type Field is Required");
        if (nz(dto.partyValue) == 0) throw new Refusal("Party Field is Required");
    }

    /** btnDelete_Click (:1224) — ActionTypeId 3 for every saved row of the loaded voucher. */
    @Transactional
    public Map<String, Object> delete(String kindName, InvoicesAdjustmentVoucherDto dto) {
        Kind k = kind(kindName);
        s.requireRight(k.screen, "canDelete", "You do not have the Delete right for this screen.");
        int recId = dto.recId == null ? 0 : dto.recId;
        if (recId == 0) throw new Refusal("Record Id not found for deletion...");
        UserAccount u = s.user();
        String now = now();
        BigDecimal voucherAmount = dec(dto.voucherAmountText);
        for (InvoicesAdjustmentVoucherDto.Line r : dto.lines) {
            int id = r.id == null ? 0 : r.id;
            if (id <= 0) continue;
            Map<String, Object> d = model(k, u, now, dto, voucherAmount);
            d.put("Id", id);
            d.put("RefDocumentTypeId", nz(r.refDocumentTypeId));
            d.put("RefDocNoId", nz(r.invoiceId));
            d.put("InvoiceAmount", dec(r.invoiceAmount));
            boolean advance = nz(dto.paymentTypeId) == k.advanceTypeForSave;
            BigDecimal adj = dec(r.adjustmentAmount);
            d.put("AdvanceAmount", advance ? adj : BigDecimal.ZERO);
            d.put("AdjustmentAmount", advance ? BigDecimal.ZERO : adj);
            d.put("Remarks", r.remarks == null ? "" : r.remarks);
            d.put("ActionTypeId", 3);
            DesktopProc.setProc(s.jdbc(), "USP_VoucherInvoicesAdjustment_InsertUpdateDelete", d);
        }
        Map<String, Object> res = new LinkedHashMap<>();
        res.put("success", true);
        res.put("message", "Deleted Successfully");
        return res;
    }

    /**
     * Model 1193 VoucherInvoicesAdjustment — every non-virtual property, at its CLR default unless
     * the form sets it (SetProc sends them all).
     */
    private Map<String, Object> model(Kind k, UserAccount u, String now, InvoicesAdjustmentVoucherDto dto, BigDecimal voucherAmount) {
        Map<String, Object> d = new LinkedHashMap<>();
        String vDate = dateTime(dto.voucherDate);
        d.put("DueDate", vDate);                      // detail.DueDate = txtVoucherDate.Value
        d.put("EntryDate", now);
        d.put("ModifyDate", now);
        d.put("VoucherDate", vDate);
        d.put("AdjustmentAmount", BigDecimal.ZERO);
        d.put("InvoiceAmount", BigDecimal.ZERO);
        d.put("AdvanceAmount", BigDecimal.ZERO);
        d.put("VoucherAmount", voucherAmount);
        d.put("ExchangeRate", BigDecimal.ZERO);
        d.put("ActionTypeId", 0);
        d.put("EntryUserId", u.getId());
        d.put("Id", 0);
        d.put("ModifyUserId", u.getId());
        d.put("PartyGlId", nz(dto.partyGlId));
        d.put("RefDocNoId", 0);
        d.put("ExImInvoicePaymentTermsDetailId", 0);
        d.put("RefDocumentTypeId", 0);
        d.put("SupplierCustomerId", nz(dto.supplierCustomerId));
        d.put("TransactionTypeId", k.transactionTypeId);
        d.put("VoucherCode", toInt(dto.voucherCode));
        d.put("VoucherHeadId", nz(dto.voucherHeadId));
        d.put("PaymentTypeId", nz(dto.paymentTypeId));
        d.put("Remarks", "");
        d.put("ScreenName", k.screen);
        d.put("PaymentTermId", 0);
        d.put("FcyId", 0);
        return d;
    }

    private static String now() { return new SimpleDateFormat("yyyy-MM-dd HH:mm:ss.SSS").format(new Date()); }

    private static String dateTime(String v) {
        if (v == null || v.trim().isEmpty()) return "1900-01-01";           // Conversion.ToDateTime of nothing
        String t = v.trim().replace('T', ' ');
        return t.length() > 19 ? t.substring(0, 19) : t;
    }

    /** Conversion.ToDecimal — thousand separators allowed, 0 when unreadable. */
    static BigDecimal dec(Object v) {
        if (v == null) return BigDecimal.ZERO;
        if (v instanceof Double) return BigDecimal.valueOf((Double) v);
        if (v instanceof Number) return new BigDecimal(v.toString());
        String t = String.valueOf(v).trim().replace(",", "");
        if (t.isEmpty()) return BigDecimal.ZERO;
        try { return new BigDecimal(t); } catch (NumberFormatException e) { return BigDecimal.ZERO; }
    }

    private static int nz(Integer v) { return v == null ? 0 : v; }
}
