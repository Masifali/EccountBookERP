package com.mst.services.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.dto.LeaveCplRequestSaveDto;
import com.mst.models.hrm.dto.LeaveCplRowDto;
import com.mst.models.hrm.dto.LeaveCplSaveDto;
import com.mst.models.hrm.dto.LeaveDateRowDto;
import com.mst.models.hrm.dto.LeaveOpeningSaveDto;
import com.mst.models.hrm.dto.LeaveRequestSaveDto;
import com.mst.models.hrm.leave.CPLAttendance;
import com.mst.models.hrm.leave.CPLEmployeeLeaveDetail;
import com.mst.models.hrm.leave.EmployeeLeaveOpening;
import com.mst.models.hrm.leave.EmployeeLeaveQuota;
import com.mst.models.hrm.leave.GenEmployeeLeave;
import com.mst.models.hrm.leave.GenEmployeeLeaveDetail;
import com.mst.repositories.hrm.HrmLeaveRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * BLL of the HRM "Leave Management" screens. Each method names the desktop form method it reproduces;
 * validation wording / order and the MessageBox texts are the form's.
 *
 *   660     frmEmployeeLeaveOpening.cs      /hrm/leave-opening
 *   661     frmEmployeeLeaveRequest.cs      /hrm/employee-leave
 *   662/463 frmEmployeeCPLLeaveOpening.cs   /hrm/cpl-leave-opening (either screen row grants View)
 *   461     frmEmployeeCPLAttendance.cs     /hrm/cpl-attendance
 *   462     frmEmployeeCPLRequest.cs        /hrm/cpl-request
 *
 * Every id the page sends that picks an existing record (employee, leave quota, leave type, leave, CPL row,
 * detail row) is checked against the company's own list from the same procedure the form fills it from.
 */
@Service
public class HrmLeaveService {

    public static final int SCREEN_LEAVE_OPENING = 660;
    public static final int SCREEN_EMPLOYEE_LEAVE = 661;
    public static final int SCREEN_CPL_LEAVE_OPENING = 662;
    public static final int SCREEN_CPL_LEAVE_OPENING_EMP = 463;
    public static final int SCREEN_CPL_ATTENDANCE = 461;
    public static final int SCREEN_CPL_REQUEST = 462;

    /** genProfile 13 = "CPL" - LeaveTypeFill / ProfileTypeFill skip it; frmEmployeeCPLRequest saves with it. */
    static final int CPL_LEAVE_TYPE = 13;
    private static final LocalDateTime MIN_DATE = LocalDateTime.of(1900, 1, 1, 0, 0);   // Conversion.ToDateTime fallback

    @Autowired private HrmLeaveRepository repo;
    @Autowired private HrmSupport hrm;

    // ================================================================== shared lookups

