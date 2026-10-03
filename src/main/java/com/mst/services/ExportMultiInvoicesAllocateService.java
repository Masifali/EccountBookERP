package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportMultiInvoicesAllocateRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportFormSupport.*;

/**
 * BLL side of 907 frmMultiInvoicesAllocateToGdn "Multi Invoices Allocate To Gdn" (Architecture.WinApp.Export),
 * ScreenName frmMultiInvoicesAllocateToGdn, DocumentTypeId 2, AutoScrollMinSize 1250 x 800. tabControl1 Form | History.
 * Rights: ScreenDefinition 907 - View, Save (Save / SaveAs), Update, Delete (BtnDelete - no handler on the desktop),
 * Print (02_GDBreakUpHeader_Slip.rpt), CanViewAllRecord (read on the desktop but never sent by the BLL).
 *
 * Quirks kept: the detail grid's Ids are reset to 0 whenever RecId == 0 (Save / SaveAs insert new rows); removed
 * saved rows go first with ActionTypeId 3; header Remarks empty -> the LAST detail row's remarks; GDValue must
 * equal the detail total; the BLL never sends FinancialYearId / CanViewAllRecord / DocumentTypeIds to FormHistory;
 * GDWeight / GDNoofContrainers are always 0.
 */
@Service
public class ExportMultiInvoicesAllocateService {

    public static final int SCREEN_ID = 907;
    public static final int DOCUMENT_TYPE_ID = 2;

    @Autowired private ExportMultiInvoicesAllocateRepository repo;
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

    /** InitializeComponentMethod. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Delete", allowed(u, "Delete"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        out.putAll(refresh(0));
        put(out, "historyBanks", () -> historyBanks(u));
        return out;
    }

    /** btnRefresh_Click: configuration, Home Country banks, bank invoices (with RecId). */
    public Map<String, Object> refresh(int recId) {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        int days;
        try { days = (int) Double.parseDouble(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")); } catch (NumberFormatException e) { days = 0; }
        out.put("defaultDaysToLessFromHistoryFromDate", days);
        put(out, "banks", () -> {
            List<Map<String, Object>> rows = new ArrayList<>();
            for (Map<String, Object> b : repo.banks(u)) {
                if (!"Home Country".equals(text(ci(b, "IsHomeland")))) continue;
                Map<String, Object> m = new LinkedHashMap<>();
                m.put("Id", asInt(ci(b, "Id")));
                m.put("BranchName", text(ci(b, "BranchName")));
                m.put("BankIBANNo", text(ci(b, "BankIBANNo")));
                rows.add(m);
            }
            return rows;
        });
        put(out, "invoices", () -> invoices(u, recId));
        return out;
    }

    public List<Map<String, Object>> invoices(UserAccount u, int recId) {
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : repo.bankInvoices(u, recId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("BankInvoiceAmount", asDouble(ci(r, "BankInvoiceAmount")));
            rows.add(m);
        }
        return rows;
    }

    public List<Map<String, Object>> invoices(int recId) { return invoices(user("View"), recId); }

    public List<Map<String, Object>> historyBanks(UserAccount u) {
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> r : repo.historyBanks(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("Name", text(ci(r, "name")));
            rows.add(m);
        }
        return rows;
    }

    public List<Map<String, Object>> historyBanks() { return historyBanks(user("View")); }

    /** ReadById(ID): header + GDBreakUpDetailList (dtdetail columns), and the invoice combo re-read with RecId. */
    public Map<String, Object> readById(int id) {
        UserAccount u = user("View");
        List<Map<String, Object>> rows = repo.byId(id);
        if (rows.isEmpty()) throw new IllegalStateException("Index was outside the bounds of the array.");
        Map<String, Object> r = rows.get(0);
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(r, "Id")));
        m.put("GDNO", text(ci(r, "GDNO")));
        m.put("GDDate", iso(ci(r, "GDDate")));
        m.put("BankInvoiceNo", text(ci(r, "BankInvoiceNo")));
        m.put("BankInvoiceDate", iso(ci(r, "BankInvoiceDate")));
        m.put("BankId", asInt(ci(r, "BankId")));
        m.put("DueDays", asInt(ci(r, "DueDays")));
        m.put("ExchangeRate", asDouble(ci(r, "ExchangeRate")));
        m.put("GDValue", asDouble(ci(r, "GDValue")));
        m.put("Freight", asDouble(ci(r, "Freight")));
        m.put("FobValue", asDouble(ci(r, "FobValue")));
        m.put("CommPercent", asDouble(ci(r, "CommPercent")));
        m.put("CommAmount", asDouble(ci(r, "CommAmount")));
        m.put("NetToBeRealized", asDouble(ci(r, "NetToBeRealized")));
        m.put("Remarks", text(ci(r, "Remarks")));
        m.put("AttachmentsValues", text(ci(r, "AttachmentsValues")));
        m.put("CustomAttachmentsValues", text(ci(r, "CustomAttachmentsValues")));
        m.put("details", details(id));
        m.put("invoices", invoices(u, id));
        return m;
    }

