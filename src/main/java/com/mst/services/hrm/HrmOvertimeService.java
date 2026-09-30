package com.mst.services.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.dto.OvertimeEmployeeDto;
import com.mst.models.hrm.dto.OvertimeEmployeeRowDto;
import com.mst.models.hrm.dto.OvertimeRequestDto;
import com.mst.models.hrm.dto.OvertimeRequestRowDto;
import com.mst.models.hrm.overtime.EmployeeOverTime;
import com.mst.models.hrm.overtime.OverTimeRequest;
import com.mst.models.hrm.overtime.OverTimeRequestDetail;
import com.mst.repositories.hrm.HrmOvertimeRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.sql.Time;
import java.sql.Timestamp;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * BLL of the HRM "Over Time Management" screens (AppModules 2025). Each method names the desktop
 * form method it reproduces; validation wording / order and the messages are the form's.
 *
 *   665 OverTimeRequest.cs (+ popup EmpOverTimeLoadForRequest.cs, a ShowDialog returning dtLoader)
 *   666 frmEmployeeOverTime.cs
 */
@Service
public class HrmOvertimeService {

    public static final int SCREEN_OVERTIME_REQUEST = 665;
    public static final int SCREEN_EMPLOYEE_OVERTIME = 666;

    @Autowired private HrmOvertimeRepository repo;
    @Autowired private HrmSupport hrm;

    // ================================================================== shared

    /** DataTable values as JSON-friendly values: date-times as ISO strings (never epoch numbers). */
    private static Object jsonVal(Object v) {
        if (v instanceof Timestamp) return ((Timestamp) v).toLocalDateTime().format(DateTimeFormatter.ISO_LOCAL_DATE_TIME);
        if (v instanceof java.sql.Date) return ((java.sql.Date) v).toLocalDate().toString();
        if (v instanceof Time) return ((Time) v).toLocalTime().format(DateTimeFormatter.ofPattern("HH:mm:ss"));
        if (v instanceof LocalDateTime) return ((LocalDateTime) v).format(DateTimeFormatter.ISO_LOCAL_DATE_TIME);
        if (v instanceof java.util.Date) return new Timestamp(((java.util.Date) v).getTime()).toLocalDateTime().format(DateTimeFormatter.ISO_LOCAL_DATE_TIME);
        return v;
    }

    private static Map<String, Object> jsonRow(Map<String, Object> r) {
        Map<String, Object> o = new LinkedHashMap<>();
        for (Map.Entry<String, Object> e : r.entrySet()) o.put(e.getKey(), jsonVal(e.getValue()));
        return o;
    }

    private static String iso(Object v) { Object o = jsonVal(v); return o == null ? null : String.valueOf(o); }

    /** Math.Round(double, 3) - .NET rounds half to even. */
    private static double round3(double v) { return BigDecimal.valueOf(v).setScale(3, RoundingMode.HALF_EVEN).doubleValue(); }

    /** DataTable Double column fed a string ("9.30", "1.30"): parsed; "-" / blank (the desktop's ArgumentException) -> null. */
    private static Double dbl(Object v) {
        if (v == null) return null;
        if (v instanceof Number) return ((Number) v).doubleValue();
        String s = String.valueOf(v).trim();
        try { return s.isEmpty() ? null : Double.parseDouble(s); } catch (NumberFormatException e) { return null; }
    }

