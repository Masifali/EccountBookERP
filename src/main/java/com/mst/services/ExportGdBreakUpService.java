package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportGdBreakUpRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeParseException;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.ExportGdBreakUpRepository.ci;

/**
 * The BLL side of 881 frmGdBreakUpByInvoice "Gd Break Up By Invoice" (Architecture.WinApp.Export).
 *
 * Two tabs on the desktop (tabControlMain): "GD BreakUp" (Form + History) and "Advance Utilized By GD"
 * (Form + History). ScreenName = "frmGdBreakUpByInvoice", DocumentTypeId = 224 (the FormDocumentTypeId
 * every GD row carries; each row's own DocumentTypeId is 1).
 *
 * Rights come from the real chain (CompanyRights + ScreenRights + tblUserRights by ScreenDefinition.Id
 * 881): View to load, Save for both Save buttons (BtnSaveGdBreakUp / BtnSaveAdvanceAgaintGD are
 * enabled by DoHaveSaveRight), Update for the History grid's Edit button (DoHaveUpdateRights). The
 * form's own validations are repeated here with the desktop's messages so a procedure is never reached
 * with what the form would have refused. Organisation and company come from the session, never the
 * request.
 */
@Service
public class ExportGdBreakUpService {

    public static final int SCREEN_ID = 881;
    public static final int FORM_DOCUMENT_TYPE_ID = 224;

    @Autowired private ExportGdBreakUpRepository repo;
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

