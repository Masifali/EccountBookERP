package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportFiUtilizedAgainstShipmentRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportShipmentFormsSupport.*;

/**
 * Architecture.WinApp.Export.FinancialInstrumentsUtilizedAgainstShipment "Financial Instruments Utilized Against Shipment"
 * (ClientSize 1014 x 461; form Text "Commercial Invoice"). The form has no ScreenDefinition row; its Load reads the rights of
 * "CommiercialInvoiceAgainstPreInvoiceTransfer" = ScreenDefinition 202, so this service checks 202 through
 * DesktopReportRights: Save for btnsave, Update for btnupdate.
 *
 * Desktop behaviour reproduced, not corrected:
 *  Q1  UtilizeAmount (sum of the grid FcyAmount) is compared with the "Balance" box text (txtInvoiceBalanceAmt), which is
 *      only refreshed on CmbInvoiceNo_Leave and is "" for 0 ("#,##.###") - the page sends that text.
 *  Q2  ReadById always switches to Update mode and sets RecId, also when the read fails with "Record Not found...".
 *  Q3  Ctrl+U calls btnsave_Click (RecId = 0) - the page's shortcut does the same; the procedure updates rows whose Id > 0.
 *  Q4  History "Financial Instrument No" filter sends @EFormRegistrationId = Conversion.ToInt(FinancialInstrumentNo text)
 *      (the FI number, not its id) and @DocumentTypeId = the row's DocumentTypeId.
 *  Q5  Grid rows with Id > 0 cannot be deleted ("You can not Delete this Row..."); "Reset Detail First..." while editing.
 *  Q6  The DueDays / %OfTotal / FcyAmount calculations run on Leave, not on every key.
 */
@Service
public class ExportFiUtilizedAgainstShipmentService {

    public static final int SCREEN_ID = 202;

    @Autowired private ExportFiUtilizedAgainstShipmentRepository repo;
    @Autowired private CurrentUserContext ctx;
    @Autowired private DesktopReportRights rights;

    private UserAccount user(String action) {
        UserAccount u = ctx.requireAccountingUser();
        rights.require(u, SCREEN_ID, action);
        return u;
    }

    private boolean allowed(UserAccount u, String action) {
        try { rights.require(u, SCREEN_ID, action); return true; }
        catch (AccessDeniedException e) { return false; }
    }

    /** ImProformaInvoice_Load: rights, InvoiceNoFill, BindFinancialInstrument, PaymentTermsFill, config, HistoryCombosFill. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        out.put("rights", perm);
        put(out, "invoices", () -> plain(repo.invoices(u, 0)));
        put(out, "fis", () -> plain(repo.fiNos(u)));
        put(out, "paymentTerms", () -> plain(repo.paymentTerms(u)));
        out.put("defaultDaysToLessFromHistoryFromDate", asInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        int fcy = asInt(repo.config(u, "DefaultNoOfDecimalPointsForFcyAmount"));
        out.put("fcyDecimals", fcy >= 1 && fcy <= 4 ? fcy : 0);
        historyCombos(u, out);
        return out;
    }

    /** btnrefersh_Click: InvoiceNoFill (with RecId), BindFinancialInstrument, PaymentTermsFill. */
    public Map<String, Object> refresh(int recId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        put(out, "invoices", () -> plain(repo.invoices(u, recId)));
        put(out, "fis", () -> plain(repo.fiNos(u)));
        put(out, "paymentTerms", () -> plain(repo.paymentTerms(u)));
        return out;
    }

    public List<Map<String, Object>> invoices(int recId) { return plain(repo.invoices(user("View"), recId)); }

    /** GetFinancialInstrumentsBalance - the first cell of the first row as decimal (0 when none). */
    public Map<String, Object> fiBalance(int documentTypeId, int id) {
        UserAccount u = user("View");
        List<Map<String, Object>> r = repo.fiBalance(u, documentTypeId, id);
        Object first = null;
        if (!r.isEmpty()) for (Object v : r.get(0).values()) { first = v; break; }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("balance", dec(first).toPlainString());
        return out;
    }

    /** btnRefreshHistory_Click / HistoryCombosFill + FINoHistoryFill. */
    public Map<String, Object> historyCombos() {
        Map<String, Object> out = new LinkedHashMap<>();
        historyCombos(user("View"), out);
        return out;
    }

