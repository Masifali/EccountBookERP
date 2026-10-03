package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportFcyGdUtilizationRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.ExportBankGdSupport.*;

/**
 * The BLL side of 953 frmGdUtilizationAgainstFcyReceipt "Fcy Receipts Utilization Against Bank
 * Invoice / GD (Fcy Receipts Mapping to GD)" (Architecture.WinApp.Export). tabControl1 Form |
 * History. Form: toolStrip1 New / Refresh(no-op) / Save / Update(hidden) / Delete(hidden) /
 * Attachment(hidden) / 901-Print(hidden) / ShortCut Keys, "Print Slip" check; groupBox2 "Main"
 * (Receipt No & Date, Fcy Amount, Exchange Rate, Lcy Amount - all disabled, filled by the pending
 * grid's Load button); tabControl2 "Detail" (grd: editable UtilizingAmount / FTT Commission /
 * FDBCNo, "Auto Utilize"); groupBox1 "Filters" (Customer Name + Show) over the "Pending Receipts"
 * grid. History: Date Type / Date From / Date To / Show, "Filtered Records" (X = delete the
 * receipt's utilisation) and "GD Utilizing Info" for the selected row.
 *
 * Rights by ScreenDefinition.Id 953: Save, Update, Delete (btnDelete), Print (btnSlip / ChkPrintslip).
 */
@Service
public class ExportFcyGdUtilizationService {

    public static final int SCREEN_ID = 953;
    public static final int REF_DOCUMENT_TYPE_ID = 4;

    @Autowired private ExportFcyGdUtilizationRepository repo;
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

    /** InitializeComponentMethod: rights, DefaultDaysToLessFromHistoryFromDate, pending receipts. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Delete", allowed(u, "Delete"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        int days;
        try { days = (int) Double.parseDouble(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")); }
        catch (NumberFormatException e) { days = 0; }
        out.put("defaultDaysToLessFromHistoryFromDate", days);
        put(out, "pending", () -> pendingRows(repo.pendingReceipts(u)));
        return out;
    }

    /** FillGrdPendingOrders -> dtPendingVouchers columns. */
    public List<Map<String, Object>> pending() { return pendingRows(repo.pendingReceipts(user("View"))); }

    private static List<Map<String, Object>> pendingRows(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(receiptRow(r));
        return out;
    }

