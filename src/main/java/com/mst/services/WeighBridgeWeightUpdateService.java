package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.WeighBridgeExtrasRepository;
import com.mst.repositories.WeighBridgeExtrasRepository.HistoryFilter;
import com.mst.repositories.WeighBridgeExtrasRepository.WeightUpdate;
import com.mst.security.CurrentUserContext;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.sql.Timestamp;
import java.time.LocalDate;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.services.WeighBridgeLookupsService.ci;
import static com.mst.services.WeighBridgeLookupsService.dbl;
import static com.mst.services.WeighBridgeLookupsService.intOf;
import static com.mst.services.WeighBridgeLookupsService.str;

/**
 * {@code Architecture.WinApp.Inventory_Reports.WeighBridge_WeightUpdate} — opened from
 * Gear → Admin Panel → "WeighBridge_WeightsUpdate" (DashboardNew.cs:3120). It has no
 * ScreenDefinition row and no rights rows; the only gate on the desktop is that the Admin Panel
 * menu is drawn only when {@code UserAccount.RoleName == "Admin"} (DashboardNew.cs:1191). The
 * same gate is applied here to every endpoint, not just the page.
 *
 * Desktop facts kept as they are:
 *  - Load fills ONLY the Branch combo (BranchesFill). GatePassTypeFill, WbType, PartyNameFill,
 *    WbPartyFill, DocTypeFill and WbStatusFill exist in the form but are never called, so those
 *    filters are always empty and never narrow the search.
 *  - The search always sends @GpStatus = 'Open'.
 *  - USP_WbTransactions_WeightUpdation declares @UserId / @Comments / @WorkingReportNo but never
 *    reads them; the required comment is not stored anywhere. The procedure only changes
 *    anything when ERP feature 22 is on for the company — otherwise it returns without error
 *    and the desktop still says "Update completed Successfully!".
 */
@Service
public class WeighBridgeWeightUpdateService {

    @Autowired private WeighBridgeExtrasRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private WeighBridgeReportsService reports;

    private UserAccount requireAdmin() {
        UserAccount u = currentUserContext.requireAccountingUser();
        String role = null;
        try { role = currentUserContext.currentRoleName(); } catch (Exception ignored) { }
        if (!"Admin".equals(role)) {
            throw new org.springframework.security.access.AccessDeniedException(
                    "WeighBridge_WeightsUpdate is on the Admin Panel, which the desktop shows only when RoleName is \"Admin\".");
        }
        return u;
    }

    /** frmWeightBridgeHistory_Load → BranchesFill() only (plus what Reset/print need). */
    public Map<String, Object> lookups() {
        requireAdmin();
        Map<String, Object> full = reports.historyLookups();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("branches", full.get("branches"));
        out.put("userBranchId", full.get("userBranchId"));
        out.put("yearStart", full.get("yearStart"));
        out.put("companyName", full.get("companyName"));
        out.put("companyAddress", full.get("companyAddress"));
        return out;
    }

    /** toolStripButton1_Click ("Refresh") — BranchesFill() only. */
    public List<Map<String, Object>> branches() {
        UserAccount u = requireAdmin();
        return repo.branchesFromWeighBridge(u);
    }

    public static final class SearchRequest {
        public String fromDate, toDate;
        public String ticketNoFrom, ticketNoTo;
        public String gpNoFrom, gpNoTo;
        public String vehicleNo;
        public String orderNoFrom, orderNoTo;
        public List<Integer> branchIds;
    }

    /** grdweightBridge() — returns {rows: lstwb, grid: the dtWb projection}. */
    public Map<String, Object> search(SearchRequest r) {
        UserAccount u = requireAdmin();
        HistoryFilter f = new HistoryFilter();
        f.financialYearId = currentUserContext.currentFinancialYearId();
        f.fromDocNo = intOf(t(r.ticketNoFrom));
        f.toDocNo = intOf(t(r.ticketNoTo));
        f.gpSrNoF = intOf(t(r.gpNoFrom));
        f.gpSrNoT = intOf(t(r.gpNoTo));
        f.fromDate = day(r.fromDate);
        f.toDate = day(r.toDate);
        f.vehicleNo = t(r.vehicleNo);
        f.orderNoFrom = intOf(r.orderNoFrom);
        f.orderNoTo = intOf(r.orderNoTo);
        // The empty combos (never filled on the desktop) contribute nothing.
        f.activity = "";
        f.supplierCustomerId = 0;
        f.documentTypeId = 0;
        f.companyName = "";
        f.refDocumentTypeIds = "";
        StringBuilder b = new StringBuilder();
        if (r.branchIds != null) for (Integer i : r.branchIds) if (i != null && i > 0) b.append(',').append(i);
        f.branchesIds = b.toString();

        List<Map<String, Object>> rows = repo.historyForUpdating(u, f, "Open");
        List<Map<String, Object>> grid = new ArrayList<>();
        for (Map<String, Object> x : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(ci(x, "Id")));
            m.put("DocTypeId", intOf(ci(x, "DocTypeId")));
            m.put("BranchName", str(ci(x, "BranchName")));
            m.put("DocumentTypeDescription", str(ci(x, "DocumentTypeDescription")));
            m.put("DocDate", ci(x, "DocDate"));
            m.put("TicketNo.", intOf(ci(x, "TicketNo")));
            m.put("OrderNo", intOf(ci(x, "OrderNo")));
            m.put("GpNo", intOf(ci(x, "GpSrNo")));
            m.put("PartyName", str(ci(x, "PartyName")));
            m.put("VehicleNo", str(ci(x, "VehicleNo")));
            m.put("BiltyNo", intOf(ci(x, "BiltyNo")));
            m.put("SupplierWeight", dbl(ci(x, "SupplierWeight")));
            m.put("FirstWeight", dbl(ci(x, "FirstWeight")));
            m.put("SecondWeight", dbl(ci(x, "SecondWeight")));
            m.put("NetWbWeight", dbl(ci(x, "NetWbWeight")));
            m.put("FirstDateTime", ci(x, "FirstDateTime"));
            m.put("SecondDateTime", ci(x, "SecondDateTime"));
            m.put("ItemDescription", str(ci(x, "ItemDescription")));
            m.put("ItemQty", dbl(ci(x, "ItemQty")));
            m.put("WeighBridgeType", str(ci(x, "WeighBridgeType")));
            m.put("WbCharges", dbl(ci(x, "WbCharges")));
            m.put("WbRemarks", str(ci(x, "WbRemarks")));
            m.put("EntryUser", str(ci(x, "FirstNameEusr")));
            m.put("EntryDate", ci(x, "EntryDate"));
            grid.add(m);
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rows", rows);
        out.put("grid", grid);
        return out;
    }