    private void historyCombos(UserAccount u, Map<String, Object> out) {
        put(out, "historyInvoices", () -> {
            List<Map<String, Object>> inv = new ArrayList<>();
            for (Map<String, Object> r : repo.historyDropDowns(u)) {
                if (!"Invoice".equals(text(ci(r, "ActivityType")))) continue;
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("name", text(ci(r, "name")));
                inv.add(m);
            }
            return inv;
        });
        put(out, "historyFisFromInvoice", () -> {
            List<Map<String, Object>> fi = new ArrayList<>();
            for (Map<String, Object> r : repo.historyDropDowns(u)) {
                if (!"InvoiceNo".equals(text(ci(r, "ActivityType")))) continue;
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(r, "Id")));
                m.put("name", text(ci(r, "name")));
                fi.add(m);
            }
            return fi;
        });
        put(out, "historyFis", () -> plain(repo.historyFis(u)));
    }

    /**
     * ReadById(Id): ExImInvoice.GetByID(Id) -> header + ExImInvoicePaymentTermsDetail; null or no detail -> "Record Not found...".
     * The page applies Q2 on either outcome.
     */
    public Map<String, Object> readById(int id) {
        user("View");
        List<Map<String, Object>> head = repo.invoiceById(id);
        if (head.isEmpty()) throw new IllegalArgumentException("Record Not found...");
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.paymentTermsByHeaderId(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(d, "Id")));
            m.put("DocumentTypeId", asInt(ci(d, "DocumentTypeId")));
            m.put("ExImEFormRegistrationId", asInt(ci(d, "ExImEFormRegistrationId")));
            m.put("FinancialInstrumentNo", raw(ci(d, "FinancialInstrumentNo")));
            m.put("PaymentTermId", asInt(ci(d, "PaymentTermId")));
            m.put("PaymentTerm", raw(ci(d, "PaymentTerm")));
            m.put("OfTotal", asDouble(ci(d, "PrcntOfTotal")));
            m.put("FcyAmount", asDouble(ci(d, "FcyAmount")));
            m.put("DueDays", asInt(ci(d, "DueDays")));
            rows.add(m);
        }
        if (rows.isEmpty()) throw new IllegalArgumentException("Record Not found...");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", asInt(ci(head.get(0), "Id")));
        out.put("rows", rows);
        return out;
    }

    /** HistoryGridFill: dtTarget columns. */
    public List<Map<String, Object>> history(int invoiceId, int eFormRegistrationId, int documentTypeId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(u, eFormRegistrationId, documentTypeId, invoiceId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("DocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("ExImInvoiceId", asInt(ci(r, "ExImInvoiceId")));
            m.put("InvoiceNo", raw(ci(r, "InvoiceNo")));
            m.put("ExImEFormRegistrationId", asInt(ci(r, "ExImEFormRegistrationId")));
            m.put("FinancialInstrumentNo", raw(ci(r, "FinancialInstrumentNo")));
            m.put("PaymentTermId", asInt(ci(r, "PaymentTermId")));
            m.put("PaymentTerm", raw(ci(r, "PaymentTerm")));
            m.put("PrcntOfTotal", asDouble(ci(r, "PrcntOfTotal")));
            m.put("FcyAmount", asDouble(ci(r, "FcyAmount")));
            m.put("DueDays", asInt(ci(r, "DueDays")));
            m.put("SortNo", asInt(ci(r, "SortNo")));
            m.put("PaymentRemarks", raw(ci(r, "PaymentRemarks")));
            m.put("EntryUserName", raw(ci(r, "EntryUserName")));
            m.put("EntryDate", isoDateTime(ci(r, "EntryDate")));
            m.put("ModifyUserName", raw(ci(r, "ModifyUserName")));
            m.put("ModifyDate", isoDateTime(ci(r, "ModifyDate")));
            out.add(m);
        }
        return out;
    }

    /**
     * INSERT(): "Grid Record not found" (thrown before FormValidation), "Please Select Invoice Number First...",
     * the confirm (page), the rows in grid order (SortNo = RowIndex + 1), "UtilizeAmount cannot be greater than
     * TotalBankAmount", SaveForPaymentTermsDetail.
     */
    public Map<String, Object> save(Map<String, Object> body) {
        int recId = asInt(body.get("recId"));
        UserAccount u = user(recId > 0 ? "Update" : "Save");
        List<Map<String, Object>> grid = list(body.get("rows"));
        if (grid.isEmpty()) throw new IllegalArgumentException("Grid Record not found");
        int invoiceId = asInt(body.get("invoiceId"));
        if (invoiceId == 0) throw new IllegalArgumentException("Please Select Invoice Number First...");
        int invoiceDocumentTypeId = asInt(body.get("invoiceDocumentTypeId"));
        double totalBankAmount = asDouble(body.get("invoiceBalanceText"));                       // Q1
        double utilize = 0;
        List<Map<String, Object>> items = new ArrayList<>();
        for (int i = 0; i < grid.size(); i++) {
            Map<String, Object> r = grid.get(i);
            Map<String, Object> vdp = new LinkedHashMap<>();
            double fcy = asDouble(r.get("FcyAmount"));
            vdp.put("FcyAmount", fcy);
            vdp.put("PrcntOfTotal", asDouble(r.get("OfTotal")));
            vdp.put("DueDays", asInt(r.get("DueDays")));
            vdp.put("EntryUserId", u.getId());
            vdp.put("ModifyUserId", u.getId());
            vdp.put("ExImEFormRegistrationId", asInt(r.get("ExImEFormRegistrationId")));
            vdp.put("ExImInvoiceId", invoiceId);
            vdp.put("RefDocumentTypeId", invoiceDocumentTypeId);
            vdp.put("Id", asInt(r.get("Id")));
            vdp.put("PaymentTermId", asInt(r.get("PaymentTermId")));
            vdp.put("DocumentTypeId", asInt(r.get("DocumentTypeId")));
            vdp.put("SortNo", i + 1);
            vdp.put("FinancialInstrumentNo", raw(r.get("FinancialInstrumentNo")));
            vdp.put("PaymentRemarks", null);
            utilize += fcy;
            items.add(vdp);
        }
        if (utilize > totalBankAmount) throw new IllegalArgumentException("UtilizeAmount cannot be greater than TotalBankAmount");
        int result = repo.save(items);
        return ok(recId == 0 ? "Save SuccessFully" : "Update SuccessFully", result);
    }
}
