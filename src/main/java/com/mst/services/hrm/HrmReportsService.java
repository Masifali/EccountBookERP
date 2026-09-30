package com.mst.services.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.dto.HrmReportsFilterDto;
import com.mst.repositories.hrm.HrmReportsRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * The HRM report screens (Architecture.WinApp.HRM_Reports, dbo.ScreenDefinition 371-378): each Show
 * reproduces the form's GridHistory - the BLL call with the parameters the form fills, then the form's
 * own "dtcol" DataTable (same columns, same order, same renamed source columns such as
 * "Employee Id" -> EmployeeId) - so the page shows exactly the desktop grid. Prints are the
 * registered Crystal contracts (com.mst.reports.HrmReportsPrints); the few that run their own procedure
 * first (1002, 1005, 1110 / 1110B) get a row count here so the page can show the desktop's
 * "Not Record Found For Display" before opening the viewer.
 *
 * Deviations (all tenancy / crash fixes, the DB sees the desktop's calls otherwise):
 *  D1. Values leave as JSON-safe text: DateTime -> "yyyy-MM-dd HH:mm:ss", varbinary (CompLogoImage) dropped;
 *      columns the form's dtcol types as DateTime but the procedure returns as FORMAT() text
 *      ('dd-MM-yyyy' / 'dd-MMM-yyyy') are parsed to "yyyy-MM-dd" as DataTable.Rows.Add does.
 *  D2. 375 Duty Roster: HRM_Reports.genDutyRosterEmployeeWise sends only @EmployeeId / @Date, while
 *      Sp_genDutyRosterEmployeeWise_rpt INNER JOINs Company on @CompanyId - the desktop report is
 *      therefore always empty. The session company is sent, the picked employee must be one of the
 *      company's employees, and rows of other companies' employees are dropped (the procedure has no
 *      company filter of its own).
 *  D3. 374 Daily Late & Early: Sp_hrmDailyLED_rpt filters no company either; rows whose EmployeeNo is
 *      not one of the company's employees are dropped, and EmployeeId (for the employee link) is looked
 *      up from the company's employee list by EmployeeNo.
 */
@Service
public class HrmReportsService {

    public static final int SCREEN_EMPLOYEE_REGISTER = 371;
    public static final int SCREEN_EMPLOYEE_MONTHLY_REGISTER = 372;
    public static final int SCREEN_DAILY_ATTENDANCE = 373;
    public static final int SCREEN_DAILY_LATE_EARLY = 374;
    public static final int SCREEN_DUTY_ROSTER = 375;
    public static final int SCREEN_EMPLOYEE_ATTENDENCE = 376;
    public static final int SCREEN_MONTHLY_SUMMARY = 377;
    public static final int SCREEN_SALARY_SHEET = 378;

    @Autowired private HrmReportsRepository repo;
    @Autowired private HrmSupport hrm;

    // ================================================================== dtcol layouts ("Target" or "Target=Source")

    /** EmployeeHistoryRpt.GridHistory dtcol. */
    static final String[] COLS_371 = { "EmployeeId", "EmployeeNo", "EmployeeName", "DOB", "LocationName", "DepartmentName", "DesignationName",
            "SectionName", "JoiningDate", "CNIC", "MobileNo", "TotalSalary", "CurrentGlBalance", "ProbationEnding", "Active" };

    /** HRM_Reports.btnShow_Click dt (Detail Register tab). */
    static final String[] COLS_372 = { "Sr#", "RM", "RY", "EmployeeAttendanceId", "DutyRoasterDetailId", "ATD_DATE", "CurrentDate",
            "RangeInDateTime", "RangeOutDateTime", "DAYNAME", "EmployeeId", "EmployeeNo", "EmployeeName", "Mobile1", "CNIC", "Email",
            "DepartmentName", "DesignationName", "InEntryMode", "OutEntryMode", "InDateTime", "OutDateTime", "SystemOutDateTime",
            "ShiftTiming", "InTime", "OutTime", "TimeDuration", "Late", "Early", "Status", "IsLateArrival", "SelectedMonth", "Timing",
            "Shift", "IsGazetted", "IsOffDay", "DRIsOffDuty", "DRShiftId", "GraceInDateTime", "GraceOutDateTime", "StartTime", "EndTime",
            "Decsription", "Short", "OT", "ShortHours", "ShortMinutes", "OTHours", "OTMinutes", "EmployeeHours", "EmployeeMinutes",
            "TotalShortHour", "TotalShortMinutes", "TotalOTMinutes", "CPLQuota", "CPLEntryMode", "TOTAL_HOUR", "Actual_HOUR",
            "EMPLOYEE_WORKINGHOUR", "OverTime", "LateMinutes", "IsRestDay", "IsHoliday", "IsLeave", "IsLeaveApproved", "IsSuspended",
            "IsAbsent", "IsPresent", "IsArrivalShortLeave", "IsDepartureShortLeave", "IsArrivalHalfDay", "IsDepartureHalfDay",
            "IsEarlyDeparture", "IsCPL", "AttendanceStatus", "BenefitPrefix" };
    static final Set<String> DATES_372 = set("ATD_DATE", "CurrentDate", "RangeInDateTime", "RangeOutDateTime", "InDateTime", "OutDateTime",
            "SystemOutDateTime", "GraceInDateTime", "GraceOutDateTime", "StartTime", "EndTime");

    /** HRM_Reports.GridHistory (Short Register tab) and genEmployeeAttendence.GridHistory dtcol. */
    static final String[] COLS_ATT = { "SRNo", "EmployeeId", "EmployeeNo", "EmployeeName", "DepartmentName", "CNIC", "Email", "Mobile1",
            "Mobile2", "OutDateTime", "InDateTime", "InTime", "OutTime", "TimeDifference" };
    static final Set<String> DATES_ATT = set("OutDateTime", "InDateTime");

    /** DailyAttendanceRpt.GridHistory dtcol (InTime / OutTime are built below). */
    static final String[] COLS_373 = { "Sr#", "DepartmentId", "EmployeeId=Employee Id", "PartyLocationId", "Party", "EmployeeNo=Employee No",
            "Mobile1", "Email", "BranchName", "ShiftId", "Employee", "Section", "Department", "Designation", "Shift", "DayName",
            "SelectedMonth", "ShiftTiming", "InTime", "OutTime", "TimeDifference", "Date", "Status" };

    /** DailyLateandEarlyDeparture.GridHistory dtcol. */
    static final String[] COLS_374 = { "SRNo", "EmployeeNo=Employee No", "Employee", "PartyLocationId", "PartyName", "DepartmentId",
            "Department", "Designation", "Shift", "Timing", "EmployeeInTime", "InTimeDifference=Difference", "EmployeeOutTime",
            "OutTimeDifference", "TotalHour=Total Hour", "Early", "Late", "Date" };

    /** genDutyRosterEmployeeWise.GridHistory dtcol. */
    static final String[] COLS_375 = { "EmployeeId", "EmployeeNo", "EmployeeName", "CNIC", "Mobile1", "EmployeeType", "DesignationName",
            "DepartmentName", "ShiftName", "ShiftShortName", "Date", "Year", "Month", "Timing", "IsOffDuty" };

    /** MonthlyAttendanceSummary.GridHistory dtcol. */
    static final String[] COLS_377 = { "EmployeeId", "ShiftId", "ShiftName", "EmployeeNo", "EmployeeName", "DepartmentName", "DesignationName",
            "Section=EmployeeSectionName", "AsOnDate", "DiffDays", "DutyMonth", "DutyYear", "TotalDays", "TotalHoliday", "TotalOffDAys",
            "DutyOffDays", "TotalPresent", "TotalAbsent", "ApplyLeave", "TotalLeaves", "TotalLate", "Month", "Year", "DutyDate",
            "TOTAL_HOUR", "Actual_HOUR", "EMPLOYEE_WORKINGHOUR", "OT" };

    /** PayRollSalarySheetRpt_Load dtcol (ArrearAmount is read from the procedure's "ArearAmount"). */
    static final String[] COLS_378 = { "EmployeeId", "EmployeeHistoryId", "AccountCode", "EmployeeNo", "EmployeeName", "JoinDate",
            "DesignationName", "DepartmentName", "SectionName", "DepartmentId", "MONTH", "YEAR", "TotalYear", "PresentDays", "OffDays",
            "LeaveDays", "SalaryDays", "AbsentDays", "EmployeeDays", "EmployeePerDaySalary", "EmployeePayrollSalary", "BasicSalary",
            "GrossSalary", "LateDeductionAmount", "IncomeTaxAmount", "LeaveAmount", "AdvanceAmount", "LoanAmount", "NoOfInstallments",
            "PFAmount", "EOBIAmount", "TotalLessAmount", "OTAmount", "TravellingAmount", "MedicalAmount", "FuelAmount", "MiscLessAmount",
            "MiscAddAmount", "MobileAmount", "FoodAmount", "ArrearAmount=ArearAmount", "TotalAddition", "AddLessAmount", "NetSalary" };

    // ================================================================== combos (shared)

    private List<Map<String, Object>> pairs(List<Map<String, Object>> rows, String id, String name) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(map("Id", r.get(id), "Name", r.get(name)));
        return out;
    }

    private List<Map<String, Object>> departments(UserAccount u) { return pairs(repo.departments(u), "DepartmentId", "DepartmentName"); }
    private List<Map<String, Object>> sections(UserAccount u) { return pairs(repo.sections(u), "SectionId", "SectionName"); }
    private List<Map<String, Object>> shifts(UserAccount u) { return pairs(repo.shifts(u), "ShiftId", "ShiftName"); }
    private List<Map<String, Object>> designations(UserAccount u) { return pairs(repo.designations(u), "DesignationId", "DesignationName"); }
    private List<Map<String, Object>> partyLocations(UserAccount u) { return pairs(repo.partyLocations(u), "PartyLocationId", "PartyLocationName"); }
    private List<Map<String, Object>> locations(UserAccount u) { return pairs(repo.locations(u), "LocationId", "LocationName"); }

    /** genEmployeeHistory.GetAllEmployeesActive -> BindDDLNew(EmployeeId, EmployeeName). */
    private List<Map<String, Object>> activeEmployees(UserAccount u, int dept, int section, int partyLocation, int shift) {
        return pairs(repo.activeEmployees(u, dept, section, partyLocation, shift), "EmployeeId", "EmployeeName");
    }

    /** Section(departmentId): genSection.Getall rows WHERE DepartmentId == departmentId (LINQ on the form). */
    private List<Map<String, Object>> sectionsOfDepartment(UserAccount u, int departmentId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.sections(u))
            if (toInt(r.get("DepartmentId")) == departmentId) out.add(map("Id", r.get("SectionId"), "Name", r.get("SectionName")));
        return out;
    }

    // ================================================================== 371 Employee Register (EmployeeHistoryRpt.cs)

    /** DailyAttendanceRpt_Load: DepartmentFill, SectionFill, ShiftFill, DesignationFill, EmployeeFill (genEmployee.Getall). */
    public Map<String, Object> employeeRegisterSetup() {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_REGISTER);
        return map("departments", departments(u), "sections", sections(u), "shifts", shifts(u), "designations", designations(u),
                "employees", pairs(repo.employees(u), "EmployeeId", "EmployeeName"));
    }

    /** GridHistory: IsApproved from cmbIsActive (1 -> true, else false); no active row -> ApprovedFilter "All" (@Active not sent). */
    public Map<String, Object> employeeRegisterShow(HrmReportsFilterDto f) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_REGISTER);
        Boolean active = f.status == 0 ? null : (f.status == 1);
        List<Map<String, Object>> rows = repo.employeeHistory(u, f.shiftId, f.sectionId, f.departmentId, f.designationId, f.employeeId, active);
        return map("rows", project(rows, COLS_371, set("DOB", "JoiningDate", "ProbationEnding")));
    }

    // ================================================================== 372 Employee Monthly Register (HRM_Reports.cs)

    /** HRM_Reports_Load: Employee() (tab 0, nothing picked), Department(), PartyLocation(), Shift(). */
    public Map<String, Object> monthlyRegisterSetup() {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_MONTHLY_REGISTER);
        return map("employees", activeEmployees(u, 0, 0, 0, 0), "departments", departments(u),
                "partyLocations", partyLocations(u), "shifts", shifts(u));
    }

    /**
     * Employee(): tab 0 sends cmbDepartment / cmbShift / cmbSection / cmbPartyLocation, tab 1 only
     * cmbDepartmentRegister (the page posts that one as departmentId).
     */
    public List<Map<String, Object>> monthlyRegisterEmployees(HrmReportsFilterDto f) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_MONTHLY_REGISTER);
        if (f.tab == 1) return activeEmployees(u, f.departmentId, 0, 0, 0);
        return activeEmployees(u, f.departmentId, f.sectionId, f.partyLocationId, f.shiftId);
    }

    /** cmbDepartment_ValueChanged -> Section(departmentId). */
    public List<Map<String, Object>> monthlyRegisterSections(int departmentId) {
        return sectionsOfDepartment(hrm.user(SCREEN_EMPLOYEE_MONTHLY_REGISTER), departmentId);
    }

    /** btnShow_Click: HRM_Reports.EmployeeMonthlyAttendenceRpt (DocDate is filled but the BLL does not send it). */
    public Map<String, Object> monthlyRegisterShow(HrmReportsFilterDto f) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_MONTHLY_REGISTER);
        List<Map<String, Object>> rows = repo.employeeMonthlyAttendence(u, f.month, f.year, f.employeeId, f.departmentId, f.sectionId,
                f.partyLocationId, f.shiftId);
        return map("rows", project(rows, COLS_372, DATES_372));
    }

    /** btn1002Print_Click: HRM_Reports.MonthlyAttendanceRegisterRpt - the rows decide "Not Record Found For Display". */
    public Map<String, Object> monthlyRegisterPrint1002Check(HrmReportsFilterDto f) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_MONTHLY_REGISTER);
        return map("rows", repo.attendanceRegister(u, f.month, f.year, f.departmentId, f.employeeId, f.shiftId, f.sectionId, f.partyLocationId).size());
    }

    /** btnShowRegister_Click -> GridHistory: HRM_Reports.genEmployeeAttendence. */
    public Map<String, Object> monthlyRegisterShowRegister(HrmReportsFilterDto f) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_MONTHLY_REGISTER);
        return map("rows", project(repo.employeeAttendence(u, f.employeeId, f.departmentId, day(f.fromDate), day(f.toDate)), COLS_ATT, DATES_ATT));
    }

    // ================================================================== 373 Daily Attendance (DailyAttendanceRpt.cs)

    public Map<String, Object> dailyAttendanceSetup() {
        UserAccount u = hrm.user(SCREEN_DAILY_ATTENDANCE);
        return map("departments", departments(u), "employees", activeEmployees(u, 0, 0, 0, 0));
    }

    /** EmployeeFill (Load, Refresh, cmbDepartment_Leave): active employees of cmbDepartment. */
    public List<Map<String, Object>> employeesOfDepartment(int screen, int departmentId) {
        return activeEmployees(hrm.user(screen), departmentId, 0, 0, 0);
    }

    /** GridHistory: InTime / OutTime = value + "  " + EntryMode, Date = Conversion.ToDateTime(Date). */
    public Map<String, Object> dailyAttendanceShow(HrmReportsFilterDto f) {
        UserAccount u = hrm.user(SCREEN_DAILY_ATTENDANCE);
        List<Map<String, Object>> src = repo.dailyAttendance(u, f.departmentId, day(f.date), f.employeeId);
        List<Map<String, Object>> rows = project(src, COLS_373, set("Date"));
        for (int i = 0; i < rows.size(); i++) {
            Map<String, Object> s = src.get(i), r = rows.get(i);
            r.put("InTime", str(s.get("InTime")) + "  " + str(s.get("InEntryMode")));
            r.put("OutTime", str(s.get("OutTime")) + "  " + str(s.get("OutEntryMode")));
        }
        return map("rows", rows);
    }

    // ================================================================== 374 Daily Late & Early Departure (DailyLateandEarlyDeparture.cs)

    /** DepartmentFill (really genLocation.Getall -> cmbPartyLocation "Location"). */
    public Map<String, Object> dailyLateEarlySetup() {
        UserAccount u = hrm.user(SCREEN_DAILY_LATE_EARLY);
        return map("locations", locations(u));
    }

    public Map<String, Object> dailyLateEarlyShow(HrmReportsFilterDto f) {
        UserAccount u = hrm.user(SCREEN_DAILY_LATE_EARLY);
        List<Map<String, Object>> rows = project(repo.dailyLateAndEarly(u, f.partyLocationId, day(f.date)), COLS_374, set("Date"));
        Map<String, Object> idByNo = new LinkedHashMap<>();
        for (Map<String, Object> e : repo.employees(u)) idByNo.put(str(e.get("EmployeeNo")).trim(), e.get("EmployeeId"));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) {                                  // D3
            String no = str(r.get("EmployeeNo")).trim();
            if (!idByNo.containsKey(no)) continue;
            r.put("EmployeeId", idByNo.get(no));
            out.add(r);
        }
        return map("rows", out);
    }

    // ================================================================== 375 Duty Roster Employee Wise (genDutyRosterEmployeeWise.cs)

    public Map<String, Object> dutyRosterSetup() {
        UserAccount u = hrm.user(SCREEN_DUTY_ROSTER);
        return map("employees", activeEmployees(u, 0, 0, 0, 0));
    }

    public Map<String, Object> dutyRosterShow(HrmReportsFilterDto f) {
        UserAccount u = hrm.user(SCREEN_DUTY_ROSTER);
        List<Map<String, Object>> mine = repo.employees(u);
        if (f.employeeId > 0 && !owns(mine, "EmployeeId", f.employeeId)) throw invalid("Record not found.");
        Set<Integer> ids = new HashSet<>();
        for (Map<String, Object> e : mine) ids.add(toInt(e.get("EmployeeId")));
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : project(repo.dutyRoster(u, f.employeeId, day(f.date)), COLS_375, set("Date")))
            if (ids.contains(toInt(r.get("EmployeeId")))) out.add(r);                                  // D2
        return map("rows", out);
    }

    // ================================================================== 376 Employee Attendence (genEmployeeAttendence.cs)

    public Map<String, Object> employeeAttendenceSetup() {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_ATTENDENCE);
        return map("departments", departments(u), "employees", activeEmployees(u, 0, 0, 0, 0));
    }

    public Map<String, Object> employeeAttendenceShow(HrmReportsFilterDto f) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_ATTENDENCE);
        return map("rows", project(repo.employeeAttendence(u, f.employeeId, f.departmentId, day(f.fromDate), day(f.toDate)), COLS_ATT, DATES_ATT));
    }

    // ================================================================== 377 Monthly Attendance Summary (MonthlyAttendanceSummary.cs)

    /** Load: DepartmentFill, EmployeeFill, SectionFill (all sections, BindAndRetainSelection without a default row). */
    public Map<String, Object> monthlySummarySetup() {
        UserAccount u = hrm.user(SCREEN_MONTHLY_SUMMARY);
        return map("departments", departments(u), "employees", activeEmployees(u, 0, 0, 0, 0), "sections", sections(u));
    }

    public Map<String, Object> monthlySummaryShow(HrmReportsFilterDto f) {
        UserAccount u = hrm.user(SCREEN_MONTHLY_SUMMARY);
        List<Map<String, Object>> rows = repo.monthlyAttendanceSummary(u, f.month, f.year, f.departmentId, f.employeeId, f.sectionId);
        return map("rows", project(rows, COLS_377, set("AsOnDate", "DutyDate")));
    }

    /** btnMonthlyRegister_Click: HRM_Reports.MonthlyAttendanceRegisterNew - row count before 1005 prints. */
    public Map<String, Object> monthlySummaryPrint1005Check(HrmReportsFilterDto f) {
        UserAccount u = hrm.user(SCREEN_MONTHLY_SUMMARY);
        return map("rows", repo.monthlyAttendanceRegisterNew(u, f.month, f.year, f.employeeId, f.departmentId).size());
    }

    // ================================================================== 378 Salary Sheet (PayRollSalarySheetRpt.cs)

    /**
     * Load: YearFill (CommonServices.GetYears: ActiveYr.Start_Period.Year .. DateTime.Now.Year), MonthFill
     * (GetMonths), DepartmentFill, EmployeeFill, GetAccountsFromEmployee("Expense").
     */
    public Map<String, Object> salarySheetSetup() {
        UserAccount u = hrm.user(SCREEN_SALARY_SHEET);
        int from = LocalDate.now().getYear();
        List<Map<String, Object>> fy = repo.financialYears(u);
        int fyId = hrm.financialYearId();
        Map<String, Object> row = null;
        for (Map<String, Object> r : fy) if (toInt(r.get("Id")) == fyId) { row = r; break; }
        if (row == null && !fy.isEmpty()) row = fy.get(0);
        LocalDateTime start = row == null ? null : toDate(row.get("Start_Period"));
        if (start != null) from = start.getYear();
        List<Map<String, Object>> years = new ArrayList<>();
        for (int y = from; y <= LocalDate.now().getYear(); y++) years.add(map("Id", y, "Year", y));
        List<Map<String, Object>> accounts = new ArrayList<>();
        for (Map<String, Object> r : repo.debitAccounts(u)) accounts.add(map("Id", r.get("Id"), "AccountTitle", r.get("AccountTitle")));
        return map("years", years, "departments", departments(u), "employees", activeEmployees(u, 0, 0, 0, 0), "accounts", accounts);
    }

    /** cmbDepartment_Leave: EmployeeFill + Section() (sections of the department). */
    public Map<String, Object> salarySheetDepartment(int departmentId) {
        UserAccount u = hrm.user(SCREEN_SALARY_SHEET);
        return map("employees", activeEmployees(u, departmentId, 0, 0, 0), "sections", sectionsOfDepartment(u, departmentId));
    }

    /** GridHistory: "Month field Required..." / "Year field Required..."; RadUnPosted -> payroll, RadPosted -> posted. */
    public Map<String, Object> salarySheetShow(HrmReportsFilterDto f) {
        UserAccount u = hrm.user(SCREEN_SALARY_SHEET);
        if (f.month < 1 || f.month > 12) throw invalid("Month field Required...");
        if (f.year <= 0) throw invalid("Year field Required...");
        String monthName = java.time.Month.of(f.month).getDisplayName(java.time.format.TextStyle.FULL, Locale.ENGLISH);
        List<Map<String, Object>> src = repo.salary(u, f.posted, f.employeeId, f.departmentId, f.sectionId, f.debitAccountId, monthName, f.year, f.month);
        List<Map<String, Object>> rows = project(src, COLS_378, set("JoinDate"));
        return map("rows", rows);
    }

    /** btn1110SalaryList_Click / SalarySlipRpt: "Month Or Year Field Required", then the slip's row count. */
    public Map<String, Object> salarySlipCheck(HrmReportsFilterDto f) {
        UserAccount u = hrm.user(SCREEN_SALARY_SHEET);
        if (f.month < 1 || f.month > 12 || f.year <= 0) throw invalid("Month Or Year Field Required");
        return map("rows", repo.salarySlip(u, f.month, f.year, f.employeeId, f.departmentId, f.debitAccountId).size());
    }

    // ================================================================== helpers

    private static Set<String> set(String... a) { return new HashSet<>(Arrays.asList(a)); }

    /** "yyyy-MM-dd" -> midnight Timestamp; null when blank (CheckDateTimeNull guard). */
    private static Timestamp day(String s) { return ts(toDay(s)); }

    /** The form's dtcol: target columns in the form's order, read from the source column (desktop names). */
    static List<Map<String, Object>> project(List<Map<String, Object>> rows, String[] spec, Set<String> dateCols) {
        if (rows == null) return Collections.emptyList();
        List<Map<String, Object>> out = new ArrayList<>(rows.size());
        for (Map<String, Object> r : rows) {
            Map<String, Object> o = new LinkedHashMap<>();
            for (String c : spec) {
                int eq = c.indexOf('=');
                String target = eq < 0 ? c : c.substring(0, eq), source = eq < 0 ? c : c.substring(eq + 1);
                Object v = r.get(source);
                o.put(target, dateCols.contains(target) ? dateText(v) : json(v));
            }
            out.add(o);
        }
        return out;
    }

    private static final DateTimeFormatter DT = DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm:ss");

    /** D1: JSON-safe value. */
    static Object json(Object v) {
        if (v == null) return null;
        if (v instanceof byte[]) return null;
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().format(DT);
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).format(DT);
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof LocalDate) return v.toString();
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().format(DT);
        if (v instanceof BigDecimal || v instanceof Number || v instanceof Boolean || v instanceof String) return v;
        return String.valueOf(v);
    }

    /** A DateTime column of dtcol: date values as they are, FORMAT() text ('dd-MM-yyyy', 'dd-MMM-yyyy') parsed. */
    static Object dateText(Object v) {
        if (v == null) return null;
        if (!(v instanceof String)) return json(v);
        String s = ((String) v).trim();
        if (s.isEmpty()) return null;
        try {
            if (s.matches("\\d{1,2}-\\d{1,2}-\\d{4}")) return LocalDate.parse(s, DateTimeFormatter.ofPattern("d-M-yyyy")).toString();
        } catch (RuntimeException e) { return s; }
        LocalDateTime d = toDate(s);
        return d == null ? s : (d.toLocalTime().equals(java.time.LocalTime.MIDNIGHT) ? d.toLocalDate().toString() : d.format(DT));
    }
}
