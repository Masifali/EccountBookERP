package com.mst.services.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.attendance.HrmManualAttendance;
import com.mst.models.hrm.dto.AttendanceFilterDto;
import com.mst.models.hrm.dto.AttendanceRowDto;
import com.mst.models.hrm.dto.AttendanceSaveDto;
import com.mst.repositories.hrm.HrmAttendanceRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.time.DayOfWeek;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.TextStyle;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * BLL of the HRM "Attendance Management" screens (AppModules 2022). Each method names the desktop
 * form method it reproduces; validation wording/order and the messages are the forms'.
 *
 *   658 frmDailyAttendance   (rights "DailyAttendance")   Load / Generate / Save / Print
 *   659 hrmManualAttendance  (rights "ManualAttendance")  Load / Generate / Update
 */
@Service
public class HrmAttendanceService {

    public static final int SCREEN_DAILY_ATTENDANCE = 658;
    public static final int SCREEN_MANUAL_ATTENDANCE = 659;
    /** btnEmployeeDefine / brnEmployee: SetRightsValueInRightsObject("EmployeeRegistration").DoHaveViewRight. */
    public static final int SCREEN_EMPLOYEE_REGISTRATION = 648;

    @Autowired private HrmAttendanceRepository repo;
    @Autowired private HrmSupport hrm;

    // ================================================================== shared: combos

    /**
     * The *ComboFill methods of both forms. Each list keeps only the value + display column the form
     * binds (BindDDL / BindDDLNew with ZeroIndex false - no "...Select Any Value..." row, nothing selected).
     */
    private Map<String, Object> combos(UserAccount u) {
        Map<String, Object> m = new HashMap<>();
        m.put("shifts", two(repo.shifts(u), "ShiftId", "ShiftName"));
        m.put("sections", two(repo.sections(u), "SectionId", "SectionName"));
        m.put("departments", two(repo.departments(u), "DepartmentId", "DepartmentName"));
        m.put("locations", two(repo.locations(u), "LocationId", "LocationName"));
        m.put("designations", two(repo.designations(u), "DesignationId", "DesignationName"));
        m.put("employeeGroups", two(repo.employeeGroups(u), "EmployeeGroupId", "EmployeeGroupName"));
        m.put("employeeCategories", two(repo.employeeCategories(u), "EmployeeCategoryId", "EmployeeCategoryName"));
        return m;
    }