    public List<Map<String, Object>> details(int headerId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.detailsByHeaderId(headerId)) {
            Map<String, Object> x = new LinkedHashMap<>();
            x.put("Id", asInt(ci(d, "Id")));
            x.put("InvoiceId", asInt(ci(d, "ExImInvoiceId")));
            x.put("InvoiceNo", text(ci(d, "InvoiceNo")));
            x.put("InvoiceAmount", asDouble(ci(d, "InvoiceAmount")));
            x.put("Remarks", text(ci(d, "Remarks")));
            out.add(x);
        }
        return out;
    }

    public List<Map<String, Object>> detailsOf(int headerId) { user("View"); return details(headerId); }

    /** HistoryFill: the ticked radio's date pair and the bank; grid columns in dtHistory order. */
    public List<Map<String, Object>> history(Map<String, Object> b) {
        UserAccount u = user("View");
        Map<String, Object> dates = new LinkedHashMap<>();
        String by = text(b.get("dateBy"));
        java.sql.Date from = sqlDate(asDate(b.get("fromDate"))), to = sqlDate(asDate(b.get("toDate")));
        String fk = "entry".equals(by) ? "EntryFromDate" : "modify".equals(by) ? "ModifyFromDate" : "FromDate";
        String tk = "entry".equals(by) ? "EntryToDate" : "modify".equals(by) ? "ModifyToDate" : "ToDate";
        if (flag(b.get("fromChecked")) && from != null) dates.put(fk, from);
        if (flag(b.get("toChecked")) && to != null) dates.put(tk, to);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, dates, asInt(b.get("bankId")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("InvoiceNos", text(ci(r, "InvoiceNo")));
            m.put("GDNo", text(ci(r, "GDNO")));
            m.put("GDDate", iso(ci(r, "GDDate")));
            m.put("BankInvoiceNo", text(ci(r, "BankInvoiceNo")));
            m.put("BankInvoiceDate", iso(ci(r, "BankInvoiceDate")));
            m.put("BankName", text(ci(r, "BankName")));
            m.put("DueDays", asInt(ci(r, "DueDays")));
            m.put("ExchangeRate", asDouble(ci(r, "ExchangeRate")));
            m.put("GDValue", asDouble(ci(r, "GDValue")));
            m.put("Freight", asDouble(ci(r, "Freight")));
            m.put("FobValue", asDouble(ci(r, "FobValue")));
            m.put("FttPercent", asDouble(ci(r, "CommPercent")));
            m.put("FttAmount", asDouble(ci(r, "CommAmount")));
            m.put("NetAmount", asDouble(ci(r, "NetToBeRealized")));
            m.put("EntryUser", text(ci(r, "EntryUser")));
            m.put("EntryDate", iso(ci(r, "EntryDate")));           // ToShortDateString on the desktop
            m.put("ModifyUser", text(ci(r, "ModifyUser")));
            m.put("ModifyDate", iso(ci(r, "ModifyDate")));
            m.put("Remarks", text(ci(r, "Remarks")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            out.add(m);
        }
        return out;
    }

    /**
     * Insert(): "Grid Record Not Found"; FormHelper.ValidateControls (Gd No, Bank Invoice No, Bank, Due Days, GD Value,
     * FOB Value, Net Amount); removed rows (ActionTypeId 3, only when RecId > 0) then the grid rows (Id 0 when
     * RecId == 0; ActionTypeId 1/2) with ValidateField (Invoice, Invoice Amount); GDValue == sum of InvoiceAmount;
     * header Remarks falls back to the last detail remarks; GDBreakUpHeader.Save.
     */
    public Map<String, Object> save(Map<String, Object> b) {
        int recId = Math.max(0, asInt(b.get("recId")));
        UserAccount u = user(recId > 0 ? "Update" : "Save");
        List<Map<String, Object>> grid = list(b.get("details"));
        if (grid.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        if (text(b.get("gdNo")).isEmpty()) throw new IllegalArgumentException("Gd No field is required");
        if (text(b.get("bankInvoiceNo")).isEmpty()) throw new IllegalArgumentException("Bank Invoice No field is required");
        if (asInt(b.get("bankId")) == 0) throw new IllegalArgumentException("Bank field is required");
        if (asInt(b.get("dueDays")) == 0) throw new IllegalArgumentException("Due Days must be a non-zero number");
        if (asDouble(b.get("gdValue")) == 0) throw new IllegalArgumentException("GD Value must be a non-zero number");
        if (asDouble(b.get("fobValue")) == 0) throw new IllegalArgumentException("FOB Value must be a non-zero number");
        if (asDouble(b.get("netAmount")) == 0) throw new IllegalArgumentException("Net Amount must be a non-zero number");

        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("BankInvoiceDate", ts(b.get("bankInvoiceDate")));
        h.put("EntryDate", now);
        h.put("GDDate", ts(b.get("gdDate")));
        h.put("ModifyDate", now);
        h.put("CommAmount", dec(b.get("fttAmount")));
        h.put("CommPercent", dec(b.get("fttPercent")));
        h.put("ExchangeRate", dec(b.get("exchangeRate")));
        h.put("FobValue", dec(b.get("fobValue")));
        h.put("Freight", dec(b.get("freight")));
        h.put("GDValue", dec(b.get("gdValue")));
        h.put("GDWeight", BigDecimal.ZERO);
        h.put("NetToBeRealized", dec(b.get("netAmount")));
        h.put("ActionId", recId == 0 ? 1 : 2);
        h.put("BankId", asInt(b.get("bankId")));
        h.put("DocumentTypeId", DOCUMENT_TYPE_ID);
        h.put("CompanyId", u.getCompanyId());
        h.put("DueDays", asInt(b.get("dueDays")));
        h.put("EntryUserId", u.getId());
        h.put("GDNoofContrainers", 0);
        h.put("Id", recId);
        h.put("ModifyUserId", u.getId());
        h.put("OrganizationId", u.getOrganizationId());
        h.put("BankInvoiceNo", str(b.get("bankInvoiceNo")));
        h.put("GDNO", str(b.get("gdNo")));
        String remarks = str(b.get("remarks"));

        List<Map<String, Object>> details = new ArrayList<>();
        if (recId > 0) for (Map<String, Object> r : list(b.get("removed"))) details.add(detail(r, asInt(r.get("Id")), 3));
        BigDecimal gdValue = dec(b.get("gdValue")), invoiceAmount = BigDecimal.ZERO;
        String remarksFromDetail = "";
        int i = 0;
        for (Map<String, Object> r : grid) {
            int id = recId != 0 ? asInt(r.get("Id")) : 0;
            Map<String, Object> d = detail(r, id, id <= 0 ? 1 : 2);
            if (asInt(r.get("InvoiceId")) == 0) throw new IllegalArgumentException("Invoice is required in Detail Grid at row No: " + (i + 1));
            if (dec(r.get("InvoiceAmount")).signum() <= 0) throw new IllegalArgumentException("Invoice Amount is required in Detail Grid at row No: " + (i + 1));
            if (remarks.trim().isEmpty()) remarksFromDetail = text(r.get("Remarks"));
            details.add(d);
            invoiceAmount = invoiceAmount.add(dec(r.get("InvoiceAmount")));
            i++;
        }
        if (gdValue.compareTo(invoiceAmount) != 0) throw new IllegalArgumentException("Invoice's Amount must be equal to Total GDValue Please Check");
        if (remarks.isEmpty()) remarks = remarksFromDetail;
        h.put("Remarks", remarks);
        h.put("AttachmentsValues", str(b.get("attachmentsValues")));
        h.put("CustomAttachmentsValues", str(b.get("customAttachmentsValues")));
        int n = repo.save(h, details);
        return saved(n, recId == 0 ? "Record Save Successfully" : "Record Update Successfully");
    }

    /** FillDetailListCommonForInsertAndDelete - the GDBreakUpDetail model in SetProc order (InvoiceNo is virtual). */
    private static Map<String, Object> detail(Map<String, Object> r, int id, int actionTypeId) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("Id", id);
        d.put("GDBreakUpHeaderId", 0);
        d.put("ExImInvoiceId", asInt(r.get("InvoiceId")));
        d.put("InvoiceAmount", dec(r.get("InvoiceAmount")));
        d.put("Remarks", text(r.get("Remarks")));
        d.put("ActionTypeId", actionTypeId);
        return d;
    }

    private static String str(Object v) { return v == null ? "" : String.valueOf(v); }

    private interface Loader { Object load() throws Exception; }

    private static void put(Map<String, Object> out, String key, Loader l) {
        try { out.put(key, l.load()); }
        catch (Exception e) { out.put(key, new ArrayList<>()); out.put(key + "Error", msg(e)); }
    }
}
