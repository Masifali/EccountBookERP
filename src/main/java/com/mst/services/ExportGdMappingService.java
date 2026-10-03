package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportGdMappingRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.ExportBankGdSupport.*;

/**
 * The BLL side of 952 frmGdBreakUpByInvoiceNew "Goods Declaration (GD) / Bank Invoice & GD Mapping"
 * (Architecture.WinApp.Export) - the newer variant of 881 (FormDocumentTypeId 214 instead of 224,
 * no duty / tax boxes, the second main tab is "Bank Invoice Payment Schedule & GD Mapping" and works
 * on the CUSTOM invoices of usp_GetCustomInvoicesForGdMapping with a Payment Term combo and the
 * invoice's payment schedule grid).
 *
 * tabControlMain: "GD BreakUp" (Form | History) | "Bank Invoice Payment Schedule & GD Mapping" (Form | History).
 * GD BreakUp / Form: New / Save / Update(hidden) / Refresh / Custom Invoice / "FI Opening Balances (Balance Advance Payment)";
 *   Custom Invoice No (+ Invoice Balance), GD No,Date, Bank Invoice No & Date, Bank Name, Due Days, Exch Rate, GD Value,
 *   Freight, FOB Value, FTT %, FTT Amount, Net Amount, Remarks; grdGdBreakUp (X first, 1 frozen).
 * GD BreakUp / History: New / Refresh; From/To Date, Bank Name, Show; grdGDhistory (BreakUp button, Edit with the Update right).
 * Mapping / Form: New / Refresh(empty) / Save / Update(hidden) / FI Opening; Invoice No, GD No, GD Value, Payment Term,
 *   Financial Instruments No, Balance, Utilize Amount, + / Update / Cancel; grdAdvanceUtlize (X) and grdpaymentdetail.
 * Mapping / History: Invoice No, From Date, To Date, Show; grdAdvanceHistory (double-click -> Form).
 *
 * Rights by ScreenDefinition.Id 952 (the desktop looks its rights up by the shared name "frmGdBreakUpByInvoice").
 */
@Service
public class ExportGdMappingService {

    public static final int SCREEN_ID = 952;
    public static final int FORM_DOCUMENT_TYPE_ID = 214;

