package com.mst.services;

import com.mst.repositories.support.DesktopProc;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.jdbc.core.JdbcTemplate;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.math.BigDecimal;
import java.math.MathContext;
import java.sql.Timestamp;
import java.text.SimpleDateFormat;
import java.util.*;

/**
 * Screen 4 "Advance Adjustment" = Architecture.WinApp.Account_Definition.frmAdvanceAdjustment.
 *
 *  Load      AccountsComboBind  CommonServices.CoaAllocationGetAllServiceBind -> COAAllocation.GetAll
 *                                 Sp_COAAllocation_GetAllMethod @OrganizationId @CompanyId @UserId (when != 0) @Activity='COAAllocationSearch';
 *                                 rows whose AccountTypeId is not 2, 11 or 15 (hidden Account / WithHoldingAc combos, never selected)
 *            SupplierCustomerFill  CommonServices.SupplierCustomerGetAllServiceBind -> SupplierCustomer.Getall
 *                                 Sp_SupplierCustomer_GetAllMethod @OrganizationId @CompanyId @Activity='ReadByOrganizationCompanyId';
 *                                 value GlAccountId, text CompanyName
 *            GenerateDocNo        VouchersAdvanceAdjustmentByInvoiceHeader.GenerateCode ->
 *                                 Sp_VouchersAdvanceAdjustmentByInvoiceHeader_GetAllMethod @OrganizationId @CompanyId @Activity='GenerateCode'
 *  cmbSupplier_Leave
 *            GetInvoiceNoByPaymentByInvoice  VoucherHead.GetInvoiceNoByPaymentByInvoice -> Sp_Vouchers_GetMethods
 *                                 @OrganizationId @CompanyId @FinancialYearId @RefAccountId (= supplier GlAccountId, sent even when 0)
 *                                 @Activity='GetInvoiceNoByPaymentByInvoice'  (@Id only when != 0: the form never sets it)
 *            GetAdvanceAmountByAccountId     VoucherHead.GetAdvanceAmountByAccountId -> Sp_Vouchers_GetMethods
 *                                 @Activity='GetAdvanceAmountByAccountId' @OrganizationId @CompanyId @FinancialYearId @RefAccountId
 *  btnSave_Click -> VouchersAdvanceAdjustmentByInvoiceHeader.Save -> DAL.SetDate(obj, Sp_..._Insert), one transaction:
 *            Sp_VouchersAdvanceAdjustmentByInvoiceHeader_Insert (returns SCOPE_IDENTITY), then
 *            Sp_VouchersAdvanceAdjustedAccountsDetail_Insert per advance-grid row, then
 *            Sp_VouchersAdvanceAdjustedInvoicesDetail_Insert per invoice-grid row. Any error rolls everything back.
 * Tenancy (organization, company, user, financial year) always comes from CurrentUserContext. The browser posts only the values
 * the user typed; the invoice Balance and the advance Available figures are re-read from the same procedures and used for the save.
 */
@Service
public class AdvanceAdjustmentService {
    private static final String HEADER_PROC = "Sp_VouchersAdvanceAdjustmentByInvoiceHeader_GetAllMethod";

    /** A desktop MessageBox the user must answer before anything is saved. */
    public static class Warning extends RuntimeException {
        public Warning(String m) { super(m); }
    }

    @Autowired private JdbcTemplate jdbc;
    @Autowired private CurrentUserContext ctx;

    // ------------------------------------------------------------------ Conversion helpers (Architecture.Common.Conversion)

    private static Object ci(Map<String, Object> r, String key) {
        if (r.containsKey(key)) return r.get(key);
        for (Map.Entry<String, Object> e : r.entrySet()) if (e.getKey().equalsIgnoreCase(key)) return e.getValue();
        return null;
    }

