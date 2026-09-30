package com.mst.services.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.dto.RosterDetailRowDto;
import com.mst.models.hrm.dto.RosterGroupRowDto;
import com.mst.models.hrm.dto.RosterHistoryDto;
import com.mst.models.hrm.dto.RosterLoadDto;
import com.mst.models.hrm.dto.RosterSaveDto;
import com.mst.models.hrm.roster.DutyRoasterGroups;
import com.mst.models.hrm.roster.GenDutyRoaster;
import com.mst.models.hrm.roster.GenDutyRoasterDetail;
import com.mst.repositories.hrm.HrmRosterRepository;
import com.mst.repositories.hrm.HrmRosterRepository.Table;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * BLL / form logic of the two duty-roster screens (AppModules 2022):
 *
 *   656 frmDutyRoasterNew   /hrm/duty-roaster     Screen "DutyRoaster"
 *   657 frmEmployeeRoaster  /hrm/employee-roaster Screen "EmployeeRoaster"
 *
 * The page keeps the form's in-memory lstDutyRoasterDetail (grid clicks, Apply, Apply all change
 * IsOnDuty there, exactly as the form does). On Save the page sends the rows still on duty (after the
 * form's RemoveAll(!IsOnDuty)); this service then does what Insert() does from that point on -
 * MakeOffDaysRecords, DutyRoasterDetailIdsUpdateForUpdate, the grdGroup rows, genDutyRoaster.Save
 * (header + every detail + every group in one transaction) and the DAL's two attendance procedures.
 * Every row the page sends must be one SP_GetDutyDatesForDutyRoaster returns for the filter the grids
 * were loaded with (tenancy guard): nothing the page invents reaches the database.
 */
@Service
public class HrmRosterService {

    public static final int SCREEN_DUTY_ROASTER = 656;
    public static final int SCREEN_EMPLOYEE_ROASTER = 657;
    public static final int SCREEN_EMPLOYEE_REGISTRATION = 648;
    public static final int SCREEN_SHIFT_TIMING = 654;

    private static final String NOT_FOUND = "Record not found.";
    private static final String CHANGED = "The duty roaster rows no longer match the database. Press Load again.";

    @Autowired private HrmRosterRepository repo;
    @Autowired private HrmSupport hrm;

    // ================================================================== 656 frmDutyRoasterNew

    /**
     * frmDutyRoaster_Load: rights (btnsave / btnupdate / btnPrint), EmployeeCategoryFill, DepartmentFill,
     * SectionFill, ShiftFill, WeekDaysFill, EmployeeName (no filter chosen yet), LocationFilterFill.
     * Also whether the Employee Registration / Shift Timing forms may be opened (their View right).
     */
    public Map<String, Object> dutySetup() {
        UserAccount u = hrm.user(SCREEN_DUTY_ROASTER);
        Map<String, Object> out = dutyListsOf(u);
        out.put("rights", hrm.rights(u, SCREEN_DUTY_ROASTER));
        out.put("weekProfiles", weekProfiles(u));
        out.put("employees", employees(repo.activeEmployees(u, 0, 0, null)));
        out.put("canEmployeeRegistration", hrm.can(u, SCREEN_EMPLOYEE_REGISTRATION, "View"));
        out.put("canShiftTiming", hrm.can(u, SCREEN_SHIFT_TIMING, "View"));
        return out;
    }

    /** btnRefresh_Click: EmployeeCategoryFill, DepartmentFill, SectionFill, ShiftFill, LocationFilterFill (EmployeeName is a separate call). */
    public Map<String, Object> dutyLists() {
        return dutyListsOf(hrm.user(SCREEN_DUTY_ROASTER));
    }

    private Map<String, Object> dutyListsOf(UserAccount u) {
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("categories", pick(repo.employeeCategories(u), "EmployeeCategoryId", "EmployeeCategoryName"));
        out.put("departments", pick(repo.departments(u), "DepartmentId", "DepartmentName"));
        out.put("sections", pick(repo.sections(u), "SectionId", "SectionName"));
        out.put("shifts", pick(repo.shifts(u), "ShiftId", "ShiftName"));
        out.put("locations", pick(repo.locations(u), "LocationId", "LocationName"));
        return out;
    }

    /**
     * EmployeeName(): genEmployeeHistory.GetAllEmployeesActive with the Section, Location (PartyLocationId)
     * and the department ids string built from cmbDepartment.Text.
     */
    public List<Map<String, Object>> dutyEmployees(int sectionId, int locationId, String departmentIds) {
        UserAccount u = hrm.user(SCREEN_DUTY_ROASTER);
        return employees(repo.activeEmployees(u, sectionId, locationId, departmentIds(departmentIds)));
    }

    /** History tab: HistoryGridFill -> genDutyRoaster.DutyRoaster_FormHistory, dtHistory built as the form builds it. */
    public List<Map<String, Object>> dutyHistory(RosterHistoryDto b) {
        UserAccount u = hrm.user(SCREEN_DUTY_ROASTER);
        if (b == null) b = new RosterHistoryDto();
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("CanViewAllRecord", true);                          // obj.CanViewAllRecord = true -> @EntryUser never sent
        String mode = str(b.mode);
        LocalDateTime f = b.fromChecked ? toDay(b.fromDate) : null;
        LocalDateTime t = b.toChecked ? toDay(b.toDate) : null;
        if ("entry".equals(mode)) { p.put("EntryFromDate", f); p.put("EntryToDate", t); }
        else if ("modify".equals(mode)) { p.put("ModifyFromDate", f); p.put("ModifyToDate", t); }
        else { p.put("FromDate", f); p.put("ToDate", t); }
        int rf = toInt(b.rosterFrom), rt = toInt(b.rosterTo);
        if (rf != 0) p.put("DutyRoasterFrom", rf);
        if (rt != 0) p.put("DutyRoasterTo", rt);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.formHistory(p)) {
            out.add(map("DutyRoaster", r.get("DutyRoasterId"), "ShiftTimingName", r.get("ShiftTimingDescription"),
                    "FromDate", dateStr(r.get("DutyRoasterFromDate")), "ToDate", dateStr(r.get("DutyRoasterToDate")),
                    "Remarks", r.get("Remarks"), "EntryDate", dateTimeStr(r.get("EntryDate")), "EntryUserName", r.get("EntryUserName"),
                    "ModifyDate", dateTimeStr(r.get("ModifyDate")), "ModifyUserName", r.get("ModifyUserName")));
        }
        return out;
    }

    /** datagridHistory_DoubleClick -> genDutyRoaster.GetByID (header + DutyRoasterGroupsList). */
    public Map<String, Object> dutyById(long id) {
        UserAccount u = hrm.user(SCREEN_DUTY_ROASTER);
        return byId(u, id);
    }

    // ================================================================== 657 frmEmployeeRoaster

    /** frmDutyRoaster_Load (frmEmployeeRoaster): rights, ShiftFill, WeekDaysFill, EmployeeName (all active employees). */
    public Map<String, Object> employeeSetup() {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_ROASTER);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", hrm.rights(u, SCREEN_EMPLOYEE_ROASTER));
        out.put("shifts", pick(repo.shifts(u), "ShiftId", "ShiftName"));
        out.put("weekProfiles", weekProfiles(u));
        out.put("employees", employees(repo.allActiveEmployees(u)));
        out.put("canEmployeeRegistration", hrm.can(u, SCREEN_EMPLOYEE_REGISTRATION, "View"));
        return out;
    }

    /** btnRefresh_Click (frmEmployeeRoaster): ShiftFill, EmployeeName. */
    public Map<String, Object> employeeLists() {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_ROASTER);
        return map("shifts", pick(repo.shifts(u), "ShiftId", "ShiftName"), "employees", employees(repo.allActiveEmployees(u)));
    }

    /** History tab (tabControl1_SelectedIndexChanged -> HistoryGridFill): GetHistoryHeaderData, dtHistory as the form builds it. */
    public List<Map<String, Object>> employeeHistory() {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_ROASTER);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.historyHeader(u)) {
            out.add(map("Id", r.get("Id"), "ShiftTimingName", r.get("TimingDescription"),
                    "FromDate", dateStr(r.get("DutyRoasterFromDate")), "ToDate", dateStr(r.get("DutyRoasterToDate")), "Remarks", r.get("Remarks")));
        }
        return out;
    }

    /** datagridHistory_DoubleClick (frmEmployeeRoaster): GetByID + GetEmployeesByDutyRosterId for CmbEmployeeName. */
    public Map<String, Object> employeeById(long id) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_ROASTER);
        Map<String, Object> out = byId(u, id);
        out.put("employees", pick(repo.employeesByRoster((int) id), "EmployeeId", "EmployeeName"));
        return out;
    }

    // ================================================================== shared

    /** datagridHistory_Click / SelectionChanged -> genDutyRoaster.GetHistoryDetailData, dtHistoryDetail as the form builds it. */
    public List<Map<String, Object>> historyDetail(int screen, int id) {
        UserAccount u = hrm.user(screen(screen));
        if (!owns(repo.rosters(u), "DutyRoasterId", id)) throw invalid(NOT_FOUND);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.historyDetail(id)) {
            Object no = r.get("EmployeeNo");
            String ns = str(no).trim();
            out.add(map("ShiftName", r.get("ShiftName"), "ShiftTiming", r.get("ShiftTiming"), "EmployeeType", r.get("EmployeeType"),
                    "Designation", r.get("DesignationName"), "Department", r.get("DepartmentName"),
                    "EmployeeNo", ns.matches("-?\\d+") ? (Object) toInt(ns) : no, "EmployeeName", r.get("EmployeeName"),
                    "CNIC", r.get("CNIC"), "Mobile", r.get("Mobile1"), "Location", r.get("Locations"), "WeekDay", r.get("WeekDayName"),
                    "DutyDate", dateStr(r.get("DutyDate")), "IsOffDuty", boolText(r.get("IsOffDuty")), "IsOnDuty", boolText(r.get("IsOnDuty"))));
        }
        return out;
    }

    /**
     * MannualDataSetInDutyRoaster: genDutyRoaster.GetDutyDatesForDutyRoaster(Filter) and the four tables
     * the form binds - grdDutyDate (Tables[0]), lstDutyRoasterDetail (Tables[1], converted as the form
     * converts each column), grdEmployee (Tables[2], columns in the procedure's order) and grdGroup (Tables[3]).
     */
    public Map<String, Object> load(int screen, RosterLoadDto b) {
        UserAccount u = hrm.user(screen(screen));
        List<Table> ds = repo.dutyDates(loadParams(u, screen, b));
        if (ds.isEmpty()) return map("tables", 0);
        Table dates = ds.get(0), all = ds.size() > 1 ? ds.get(1) : new Table(), emp = ds.size() > 2 ? ds.get(2) : new Table(),
                grp = ds.size() > 3 ? ds.get(3) : new Table();
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("tables", ds.size());
        out.put("dates", maps(dates));
        List<Object[]> lst = new ArrayList<>();
        for (Object[] r : all.rows) lst.add(detailRow(all, r));
        out.put("allCols", DETAIL_COLS);
        out.put("all", lst);
        out.put("empCols", emp.cols);
        out.put("emp", maps(emp));
        out.put("groups", maps(grp));
        return out;
    }

    /** Columns of each lstDutyRoasterDetail row the page receives (arrays, in this order). */
    private static final List<String> DETAIL_COLS = Collections.unmodifiableList(java.util.Arrays.asList(
            "EmployeeId", "EmployeeHistoryId", "ShiftId", "ShiftShortName", "DutyDate", "WeekDayName", "EmployeeGroupId",
            "IsOnDuty", "DutyRoasterDetailId", "DutyRoasterId", "IsHoliday"));

    /** The loop of MannualDataSetInDutyRoaster: Convert.ToInt64 / ToInt32 / ToDateTime / ToBoolean throw on DBNull, Conversion.* read 0 / false. */
    private static Object[] detailRow(Table t, Object[] r) {
        return new Object[] {
                strictLong(t.get(r, "EmployeeId")), strictLong(t.get(r, "EmployeeHistoryId")), (int) strictLong(t.get(r, "ShiftId")),
                str(t.get(r, "ShiftShortName")), strictDate(t.get(r, "DutyDate")), str(t.get(r, "WeekDayName")),
                (int) strictLong(t.get(r, "EmployeeGroupId")), strictBool(t.get(r, "IsonDuty")),
                toLong(t.get(r, "DutyRoasterDetailId")), toLong(t.get(r, "DutyRoasterId")), toBool(t.get(r, "IsHoliday"))
        };
    }

    /**
     * Insert() of both forms from the point the rows leave the form: the header filled as the form fills
     * it, MakeOffDaysRecords, DutyRoasterDetailIdsUpdateForUpdate (when RecId &gt; 0), the grdGroup rows that
     * have a group, a shift and a rest day, genDutyRoaster.Save and the messages "Record Saved" / "Record Update".
     */
    public Map<String, Object> save(int screen, RosterSaveDto b) {
        int scr = screen(screen);
        UserAccount u = hrm.user(scr);
        if (b == null) throw invalid(NOT_FOUND);
        long recId = Math.max(0L, b.recId);
        if (scr == SCREEN_EMPLOYEE_ROASTER && recId <= 0) throw invalid("RecId Not Found");
        hrm.require(u, scr, recId > 0 ? "Update" : "Save");
        if (recId > 0 && !owns(repo.rosters(u), "DutyRoasterId", (int) recId)) throw invalid(NOT_FOUND);
        LocalDateTime from = toDay(b.fromDate), to = toDay(b.toDate);
        if (from == null || to == null || to.isBefore(from)) throw invalid("DutyRoasterDetail not found Please Check");

        LocalDateTime now = LocalDateTime.now();
        long userId = u.getId();

        // header - obj filled as Insert() fills it (frmEmployeeRoaster leaves the four filters at 0)
        GenDutyRoaster obj = new GenDutyRoaster();
        obj.DutyRoasterId = recId;
        obj.ActionTypeId = recId > 0 ? 2 : 1;
        obj.FromDate = from;
        obj.ToDate = to;
        obj.CreatedOn = now;
        obj.AlteredOn = now;
        obj.CreatedById = userId;
        obj.AlteredById = userId;
        obj.Description = trim(b.description);
        obj.OrganizationId = u.getOrganizationId();
        obj.CompanyId = u.getCompanyId();
        if (scr == SCREEN_DUTY_ROASTER) {
            if (b.employeeCategoryId != 0 && !owns(repo.employeeCategories(u), "EmployeeCategoryId", b.employeeCategoryId)) throw invalid(NOT_FOUND);
            if (b.sectionId != 0 && !owns(repo.sections(u), "SectionId", b.sectionId)) throw invalid(NOT_FOUND);
            if (b.locationId != 0 && !owns(repo.locations(u), "LocationId", b.locationId)) throw invalid(NOT_FOUND);
            obj.EmployeeCategoryId = b.employeeCategoryId;
            obj.DepartmentId = 0;                     // Conversion.ToInt(cmbDepartment.Value): a checked-list value -> 0
            obj.SectionId = b.sectionId;
            obj.LocationId = b.locationId;
        }

        // the rows the page kept on duty, verified against the grids' own data source
        List<RosterDetailRowDto> rows = b.details == null ? Collections.emptyList() : b.details;
        List<RosterGroupRowDto> grpRows = b.groups == null ? Collections.emptyList() : b.groups;
        Set<String> known = new HashSet<>();
        Set<Integer> knownGroups = new HashSet<>();
        if (!rows.isEmpty() || !grpRows.isEmpty()) {
            if (b.load == null) throw invalid(CHANGED);
            List<Table> ds = repo.dutyDates(loadParams(u, scr, b.load));
            if (ds.size() > 1) {
                Table all = ds.get(1);
                for (Object[] r : all.rows) {
                    Object[] d = detailRow(all, r);
                    known.add(key((Long) d[0], (Long) d[1], (Integer) d[2], (String) d[4], (String) d[5], (Integer) d[6], (Long) d[8], (Long) d[9]));
                }
            }
            if (ds.size() > 3) {
                Table g = ds.get(3);
                for (Object[] r : g.rows) knownGroups.add(toInt(g.get(r, "EmployeeGroupId")));
            }
        }
        for (RosterDetailRowDto r : rows) {
            String dd = dateStr(r.dutyDate);
            if (!known.contains(key(r.employeeId, r.employeeHistoryId, r.shiftId, dd, str(r.weekDayName), r.employeeGroupId, r.dutyRoasterDetailId, r.dutyRoasterId)))
                throw invalid(CHANGED);
            GenDutyRoasterDetail d = new GenDutyRoasterDetail();
            d.EmployeeId = r.employeeId;
            d.EmployeeHistoryId = r.employeeHistoryId;
            d.ShiftId = r.shiftId;
            d.DutyDate = toDay(dd);
            d.WeekDayName = str(r.weekDayName);
            d.EmployeeGroupId = r.employeeGroupId;
            d.IsOnDuty = true;
            d.DutyRoasterDetailId = r.dutyRoasterDetailId;
            d.DutyRoasterId = r.dutyRoasterId;
            d.DutyRoasterDetailLineId = r.line;
            d.ActionTypeId = d.DutyRoasterDetailId > 0 ? 2 : 1;
            d.CreatedOn = now;
            d.AlteredOn = now;
            d.CreatedById = userId;
            d.AlteredById = userId;
            obj.GenDutyRoasterDetailsList.add(d);
        }
        makeOffDaysRecords(obj, recId, now, userId);
        if (recId > 0) dutyRoasterDetailIdsUpdateForUpdate(obj, recId);

        // grdGroup rows with EmployeeGroupId, ShiftId and WeekDayName > 0
        List<Map<String, Object>> shifts = null, week = null;
        for (RosterGroupRowDto g : grpRows) {
            if (g == null || g.employeeGroupId <= 0 || g.shiftId <= 0 || g.weekDayId <= 0) continue;
            if (shifts == null) { shifts = repo.shifts(u); week = repo.weekProfiles(u); }
            if (!knownGroups.contains(g.employeeGroupId) || !owns(shifts, "ShiftId", g.shiftId) || !owns(week, "ProfileId", g.weekDayId)) throw invalid(NOT_FOUND);
            DutyRoasterGroups m = new DutyRoasterGroups();
            m.EmployeeGroupId = g.employeeGroupId;
            m.ShiftId = g.shiftId;
            m.WeekDayId = g.weekDayId;
            obj.DutyRoasterGroupsList.add(m);
        }

        long id = setData(obj);
        repo.updateAttendance(u);
        return saved((int) id, recId > 0 ? "Record Update" : "Record Saved");
    }

    /** BLL genDutyRoaster.Save + DAL SetData: Insert when DutyRoasterId == 0 else Update, then every detail and group in the same transaction. */
    private long setData(GenDutyRoaster obj) {
        boolean insert = obj.DutyRoasterId == 0L;
        obj.ActionTypeId = insert ? 1 : 2;
        return repo.tx(() -> {
            long num = repo.saveHeader(obj, insert);
            if (num > 0) obj.DutyRoasterId = num;
            else num = obj.DutyRoasterId;
            for (GenDutyRoasterDetail d : obj.GenDutyRoasterDetailsList) {
                d.DutyRoasterId = obj.DutyRoasterId;
                repo.saveDetail(d);
            }
            for (DutyRoasterGroups g : obj.DutyRoasterGroupsList) {
                g.DutyRoasterId = obj.DutyRoasterId;
                repo.saveGroup(g);
            }
            return num;
        });
    }

    /**
     * MakeOffDaysRecords: for every employee of the list (in the order they first appear) and every date
     * of GetRangeList (FromDate .. ToDate), a date the employee has no row for gets an off-duty row -
     * line = Max(DutyRoasterDetailLineId + 1), EmployeeHistoryId = Max(EmployeeHistoryId) of the whole list
     * (as the form does), IsOffDuty true, IsOnDuty false, ActionTypeId 2 when RecId &gt; 0 else 1.
     */
    private static void makeOffDaysRecords(GenDutyRoaster obj, long recId, LocalDateTime now, long userId) {
        List<GenDutyRoasterDetail> list = obj.GenDutyRoasterDetailsList;
        if (list.isEmpty()) return;
        Set<Long> employees = new LinkedHashSet<>();
        Set<String> have = new HashSet<>();
        long maxLine = Long.MIN_VALUE, maxHist = Long.MIN_VALUE;
        for (GenDutyRoasterDetail d : list) {
            employees.add(d.EmployeeId);
            have.add(d.EmployeeId + "|" + d.DutyDate);
            maxLine = Math.max(maxLine, d.DutyRoasterDetailLineId + 1);
            maxHist = Math.max(maxHist, d.EmployeeHistoryId);
        }
        List<LocalDateTime> range = new ArrayList<>();
        for (LocalDate x = obj.FromDate.toLocalDate(); !x.isAfter(obj.ToDate.toLocalDate()); x = x.plusDays(1)) range.add(x.atStartOfDay());
        for (Long emp : employees) {
            for (LocalDateTime date : range) {
                if (have.contains(emp + "|" + date)) continue;
                GenDutyRoasterDetail d = new GenDutyRoasterDetail();
                d.EmployeeId = emp;
                d.DutyRoasterDetailLineId = maxLine;
                d.EmployeeHistoryId = maxHist;
                d.DutyDate = date;
                d.IsOffDuty = true;
                d.IsOnDuty = false;
                d.ActionTypeId = recId > 0 ? 2 : 1;
                d.CreatedOn = now;
                d.AlteredOn = now;
                d.CreatedById = userId;
                d.AlteredById = userId;
                list.add(d);
                have.add(emp + "|" + date);
                maxLine = Math.max(maxLine, d.DutyRoasterDetailLineId + 1);
                maxHist = Math.max(maxHist, d.EmployeeHistoryId);
            }
        }
    }

    /**
     * DutyRoasterDetailIdsUpdateForUpdate: every row becomes ActionTypeId 2; then, for each row of
     * GetoffdaysfromDutyRoasterDetailByHeaderId(RecId) in order, every list row of that employee and date
     * takes its DutyRoasterDetailId.
     */
    private void dutyRoasterDetailIdsUpdateForUpdate(GenDutyRoaster obj, long recId) {
        Map<String, List<GenDutyRoasterDetail>> byKey = new HashMap<>();
        for (GenDutyRoasterDetail d : obj.GenDutyRoasterDetailsList) {
            d.ActionTypeId = 2;
            byKey.computeIfAbsent(d.EmployeeId + "|" + d.DutyDate, k -> new ArrayList<>()).add(d);
        }
        for (Map<String, Object> r : repo.offDays(recId)) {
            LocalDateTime dd = toDate(r.get("DutyDate"));
            List<GenDutyRoasterDetail> hit = byKey.get(toLong(r.get("EmployeeId")) + "|" + dd);
            if (hit == null) continue;
            long id = toLong(r.get("DutyRoasterDetailId"));
            for (GenDutyRoasterDetail d : hit) d.DutyRoasterDetailId = id;
        }
    }

    /** GetByID: the header columns the forms read and DutyRoasterGroupsList (DAL GetAll's child read). */
    private Map<String, Object> byId(UserAccount u, long id) {
        if (!owns(repo.rosters(u), "DutyRoasterId", (int) id)) throw invalid(NOT_FOUND);
        Map<String, Object> r = one(repo.roster(id));
        List<Map<String, Object>> groups = new ArrayList<>();
        for (Map<String, Object> g : repo.rosterGroups(id)) {
            groups.add(map("EmployeeGroupId", toInt(g.get("EmployeeGroupId")), "ShiftId", toInt(g.get("ShiftId")), "WeekDayId", toInt(g.get("WeekDayId"))));
        }
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("DutyRoasterId", toLong(r.get("DutyRoasterId")));
        out.put("FromDate", dateStr(r.get("FromDate")));
        out.put("ToDate", dateStr(r.get("ToDate")));
        out.put("Description", str(r.get("Description")));
        out.put("EmployeeCategoryId", toInt(r.get("EmployeeCategoryId")));
        out.put("DepartmentId", toInt(r.get("DepartmentId")));
        out.put("SectionId", toInt(r.get("SectionId")));
        out.put("LocationId", toInt(r.get("LocationId")));
        out.put("groups", groups);
        return out;
    }

    /**
     * The ReportsParameters of MannualDataSetInDutyRoaster, sent as GetDutyDatesForDutyRoaster sends it.
     * frmEmployeeRoaster fills only the dates, EmployeeId and DutyRoasterId.
     */
    private Map<String, Object> loadParams(UserAccount u, int scr, RosterLoadDto b) {
        if (b == null) b = new RosterLoadDto();
        LocalDateTime from = toDay(b.fromDate), to = toDay(b.toDate);
        if (from == null || to == null) throw invalid("Select the From Date and the To Date.");
        if (b.dutyRoasterId != 0 && !owns(repo.rosters(u), "DutyRoasterId", (int) b.dutyRoasterId)) throw invalid(NOT_FOUND);
        boolean duty = scr == SCREEN_DUTY_ROASTER;
        Map<String, Object> p = new LinkedHashMap<>();
        p.put("OrganizationId", u.getOrganizationId());
        p.put("CompanyId", u.getCompanyId());
        p.put("FromDate", ts(from));
        p.put("ToDate", ts(to));
        if (b.dutyRoasterId != 0L) p.put("DutyRoasterId", b.dutyRoasterId);
        String ids = duty ? departmentIds(b.departmentIds) : null;
        if (ids != null && !ids.isEmpty()) p.put("DepartmentIds", ids);
        if (duty && b.sectionId != 0) p.put("SectionId", b.sectionId);
        if (duty && b.employeeCategoryId != 0) p.put("EmployeeCategoryId", b.employeeCategoryId);
        if (b.employeeId != 0) p.put("EmployeeId", b.employeeId);
        if (duty && b.locationId != 0) p.put("LocationId", b.locationId);
        return p;
    }

    // ------------------------------------------------------------------ helpers

    private static int screen(int s) {
        if (s != SCREEN_DUTY_ROASTER && s != SCREEN_EMPLOYEE_ROASTER) throw invalid(NOT_FOUND);
        return s;
    }

    /** The ",3,7" string the form builds from cmbDepartment.Text (only digits and commas pass). */
    private static String departmentIds(String s) {
        String v = str(s).trim();
        if (v.isEmpty()) return null;
        if (!v.matches("[0-9,]*")) throw invalid(NOT_FOUND);
        return v;
    }

    private List<Map<String, Object>> weekProfiles(UserAccount u) { return pick(repo.weekProfiles(u), "ProfileId", "ProfileName"); }

    /** BindDDLNew(dt, CmbEmployeeName, "EmployeeId", "EmployeeName") - one row per EmployeeId value as bound. */
    private static List<Map<String, Object>> employees(List<Map<String, Object>> rows) { return pick(rows, "EmployeeId", "EmployeeName"); }

    /** BindDDLNew copies only the value and display columns. */
    private static List<Map<String, Object>> pick(List<Map<String, Object>> rows, String v, String t) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(map(v, r.get(v), t, r.get(t)));
        return out;
    }

    private static List<Map<String, Object>> maps(Table t) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Object[] r : t.rows) {
            Map<String, Object> m = new LinkedHashMap<>();
            for (int i = 0; i < t.cols.size(); i++) m.put(t.cols.get(i), json(r[i]));
            out.add(m);
        }
        return out;
    }

    private static Object json(Object v) {
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof Timestamp) {
            LocalDateTime d = ((Timestamp) v).toLocalDateTime();
            return d.toLocalTime().toSecondOfDay() == 0 ? d.toLocalDate().toString() : d.toString();
        }
        if (v instanceof LocalDate || v instanceof LocalDateTime) return v.toString();
        return v;
    }

    private static String key(long emp, long hist, int shift, String date, String weekDay, int group, long detailId, long rosterId) {
        return emp + "|" + hist + "|" + shift + "|" + date + "|" + weekDay + "|" + group + "|" + detailId + "|" + rosterId;
    }

    /** yyyy-MM-dd of a date value; "" when null. */
    static String dateStr(Object v) {
        LocalDateTime d = toDate(v);
        return d == null ? "" : d.toLocalDate().toString();
    }

    static String dateTimeStr(Object v) {
        LocalDateTime d = toDate(v);
        return d == null ? "" : d.toString();
    }

    /** DataRow value put into a string column of a DataTable: bool -> "True" / "False", DBNull -> "". */
    private static String boolText(Object v) {
        if (v == null) return "";
        return toBool(v) ? "True" : "False";
    }

    /** Conversion.ToInt64: 0 for null / not numeric. */
    static long toLong(Object v) {
        if (v == null) return 0L;
        if (v instanceof Number) return ((Number) v).longValue();
        String s = String.valueOf(v).trim();
        if (s.isEmpty()) return 0L;
        try { return Long.parseLong(s); } catch (NumberFormatException e) {
            try { return new BigDecimal(s).longValue(); } catch (NumberFormatException e2) { return 0L; }
        }
    }

    private static final String DBNULL = "Object cannot be cast from DBNull to other types.";

    /** Convert.ToInt64(DataRow value): DBNull throws. */
    private static long strictLong(Object v) {
        if (v == null) throw new IllegalStateException(DBNULL);
        return toLong(v);
    }

    /** Convert.ToBoolean(DataRow value): DBNull throws. */
    private static boolean strictBool(Object v) {
        if (v == null) throw new IllegalStateException(DBNULL);
        return toBool(v);
    }

    /** Convert.ToDateTime(DataRow value): DBNull throws. */
    private static String strictDate(Object v) {
        if (v == null) throw new IllegalStateException(DBNULL);
        return dateStr(v);
    }
}
