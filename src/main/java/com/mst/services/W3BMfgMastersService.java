package com.mst.services;

import com.mst.models.UserAccount;
import com.mst.repositories.W3BMfgRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;

import static com.mst.repositories.support.DesktopProc.params;
import static com.mst.services.Prod273Support.ci;
import static com.mst.services.Prod273Support.str;
import static com.mst.services.Prod273Support.toDouble;
import static com.mst.services.Prod273Support.toInt;
import static com.mst.services.Prod273Support.truthy;

/**
 * W3B - Mfg Production hub (module 103). Three small master screens of Architecture.WinApp.Mfg:
 *
 * <pre>
 *   770 ProductionOperation     (ProductionOperation.cs, 594 lines)    Operation Name only
 *   771 ProductionWorkStation   (ProductionWorkStation.cs, 1026 lines) Department, Name, Code, costs, Active
 *   595 WorkStationOperation    (WorkStationOperation.cs, 1085 lines)  Work Station x Operation + Hours/Day
 * </pre>
 *
 * DESKTOP BEHAVIOUR REPRODUCED
 * <ol>
 *  <li>Insert() on each form: FormValidation messages verbatim, "Are you sure to Save?" / "Are you sure to Update?"
 *      confirm (done by the page), org / company / branch from the signed-in user, then
 *      "Record Save Successfully" / "Record Update Successfully" and Reset() (RecId 0, fields cleared, history reloaded).</li>
 *  <li>Update goes through the same Insert() with RecId (btnUpdate_Click: "RecId not found" when 0).</li>
 *  <li>770 history grid: Id (hidden), SerialNo (row no.), OperationName (FormHistory:~230).</li>
 *  <li>771 history grid: grouped by DepartmentName (GrdSetting), SerialNo restarts per department, columns
 *      WorkStationName, WorkStationCode, ElectricityCost, ProductionCapacity, RentCost, Wages, Active; double click = ReadById.</li>
 *  <li>595 history: two grids from the same FormHistory result, one grouped by WorkStationName (grdDetail), one by
 *      OperationName (grdII), SerialNo restarting per group; double click loads the row into the form. The
 *      procedure itself rejects a duplicate Work Station + Operation on insert and an update of a row used in a BOM.</li>
 * </ol>
 *
 * DEVIATIONS / NOTES
 * <ol>
 *  <li>The desktop calls Architecture.BLL.Mfg (assembly not in the recovered source). Each BLL call is mapped to the
 *      [Mfg] procedure of the same name; SortNo is not set by these forms so 0 is sent (the C# int default).</li>
 *  <li>EntryUserId / ModifyUserId on 771 are filled with the signed-in user (the desktop leaves them to the BLL).</li>
 *  <li>The Define Operations / Define Work Stations toolbar buttons that open another desktop form are links to the
 *      matching web page (770) or are omitted when the desktop button is hidden (Define Work Stations).</li>
 *  <li>Keyboard shortcuts of the desktop (Ctrl+S, Ctrl+U, Ctrl+N, Ctrl+E/Esc, Ctrl+F5, Ctrl+Up/Down) are wired on the page.</li>
 * </ol>
 */
@Service
public class W3BMfgMastersService {

    public static final String SCREEN_OPERATION = "ProductionOperation";
    public static final String SCREEN_WORKSTATION = "ProductionWorkStation";
    public static final String SCREEN_WSO = "WorkStationOperation";

    private final Prod273Support db;
    private final W3BMfgRepository repo;

    public W3BMfgMastersService(Prod273Support db, W3BMfgRepository repo) {
        this.db = db;
        this.repo = repo;
    }

    private static Map<String, Object> ok(String message, int id) {
        Map<String, Object> m = new LinkedHashMap<>();
        m.put("success", true);
        m.put("message", message);
        m.put("id", id);
        return m;
    }

    private static void requireTenant(UserAccount u, Map<String, Object> row) {
        if (row == null) throw new IllegalArgumentException("Record not found...");
        if (toInt(ci(row, "OrganizationId")) != toInt(u.getOrganizationId()) || toInt(ci(row, "CompanyId")) != toInt(u.getCompanyId())) {
            throw new IllegalArgumentException("Record not found...");
        }
    }

    // ================================================================================ 770 ProductionOperation