    @Autowired private ExportGdMappingRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(String action) {
        UserAccount u = currentUserContext.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; }
        catch (AccessDeniedException ex) { return false; }
    }

    // ================================================================= load

    /** InitializeComponentMethod. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        out.put("permissions", perm);
        out.put("config", config(u));
        put(out, "invoices", () -> invoiceRows(repo.bankInvoicesForGdBreakUp(u, 0)));
        put(out, "banks", () -> bankRows(repo.banks(u)));
        put(out, "customInvoices", () -> customInvoiceRows(repo.customInvoicesForGdMapping(u, 0)));
        put(out, "historyBanks", () -> repo.historyBanks(u));
        put(out, "advHistoryInvoices", () -> repo.advanceHistoryInvoices(u));
        return out;
    }

    public Map<String, Object> config(UserAccount u) {
        Map<String, Object> c = new LinkedHashMap<>();
        int days;
        try { days = (int) Double.parseDouble(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")); }
        catch (NumberFormatException e) { days = 0; }
        c.put("defaultDaysToLessFromHistoryFromDate", days);
        c.put("allowOneRowPerInvoiceOnGD", asBool(repo.config(u, "AllowOneRowPerInvoiceOnGD")));
        return c;
    }

    /** BtnRefreshGdBreakUp_Click. */
    public Map<String, Object> refreshGdBreakUp(int recId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("config", config(u));
        out.put("invoices", invoiceRows(repo.bankInvoicesForGdBreakUp(u, recId)));
        out.put("banks", bankRows(repo.banks(u)));
        return out;
    }

    public List<Map<String, Object>> invoices(int recId) { return invoiceRows(repo.bankInvoicesForGdBreakUp(user("View"), recId)); }

    private static List<Map<String, Object>> invoiceRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("BankInvoiceAmount", asDouble(ci(r, "BankInvoiceAmount")));
            out.add(m);
        }
        return out;
    }

    private static List<Map<String, Object>> bankRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (!"Home Country".equals(text(ci(r, "IsHomeland")))) continue;
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("BranchName", text(ci(r, "BranchName")));
            m.put("BankIBANNo", text(ci(r, "BankIBANNo")));
            out.add(m);
        }
        return out;
    }

    // ================================================================= GD BreakUp: read / history

    public Map<String, Object> byInvoice(int invoiceId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.gdBreakUpByInvoiceId(invoiceId)) rows.add(gdRow(d));
        out.put("rows", rows);
        out.put("invoices", invoiceRows(repo.bankInvoicesForGdBreakUp(u, invoiceId)));
        return out;
    }

    /** FillOtherItemDetailFromListCommonForReadById - the 19 dtGdBreakUp columns. */
    private static Map<String, Object> gdRow(Map<String, Object> d) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(d, "Id")));
        m.put("InvoiceId", asInt(ci(d, "ExImInvoiceId")));
        m.put("PartyInvoiceNo", text(ci(d, "InvoiceNo")));
        m.put("GDNo", text(ci(d, "GDNO")));
        m.put("GDDate", iso(ci(d, "GDDate")));
        m.put("InvoiceNo", text(ci(d, "BankInvoiceNo")));
        m.put("InvoiceDate", iso(ci(d, "BankInvoiceDate")));
        m.put("OtherAmount", asDouble(ci(d, "OtherAmount")));
        m.put("BankId", asInt(ci(d, "BankId")));
        m.put("BankName", text(ci(d, "BankName")));
        m.put("DueDays", asInt(ci(d, "DueDays")));
        m.put("ExchangeRate", asDouble(ci(d, "ExchangeRate")));
        m.put("GDValue", asDouble(ci(d, "GDValue")));
        m.put("Freight", asDouble(ci(d, "Freight")));
        m.put("FOBValue", asDouble(ci(d, "FobValue")));
        m.put("FTTPercent", asDouble(ci(d, "CommPercent")));
        m.put("FttAmount", asDouble(ci(d, "CommAmount")));
        m.put("NetAmount", asDouble(ci(d, "NetToBeRealized")));
        m.put("Remarks", text(ci(d, "Remarks")));
        return m;
    }

    public List<Map<String, Object>> gdHistory(String fromDate, String toDate, int bankId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.gdBreakUpHistory(u, sqlDate(fromDate), sqlDate(toDate), bankId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("Id", asInt(ci(r, "Id")));
            m.put("ExImInvoiceId", asInt(ci(r, "ExImInvoiceId")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("GDNO", text(ci(r, "GDNO")));
            m.put("GDDate", iso(ci(r, "GDDate")));
            m.put("BankInvoiceNo", text(ci(r, "BankInvoiceNo")));
            m.put("BankInvoiceDate", iso(ci(r, "BankInvoiceDate")));
            m.put("OtherAmount", asDouble(ci(r, "OtherAmount")));
            m.put("BankName", text(ci(r, "BranchName")));
            m.put("DueDays", asInt(ci(r, "DueDays")));
            m.put("ExchangeRate", asDouble(ci(r, "ExchangeRate")));
            m.put("GDValue", asDouble(ci(r, "GDValue")));
            m.put("Freight", asDouble(ci(r, "Freight")));
            m.put("FobValue", asDouble(ci(r, "FobValue")));
            m.put("FTTPercent", asDouble(ci(r, "CommPercent")));
            m.put("FTTAmount", asDouble(ci(r, "CommAmount")));
            m.put("NetAmount", asDouble(ci(r, "NetToBeRealized")));
            m.put("Remarks", text(ci(r, "Remarks")));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> historyBanks() { return repo.historyBanks(user("View")); }

    // ================================================================= GD BreakUp: save

    /** Insert() of the GD BreakUp tab - as 881, with FormDocumentTypeId 214 and no duty / tax values. */
    public Map<String, Object> saveGdBreakUp(Map<String, Object> body) {
        UserAccount u = user("Save");
        List<Map<String, Object>> rows = list(body.get("rows"));
        List<Map<String, Object>> removed = list(body.get("removed"));
        if (rows.isEmpty()) throw new IllegalArgumentException("GDBreakUp Detail Record Not Found");

        boolean oneRow = asBool(repo.config(u, "AllowOneRowPerInvoiceOnGD"));
        int invoiceId = asInt(rows.get(0).get("InvoiceId"));
        for (Map<String, Object> r : rows) {
            if (asInt(r.get("InvoiceId")) != invoiceId) throw new IllegalArgumentException("You can only add data against one invoice in the grid");
        }
        if (oneRow && rows.size() > 1) throw new IllegalArgumentException("You can only one Row Per invoice in the grid");
        if (oneRow && asDouble(rows.get(0).get("GDValue")) != asDouble(rows.get(0).get("OtherAmount")))
            throw new IllegalArgumentException("GD Value Should be Equal To Invoice Balance");

        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : removed) {
            Map<String, Object> vd = gdModel(r);
            vd.put("Id", asInt(r.get("Id")));
            vd.put("ActionTypeId", 3);
            items.add(vd);
        }
        BigDecimal bankInvoiceAmount = BigDecimal.ZERO, gdValue = BigDecimal.ZERO;
        int i = 0;
        for (Map<String, Object> r : rows) {
            Map<String, Object> vd = gdModel(r);
            int id = asInt(r.get("Id"));
            vd.put("Id", id);
            vd.put("ActionTypeId", id <= 0 ? 1 : 2);
            validateField(asInt(vd.get("ExImInvoiceId")), "Invoice No", i);
            validateField(vd.get("GDNO"), "GD No", i);
            validateField(vd.get("BankInvoiceNo"), "Bank Invoice No", i);
            validateField(asInt(vd.get("DueDays")), "Due Days", i);
            validateField((BigDecimal) vd.get("GDValue"), "GD Value", i);
            validateField((BigDecimal) vd.get("FobValue"), "FOB Value", i);
            validateField((BigDecimal) vd.get("NetToBeRealized"), "Net Amount", i);
            gdValue = gdValue.add((BigDecimal) vd.get("GDValue"));
            bankInvoiceAmount = dec(r.get("OtherAmount"));
            items.add(vd);
            i++;
        }
        if (bankInvoiceAmount.compareTo(gdValue) != 0)
            throw new IllegalArgumentException("Bank InvoiceAmount must be equal to Total GDValue Please Check");
        int result = repo.saveGdBreakUp(items);
        return ok(result, "Record Save Successfully");
    }

    /** ExImInvoiceBankGDBreakUp non-virtual properties (duty / tax values are never set by this form -> 0). */
    private static Map<String, Object> gdModel(Map<String, Object> r) {
        Map<String, Object> vd = new LinkedHashMap<>();
        vd.put("Id", 0);
        vd.put("ExImInvoiceId", asInt(r.get("InvoiceId")));
        vd.put("BankInvoiceNo", text(r.get("InvoiceNo")));
        vd.put("BankInvoiceDate", ts(r.get("InvoiceDate")));
        vd.put("GDNO", text(r.get("GDNo")));
        vd.put("GDDate", ts(r.get("GDDate")));
        vd.put("GDValue", dec(r.get("GDValue")));
        vd.put("GDWeight", BigDecimal.ZERO);
        vd.put("GDNoofContrainers", 0);
        vd.put("BankId", asInt(r.get("BankId")));
        vd.put("ActionTypeId", 0);
        vd.put("FormDocumentTypeId", FORM_DOCUMENT_TYPE_ID);
        vd.put("DueDays", asInt(r.get("DueDays")));
        vd.put("ExchangeRate", dec(r.get("ExchangeRate")));
        vd.put("Freight", dec(r.get("Freight")));
        vd.put("FobValue", dec(r.get("FOBValue")));
        vd.put("CommAmount", dec(r.get("FttAmount")));
        vd.put("NetToBeRealized", dec(r.get("NetAmount")));
        vd.put("CommPercent", dec(r.get("FTTPercent")));
        vd.put("Remarks", text(r.get("Remarks")));
        vd.put("DocumentTypeId", 1);
        vd.put("CustomDutyPercent", 0d);
        vd.put("CustomDutyAmount", 0d);
        vd.put("SalesTaxPercent", 0d);
        vd.put("SalesTaxAmount", 0d);
        vd.put("AdditionalCustomDutyPercent", 0d);
        vd.put("AdditionalCustomDutyAmount", 0d);
        vd.put("IncomeTaxPercent", 0d);
        vd.put("IncomeTaxAmount", 0d);
        return vd;
    }

    // ================================================================= Bank Invoice Payment Schedule & GD Mapping

    /** dtInvoices rows: Id, InvoiceNo, DocumentTypeId, GdId, GDNO, GDValue, GdDocTypeId. */
    private static List<Map<String, Object>> customInvoiceRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("GdId", asInt(ci(r, "GdId")));
            m.put("GDNO", text(ci(r, "GDNO")));
            m.put("GDValue", asDouble(ci(r, "GDValue")));
            m.put("GdDocTypeId", asInt(ci(r, "GdDocTypeId")));
            out.add(m);
        }
        return out;
    }

    /** BtnNewAdvanceAgaintGD_Click / grdAdvanceHistory_DoubleClick: GetCustomInvoicesForGdMapping(.., invoiceId). */
    public List<Map<String, Object>> customInvoices(int invoiceId) { return customInvoiceRows(repo.customInvoicesForGdMapping(user("View"), invoiceId)); }

    /** getPaymentTermDetailByInvoiceId -> dtPaymentTerm columns. */
    public List<Map<String, Object>> paymentTerms(int invoiceId) {
        user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.paymentTermDetail(invoiceId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("PaymentTermId", asInt(ci(r, "PaymentTermId")));
            m.put("PaymentTerm", text(ci(r, "PaymentTerm")));
            m.put("ExImEFormRegistrationId", asInt(ci(r, "ExImEFormRegistrationId")));
            m.put("FinancialInstrumentNo", text(ci(r, "FinancialInstrumentNo")));
            m.put("PrcntOfTotal", asDouble(ci(r, "PrcntOfTotal")));
            m.put("FcyAmount", asDouble(ci(r, "FcyAmount")));
            m.put("DueDays", asInt(ci(r, "DueDays")));
            out.add(m);
        }
        return out;
    }

    public List<Map<String, Object>> advanceHistoryInvoices() { return repo.advanceHistoryInvoices(user("View")); }

    /** button1_Click (btnShowAdvanceHistory) -> dthis columns. */
    public List<Map<String, Object>> advanceHistory(int invoiceId, String fromDate, String toDate) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.advanceHistory(u, invoiceId, sqlDate(fromDate), sqlDate(toDate))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("InvoiceId", asInt(ci(r, "ExImInvoiceId")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("GDId", asInt(ci(r, "GDId")));
            m.put("GDNo", text(ci(r, "GDNO")));
            m.put("PaymentTermId", asInt(ci(r, "PaymentTermId")));
            m.put("PaymentTerm", text(ci(r, "PaymentTerm")));
            m.put("DocumentTypeId", asInt(ci(r, "RefDocumentTypeId")));
            m.put("FIId", asInt(ci(r, "FIId")));
            m.put("FINo", text(ci(r, "FINo")));
            m.put("UtilizeAmount", asDouble(ci(r, "UtilizeAmount")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("EntryUser", text(ci(r, "EntryUser")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUser")));
            m.put("FIDocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            out.add(m);
        }
        return out;
    }

    /**
     * BtnSaveAdvanceAgaintGD_Click: "Advance Utilize Grid Record not found"; ValidateByKey(GD combo table,
     * grid, GdId, GDValue vs UtilizeAmount, "GDNO") and ValidateByKey(grid, payment terms, PaymentTermId,
     * UtilizeAmount vs FcyAmount, "Payment Term") - each key of the union of both tables must have equal
     * 2-decimal sums ("{caption} wise mismatch. ..."); rows -> AdvanceUtilizeagainstGDs (InvoiceDocTypeId 214,
     * DocumentTypeId = FIDocumentTypeId, user / tenancy from the session, EntryDate / ModifyDate stamped by
     * the DAL); more than one invoice -> "Record cannot be inserted against multiple invoices".
     */
    public Map<String, Object> saveAdvanceUtilize(Map<String, Object> body) {
        UserAccount u = user("Save");
        List<Map<String, Object>> rows = list(body.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Advance Utilize Grid Record not found");
        List<Map<String, Object>> gdTable = list(body.get("gds"));
        List<Map<String, Object>> ptTable = list(body.get("paymentTerms"));
        validateByKey(gdTable, rows, "GdId", "GDId", "GDValue", "UtilizeAmount", "GDNO");
        validateByKey(rows, ptTable, "PaymentTermId", "PaymentTermId", "UtilizeAmount", "FcyAmount", "Payment Term");

        Timestamp now = new Timestamp(System.currentTimeMillis());
        List<Map<String, Object>> items = new ArrayList<>();
        Set<Integer> invoices = new LinkedHashSet<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> vd = new LinkedHashMap<>();
            vd.put("Id", asInt(r.get("Id")));
            vd.put("GDId", asInt(r.get("GDId")));
            vd.put("EximInvoiceId", asInt(r.get("InvoiceId")));
            vd.put("FIId", asInt(r.get("FIId")));
            vd.put("FINo", text(r.get("FINo")));
            vd.put("DocumentTypeId", asInt(r.get("FIDocumentTypeId")));
            vd.put("UtilizeAmount", dec(r.get("UtilizeAmount")));
            vd.put("RefDocumentTypeId", asInt(r.get("DocumentTypeId")));
            vd.put("PaymentTermId", asInt(r.get("PaymentTermId")));
            vd.put("InvoiceDocTypeId", FORM_DOCUMENT_TYPE_ID);
            vd.put("EntryUserId", u.getId());
            vd.put("EntryDate", now);
            vd.put("ModifyUserId", u.getId());
            vd.put("ModifyDate", now);
            vd.put("OrganizationId", u.getOrganizationId());
            vd.put("CompanyId", u.getCompanyId());
            if (asInt(r.get("InvoiceId")) > 0) invoices.add(asInt(r.get("InvoiceId")));
            items.add(vd);
        }
        if (invoices.size() > 1) throw new IllegalArgumentException("Record cannot be inserted against multiple invoices");
        int result = repo.saveAdvanceUtilize(items, text(body.get("removedIds")));
        return ok(result, "Record Save Successfully");
    }

    /** ValidateByKey(gridA, gridB, keyColumn, amountColumnA, amountColumnB, keyCaption) - keys are the union of both tables. */
    private static void validateByKey(List<Map<String, Object>> a, List<Map<String, Object>> b, String keyA, String keyB,
                                      String amountA, String amountB, String caption) {
        Set<Integer> keys = new LinkedHashSet<>();
        for (Map<String, Object> r : a) keys.add(asInt(ci(r, keyA)));
        for (Map<String, Object> r : b) keys.add(asInt(ci(r, keyB)));
        for (int key : keys) {
            double sumA = 0, sumB = 0;
            for (Map<String, Object> r : a) if (asInt(ci(r, keyA)) == key) sumA += asDouble(ci(r, amountA));
            for (Map<String, Object> r : b) if (asInt(ci(r, keyB)) == key) sumB += asDouble(ci(r, amountB));
            if (Math.round(sumA * 100) != Math.round(sumB * 100))
                throw new IllegalArgumentException(caption + " wise mismatch.\n\n" + caption + ": " + key + "\nAdvance Total: " + n2(sumA) + "\nSchedule Total: " + n2(sumB));
        }
    }

    private static String n2(double v) { return String.format("%,.2f", v); }
}
