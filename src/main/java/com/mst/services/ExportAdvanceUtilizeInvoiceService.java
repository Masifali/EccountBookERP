package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportAdvanceUtilizeInvoiceRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportFormSupport.*;

/**
 * BLL side of 915 FIOrAdvanceUtilizedAgainstInvoice "Advance Utilize Against Invoice" ("Financial Instruments
 * Utilized Against Shipment", Architecture.WinApp.Export), ClientSize 1014 x 461. tabControl1 Form | History;
 * tabControl2 "Payment Detail". Rights: the desktop reads the rights of screen name
 * "CommiercialInvoiceAgainstPreInvoiceTransfer"; the web uses ScreenDefinition 915 (View, Save, Update).
 *
 * The form writes ExImInvoicePaymentTermsDetail rows of an export invoice (payment terms / financial instruments
 * with their FCY share). Quirks kept: Save forces RecId = 0 but the rows keep their Ids (the insert procedure
 * updates by @Id); the Ctrl+U shortcut calls the Save handler; the desktop confirms before validating the FcyAmount
 * total; Received > FcyAmount and |TotalInvoiceAmount - sum(FcyAmount)| > 0.99 are refused with the desktop texts;
 * ReadById(Id) throws "Record Not found..." when the invoice has no payment-term rows.
 */
@Service
public class ExportAdvanceUtilizeInvoiceService {

    public static final int SCREEN_ID = 915;

    @Autowired private ExportAdvanceUtilizeInvoiceRepository repo;
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

