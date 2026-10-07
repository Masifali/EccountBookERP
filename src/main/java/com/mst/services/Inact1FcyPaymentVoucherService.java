package com.mst.services;

import com.mst.models.dto.ContraVoucherDto;
import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import com.mst.services.desktopvoucher.DesktopVoucherSupport;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.*;

import static com.mst.services.desktopvoucher.DesktopVoucherSupport.toInt;

/**
 * Screen 23 "frmFcyPaymentVoucher" - Architecture.WinApp.Account_Definition/AcfrmFcyPaymentVoucher.cs (DocumentTypeId 204).
 * BLL 0414 ExImFCBankPaymensts (MakeVoucher / Save / GenerateCode / GetByID / HistoryForm), DAL 0471 SetDate, BLL 0418 ImInvoice,
 * 0419 ImLcOrder, 0076 MultiCurrency, 0476 ExImProceedsChargesType.
 *
 * Procedures are the desktop's, unchanged. Sp_ImInvoice_GetAllMethod and Sp_ImLcOrder_GetAllMethod (called by BLL 0418 / 0419 for the supplier,
 * invoice, contract, amount and exchange-rate lists) are not in procdure.utf8.sql (the dump only holds USP_ImLcOrder_GetAllMethod); they are
 * still called by name, so whatever the database answers (including "Could not find stored procedure ...") is shown as the desktop MessageBox shows it.
 *
 * Desktop behaviour kept on purpose: fcbank.VoucherHeadId carries the Gain/Loss ACCOUNT id; BranchId / ProjectId are the form fields BrancheId /
 * ProjectId (first row of Branches / Projects at load, 0 after RESETMAIN); the hidden bank-charges fields are saved as they stand.
 */
@Service
public class Inact1FcyPaymentVoucherService {
    public static final int SCREEN_ID = 23;
    public static final String SCREEN_NAME = "frmFcyPaymentVoucher";
    public static final int DOC_TYPE = 204;
    private static final String OUT_OF_RANGE = "Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index";
    private static final String INVOICE = "Invoice Payment";
    private static final String ADVANCE = "Advanced Payment";
    private static final DateTimeFormatter DMY = DateTimeFormatter.ofPattern("dd-MMM-yyyy", Locale.ENGLISH);

    @Autowired private JdbcTemplate jdbc;
    @Autowired private CurrentUserContext ctx;
    @Autowired private DesktopVoucherSupport support;

    // ------------------------------------------------------------------------------------------------ helpers

    private int org() { return ctx.currentOrganizationId(); }
    private int comp() { return ctx.currentCompanyId(); }

    private List<Map<String, Object>> rows(String proc, Object... kv) { return DesktopProc.rows(jdbc, proc, DesktopProc.params(kv)); }