    /** FormHistory():237 - Id, SerialNo (i + 1), OperationName. */
    public List<Map<String, Object>> operationHistory() {
        UserAccount u = db.user();
        List<Map<String, Object>> out = new ArrayList<>();
        int n = 0;
        for (Map<String, Object> r : repo.operationHistory(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("SerialNo", ++n);
            o.put("OperationName", str(ci(r, "OperationName")));
            out.add(o);
        }
        return out;
    }

    /** grdDetail_DoubleClick:391 ProductionOperation.ReadById(RecId) -> OperationName. */
    public Map<String, Object> operationLoad(int id) {
        UserAccount u = db.user();
        Map<String, Object> r = repo.operationById(id);
        requireTenant(u, r);
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        o.put("OperationName", str(ci(r, "OperationName")));
        return o;
    }

    /** Insert():145 with FormValidation():134. */
    @Transactional
    public Map<String, Object> operationSave(Map<String, Object> body) {
        UserAccount u = db.user();
        int recId = toInt(body.get("id"));
        String name = str(body.get("operationName"));
        if (name.isEmpty()) throw new IllegalArgumentException("Operation Name Field is Required");
        if (recId > 0) requireTenant(u, repo.operationById(recId));
        Map<String, Object> p = params("ProductionOperationId", recId > 0 ? recId : null, "OperationName", name, "SortNo", 0,
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchId", u.getBranchesId());
        int id = repo.operationSave(p, recId > 0);
        return ok(recId > 0 ? "Record Update Successfully" : "Record Save Successfully", recId > 0 ? recId : id);
    }

    // ================================================================================ 771 ProductionWorkStation

    public Map<String, Object> workStationLookups() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("departments", departments(u));
        out.put("history", workStationHistory());
        return out;
    }

    /** DeprtmentFill():131 - Department.GetAll -> Id / DepartmentName. */
    public List<Map<String, Object>> departments(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.departments(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Name", str(ci(r, "DepartmentName")));
            out.add(o);
        }
        return out;
    }

    public List<Map<String, Object>> departments() { return departments(db.user()); }

    /** FormHistory():365 - grouped by DepartmentId (first-seen order), SerialNo restarts at 1 per department. */
    public List<Map<String, Object>> workStationHistory() {
        UserAccount u = db.user();
        Map<Integer, List<Map<String, Object>>> groups = new LinkedHashMap<>();
        for (Map<String, Object> r : repo.workStationHistory(u)) {
            groups.computeIfAbsent(toInt(ci(r, "DepartmentId")), k -> new ArrayList<>()).add(r);
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (List<Map<String, Object>> g : groups.values()) {
            int serial = 1;
            for (Map<String, Object> r : g) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", toInt(ci(r, "Id")));
                o.put("DepartmentId", toInt(ci(r, "DepartmentId")));
                o.put("DepartmentName", str(ci(r, "DepartmentName")));
                o.put("SerialNo", serial++);
                o.put("WorkStationName", str(ci(r, "WorkStationName")));
                o.put("WorkStationCode", str(ci(r, "workStationCode")));
                o.put("ElectricityCost", toDouble(ci(r, "electricityCost")));
                o.put("ProductionCapacity", toDouble(ci(r, "productionCapacity")));
                o.put("RentCost", toDouble(ci(r, "rentCost")));
                o.put("Wages", toDouble(ci(r, "wages")));
                o.put("Active", truthy(ci(r, "isActive")));
                out.add(o);
            }
        }
        return out;
    }

    /** ReadById():337. */
    public Map<String, Object> workStationLoad(int id) {
        UserAccount u = db.user();
        Map<String, Object> r = repo.workStationById(id);
        requireTenant(u, r);
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("Id", id);
        o.put("departmentId", toInt(ci(r, "DepartmentId")));
        o.put("workStationName", str(ci(r, "WorkStationName")));
        o.put("workStationCode", str(ci(r, "workStationCode")));
        o.put("electricityCost", toDouble(ci(r, "electricityCost")));
        o.put("productionCapacity", toDouble(ci(r, "productionCapacity")));
        o.put("wages", toDouble(ci(r, "wages")));
        o.put("rentCost", toDouble(ci(r, "rentCost")));
        o.put("isActive", truthy(ci(r, "isActive")));
        return o;
    }

    /** Insert():238 with FormValidation():215 (Department, Name, Code, in that order). */
    @Transactional
    public Map<String, Object> workStationSave(Map<String, Object> body) {
        UserAccount u = db.user();
        int recId = toInt(body.get("id"));
        int dept = toInt(body.get("departmentId"));
        String name = str(body.get("workStationName"));
        String code = str(body.get("workStationCode"));
        if (dept == 0) throw new IllegalArgumentException("Department Field Is Required");
        if (name.isEmpty()) throw new IllegalArgumentException("Work Station Name Field Is Required");
        if (code.isEmpty()) throw new IllegalArgumentException("Work Station Code Field Is Required");
        if (recId > 0) requireTenant(u, repo.workStationById(recId));
        Integer uid = u.getId();
        Map<String, Object> p = params("ProductionWorkStationId", recId > 0 ? recId : null, "DepartmentId", dept,
                "workStationCode", code, "WorkStationName", name,
                "productionCapacity", toDouble(body.get("productionCapacity")), "electricityCost", toDouble(body.get("electricityCost")),
                "rentCost", toDouble(body.get("rentCost")), "wages", toDouble(body.get("wages")),
                "isActive", truthy(body.get("isActive")), "SortNo", 0,
                "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(), "BranchId", u.getBranchesId());
        if (recId > 0) p.put("ModifyUserId", uid); else p.put("EntryUserId", uid);
        int id = repo.workStationSave(p, recId > 0);
        return ok(recId > 0 ? "Record Update Successfully" : "Record Save Successfully", recId > 0 ? recId : id);
    }

    // ================================================================================ 595 WorkStationOperation

    public Map<String, Object> wsoLookups() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("operations", operationCombo(u));
        out.put("workStations", workStationCombo(u));
        out.put("history", wsoHistory());
        return out;
    }

    /** OeraptionNamefill():120 - ProductionOperation.FormHistory -> Id / OperationName. */
    public List<Map<String, Object>> operationCombo(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.operationHistory(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Name", str(ci(r, "OperationName")));
            out.add(o);
        }
        return out;
    }

    /** StationNamefill():159 - ProductionWorkStation.FormHistory -> Id / WorkStationName. */
    public List<Map<String, Object>> workStationCombo(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.workStationHistory(u)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", toInt(ci(r, "Id")));
            o.put("Name", str(ci(r, "WorkStationName")));
            out.add(o);
        }
        return out;
    }