    /** genEmployee.Getall -> DataTable { Id = EmployeeId, Name = EmployeeName } (EmployeeNameFill of 660 / 462). */
    private List<Map<String, Object>> employeeCombo(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.employees(u)) out.add(map("Id", r.get("EmployeeId"), "Name", r.get("EmployeeName")));
        return out;
    }

    /** BindDDLNew(GetAllActiveEmployee, "EmployeeId", "EmployeeName") (661, 662/463, 461) - every row, as BindDDLNew copies them. */
    private List<Map<String, Object>> activeEmployeeCombo(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.activeEmployees(u)) out.add(map("EmployeeId", r.get("EmployeeId"), "EmployeeName", r.get("EmployeeName")));
        return out;
    }

    /** LeaveTypeFill / ProfileTypeFill: ReadByProfileTypeId 3 without ProfileId 13. */
    private List<Map<String, Object>> leaveTypeCombo(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.leaveTypes(u)) {
            if (toInt(r.get("ProfileId")) != CPL_LEAVE_TYPE) out.add(map("ProfileId", r.get("ProfileId"), "ProfileName", r.get("ProfileName")));
        }
        return out;
    }

    /** LeaveQuotaBind: GetDataForLeaveQuotaDropDown with FromDate = ActiveYr.Start_Period. */
    private List<Map<String, Object>> leaveQuotaCombo(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.leaveQuotas(u, activeYearStart(u))) {
            out.add(map("LeaveQuotaId", r.get("LeaveQuotaId"), "LeaveQuotaDesription", r.get("LeaveQuotaDesription")));
        }
        return out;
    }

    /** clsGlobalVariables.ActiveYr.Start_Period of the signed-in financial year (Conversion.ToDateTime -> 1900-01-01 when missing). */
    private LocalDateTime activeYearStart(UserAccount u) {
        try {
            int yearId = hrm.financialYearId();
            List<Map<String, Object>> years = repo.activeYears(u);
            Map<String, Object> row = null;
            for (Map<String, Object> y : years) if (toInt(y.get("Id")) == yearId) { row = y; break; }
            if (row == null && !years.isEmpty()) row = years.get(0);
            LocalDateTime d = row == null ? null : toDate(row.get("Start_Period"));
            return d == null ? MIN_DATE : d;
        } catch (RuntimeException e) {
            return MIN_DATE;
        }
    }

    /**
     * GetEmployeeHistory(EmployeeId): the FIRST GetAllActiveEmployee row of the employee -> txtEmployeeNo and the
     * four read-only combos (Designation, Department, Section, Location). found = false when the list has no row
     * for it (the desktop then leaves the combos empty).
     */
    private Map<String, Object> employeeHistory(UserAccount u, long employeeId) {
        if (employeeId <= 0) return map("found", false);
        for (Map<String, Object> r : repo.activeEmployees(u)) {
            if (toInt(r.get("EmployeeId")) == employeeId) {
                return map("found", true, "EmployeeNo", str(r.get("EmployeeNo")),
                        "DesignationId", r.get("DesignationId"), "DesignationName", r.get("DesignationName"),
                        "DepartmentId", r.get("DepartmentId"), "DepartmentName", r.get("DepartmentName"),
                        "SectionId", r.get("SectionId"), "SectionName", r.get("SectionName"),
                        "LocationId", r.get("LocationId"), "LocationName", r.get("LocationName"));
            }
        }
        return map("found", false);
    }

    private static Set<Long> ids(List<Map<String, Object>> rows, String key) {
        Set<Long> s = new HashSet<>();
        for (Map<String, Object> r : rows) s.add((long) toInt(r.get(key)));
        return s;
    }

    private static String day(Object v) { LocalDateTime d = toDate(v); return d == null ? null : d.toLocalDate().toString(); }

    private static String dateTime(Object v) { LocalDateTime d = toDate(v); return d == null ? null : d.toString(); }

    /** Convert.ToDateTime(DutyDate + " " + time.ToString("hh:mm tt")): the date part of day, the hour:minute of t. */
    private static LocalDateTime atTime(LocalDateTime day, String t) {
        LocalDateTime base = day == null ? MIN_DATE : day.toLocalDate().atStartOfDay();
        String s = str(t).trim();
        if (s.matches("\\d{1,2}:\\d{2}(:\\d{2})?")) {
            String[] p = s.split(":");
            return base.with(LocalTime.of(Integer.parseInt(p[0]), Integer.parseInt(p[1])));
        }
        LocalDateTime full = toDate(s);
        return full == null ? base : base.with(LocalTime.of(full.getHour(), full.getMinute()));
    }

    private static List<LeaveDateRowDto> nn(List<LeaveDateRowDto> l) { return l == null ? new ArrayList<>() : l; }

    private static List<LeaveCplRowDto> nnc(List<LeaveCplRowDto> l) { return l == null ? new ArrayList<>() : l; }

    // ================================================================== 660 Leave Opening (frmEmployeeLeaveOpening.cs)

    /** frmItemPricingSchedule_Load: EmployeeNameFill, LeaveQuotaBind, ProfileTypeFill (the History tab is removed). */
    public Map<String, Object> openingSetup() {
        UserAccount u = hrm.user(SCREEN_LEAVE_OPENING);
        Map<String, Object> m = openingCombos(u);
        m.put("rights", hrm.rights(u, SCREEN_LEAVE_OPENING));
        return m;
    }

    /** btnRefresh_Click: EmployeeNameFill, LeaveQuotaBind, ProfileTypeFill again. */
    public Map<String, Object> openingCombos() { return openingCombos(hrm.user(SCREEN_LEAVE_OPENING)); }

    private Map<String, Object> openingCombos(UserAccount u) {
        return map("employees", employeeCombo(u), "quotas", leaveQuotaCombo(u), "leaveTypes", leaveTypeCombo(u));
    }

    /** btnLoad_Click -> GridBindByLeaveProfileTypeId: GetDataForLeaveOpening with the three combos (0 when empty). */
    public List<Map<String, Object>> openingLoad(int leaveQuotaId, int leaveTypeProfileId, long employeeId) {
        UserAccount u = hrm.user(SCREEN_LEAVE_OPENING);
        if (leaveQuotaId != 0 && !owns(leaveQuotaCombo(u), "LeaveQuotaId", leaveQuotaId)) throw invalid("Record not found.");
        if (leaveTypeProfileId != 0 && !owns(leaveTypeCombo(u), "ProfileId", leaveTypeProfileId)) throw invalid("Record not found.");
        if (employeeId != 0 && !owns(repo.employees(u), "EmployeeId", (int) employeeId)) throw invalid("Record not found.");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.leaveOpeningRows(u, leaveQuotaId, leaveTypeProfileId, employeeId)) {
            out.add(map("EmployeeId", r.get("EmployeeId"), "EmployeeNo", r.get("EmployeeNo"), "EmployeeName", r.get("EmployeeName"),
                    "LeaveQuotaDetailId", r.get("LeaveQuotaDetailId"), "JoiningDate", day(r.get("JoiningDate")),
                    "DepartmentName", r.get("DepartmentName"), "DesignationName", r.get("DesignationName"),
                    "QuotaValue", r.get("QuotaValue"), "QuotaFromDate", day(r.get("QuotaFromDate")), "QuotaToDate", day(r.get("QuotaToDate")),
                    "TotalMonth", r.get("TotalMonth"), "Assign", r.get("Assign"), "Availed", r.get("Availed"), "Balance", r.get("Balance")));
        }
        return out;
    }

    /**
     * Insert(): FormValiadation ("LeaveQuota Field Required", "LeaveType Field Required"), the header as the form
     * fills it and one EmployeeLeaveQuota per grid row (Assign, Availed, ActionTypeId 1, UserLogId 1), then
     * EmployeeLeaveOpening.Save. Assign is re-read from GetDataForLeaveOpening (the grid column is read-only there).
     */
    public Map<String, Object> openingSave(LeaveOpeningSaveDto b) {
        UserAccount u = hrm.user(SCREEN_LEAVE_OPENING);
        if (b.leaveQuotaId == 0) throw invalid("LeaveQuota Field Required");
        if (b.leaveTypeProfileId == 0) throw invalid("LeaveType Field Required");
        if (!owns(leaveQuotaCombo(u), "LeaveQuotaId", b.leaveQuotaId)) throw invalid("Record not found.");
        if (!owns(leaveTypeCombo(u), "ProfileId", b.leaveTypeProfileId)) throw invalid("Record not found.");
        if (b.id != 0) throw invalid("Record not found.");                 // RecId is never set to a record on this form

        Map<String, BigDecimal> assign = new HashMap<>();
        for (Map<String, Object> r : repo.leaveOpeningRows(u, b.leaveQuotaId, b.leaveTypeProfileId, 0L)) {
            assign.put(toInt(r.get("EmployeeId")) + ":" + toInt(r.get("LeaveQuotaDetailId")), toDec(r.get("Assign")));
        }
        LocalDateTime now = LocalDateTime.now();
        EmployeeLeaveOpening m = new EmployeeLeaveOpening();
        m.EmployeeLeaveOpeningId = 0;
        m.LeaveQuotaId = b.leaveQuotaId;
        m.LeaveTypeProfileId = b.leaveTypeProfileId;
        m.CreatedById = u.getId();
        m.CreatedOn = now;
        m.AlteredById = u.getId();
        m.AlteredOn = now;
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        if (b.details != null) {
            for (LeaveOpeningSaveDto.Row r : b.details) {
                BigDecimal a = assign.get(r.employeeId + ":" + r.leaveQuotaDetailId);
                if (a == null) throw invalid("Record not found.");
                EmployeeLeaveQuota q = new EmployeeLeaveQuota();
                q.EmployeeId = r.employeeId;
                q.LeaveQuotaDetailId = r.leaveQuotaDetailId;
                q.Assigned = a;
                q.Availed = toDec(r.availed);
                q.CreatedById = u.getId();
                q.AlteredById = u.getId();
                q.CreatedOn = now;
                q.AlteredOn = now;
                q.ActionTypeId = 1;
                q.UserLogId = 1L;
                m.EmployeeLeaveQuotasList.add(q);
            }
        }
        long id = repo.saveLeaveOpening(m);
        return saved((int) id, "Saved Successfully");
    }

    // ================================================================== 661 Employee Leave (frmEmployeeLeaveRequest.cs)

    /** frmItemPricingSchedule_Load: EmployeeNameFill (active), LeaveTypeFill, LeaveQuotaBind. */
    public Map<String, Object> leaveSetup() {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_LEAVE);
        return map("rights", hrm.rights(u, SCREEN_EMPLOYEE_LEAVE), "employees", activeEmployeeCombo(u),
                "leaveTypes", leaveTypeCombo(u), "quotas", leaveQuotaCombo(u));
    }

    /** cmbEmployee Leave -> GetEmployeeHistory(cmbEmployee.Value). */
    public Map<String, Object> leaveEmployee(long employeeId) {
        return employeeHistory(hrm.user(SCREEN_EMPLOYEE_LEAVE), employeeId);
    }

    /**
     * LeaveBalance(): EmployeeLeaveOpening.GetLeavesBalanceByEmployeeId with the three combos (0 when empty) ->
     * txtLeaveBalance.Text = Balance.ToString() (0 when no row). The procedure filters on the session company.
     */
    public Map<String, Object> leaveBalance(long employeeId, int leaveQuotaId, int leaveTypeProfileId) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_LEAVE);
        List<Map<String, Object>> rows = repo.leaveBalance(u, employeeId, leaveQuotaId, leaveTypeProfileId);
        BigDecimal bal = BigDecimal.ZERO;
        if (!rows.isEmpty()) bal = toDec(rows.get(0).values().iterator().next());
        return map("balance", bal.toPlainString());
    }

    /** FormHistory(): genEmployeeLeave.ReadAll -> EmployeeLeaveId, EmployeeName, LeaveType, NoOfDays, Reason, Remarks. */
    public List<Map<String, Object>> leaveHistory() {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_LEAVE);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.leaves(u, 0)) {
            out.add(map("EmployeeLeaveId", r.get("EmployeeLeaveId"), "EmployeeName", r.get("EmployeeName"), "LeaveType", r.get("LeaveType"),
                    "NoOfDays", toInt(r.get("NoOfDays")), "Reason", r.get("Reason"), "Remarks", r.get("Remarks")));
        }
        return out;
    }

    /** DataGridHistory_SelectionChanged: genEmployeeLeave.GetByID -> grdDetailHistory { Id, LeaveDate }. */
    public List<Map<String, Object>> leaveHistoryDetail(long id) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_LEAVE);
        if (!owns(repo.leaves(u, 0), "EmployeeLeaveId", (int) id)) throw invalid("Record not found.");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> d : repo.leaveDetails(id)) out.add(map("Id", d.get("EmployeeLeaveDetailId"), "LeaveDate", day(d.get("LeaveDate"))));
        return out;
    }

    /** DataGridHistory_ColumnButtonClick "Edit" -> GetByItemId(EmployeeLeaveId). */
    public Map<String, Object> leaveById(long id) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_LEAVE);
        if (!owns(repo.leaves(u, 0), "EmployeeLeaveId", (int) id)) throw invalid("Record not found.");
        return leaveRecord(id);
    }

    private Map<String, Object> leaveRecord(long id) {
        Map<String, Object> h = one(repo.leave(id));
        List<Map<String, Object>> details = new ArrayList<>();
        for (Map<String, Object> d : repo.leaveDetails(id)) {
            details.add(map("EmployeeLeaveDetailId", d.get("EmployeeLeaveDetailId"), "LeaveDate", day(d.get("LeaveDate"))));
        }
        List<Map<String, Object>> cpl = new ArrayList<>();
        for (Map<String, Object> c : repo.leaveCplDetails(id)) {
            cpl.add(map("CPLEmployeeLeaveId", c.get("CPLEmployeeLeaveId"), "CPLAttendanceId", c.get("CPLAttendanceId"),
                    "EmployeeLeaveId", c.get("EmployeeLeaveId"), "ActionTypeId", c.get("ActionTypeId"), "CPLDate", day(c.get("CPLDate")),
                    "WeekDayName", c.get("WeekDayName"), "CPLQuota", c.get("CPLQuota"), "Active", toBool(c.get("Active"))));
        }
        return map("EmployeeLeaveId", h.get("EmployeeLeaveId"), "EmployeeId", h.get("EmployeeId"), "LeaveTypeProfileId", h.get("LeaveTypeProfileId"),
                "EmployeeLeaveQuotaId", h.get("EmployeeLeaveQuotaId"), "FromDate", day(h.get("FromDate")), "ToDate", day(h.get("ToDate")),
                "Reason", str(h.get("Reason")), "Remarks", str(h.get("Remarks")), "details", details, "cpl", cpl);
    }

    /**
     * Insert(): FormValiadation (Employee / LeaveType / LeaveQuota / Reason, the desktop's order and wording), then
     * "Please Add Detail Record" when the grid is empty, the header as the form fills it (NoOfDays = grid rows,
     * DocMovementId 1, FromDate = ToDate = now, ApprovedBy = user) and genEmployeeLeave.Save with the grid rows
     * (ActionTypeId 1 new / 2 existing) plus Deletelst (ActionTypeId 3). The procedures check the balance.
     */
    public Map<String, Object> leaveSave(LeaveRequestSaveDto b) {
        UserAccount u = hrm.user(SCREEN_EMPLOYEE_LEAVE);
        if (b.employeeId == 0) throw invalid("Employee Field Required");
        if (b.leaveTypeProfileId == 0) throw invalid("LeaveType Field Required");
        if (b.leaveQuotaId == 0) throw invalid("LeaveQuota Field Required");
        if (str(b.reason).isEmpty()) throw invalid("Reason Field Required");
        List<LeaveDateRowDto> details = nn(b.details), deletes = nn(b.deletes);
        if (details.isEmpty()) throw invalid("Please Add Detail Record");

        Map<String, Object> emp = employeeHistory(u, b.employeeId);
        if (!toBool(emp.get("found"))) throw invalid("Employee Field Required");
        if (!owns(leaveTypeCombo(u), "ProfileId", b.leaveTypeProfileId)) throw invalid("Record not found.");
        if (!owns(leaveQuotaCombo(u), "LeaveQuotaId", b.leaveQuotaId)) throw invalid("Record not found.");
        int recId = Math.max(0, b.id);
        if (recId > 0 && !owns(repo.leaves(u, 0), "EmployeeLeaveId", recId)) throw invalid("Record not found.");
        Set<Long> own = recId > 0 ? ids(repo.leaveDetails(recId), "EmployeeLeaveDetailId") : new HashSet<>();

        LocalDateTime now = LocalDateTime.now();
        GenEmployeeLeave m = new GenEmployeeLeave();
        m.ActionTypeId = recId > 0 ? 2 : 1;
        m.EmployeeLeaveId = recId;
        m.LocationId = toInt(emp.get("LocationId"));
        m.DocMovementId = 1;
        m.EmployeeId = b.employeeId;
        m.LeaveTypeProfileId = b.leaveTypeProfileId;
        m.EmployeeLeaveQuotaId = b.leaveQuotaId;
        m.FromDate = now;
        m.ToDate = now;
        m.NoOfDays = BigDecimal.valueOf(details.size());
        m.Reason = str(b.reason);
        m.Remarks = str(b.remarks);
        m.ApprovedById = u.getId();
        m.ApprovedOn = now;
        m.CreatedById = u.getId();
        m.CreatedOn = now;
        m.AlteredById = u.getId();
        m.AlteredOn = now;
        m.UserLogId = u.getId();
        m.CompanyId = u.getCompanyId();
        m.OrganizationId = u.getOrganizationId();
        addLeaveDates(m, u, details, deletes, own, now);
        long id = repo.saveLeave(m);
        return saved((int) id, recId == 0 && id > 0 ? "Save Successfully" : "Update Successfully");
    }

    /** The datagrid rows (ActionTypeId 2 when EmployeeLeaveDetailId > 0 else 1) and then Deletelst (ActionTypeId 3). */
    private void addLeaveDates(GenEmployeeLeave m, UserAccount u, List<LeaveDateRowDto> details, List<LeaveDateRowDto> deletes,
                               Set<Long> own, LocalDateTime now) {
        for (LeaveDateRowDto r : details) {
            if (r.employeeLeaveDetailId > 0 && !own.contains(r.employeeLeaveDetailId)) throw invalid("Record not found.");
            GenEmployeeLeaveDetail d = leaveDate(u, r, now);
            d.ActionTypeId = r.employeeLeaveDetailId > 0 ? 2 : 1;
            m.genEmployeeLeaveDetailList.add(d);
        }
        for (LeaveDateRowDto r : deletes) {
            if (r.employeeLeaveDetailId <= 0) continue;                    // only rows with an id go to Deletelst
            if (!own.contains(r.employeeLeaveDetailId)) throw invalid("Record not found.");
            GenEmployeeLeaveDetail d = leaveDate(u, r, now);
            d.ActionTypeId = 3;
            m.genEmployeeLeaveDetailList.add(d);
        }
    }

    private static GenEmployeeLeaveDetail leaveDate(UserAccount u, LeaveDateRowDto r, LocalDateTime now) {
        GenEmployeeLeaveDetail d = new GenEmployeeLeaveDetail();
        d.EmployeeLeaveDetailId = r.employeeLeaveDetailId;
        LocalDateTime ld = toDay(r.leaveDate);
        d.LeaveDate = ld == null ? MIN_DATE : ld;
        d.IsApproved = false;
        d.CreatedById = u.getId();
        d.CreatedOn = now;
        d.AlteredById = u.getId();
        d.AlteredOn = now;
        d.UserLogId = u.getId();
        return d;
    }

    // ================================================================== 662 / 463 CPL Leave Opening (frmEmployeeCPLLeaveOpening.cs)

    /** Either screen row (662 in HRM, 463 in the older module 47) opens the same form. */
    private int cplOpeningScreen() {
        UserAccount u = hrm.user();
        if (hrm.can(u, SCREEN_CPL_LEAVE_OPENING, "View")) return SCREEN_CPL_LEAVE_OPENING;
        hrm.require(u, SCREEN_CPL_LEAVE_OPENING_EMP, "View");
        return SCREEN_CPL_LEAVE_OPENING_EMP;
    }

    /** frmItemPricingSchedule_Load: EmployeeNameFill (GetAllEmployeesActive, BindDDLNew). */
    public Map<String, Object> cplOpeningSetup() {
        int screen = cplOpeningScreen();
        UserAccount u = hrm.user(screen);
        return map("rights", hrm.rights(u, screen), "screen", screen, "employees", activeEmployeeCombo(u));
    }

    /** btnRefresh_Click: EmployeeNameFill. */
    public List<Map<String, Object>> cplOpeningEmployees() { return activeEmployeeCombo(hrm.user(cplOpeningScreen())); }

    /** GetHistory(): CPLAttendance.ReadAll ReqTypeId 1. */
    public List<Map<String, Object>> cplOpeningHistory() { return cplHistory(hrm.user(cplOpeningScreen()), 1); }

    private List<Map<String, Object>> cplHistory(UserAccount u, int reqTypeId) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.cplList(u, reqTypeId)) {
            out.add(map("CPLAttendanceId", r.get("CPLAttendanceId"), "EmployeeId", r.get("EmployeeId"), "EmployeeNo", r.get("EmployeeNo"),
                    "EmployeeName", r.get("EmployeeName"), "CPLDate", day(r.get("CPLDate")), "InDateTime", str(r.get("InDateTime")),
                    "OutDateTime", str(r.get("OutDateTime")), "CPLQuota", r.get("CPLQuota")));
        }
        return out;
    }

    /** DataGridHistory_DoubleClick -> ReadById(CPLAttendanceId): the record as one grid row. */
    public Map<String, Object> cplOpeningById(long id) {
        UserAccount u = hrm.user(cplOpeningScreen());
        return cplRecord(u, 1, id);
    }

    private Map<String, Object> cplRecord(UserAccount u, int reqTypeId, long id) {
        if (!owns(repo.cplList(u, reqTypeId), "CPLAttendanceId", (int) id)) throw invalid("Record not found.");
        Map<String, Object> r = one(repo.cpl(id));
        return map("CPLAttendanceId", r.get("CPLAttendanceId"), "EmployeeAttendanceId", toInt(r.get("EmployeeAttendanceId")),
                "EmployeeId", r.get("EmployeeId"), "EmployeeName", r.get("EmployeeName"), "CPLDate", day(r.get("CPLDate")),
                "InTime", dateTime(r.get("InTime")), "OutTime", dateTime(r.get("OutTime")), "CPLQuota", r.get("CPLQuota"));
    }

    /**
     * Insert(): every grid row as a CPLAttendance (InTime / OutTime = CPLDate + the picker's hh:mm, ReqTypeId 1,
     * EmployeeAttendanceId 0, ActionTypeId 1 new / 2 existing) and Deletelst (ActionTypeId 3, ReqTypeId not set = 0),
     * then CPLAttendance.Save -> "Saved Successfully". An empty grid saves nothing and still says so, as the desktop.
     */
    public Map<String, Object> cplOpeningSave(LeaveCplSaveDto b) {
        UserAccount u = hrm.user(cplOpeningScreen());
        List<LeaveCplRowDto> rows = nnc(b.rows), deletes = nnc(b.deletes);
        if (rows.isEmpty() && deletes.isEmpty()) return saved(0, "Saved Successfully");
        Set<Long> employees = ids(repo.activeEmployees(u), "EmployeeId");
        Map<Long, Long> own = new HashMap<>();
        for (Map<String, Object> r : repo.cplList(u, 1)) own.put((long) toInt(r.get("CPLAttendanceId")), (long) toInt(r.get("EmployeeId")));
        LocalDateTime now = LocalDateTime.now();
        List<CPLAttendance> list = new ArrayList<>();
        for (LeaveCplRowDto r : rows) {
            if (!employees.contains(r.employeeId)) throw invalid("EmployeeName Field Required");
            if (r.id > 0 && !own.containsKey(r.id)) throw invalid("Record not found.");
            CPLAttendance c = cplOpeningRow(u, r, own, now);
            c.EmployeeAttendanceId = 0L;
            c.ActionTypeId = r.id == 0 ? 1 : 2;
            c.ReqTypeId = 1;
            list.add(c);
        }
        for (LeaveCplRowDto r : deletes) {
            if (r.id <= 0) continue;
            if (!own.containsKey(r.id)) throw invalid("Record not found.");
            CPLAttendance c = cplOpeningRow(u, r, own, now);
            if (!employees.contains(r.employeeId)) c.EmployeeId = own.get(r.id);   // the record's own employee
            c.ActionTypeId = 3;
            list.add(c);
        }
        repo.saveCpl(list);
        return saved(0, "Saved Successfully");
    }

    private CPLAttendance cplOpeningRow(UserAccount u, LeaveCplRowDto r, Map<Long, Long> own, LocalDateTime now) {
        CPLAttendance c = new CPLAttendance();
        c.CPLAttendanceId = r.id;
        c.EmployeeId = r.id > 0 && r.employeeId == 0 ? own.get(r.id) : r.employeeId;
        LocalDateTime d = toDay(r.cplDate);
        c.CPLDate = d == null ? MIN_DATE : d;
        c.CPLQuota = toDec(r.cplQuota);
        c.InTime = atTime(c.CPLDate, r.inTime);
        c.OutTime = atTime(c.CPLDate, r.outTime);
        c.CreatedById = u.getId();
        c.AlteredById = u.getId();
        c.CreatedOn = now;
        c.AlteredOn = now;
        c.OrganizationId = u.getOrganizationId();
        c.CompanyId = u.getCompanyId();
        return c;
    }

    // ================================================================== 461 CPL Attendance (frmEmployeeCPLAttendance.cs)

    /** frmItemPricingSchedule_Load: EmployeeNameFill (YearFill / MonthFill are built in code - on the page). */
    public Map<String, Object> cplAttendanceSetup() {
        UserAccount u = hrm.user(SCREEN_CPL_ATTENDANCE);
        return map("rights", hrm.rights(u, SCREEN_CPL_ATTENDANCE), "employees", activeEmployeeCombo(u));
    }

    /** btnLoad_Click ("Show"): GetEmployeeAttendanceRecordForCPLAttendance(employee, month, year). */
    public List<Map<String, Object>> cplAttendanceLoad(long employeeId, int month, int year) {
        UserAccount u = hrm.user(SCREEN_CPL_ATTENDANCE);
        if (!ids(repo.activeEmployees(u), "EmployeeId").contains(employeeId)) throw invalid("EmployeeName Field Required");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.cplAttendanceRecords(u, employeeId, month, year)) {
            out.add(map("CPLAttendanceId", toInt(r.get("CPLAttendanceId")), "EmployeeId", r.get("EmployeeId"),
                    "EmployeeAttendanceId", r.get("EmployeeAttendanceId"), "DutyDate", day(r.get("DutyDate")),
                    "InDateTime", dateTime(r.get("InDateTime")), "OutDateTime", dateTime(r.get("OutDateTime")), "CPLQuota", r.get("CPLQuota")));
        }
        return out;
    }

    /** GetHistory(): CPLAttendance.ReadAll ReqTypeId 2. */
    public List<Map<String, Object>> cplAttendanceHistory() { return cplHistory(hrm.user(SCREEN_CPL_ATTENDANCE), 2); }

    /** DataGridHistory_DoubleClick -> ReadById. */
    public Map<String, Object> cplAttendanceById(long id) { return cplRecord(hrm.user(SCREEN_CPL_ATTENDANCE), 2, id); }

    /**
     * Insert(): every grid row as a CPLAttendance (EmployeeAttendanceId, CPLAttendanceId, EmployeeId, CPLDate = DutyDate,
     * CPLQuota, InTime = InDateTime, OutTime = OutDateTime, ReqTypeId 2, ActionTypeId 1 / 2) plus Deletelst, then
     * CPLAttendance.Save -> "Saved Successfully".
     */
    public Map<String, Object> cplAttendanceSave(LeaveCplSaveDto b) {
        UserAccount u = hrm.user(SCREEN_CPL_ATTENDANCE);
        List<LeaveCplRowDto> rows = nnc(b.rows), deletes = nnc(b.deletes);
        if (rows.isEmpty() && deletes.isEmpty()) return saved(0, "Saved Successfully");
        Set<Long> employees = ids(repo.activeEmployees(u), "EmployeeId");
        Map<Long, Long> own = new HashMap<>();
        for (Map<String, Object> r : repo.cplList(u, 2)) own.put((long) toInt(r.get("CPLAttendanceId")), (long) toInt(r.get("EmployeeId")));
        Map<String, Set<Long>> attendance = new HashMap<>();
        LocalDateTime now = LocalDateTime.now();
        List<CPLAttendance> list = new ArrayList<>();
        for (LeaveCplRowDto r : rows) {
            if (!employees.contains(r.employeeId)) throw invalid("EmployeeName Field Required");
            if (r.id > 0 && !own.containsKey(r.id)) throw invalid("Record not found.");
            LocalDateTime duty = toDay(r.cplDate);
            if (r.employeeAttendanceId > 0) {
                boolean ok = false;
                if (duty != null) {
                    String k = r.employeeId + ":" + duty.getMonthValue() + ":" + duty.getYear();
                    Set<Long> s = attendance.computeIfAbsent(k, x -> ids(repo.cplAttendanceRecords(u, r.employeeId, duty.getMonthValue(), duty.getYear()), "EmployeeAttendanceId"));
                    ok = s.contains(r.employeeAttendanceId);
                }
                if (!ok && r.id > 0) ok = toInt(one(repo.cpl(r.id)).get("EmployeeAttendanceId")) == r.employeeAttendanceId;
                if (!ok) throw invalid("Record not found.");
            }
            CPLAttendance c = new CPLAttendance();
            c.EmployeeAttendanceId = r.employeeAttendanceId;
            c.CPLAttendanceId = r.id;
            c.EmployeeId = r.employeeId;
            c.CPLDate = duty == null ? MIN_DATE : duty;
            c.CPLQuota = toDec(r.cplQuota);
            LocalDateTime in = toDate(r.inTime), out = toDate(r.outTime);
            c.InTime = in == null ? MIN_DATE : in;
            c.OutTime = out == null ? MIN_DATE : out;
            c.CreatedById = u.getId();
            c.AlteredById = u.getId();
            c.CreatedOn = now;
            c.AlteredOn = now;
            c.ActionTypeId = r.id == 0 ? 1 : 2;
            c.OrganizationId = u.getOrganizationId();
            c.CompanyId = u.getCompanyId();
            c.ReqTypeId = 2;
            list.add(c);
        }
        for (LeaveCplRowDto r : deletes) {                              // grd_ColumnButtonClick "Delete" (Deletelst)
            if (r.id <= 0) continue;
            if (!own.containsKey(r.id)) throw invalid("Record not found.");
            CPLAttendance c = cplOpeningRow(u, r, own, now);
            if (!employees.contains(r.employeeId)) c.EmployeeId = own.get(r.id);   // the record's own employee
            c.ActionTypeId = 3;
            list.add(c);
        }
        repo.saveCpl(list);
        return saved(0, "Saved Successfully");
    }

    // ================================================================== 462 CPL Request (frmEmployeeCPLRequest.cs)

    /** frmItemPricingSchedule_Load: EmployeeNameFill (genEmployee.Getall, BindDDL Id / Name). */
    public Map<String, Object> cplRequestSetup() {
        UserAccount u = hrm.user(SCREEN_CPL_REQUEST);
        return map("rights", hrm.rights(u, SCREEN_CPL_REQUEST), "employees", employeeCombo(u));
    }

    /** cmbEmployee_ValueChanged -> GetEmployeeHistory. */
    public Map<String, Object> cplRequestEmployee(long employeeId) {
        UserAccount u = hrm.user(SCREEN_CPL_REQUEST);
        if (employeeId > 0 && !owns(repo.employees(u), "EmployeeId", (int) employeeId)) throw invalid("Record not found.");
        return employeeHistory(u, employeeId);
    }

    /** cmbEmployee_TextChanged -> CPLAttendance.GetCPLRecordForEmployeeLeaveRequest(employee): the unused CPL days. */
    public List<Map<String, Object>> cplRequestCpl(long employeeId) {
        UserAccount u = hrm.user(SCREEN_CPL_REQUEST);
        if (employeeId > 0 && !owns(repo.employees(u), "EmployeeId", (int) employeeId)) throw invalid("Record not found.");
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> c : repo.cplForLeaveRequest(u, employeeId)) {
            out.add(map("CPLEmployeeLeaveId", c.get("CPLEmployeeLeaveId"), "EmployeeLeaveId", c.get("EmployeeLeaveId"),
                    "CPLAttendanceId", c.get("CPLAttendanceId"), "ActionTypeId", c.get("ActionTypeId"), "CPLDate", day(c.get("CPLDate")),
                    "WeekDayName", c.get("WeekDayName"), "CPLQuota", c.get("CPLQuota"), "Active", toBool(c.get("Active"))));
        }
        return out;
    }

    /** FormHistory(): genEmployeeLeave.ReadAll LeaveTypeProfileId 13. */
    public List<Map<String, Object>> cplRequestHistory() {
        UserAccount u = hrm.user(SCREEN_CPL_REQUEST);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.leaves(u, CPL_LEAVE_TYPE)) {
            out.add(map("EmployeeLeaveId", r.get("EmployeeLeaveId"), "EmployeeName", r.get("EmployeeName"), "LocationName", r.get("LocationName"),
                    "LeaveType", r.get("LeaveType"), "FromDate", day(r.get("FromDate")), "ToDate", day(r.get("ToDate")),
                    "NoOfDays", r.get("NoOfDays"), "Reason", r.get("Reason")));
        }
        return out;
    }

    /** DataGridHistory_ColumnButtonClick "Edit" -> GetByItemId. */
    public Map<String, Object> cplRequestById(long id) {
        UserAccount u = hrm.user(SCREEN_CPL_REQUEST);
        if (!owns(repo.leaves(u, CPL_LEAVE_TYPE), "EmployeeLeaveId", (int) id)) throw invalid("Record not found.");
        return leaveRecord(id);
    }

    /**
     * Insert(): (1) datagrid empty -> "Please add record first in grid"; (2) rows != sum of the ticked CPLQuota ->
     * "Noofleaves cannot be equal to sum of CPL of grid"; (3) FormValiadation ("Employee Required", "Reason Required");
     * then the header (LeaveTypeProfileId 13, EmployeeLeaveQuotaId 0, NoOfDays = ToDate.Day - FromDate.Day,
     * ActionTypeId 1 on save and update alike), the leave dates + Deletelst and one CPLEmployeeLeaveDetail per
     * ticked CPL row, genEmployeeLeave.Save. CPL quotas are read from the database, not from the page.
     */
    public Map<String, Object> cplRequestSave(LeaveCplRequestSaveDto b) {
        UserAccount u = hrm.user(SCREEN_CPL_REQUEST);
        List<LeaveDateRowDto> details = nn(b.details), deletes = nn(b.deletes);
        int recId = Math.max(0, b.id);
        if (recId > 0 && !owns(repo.leaves(u, CPL_LEAVE_TYPE), "EmployeeLeaveId", recId)) throw invalid("Record not found.");
        if (b.employeeId > 0 && !owns(repo.employees(u), "EmployeeId", b.employeeId)) throw invalid("Record not found.");

        Map<Long, Double> quota = new HashMap<>();
        if (b.employeeId > 0) for (Map<String, Object> c : repo.cplForLeaveRequest(u, b.employeeId)) quota.put((long) toInt(c.get("CPLAttendanceId")), toDouble(c.get("CPLQuota")));
        if (recId > 0) for (Map<String, Object> c : repo.leaveCplDetails(recId)) quota.put((long) toInt(c.get("CPLAttendanceId")), toDouble(c.get("CPLQuota")));
        double cplQuota = 0.0;
        List<Long> picked = new ArrayList<>();
        if (b.cpl != null) {
            for (LeaveCplRequestSaveDto.Cpl c : b.cpl) {
                if (!c.active) continue;
                Double q = quota.get(c.cplAttendanceId);
                if (q == null) throw invalid("Record not found.");
                cplQuota += q;
                picked.add(c.cplAttendanceId);
            }
        }
        int num = details.size();
        if (num == 0) throw invalid("Please add record first in grid");
        if ((double) num != cplQuota) throw invalid("Noofleaves cannot be equal to sum of CPL of grid");
        if (b.employeeId == 0) throw invalid("Employee Required");
        if (str(b.reason).isEmpty()) throw invalid("Reason Required");
        Set<Long> own = recId > 0 ? ids(repo.leaveDetails(recId), "EmployeeLeaveDetailId") : new HashSet<>();

        LocalDateTime now = LocalDateTime.now();
        LocalDate from = toDay(b.fromDate) == null ? now.toLocalDate() : toDay(b.fromDate).toLocalDate();
        LocalDate to = toDay(b.toDate) == null ? now.toLocalDate() : toDay(b.toDate).toLocalDate();
        Map<String, Object> emp = employeeHistory(u, b.employeeId);
        GenEmployeeLeave m = new GenEmployeeLeave();
        m.EmployeeLeaveId = recId;
        m.LocationId = toInt(emp.get("LocationId"));
        m.DocMovementId = 1;
        m.EmployeeId = b.employeeId;
        m.LeaveTypeProfileId = CPL_LEAVE_TYPE;
        m.EmployeeLeaveQuotaId = 0L;
        m.FromDate = from.atTime(now.toLocalTime());                    // DateTimePicker.Value keeps the time of day
        m.ToDate = to.atTime(now.toLocalTime());
        m.NoOfDays = BigDecimal.valueOf(to.getDayOfMonth() - from.getDayOfMonth());
        m.Reason = str(b.reason);
        m.ApprovedById = u.getId();
        m.ApprovedOn = now;
        m.CreatedById = u.getId();
        m.CreatedOn = now;
        m.AlteredById = u.getId();
        m.AlteredOn = now;
        m.ActionTypeId = 1;
        m.UserLogId = u.getId();
        m.CompanyId = u.getCompanyId();
        m.OrganizationId = u.getOrganizationId();
        List<LeaveDateRowDto> none = new ArrayList<>();
        addLeaveDates(m, u, details, none, own, now);
        for (Long id : picked) {
            CPLEmployeeLeaveDetail c = new CPLEmployeeLeaveDetail();
            c.CPLAttendanceId = id;
            c.ActionTypeId = 1;
            m.CPLEmployeeLeaveDetailslist.add(c);
        }
        List<GenEmployeeLeaveDetail> dels = new ArrayList<>();
        GenEmployeeLeave tmp = new GenEmployeeLeave();
        addLeaveDates(tmp, u, none, deletes, own, now);
        dels.addAll(tmp.genEmployeeLeaveDetailList);
        m.genEmployeeLeaveDetailList.addAll(dels);                         // Deletelst after the grid rows
        long id = repo.saveLeave(m);
        return saved((int) id, recId == 0 && id > 0 ? "Save Successfully" : "Update Successfully");
    }
}
