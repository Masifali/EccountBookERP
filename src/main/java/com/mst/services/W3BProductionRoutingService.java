package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.W3BMfgRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.sql.Timestamp;
import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;
import static com.mst.services.Prod273Support.ci;
import static com.mst.services.Prod273Support.list;
import static com.mst.services.Prod273Support.now;
import static com.mst.services.Prod273Support.str;
import static com.mst.services.Prod273Support.toDouble;
import static com.mst.services.Prod273Support.toInt;
import static com.mst.services.Prod273Support.truthy;

/**
 * W3B - Mfg Production hub (module 103) - screen 773 "Production Routing".
 *
 * <pre>
 *   Desktop form   Architecture.WinApp.Mfg.frmProductionRouting (.cs, 2649 lines), base.Name "frmProductionRouting" (rights key)
 *   Route          /production/mfg/routing      API /production/mfg/routing/api
 *   BLL            productionRouting / ProductionRoutingName / WorkStationOperation (Architecture.BLL.Mfg)
 *   Procedures     see {@link W3BMfgRepository}
 * </pre>
 *
 * DESKTOP BEHAVIOUR REPRODUCED
 * <ol>
 *  <li>Load:169 - rights (Save, Update, Print, Delete), RoutingNameBind:263 (ProductionRoutingName.FormHistory, no blank row),
 *      AllWorkStationOperationFill:311 ("Work Station/Operation" combo, blank row, Description = "WorkStation,Operation"),
 *      HistoryRoutingNameBind:214 (routing names already used by a routing).</li>
 *  <li>FormValidationDetail:360 - "Operation Name Field is Required" / "CapacityQty Field is Required"; BtnAddOp_Click:377 -
 *      "Operation Already Exist in Grid!"; BtnUpdateOp_Click:418 - "WorkStation Operation Already Exist in Grid!" (page side).</li>
 *  <li>Insert():778 - "Routing Name is Required...", "Grid Record Not Found...\nGrid Should have atleast one record", then
 *      GetMissingSequence:754 ("Missing sequence numbers:1,2,"), removed rows first with ActionId 3 (DeleteDetailRow:593), then each
 *      grid row with a work-station operation: "CapacityQty Field Required in Grid Row No: n", "Hours Or Minutes Required in Grid
 *      Row No:n"; ProductionRoutingId = row Id only in update mode, ActionId 1 (new row) / 2; OperationTime = TimeSpan(h, m, 0)
 *      "hh:mm:ss"; every row saved with USP_ProductionRouting_InsertAndUpdate; "Data Save Successfully....  name" / "Data Update Successfully....  name".</li>
 *  <li>ReadById:925 (all rows of a routing name; Hours / Minutes from operationTime) from CmbRountingName_Leave:988 (only with the
 *      Update right) and from the history grid (double click / Edit: "you don't have update rights...").</li>
 *  <li>HistoryGridFill:1068 - FormHistory with the routing-name filter; columns RoutingName (group), Seq No, OperationName,
 *      WorkStation, CapacityQty, Hours, Minutes, Active.</li>
 *  <li>btnDelete_Click:959 - DeleteByName (ActionId 3 on every row of the routing name), "Delete Record Successfully".</li>
 * </ol>
 *
 * DEVIATIONS / NOTES
 * <ol>
 *  <li>btnDelete_Click tests {@code !IsNullOrEmpty(CmbRountingName.Text) || Value == 0} and then says "Please Select RoutingName..." - as
 *      written it refuses whenever a name IS selected, so the desktop button can hardly ever delete. The page implements the evident
 *      intent: it deletes when a routing name is selected, and says "Please Select RoutingName..." when none is.</li>
 *  <li>WorkStationOperation.GetWorkStationOpertaionByWSOId(org, comp, 0): the BLL assembly is not in the recovered source; @Id is sent
 *      only when non-zero (the procedure's "@Id is null or ..." branch), otherwise no row could ever be listed.</li>
 *  <li>History EntryUserId is always sent (the desktop sets EntryUser only under CanViewAllRecord; the procedure then shows every
 *      record for CanViewAllRecord = 1 and the user's own otherwise).</li>
 *  <li>BtnLookup (opens the ProductionRoutingName define form) is a link to a screen that is not in this batch - Known gap.</li>
 *  <li>Print button of the desktop (btnPrint_Click) has no body in the source; it is not ported.</li>
 * </ol>
 */