    public static final class RowEdit {
        public int id;
        public String firstWeight;
        public String secondWeight;
    }

    public static final class UpdateRequest {
        public boolean single;          // the row "Update" button (UpdateSingleRecord) vs btnUpdate
        public String comments;
        public List<RowEdit> rows;
    }

    /**
     * UpdateSingleRecord():845 and btnUpdate_Click():922, with their messages and their order of
     * checks. The weights are compared with the ticket's stored values (the desktop compares with
     * the list it loaded, which holds the same values unless someone changed the ticket since).
     * A second weight only counts as changed when the stored second weight is not 0.
     */
    public Map<String, Object> update(UpdateRequest r) {
        UserAccount u = requireAdmin();
        List<RowEdit> edits = r.rows == null ? new ArrayList<>() : r.rows;
        String comments = r.comments == null ? "" : r.comments;
        Map<String, Object> out = new LinkedHashMap<>();

        if (r.single) {
            if (edits.size() != 1) throw new IllegalArgumentException("Record Not Found");
            RowEdit e = edits.get(0);
            double fw = number(e.firstWeight), sw = number(e.secondWeight);
            Map<String, Object> original = owned(u, e.id);
            int action = action(original, fw, sw);
            if (action == 3) throw new IllegalArgumentException("No Change Found!");
            if (comments.isEmpty()) throw new IllegalArgumentException("Comments Field Required");
            List<WeightUpdate> list = new ArrayList<>();
            list.add(upd(e.id, action, fw, sw, comments));
            repo.weightUpdation(u, list);
            out.put("success", true);
            out.put("message", "Update completed Successfully!");
            return out;
        }

        if (edits.isEmpty()) throw new IllegalArgumentException("Checked Rows First then update!");
        if (comments.isEmpty()) throw new IllegalArgumentException("Comments Field Required");
        List<WeightUpdate> list = new ArrayList<>();
        String ticketNos = "";
        for (RowEdit e : edits) {
            double fw = number(e.firstWeight), sw = number(e.secondWeight);
            Map<String, Object> original = owned(u, e.id);
            int action = action(original, fw, sw);
            if (action == 3) { ticketNos += ","; continue; }     // as written: only a comma is added
            list.add(upd(e.id, action, fw, sw, comments));
        }
        if (list.isEmpty()) {                                     // desktop: no call, no message
            out.put("success", true);
            out.put("message", null);
            return out;
        }
        repo.weightUpdation(u, list);
        out.put("success", true);
        out.put("message", ticketNos.isEmpty()
                ? "Update completed Successfully!"
                : "Update completed successfully! However, some rows with Ticket Number's(" + ticketNos
                  + ") were not updated because no changes were detected.");
        return out;
    }

    /** 0 = both changed, 1 = first only, 2 = second only, 3 = no change. */
    private static int action(Map<String, Object> original, double fw, double sw) {
        double of = dbl(ci(original, "FirstWeight"));
        double os = dbl(ci(original, "SecondWeight"));
        boolean first = fw != of;
        boolean second = sw != os && os != 0d;
        if (first && second) return 0;
        if (first) return 1;
        if (second) return 2;
        return 3;
    }

    /** The procedure takes the organisation and company from the row itself, so a ticket of
     *  another company is refused here before it reaches it. */
    private Map<String, Object> owned(UserAccount u, int id) {
        Map<String, Object> row = id > 0 ? repo.wbTransactionById(id) : null;
        if (row == null
                || intOf(ci(row, "CompanyId")) != (u.getCompanyId() == null ? 0 : u.getCompanyId())
                || intOf(ci(row, "OrganizationId")) != (u.getOrganizationId() == null ? 0 : u.getOrganizationId())) {
            throw new IllegalArgumentException("Record Not Found");
        }
        return row;
    }

    private static WeightUpdate upd(int id, int action, double fw, double sw, String comments) {
        WeightUpdate w = new WeightUpdate();
        w.id = id;
        w.actionId = action;
        w.firstWeight = fw;
        w.secondWeight = sw;
        w.comments = comments;
        return w;
    }

    /** DataGridHistory_UpdatingCell — only numbers go into the weight cells. */
    private static double number(String s) {
        String v = s == null ? "" : s.trim().replace(",", "");
        if (v.isEmpty()) return 0d;
        try { return Double.parseDouble(v); }
        catch (NumberFormatException e) { throw new IllegalArgumentException("Please Type Only Numeric Value"); }
    }

    private static String t(String s) { return s == null ? "" : s.trim(); }

    private static Timestamp day(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return Timestamp.valueOf(LocalDate.parse(s.trim().substring(0, 10)).atStartOfDay()); }
        catch (Exception e) { throw new IllegalArgumentException("Invalid date: " + s); }
    }
}