    private static Map<String, Object> receiptRow(Map<String, Object> r) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("Id", asInt(ci(r, "Id")));
        m.put("ReceiptDate", iso(ci(r, "DocumentDate")));
        m.put("ReceiptCode", text(ci(r, "DocumentNo")));
        m.put("SupplierCustomerId", asInt(ci(r, "SupplierCustomerId")));
        m.put("Customer", text(ci(r, "CustomerName")));
        m.put("PaymentTerm", text(ci(r, "ExmLcPaymentTerm")));
        m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
        m.put("BankName", text(ci(r, "BankName")));
        m.put("FcyCode", text(ci(r, "FcyCode")));
        m.put("ExchangeRate", asDouble(ci(r, "ExchangeRate")));
        m.put("FcyAmount", asDouble(ci(r, "FcyAmount")));
        m.put("LcyAmount", asDouble(ci(r, "LcyAmount")));
        return m;
    }

    /** GetGdData(FcyReceiptId) -> dtGrid rows (RefDocumentTypeId 4, UtilizingAmount 0, BalanceAmount = GDBalance). */
    public List<Map<String, Object>> gds(int fcyReceiptId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.gdsForMapping(u)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", 0);
            m.put("FcyBankReceiptId", fcyReceiptId);
            m.put("RefDocumentTypeId", REF_DOCUMENT_TYPE_ID);
            m.put("InvoiceId", asInt(ci(r, "ExImInvoiceId")));
            m.put("InvoiceNo", text(ci(r, "BankInvoiceNo")));
            m.put("GdRefDocTypeId", asInt(ci(r, "DocumentTypeId")));
            m.put("GDId", asInt(ci(r, "Id")));
            m.put("GDNo", text(ci(r, "GDNO")));
            m.put("GDValue", asDouble(ci(r, "GDValue")));
            m.put("AdvanceUtilize", asDouble(ci(r, "AdvanceUtilize")));
            m.put("UtilizedAmount", asDouble(ci(r, "FcyRealized")));
            m.put("GDBalance", asDouble(ci(r, "GDBalance")));
            m.put("UtilizingAmount", 0d);
            m.put("Commission", asDouble(ci(r, "FTTAmount")));
            m.put("FDBCNo", "");
            m.put("PrevSavedStepStatus", text(ci(r, "GdStepStatus")));
            m.put("StepStatus", "");
            m.put("BalanceAmount", asDouble(ci(r, "GDBalance")));
            out.add(m);
        }
        return out;
    }

    /** FillHistory -> the grdHistory columns (same shape as the pending grid). */
    public List<Map<String, Object>> history(String fromDate, String toDate) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.history(u, sqlDate(fromDate), sqlDate(toDate))) out.add(receiptRow(r));
        return out;
    }

    /** GetGdInfoAgainstReceipt(Id) -> dtDetail of GridGdUtilizedHistory. */
    public List<Map<String, Object>> utilizationByReceipt(int id) {
        user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.utilizationByReceipt(id)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("GDId", asInt(ci(r, "GDId")));
            m.put("GdRefDocTypeId", asInt(ci(r, "GdRefDocTypeId")));
            m.put("RefDocRecordId", asInt(ci(r, "RefDocRecordId")));
            m.put("GDNo", text(ci(r, "GDNo")));
            m.put("GDValue", asDouble(ci(r, "GdValue")));
            m.put("InvoiceNo", text(ci(r, "InvoiceNo")));
            m.put("FDBCNo", text(ci(r, "ReceiverRefNo")));
            m.put("RealizedAmount", asDouble(ci(r, "RealizedAmount")));
            m.put("AgencyComm", asDouble(ci(r, "AgencyComm")));
            m.put("StepStatus", text(ci(r, "StepStatus")));
            out.add(m);
        }
        return out;
    }

    /**
     * ReadById(Id) on the desktop: Reset, GetGDUtilizationHistoryByFcyReceipt, then dtGrid.Rows.Add with
     * dataRow["RefDocumentType"] - a column the procedure does not return - so every non-empty result
     * ends in MessageBox "Column 'RefDocumentType' does not belong to table ." after the form was reset.
     * Reproduced: an empty result is a silent no-op, a non-empty one is that message.
     */
    public Map<String, Object> readById(int id) {
        user("View");
        List<Map<String, Object>> rows = repo.utilizationByReceipt(id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows.size());
        if (!rows.isEmpty()) out.put("message", "Column 'RefDocumentType' does not belong to table .");
        return out;
    }

    /**
     * Insert(): "Grid Record Not Found", FormValidation ("Fcy Amount Field is Required"), rows with Id > 0 or
     * UtilizingAmount > 0 -> FcyBankReceiptBreakUp (Id = RecId != 0 ? Id : 0, ActionTypeId 1/2, RefDocumentTypeId
     * 4, RefDocRecordId = InvoiceId, GdRefDocTypeId, GDId, RealizedAmount = UtilizingAmount, AgencyComm =
     * Commission, ReceiverRefNo = FDBCNo, StepStatus) with the row checks in the desktop's order; "At least
     * enter one row in detail "; removed rows (ActionTypeId 3) appended when RecId > 0; the 4-decimal total
     * must equal the receipt amount. obj.GDNo is never set, so the success text ends in "[] ".
     */
    public Map<String, Object> save(Map<String, Object> body) {
        int recId = asInt(body.get("recId"));
        user(recId > 0 ? "Update" : "Save");
        List<Map<String, Object>> grid = list(body.get("rows"));
        if (grid.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found");
        String fcyText = text(body.get("fcyAmount"));
        if (fcyText.isEmpty() || "0".equals(fcyText)) throw new IllegalArgumentException("Fcy Amount Field is Required");

        List<Map<String, Object>> items = new ArrayList<>();
        BigDecimal total = BigDecimal.ZERO;
        int receiptId = 0;
        for (int i = 0; i < grid.size(); i++) {
            Map<String, Object> r = grid.get(i);
            if (!(asInt(r.get("Id")) > 0 || dec(r.get("UtilizingAmount")).signum() > 0)) continue;
            Map<String, Object> d = new LinkedHashMap<>();
            int id = recId != 0 ? asInt(r.get("Id")) : 0;
            d.put("Id", id);
            d.put("FcyBankReceiptId", asInt(r.get("FcyBankReceiptId")));
            receiptId = asInt(r.get("FcyBankReceiptId"));
            d.put("SenderRefNo", null);
            d.put("ReceiverRefNo", text(r.get("FDBCNo")));
            d.put("RefDocumentTypeId", REF_DOCUMENT_TYPE_ID);
            d.put("RefDocRecordId", asInt(r.get("InvoiceId")));
            d.put("RealizedAmount", dec(r.get("UtilizingAmount")));
            d.put("AgencyComm", dec(r.get("Commission")));
            d.put("StepStatus", text(r.get("StepStatus")));
            d.put("SortNo", 0);
            d.put("ActionTypeId", id <= 0 ? 1 : 2);
            d.put("GDId", asInt(r.get("GDId")));
            d.put("GdRefDocTypeId", asInt(r.get("GdRefDocTypeId")));
            total = total.add((BigDecimal) d.get("RealizedAmount"));
            int rowNo = i + 1;
            if (asInt(d.get("RefDocRecordId")) == 0) throw new IllegalArgumentException("Invoice Id Required in Detail Grid And row No: " + rowNo);
            if (((BigDecimal) d.get("RealizedAmount")).signum() == 0) throw new IllegalArgumentException("Realized Required in Detail Grid And row No: " + rowNo);
            if (asInt(d.get("GDId")) == 0) throw new IllegalArgumentException("GDId Required in Detail Grid And row No: " + rowNo);
            if (text(d.get("ReceiverRefNo")).isEmpty()) throw new IllegalArgumentException("FDBC No Required in Detail Grid And row No: " + rowNo);
            items.add(d);
        }
        if (items.isEmpty()) throw new IllegalArgumentException("At least enter one row in detail ");
        if (recId > 0) {
            for (Map<String, Object> r : list(body.get("removed"))) {
                Map<String, Object> d = new LinkedHashMap<>();
                d.put("Id", asInt(r.get("Id")));
                d.put("FcyBankReceiptId", asInt(r.get("FcyBankReceiptId")));
                d.put("SenderRefNo", null);
                d.put("ReceiverRefNo", text(r.get("FDBCNo")));
                d.put("RefDocumentTypeId", REF_DOCUMENT_TYPE_ID);
                d.put("RefDocRecordId", asInt(r.get("InvoiceId")));
                d.put("RealizedAmount", dec(r.get("UtilizingAmount")));
                d.put("AgencyComm", dec(r.get("Commission")));
                d.put("StepStatus", text(r.get("StepStatus")));
                d.put("SortNo", 0);
                d.put("ActionTypeId", 3);
                d.put("GDId", asInt(r.get("GDId")));
                d.put("GdRefDocTypeId", asInt(r.get("GdRefDocTypeId")));
                items.add(d);
            }
        }
        total = total.setScale(4, RoundingMode.HALF_UP);
        BigDecimal fcy = dec(fcyText);
        if (total.compareTo(fcy) != 0)
            throw new IllegalArgumentException("Total Utilizing Amount " + total.stripTrailingZeros().toPlainString() + " In Grid should be equal to receipt amount " + fcy.stripTrailingZeros().toPlainString());
        int result = repo.save(receiptId, items);
        return ok(result, recId == 0 ? "Record Saved Successfully of GD No [] " : "Record Update Successfully of GD No [] ");
    }

    /** grdHistory "X" -> DeleteRecordAgainstFcyReceipt(Id); the desktop guards this button with no right beyond opening the form. */
    public Map<String, Object> deleteByReceipt(int id) {
        user("View");
        repo.deleteByFcyReceipt(id);
        return ok(id, "Deleted");
    }
}