    private List<Map<String, Object>> idName(List<Map<String, Object>> rows, String id, String name, String nameKey) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : rows) out.add(map("Id", r.get(id), nameKey, r.get(name)));
        return out;
    }

    // ================================================================== 665 OverTimeRequest

    /**
     * FrmExportSalesContract_Load: rights (SetRightsValueInRightsObject("OverTimeRequest")),
     * DepartmentFill / SectionFill / RequestedByFill (the same lists fill the History combos),
     * DefaultDaysToLessFromHistoryFromDate.
     */
    public Map<String, Object> requestSetup() {
        UserAccount u = hrm.user(SCREEN_OVERTIME_REQUEST);
        Map<String, Object> out = requestCombos(u);
        out.put("rights", hrm.rights(u, SCREEN_OVERTIME_REQUEST));
        out.put("defaultDays", toInt(repo.config(u, "DefaultDaysToLessFromHistoryFromDate")));
        return out;
    }

    /** btnRefresh_Click_1 / btnhistoryrefresh_Click: DepartmentFill(); SectionFill(); RequestedByFill(). */
    public Map<String, Object> requestCombos() { return requestCombos(hrm.user(SCREEN_OVERTIME_REQUEST)); }

    private Map<String, Object> requestCombos(UserAccount u) {
        List<Map<String, Object>> emps = new ArrayList<>();
        for (Map<String, Object> r : repo.employees(u))       // dtEmployees: EmployeeId / EmployeeName, DepartmentId for cmbDepartmnet_Leave
            emps.add(map("EmployeeId", r.get("EmployeeId"), "EmployeeName", r.get("EmployeeName"), "DepartmentId", r.get("DepartmentId")));
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("departments", idName(repo.departments(u), "DepartmentId", "DepartmentName", "Name"));
        out.put("sections", idName(repo.sections(u), "SectionId", "SectionName", "Name"));
        out.put("employees", emps);
        return out;
    }

    /** OverTimeRequest.GetByID with the company guard (ReadById itself does not filter by company). */
    private Map<String, Object> ownedRequest(UserAccount u, long id) {
        Map<String, Object> h = one(repo.request(id));
        if (toInt(h.get("OrganizationId")) != u.getOrganizationId() || toInt(h.get("CompanyId")) != u.getCompanyId())
            throw invalid("Record not found.");
        return h;
    }

    /** ReadById(ID): the header controls and dtdetail (Id, EmployeeId, EmployeeName, FromDateTime, ToDateTime, OTHours, OTRate, IsAllowMeal, EntryByLoader = RequestStatus). */
    public Map<String, Object> request(long id) {
        UserAccount u = hrm.user(SCREEN_OVERTIME_REQUEST);
        Map<String, Object> h = ownedRequest(u, id);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.requestDetails(id))
            rows.add(map("Id", d.get("OverTimeRequestDetailId"), "EmployeeId", d.get("EmployeeId"), "EmployeeName", d.get("EmployeeName"),
                    "FromDateTime", iso(d.get("FromDateTime")), "ToDateTime", iso(d.get("ToDateTime")), "OTHours", d.get("OTHours"),
                    "OTRate", d.get("OTRate"), "IsAllowMeal", toBool(d.get("IsAllowMeal")), "EntryByLoader", toBool(d.get("RequestStatus"))));
        Map<String, Object> o = map("OverTimeRequestId", h.get("OverTimeRequestId"), "RequestDate", iso(h.get("RequestDate")),
                "OverTimeDate", iso(h.get("OverTimeDate")), "DepartmentId", h.get("DepartmentId"), "SectionId", h.get("SectionId"),
                "IsOffDuty", toBool(h.get("IsOffDuty")), "Reason", str(h.get("Reason")), "RequestById", h.get("RequestById"));
        o.put("details", rows);
        return o;
    }

    /** DataGridHistory_SelectionChanged: GetByID(RecId).OverTimeRequestDetailList -> EmployeeName, FromDateTime, ToDateTime, OTHours, OTRate, IsAllowMeal. */
    public List<Map<String, Object>> requestHistoryDetail(long id) {
        UserAccount u = hrm.user(SCREEN_OVERTIME_REQUEST);
        ownedRequest(u, id);
        List<Map<String, Object>> rows = new ArrayList<>();
        for (Map<String, Object> d : repo.requestDetails(id))
            rows.add(map("EmployeeName", d.get("EmployeeName"), "FromDateTime", iso(d.get("FromDateTime")), "ToDateTime", iso(d.get("ToDateTime")),
                    "OTHours", d.get("OTHours"), "OTRate", d.get("OTRate"), "IsAllowMeal", toBool(d.get("IsAllowMeal"))));
        return rows;
    }

    /**
     * HistoryFill: the radio picks which pair of ReportsParameters dates the two pickers fill
     * (Requested -> @RequestFromDate/@RequestToDate, Over Time -> @OverTimeFromDate/@OverTimeToDate,
     * Entry, Modify, Approved); an unchecked picker sends nothing; Section / Department / Requested By
     * only when not 0. Rows re-shaped exactly as the form's DataTable.
     */
    public List<Map<String, Object>> requestHistory(String mode, String from, String to, int sectionId, int departmentId, int requestedById) {
        UserAccount u = hrm.user(SCREEN_OVERTIME_REQUEST);
        String[] pair;
        switch (str(mode)) {
            case "overtime": pair = new String[] { "OverTimeFromDate", "OverTimeToDate" }; break;
            case "entry":    pair = new String[] { "EntryFromDate", "EntryToDate" }; break;
            case "modify":   pair = new String[] { "ModifyFromDate", "ModifyToDate" }; break;
            case "approved": pair = new String[] { "ApprovedFromDate", "ApprovedToDate" }; break;
            default:         pair = new String[] { "RequestFromDate", "RequestToDate" };
        }
        Map<String, Object> f = new LinkedHashMap<>();
        LocalDateTime t = toDate(to), fr = toDate(from);
        if (t != null) f.put(pair[1], ts(t));
        if (fr != null) f.put(pair[0], ts(fr));
        if (sectionId != 0) f.put("SectionId", sectionId);
        if (departmentId != 0) f.put("DepartmentId", departmentId);
        if (requestedById != 0) f.put("RequestById", requestedById);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.requestHistory(u, f)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("OverTimeRequestId", r.get("OverTimeRequestId"));
            o.put("RequestDate", iso(r.get("RequestDate")));
            o.put("OverTimeDate", iso(r.get("OverTimeDate")));
            o.put("DepartmentName", r.get("DepartmentName"));
            o.put("SectionName", r.get("SectionName"));
            o.put("IsOffDuty", r.get("IsOffDuty") == null ? "" : (toBool(r.get("IsOffDuty")) ? "True" : "False"));  // a string column on the desktop
            o.put("Reason", r.get("Reason"));
            o.put("RequestedByPerson", r.get("RequestedByPerson"));
            o.put("EntryUser", r.get("EntryUserName"));
            o.put("EntryDate", iso(r.get("CreatedOn")));
            o.put("ModifyUser", r.get("ModifyUserName"));
            o.put("ModifyDate", iso(r.get("AlteredOn")));
            o.put("ApprovedUser", r.get("ApprovedUserName"));
            o.put("ApprovedDate", iso(r.get("ApprovedOn")));
            o.put("NoOfAttachment", r.get("NoOfAttachments"));
            out.add(o);
        }
        return out;
    }

    /** "hh:mm tt" part of a grid time cell ("HH:mm" typed / loaded, or the ISO value a saved row carries); Conversion.ToDateTime(null) = 1900-01-01 00:00. */
    private static LocalTime timeOf(String v) {
        String s = str(v).trim();
        if (s.matches("\\d{1,2}:\\d{2}(:\\d{2})?")) {
            String[] p = s.split(":");
            return LocalTime.of(Integer.parseInt(p[0]), Integer.parseInt(p[1]));
        }
        LocalDateTime d = toDate(s);
        return d == null ? LocalTime.MIDNIGHT : LocalTime.of(d.getHour(), d.getMinute());
    }

    /** Conversion.ToDateTime(cell) of a deleted row: the stored value itself (a typed "HH:mm" would be today at that time). */
    private static LocalDateTime rawDateTime(String v) {
        String s = str(v).trim();
        if (s.matches("\\d{1,2}:\\d{2}(:\\d{2})?")) return LocalDate.now().atTime(timeOf(s));
        LocalDateTime d = toDate(s);
        return d == null ? LocalDate.of(1900, 1, 1).atStartOfDay() : d;
    }

    /** Convert.ToInt64(double) - rounds half to even. */
    private static long toInt64(double v) { return (long) Math.rint(v); }

    /**
     * btnsave_Click / btnUpdate_Click: FormValidation (desktop wording and order), the model filled
     * as the form fills it, the detail list = lstRemoveDetailRecord (ActionTypeId 3) + every grid row
     * (ActionTypeId 1 when Id <= 0, else 2; From/To = OverTimeDate's date + the row's "hh:mm tt"),
     * "Grid record not found" when the grid is empty, then OverTimeRequest.Save in one transaction.
     */
    public Map<String, Object> saveRequest(OvertimeRequestDto b) {
        UserAccount u = hrm.user(SCREEN_OVERTIME_REQUEST);
        long recId = Math.max(0, b.id);
        hrm.require(u, SCREEN_OVERTIME_REQUEST, recId > 0 ? "Update" : "Save");     // btnsave.Enabled / btnUpdate.Enabled
        if (b.departmentId == 0 || !owns(repo.departments(u), "DepartmentId", b.departmentId)) throw invalid("Department Required");
        if (b.sectionId == 0 || !owns(repo.sections(u), "SectionId", b.sectionId)) throw invalid("Section Required");
        if (b.reason == null || b.reason.isEmpty()) throw invalid("Reason Required");
        List<Map<String, Object>> emps = repo.employees(u);
        if (b.requestById == 0 || !owns(emps, "EmployeeId", b.requestById)) throw invalid("Requested By Required");

        Set<Long> savedDetailIds = new HashSet<>();
        if (recId > 0) {
            ownedRequest(u, recId);
            for (Map<String, Object> d : repo.requestDetails(recId)) savedDetailIds.add((long) toInt(d.get("OverTimeRequestDetailId")));
        }
        Set<Long> employeeIds = new HashSet<>();
        for (Map<String, Object> r : emps) employeeIds.add((long) toInt(r.get("EmployeeId")));
        boolean activeRead = false;

        LocalDateTime now = LocalDateTime.now();
        LocalDateTime requestDate = toDate(b.requestDate), overTimeDate = toDate(b.overTimeDate);
        if (requestDate == null) requestDate = now;
        if (overTimeDate == null) overTimeDate = now;

        OverTimeRequest h = new OverTimeRequest();
        h.OverTimeRequestId = recId;
        h.RequestDate = requestDate;
        h.OverTimeDate = overTimeDate;
        h.DepartmentId = b.departmentId;
        h.SectionId = b.sectionId;
        h.IsOffDuty = b.isOffDuty;
        h.Reason = b.reason;
        h.RequestById = b.requestById;
        h.OrganizationId = u.getOrganizationId();
        h.CompanyId = u.getCompanyId();
        h.CreatedOn = now;
        h.AlteredOn = now;
        h.CreatedById = u.getId();
        h.AlteredById = u.getId();
        h.ApprovedOn = now;
        h.ApprovedById = u.getId();
        h.IsApproved = false;

        List<OverTimeRequestDetail> list = new ArrayList<>();
        if (b.removed != null) {
            for (OvertimeRequestRowDto r : b.removed) {                          // grdDetails_ColumnButtonClick "Delete" (Id > 0)
                if (r == null) continue;
                if (r.id <= 0 || !savedDetailIds.contains(r.id)) throw invalid("Record not found.");
                OverTimeRequestDetail d = new OverTimeRequestDetail();
                d.OverTimeRequestDetailId = r.id;
                d.EmployeeId = r.employeeId;
                d.FromDateTime = rawDateTime(r.fromDateTime);
                d.ToDateTime = rawDateTime(r.toDateTime);
                d.OTHours = toInt64(r.otHours);
                d.OTRate = BigDecimal.valueOf(r.otRate);
                d.IsAllowMeal = r.isAllowMeal;
                d.RequestStatus = r.entryByLoader;
                d.ActionTypeId = 3;
                list.add(d);
            }
        }
        if (b.details == null || b.details.isEmpty()) throw invalid("Grid record not found");
        LocalDate day = overTimeDate.toLocalDate();
        for (OvertimeRequestRowDto r : b.details) {
            if (r == null) continue;
            if (r.id > 0 && !savedDetailIds.contains(r.id)) throw invalid("Record not found.");
            if (!employeeIds.contains(r.employeeId) && !activeRead) {            // loader rows come from the active-employee list
                for (Map<String, Object> e : repo.activeEmployees(u, 0)) employeeIds.add((long) toInt(e.get("EmployeeId")));
                activeRead = true;
            }
            if (!employeeIds.contains(r.employeeId)) throw invalid("Employee Required");
            OverTimeRequestDetail d = new OverTimeRequestDetail();
            d.OverTimeRequestDetailId = Math.max(0, r.id);
            d.ActionTypeId = d.OverTimeRequestDetailId <= 0 ? 1 : 2;
            d.EmployeeId = r.employeeId;
            d.FromDateTime = day.atTime(timeOf(r.fromDateTime));
            d.ToDateTime = day.atTime(timeOf(r.toDateTime));
            d.OTHours = toInt64(r.otHours);
            d.OTRate = BigDecimal.valueOf(r.otRate);
            d.IsAllowMeal = r.isAllowMeal;
            d.CreatedOn = now;
            d.AlteredOn = now;
            d.CreatedById = u.getId();
            d.AlteredById = u.getId();
            d.IsApproved = false;
            d.RequestStatus = r.entryByLoader;
            list.add(d);
        }
        long id = repo.saveRequest(h, list);
        return saved((int) id, recId > 0 ? "Data Update Successfully" : "Data Save Successfully");
    }

    /**
     * CommonServices.OverTimeSlip(PrintId) before the viewer opens: PrintId 0 or no rows from
     * OTSlipandRegister -> "No Record Found For Display". The page then prints hrm-1010.
     */
    public Map<String, Object> requestSlipCheck(long id) {
        UserAccount u = hrm.user(SCREEN_OVERTIME_REQUEST);
        if (id == 0) throw invalid("No Record Found For Display");
        if (repo.requestSlip(u, id).isEmpty()) throw invalid("No Record Found For Display");
        return map("success", true, "id", id);
    }

    // ------------------------------------------------------------------ EmpOverTimeLoadForRequest (popup of 665)

    /** DailyAttendanceRpt_Load / toolStripButton1_Click: DepartmentFill (ZeroIndex false) + EmployeeFill(cmbDepartment). */
    public Map<String, Object> loaderSetup(int departmentId) {
        UserAccount u = hrm.user(SCREEN_OVERTIME_REQUEST);
        return map("departments", idName(repo.departments(u), "DepartmentId", "DepartmentName", "name"),
                "employees", loaderEmployees(u, departmentId));
    }

    private List<Map<String, Object>> loaderEmployees(UserAccount u, int departmentId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.activeEmployees(u, departmentId)) out.add(map("EmployeeId", r.get("EmployeeId"), "EmployeeName", r.get("EmployeeName")));
        return out;
    }

    /** GridHistory(): EmployeeOverTime.ActualOverTimeLoaderForRequest - the rows as the procedure returns them (dtGrid; dtLoader is a copy of the checked ones). */
    public List<Map<String, Object>> loaderRows(String from, String to, long employeeId, long departmentId) {
        UserAccount u = hrm.user(SCREEN_OVERTIME_REQUEST);
        LocalDateTime f = toDate(from), t = toDate(to);
        if (f == null) f = LocalDateTime.now();
        if (t == null) t = LocalDateTime.now();
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.overtimeLoader(u, f, t, employeeId, departmentId)) out.add(jsonRow(r));
        return out;
    }

    // ================================================================== 666 frmEmployeeOverTime

    /** EmployeeFamilyInfo_Load: YearFill (CommonServices.GetYears), MonthFill (GetMonths - fixed on the page), DropDownBindsForHistory. */
    public Map<String, Object> employeeSetup() {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_OVERTIME);
        Map<String, Object> out = new LinkedHashMap<>();
        out.put("rights", hrm.rights(u, SCREEN_EMPLOYEE_OVERTIME));
        out.put("years", years(u));
        out.put("history", historyDropDowns(u));
        return out;
    }

    /** CommonServices.GetYears: ActiveYr.Start_Period.Year .. DateTime.Now.Year, rows { Id, Year }. */
    private List<Map<String, Object>> years(UserAccount u) {
        int start = LocalDate.now().getYear();
        int fy = hrm.financialYearId();
        List<Map<String, Object>> fys = repo.financialYears(u);
        Map<String, Object> row = null;
        for (Map<String, Object> r : fys) if (toInt(r.get("Id")) == fy) { row = r; break; }
        if (row == null && !fys.isEmpty()) row = fys.get(0);
        if (row != null) {
            LocalDateTime s = toDate(row.get("Start_Period"));
            if (s != null) start = s.getYear();
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (int y = start; y <= LocalDate.now().getYear(); y++) out.add(map("Id", y, "Year", y));
        return out;
    }

    /** BtnRefreshHistory_Click -> DropDownBindsForHistory. */
    public Map<String, Object> employeeHistoryDropDowns() { return historyDropDowns(hrm.user(SCREEN_EMPLOYEE_OVERTIME)); }

    /** DropDownBindsForHistory: one call, split by the Activity column into Month / Year / Employee { Id, name }. */
    private Map<String, Object> historyDropDowns(UserAccount u) {
        List<Map<String, Object>> m = new ArrayList<>(), y = new ArrayList<>(), e = new ArrayList<>();
        for (Map<String, Object> r : repo.employeeOverTimeDropDowns(u)) {
            String a = str(r.get("Activity"));
            Map<String, Object> o = map("Id", r.get("Id"), "name", r.get("ReferenceName"));
            if ("OverTimeMonth".equals(a)) m.add(o);
            else if ("OverTimeYear".equals(a)) y.add(o);
            else if ("Employee".equals(a)) e.add(o);
        }
        return map("months", m, "years", y, "employees", e);
    }

    /** GetEmployeesData: getOTEmployees(month, year) -> { Id, EmployeeName, Designation, Branch, Department, Section, Mobile }. */
    public List<Map<String, Object>> otEmployees(int month, int year) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_OVERTIME);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.otEmployees(u, month, year))
            out.add(map("Id", r.get("EmployeeId"), "EmployeeName", r.get("EmployeeName"), "Designation", r.get("Designation"),
                    "Branch", r.get("Branch"), "Department", r.get("Department"), "Section", r.get("Section"), "Mobile", r.get("Mobile1")));
        return out;
    }

    /**
     * GetDetailDataByEmployee: GetEmployeeAttendanceMonthWise re-shaped as the form's DataTable -
     * OTRate = Math.Round(OT Rate, 3), OTHours = ApproveOTHour, AddLess = AddLessOTHours,
     * NetOTHours = ApproveOTHour + AddLessOTHours, Amount = NetOTHours * OTRate. (The page leaves out
     * rows already in its detailList, as the form does.)
     */
    public List<Map<String, Object>> employeeDetail(long employeeId, int month, int year) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_OVERTIME);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.attendanceMonthWise(u, employeeId, month, year)) out.add(detailRow(r));
        return out;
    }

    private static Map<String, Object> detailRow(Map<String, Object> r) {
        double rate = round3(toDouble(r.get("OT Rate")));
        double approve = toDouble(r.get("ApproveOTHour")), addLess = toDouble(r.get("AddLessOTHours"));
        Map<String, Object> o = new LinkedHashMap<>();
        o.put("EmpOvertimeId", r.get("EmployeeOverTimeId"));
        o.put("OvertimeDetailId", r.get("OverTimeRequestDetailId"));
        o.put("EmpAttendanceId", r.get("EmployeeAttendanceId"));
        o.put("EmployeeId", r.get("EmployeeId"));
        o.put("OvertimeMonth", r.get("OverTimeMonth"));
        o.put("OvertimeYear", r.get("OverTimeYear"));
        o.put("Employee", r.get("Employee"));
        o.put("Date", toInt(r.get("Date")));
        o.put("Day", r.get("Day"));
        o.put("FromTime", iso(r.get("FromDateTime")));
        o.put("ToTime", iso(r.get("ToDateTime")));
        o.put("Timing", r.get("Timing"));
        o.put("WorkingHour", dbl(r.get("Working Hour")));
        o.put("ReqOTHours", r.get("ReqOTHours"));
        o.put("ActualOTHour", r.get("ActualOTHour"));
        o.put("ShortHours", dbl(r.get("ShortHour")));
        o.put("OTRate", rate);
        o.put("OTHours", r.get("ApproveOTHour"));
        o.put("AddLess", r.get("AddLessOTHours"));
        o.put("NetOTHours", approve + addLess);
        o.put("Amount", (approve + addLess) * rate);
        return o;
    }

    /**
     * btnsave_Click -> Insert(): "No Record Found" when detailList is empty; every entry as
     * addDataToDetailList builds it (ActionTypeId 1, user / now stamps, IsApproved true, OTHours =
     * ActualOTHour, ShortHours, OTRate, NetOTHours = the OTHours cell, AddLessOTHours = AddLess,
     * ApprovedHours = NetOTHours cell = OTHours + AddLess), then EmployeeOverTime.Save. The keys and
     * hour / rate cells are re-read from the same procedure for this company, so only AddLess comes from the page.
     */
    public Map<String, Object> saveEmployeeOverTime(OvertimeEmployeeDto b) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_OVERTIME);
        if (b == null || b.details == null || b.details.isEmpty()) throw invalid("No Record Found");
        LocalDateTime now = LocalDateTime.now();
        Map<String, List<Map<String, Object>>> cache = new HashMap<>();
        List<EmployeeOverTime> list = new ArrayList<>();
        Set<Long> seen = new HashSet<>();
        for (OvertimeEmployeeRowDto r : b.details) {
            if (r == null || !seen.add(r.overTimeRequestDetailId)) continue;     // detailList.Any(OverTimeRequestDetailId)
            String key = r.employeeId + "/" + r.month + "/" + r.year;
            List<Map<String, Object>> rows = cache.computeIfAbsent(key, k -> repo.attendanceMonthWise(u, r.employeeId, r.month, r.year));
            Map<String, Object> src = null;
            for (Map<String, Object> x : rows)
                if (toInt(x.get("OverTimeRequestDetailId")) == r.overTimeRequestDetailId && toInt(x.get("EmployeeAttendanceId")) == r.employeeAttendanceId) { src = x; break; }
            if (src == null) throw invalid("Record not found.");
            Map<String, Object> row = detailRow(src);
            EmployeeOverTime m = new EmployeeOverTime();
            m.ActionTypeId = 1;
            m.AlteredById = u.getId();
            m.CreatedById = u.getId();
            m.ApprovedById = u.getId();
            m.CreatedOn = now;
            m.AlteredOn = now;
            m.ApprovedOn = now;
            m.OrganizationId = u.getOrganizationId();
            m.CompanyId = u.getCompanyId();
            m.EmployeeAttendanceId = r.employeeAttendanceId;
            m.OverTimeRequestDetailId = r.overTimeRequestDetailId;
            m.EmployeeId = toInt(src.get("EmployeeId"));
            m.OverTimeMonth = toInt(src.get("OverTimeMonth"));
            m.OverTimeYear = toInt(src.get("OverTimeYear"));
            m.OTHours = toDec(row.get("ActualOTHour"));
            m.ShortHours = toDec(row.get("ShortHours"));
            m.OTRate = toDec(row.get("OTRate"));
            m.NetOTHours = toDec(row.get("OTHours"));
            m.AddLessOTHours = BigDecimal.valueOf(r.addLess);
            m.ApprovedHours = BigDecimal.valueOf(toDouble(row.get("OTHours")) + r.addLess);
            m.IsApproved = true;
            list.add(m);
        }
        repo.saveEmployeeOverTime(list);
        return saved(0, "Saved Successfully");      // RecId is always 0 here (btnupdate is never shown)
    }

    /**
     * btnShowHistory_Click: "Month Required" / "Year Required", then FormHistory re-shaped as the
     * form's DataTable (Id, EmployeeName, OverTimeDate, InTime, OutTime, TimeDuration, ActualOTHours,
     * OTHours = NetOTHours, AddLess, ApprovedHours, OTRate (round 3), ApprovedAmount, EntryDate, EntryUser).
     */
    public List<Map<String, Object>> employeeHistory(int month, int year, long employeeId) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_OVERTIME);
        if (month <= 0) throw invalid("Month Required");
        if (year <= 0) throw invalid("Year Required");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.employeeOverTimeHistory(u, month, year, employeeId)) {
            Map<String, Object> o = new LinkedHashMap<>();
            o.put("Id", r.get("EmployeeOverTimeId"));
            o.put("EmployeeName", r.get("EmployeeName"));
            o.put("OverTimeDate", iso(r.get("OverTimeDate")));
            o.put("InTime", r.get("InTime"));
            o.put("OutTime", r.get("OutTime"));
            o.put("TimeDuration", r.get("TimeDuration"));
            o.put("ActualOTHours", r.get("OTHours"));
            o.put("OTHours", r.get("NetOTHours"));
            o.put("AddLess", r.get("AddLessOTHours"));
            o.put("ApprovedHours", r.get("ApprovedHours"));
            o.put("OTRate", round3(toDouble(r.get("OTRate"))));
            o.put("ApprovedAmount", r.get("ApprovedAmount"));
            o.put("EntryDate", iso(r.get("CreatedOn")));
            o.put("EntryUser", r.get("CreatedBy"));
            out.add(o);
        }
        return out;
    }

    /**
     * btnPrint1011_Click before the viewer: "Month Required" / "Year Required", then
     * EmployeeOverTime_SlipAndRegister must return rows, else "No Record Found". The form passes
     * Conversion.ToInt64(cmbEmployeeHistory) - the control itself, always 0 - so @EmployeeId is never sent.
     */
    public Map<String, Object> employeeSlipCheck(int month, int year) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_OVERTIME);
        if (month <= 0) throw invalid("Month Required");
        if (year <= 0) throw invalid("Year Required");
        if (repo.employeeOverTimeSlip(u, month, year, 0).isEmpty()) throw invalid("No Record Found");
        return map("success", true);
    }
}