    private static List<Map<String, Object>> two(List<Map<String, Object>> rows, String id, String name) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(map(id, r.get(id), name, r.get(name)));
        return out;
    }

    /** EmployeeName(): DataTable { Id = EmployeeId, EmployeeName } from GetAllEmployeesActive. */
    private List<Map<String, Object>> employeeList(UserAccount u, AttendanceFilterDto f) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.employeesActive(u, f == null ? new AttendanceFilterDto() : f))
            out.add(map("Id", r.get("EmployeeId"), "EmployeeName", r.get("EmployeeName")));
        return out;
    }

    private Map<String, Object> setup(UserAccount u, int screen) {
        Map<String, Object> m = combos(u);
        m.put("rights", hrm.rights(u, screen));
        m.put("employeeRegistrationView", hrm.can(u, SCREEN_EMPLOYEE_REGISTRATION, "View"));
        m.put("employees", employeeList(u, null));
        return m;
    }

    // ================================================================== 658 Daily Attendance

    /** frmDailyAttendance_Load: rights, ShiftNameComboFill, SectionFill, DepartmentNameComboFill, EmployeeName, LocationFilterFill, DesignationNameComboFill, EmployeeGroupComboFill, EmployeeCategoryFill. */
    public Map<String, Object> dailySetup() {
        return setup(hrm.user(SCREEN_DAILY_ATTENDANCE), SCREEN_DAILY_ATTENDANCE);
    }

    /** btnRefresh_Click: every combo filled again, EmployeeName() with the filter values as they stand. */
    public Map<String, Object> dailyCombos(AttendanceFilterDto f) {
        UserAccount u = hrm.user(SCREEN_DAILY_ATTENDANCE);
        Map<String, Object> m = combos(u);
        m.put("employees", employeeList(u, f));
        return m;
    }

    /** cmbShift_Leave / CmbLocation_Leave / cmbDepartment_Leave / CmbDesignation_Leave / cmbSection_Leave / CmbEmployeeCategory_Leave / CmbEmployeeGroup_Leave -> EmployeeName(). */
    public List<Map<String, Object>> dailyEmployees(AttendanceFilterDto f) {
        return employeeList(hrm.user(SCREEN_DAILY_ATTENDANCE), f);
    }

    /**
     * grdFill(): DocDate = txtDate; ChkRestDay checked -> IsRestDay = true (sent), otherwise
     * ApprovedFilter = "All" (@IsRestDay not sent). Rows in the form's dtgrid column order.
     */
    public List<Map<String, Object>> dailyLoad(AttendanceFilterDto f) {
        UserAccount u = hrm.user(SCREEN_DAILY_ATTENDANCE);
        LocalDateTime date = toDate(f.date);
        if (date == null) throw invalid("Please select a valid Date.");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.attendanceLoad(u, filterOf(f, false), date, f.isRestDay ? Boolean.TRUE : null)) {
            Map<String, Object> g = new java.util.LinkedHashMap<>();
            g.put("ManualAttendanceId", r.get("ManualAttendanceId"));
            g.put("Day", dayOfWeek(r.get("DutyDate")));
            g.put("EmployeeNo", r.get("EmployeeNo"));
            g.put("EmployeeId", r.get("EmployeeId"));
            g.put("LocationId", r.get("LocationId"));
            g.put("DepartmentId", r.get("DepartmentId"));
            g.put("ShiftId", r.get("ShiftId"));
            g.put("EmployeeName", r.get("EmployeeName"));
            g.put("DutyDate", r.get("DutyDate"));
            g.put("ShiftTiming", r.get("Timing"));
            g.put("InTime", hhmm(r.get("InDateTime")));
            g.put("OutTime", hhmm(r.get("OutDateTime")));
            g.put("InEntryMode", trimmed(r.get("InEntryMode")));
            g.put("OutEntryMode", trimmed(r.get("OutEntryMode")));
            g.put("Status", "");
            g.put("Description", r.get("Decsription"));
            g.put("FlagInTime", "");
            g.put("FlagOutTime", "");
            g.put("IsPresent", r.get("IsPresent"));
            g.put("IsLeave", r.get("IsLeave"));
            g.put("IsHoliday", r.get("IsHoliday"));
            g.put("IsSuspended", r.get("IsSuspended"));
            g.put("IsTerminate", r.get("IsTerminate"));
            g.put("CPLQuota", r.get("CPLQuota"));
            g.put("IsRestDay", toBool(r.get("IsRestDay")));
            g.put("IsAbsent", toBool(r.get("IsAbsent")));
            g.put("IsCPL", toBool(r.get("IsCPL")));
            out.add(g);
        }
        return out;
    }

    /**
     * btnsave_Click: "Grid Record Not Found Please Check" when the grid has no row; every row whose
     * InEntryMode or OutEntryMode is "M" and that has an In or an Out time becomes one hrmManualAttendance
     * (DutyDate + the cell's time, ActionTypeId 1 / 2 / 3 = IsAbsent, ManualCPLQuota = CPLQuota,
     * AttendanceStatus = Status, Description) and hrmManualAttendance.Save runs them in one transaction,
     * then the two Sp_SysUpdateAttendance* procedures. Message "Saved Seccessfully" (desktop spelling).
     */
    public Map<String, Object> dailySave(AttendanceSaveDto b) {
        UserAccount u = hrm.user(SCREEN_DAILY_ATTENDANCE);
        hrm.require(u, SCREEN_DAILY_ATTENDANCE, "Save");
        if (b == null || b.details == null || b.details.isEmpty()) throw invalid("Grid Record Not Found Please Check");
        LocalDateTime date = b.loaded == null ? null : toDate(b.loaded.date);
        if (date == null) throw invalid("Record not found.");
        Map<String, Map<String, Object>> own = ownRows(repo.attendanceLoad(u, new AttendanceFilterDto(), date, null));
        List<HrmManualAttendance> list = new ArrayList<>();
        for (AttendanceRowDto r : b.details) {
            boolean manual = "M".equals(trim(r.inEntryMode)) || "M".equals(trim(r.outEntryMode));
            if (!(manual && (!trim(r.inTime).isEmpty() || !trim(r.outTime).isEmpty()))) continue;
            Map<String, Object> dbRow = own.get(key(r.employeeId, r.shiftId, r.dutyDate));
            if (dbRow == null) throw invalid("Record not found.");
            BigDecimal cpl = toDec(r.cplQuota);
            boolean restDay = toBool(dbRow.get("IsRestDay"));
            // grd_CellUpdated guards, re-checked here: the page could not have sent these values.
            if (cpl.signum() != 0 && !restDay) throw invalid("CPL can only be Incremented against RestDay");
            if (r.isAbsent && restDay) throw invalid("You cannot Update IsAbsent because it is RestDay");
            HrmManualAttendance m = model(u, r);
            m.ManualCPLQuota = cpl;
            list.add(m);
        }
        int n = repo.saveManualAttendance(u.getOrganizationId(), u.getCompanyId(), list);
        return saved(n, "Saved Seccessfully");
    }

    /**
     * btnPrint_Click: HRM_Reports.DailyAttendanceRpt(... ApprovedFilter = "All"); "Not Record Found For Display"
     * when table 0 is empty, otherwise the 1003-DailyAttendance.rpt viewer (CrystalPrint 'hrm-1003-658').
     */
    public Map<String, Object> dailyPrintCheck(AttendanceFilterDto f) {
        UserAccount u = hrm.user(SCREEN_DAILY_ATTENDANCE);
        hrm.require(u, SCREEN_DAILY_ATTENDANCE, "Print");
        List<Map<String, Object>> rows = repo.dailyAttendanceRpt(u, filterOf(f, false), toDate(f.date));
        if (rows.isEmpty()) throw invalid("Not Record Found For Display");
        return map("success", true, "count", rows.size());
    }

    // ================================================================== 659 Manual Attendance

    /** frmDailyAttendance_Load (hrmManualAttendance): rights, MonthFill (current month), YearFill, EmployeeName and the combo fills. */
    public Map<String, Object> manualSetup() {
        UserAccount u = hrm.user(SCREEN_MANUAL_ATTENDANCE);
        Map<String, Object> m = setup(u, SCREEN_MANUAL_ATTENDANCE);
        m.put("years", years(u));
        m.put("month", LocalDate.now().getMonthValue());
        m.put("year", LocalDate.now().getYear());
        return m;
    }

    /** btnRefresh_Click: EmployeeName() first (filters as they stand), then the combo fills. */
    public Map<String, Object> manualCombos(AttendanceFilterDto f) {
        UserAccount u = hrm.user(SCREEN_MANUAL_ATTENDANCE);
        Map<String, Object> m = combos(u);
        m.put("employees", employeeList(u, f));
        return m;
    }

    /** The seven *_Leave handlers -> EmployeeName(). */
    public List<Map<String, Object>> manualEmployees(AttendanceFilterDto f) {
        return employeeList(hrm.user(SCREEN_MANUAL_ATTENDANCE), f);
    }

    /**
     * btnLoad_Click -> grdFill(): "Month Field Required" / "Year Field Required"; @IsRestDay is always sent
     * (ApprovedFilter is never set on this form, so it is not "All"): chkIsRestDay true / false.
     * No DutyDate (DocDate stays DateTime.MinValue, which CheckDateTimeNull drops).
     */
    public List<Map<String, Object>> manualLoad(AttendanceFilterDto f) {
        UserAccount u = hrm.user(SCREEN_MANUAL_ATTENDANCE);
        if (f.month == 0) throw invalid("Month Field Required");
        if (f.year == 0) throw invalid("Year Field Required");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.attendanceLoad(u, filterOf(f, true), null, f.isRestDay)) {
            Map<String, Object> g = new java.util.LinkedHashMap<>();
            g.put("ManualAttendanceId", r.get("ManualAttendanceId"));
            g.put("Day", dayOfWeek(r.get("DutyDate")));
            g.put("EmployeeNo", r.get("EmployeeNo"));
            g.put("EmployeeId", r.get("EmployeeId"));
            g.put("LocationId", r.get("LocationId"));
            g.put("DepartmentId", r.get("DepartmentId"));
            g.put("ShiftId", r.get("ShiftId"));
            g.put("Employee", r.get("EmployeeName"));
            g.put("DutyDate", r.get("DutyDate"));
            g.put("ShiftTiming", r.get("Timing"));
            g.put("InTime", hhmm(r.get("InDateTime")));
            g.put("OutTime", hhmm(r.get("OutDateTime")));
            g.put("InEntryMode", trimmed(r.get("InEntryMode")));
            g.put("OutEntryMode", trimmed(r.get("OutEntryMode")));
            g.put("Status", "");
            g.put("Description", "");
            g.put("FlagInTime", "");
            g.put("FlagOutTime", "");
            g.put("IsPresent", r.get("IsPresent"));
            g.put("IsLeave", r.get("IsLeave"));
            g.put("IsHoliday", r.get("IsHoliday"));
            g.put("IsRestDay", r.get("IsRestDay"));
            g.put("IsSuspended", r.get("IsSuspended"));
            g.put("IsTerminate", r.get("IsTerminate"));
            g.put("CPLQuota", r.get("CPLQuota"));
            g.put("IsAbsent", toBool(r.get("IsAbsent")));
            out.add(g);
        }
        return out;
    }

    /**
     * btnupdate_Click: "Grid Record Not Found Please Check"; rows whose InEntryMode or OutEntryMode is "M"
     * and that have BOTH an In and an Out time; ManualCPLQuota is never set here (0). Message
     * "Update Seccessfully" (desktop spelling).
     */
    public Map<String, Object> manualUpdate(AttendanceSaveDto b) {
        UserAccount u = hrm.user(SCREEN_MANUAL_ATTENDANCE);
        hrm.require(u, SCREEN_MANUAL_ATTENDANCE, "Update");
        if (b == null || b.details == null || b.details.isEmpty()) throw invalid("Grid Record Not Found Please Check");
        if (b.loaded == null || b.loaded.month == 0 || b.loaded.year == 0) throw invalid("Record not found.");
        AttendanceFilterDto own = new AttendanceFilterDto();
        own.month = b.loaded.month;
        own.year = b.loaded.year;
        Map<String, Map<String, Object>> rows = ownRows(repo.attendanceLoad(u, own, null, null));
        List<HrmManualAttendance> list = new ArrayList<>();
        for (AttendanceRowDto r : b.details) {
            boolean manual = "M".equals(trim(r.inEntryMode)) || "M".equals(trim(r.outEntryMode));
            if (!(manual && !trim(r.inTime).isEmpty() && !trim(r.outTime).isEmpty())) continue;
            if (!rows.containsKey(key(r.employeeId, r.shiftId, r.dutyDate))) throw invalid("Record not found.");
            list.add(model(u, r));
        }
        int n = repo.saveManualAttendance(u.getOrganizationId(), u.getCompanyId(), list);
        return saved(n, "Update Seccessfully");
    }

    // ================================================================== helpers

    /**
     * One hrmManualAttendance exactly as the two forms fill it: DutyDate = the cell; InDateTime /
     * OutDateTime = DutyDate.ToShortDateString() + " " + the cell's "hh:mm tt" (minutes precision);
     * Altered/CreatedById = user; Company/Organization from the session; AttendanceStatus = Status;
     * Description; Location/Department/Shift ids of the row; ActionTypeId 1 (new) / 2 (ManualAttendanceId set)
     * / 3 (IsAbsent); AlteredOn = CreatedOn = now. BranchId / UserLogId stay 0 and IsPresent null, as on the desktop.
     */
    private HrmManualAttendance model(UserAccount u, AttendanceRowDto r) {
        LocalDateTime duty = toDay(r.dutyDate);
        if (duty == null) throw invalid("String was not recognized as a valid DateTime.");
        HrmManualAttendance m = new HrmManualAttendance();
        m.DutyDate = duty;
        LocalTime in = time(r.inTime), out = time(r.outTime);
        if (in != null) m.InDateTime = duty.toLocalDate().atTime(in);
        if (out != null) m.OutDateTime = duty.toLocalDate().atTime(out);
        m.AlteredById = u.getId();
        m.CreatedById = u.getId();
        m.EmployeeId = r.employeeId;
        m.CompanyId = u.getCompanyId();
        m.OrganizationId = u.getOrganizationId();
        m.AttendanceStatus = str(r.status);
        m.Description = str(r.description);
        m.LocationId = r.locationId;
        m.DepartmentId = r.departmentId;
        m.ShiftId = r.shiftId;
        m.ManualAttendanceId = Math.max(0, r.manualAttendanceId);
        m.ActionTypeId = m.ManualAttendanceId == 0 ? 1 : 2;
        if (r.isAbsent) m.ActionTypeId = 3;
        LocalDateTime now = LocalDateTime.now();
        m.AlteredOn = now;
        m.CreatedOn = now;
        return m;
    }

    /** The filter as the form's grdFill sets it (the manual form sends Month / Year, the daily form DutyDate). */
    private static AttendanceFilterDto filterOf(AttendanceFilterDto f, boolean monthYear) {
        AttendanceFilterDto o = new AttendanceFilterDto();
        o.shiftId = f.shiftId; o.sectionId = f.sectionId; o.departmentId = f.departmentId; o.designationId = f.designationId;
        o.locationId = f.locationId; o.employeeGroupId = f.employeeGroupId; o.employeeCategoryId = f.employeeCategoryId;
        o.employeeId = f.employeeId;
        if (monthYear) { o.month = f.month; o.year = f.year; }
        return o;
    }

    /** The company's own attendance rows for the loaded date / month, keyed EmployeeId|ShiftId|DutyDate - the tenancy guard of a save. */
    private static Map<String, Map<String, Object>> ownRows(List<Map<String, Object>> rows) {
        Map<String, Map<String, Object>> m = new HashMap<>();
        for (Map<String, Object> r : rows) m.putIfAbsent(key(toInt(r.get("EmployeeId")), toInt(r.get("ShiftId")), str(r.get("DutyDate"))), r);
        return m;
    }

    private static String key(int employeeId, int shiftId, String dutyDate) {
        return employeeId + "|" + shiftId + "|" + trim(dutyDate).toLowerCase(Locale.ROOT);
    }

    /** Conversion.ToDateTime(DutyDate).DayOfWeek (DateTime.MinValue, a Monday, when blank). */
    private static String dayOfWeek(Object dutyDate) {
        LocalDateTime d = toDate(dutyDate);
        DayOfWeek w = d == null ? DayOfWeek.MONDAY : d.getDayOfWeek();
        return w.getDisplayName(TextStyle.FULL, Locale.ENGLISH);
    }

    private static final Pattern TIME = Pattern.compile("^\\s*(\\d{1,2}):(\\d{2})(?::\\d{2})?\\s*([AaPp])?\\.?[Mm]?\\.?\\s*$");

    /** The procedure's format(..., 'hh:mm tt') text -> "HH:mm" (the DateTime cell the form's dtgrid holds); "" for NULL. */
    static String hhmm(Object v) {
        LocalTime t = time(v == null ? null : String.valueOf(v));
        return t == null ? "" : String.format("%02d:%02d", t.getHour(), t.getMinute());
    }

    /** "HH:mm", "hh:mm AM/PM" -> LocalTime; null when blank. */
    static LocalTime time(String s) {
        if (s == null || s.trim().isEmpty()) return null;
        Matcher m = TIME.matcher(s);
        if (!m.matches()) throw invalid("String was not recognized as a valid DateTime.");
        int h = Integer.parseInt(m.group(1)), min = Integer.parseInt(m.group(2));
        String ap = m.group(3);
        if (ap != null) {
            if (h < 1 || h > 12) throw invalid("String was not recognized as a valid DateTime.");
            boolean pm = ap.equalsIgnoreCase("p");
            if (h == 12) h = pm ? 12 : 0; else if (pm) h += 12;
        }
        if (h > 23 || min > 59) throw invalid("String was not recognized as a valid DateTime.");
        return LocalTime.of(h, min);
    }

    private static String trimmed(Object v) { return v == null ? null : String.valueOf(v).trim(); }

    /** CommonServices.GetYears: ActiveYr.Start_Period.Year .. DateTime.Now.Year as { Id, Year }. */
    private List<Map<String, Object>> years(UserAccount u) {
        int now = LocalDate.now().getYear(), start = now;
        try {
            List<Map<String, Object>> fy = repo.activeYears(u);
            int id = hrm.financialYearId();
            Map<String, Object> row = null;
            for (Map<String, Object> r : fy) if (toInt(r.get("Id")) == id) { row = r; break; }
            if (row == null && !fy.isEmpty()) row = fy.get(0);
            LocalDateTime s = row == null ? null : toDate(row.get("Start_Period"));
            if (s != null) start = s.getYear();
        } catch (RuntimeException e) {
            start = now;
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (int y = start; y <= now; y++) out.add(map("Id", y, "Year", y));
        return out;
    }
}