    /** ImProformaInvoice_Load: rights, InvoiceNoFill, PaymentTermsFill, HistoryCombosFill. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        out.put("permissions", perm);
        out.putAll(refresh(0));
        out.putAll(historyCombos(u));
        return out;
    }

    /** btnrefersh_Click: InvoiceNoFill + PaymentTermsFill. */
    public Map<String, Object> refresh(int recId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "invoices", () -> invoices(u, recId));
        put(out, "paymentTerms", () -> pick(repo.paymentTerms(u), "Id", "LcOrderTerm"));
        return out;
    }

    private List<Map<String, Object>> invoices(UserAccount u, int recId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.invoices(u, recId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("BankAmount", asDouble(ci(r, "BankAmount")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("SupplierCustomerId", asInt(ci(r, "SupplierCustomerId")));
            m.put("TotalAmount", asDouble(ci(r, "TotalAmount")));
            out.add(m);
        }
        return out;
    }

    /** HistoryCombosFill + FINoHistoryFill (the FI combo ends up bound to usp_GetFinancialInstrumentsForDropDown). */
    public Map<String, Object> historyCombos(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "historyInvoices", () -> {
            List<Map<String, Object>> rows = new ArrayList<>();
            for (Map<String, Object> r : repo.historyInvoices(u)) {
                if (!"Invoice".equals(text(ci(r, "ActivityType")))) continue;
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("Name", text(ci(r, "name")));
                rows.add(m);
            }
            return rows;
        });
        put(out, "historyFis", () -> {
            List<Map<String, Object>> rows = new ArrayList<>();
            for (Map<String, Object> r : repo.historyFis(u)) {
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("FinancialInstrumentNo", text(ci(r, "FinancialInstrumentNo")));
                m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
                rows.add(m);
            }
            return rows;
        });
        return out;
    }

    public Map<String, Object> historyCombos() { return historyCombos(user("View")); }

    private static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String idCol, String nameCol) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, idCol)));
            m.put("Name", text(ci(r, nameCol)));
            out.add(m);
        }
        return out;
    }

    /** CmbInvoiceNo_Leave: getPaymentTermDetailByInvoiceId(InvoiceId, 0) rows + BindFinancialInstrument (usp_GetFINoForInvoice for the invoice's customer). */
    public Map<String, Object> invoiceLeave(int invoiceId, int supplierCustomerId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : repo.paymentTermDetail(invoiceId)) rows.add(termRow(r, true));
        out.put("rows", rows);
        List<Map<String, Object>> fis = new ArrayList<>();
        for (Map<String, Object> r : repo.fiNos(u, supplierCustomerId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("EFormNo", text(ci(r, "EFormNo")));
            m.put("PaymenttermId", asInt(ci(r, "PaymenttermId")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            fis.add(m);
        }
        out.put("fis", fis);
        return out;
    }

    /** dtPaymentTerm row from usp_getPaymentTermDetailByInvoiceId (Received / Balance) or ReadExImInvoicePaymentTermsDetailByHeaderId (none). */
    private static Map<String, Object> termRow(Map<String, Object> r, boolean withReceived) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(r, "Id")));
        m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
        m.put("ExImEFormRegistrationId", asInt(ci(r, "ExImEFormRegistrationId")));
        m.put("FinancialInstrumentNo", text(ci(r, "FinancialInstrumentNo")));
        m.put("PaymentTermId", asInt(ci(r, "PaymentTermId")));
        m.put("PaymentTerm", text(ci(r, "PaymentTerm")));
        m.put("PrcntOfTotal", asDouble(ci(r, "PrcntOfTotal")));
        m.put("FcyAmount", asDouble(ci(r, "FcyAmount")));
        m.put("DueDays", asInt(ci(r, "DueDays")));
        m.put("Received", withReceived ? asDouble(ci(r, "Received")) : 0d);
        m.put("Balance", withReceived ? asDouble(ci(r, "Balance")) : 0d);
        return m;
    }

    /** dcmbfino_ValueChanged -> GetFinancialInstrumentsBalance(DocumentTypeId, Id). */
    public Map<String, Object> fiBalance(int documentTypeId, int id) {
        UserAccount u = user("View");
        List<Map<String, Object>> r = repo.fiBalance(u, documentTypeId, id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("balance", r.isEmpty() ? 0d : asDouble(r.get(0).values().iterator().next()));
        return out;
    }

    /** ReadById(Id): ExImInvoice.GetByID -> ExImInvoicePaymentTermsDetail rows; none -> "Record Not found...". */
    public Map<String, Object> readById(int id) {
        UserAccount u = user("View");
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : repo.paymentTermsByHeaderId(id)) rows.add(termRow(r, false));
        if (rows.isEmpty()) throw new IllegalStateException("Record Not found...");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", id);
        out.put("rows", rows);
        out.put("invoices", invoices(u, id));
        return out;
    }

    /** HistoryGridFill: FormHistory_PaymentTermsDetail with the invoice and the FI (Id + DocumentTypeId); dtTarget columns. */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(u, asInt(b.get("fiId")), asInt(b.get("fiDocumentTypeId")), asInt(b.get("invoiceId")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("ExImInvoiceId", asInt(ci(r, "ExImInvoiceId")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("ExImEFormRegistrationId", asInt(ci(r, "ExImEFormRegistrationId")));
            m.put("FinancialInstrumentNo", text(ci(r, "FinancialInstrumentNo")));
            m.put("PaymentTermId", asInt(ci(r, "PaymentTermId")));
            m.put("PaymentTerm", text(ci(r, "PaymentTerm")));
            m.put("PrcntOfTotal", asDouble(ci(r, "PrcntOfTotal")));
            m.put("FcyAmount", asDouble(ci(r, "FcyAmount")));
            m.put("DueDays", asInt(ci(r, "DueDays")));
            m.put("SortNo", asInt(ci(r, "SortNo")));
            m.put("PaymentRemarks", text(ci(r, "PaymentRemarks")));
            m.put("EntryUserName", text(ci(r, "EntryUserName")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("ModifyUserName", text(ci(r, "ModifyUserName")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            out.add(m);
        }
        return out;
    }

    /**
     * INSERT(): "Grid Record not found"; "Please Select Invoice Number First..."; each row -> ExImInvoicePaymentTermsDetail
     * (ExImInvoiceId, RefDocumentTypeId = the invoice's DocumentTypeId, SortNo = row + 1, Entry/Modify user);
     * Received > FcyAmount -> "Received Amount cannot be greater than FcyAmount.please check ...."; |Total - sum| > 0.99 ->
     * "FcyAmount should be equal to Total Invoice Amount"; SaveForPaymentTermsDetail.
     */
    public Map<String, Object> save(Map<String, Object> b) {
        int recId = Math.max(0, asInt(b.get("recId")));
        UserAccount u = user(recId > 0 ? "Update" : "Save");
        List<Map<String, Object>> grid = list(b.get("rows"));
        if (grid.isEmpty()) throw new IllegalArgumentException("Grid Record not found");
        int invoiceId = asInt(b.get("invoiceId"));
        if (invoiceId == 0) throw new IllegalArgumentException("Please Select Invoice Number First...");
        int refDocTypeId = asInt(b.get("invoiceDocumentTypeId"));
        double total = asDouble(b.get("totalInvoiceAmount")), fcy = 0;
        List<Map<String, Object>> rows = new ArrayList<>();
        int i = 0;
        for (Map<String, Object> r : grid) {
            double amount = asDouble(r.get("FcyAmount"));
            if (asDouble(r.get("Received")) > amount) throw new IllegalArgumentException("Received Amount cannot be greater than FcyAmount.please check ....");
            fcy += amount;
            Map<String, Object> d = new LinkedHashMap<>();
            d.put("FcyAmount", BigDecimal.valueOf(amount));
            d.put("PrcntOfTotal", BigDecimal.valueOf(asDouble(r.get("PrcntOfTotal"))));
            d.put("DueDays", asInt(r.get("DueDays")));
            d.put("EntryUserId", u.getId());
            d.put("ModifyUserId", u.getId());
            d.put("ExImEFormRegistrationId", asInt(r.get("ExImEFormRegistrationId")));
            d.put("ExImInvoiceId", invoiceId);
            d.put("RefDocumentTypeId", refDocTypeId);
            d.put("Id", asInt(r.get("Id")));
            d.put("PaymentTermId", asInt(r.get("PaymentTermId")));
            d.put("DocumentTypeId", asInt(r.get("DocumentTypeId")));
            d.put("SortNo", i + 1);
            d.put("FinancialInstrumentNo", text(r.get("FinancialInstrumentNo")));
            d.put("PaymentRemarks", null);
            rows.add(d);
            i++;
        }
        if (Math.abs(BigDecimal.valueOf(total - fcy).doubleValue()) > 0.99) throw new IllegalArgumentException("FcyAmount should be equal to Total Invoice Amount");
        int n = repo.save(rows);
        return saved(n, recId == 0 ? "Save SuccessFully" : "Update SuccessFully");
    }

    private interface Loader { Object load() throws Exception; }

    private static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", msg(e)); }
    }
}
