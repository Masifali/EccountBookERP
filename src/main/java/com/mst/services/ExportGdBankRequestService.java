package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.ExportGdBankRequestRepository;
import com.mst.security.CurrentUserContext;
import com.mst.security.DesktopReportRights;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.ExportBankGdSupport.*;

/**
 * The BLL side of 197 GdBankRequest "Gd Bank Request" (Architecture.WinApp.Export). tabControl1
 * Form | History. Form: toolStrip1 New / Refresh / Save / Update(hidden) / Print-560 / Attachment /
 * ShortCut Keys; groupBox1 "Main Information" (Doc No disabled, Doc Date, Bank Name, Remarks);
 * groupBox2 "Detail Information" (Amount Rcvd, GD #, Invoice #, GD Value, FOB Value, Balance,
 * Realized Value, FTT, Status, Remarks, + / Update / Cancel); grdDetail (X column, 2 frozen).
 * History: New / Refresh / Print(no handler); Filters (Doc/Entry/Modify/Approved Date radios,
 * From/To Date, From/To Doc No, Bank Name, Show); "Filtered Records" grid (Edit / Slip buttons,
 * NoOfAttachments link) and "Detail Information of Above Selected Row".
 *
 * Rights by ScreenDefinition.Id 197: btnSave = Save, btnUpdate = Update, BtnPrint = Print. The
 * grid arithmetic (Calculation / CalculateRealizedFromGrid / CheckValidation / SubCode grouping)
 * lives on the page; the header validations and the SubCode = Realized-sum check of Insert() are
 * repeated here so a hand-made request cannot skip them.
 */
@Service
public class ExportGdBankRequestService {

    public static final int SCREEN_ID = 197;

    @Autowired private ExportGdBankRequestRepository repo;
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

    /** GdBankRequest_Load: rights, GenerateCode, BankFill, HistoryBankFill, DefaultDaysToLessFromHistoryFromDate. */
    public Map<String, Object> setup() {
        UserAccount u = user("View");
        Map<String, Object> out = new LinkedHashMap<>();
        Map<String, Object> perm = new LinkedHashMap<>();
        perm.put("Save", allowed(u, "Save"));
        perm.put("Update", allowed(u, "Update"));
        perm.put("Print", allowed(u, "Print"));
        out.put("permissions", perm);
        put(out, "docNo", () -> generateCode(u));
        put(out, "banks", () -> idName(repo.banks(u)));
        put(out, "historyBanks", () -> idName(repo.historyBanks(u)));
        int days;
        try { days = (int) Double.parseDouble(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")); }
        catch (NumberFormatException e) { days = 0; }
        out.put("defaultDaysToLessFromHistoryFromDate", days);
        return out;
    }

    /** GenerateCode: DocNo of the GenerateCode activity, shown only when > 0. */
    public int generateCode(UserAccount u) {
        List<Map<String, Object>> r = repo.generateCode(u);
        return r.isEmpty() ? 0 : asInt(ci(r.get(0), "DocNo"));
    }

    public Map<String, Object> generateCode() {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("docNo", generateCode(user("View")));
        return out;
    }

    /** BankFill / HistoryBankFill: (Id, name) -> (Id, Name). */
    private static List<Map<String, Object>> idName(List<Map<String, Object>> rows) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("Name", text(ci(r, "name")));
            out.add(m);
        }
        return out;
    }

    /** btnRefresh_Click -> BankFill. */
    public List<Map<String, Object>> banks() { return idName(repo.banks(user("View"))); }

    /** btnRefreshHistory_Click -> HistoryBankFill. */
    public List<Map<String, Object>> historyBanks() { return idName(repo.historyBanks(user("View"))); }

    /** GetGdsAgainstBank(BankId) -> dtGd (Id, GdNo, InvoiceNo, GdValue, GdBalance, Status, FobValue, RefDocumentTypeId). */
    public List<Map<String, Object>> gds(int bankId, int recId) {
        UserAccount u = user("View");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.gdsAgainstBank(u, bankId, recId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("GdNo", text(ci(r, "GDNO")));
            m.put("InvoiceNo", text(ci(r, "BankInvoiceNo")));
            m.put("GdValue", asDouble(ci(r, "GDValue")));
            m.put("GdBalance", asDouble(ci(r, "GDBalance")));
            m.put("Status", asInt(ci(r, "GdStatus")));
            m.put("FobValue", asDouble(ci(r, "FobValue")));
            m.put("RefDocumentTypeId", asInt(ci(r, "DocumentTypeId")));
            out.add(m);
        }
        return out;
    }