    private static Object ci(Map<String, Object> r, String key) {
        if (r == null) return null;
        if (r.containsKey(key)) return r.get(key);
        for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    /** Conversion.ToDouble of a text box: thousands separators are accepted, blank is 0. */
    private static double num(Object o) {
        if (o == null) return 0d;
        if (o instanceof Number) return ((Number) o).doubleValue();
        String s = o.toString().trim().replace(",", "");
        if (s.isEmpty()) return 0d;
        try { return Double.parseDouble(s); } catch (Exception e) { return 0d; }
    }

    private static String txt(Object o) { return o == null ? "" : o.toString(); }

    private static String msg(Exception e) {
        Throwable c = e;
        while (c.getCause() != null && c.getCause() != c) c = c.getCause();
        return c.getMessage() == null ? e.getClass().getSimpleName() : c.getMessage();
    }

    /** Dates in procedure rows are written as dd-MMM-yyyy text so the page never depends on Jackson's date handling. */
    private static Object cell(Object v) {
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toLocalDate().format(DMY);
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().format(DMY);
        if (v instanceof java.util.Date) return new java.sql.Date(((java.util.Date) v).getTime()).toLocalDate().format(DMY);
        return v;
    }

    private static String iso(Object v) {
        if (v == null) return "";
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        String s = v.toString();
        return s.length() > 10 ? s.substring(0, 10) : s;
    }

    private Map<String, Object> pick(Map<String, Object> r, String... keys) {
        Map<String, Object> o = new LinkedHashMap<>();
        for (String k : keys) o.put(k, ci(r, k));
        return o;
    }

    // ------------------------------------------------------------------------------------------------ load

    public Map<String, Object> rights() {
        Map<String, Boolean> r = support.rights(SCREEN_NAME);
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("save", r.get("save")); o.put("update", r.get("update")); o.put("print", r.get("print"));
        o.put("canViewAllRecord", r.get("canViewAllRecord"));
        return o;
    }

    /** Acfrmfcbankreceipt_Load: rights, GenerateDocumentNo, BankAccountDr, GainLossAccountFill, CurrencyCOde, ChargerType, CombBranchFill, CombProjectFill. */
    public Map<String, Object> load() {
        Map<String, Object> m = new LinkedHashMap<>();
        List<String> errors = new ArrayList<>();
        m.put("rights", rights());
        try { m.put("docNo", generateCode()); } catch (Exception e) { errors.add(msg(e)); m.put("docNo", 0); }
        try { m.put("bankAccounts", bankAccounts()); } catch (Exception e) { m.put("bankAccounts", new ArrayList<>()); }
        try { m.put("gainLoss", gainLossAccounts()); } catch (Exception e) { m.put("gainLoss", new ArrayList<>()); }
        try { m.put("currencies", currencies()); } catch (Exception e) { m.put("currencies", new ArrayList<>()); }
        try { m.put("chargeTypes", chargeTypes()); } catch (Exception e) { m.put("chargeTypes", new ArrayList<>()); }
        try { m.put("branchId", firstBranchId()); } catch (Exception e) { errors.add(msg(e)); m.put("branchId", 0); }
        try { m.put("projectId", firstProjectId()); } catch (Exception e) { errors.add(msg(e)); m.put("projectId", 0); }
        m.put("errors", errors);
        return m;
    }

    /** btnRefresh_Click: BankAccountDr, CurrencyCOde, ChargerType. */
    public Map<String, Object> refresh() {
        Map<String, Object> m = new LinkedHashMap<>();
        try { m.put("bankAccounts", bankAccounts()); } catch (Exception e) { /* the form swallows it */ }
        try { m.put("currencies", currencies()); } catch (Exception e) { /* the form swallows it */ }
        try { m.put("chargeTypes", chargeTypes()); } catch (Exception e) { /* the form swallows it */ }
        return m;
    }

    /** GenerateDocumentNo: ExImFCBankPaymensts.GenerateCode (the form only writes the box when Code > 0). */
    public int generateCode() {
        List<Map<String, Object>> r = rows("Sp_ExImFCBankPayments_GetAllMethod", "OrganizationId", org(), "CompanyId", comp(),
                "DocumentTypeId", DOC_TYPE, "Activity", "GenerateDocNo");
        if (r.isEmpty()) throw new IllegalStateException(OUT_OF_RANGE);
        return toInt(ci(r.get(0), "DocNo"));
    }

    private List<Map<String, Object>> coaFiltered(boolean banks) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : support.coaAllocationSearch()) {
            int t = toInt(ci(r, "AccountTypeId"));
            boolean ok = banks ? t == 15 : (t != 15 && t != 11 && t != 12 && t != 2 && t != 4);
            if (ok) out.add(pick(r, "Id", "AccountTitle"));
        }
        return out;
    }

    /** BankAccountDr: CoaAllocationGetAllServiceBind, AccountTypeId == 15. */
    public List<Map<String, Object>> bankAccounts() { return coaFiltered(true); }

    /** GainLossAccountFill: AccountTypeId not in 15, 11, 12, 2, 4. */
    public List<Map<String, Object>> gainLossAccounts() { return coaFiltered(false); }

    /** CurrencyCOde: MultiCurrency.GetAll (ReadAll). */
    public List<Map<String, Object>> currencies() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows("Sp_MultiCurrency_GetAllMethod", "OrganizationId", org(), "CompanyId", comp(), "Activity", "ReadAll"))
            out.add(pick(r, "Id", "CurrencyCode"));
        return out;
    }

    /** ChargerType: ExImProceedsChargesType.Getall (ReadByOrganizationCompanyId). */
    public List<Map<String, Object>> chargeTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows("Sp_ExImProceedsChargesType_GetAllMethod", "OrganizationId", org(), "CompanyId", comp(),
                "Activity", "ReadByOrganizationCompanyId"))
            out.add(pick(r, "Id", "ProceedsChargesTypeCode"));
        return out;
    }

    /** CombBranchFill: BrancheServiceBind -> Branches.GetAll (Sp_Branches_GetAllMethod, GetAll), first row. */
    private int firstBranchId() {
        List<Map<String, Object>> r = rows("Sp_Branches_GetAllMethod", "OrganizationId", org(), "CompanyId", comp(), "Activity", "GetAll");
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "Id"));
    }

    /** CombProjectFill: ProjectServiceBind -> Projects.GetAlldt (Sp_Projects_GetAllMethod, MethodType GetAll), first row. */
    private int firstProjectId() {
        List<Map<String, Object>> r = rows("Sp_Projects_GetAllMethod", "OrganizationId", org(), "CompanyId", comp(), "MethodType", "GetAll");
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "Id"));
    }

    // ------------------------------------------------------------------------------------------------ payment term lookups

    /** PaymentTerm(): the supplier list of the chosen term. Advanced: ImLcOrder.GetImportCustomerByAdvancePayment; Invoice: ImInvoice.GetSupplierForFcyPaymentVoucher. */
    public List<Map<String, Object>> suppliers(String term) {
        List<Map<String, Object>> src = ADVANCE.equals(term)
                ? rows("Sp_ImLcOrder_GetAllMethod", "OrganizationId", org(), "CompanyId", comp(), "Activity", "GetImportCustomerByAdvancePayment")
                : rows("Sp_ImInvoice_GetAllMethod", "OrganizationId", org(), "CompanyId", comp(), "Activity", "GetSupplierForFcyPaymentVoucher");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : src) out.add(pick(r, "SupplierCustomerId", "CompanyName"));
        return out;
    }

    /** LcOrder(): ImLcOrder.GetLcOrderNoBySupplierCustomerId (@SupCustId). */
    public List<Map<String, Object>> lcOrders(int supplierId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows("Sp_ImLcOrder_GetAllMethod", "OrganizationId", org(), "CompanyId", comp(),
                "SupCustId", supplierId, "Activity", "GetLcOrderNoBySupplierCustomerId"))
            out.add(pick(r, "Id", "LcOrderNo"));
        return out;
    }

    /** InvoiceNo(): ImInvoice.GetInvoiceNoForFcyPaymentVoucher. */
    public List<Map<String, Object>> invoices(int supplierId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows("Sp_ImInvoice_GetAllMethod", "OrganizationId", org(), "CompanyId", comp(),
                "SupplierCustomerId", supplierId, "Activity", "GetInvoiceNoForFcyPaymentVoucher"))
            out.add(pick(r, "Id", "InvoiceNo"));
        return out;
    }

    /** InvoiceAmountGet(): ImInvoice.GetInvoiceAmountAndPaidAmount; null when the procedure returns no row. */
    public Map<String, Object> invoiceAmount(int invoiceId) {
        List<Map<String, Object>> r = rows("Sp_ImInvoice_GetAllMethod", "OrganizationId", org(), "CompanyId", comp(),
                "Id", invoiceId, "Activity", "GetInvoiceAmountAndPaidAmount");
        if (r.isEmpty()) return null;
        return pick(r.get(0), "InvoiceAmount", "FcyPaidAmount");
    }

    /** GetExchangeRateandCurrencyFromVoucher(): ImInvoice.GetExchangeRateandCurrencyFromVoucher; null when no row. */
    public Map<String, Object> invoiceRate(int invoiceId) {
        List<Map<String, Object>> r = rows("Sp_ImInvoice_GetAllMethod", "OrganizationId", org(), "CompanyId", comp(),
                "Id", invoiceId, "Activity", "GetExchangeRateandCurrencyFromVoucher");
        if (r.isEmpty()) return null;
        return pick(r.get(0), "ExchangeCurrencyRate", "MultiCurrencyId");
    }

    /** CommonServices.VoucherHeadIdGet(id, 204). */
    public int voucherHeadId(int id) {
        List<Map<String, Object>> r = rows("Sp_Vouchers_GetMethods", "Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", org(), "CompanyId", comp(), "DocumentTypeId", DOC_TYPE, "DocumentTypeSrNo", id);
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "Id"));
    }

    /** btnPrint / the automatic slip after Save: the voucher head of the record, printed through acc-102 with DocumentTypeId 204. */
    public Map<String, Object> printTarget(int id) {
        support.requireRight(SCREEN_NAME, "print", "You do not have the right to print this voucher.");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("voucherHeadId", id > 0 ? voucherHeadId(id) : 0);
        m.put("documentTypeId", DOC_TYPE);
        return m;
    }

    // ------------------------------------------------------------------------------------------------ read / history

    /** ReadById: ExImFCBankPaymensts.GetByID (header ReadById + deductions ReadByHeaderId) and VoucherHeadIdGet. */
    public Map<String, Object> read(int id) {
        List<Map<String, Object>> h = rows("Sp_ExImFCBankPayments_GetAllMethod", "Id", id, "Activity", "ReadById");
        if (h.isEmpty() || toInt(ci(h.get(0), "OrganizationId")) != org() || toInt(ci(h.get(0), "CompanyId")) != comp())
            throw new IllegalStateException(OUT_OF_RANGE);
        Map<String, Object> hd = h.get(0);
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        o.put("VoucherHeadIdOfRecord", voucherHeadId(id));
        o.put("DocumentNo", ci(hd, "DocumentNo"));
        o.put("DocumentDate", iso(ci(hd, "DocumentDate")));
        o.put("ExmLcPaymentTerm", ci(hd, "ExmLcPaymentTerm"));
        for (String k : new String[]{"SupplierCustomerId", "ExImLcOrderId", "ExImInvoiceId", "BankGlDrAc", "MultiCurrencyId", "VoucherHeadId"})
            o.put(k, toInt(ci(hd, k)));
        o.put("BankFbpNo", ci(hd, "BankFbpNo"));
        for (String k : new String[]{"FcGrossAmount", "FcFBCharges", "FcNetAmount", "ExchangeRate", "NetAmountRs", "BankDrAmount", "FcBankChargesAmountRs"})
            o.put(k, num(ci(hd, k)));
        o.put("Remarks", ci(hd, "Remarks") == null ? "" : ci(hd, "Remarks"));
        List<Map<String, Object>> ded = new ArrayList<>();
        for (Map<String, Object> r : rows("Sp_ExImFCBankPayments_GetAllMethod", "Id", id, "Activity", "ReadByHeaderId")) {
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Type", ci(r, "Type"));
            d.put("ChargesTypeId", toInt(ci(r, "ExImProceedsChargesTypeId")));
            d.put("ChargesType", ci(r, "ProceedsChargesTypeCode"));
            d.put("Remarks", ci(r, "ProceedsRemarks"));
            d.put("FcAmount", num(ci(r, "FcAmount")));
            d.put("RsAmount", num(ci(r, "RsAmount")));
            ded.add(d);
        }
        o.put("deductions", ded);
        return o;
    }

    /** bindHistory (NoOfRecords 50) / btnLoadAll (all): ExImFCBankPaymensts.HistoryForm. */
    public List<Map<String, Object>> history(boolean all) {
        boolean canAll = Boolean.TRUE.equals(support.rights(SCREEN_NAME).get("canViewAllRecord"));
        Map<String, Object> p = DesktopProc.params("OrganizationId", org(), "CompanyId", comp(), "DocumentTypeId", DOC_TYPE, "CanViewAllRecord", canAll);
        if (!all) p.put("NoOfRecords", 50);
        if (!canAll) p.put("EntryUser", ctx.currentUserId());
        p.put("Activity", "FormHistory");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_ExImFCBankPayments_GetAllMethod", p)) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : r.entrySet()) o.put(e.getKey(), cell(e.getValue()));
            out.add(o);
        }
        return out;
    }

    // ------------------------------------------------------------------------------------------------ save / update

    private Map<String, Object> result(boolean saved, String message, int id, int voucherHeadId) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("saved", saved); m.put("message", message); m.put("id", id); m.put("voucherHeadId", voucherHeadId);
        return m;
    }

    @SuppressWarnings("unchecked")
    private List<Map<String, Object>> list(Object o) { return o instanceof List ? (List<Map<String, Object>>) o : new ArrayList<>(); }

    /** FormValidation(): first failure wins, same texts and order. */
    private void formValidation(Map<String, Object> b) {
        String fbp = txt(b.get("fbpNo")).trim();
        if (fbp.isEmpty() || fbp.equals("0")) throw new IllegalArgumentException("FBP Bank Field Required");
        String gross = txt(b.get("gross")).trim();
        if (gross.isEmpty() || num(gross) == 0d) throw new IllegalArgumentException("Fc Gross Field Required");
        String netFc = txt(b.get("netFc")).trim();
        if (netFc.isEmpty() || num(netFc) == 0d) throw new IllegalArgumentException("Fc Net Amount Field Required");
        String rate = txt(b.get("rate")).trim();
        if (rate.isEmpty() || num(rate) == 0d) throw new IllegalArgumentException("Exchange Rate Field Required");
        String net = txt(b.get("netAmount")).trim();
        if (net.isEmpty() || num(net) == 0d) throw new IllegalArgumentException("Net Amount Field Required");
        if (toInt(b.get("supplierId")) <= 0 || !Boolean.TRUE.equals(b.get("supplierSelected"))) throw new IllegalArgumentException("customer  Field Required");
        String term = txt(b.get("term"));
        if (!INVOICE.equals(term) && !ADVANCE.equals(term)) throw new IllegalArgumentException("Payment term Field Required");
        if (INVOICE.equals(term) && toInt(b.get("gainLossId")) <= 0) throw new IllegalArgumentException("GainLossAc Field Required");
        if (ADVANCE.equals(term) && txt(b.get("lcOrderText")).isEmpty()) throw new IllegalArgumentException("LC Order Field Required");
        if (INVOICE.equals(term) && txt(b.get("invoiceText")).isEmpty()) throw new IllegalArgumentException("Invoice Field Required");
        if (toInt(b.get("bankAccId")) <= 0) throw new IllegalArgumentException("Bank Account (DR) Field Required");
        if (toInt(b.get("currencyId")) <= 0) throw new IllegalArgumentException("Currency Code Field Required");
    }

    private boolean idIn(List<Map<String, Object>> src, int id) {
        for (Map<String, Object> r : src) if (toInt(ci(r, "Id")) == id) return true;
        return false;
    }

    /** btnsave_Click / btnUpdate_Click: validation, MakeVoucher, then DAL SetDate in one transaction. */
    @Transactional
    public Map<String, Object> save(Map<String, Object> b) {
        int id = toInt(b.get("id"));
        boolean upd = Boolean.TRUE.equals(b.get("update")) || id > 0;
        support.requireRight(SCREEN_NAME, upd ? "update" : "save", upd ? "You do not have the right to update." : "You do not have the right to save.");
        if (upd && id == 0) throw new IllegalArgumentException("Record Not Update  " + id);
        String grossTxt = txt(b.get("gross"));
        String chargesTxt = txt(b.get("bankCharges"));
        if (!chargesTxt.isEmpty() && !grossTxt.isEmpty() && num(chargesTxt) > num(grossTxt))
            throw new IllegalArgumentException("FCY Bank Charges not gratter than FCY Amount");
        formValidation(b);

        String term = txt(b.get("term"));
        int supplierId = toInt(b.get("supplierId"));
        int invoiceId = toInt(b.get("invoiceId"));
        double previous = 0d;
        if (id > 0) {
            List<Map<String, Object>> cur = rows("Sp_ExImFCBankPayments_GetAllMethod", "Id", id, "Activity", "ReadById");
            if (cur.isEmpty() || toInt(ci(cur.get(0), "OrganizationId")) != org() || toInt(ci(cur.get(0), "CompanyId")) != comp())
                throw new IllegalStateException("Record Not Update  " + id);
            previous = Math.floor(Math.abs(num(ci(cur.get(0), "FcGrossAmount"))) + 0.5) * (num(ci(cur.get(0), "FcGrossAmount")) < 0 ? -1 : 1);
        }
        if (INVOICE.equals(term)) {
            Map<String, Object> amt = invoiceAmount(invoiceId);
            double balance = 0d;
            if (amt != null) {
                double d = num(amt.get("InvoiceAmount")) - num(amt.get("FcyPaidAmount"));
                balance = Math.floor(Math.abs(d) + 0.5) * (d < 0 ? -1 : 1);
            }
            if (num(grossTxt) > balance + previous) throw new IllegalArgumentException("FCY Amount cannot be greater than Balance Amount please check");
        }
        int bankAcc = toInt(b.get("bankAccId"));
        if (!idIn(coaFiltered(true), bankAcc)) throw new IllegalArgumentException("Bank Account (DR) Field Required");
        int gainLoss = toInt(b.get("gainLossId"));
        if (gainLoss > 0 && !idIn(coaFiltered(false), gainLoss)) throw new IllegalArgumentException("GainLossAc Field Required");
        int currencyId = toInt(b.get("currencyId"));
        if (!idIn(currencies(), currencyId)) throw new IllegalArgumentException("Currency Code Field Required");

        int user = ctx.currentUserId();
        String now = DesktopVoucherSupport.now();
        String docDate = txt(b.get("docDate"));
        boolean zeroed = Boolean.TRUE.equals(b.get("branchProjectZero"));
        int branchId = zeroed ? 0 : firstBranchId();
        int projectId = zeroed ? 0 : firstProjectId();

        /* model */
        Map<String, Object> fc = new LinkedHashMap<>();
        fc.put("Id", id);
        fc.put("DocumentTypeId", DOC_TYPE);
        fc.put("DocumentNo", (int) Math.rint(num(b.get("docNo"))));
        fc.put("DocumentDate", docDate);
        fc.put("SupplierCustomerId", supplierId);
        fc.put("ExImLcOrderId", toInt(b.get("lcOrderId")));
        fc.put("ExImInvoiceId", invoiceId);
        fc.put("BankGlDrAc", bankAcc);
        fc.put("BankFbpNo", txt(b.get("fbpNo")));
        fc.put("ExmLcPaymentTerm", term);
        fc.put("MultiCurrencyId", currencyId);
        fc.put("FcGrossAmount", num(grossTxt));
        fc.put("FcFBCharges", num(chargesTxt));
        fc.put("FcNetAmount", num(b.get("netFc")));
        fc.put("ExchangeRate", num(b.get("rate")));
        fc.put("NetAmountRs", num(b.get("netAmount")));
        fc.put("BankDrAmount", num(b.get("bankDr")));
        fc.put("VoucherHeadId", gainLoss);
        fc.put("FcBankChargesTypeAcId", toInt(b.get("chargesTypeAcId")));
        fc.put("FcBankChargesAmountRs", num(b.get("chargesRs")));
        double voucherRate = num(b.get("exportVoucherRate"));
        fc.put("Remarks", txt(b.get("remarks")));
        fc.put("OrganizationId", org());
        fc.put("CompanyId", comp());
        fc.put("BranchId", branchId);
        fc.put("ProjectId", projectId);
        fc.put("FinancialYearId", ctx.currentFinancialYearId());
        fc.put("EntryUser", user);
        fc.put("ModifyUser", user);

        List<Map<String, Object>> ded = new ArrayList<>();
        for (Map<String, Object> r : list(b.get("rows"))) {
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("Type", txt(r.get("type")));
            d.put("ExImProceedsChargesTypeId", toInt(r.get("chargesTypeId")));
            d.put("ProceedsRemarks", txt(r.get("remarks")));
            d.put("FcAmount", num(r.get("fc")));
            d.put("RsAmount", num(r.get("rs")));
            ded.add(d);
        }

        /* BLL MakeVoucher */
        List<Map<String, Object>> types = rows("Sp_ExImProceedsChargesType_GetAllMethod", "OrganizationId", org(), "CompanyId", comp(),
                "Activity", "ReadByOrganizationCompanyId");
        double debitsAll = 0d, credits = 0d;
        ContraVoucherDto.Head vh = new ContraVoucherDto.Head();
        List<ContraVoucherDto.Detail> vds = new ArrayList<>();
        vh.DocumentTypeId = DOC_TYPE;
        vh.DocumentTypeSrNo = id;
        vh.RefDocNoId = id;
        vh.VoucherCode = (Integer) fc.get("DocumentNo");
        vh.VoucherDate = docDate;
        vh.Remarks = "";
        vh.RemarksOtherLingo = "";
        List<Map<String, Object>> sup = rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", org(), "CompanyId", comp(),
                "Activity", "GetGlAccountIdandCompanyNameBySupplierCustomerId");
        if (!sup.isEmpty()) {
            Map<String, Object> found = null;
            for (Map<String, Object> r : sup) if (toInt(ci(r, "Id")) == supplierId) { found = r; break; }
            if (found == null) throw new IllegalStateException("Supplier GLAccountId not Found");
            vh.RefAccountId = toInt(ci(found, "GlAccountId"));
        }
        vh.ChequeDate = LocalDate.now().toString();
        vh.IncludeWHT = Boolean.FALSE;
        vh.BranchId = branchId;
        vh.ProjectId = projectId;
        vh.ManualBillNo = "";
        vh.DueDate = now;
        vh.DueDays = 0;
        vh.ExchangeCurrencyRate = (Double) fc.get("ExchangeRate");
        vh.FcAmount = (Double) fc.get("FcNetAmount");
        vh.MultiCurrencyId = currencyId;
        vh.OrganizationId = org();
        vh.CompanyId = comp();
        vh.FinancialYearId = (Integer) fc.get("FinancialYearId");
        vh.EntryUser = user;
        vh.EntryDate = now;
        vh.ModifyDate = now;
        vh.ModifyUser = user;
        vh.PostUser = 0;
        double rate = (Double) fc.get("ExchangeRate");
        double bankDr = (Double) fc.get("BankDrAmount");
        String remarks = (String) fc.get("Remarks");

        ContraVoucherDto.Detail d1 = new ContraVoucherDto.Detail();
        d1.AccountId = bankAcc;
        d1.AgainstAccountId = vh.RefAccountId;
        d1.CreditAmount = bankDr;
        credits += d1.CreditAmount;
        d1.DebitAmount = 0d;
        d1.Comments = remarks;
        d1.DMultiCurrencyId = currencyId;
        d1.DExchangeCurrencyRate = rate;
        d1.DCurrencyAmount = bankDr / rate;
        vds.add(d1);
        if (INVOICE.equals(term) && voucherRate > 0d) {
            double num5 = num(grossTxt) * voucherRate;
            double num6 = bankDr - num5;
            ContraVoucherDto.Detail d2 = new ContraVoucherDto.Detail();
            d2.AccountId = vh.RefAccountId;
            d2.AgainstAccountId = bankAcc;
            d2.CreditAmount = 0d;
            d2.DebitAmount = num5;
            debitsAll += d2.DebitAmount;
            d2.Comments = remarks;
            d2.DMultiCurrencyId = currencyId;
            d2.DExchangeCurrencyRate = rate;
            d2.DCurrencyAmount = num5 / voucherRate;
            vds.add(d2);
            ContraVoucherDto.Detail d3 = new ContraVoucherDto.Detail();
            d3.AccountId = gainLoss;
            d3.AgainstAccountId = vh.RefAccountId;
            if (num6 > 0d) { d3.CreditAmount = 0d; d3.DebitAmount = num6; }
            else { d3.DebitAmount = 0d; d3.CreditAmount = Math.abs(num6); }
            d3.Comments = remarks;
            d3.DMultiCurrencyId = currencyId;
            d3.DExchangeCurrencyRate = rate;
            d3.DCurrencyAmount = num6 / voucherRate;
            vds.add(d3);
        } else {
            ContraVoucherDto.Detail d4 = new ContraVoucherDto.Detail();
            d4.AccountId = vh.RefAccountId;
            d4.AgainstAccountId = bankAcc;
            d4.CreditAmount = 0d;
            d4.DebitAmount = bankDr;
            debitsAll += d4.DebitAmount;
            d4.Comments = remarks;
            d4.DMultiCurrencyId = currencyId;
            d4.DExchangeCurrencyRate = rate;
            d4.DCurrencyAmount = bankDr / rate;
            vds.add(d4);
        }
        if (!ded.isEmpty()) {
            for (Map<String, Object> r : ded) {
                if (types.isEmpty()) continue;
                Map<String, Object> t = null;
                for (Map<String, Object> x : types) if (toInt(ci(x, "Id")) == (Integer) r.get("ExImProceedsChargesTypeId")) { t = x; break; }
                if (t == null) throw new IllegalStateException("Charges AccountType GLAccountId not Found");
                int gl = toInt(ci(t, "ProceedsChargesGlAccount"));
                double rs = (Double) r.get("RsAmount");
                boolean local = "Local".equals(r.get("Type"));
                ContraVoucherDto.Detail a = new ContraVoucherDto.Detail();
                a.AccountId = gl;
                a.AgainstAccountId = bankAcc;
                a.Comments = (String) r.get("ProceedsRemarks");
                a.DebitAmount = rs;
                credits += a.DebitAmount;
                a.DCurrencyAmount = local ? rs / rate : (Double) r.get("FcAmount");
                a.DExchangeCurrencyRate = rate;
                a.DMultiCurrencyId = currencyId;
                a.CreditAmount = 0d;
                vds.add(a);
                ContraVoucherDto.Detail c = new ContraVoucherDto.Detail();
                c.AccountId = bankAcc;
                c.AgainstAccountId = gl;
                c.Comments = (String) r.get("ProceedsRemarks");
                c.DebitAmount = 0d;
                c.CreditAmount = rs;
                debitsAll += c.CreditAmount;
                c.DCurrencyAmount = local ? rs / rate : (Double) r.get("FcAmount");
                c.DExchangeCurrencyRate = rate;
                c.DMultiCurrencyId = currencyId;
                vds.add(c);
            }
            vh.VoucherAmount = credits;
            vh.BillAmount = debitsAll;
        }

        /* DAL SetDate */
        Map<String, Object> hp = new LinkedHashMap<>(fc);
        hp.put("IsApproved", Boolean.FALSE);
        hp.put("PostState", Boolean.FALSE);
        hp.put("PostUser", 0);
        hp.put("EntryDate", now);
        hp.put("ModifyDate", now);
        hp.put("PostDate", now);
        hp.remove("Remarks");
        hp.put("Remarks", fc.get("Remarks"));
        int num3 = DesktopProc.setProc(jdbc, id == 0 ? "Sp_ExImFCBankPaymensts_Insert" : "Sp_ExImFCBankPaymensts_Update", hp);
        int recId = id;
        if (num3 > 0) recId = num3; else num3 = recId;
        for (Map<String, Object> d : ded) {
            Map<String, Object> p = new LinkedHashMap<>();
            p.put("Id", 0);
            p.put("ExImFCBankPaymentsId", recId);
            p.put("ExImProceedsChargesTypeId", d.get("ExImProceedsChargesTypeId"));
            p.put("FcAmount", d.get("FcAmount"));
            p.put("RsAmount", d.get("RsAmount"));
            p.put("ProceedsRemarks", d.get("ProceedsRemarks"));
            p.put("Type", d.get("Type"));
            DesktopProc.setProc(jdbc, "Sp_ExImFcBankPaymentsDeductions_Insert", p);
        }
        int existing = voucherHeadId(recId);
        if (existing != 0) vh.Id = existing;
        vh.DocumentTypeSrNo = recId;
        vh.RefDocNoId = recId;
        int num2 = DesktopProc.setProc(jdbc, existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", DesktopVoucherSupport.fields(vh));
        if (num2 > 0) vh.Id = num2;
        if (vds.isEmpty()) throw new IllegalStateException("VoucherDetail List Not Found");
        for (ContraVoucherDto.Detail d : vds) {
            d.VoucherHeadId = vh.Id;
            DesktopProc.setProc(jdbc, "Sp_VoucherDetail_Insert", DesktopVoucherSupport.fields(d));
        }
        vh.RefDocNoId = vh.Id;
        int refId = DesktopProc.setProc(jdbc, "Sp_VoucherHead_H_Insert", DesktopVoucherSupport.fields(vh));
        for (ContraVoucherDto.Detail d : vds) {
            d.VoucherHeadId = vh.Id;
            d.DocumentTypeIdRef = refId;
            DesktopProc.setProc(jdbc, "Sp_VoucherDetail_H_Insert", DesktopVoucherSupport.fields(d));
        }
        String m = num3 > 0
                ? (!upd ? "Record Save Successfully [" + fc.get("DocumentNo") + "]" : "Receord Update Successfully [" + fc.get("DocumentNo") + "]")
                : "";
        return result(true, m, num3, voucherHeadId(num3));
    }
}
