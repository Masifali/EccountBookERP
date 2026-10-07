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

import static com.mst.services.desktopvoucher.DesktopVoucherSupport.toDouble;
import static com.mst.services.desktopvoucher.DesktopVoucherSupport.toInt;

/**
 * Screens 36 "Payment By Invoice" (Architecture.WinApp.Account_Definition.PaymentByInvoiceAccount, DocumentTypeId 14) and
 * 35 "Receipt By Invoice" (ReceiptByInvoiceAccount, DocumentTypeId 13). One class, two kinds, because the two desktop forms are
 * the same source with the differences listed below (PaymentByInvoiceAccount.cs / ReceiptByInvoiceAccount.cs, BLL 0646 / 0651,
 * DAL 0577 / 0582, VoucherHead BLL 0654).
 *
 * Desktop behaviour reproduced on purpose (the page must behave as the forms do):
 *  - Receipt: the invoice list on supplier Leave is VoucherHead.GetReceivablesByInvoice, i.e. Sp_PaymentByInvoiceHeader_GetReceivablesByInvoice
 *    (the form passes SaleInvoiceDocumentTypeIds, which that BLL method ignores); only the History tab uses the Receipt procedure.
 *  - Receipt: the supplier list is SupplierCustomerGetforComboServiceBind (Id, CompanyName, GlAccountId only), so TransType Advance fails
 *    on the form with "Column 'AdvanceGlAcId' does not belong to table .".
 *  - Receipt: GenerateCode is sent DocumentTypeId = 0 and FinancialYearId = 0 (the form sets Id = 13, not DocumentTypeId); the header
 *    insert writes FinancialYearId = 0 and Tax* = 0; OtherDetail.TransType is Conversion.ToInt(name) = 0; PaymentTermsId = 0.
 *  - Receipt: BLL MakeVoucher only builds a voucher for DocumentTypeId 29, so the voucher head handed to the DAL is an empty
 *    VoucherHead (EntryDate / ModifyDate / VoucherDate = 0001-01-01). The DAL inserts the header and detail rows first and then sends that
 *    head: SQL Server rejects the date ("SqlDateTime overflow ...") and the whole transaction is rolled back. The page therefore runs the
 *    same writes and ends with the same message and the same rollback; nothing is stored.
 *  - Payment/Receipt: wht per row and the per-row Payment are Conversion.ToInt (banker's rounding), amounts are Conversion.ToSingle (float).
 *  - The customer balance label uses the form field CustomerGlId, which only SupplierGLIdGet (called from Insert) sets; the page sends the same
 *    stale value.
 */
@Service
public class Inact1InvoiceAccountService {
    public enum Kind {
        PAYMENT(36, "PaymentByInvoiceAccount", 14, "PaymentByInvoice", "PaymentByInvoicePurchaseInvoice"),
        RECEIPT(35, "ReceiptByInvoiceAccount", 13, "ReceiptByInvoice", "ReceiptsByInvoiceSalesInvoice");
        public final int screenId; public final String screenName; public final int docType; public final String table; public final String config;
        Kind(int s, String n, int d, String t, String c) { screenId = s; screenName = n; docType = d; table = t; config = c; }
        public static Kind of(String s) { return "receipt".equalsIgnoreCase(s) ? RECEIPT : PAYMENT; }
    }

    private static final String OUT_OF_RANGE = "Index was out of range. Must be non-negative and less than the size of the collection.\r\nParameter name: index";
    private static final String SQL_OVERFLOW = "SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.";
    private static final DateTimeFormatter DMY = DateTimeFormatter.ofPattern("dd-MMM-yyyy", Locale.ENGLISH);

    @Autowired private JdbcTemplate jdbc;
    @Autowired private CurrentUserContext ctx;
    @Autowired private DesktopVoucherSupport support;

    // ------------------------------------------------------------------------------------------------ helpers

