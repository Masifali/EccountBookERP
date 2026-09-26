package com.mst.services;

import com.mst.models.Company;
import com.mst.models.UserAccount;
import com.mst.repositories.ICompanyRepository;
import com.mst.repositories.WeighBridgeExtrasRepository;
import com.mst.repositories.WeighBridgeExtrasRepository.HistoryFilter;
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
 * Module 27 — the two weigh bridge reports.
 *
 *  359 frmWeighBridgeRejectedTicketNos "WeighBridge History For Rejected Status"
 *  360 frmWeightBridgeHistory           "Weigh Bridge Report"
 *
 * Neither form checks a right in its own code; opening the screen is governed by the View right
 * through the sidebar, as for every other screen. Organisation, company, user and year come from
 * the session.
 */
@Service
public class WeighBridgeReportsService {

    /** Both desktop calls hard-code ReferenceDocTypeId = 74 (Move Order). */
    private static final int MOVE_ORDER = 74;

    @Autowired private WeighBridgeExtrasRepository repo;
    @Autowired private CurrentUserContext currentUserContext;
    @Autowired private ICompanyRepository companies;

    // ===================================================== 359 frmWeighBridgeRejectedTicketNos

    /** GridFill():57. Columns: Id, DocDate, TicketNo, DocumentTypeDescription, ItemQty,
     *  FirstWeight, SecondWeight, NetWbWeight, VehicleNo, BiltyNo, ItemDescription, WbRemarks, WbCharges. */
    public List<Map<String, Object>> rejectableTickets() {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.rejectableTickets(u, MOVE_ORDER);
    }

    /** grdHistory_ColumnButtonClick "Reject". The procedure's UPDATE is scoped to the session's
     *  organisation / company and to ReferenceDocTypeId 74, so a foreign id changes nothing; the
     *  id is also checked against the list the screen shows. */
    public Map<String, Object> reject(int id) {
        UserAccount u = currentUserContext.requireAccountingUser();
        boolean listed = false;
        for (Map<String, Object> r : repo.rejectableTickets(u, MOVE_ORDER)) {
            if (intOf(ci(r, "Id")) == id) { listed = true; break; }
        }
        if (!listed) throw new IllegalArgumentException("Record Not Found");
        repo.rejectTicket(u, MOVE_ORDER, id);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", "Record Rejected Successfully");
        out.put("rows", repo.rejectableTickets(u, MOVE_ORDER));
        return out;
    }

    // ============================================================ 360 frmWeightBridgeHistory

    /** frmWeightBridgeHistory_Load — BranchesFill, GatePassTypeFill, WbType, PartyNameFill,
     *  WbPartyFill, DocTypeFill; plus what Reset() needs (the year start). */
    public Map<String, Object> historyLookups() {
        UserAccount u = currentUserContext.requireAccountingUser();
        int fy = currentUserContext.currentFinancialYearId();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("branches", repo.branchesFromWeighBridge(u));
        out.put("userBranchId", u.getBranchesId() == null ? 0 : u.getBranchesId());
        out.put("gatePassTypes", repo.gatePassReferenceTypes(u));
        out.put("weighBridgeTypes", repo.weighBridgeTypes(u));
        out.put("parties", repo.partiesFromWbTransactions(u));
        out.put("wbParties", repo.wbParties(u));
        List<Map<String, Object>> docTypes = new ArrayList<>();
        docTypes.add(row("Id", 102, "DocType", "Auto"));
        docTypes.add(row("Id", 113, "DocType", "Manual"));
        out.put("docTypes", docTypes);
        Object start = repo.financialYearStart(u, fy);
        out.put("yearStart", start == null ? null : str(start).substring(0, Math.min(10, str(start).length())));
        String[] co = company(u);
        out.put("companyName", co[0]);
        out.put("companyAddress", co[1]);
        return out;
    }

    /** toolStripButton1_Click ("Refresh") — PartyNameFill() and WbType() only. */
    public Map<String, Object> historyRefresh() {
        UserAccount u = currentUserContext.requireAccountingUser();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("parties", repo.partiesFromWbTransactions(u));
        out.put("weighBridgeTypes", repo.weighBridgeTypes(u));
        return out;
    }

    public static final class HistoryRequest {
        public String fromDate, toDate;           // yyyy-MM-dd
        public String ticketNoFrom, ticketNoTo;
        public String gpNoFrom, gpNoTo;
        public String vehicleNo;
        public String weighBridgeType;            // CmbWeighBridgeType.Text
        public int supplierCustomerId;
        public String orderNoFrom, orderNoTo;
        public int documentTypeId;
        public String wbPartyName;                // CmbWeighBridgeParty.Text
        public List<Integer> gatePassTypeIds;     // checked CmbGatePassType rows
        public List<Integer> branchIds;           // checked CmbBranch rows
    }