    /** btnRefresh_Click:229 - OeraptionNamefill + StationNamefill. */
    public Map<String, Object> wsoRefresh() {
        UserAccount u = db.user();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("operations", operationCombo(u));
        out.put("workStations", workStationCombo(u));
        return out;
    }

    /**
     * FormHistory():365 / CreateGroupedDataTable:398 - two lists: "byWorkStation" (grdDetail, grouped by WorkStationName)
     * and "byOperation" (grdII, grouped by OperationName); SerialNo restarts at 1 in each group.
     */
    public Map<String, Object> wsoHistory() {
        UserAccount u = db.user();
        List<Map<String, Object>> src = repo.wsoHistory(u);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("byWorkStation", group(src, "WorkStationName"));
        out.put("byOperation", group(src, "OperationName"));
        return out;
    }

    private static List<Map<String, Object>> group(List<Map<String, Object>> src, String col) {
        Map<String, List<Map<String, Object>>> groups = new LinkedHashMap<>();
        for (Map<String, Object> r : src) groups.computeIfAbsent(str(ci(r, col)), k -> new ArrayList<>()).add(r);
        List<Map<String, Object>> out = new ArrayList<>();
        for (List<Map<String, Object>> g : groups.values()) {
            int serial = 1;
            for (Map<String, Object> r : g) {
                Map<String, Object> o = new LinkedHashMap<>();
                o.put("Id", toInt(ci(r, "Id")));
                o.put("SerialNo", serial++);
                o.put("WorkStationName", str(ci(r, "WorkStationName")));
                o.put("OperationName", str(ci(r, "OperationName")));
                o.put("OpHoursPerDay", toDouble(ci(r, "OpHoursPerDay")));
                o.put("ProductionWorkStationId", toInt(ci(r, "ProductionWorkStationId")));
                o.put("ProductionOperationId", toInt(ci(r, "ProductionOperationId")));
                out.add(o);
            }
        }
        return out;
    }

    /** Insert():271 with FormValidation():254 (Operation first, then Work Station). */
    @Transactional
    public Map<String, Object> wsoSave(Map<String, Object> body) {
        UserAccount u = db.user();
        int recId = toInt(body.get("id"));
        int op = toInt(body.get("operationId"));
        int ws = toInt(body.get("workStationId"));
        if (op == 0) throw new IllegalArgumentException("Operation Name Field Is Required");
        if (ws == 0) throw new IllegalArgumentException("Work Station Name Field Is Required");
        if (recId > 0) {                                    // tenancy: only a row of the signed-in branch can be updated
            boolean mine = false;
            for (Map<String, Object> r : repo.wsoHistory(u)) if (toInt(ci(r, "Id")) == recId) { mine = true; break; }
            if (!mine) throw new IllegalArgumentException("Record not found...");
        }
        Map<String, Object> p = params("WorkStationOperationId", recId > 0 ? recId : null, "ProductionWorkStationId", ws,
                "ProductionOperationId", op, "SortNo", 0, "OrganizationId", u.getOrganizationId(), "CompanyId", u.getCompanyId(),
                "BranchId", u.getBranchesId(), "OpHoursPerDay", toDouble(body.get("opHoursPerDay")));
        int id = repo.wsoSave(p, recId > 0);
        return ok(recId > 0 ? "Record Update Successfully" : "Record Save Successfully", recId > 0 ? recId : id);
    }
}