    /** ReadById(ID): GetByID = header + the detail list (GetAll fills GdBreakUpBankRequestDetailList per header). */
    public Map<String, Object> readById(int id) {
        user("View");
        List<Map<String, Object>> h = repo.headerById(id);
        if (h.isEmpty()) throw new IllegalStateException("Index was out of range. Must be non-negative and less than the size of the collection.");
        Map<String, Object> r = h.get(0);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("Id", asInt(ci(r, "Id")));
        out.put("DocDate", iso(ci(r, "DocDate")));
        out.put("DocNo", asInt(ci(r, "DocNo")));
        out.put("BankId", asInt(ci(r, "BankId")));
        out.put("RemarksHeader", text(ci(r, "RemarksHeader")));
        out.put("details", detailRows(id));
        return out;
    }

    /** dtDetail columns from the detail list (GdBalance is 0.00 from the procedure). */
    private List<Map<String, Object>> detailRows(int headerId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.detailByHeaderId(headerId)) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(d, "Id")));
            m.put("SubCode", asInt(ci(d, "SubCode")));
            m.put("AmountRcvd", asDouble(ci(d, "BankReceivedAmount")));
            m.put("GdId", asInt(ci(d, "GDId")));
            m.put("RefDocumentTypeId", asInt(ci(d, "RefDocumentTypeId")));
            m.put("GdNo", text(ci(d, "GDNo")));
            m.put("GdValue", asDouble(ci(d, "GdValue")));
            m.put("FobValue", asDouble(ci(d, "FobValue")));
            m.put("GdBalance", asDouble(ci(d, "GdBalance")));
            m.put("Realized", asDouble(ci(d, "RealizedAmount")));
            m.put("FTT", asDouble(ci(d, "CommAmount")));
            m.put("Status", text(ci(d, "Status")));
            m.put("Remarks", text(ci(d, "RemarksDetail")));
            m.put("BankInvoiceNo", text(ci(d, "BankInvoiceNo")));
            out.add(m);
        }
        return out;
    }

    /** DataGridHistory_SelectionChanged -> GetByID(Id) detail list into the lower grid. */
    public List<Map<String, Object>> historyDetail(int id) { user("View"); return detailRows(id); }

    /** HistoryFill -> dtHistory columns. */
    public List<Map<String, Object>> history(Map<String, Object> body) {
        UserAccount u = user("View");
        String kind = text(body.get("dateKind"));
        java.sql.Date from = asBool(body.get("fromChecked")) ? sqlDate(body.get("fromDate")) : null;
        java.sql.Date to = asBool(body.get("toChecked")) ? sqlDate(body.get("toDate")) : null;
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(u, kind.isEmpty() ? "doc" : kind, from, to,
                asInt(body.get("fromDocNo")), asInt(body.get("toDocNo")), asInt(body.get("bankId")))) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", asInt(ci(r, "Id")));
            m.put("DocNo", asInt(ci(r, "DocNo")));
            m.put("DocDate", iso(ci(r, "DocDate")));
            m.put("BankName", text(ci(r, "BankName")));
            m.put("EntryDate", iso(ci(r, "EntryDate")));
            m.put("EntryUser", text(ci(r, "EntryUserName")));
            m.put("ModifyDate", iso(ci(r, "ModifyDate")));
            m.put("ModifyUser", text(ci(r, "ModifyUserName")));
            m.put("NoOfAttachments", asInt(ci(r, "NoOfAttachments")));
            m.put("Remarks", text(ci(r, "RemarksHeader")));
            out.add(m);
        }
        return out;
    }

    /**
     * Insert(): formvalidation (Doc No, Bank, "You Cant Save Record Beacuse Bank Recieved Amount Is not
     * Completly Proportionated" when the page's Amount Rcvd box is disabled), "Grid Record not found",
     * per-SubCode AmountRcvd == Sum(Realized) ("Amount Received {a} not Equal To Total Realized {s}"),
     * then the header + (removed rows with ActionTypeId 3 when updating) + grid rows (Id <= 0 -> 1 else 2).
     */
    public Map<String, Object> save(Map<String, Object> body) {
        int recId = asInt(body.get("recId"));
        UserAccount u = user(recId == 0 ? "Save" : "Update");
        int docNo = asInt(body.get("docNo"));
        if (docNo == 0) throw new IllegalArgumentException("Doc No Is Required");
        int bankId = asInt(body.get("bankId"));
        if (bankId <= 0) throw new IllegalArgumentException("Bank Is Required");
        if (!asBool(body.get("amountRcvdEnabled"))) throw new IllegalArgumentException("You Cant Save Record Beacuse Bank Recieved Amount Is not Completly Proportionated");
        List<Map<String, Object>> rows = list(body.get("rows"));
        List<Map<String, Object>> removed = list(body.get("removed"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record not found");

        Set<Integer> subCodes = new LinkedHashSet<>();
        for (Map<String, Object> r : rows) subCodes.add(asInt(r.get("SubCode")));
        for (int sc : subCodes) {
            double sum = 0, rcvd = 0; boolean first = true;
            for (Map<String, Object> r : rows) {
                if (asInt(r.get("SubCode")) != sc) continue;
                sum += asDouble(r.get("Realized"));
                if (first) { rcvd = asDouble(r.get("AmountRcvd")); first = false; }
            }
            if (rcvd != sum) throw new IllegalArgumentException("Amount Received " + num(rcvd) + " not Equal To Total Realized " + num(sum));
        }

        Timestamp now = new Timestamp(System.currentTimeMillis());
        Map<String, Object> h = new LinkedHashMap<>();
        h.put("Id", recId > 0 ? recId : 0);
        h.put("DocDate", ts(body.get("docDate")));
        h.put("DocNo", docNo);
        h.put("BankId", bankId);
        h.put("RemarksHeader", text(body.get("remarks")));
        h.put("EntryDate", now);
        h.put("EntryUserId", u.getId());
        h.put("ModifyDate", now);
        h.put("ModifyUserId", u.getId());
        h.put("OrganizationId", u.getOrganizationId());
        h.put("CompanyId", u.getCompanyId());
        h.put("ActionId", recId == 0 ? 1 : 2);

        List<Map<String, Object>> details = new ArrayList<>();
        if (recId > 0) {
            for (Map<String, Object> r : removed) {
                /* grdDetail_ColumnButtonClick "Delete": Id, BankReceivedAmount, GDId, RealizedAmount, CommAmount, Status, RemarksDetail, ActionTypeId 3
                   (SubCode and RefDocumentTypeId are not set on the removed model -> 0). */
                Map<String, Object> d = detail(r);
                d.put("SubCode", 0);
                d.put("RefDocumentTypeId", 0);
                d.put("ActionTypeId", 3);
                details.add(d);
            }
        }
        for (Map<String, Object> r : rows) {
            Map<String, Object> d = detail(r);
            d.put("ActionTypeId", asInt(r.get("Id")) <= 0 ? 1 : 2);
            details.add(d);
        }
        int id = repo.save(h, details);
        return ok(id, recId == 0 ? "Save SuccessFully" : "Update SuccessFully");
    }

    /** GDBreakUpBankRequestDetail non-virtual properties (SetProc sends every one). */
    private static Map<String, Object> detail(Map<String, Object> r) {
        Map<String, Object> d = new LinkedHashMap<>();
        d.put("Id", asInt(r.get("Id")));
        d.put("GDBreakUpBankRequestHeaderId", 0);
        d.put("BankReceivedAmount", dec(r.get("AmountRcvd")));
        d.put("GDId", asInt(r.get("GdId")));
        d.put("SubCode", asInt(r.get("SubCode")));
        d.put("RealizedAmount", dec(r.get("Realized")));
        d.put("CommAmount", dec(r.get("FTT")));
        d.put("Status", text(r.get("Status")));
        d.put("RemarksDetail", text(r.get("Remarks")));
        d.put("ActionTypeId", 0);
        d.put("RefDocumentTypeId", asInt(r.get("RefDocumentTypeId")));
        return d;
    }

    /** Conversion.ToString of a double for the warning text. */
    private static String num(double v) {
        if (v == Math.rint(v) && Math.abs(v) < 1e15) return String.valueOf((long) v);
        return String.valueOf(v);
    }
}