    /**
     * InitializeComponentMethod: rights, GetBankInvoicesForGdBreakUpNew(.., 0), Bank.GetAll,
     * GetInvoicesForAdvanceUtilize, GetFINoForInvoice, GetDataForDropDownFromExportGdBreakup("Bank"),
     * then GetConfigurationsFromGlobal (DefaultDaysToLessFromHistoryFromDate, AllowOneRowPerInvoiceOnGD)
     * and the history From date = today - that many days (else 3), To date = today.
     */
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
        put(out, "advInvoices", () -> repo.invoicesForAdvanceUtilize(u));
        put(out, "fis", () -> repo.fiNosForInvoice(u));
        put(out, "historyBanks", () -> repo.historyBanks(u));
        return out;
    }

    /** GetConfigurationsFromGlobal - also re-read by BtnRefreshGdBreakUp. */
    public Map<String, Object> config(UserAccount u) {
        Map<String, Object> c = new LinkedHashMap<>();
        int days;
        try { days = (int) Double.parseDouble(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")); }
        catch (NumberFormatException e) { days = 0; }
        c.put("defaultDaysToLessFromHistoryFromDate", days);
        c.put("allowOneRowPerInvoiceOnGD", asBool(repo.config(u, "AllowOneRowPerInvoiceOnGD")));
        return c;
    }

    /** BtnRefreshGdBreakUp_Click: configuration, invoices (with the current GdBreakUpRecId) and banks. */
    public Map<String, Object> refreshGdBreakUp(int recId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("config", config(u));
        out.put("invoices", invoiceRows(repo.bankInvoicesForGdBreakUp(u, recId)));
        out.put("banks", bankRows(repo.banks(u)));
        return out;
    }

    /** InvoicesNoBindGdBreakUp(GetBankInvoicesForGdBreakUpNew(.., GdBreakUpRecId)). */
    public List<Map<String, Object>> invoices(int recId) {
        return invoiceRows(repo.bankInvoicesForGdBreakUp(user("View"), recId));
    }

    /** Invoice combo rows exactly as the procedure returns them (Id, InvoiceNo, BankInvoiceAmount). */
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

    /** BankBind: only IsHomeland == "Home Country", columns Id, BranchName, BankIBANNo (all shown). */
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

    /**
     * ReadyByIdGdBreak(InvoiceId): GetGdBreakUpByInvoiceId (USP_ExImInvoiceBankGDBreakUp_ReadById) into
     * the dtGdBreakUp columns, plus the invoice combo re-bound with GdBreakUpRecId = InvoiceId. The BLL
     * never returns null (an empty list when nothing matches), so "Record not found" is unreachable on
     * the desktop; an empty grid comes back here too.
     */
    public Map<String, Object> byInvoice(int invoiceId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.gdBreakUpByInvoiceId(invoiceId)) rows.add(gdRow(d));
        out.put("rows", rows);
        out.put("invoices", invoiceRows(repo.bankInvoicesForGdBreakUp(u, invoiceId)));
        return out;
    }

    /** FillOtherItemDetailFromListCommonForReadById - one dtGdBreakUp row from one ReadById row. */
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
        m.put("CustomDutyPercent", asDouble(ci(d, "CustomDutyPercent")));
        m.put("CustomDutyAmount", asDouble(ci(d, "CustomDutyAmount")));
        m.put("SalesTaxPercent", asDouble(ci(d, "SalesTaxPercent")));
        m.put("SalesTaxAmount", asDouble(ci(d, "SalesTaxAmount")));
        m.put("AdditionalCustomDutyPercent", asDouble(ci(d, "AdditionalCustomDutyPercent")));
        m.put("AdditionalCustomDutyAmount", asDouble(ci(d, "AdditionalCustomDutyAmount")));
        m.put("IncomeTaxPercent", asDouble(ci(d, "IncomeTaxPercent")));
        m.put("IncomeTaxAmount", asDouble(ci(d, "IncomeTaxAmount")));
        m.put("Remarks", text(ci(d, "Remarks")));
        return m;
    }

    /**
     * FillGDFormHistory: From/To only when ticked, @BankId = CmbBankGdBankHistory.Value when chosen. The
     * grid columns are dtHistoryGridGdBreakUp's, in its order; GDDate is ToShortDateString on the desktop.
     */
    public List<Map<String, Object>> gdHistory(String fromDate, String toDate, int bankId) {
        UserAccount u = user("View");
        LocalDate from = asDate(fromDate), to = asDate(toDate);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.gdBreakUpHistory(u,
                from == null ? null : java.sql.Date.valueOf(from), to == null ? null : java.sql.Date.valueOf(to), bankId)) {
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

    /** btnRefreshGdBreakupHistory_Click. */
    public List<Map<String, Object>> historyBanks() { return repo.historyBanks(user("View")); }

    // ================================================================= GD BreakUp: save

    /**
     * Insert(): grid empty -> "GDBreakUp Detail Record Not Found"; the list is the removed rows
     * (ActionTypeId 3) followed by the grid rows (Id <= 0 -> 1, else 2); each grid row goes through
     * FormHelper.ValidateField for Invoice No, GD No, Bank Invoice No, Due Days, GD Value, FOB Value and
     * Net Amount; then BankInvoiceAmount (the OtherAmount of the LAST grid row) must equal the total
     * GDValue - "Bank InvoiceAmount must be equal to Total GDValue Please Check"; then SaveGdBreakUp.
     *
     * The page repeats btnAddGDBreakUpDetail_Click's own rules before a row reaches the grid (one
     * invoice per grid, AllowOneRowPerInvoiceOnGD, GD Value == Invoice Balance); they are checked again
     * here so a hand-made request cannot skip them.
     */
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
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", result);
        out.put("message", "Record Save Successfully");
        return out;
    }

    /**
     * FillDetailListCommonForInsertAndDelete + the model's remaining non-virtual properties, in the
     * order GenericProvider.SetProc sends them (every one is sent; GDWeight and GDNoofContrainers are
     * never set by this form and go as 0). InvoiceNo / OtherAmount / BankName are virtual on the model
     * and therefore never sent.
     */
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
        vd.put("CustomDutyPercent", asDouble(r.get("CustomDutyPercent")));
        vd.put("CustomDutyAmount", asDouble(r.get("CustomDutyAmount")));
        vd.put("SalesTaxPercent", asDouble(r.get("SalesTaxPercent")));
        vd.put("SalesTaxAmount", asDouble(r.get("SalesTaxAmount")));
        vd.put("AdditionalCustomDutyPercent", asDouble(r.get("AdditionalCustomDutyPercent")));
        vd.put("AdditionalCustomDutyAmount", asDouble(r.get("AdditionalCustomDutyAmount")));
        vd.put("IncomeTaxPercent", asDouble(r.get("IncomeTaxPercent")));
        vd.put("IncomeTaxAmount", asDouble(r.get("IncomeTaxAmount")));
        return vd;
    }

    // ================================================================= Advance Utilized By GD

    /** BtnRefreshAdvanceAgaintGD_Click / ResetGdAdvanceUtilizeDetails: invoices and FI numbers. */
    public Map<String, Object> advanceSetup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "advInvoices", () -> repo.invoicesForAdvanceUtilize(u));
        put(out, "fis", () -> repo.fiNosForInvoice(u));
        return out;
    }

    /** CmbInvoiceAdvanceUtilize_Leave: the GD combo for the invoice, and the saved utilisations (grid). */
    public Map<String, Object> advanceByInvoice(int invoiceId, int invoiceDocumentTypeId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("gds", repo.gdsForAdvanceUtilize(u, invoiceId));
        List<Map<String, Object>> rows = new ArrayList<>();
        if (invoiceId > 0) {
            for (Map<String, Object> r : repo.advanceUtilizeByInvoiceId(invoiceId, invoiceDocumentTypeId)) rows.add(advRow(r));
        }
        out.put("rows", rows);
        return out;
    }

    /** dtAdvanceUtilize columns from a ReadById / FormHistory row. */
    private static Map<String, Object> advRow(Map<String, Object> r) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(r, "Id")));
        m.put("InvoiceId", asInt(ci(r, "ExImInvoiceId")));
        m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
        m.put("GDId", asInt(ci(r, "GDId")));
        m.put("GDNo", text(ci(r, "GDNO")));
        m.put("DocumentTypeId", asInt(ci(r, "RefDocumentTypeId")));
        m.put("FIId", asInt(ci(r, "FIId")));
        m.put("FINo", text(ci(r, "FINo")));
        m.put("UtilizeAmount", asDouble(ci(r, "UtilizeAmount")));
        return m;
    }

    /** tabControl3_SelectedIndexChanged (History tab): AdvanceUtilizeagainstGDs_FormHistory. */
    public List<Map<String, Object>> advanceHistory() {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.advanceUtilizeHistory(user("View"))) out.add(advRow(r));
        return out;
    }

    /**
     * BtnSaveAdvanceAgaintGD_Click: grid empty -> "Advance Utilize Grid Record not found"; each row's
     * Id, EximInvoiceId, GDId, RefDocumentTypeId, FIId, FINo, UtilizeAmount from the grid; Ids = the
     * removed saved rows; SaveAdvanceUtilize. The model's other properties (DocumentTypeId,
     * PaymentTermId, InvoiceDocTypeId, EntryUserId, ModifyUserId, OrganizationId, CompanyId) are never
     * set by the form and are sent as 0, exactly as GenericProvider.SetProc sends them; the DAL stamps
     * EntryDate and ModifyDate with DateTime.Now.
     */
    public Map<String, Object> saveAdvanceUtilize(Map<String, Object> body) {
        user("Save");
        List<Map<String, Object>> rows = list(body.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Advance Utilize Grid Record not found");
        Timestamp now = new Timestamp(System.currentTimeMillis());
        List<Map<String, Object>> items = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            if (asInt(r.get("GDId")) == 0) throw new IllegalArgumentException("GD No field is required");
            if (asInt(r.get("FIId")) == 0) throw new IllegalArgumentException("FI No field is required");
            if (asDouble(r.get("UtilizeAmount")) == 0) throw new IllegalArgumentException("Utilize Amount must be a non-zero number");
            Map<String, Object> vd = new LinkedHashMap<>();
            vd.put("Id", asInt(r.get("Id")));
            vd.put("GDId", asInt(r.get("GDId")));
            vd.put("EximInvoiceId", asInt(r.get("InvoiceId")));
            vd.put("FIId", asInt(r.get("FIId")));
            vd.put("RefDocumentTypeId", asInt(r.get("DocumentTypeId")));
            vd.put("DocumentTypeId", 0);
            vd.put("PaymentTermId", 0);
            vd.put("InvoiceDocTypeId", 0);
            vd.put("EntryUserId", 0);
            vd.put("ModifyUserId", 0);
            vd.put("OrganizationId", 0);
            vd.put("CompanyId", 0);
            vd.put("FINo", text(r.get("FINo")));
            vd.put("UtilizeAmount", dec(r.get("UtilizeAmount")));
            vd.put("EntryDate", now);
            vd.put("ModifyDate", now);
            items.add(vd);
        }
        String ids = text(body.get("removedIds"));
        int result = repo.saveAdvanceUtilize(items, ids);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("id", result);
        out.put("message", "Record Save Successfully");
        return out;
    }

    // ================================================================= helpers

    private interface Loader { Object load() throws Exception; }

    /** InitializeComponentMethod runs its reads together; one failing read is reported beside the others. */
    private static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", msg(e)); }
    }

    /** FormHelper.ValidateField: null, 0, <= 0 decimals, blank strings -> "{field} is required in Detail Grid at row No: {n}". */
    private static void validateField(Object value, String field, int rowIndex) {
        boolean bad = value == null
                || (value instanceof Integer && (Integer) value == 0)
                || (value instanceof BigDecimal && ((BigDecimal) value).signum() <= 0)
                || (value instanceof Double && (Double) value <= 0)
                || (value instanceof String && ((String) value).trim().isEmpty());
        if (bad) throw new IllegalArgumentException(field + " is required in Detail Grid at row No: " + (rowIndex + 1));
    }

    @SuppressWarnings("unchecked")
    private static List<Map<String, Object>> list(Object v) {
        List<Map<String, Object>> out = new ArrayList<>();
        if (v instanceof List) for (Object o : (List<Object>) v) if (o instanceof Map) out.add((Map<String, Object>) o);
        return out;
    }

    private static String msg(Exception e) {
        Throwable t = e;
        while (t.getCause() != null && t.getCause() != t) t = t.getCause();
        return t.getMessage() == null ? String.valueOf(e.getMessage()) : t.getMessage();
    }

    private static String text(Object v) { return v == null ? "" : String.valueOf(v).trim(); }

    static int asInt(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).intValue();
        try { return Integer.parseInt(String.valueOf(v).trim()); }
        catch (NumberFormatException e) {
            try { return (int) Double.parseDouble(String.valueOf(v).trim().replace(",", "")); }
            catch (NumberFormatException e2) { return 0; }
        }
    }

    /** Conversion.ToDouble - unparseable text is 0 (the desktop's boxes carry "#,##0.###" text). */
    static double asDouble(Object v) {
        if (v == null) return 0;
        if (v instanceof Number) return ((Number) v).doubleValue();
        try { return Double.parseDouble(String.valueOf(v).trim().replace(",", "")); }
        catch (NumberFormatException e) { return 0; }
    }

    /** Conversion.ToDecimal of a grid cell. */
    private static BigDecimal dec(Object v) {
        if (v instanceof BigDecimal) return (BigDecimal) v;
        return BigDecimal.valueOf(asDouble(v));
    }

    private static boolean asBool(Object v) {
        if (v == null) return false;
        if (v instanceof Boolean) return (Boolean) v;
        String s = String.valueOf(v).trim().toLowerCase();
        return "true".equals(s) || "1".equals(s) || "on".equals(s);
    }

    private static LocalDate asDate(Object v) {
        if (v == null) return null;
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return null;
        if (s.length() > 10) s = s.substring(0, 10);
        try { return LocalDate.parse(s); }
        catch (DateTimeParseException e) { return null; }
    }

    /** A date-time for a DATETIME parameter; the page sends yyyy-MM-dd, the desktop sends the picker's DateTime. */
    private static Timestamp ts(Object v) {
        if (v instanceof Timestamp) return (Timestamp) v;
        if (v == null) return new Timestamp(System.currentTimeMillis());
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return new Timestamp(System.currentTimeMillis());
        try { return Timestamp.valueOf(LocalDateTime.parse(s.length() == 10 ? s + "T00:00:00" : s.length() == 16 ? s + ":00" : s)); }
        catch (DateTimeParseException e) {
            try { return Timestamp.valueOf(s.replace('T', ' ')); }
            catch (IllegalArgumentException e2) { return new Timestamp(System.currentTimeMillis()); }
        }
    }

    /** yyyy-MM-dd for the page from whatever the driver returned. */
    static String iso(Object v) {
        if (v == null) return "";
        if (v instanceof java.sql.Timestamp) return ((java.sql.Timestamp) v).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof java.util.Date) return new java.sql.Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().toLocalDate().toString();
        if (v instanceof java.time.LocalDateTime) return ((java.time.LocalDateTime) v).toLocalDate().toString();
        if (v instanceof java.time.LocalDate) return v.toString();
        String s = String.valueOf(v).trim();
        return s.length() >= 10 ? s.substring(0, 10) : s;
    }
}