    private static Object ci(Map<String, Object> r, String key) {
        if (r == null) return null;
        if (r.containsKey(key)) return r.get(key);
        for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    /** "dd-MMM-yyyy" of a database date / timestamp / text, "" for null. */
    private static String day(Object o) {
        if (o == null) return "";
        String s = o.toString();
        try { return LocalDate.parse(s.length() > 10 ? s.substring(0, 10) : s).format(DMY); } catch (Exception e) { return s; }
    }

    /** Convert.ToDateTime of a cell that must hold a date; DBNull throws like the form's Convert.ToDateTime(DBNull). */
    private static String dayStrict(Object o) {
        if (o == null) throw new IllegalStateException("Object cannot be cast from DBNull to other types.");
        return day(o);
    }

    private static String isoDay(Object o) {
        if (o == null) return null;
        String s = o.toString().trim();
        if (s.isEmpty()) return null;
        if (s.matches("\\d{2}-[A-Za-z]{3}-\\d{4}")) return LocalDate.parse(s, DMY).toString();
        return s.length() > 10 ? s.substring(0, 10) : s;
    }

    /** Conversion.ToDateTime: an unreadable cell becomes 1900-01-01. */
    private static String dateOr1900(Object o) {
        try { String s = isoDay(o); return s == null ? "1900-01-01" : LocalDate.parse(s).toString(); } catch (Exception e) { return "1900-01-01"; }
    }

    /** Convert.ToInt32(double): round half to even. */
    private static int toIntRounded(double v) { return (int) Math.rint(v); }

    /** Conversion.ToSingle: the value goes through a 32-bit float. */
    private static double single(Object o) { return (double) (float) toDouble(o); }

    private static boolean bool(Object o) { return DesktopVoucherSupport.toBool(o); }

    private int org() { return ctx.currentOrganizationId(); }
    private int comp() { return ctx.currentCompanyId(); }

    private List<Map<String, Object>> rows(String proc, Object... kv) { return DesktopProc.rows(jdbc, proc, DesktopProc.params(kv)); }

    private boolean ownsAccount(int id) {
        Integer n = jdbc.queryForObject("SELECT COUNT(1) FROM dbo.ChartofAccount WHERE Id = ? AND OrganizationId = ? AND CompanyId = ?",
                Integer.class, id, org(), comp());
        return n != null && n > 0;
    }

    // ------------------------------------------------------------------------------------------------ load

    /** ReceiptByInvoice_Load: rights, SupplierCustomer, DebitAccount, VoucherNofill, TransTypeFill, BindTaxTypes. Each list fails on its own like the form's try blocks. */
    public Map<String, Object> load(Kind k) {
        Map<String, Object> m = new LinkedHashMap<>();
        List<String> errors = new ArrayList<>();
        m.put("rights", rights(k));
        try { m.put("suppliers", suppliers(k)); } catch (Exception e) { errors.add(msg(e)); m.put("suppliers", new ArrayList<>()); }
        try { m.put("coa", coa()); } catch (Exception e) { errors.add(msg(e)); m.put("coa", new ArrayList<>()); }
        try { m.put("docNo", generateCode(k)); } catch (Exception e) { errors.add(msg(e)); m.put("docNo", 0); }
        try { m.put("taxTypes", taxTypes()); } catch (Exception e) { errors.add(msg(e)); m.put("taxTypes", new ArrayList<>()); }
        m.put("errors", errors);
        return m;
    }

    /** btnRefresh_Click: DebitAccount, SupplierCustomer, BindTaxTypes. */
    public Map<String, Object> refresh(Kind k) {
        Map<String, Object> m = new LinkedHashMap<>();
        List<String> errors = new ArrayList<>();
        try { m.put("coa", coa()); } catch (Exception e) { errors.add(msg(e)); }
        try { m.put("suppliers", suppliers(k)); } catch (Exception e) { errors.add(msg(e)); }
        try { m.put("taxTypes", taxTypes()); } catch (Exception e) { errors.add(msg(e)); }
        m.put("errors", errors);
        return m;
    }

    private static String msg(Exception e) {
        Throwable c = e;
        while (c.getCause() != null && c.getCause() != c) c = c.getCause();
        return c.getMessage() == null ? e.getClass().getSimpleName() : c.getMessage();
    }

    private Map<String, Object> rights(Kind k) {
        Map<String, Boolean> r = support.rights(k.screenName);
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("save", r.get("save")); o.put("update", r.get("update")); o.put("print", r.get("print")); o.put("delete", r.get("delete"));
        return o;
    }

    /** Payment: SupplierCustomerGetAllServiceBind (ReadByOrganizationCompanyId); Receipt: SupplierCustomerGetforComboServiceBind (ReadByOrganizationIdCompanyIdForBinding). */
    private List<Map<String, Object>> suppliers(Kind k) {
        List<Map<String, Object>> out = new ArrayList<>();
        boolean pay = k == Kind.PAYMENT;
        for (Map<String, Object> r : rows("Sp_SupplierCustomer_GetAllMethod", "OrganizationId", org(), "CompanyId", comp(),
                "Activity", pay ? "ReadByOrganizationCompanyId" : "ReadByOrganizationIdCompanyIdForBinding")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id")); o.put("CompanyName", ci(r, "CompanyName")); o.put("GlAccountId", ci(r, "GlAccountId"));
            if (pay) { o.put("AdvanceGlAcId", ci(r, "AdvanceGlAcId")); o.put("AccountTitleAdvance", ci(r, "AccountTitleAdvance")); }
            out.add(o);
        }
        return out;
    }

    /** CoaAllocationGetAllServiceBind -> COAAllocationSearch; the form filters it by AccountTypeId on the client side. */
    private List<Map<String, Object>> coa() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : support.coaAllocationSearch()) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id")); o.put("AccountTitle", ci(r, "AccountTitle")); o.put("AccountTypeId", ci(r, "AccountTypeId"));
            out.add(o);
        }
        return out;
    }

    /** BindTaxTypes: TaxesTypes.GetForComboBind(Type = 1). */
    private List<Map<String, Object>> taxTypes() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows("Sp_TaxesTypes_GetAllMethod", "OrganizationId", org(), "CompanyId", comp(), "Type", 1, "Activity", "ReadByCombo")) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id")); o.put("TaxName", ci(r, "TaxName"));
            out.add(o);
        }
        return out;
    }

    /** VoucherNofill: GenerateCode. Receipt sends DocumentTypeId 0 and FinancialYearId 0 (the form fills Id = 13 instead). */
    public int generateCode(Kind k) {
        List<Map<String, Object>> r = k == Kind.PAYMENT
                ? rows("Sp_PaymentByInvoiceHeader_GetAllMethod", "OrganizationId", org(), "CompanyId", comp(), "DocumentTypeId", 14, "Activity", "GenerateCode")
                : rows("Sp_ReceiptByInvoiceHeader_GetAllMethod", "OrganizationId", org(), "CompanyId", comp(), "FinancialYearId", 0, "DocumentTypeId", 0, "Activity", "GenerateCode");
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "DocNo"));
    }

    // ------------------------------------------------------------------------------------------------ events

    /** AccountCurrentBalance / CustomerCurrentBalance: ReadByCurrentBalanceByDateAndAccountId at now; null when no row. */
    public Object balance(int accountId) {
        List<Map<String, Object>> r = support.currentBalance(DesktopVoucherSupport.now(), accountId);
        return r.isEmpty() ? null : ci(r.get(0), "Balance");
    }

    /** cmbsupcust_Leave grid: Sp_PaymentByInvoiceHeader_GetReceivablesByInvoice for both kinds (see the class comment). */
    public List<Map<String, Object>> invoices(int supplierId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows("Sp_PaymentByInvoiceHeader_GetReceivablesByInvoice", "OrganizationId", org(), "CompanyId", comp(), "SupplierCustomerId", supplierId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "Id"));
            o.put("DocNo", ci(r, "VoucherCode"));
            o.put("DocDate", dayStrict(ci(r, "VoucherDate")));
            o.put("InvoiceNo", ci(r, "DocumentTypeSrNo"));
            o.put("DocumentType", ci(r, "DocumentTypeCode"));
            o.put("DocumentTypeId", ci(r, "DocumentTypeId"));
            o.put("DueDate", dayStrict(ci(r, "DueDate")));
            o.put("ManualBillNo", ci(r, "ManualBillNo"));
            o.put("AdvanceAmount", toDouble(ci(r, "AdvanceAmount")));
            o.put("BillAmount", toDouble(ci(r, "BillAmount")));
            o.put("TotalPaid", toDouble(ci(r, "ReceivedPaidTotal")));
            o.put("Balance", toDouble(ci(r, "BalanceAmount")));
            o.put("Payment", 0d);
            o.put("Wht", toDouble(ci(r, "DWhtAmount")));
            o.put("Adjustment", toDouble(ci(r, "DAdjustmentAmount")));
            out.add(o);
        }
        return out;
    }

    /** CmbTranType_Leave WhtHolding / CmbTaxType_Leave: TaxScheduleMain.ReadTaxSchedule (Sp_TaxSchedule_GetAllMehtod GetTaxPercentInTaxSchedule). */
    public Map<String, Object> taxSchedule(int taxTypeId, String date) {
        List<Map<String, Object>> r = rows("Sp_TaxSchedule_GetAllMehtod", "OrganizationId", org(), "CompanyId", comp(),
                "EffectedDate", isoDay(date), "TaxNameId", taxTypeId, "Activity", "GetTaxPercentInTaxSchedule");
        Map<String, Object> o = new LinkedHashMap<>();
        if (r.isEmpty()) { o.put("found", false); return o; }
        o.put("found", true);
        o.put("TaxPercent", ci(r.get(0), "TaxPercent")); o.put("TaxGLAccountId", ci(r.get(0), "TaxGLAccountId")); o.put("AccountTitle", ci(r.get(0), "AccountTitle"));
        return o;
    }

    /** cmbcashbankac_Leave Bank: configuration "CheqBook Enabled", then CheqBookHeader.OutstandingCheqNo(BankId = the account). */
    public Map<String, Object> cheques(int bankId) {
        Map<String, Object> o = new LinkedHashMap<>();
        String cfg = support.config("CheqBook Enabled");
        if (cfg == null) { o.put("message", "CheqBook Enabled Configuration not found"); return o; }
        String t = cfg.trim();
        boolean on;
        if ("true".equalsIgnoreCase(t)) on = true;
        else if ("false".equalsIgnoreCase(t)) on = false;
        else throw new IllegalStateException("String was not recognized as a valid Boolean.");
        if (!on) { o.put("message", "CheqBook Enabled Configuration False"); return o; }
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : support.outstandingCheques(bankId, 0)) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", ci(r, "Id")); x.put("CheqNo", ci(r, "CheqNo"));
            out.add(x);
        }
        o.put("rows", out);
        return o;
    }

    /** PdcInventory.GetByType (Sp_PdcInventory_GetAllMethod ReadByType): AccountId 0, DebitAcId = the credit account, CheqId only when != 0. */
    public List<Map<String, Object>> pdc(int debitAcId, int cheqId) {
        Map<String, Object> p = DesktopProc.params("OrganizationId", org(), "CompanyId", comp(), "AccountId", 0, "DebitAcId", debitAcId,
                "CheqId", cheqId != 0 ? cheqId : null, "Activity", "ReadByType");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_PdcInventory_GetAllMethod", p)) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", ci(r, "Id")); x.put("CheqNo", ci(r, "CheqNo")); x.put("CheqDate", day(ci(r, "CheqDate"))); x.put("CheqAmount", ci(r, "CheqAmount"));
            out.add(x);
        }
        return out;
    }

    /** cmbcashbankac_Leave Advance: Sp_{Payment|Receipt}ByInvoiceAdvanceBalance(AccountId = the credit account); BalanceAdvance is Conversion.ToInt. */
    public int advanceBalance(Kind k, int accountId) {
        List<Map<String, Object>> r = rows(k == Kind.PAYMENT ? "Sp_PaymentByInvoiceAdvanceBalance" : "Sp_ReceiptByInvoiceAdvanceBalance",
                "OrganizationId", org(), "CompanyId", comp(), "AccountId", accountId);
        return r.isEmpty() ? 0 : toIntRounded(toDouble(ci(r.get(0), "BalanceAdvance")));
    }

    // ------------------------------------------------------------------------------------------------ read

    /** txtdocno_Leave: GetIdByDocNo. */
    public int idByDocNo(Kind k, int docNo) {
        List<Map<String, Object>> r = rows(k == Kind.PAYMENT ? "Sp_PaymentByInvoiceHeader_GetAllMethod" : "Sp_ReceiptByInvoiceHeader_GetAllMethod",
                "OrganizationId", org(), "CompanyId", comp(), "DocumentTypeId", k.docType, "DocNo", docNo, "Activity", "GetIdByDocNo");
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "Id"));
    }

    /** CommonServices.VoucherHeadIdGet(id, DocumentTypeId): Sp_Vouchers_GetMethods GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId. */
    public int voucherHeadId(Kind k, int id) {
        List<Map<String, Object>> r = rows("Sp_Vouchers_GetMethods", "Activity", "GetVoucherHeadIdByReferenceDocumentTypeIdandRefEntryId",
                "OrganizationId", org(), "CompanyId", comp(), "DocumentTypeId", k.docType, "DocumentTypeSrNo", id);
        return r.isEmpty() ? 0 : toInt(ci(r.get(0), "Id"));
    }

    private String procGetAll(Kind k) { return k == Kind.PAYMENT ? "Sp_PaymentByInvoiceHeader_GetAllMethod" : "Sp_ReceiptByInvoiceHeader_GetAllMethod"; }

    /** ReadById / HistoryGridFill(int): GetByID (header, ReadByIdDetail, ReadByIdOtherDetail) and VoucherhHeadIdGet. */
    public Map<String, Object> read(Kind k, int id) {
        List<Map<String, Object>> h = rows(procGetAll(k), "Id", id, "Activity", "ReadByID");
        if (h.isEmpty() || toInt(ci(h.get(0), "OrganizationId")) != org() || toInt(ci(h.get(0), "CompanyId")) != comp()) throw new IllegalStateException(OUT_OF_RANGE);
        Map<String, Object> hd = h.get(0);
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("id", id);
        o.put("docDate", isoDay(ci(hd, "DocDate")));
        o.put("supplierId", toInt(ci(hd, "SupplierCustomerId")));
        o.put("docNo", toInt(ci(hd, "DocNo")));
        List<Map<String, Object>> grid = new ArrayList<>();
        for (Map<String, Object> r : rows(procGetAll(k), "Id", id, "Activity", "ReadByIdDetail")) {
            Map<String, Object> g = new LinkedHashMap<>();
            g.put("Id", ci(r, "Id"));
            g.put("DocNo", ci(r, "InvoiceNo"));
            g.put("DocDate", dayStrict(ci(r, "InvoiceDate")));
            g.put("InvoiceNo", ci(r, "RefDocNoId"));
            g.put("DocumentType", ci(r, "DocumentTypeCode"));
            g.put("DocumentTypeId", ci(r, "RefDocumentTypeId"));
            g.put("DueDate", dayStrict(ci(r, "DueDate")));
            g.put("ManualBillNo", ci(r, "MannualBillNo"));
            g.put("AdvanceAmount", toDouble(ci(r, "AdvanceAmount")));
            g.put("BillAmount", toDouble(ci(r, "BillAmount")));
            g.put("TotalPaid", toDouble(ci(r, k == Kind.PAYMENT ? "TotalPaidAmount" : "TotalReceivedAmount")));
            g.put("Balance", toDouble(ci(r, "BalanceAmount")));
            g.put("Payment", toDouble(ci(r, "Amount")));
            g.put("Wht", toDouble(ci(r, "DWhtAmount")));
            g.put("Adjustment", toDouble(ci(r, "DAdjustmentAmount")));
            grid.add(g);
        }
        o.put("grid", grid);
        List<Map<String, Object>> det = new ArrayList<>();
        for (Map<String, Object> r : rows(procGetAll(k), "Id", id, "Activity", "ReadByIdOtherDetail")) {
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("TransType", ci(r, "TransType"));
            d.put("GLAccountId", ci(r, "GLAccountId"));
            d.put("AccountTitle", ci(r, "AccountTitle"));
            d.put("Remarks", ci(r, "Remarks"));
            d.put("Amount", toDouble(ci(r, "Amount")));
            d.put("CheqId", ci(r, "CheqId"));
            d.put("CheqNo", ci(r, "CheqNo"));
            d.put("CheqDate", day(ci(r, "CheqDate")));
            d.put("Paytitle", ci(r, "PayTitle"));
            det.add(d);
        }
        o.put("details", det);
        o.put("voucherHeadId", voucherHeadId(k, id));
        return o;
    }

    // ------------------------------------------------------------------------------------------------ history

    /**
     * HistoryGridFill: VoucherHead.GetReceivablesByInvoice (Payment, config PaymentByInvoicePurchaseInvoice, the BLL method does not send it)
     * or GetReceivablesByInvoiceReceipt (Receipt, config ReceiptsByInvoiceSalesInvoice, sent when not empty). Dates are always sent,
     * FromDocNo / ToDocNo only when not 0. The grid takes the procedure's columns as they come.
     */
    public List<Map<String, Object>> history(Kind k, Map<String, Object> f) {
        String cfg = support.config(k.config);
        String saleIds = cfg == null ? "" : cfg;
        Map<String, Object> p = DesktopProc.params("OrganizationId", org(), "CompanyId", comp(), "SupplierCustomerId", toInt(f.get("supplierId")),
                "FromDate", isoDay(f.get("billFrom")), "ToDate", isoDay(f.get("billTo")),
                "DueDateFrom", isoDay(f.get("dueFrom")), "DueDateTo", isoDay(f.get("dueTo")),
                "FromDocNo", toInt(f.get("invFrom")) != 0 ? toInt(f.get("invFrom")) : null,
                "ToDocNo", toInt(f.get("invTo")) != 0 ? toInt(f.get("invTo")) : null);
        if (k == Kind.RECEIPT && !saleIds.isEmpty()) p.put("SaleInvoiceDocumentTypeIds", saleIds);
        List<Map<String, Object>> r = DesktopProc.rows(jdbc, k == Kind.PAYMENT ? "Sp_PaymentByInvoiceHeader_GetReceivablesByInvoice"
                : "Sp_ReceiptByInvoiceHeader_GetReceivablesByInvoice", p);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> x : r) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (Map.Entry<String, Object> e : x.entrySet()) {
                Object v = e.getValue();
                String n = e.getKey().toLowerCase();
                if (v instanceof java.util.Date || v instanceof java.time.temporal.TemporalAccessor) v = day(v);
                else if (v != null && (n.endsWith("date") && v.toString().matches("\\d{4}-\\d{2}-\\d{2}.*"))) v = day(v);
                o.put(e.getKey(), v);
            }
            out.add(o);
        }
        return out;
    }

    // ------------------------------------------------------------------------------------------------ save

    /**
     * Insert(): Formvalidation, GrandTotal and the equality checks in the form's order, row lists, Save (BLL MakeVoucher + DAL SetDate in one transaction).
     * The page posts ALL invoice grid rows (the totals of Advance and Wht are grid totals of every row) with a checked flag; only Payment, Wht and the
     * flag come from the page for an invoice, everything else is re-read (open invoices of the supplier, or the saved detail when updating).
     * Returns {saved, message}; saved=false with an empty message is the form's silent return.
     */
    @Transactional
    @SuppressWarnings("unchecked")
    public Map<String, Object> save(Kind k, Map<String, Object> b) {
        int recId = toInt(b.get("id"));
        support.requireRight(k.screenName, recId > 0 ? "update" : "save", "You do not have the right to " + (recId > 0 ? "update" : "save") + " this voucher.");
        int supplierId = toInt(b.get("supplierId"));
        List<Map<String, Object>> sup = suppliers(k);
        Map<String, Object> supplier = null;
        for (Map<String, Object> s : sup) if (toInt(s.get("Id")) == supplierId) { supplier = s; break; }
        if (supplierId == 0 || supplier == null) throw new IllegalArgumentException("Please Select Supplier");
        String docNoText = b.get("docNo") == null ? "" : b.get("docNo").toString().trim();
        if (docNoText.isEmpty() || docNoText.equals("0")) throw new IllegalArgumentException("DocNo field required");

        List<Map<String, Object>> grid = (List<Map<String, Object>>) b.get("grid");
        List<Map<String, Object>> others = (List<Map<String, Object>>) b.get("details");
        if (grid == null) grid = new ArrayList<>();
        if (others == null) others = new ArrayList<>();

        /* what each posted invoice row is allowed to be */
        Map<String, Map<String, Object>> baseline = new HashMap<>();
        if (recId > 0) {
            Map<String, Object> saved = read(k, recId);
            for (Map<String, Object> g : (List<Map<String, Object>>) saved.get("grid")) baseline.put(toInt(g.get("InvoiceNo")) + "|" + toInt(g.get("DocumentTypeId")), g);
            if (toInt(saved.get("supplierId")) != supplierId) throw new IllegalArgumentException("Please Select Supplier");
        } else {
            for (Map<String, Object> g : invoices(supplierId)) baseline.put(toInt(g.get("InvoiceNo")) + "|" + toInt(g.get("DocumentTypeId")), g);
        }
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> g : grid) {
            Map<String, Object> base = baseline.get(toInt(g.get("InvoiceNo")) + "|" + toInt(g.get("DocumentTypeId")));
            if (base == null) throw new IllegalArgumentException("Please Checked Record For Save");
            Map<String, Object> r = new LinkedHashMap<>(base);
            r.put("Payment", toDouble(g.get("Payment")));
            r.put("Wht", toDouble(g.get("Wht")));
            r.put("checked", bool(g.get("checked")));
            if (bool(g.get("checked")) && toDouble(r.get("Payment")) > toDouble(r.get("Balance")))
                throw new IllegalArgumentException("Payment Amount Is Larger Than Balance Amount");
            rows.add(r);
        }

        /* GrandTotal: checked rows Payment + Wht + Adjustment; detail amounts */
        double tp = 0, tw = 0, ta = 0, grand = 0;
        if (!rows.isEmpty()) {
            for (Map<String, Object> r : rows) if (Boolean.TRUE.equals(r.get("checked"))) { tp += toDouble(r.get("Payment")); tw += toDouble(r.get("Wht")); ta += toDouble(r.get("Adjustment")); }
            grand = tp + tw + ta;
        }
        double detailAmount = 0;
        for (Map<String, Object> o : others) detailAmount += toDouble(o.get("Amount"));
        double diff = grand - detailAmount;
        if (diff > 0.0 || diff < 0.0) throw new IllegalArgumentException("Please Clear The Difference Amount");
        if (detailAmount != grand) throw new IllegalStateException("Please Check Your Payment & Total Amount Is Not Equal");
        double adv = 0, wht = 0;
        for (Map<String, Object> o : others) {
            String t = o.get("TransType") == null ? "" : o.get("TransType").toString();
            if (t.equals("Advance")) adv += toDouble(o.get("Amount"));
            if (t.equals("WhtHolding")) wht += toDouble(o.get("Amount"));
        }
        double advTotal = 0, whtTotal = 0;
        for (Map<String, Object> r : rows) { advTotal += toDouble(r.get("AdvanceAmount")); whtTotal += toDouble(r.get("Wht")); }
        if (adv != advTotal) throw new IllegalStateException("Please Check Your Advance Amount Is Not Equal");
        if (whtTotal != wht) throw new IllegalStateException("Please Check Your WhtHolding Amount Is Not Equal");

        String docDate = isoDay(b.get("docDate"));
        if (docDate == null) throw new IllegalArgumentException("Payment Date is required");
        int docNo = toInt(docNoText);
        int user = ctx.currentUserId();
        String now = DesktopVoucherSupport.now();

        /* ReceiptByInvoiceOtherDetail / PaymentByInvoiceOtherDetail rows */
        List<Map<String, Object>> od = new ArrayList<>();
        Set<String> types = new HashSet<>(Arrays.asList("PartyName", "Cash", "Bank", "WhtHolding", "Advance", "PdcCheq"));
        for (Map<String, Object> o : others) {
            String t = o.get("TransType") == null ? "" : o.get("TransType").toString();
            if (!types.contains(t) && !(k == Kind.RECEIPT && t.matches("\\d+"))) throw new IllegalArgumentException("TransType field required");
            int gl = toInt(o.get("GLAccountId"));
            if (!ownsAccount(gl)) throw new IllegalArgumentException("Credit Account field required");
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("TransType", t);
            d.put("GLAccountId", gl);
            d.put("Remarks", o.get("Remarks") == null ? "" : o.get("Remarks").toString());
            d.put("Amount", toDouble(o.get("Amount")));
            d.put("CheqId", toInt(o.get("CheqId")));
            d.put("CheqNo", o.get("CheqNo") == null ? "" : o.get("CheqNo").toString());
            d.put("CheqDate", dateOr1900(o.get("CheqDate")));
            d.put("PayTitle", o.get("Paytitle") == null ? "" : o.get("Paytitle").toString());
            od.add(d);
        }

        if (rows.isEmpty()) return result(false, "");                         /* grd.RowCount <= 0: return */
        List<Map<String, Object>> chosen = new ArrayList<>();
        for (Map<String, Object> r : rows) if (Boolean.TRUE.equals(r.get("checked"))) chosen.add(r);
        if (chosen.isEmpty()) return result(false, "Please Checked Record For Save");

        /* BLL MakeVoucher (Payment, DocumentTypeId 14) - before anything is written */
        ContraVoucherDto.Head vh = null;
        List<ContraVoucherDto.Detail> vds = new ArrayList<>();
        if (k == Kind.PAYMENT) {
            int ref = toInt(supplier.get("GlAccountId"));
            int yearId = ctx.currentFinancialYearId();
            if (yearId <= 0) throw new IllegalStateException("Financial Year Not Found Please Check");
            vh = new ContraVoucherDto.Head();
            vh.FinancialYearId = yearId;
            vh.EntryDate = now; vh.EntryUser = user;
            vh.ModifyDate = LocalDate.now().toString(); vh.ModifyUser = user;
            vh.OrganizationId = org(); vh.CompanyId = comp();
            vh.VoucherDate = docDate; vh.VoucherCode = docNo; vh.DocumentTypeId = k.docType;
            vh.RefAccountId = ref; vh.AgainstAccountId = ref;
            double credit = 0;
            for (Map<String, Object> d : od) {
                double amt = (Double) d.get("Amount");
                if (toIntRounded(amt) <= 0) continue;
                ContraVoucherDto.Detail c = new ContraVoucherDto.Detail();
                c.AccountId = (Integer) d.get("GLAccountId");
                c.AgainstAccountId = ref;
                c.Comments = (String) d.get("Remarks");
                c.DebitAmount = 0d;
                c.CreditAmount = amt;
                credit += c.CreditAmount;
                if ("Advance".equals(d.get("TransType"))) { c.AdvanceAmount = amt; c.PaymentType = "Advance"; }
                c.InvoiceNoRefId = (Integer) d.get("CheqId");
                c.DCheqDate = (String) d.get("CheqDate");
                c.CheqNoDetail = (String) d.get("CheqNo");
                vds.add(c);
                ContraVoucherDto.Detail dr = new ContraVoucherDto.Detail();
                dr.AccountId = ref;
                dr.AgainstAccountId = (Integer) d.get("GLAccountId");
                dr.Comments = (String) d.get("Remarks");
                dr.CreditAmount = 0d;
                dr.DebitAmount = amt;
                vds.add(dr);
            }
            vh.VoucherAmount = credit;
        }

        /* DAL SetDate (one transaction) */
        int num3;
        if (k == Kind.PAYMENT) {
            num3 = DesktopProc.setProc(jdbc, recId == 0 ? "Sp_PaymentByInvoiceHeader_Insert" : "Sp_PaymentByInvoiceHeader_Update", DesktopProc.params(
                    "Id", recId, "DocDate", docDate, "DocumentTypeId", 14, "DocNo", docNo, "SupplierCustomerId", supplierId,
                    "OrganizationId", org(), "CompanyId", comp(), "EntryUser", user, "EntryDate", now, "ModifyUser", user, "ModifyDate", now));
        } else {
            num3 = DesktopProc.setProc(jdbc, recId == 0 ? "Sp_ReceiptByInvoiceHeader_Insert" : "Sp_ReceiptByInvoiceHeader_Update", DesktopProc.params(
                    "Id", recId, "DocDate", docDate, "DocumentTypeId", 13, "DocNo", docNo, "SupplierCustomerId", supplierId,
                    "OrganizationId", org(), "CompanyId", comp(), "EntryUser", user, "EntryDate", now, "ModifyUser", user, "ModifyDate", now,
                    "TaxTypeId", 0, "TaxCreditAcId", 0, "TaxDebitAcId", 0, "TaxPrct", 0d, "TaxAmount", 0d, "FinancialYearId", 0));
        }
        int hid = num3 > 0 ? num3 : recId;
        String hdrCol = k == Kind.PAYMENT ? "PaymentByInvoiceHeaderId" : "ReceiptByInvoiceHeaderId";
        for (Map<String, Object> r : chosen) {
            Map<String, Object> p = DesktopProc.params("Id", 0, hdrCol, hid,
                    "InvoiceDate", dateOr1900(r.get("DocDate")), "InvoiceNo", toInt(r.get("DocNo")),
                    "RefDocumentTypeId", toInt(r.get("DocumentTypeId")), "RefDocNoId", toInt(r.get("InvoiceNo")),
                    "DueDate", dateOr1900(r.get("DueDate")), "MannualBillNo", r.get("ManualBillNo") == null ? "" : r.get("ManualBillNo").toString(),
                    "AdvanceAmount", single(r.get("AdvanceAmount")), "BillAmount", single(r.get("BillAmount")));
            p.put(k == Kind.PAYMENT ? "TotalPaidAmount" : "TotalReceivedAmount", single(r.get("TotalPaid")));
            p.put("BalanceAmount", single(r.get("Balance")));
            p.put("Amount", single(r.get("Payment")));
            p.put("DWhtAmount", (double) toIntRounded(toDouble(r.get("Wht"))));
            p.put("DAdjustmentAmount", single(r.get("Adjustment")));
            if (k == Kind.RECEIPT) p.put("PaymentTermsId", 0d);
            DesktopProc.setProc(jdbc, k == Kind.PAYMENT ? "Sp_PaymentByInvoiceDetail_Insert" : "Sp_ReceiptByInvoiceDetail_Insert", p);
        }
        for (Map<String, Object> d : od) {
            Map<String, Object> p = DesktopProc.params("Id", 0, hdrCol, hid);
            p.put("TransType", k == Kind.PAYMENT ? d.get("TransType") : (Object) 0);
            p.put("GLAccountId", d.get("GLAccountId"));
            p.put("Remarks", d.get("Remarks"));
            p.put("CheqId", d.get("CheqId"));
            p.put("CheqNo", d.get("CheqNo"));
            p.put("CheqDate", d.get("CheqDate"));
            p.put("PayTitle", d.get("PayTitle"));
            p.put("Amount", d.get("Amount"));
            DesktopProc.setProc(jdbc, k == Kind.PAYMENT ? "Sp_PaymentByInvoiceOtherDetail_Insert" : "Sp_ReceiptByInvoiceOtherDetail_Insert", p);
        }
        int existing = voucherHeadId(k, hid);
        if (k == Kind.RECEIPT) {
            /* the DAL now sends the empty VoucherHead: EntryDate 0001-01-01 is refused by SQL Server and everything above is rolled back */
            throw new IllegalStateException(SQL_OVERFLOW);
        }
        if (existing != 0) vh.Id = existing;
        vh.DocumentTypeSrNo = hid;
        int num2 = DesktopProc.setProc(jdbc, existing == 0 ? "Sp_VoucherHead_Insert" : "Sp_VoucherHead_Update", DesktopVoucherSupport.fields(vh));
        if (num2 > 0) vh.Id = num2;
        for (ContraVoucherDto.Detail d : vds) {
            d.VoucherHeadId = vh.Id;
            DesktopProc.setProc(jdbc, "Sp_VoucherDetail_Insert", DesktopVoucherSupport.fields(d));
        }
        int refId = DesktopProc.setProc(jdbc, "Sp_VoucherHead_H_Insert", DesktopVoucherSupport.fields(vh));
        for (ContraVoucherDto.Detail d : vds) {
            d.VoucherHeadId = vh.Id;
            d.DocumentTypeIdRef = refId;
            DesktopProc.setProc(jdbc, "Sp_VoucherDetail_H_Insert", DesktopVoucherSupport.fields(d));
        }
        return result(true, recId == 0 ? "Record Save Successfully" : "Record Update Successfully");
    }

    private static Map<String, Object> result(boolean saved, String message) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("saved", saved); m.put("message", message);
        return m;
    }

    /** print_Click: the voucher head of the record read last (VoucherHeadIdGet). The page opens acc-102 with it and the kind's DocumentTypeId. */
    public Map<String, Object> printTarget(Kind k, int id) {
        support.requireRight(k.screenName, "print", "You do not have the right to print this voucher.");
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("voucherHeadId", id > 0 ? voucherHeadId(k, id) : 0);
        m.put("documentTypeId", k.docType);
        return m;
    }
}