    /**
     * grdweightBridge():230. Returns {rows: the raw procedure rows (the 281 register prints these,
     * as lstwb), grid: the dtWb projection the desktop shows}.
     *
     * CmbBranch is a checked-list combo whose Value is the list of checked items, so
     * Conversion.ToInt(CmbBranch.Value) is 0 and @BranchId is not sent; the checked branches go
     * as @BranchesIds. The id lists keep the desktop's leading comma (",3,4").
     */
    public Map<String, Object> history(HistoryRequest r) {
        UserAccount u = currentUserContext.requireAccountingUser();
        HistoryFilter f = new HistoryFilter();
        f.financialYearId = currentUserContext.currentFinancialYearId();
        f.fromDocNo = intOf(t(r.ticketNoFrom));
        f.toDocNo = intOf(t(r.ticketNoTo));
        f.gpSrNoF = intOf(t(r.gpNoFrom));
        f.gpSrNoT = intOf(t(r.gpNoTo));
        f.fromDate = day(r.fromDate);
        f.toDate = day(r.toDate);
        f.vehicleNo = t(r.vehicleNo);
        f.activity = r.weighBridgeType == null ? "" : r.weighBridgeType;
        f.branchesId = 0;
        f.supplierCustomerId = r.supplierCustomerId;
        f.orderNoFrom = intOf(r.orderNoFrom);
        f.orderNoTo = intOf(r.orderNoTo);
        f.documentTypeId = (r.documentTypeId == 102 || r.documentTypeId == 113) ? r.documentTypeId : 0;
        f.companyName = r.wbPartyName == null ? "" : r.wbPartyName;
        f.refDocumentTypeIds = idList(r.gatePassTypeIds);
        f.branchesIds = idList(r.branchIds);

        List<Map<String, Object>> rows = repo.history(u, f);
        List<Map<String, Object>> grid = new ArrayList<>();
        for (Map<String, Object> x : rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            m.put("Id", intOf(ci(x, "Id")));
            m.put("DocTypeId", intOf(ci(x, "DocTypeId")));
            m.put("ReferenceDocTypeId", intOf(ci(x, "ReferenceDocTypeId")));
            m.put("ReferenceDocNoId", intOf(ci(x, "ReferenceDocNoId")));
            m.put("BranchName", str(ci(x, "BranchName")));
            m.put("DocumentTypeDescription", str(ci(x, "DocumentTypeDescription")));
            m.put("WeighBridgeType", str(ci(x, "WeighBridgeType")));
            m.put("DocDate", ci(x, "DocDate"));
            m.put("TicketNo.", intOf(ci(x, "TicketNo")));
            m.put("OrderNo", intOf(ci(x, "OrderNo")));
            m.put("GpNo", intOf(ci(x, "GpSrNo")));
            m.put("SupplierCustomerId", intOf(ci(x, "SupplierCustomerId")));
            m.put("PartyName", str(ci(x, "PartyName")));
            m.put("VehicleNo", str(ci(x, "VehicleNo")));
            m.put("BiltyNo", intOf(ci(x, "BiltyNo")));          // typeof(int) + Conversion.ToInt, as written
            m.put("ItemDescription", str(ci(x, "ItemDescription")));
            m.put("ItemQty", dbl(ci(x, "ItemQty")));
            m.put("SupplierWeight", dbl(ci(x, "SupplierWeight")));
            m.put("WbCharges", dbl(ci(x, "WbCharges")));
            m.put("FirstWeight", dbl(ci(x, "FirstWeight")));
            m.put("SecondWeight", dbl(ci(x, "SecondWeight")));
            m.put("NetWbWeight", dbl(ci(x, "NetWbWeight")));
            m.put("FirstDateTime", ci(x, "FirstDateTime"));
            m.put("SecondDateTime", ci(x, "SecondDateTime"));
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

    /** DataGridHistory_ColumnButtonClick "Print" — the 280 slip for that row's Id and DocTypeId. */
    public List<Map<String, Object>> slip(int id, int documentTypeId) {
        UserAccount u = currentUserContext.requireAccountingUser();
        return repo.slip280(u, id, documentTypeId);
    }

    // ================================================================================ helpers

    private String[] company(UserAccount u) {
        try {
            Company c = companies.findById(u.getCompanyId()).orElse(null);
            if (c != null) return new String[] { str(c.getCompName()), str(c.getCompAddress()) };
        } catch (Exception ignored) {
        }
        return new String[] { "", "" };
    }

    private static Map<String, Object> row(String k1, Object v1, String k2, Object v2) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put(k1, v1);
        m.put(k2, v2);
        return m;
    }

    private static String t(String s) { return s == null ? "" : s.trim(); }

    private static Timestamp day(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        try { return Timestamp.valueOf(LocalDate.parse(s.trim().substring(0, 10)).atStartOfDay()); }
        catch (Exception e) { throw new IllegalArgumentException("Invalid date: " + s); }
    }

    private static String idList(List<Integer> ids) {
        if (ids == null || ids.isEmpty()) return "";
        StringBuilder b = new StringBuilder();
        for (Integer i : ids) if (i != null && i > 0) b.append(',').append(i);
        return b.toString();
    }
}