@Service
public class W3BProductionRoutingService {

    public static final String SCREEN = "frmProductionRouting";

    private final Prod273Support db;
    private final W3BMfgRepository repo;

    public W3BProductionRoutingService(Prod273Support db, W3BMfgRepository repo) {
        this.db = db;
        this.repo = repo;
    }

    private static Map<String, Object> pair(Object id, Object name) {
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        o.put("Name", name);
        return o;
    }

    // ====================================================================================== load

    /** frmBillOfMaterial_Manufacturing_Load:169 (rights + combos). */
    public Map<String, Object> lookups() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", db.rights(SCREEN));
        out.putAll(combos(u));
        out.put("historyNames", historyNames(u));
        return out;
    }

    public Map<String, Object> refresh() {
        UserAccount u = db.user();
        Map<String, Object> out = combos(u);
        out.put("historyNames", historyNames(u));
        return out;
    }

    private Map<String, Object> combos(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("routingNames", routingNames(u));
        out.put("workStationOperations", wsOps(u));
        return out;
    }

    /** RoutingNameBind:263 - ProductionRoutingNameId / ProductionRoutingName. */
    private List<Map<String, Object>> routingNames(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.routingNames(u)) out.add(pair(toInt(ci(r, "ProductionRoutingNameId")), str(ci(r, "ProductionRoutingName"))));
        return out;
    }

    /** HistoryRoutingNameBind:214 - dropdown 'RoutingName' (Id, ReferenceName). */
    private List<Map<String, Object>> historyNames(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.routingNamesInUse(u)) out.add(pair(toInt(ci(r, "Id")), str(ci(r, "ReferenceName"))));
        return out;
    }

    /** AllWorkStationOperationFill:311 - Id, Description (combo), WorkStationName, OperationName. */
    private List<Map<String, Object>> wsOps(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.wsoByWsoId(u, 0)) {
            Map<String, Object> o = pair(toInt(ci(r, "Id")), str(ci(r, "Description")));
            o.put("WorkStationName", str(ci(r, "WorkStationName")));
            o.put("OperationName", str(ci(r, "OperationName")));
            out.add(o);
        }
        return out;
    }

    // ====================================================================================== read / history

    private static String[] hm(Object operationTime) {
        String s = str(operationTime).trim();
        String[] p = s.split(":");
        int h = p.length > 0 ? toInt(p[0]) : 0;
        int m = p.length > 1 ? toInt(p[1]) : 0;
        return new String[] { String.valueOf(h), String.valueOf(m) };
    }

    private boolean mine(UserAccount u, Map<String, Object> r) {
        return toInt(ci(r, "OrganizationId")) == toInt(u.getOrganizationId()) && toInt(ci(r, "CompanyId")) == toInt(u.getCompanyId());
    }

    /** ReadById(DataTable):925 - every row of one routing name; "Record not found..." when none. */
    public Map<String, Object> read(int routingNameId) {
        UserAccount u = db.user();
        if (!db.has(SCREEN, "update")) throw new IllegalStateException("you don't have update rights...");
        List<Map<String, Object>> rows = new ArrayList<>();
        int recId = 0;
        for (Map<String, Object> r : repo.routingByNameId(routingNameId)) {
            if (!mine(u, r)) continue;
            if (rows.isEmpty()) recId = toInt(ci(r, "productionRoutingId"));
            String[] t = hm(ci(r, "operationTime"));
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "productionRoutingId")));
            o.put("OperationId", toInt(ci(r, "WorkStationOperationId")));
            o.put("OperationName", str(ci(r, "OperationName")));
            o.put("WorkStation", str(ci(r, "WorkStationName")));
            o.put("CapacityQty", toDouble(ci(r, "CapacityQty")));
            o.put("Hours", toInt(t[0]));
            o.put("Minutes", toInt(t[1]));
            o.put("SeqNo", toInt(ci(r, "sequenceNo")));
            o.put("Active", truthy(ci(r, "isActive")));
            rows.add(o);
        }
        if (rows.isEmpty()) throw new IllegalArgumentException("Record not found...");
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("recId", recId);
        out.put("routingNameId", routingNameId);
        out.put("rows", rows);
        return out;
    }

    /** HistoryGridFill:1068. */
    public List<Map<String, Object>> history(int routingNameId) {
        UserAccount u = db.user();
        boolean viewAll = db.has(SCREEN, "viewAll");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.routingHistory(u, routingNameId, viewAll, toInt(u.getId()))) {
            String[] t = hm(ci(r, "operationTime"));
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "ProductionRoutingId")));
            o.put("RoutingNameId", toInt(ci(r, "RoutingNameId")));
            o.put("RoutingName", str(ci(r, "RoutingName")));
            o.put("SequenceNo", toInt(ci(r, "sequenceNo")));
            o.put("OperationId", toInt(ci(r, "WorkStationOperationId")));
            o.put("OperationName", str(ci(r, "OperationName")));
            o.put("WorkStation", str(ci(r, "WorkStationName")));
            o.put("CapacityQty", toDouble(ci(r, "CapacityQty")));
            o.put("Hours", toInt(t[0]));
            o.put("Minutes", toInt(t[1]));
            o.put("Active", truthy(ci(r, "isActive")));
            out.add(o);
        }
        return out;
    }

    // ====================================================================================== save / delete

    /** TimeSpan(h, m, 0).ToString("hh\\:mm\\:ss"). */
    static String timeSpan(int h, int m) {
        long total = (long) h * 60 + m;
        return String.format("%02d:%02d:00", (total / 60) % 24, total % 60);
    }

    private ParamsHolder common(UserAccount u, Timestamp stamp) {
        return new ParamsHolder(u, stamp);
    }

    private static final class ParamsHolder {
        final UserAccount u;
        final Timestamp stamp;
        ParamsHolder(UserAccount u, Timestamp stamp) { this.u = u; this.stamp = stamp; }
        Map<String, Object> base(int routingNameId, String routingName) {
            Map<String, Object> p = params("RoutingNameId", routingNameId, "routingName", routingName,
                    "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchId", u.getBranchesId(),
                    "EntryDate", stamp, "EntryUserId", u.getId(), "ModifyDate", stamp, "ModifyUserId", u.getId());
            return p;
        }
    }

    /**
     * Insert():778. Body: id (RecId, 0 = new), routingNameId, rows [{Id, OperationId, CapacityQty, Hours, Minutes, SeqNo, Active}],
     * removed [{Id, OperationId, CapacityQty, Hours, Minutes, SeqNo, Active}].
     */
    @Transactional
    public Map<String, Object> save(Map<String, Object> body) {
        UserAccount u = db.user();
        int recId = toInt(body.get("id"));
        boolean update = recId > 0;
        if (update ? !db.has(SCREEN, "update") : !db.has(SCREEN, "save")) {
            throw new IllegalStateException(update ? "You do not have the Update right on Production Routing." : "You do not have the Save right on Production Routing.");
        }
        int routingNameId = toInt(body.get("routingNameId"));
        String routingName = "";
        for (Map<String, Object> n : routingNames(u)) if (toInt(n.get("Id")) == routingNameId) routingName = str(n.get("Name")).trim();
        if (routingNameId == 0 || routingName.isEmpty()) throw new IllegalArgumentException("Routing Name is Required...");
        List<Map<String, Object>> rows = list(body.get("rows"));
        if (rows.isEmpty()) throw new IllegalArgumentException("Grid Record Not Found...\nGrid Should have atleast one record");

        // Valid operation ids (the combo's own rows) - a row may only point at a work-station operation of this branch.
        List<Integer> validOps = new ArrayList<>();
        for (Map<String, Object> o : wsOps(u)) validOps.add(toInt(o.get("Id")));

        // GetMissingSequence:754
        StringBuilder missing = new StringBuilder();
        List<Integer> seqs = new ArrayList<>();
        for (Map<String, Object> r : rows) seqs.add(toInt(r.get("SeqNo")));
        for (int i = 1; i <= rows.size(); i++) if (!seqs.contains(i)) missing.append(i).append(',');
        if (missing.length() > 0) throw new IllegalArgumentException("Missing sequence numbers:" + missing);

        Timestamp stamp = now();
        ParamsHolder ph = common(u, stamp);
        List<Map<String, Object>> calls = new ArrayList<>();

        for (Map<String, Object> d : list(body.get("removed"))) {                         // DeleteDetailRow:593 (ActionId 3)
            int id = toInt(d.get("Id"));
            if (id == 0) continue;
            Map<String, Object> p = ph.base(routingNameId, routingName);
            p.put("productionRoutingId", id);
            p.put("sequenceNo", toInt(d.get("SeqNo")));
            p.put("WorkStationOperationId", toInt(d.get("OperationId")));
            p.put("CapacityQty", toDouble(d.get("CapacityQty")));
            p.put("operationTime", timeSpan(toInt(d.get("Hours")), toInt(d.get("Minutes"))));
            p.put("operatingCost", 0);
            p.put("isActive", truthy(d.get("Active")));
            p.put("ActionId", 3);
            calls.add(p);
        }
        int rowNo = 0;
        for (Map<String, Object> r : rows) {
            rowNo++;
            int opId = toInt(r.get("OperationId"));
            if (opId == 0) continue;                                                      // Insert() row loop
            if (toDouble(r.get("CapacityQty")) == 0d) throw new IllegalArgumentException("CapacityQty Field Required in Grid Row No: " + rowNo);
            int h = toInt(r.get("Hours")), m = toInt(r.get("Minutes"));
            if (h == 0 && m == 0) throw new IllegalArgumentException("Hours Or Minutes Required in Grid Row No:" + rowNo);
            if (!validOps.contains(opId)) throw new IllegalArgumentException("WorkStationOperation Field Required in Grid Row No: " + rowNo);
            int id = recId != 0 ? toInt(r.get("Id")) : 0;                                 // Insert() row loop
            Map<String, Object> p = ph.base(routingNameId, routingName);
            p.put("productionRoutingId", id);
            p.put("sequenceNo", toInt(r.get("SeqNo")));
            p.put("WorkStationOperationId", opId);
            p.put("CapacityQty", toDouble(r.get("CapacityQty")));
            p.put("operationTime", timeSpan(h, m));
            p.put("operatingCost", 0);
            p.put("isActive", truthy(r.get("Active")));
            p.put("ActionId", id <= 0 ? 1 : 2);                                           // Insert() row loop
            calls.add(p);
        }
        int last = 0;
        for (Map<String, Object> p : calls) last = repo.routingRowSave(p);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", (update ? "Data Update Successfully....  " : "Data Save Successfully....  ") + routingName);   // Insert() end
        out.put("id", last);
        return out;
    }

    /** btnDelete_Click:959 - DeleteByName(org, comp, user, routingNameId). */
    @Transactional
    public Map<String, Object> delete(int routingNameId) {
        UserAccount u = db.user();
        if (!db.has(SCREEN, "delete")) throw new IllegalStateException("You do not have the Delete right on Production Routing.");
        if (routingNameId == 0) throw new IllegalArgumentException("Please Select RoutingName...");
        repo.routingDeleteByName(u, toInt(u.getId()), routingNameId);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("success", true);
        out.put("message", "Delete Record Successfully");
        return out;
    }
}