    /** Conversion.ToInt(string): Convert.ToInt32(text) - a decimal text such as "1234.5" is a FormatException, i.e. 0. */
    static int toIntText(String s) {
        if (s == null) return 0;
        String t = s.trim();
        if (t.isEmpty()) return 0;
        try { return Integer.parseInt(t.startsWith("+") ? t.substring(1) : t); } catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToInt(double): Convert.ToInt32(double) rounds half to even; out of range throws -> 0. */
    static int toIntNumber(double d) {
        if (Double.isNaN(d) || d >= 2147483647.5 || d <= -2147483648.5) return 0;
        return (int) Math.rint(d);
    }

    /** Conversion.ToDouble(string) (Convert.ToDouble accepts thousands separators); anything else is 0. */
    static double toDoubleText(String s) {
        if (s == null || s.trim().isEmpty()) return 0.0;
        try {
            double d = Double.parseDouble(s.trim().replace(",", ""));
            return Double.isInfinite(d) || Double.isNaN(d) ? 0.0 : d;
        } catch (NumberFormatException e) { return 0.0; }
    }

    static double toDouble(Object o) {
        if (o == null) return 0.0;
        if (o instanceof Number) { double d = ((Number) o).doubleValue(); return Double.isInfinite(d) || Double.isNaN(d) ? 0.0 : d; }
        return toDoubleText(o.toString());
    }

    /** GetTotal(...).ToString() then Conversion.ToDouble: the sum as a 15-significant-digit .NET string. */
    static double r15(double d) { return new BigDecimal(d).round(new MathContext(15)).doubleValue(); }

    private static String dmy(Object o) {
        Date d = null;
        if (o instanceof Timestamp) d = new Date(((Timestamp) o).getTime());
        else if (o instanceof java.sql.Date) d = new Date(((java.sql.Date) o).getTime());
        else if (o instanceof Date) d = (Date) o;
        else if (o != null && !o.toString().isEmpty()) {
            try { d = new SimpleDateFormat("yyyy-MM-dd").parse(o.toString().substring(0, Math.min(10, o.toString().length()))); } catch (Exception e) { d = null; }
        }
        if (d == null) { Calendar c = new GregorianCalendar(1900, Calendar.JANUARY, 1); d = c.getTime(); }   // Conversion.ToDateTime(null)
        return new SimpleDateFormat("dd-MMM-yyyy", Locale.ENGLISH).format(d);
    }

    // ------------------------------------------------------------------ Load

    public Map<String, Object> load() {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("accounts", accounts());
        m.put("suppliers", suppliers());
        m.put("docNo", generateDocNo());
        return m;
    }

    private List<Map<String, Object>> accounts() {
        Map<String, Object> p = DesktopProc.params("OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "UserId", ctx.currentUserId() == 0 ? null : ctx.currentUserId(), "Activity", "COAAllocationSearch");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_COAAllocation_GetAllMethod", p)) {
            int t = (int) toDouble(ci(r, "AccountTypeId"));
            if (t != 2 && t != 11 && t != 15) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", ci(r, "Id"));
                o.put("AccountTitle", ci(r, "AccountTitle"));
                out.add(o);
            }
        }
        return out;
    }

    private List<Map<String, Object>> suppliers() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : DesktopProc.rows(jdbc, "Sp_SupplierCustomer_GetAllMethod", DesktopProc.params(
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(), "Activity", "ReadByOrganizationCompanyId"))) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", ci(r, "GlAccountId"));
            o.put("Name", ci(r, "CompanyName"));
            out.add(o);
        }
        return out;
    }

    /** GenerateDocNo: the text box is only set when the code is > 0. */
    public Integer generateDocNo() {
        List<Map<String, Object>> rows = DesktopProc.rows(jdbc, HEADER_PROC, DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(), "Activity", "GenerateCode"));
        int n = rows.isEmpty() ? 0 : (int) toDouble(ci(rows.get(0), "DocNo"));
        return n > 0 ? n : null;
    }

    // ------------------------------------------------------------------ cmbSupplier_Leave

    private List<Map<String, Object>> invoiceRows(int accountId) {
        return DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", DesktopProc.params("OrganizationId", ctx.currentOrganizationId(),
                "CompanyId", ctx.currentCompanyId(), "FinancialYearId", ctx.currentFinancialYearId(), "RefAccountId", accountId,
                "Activity", "GetInvoiceNoByPaymentByInvoice"));
    }

    private List<Map<String, Object>> advanceRows(int accountId) {
        return DesktopProc.rows(jdbc, "Sp_Vouchers_GetMethods", DesktopProc.params("Activity", "GetAdvanceAmountByAccountId",
                "OrganizationId", ctx.currentOrganizationId(), "CompanyId", ctx.currentCompanyId(),
                "FinancialYearId", ctx.currentFinancialYearId(), "RefAccountId", accountId));
    }

    /** The two DataTables the Leave handler binds: grdInvoices and grdAdvancesAdjustment (columns and order as the desktop builds them). */
    public Map<String, Object> supplierGrids(int accountId) {
        List<Map<String, Object>> invoices = new ArrayList<>();
        for (Map<String, Object> r : invoiceRows(accountId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("RefDocumentTypeId", ci(r, "DocumentTypeId"));
            o.put("RefDocNoId", ci(r, "Id"));
            o.put("VoucherDate", dmy(ci(r, "VoucherDate")));
            o.put("DocumentType", ci(r, "DocumentTypeCode"));
            o.put("DueDate", dmy(ci(r, "DueDate")));
            o.put("ManualBillNo", ci(r, "ManualBillNo"));
            o.put("BalanceAmount", toDouble(ci(r, "BillAmount")));
            o.put("PaidAmount", 0.0);
            o.put("Remarks", "");
            invoices.add(o);
        }
        List<Map<String, Object>> advances = new ArrayList<>();
        for (Map<String, Object> r : advanceRows(accountId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("AccountId", ci(r, "AccountId"));
            o.put("AccountTitle", ci(r, "AccountTitle"));
            o.put("AvailableAmount", toDouble(ci(r, "AdvanceAmount")));
            o.put("AdjustmentAmount", 0.0);
            o.put("Remarks", "");
            advances.add(o);
        }
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("invoices", invoices);
        m.put("advances", advances);
        return m;
    }

    // ------------------------------------------------------------------ btnSave_Click

    /**
     * @param docNoText        txtDocNo.Text
     * @param adjustedText     txtAdjustedAmount.Text
     * @param supplierId       cmbSupplier.Value (0 / null when nothing is chosen)
     * @param invoices         one map per grdInvoices row: refDocumentTypeId, refDocNoId, paidAmount, remarks
     * @param advances         one map per grdAdvancesAdjustment row: accountId, adjustmentAmount, remarks
     */
    @Transactional
    public int save(String docDate, String docNoText, String adjustedText, String remarks, Integer supplierId,
                    List<Map<String, Object>> invoices, List<Map<String, Object>> advances) {
        if (docNoText == null || docNoText.isEmpty() || toIntText(docNoText) == 0) throw new Warning("Doc No Field is Required");
        if (supplierId == null || supplierId == 0) throw new Warning("Supplier Field is Required");
        if (adjustedText == null || adjustedText.isEmpty() || toIntText(adjustedText) == 0) throw new Warning("Adjustment Amount Field is Required");
        if (advances == null || advances.isEmpty()) throw new Warning("Please Check Advance Adjustment Grid");
        if (invoices == null || invoices.isEmpty()) throw new Warning("Please Check Invoices Grid");

        /* the grids as the Leave handler built them for this supplier: the supplier must be one of this company's, rows must be its own */
        boolean known = false;
        for (Map<String, Object> s : suppliers()) if ((int) toDouble(s.get("Id")) == supplierId) { known = true; break; }
        if (!known) throw new IllegalArgumentException("Supplier does not belong to the current company");
        Map<String, Double> balance = new HashMap<>();
        for (Map<String, Object> r : invoiceRows(supplierId)) balance.put((int) toDouble(ci(r, "DocumentTypeId")) + ":" + (int) toDouble(ci(r, "Id")), toDouble(ci(r, "BillAmount")));
        Map<Integer, Double> available = new HashMap<>();
        for (Map<String, Object> r : advanceRows(supplierId)) available.put((int) toDouble(ci(r, "AccountId")), toDouble(ci(r, "AdvanceAmount")));

        double adjustmentTotal = 0, availableTotal = 0, paidTotal = 0;
        double[] adj = new double[advances.size()], avail = new double[advances.size()];
        for (int i = 0; i < advances.size(); i++) {
            int acc = (int) toDouble(advances.get(i).get("accountId"));
            if (!available.containsKey(acc)) throw new IllegalArgumentException("Advance account does not belong to the selected supplier");
            adj[i] = toDouble(advances.get(i).get("adjustmentAmount"));
            avail[i] = available.get(acc);
            adjustmentTotal += adj[i];
            availableTotal += avail[i];
        }
        double[] paid = new double[invoices.size()], bal = new double[invoices.size()];
        for (int i = 0; i < invoices.size(); i++) {
            String key = (int) toDouble(invoices.get(i).get("refDocumentTypeId")) + ":" + (int) toDouble(invoices.get(i).get("refDocNoId"));
            if (!balance.containsKey(key)) throw new IllegalArgumentException("Invoice does not belong to the selected supplier");
            paid[i] = toDouble(invoices.get(i).get("paidAmount"));
            bal[i] = balance.get(key);
            paidTotal += paid[i];
        }
        adjustmentTotal = r15(adjustmentTotal); availableTotal = r15(availableTotal); paidTotal = r15(paidTotal);
        if (adjustmentTotal > availableTotal) throw new Warning("Adjustment Amount Should not greater then Available Amount");
        for (int i = 0; i < invoices.size(); i++)
            if (toIntNumber(paid[i]) > toIntNumber(bal[i])) throw new Warning("Please Check Paid Amount! This Should not Greater then Balance Amount");
        if (adjustmentTotal != paidTotal) throw new Warning("Please Check Paid Amount! This Should be Equal To Adjustment Amount");
        for (int i = 0; i < invoices.size(); i++)
            if (!(paid[i] > 0.0)) throw new Warning("Please Check Paid Amount");

        int org = ctx.currentOrganizationId(), comp = ctx.currentCompanyId(), user = ctx.currentUserId();
        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> h = DesktopProc.params("Id", 0, "DocNo", toIntText(docNoText), "DocDate", AccountReportsDesktopService.date(docDate),
                "SupplierCustomerId", supplierId, "AdjustedAmount", toDoubleText(adjustedText), "RemarksHeader", remarks == null ? "" : remarks,
                "WhtTaxAccountId", 0, "GlAccountId", 0, "TaxPrcnt", 0.0, "TaxAmount", 0.0, "OrganizationId", org, "CompanyId", comp,
                "EntryDate", now, "EntryUser", user, "ModifyDate", now, "ModifyUser", user, "PostDate", now, "PostUser", user, "PostState", Boolean.FALSE);
        int id = DesktopProc.setProc(jdbc, "Sp_VouchersAdvanceAdjustmentByInvoiceHeader_Insert", h);
        if (id <= 0) return 0;
        for (int i = 0; i < advances.size(); i++) {
            Map<String, Object> g = advances.get(i);
            DesktopProc.setProc(jdbc, "Sp_VouchersAdvanceAdjustedAccountsDetail_Insert", DesktopProc.params("Id", 0,
                    "VouchersAdvanceAdjustmentByInvoiceId", id, "AccountId", (int) toDouble(g.get("accountId")), "AvailableBalance", avail[i],
                    "AdjustedAmount", adj[i], "RemarksAdjustAccounts", g.get("remarks") == null ? "" : g.get("remarks").toString()));
        }
        for (int i = 0; i < invoices.size(); i++) {
            Map<String, Object> g = invoices.get(i);
            DesktopProc.setProc(jdbc, "Sp_VouchersAdvanceAdjustedInvoicesDetail_Insert", DesktopProc.params("Id", 0,
                    "VouchersAdvanceAdjustmentByInvoiceId", id, "RefDocumentTypeId", (int) toDouble(g.get("refDocumentTypeId")),
                    "RefDocNoId", (int) toDouble(g.get("refDocNoId")), "BalanceAmount", bal[i], "PaidAmount", paid[i],
                    "SubRemarksInvoices", g.get("remarks") == null ? "" : g.get("remarks").toString()));
        }
        return id;
    }
}
