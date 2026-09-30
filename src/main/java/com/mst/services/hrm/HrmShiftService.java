package com.mst.services.hrm;

import com.mst.models.UserAccount;
import com.mst.models.hrm.dto.ShiftHolidayRowDto;
import com.mst.models.hrm.dto.ShiftHolidaySaveDto;
import com.mst.models.hrm.dto.ShiftLocationSaveDto;
import com.mst.models.hrm.dto.ShiftSaveDto;
import com.mst.models.hrm.dto.ShiftTimingSaveDto;
import com.mst.models.hrm.dto.ShiftWeekDayDto;
import com.mst.models.hrm.shift.GenHoliday;
import com.mst.models.hrm.shift.GenShift;
import com.mst.models.hrm.shift.GenShiftLocation;
import com.mst.models.hrm.shift.GenShiftTiming;
import com.mst.models.hrm.shift.GenShiftTimingWeekDay;
import com.mst.repositories.hrm.HrmShiftRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import static com.mst.services.hrm.HrmSupport.*;

/**
 * BLL of the HRM shift screens (AppModules 2022): 652 Define Shift, 653 Shift Location, 654 Shift Timing,
 * 655 Holiday. Each method names the desktop form method it reproduces; validation wording/order and the
 * messages are the form's.
 */
@Service
public class HrmShiftService {

    public static final int SCREEN_SHIFT = 652;
    public static final int SCREEN_SHIFT_LOCATION = 653;
    public static final int SCREEN_SHIFT_TIMING = 654;
    public static final int SCREEN_HOLIDAY = 655;
    /** frmDutyRoasterNew ("DutyRoaster") - opened from Shift Timing's toolbar. */
    public static final int SCREEN_DUTY_ROASTER = 656;
    /** genProfile type of the week days (frmGenShiftTiming.WeekDaysFill). */
    public static final int PROFILE_TYPE_WEEK_DAYS = 60;

    private static final DateTimeFormatter HM = DateTimeFormatter.ofPattern("HH:mm");
    private static final DateTimeFormatter YMD = DateTimeFormatter.ofPattern("yyyy-MM-dd");

    @Autowired private HrmShiftRepository repo;
    @Autowired private HrmSupport hrm;

    // ------------------------------------------------------------------ shared combos

    /** braches(): BindDDLNew(dtBranch, "Id", "BranchName") - the first row is activated. */
    private List<Map<String, Object>> branchCombo(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.branches(u)) out.add(map("Id", r.get("Id"), "BranchName", r.get("BranchName")));
        return out;
    }

    /** Project(): BindDDLNew(dtProject, "Id", "ProjectName") - the first row is activated. */
    private List<Map<String, Object>> projectCombo(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.projects(u)) out.add(map("Id", r.get("Id"), "ProjectName", r.get("ProjectName")));
        return out;
    }

    /** A hidden Branch / Project combo value: 0 (empty) or one of the company's own rows. */
    private static int ownOrZero(List<Map<String, Object>> rows, String key, int id) {
        if (id == 0) return 0;
        if (!owns(rows, key, id)) throw invalid("Record not found.");
        return id;
    }

    private static String hm(Object v) { LocalDateTime d = toDate(v); return d == null ? "" : d.format(HM); }

    private static String ymd(Object v) { LocalDateTime d = toDate(v); return d == null ? "" : d.format(YMD); }

    // ================================================================== 652 frmGenShift

    /** frmGenShift_Load: Project(), braches(), GridBind(); + the "ShiftTiming" view right btnShiftTiming checks. */
    public Map<String, Object> shiftSetup() {
        UserAccount u = hrm.user(SCREEN_SHIFT);
        return map("rights", hrm.rights(u, SCREEN_SHIFT), "branches", branchCombo(u), "projects", projectCombo(u),
                "rows", shiftGrid(u), "canShiftTiming", hrm.can(u, SCREEN_SHIFT_TIMING, "View"));
    }

    /** ResetForm(): Project(), braches(), GridBind(). */
    public Map<String, Object> shiftReload() {
        UserAccount u = hrm.user(SCREEN_SHIFT);
        return map("branches", branchCombo(u), "projects", projectCombo(u), "rows", shiftGrid(u));
    }

    public List<Map<String, Object>> shifts() { return shiftGrid(hrm.user(SCREEN_SHIFT)); }

    /** GridBind: Id (hidden), Shift = ShiftName, ShortName = ShiftShortName. */
    private List<Map<String, Object>> shiftGrid(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.shifts(u)) out.add(map("Id", r.get("ShiftId"), "Shift", r.get("ShiftName"), "ShortName", r.get("ShiftShortName")));
        return out;
    }

    /** grdShift_DoubleClick -> ReadById(Id) -> genShift.GetByID. */
    public Map<String, Object> shift(int id) {
        UserAccount u = hrm.user(SCREEN_SHIFT);
        if (!owns(repo.shifts(u), "ShiftId", id)) throw invalid("Record not found.");
        Map<String, Object> r = one(repo.shift(id));
        return map("ShiftId", r.get("ShiftId"), "BranchId", toInt(r.get("BranchId")), "ProjectId", toInt(r.get("ProjectId")),
                "ShiftName", r.get("ShiftName"), "ShiftShortName", r.get("ShiftShortName"), "ShiftUrduName", r.get("ShiftUrduName"));
    }

    /**
     * btnsave_Click / btnupdate_Click: Validation() ("Shift Name Filed Required!"), then the model as each
     * button fills it - Save: Company, Organization, CreatedById, Branch, Project, names; Update: + ShiftId,
     * AlteredById instead of CreatedById - and genShift.Save (ActionTypeId 1 + CreatedOn / 2 + AlteredOn).
     * UserLogId is never set by the form (0), as on the desktop.
     */
    public Map<String, Object> saveShift(ShiftSaveDto b) {
        UserAccount u = hrm.user(SCREEN_SHIFT);
        if (str(b.shiftName).isEmpty()) throw invalid("Shift Name Filed Required!");
        int id = Math.max(0, b.id);
        if (id > 0 && !owns(repo.shifts(u), "ShiftId", id)) throw invalid("Record not found.");
        GenShift m = new GenShift();
        m.ShiftId = id;
        m.CompanyId = u.getCompanyId();
        m.OrganizationId = u.getOrganizationId();
        if (id > 0) m.AlteredById = u.getId(); else m.CreatedById = u.getId();
        m.BranchId = ownOrZero(repo.branches(u), "Id", b.branchId);
        m.ProjectId = ownOrZero(repo.projects(u), "Id", b.projectId);
        m.ShiftName = str(b.shiftName);
        m.ShiftShortName = str(b.shortName);
        m.ShiftUrduName = str(b.urduName);
        LocalDateTime now = LocalDateTime.now();
        if (id == 0) { m.ActionTypeId = 1; m.CreatedOn = now; } else { m.ActionTypeId = 2; m.AlteredOn = now; }
        int n = repo.saveShift(m);
        if (n <= 0) throw invalid("Record not saved.");
        return saved(n, id > 0 ? "Update Successfully" : "Save Successfully");
    }

    // ================================================================== 653 frmShiftLocation

    /** frmShiftLocation_Load: Project(), braches(), ShiftComboFill(), LocationComboFill(), GridBind(). */
    public Map<String, Object> shiftLocationSetup() {
        UserAccount u = hrm.user(SCREEN_SHIFT_LOCATION);
        List<Map<String, Object>> shifts = new ArrayList<>();
        for (Map<String, Object> r : repo.shifts(u)) shifts.add(map("Id", r.get("ShiftId"), "SelectShift", r.get("ShiftName")));
        List<Map<String, Object>> locations = new ArrayList<>();
        for (Map<String, Object> r : repo.locations(u)) locations.add(map("Id", r.get("LocationId"), "SelectLocation", r.get("LocationName")));
        return map("rights", hrm.rights(u, SCREEN_SHIFT_LOCATION), "branches", branchCombo(u), "projects", projectCombo(u),
                "shifts", shifts, "locations", locations, "rows", shiftLocationGrid(u));
    }

    public List<Map<String, Object>> shiftLocations() { return shiftLocationGrid(hrm.user(SCREEN_SHIFT_LOCATION)); }

    /** GridBind: ShiftLocationId (hidden), Shift, Location, IsActive ("True"/"False" check box). */
    private List<Map<String, Object>> shiftLocationGrid(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.shiftLocations(u, 0, false)) {
            out.add(map("ShiftLocationId", r.get("ShiftLocationId"), "Shift", r.get("ShiftName"), "Location", r.get("LocationName"),
                    "IsActive", toBool(r.get("IsActive")) ? "True" : "False"));
        }
        return out;
    }

    /** grdShiftLocation_DoubleClick -> RetrivedData(Cells[0]) -> genShiftLocation.GetByID. */
    public Map<String, Object> shiftLocation(int id) {
        UserAccount u = hrm.user(SCREEN_SHIFT_LOCATION);
        if (!owns(repo.shiftLocations(u, 0, false), "ShiftLocationId", id)) throw invalid("Record No Found");
        Map<String, Object> r = one(repo.shiftLocation(id));
        return map("ShiftLocationId", r.get("ShiftLocationId"), "BranchId", toInt(r.get("BranchId")), "ProjectId", toInt(r.get("ProjectId")),
                "ShiftId", toInt(r.get("ShiftId")), "LocationId", toInt(r.get("LocationId")), "IsActive", toBool(r.get("IsActive")));
    }

    /**
     * btnsave_Click / btnupdate_Click: Validation() ("Shift Required!", "Location Required!"), the model as each
     * button fills it (Save: CreatedById; Update: ShiftLocationId + AlteredById - the Update procedure then
     * writes CreatedById 0 / CreatedOn NULL, as the desktop does) and genShiftLocation.Save.
     */
    public Map<String, Object> saveShiftLocation(ShiftLocationSaveDto b) {
        UserAccount u = hrm.user(SCREEN_SHIFT_LOCATION);
        if (b.shiftId == 0 || !owns(repo.shifts(u), "ShiftId", b.shiftId)) throw invalid("Shift Required!");
        if (b.locationId == 0 || !owns(repo.locations(u), "LocationId", b.locationId)) throw invalid("Location Required!");
        int id = Math.max(0, b.id);
        if (id > 0 && !owns(repo.shiftLocations(u, 0, false), "ShiftLocationId", id)) throw invalid("Record No Found");
        GenShiftLocation m = new GenShiftLocation();
        m.ShiftLocationId = id;
        m.ShiftId = b.shiftId;
        m.LocationId = b.locationId;
        m.BranchId = ownOrZero(repo.branches(u), "Id", b.branchId);
        m.ProjectId = ownOrZero(repo.projects(u), "Id", b.projectId);
        m.IsActive = b.isActive;
        m.OrganizationId = u.getOrganizationId();
        m.CompanyId = u.getCompanyId();
        if (id > 0) m.AlteredById = u.getId(); else m.CreatedById = u.getId();
        LocalDateTime now = LocalDateTime.now();
        if (id == 0) { m.CreatedOn = now; m.ActionTypeId = 1; } else { m.ActionTypeId = 2; m.AlteredOn = now; }
        int n = repo.saveShiftLocation(m);
        if (n <= 0) throw invalid("Record not saved.");
        return saved(n, id > 0 ? "Update Successfully" : "Save Successfully");
    }

    // ================================================================== 654 frmGenShiftTiming

    /** frmGenShiftTiming_Load: rights ("ShiftTiming"), BindShift(), WeekDaysFill(); + the "DutyRoaster" view right. */
    public Map<String, Object> shiftTimingSetup() {
        UserAccount u = hrm.user(SCREEN_SHIFT_TIMING);
        return map("rights", hrm.rights(u, SCREEN_SHIFT_TIMING), "shifts", shiftTimingShifts(u), "weekDays", weekDays(u),
                "canDutyRoaster", hrm.can(u, SCREEN_DUTY_ROASTER, "View"));
    }

    /** toolStripButton1_Click (Refresh) / RefreshForm: BindShift() + WeekDaysFill(). */
    public Map<String, Object> shiftTimingReload() {
        UserAccount u = hrm.user(SCREEN_SHIFT_TIMING);
        return map("shifts", shiftTimingShifts(u), "weekDays", weekDays(u));
    }

    /** BindShift: BindDDLNew(genShift.Getall, "ShiftId", "ShiftName"). */
    private List<Map<String, Object>> shiftTimingShifts(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.shifts(u)) out.add(map("ShiftId", r.get("ShiftId"), "ShiftName", r.get("ShiftName")));
        return out;
    }

    /** WeekDaysFill: genProfile type 60 -> ProfileId (hidden), ProfileName ("Day Name"). */
    private List<Map<String, Object>> weekDays(UserAccount u) {
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.profilesByType(u, PROFILE_TYPE_WEEK_DAYS)) out.add(map("ProfileId", r.get("ProfileId"), "ProfileName", r.get("ProfileName")));
        return out;
    }

    /**
     * CmbShift_Leave -> BindShiftLocation(): genShiftLocation.Getall(ShiftId = CmbShift.Value, IsActive = true);
     * the combo's value column "ShiftId" carries ShiftLocationId. ShiftId 0 is not sent (every active location).
     */
    public List<Map<String, Object>> shiftTimingLocations(int shiftId) {
        UserAccount u = hrm.user(SCREEN_SHIFT_TIMING);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.shiftLocations(u, shiftId, true))
            out.add(map("ShiftId", r.get("ShiftLocationId"), "LocationName", r.get("LocationName"), "ShiftName", r.get("ShiftName")));
        return out;
    }

    /** GridBind (the History tab): the desktop's DataTable columns, dates dd-MMM-yyyy (page) and times HH:mm. */
    public List<Map<String, Object>> shiftTimings() {
        UserAccount u = hrm.user(SCREEN_SHIFT_TIMING);
        List<Map<String, Object>> out = new ArrayList<>();
        for (Map<String, Object> r : repo.shiftTimings(u)) {
            Map<String, Object> o = map("Id", toInt(r.get("ShiftTimingId")), "LoacationName", r.get("LocationName"), "ShiftName", r.get("ShiftName"),
                    "ShiftTimingDesc", r.get("TimingDescription"), "Location", r.get("ShiftLocationId"), "WeekDay", r.get("WeekDayText"),
                    "StartDate", ymd(r.get("StartDate")), "EndDate", ymd(r.get("EndDate")),
                    "StartTime", hm(r.get("StartTime")), "EndTime", hm(r.get("EndTime")),
                    "startBreaktime", hm(r.get("StartBreakTime")), "EndBreaktime", hm(r.get("EndBreakTime")),
                    "GraceInTime", hm(r.get("GraceTime")), "GraceEndTime", hm(r.get("GraceInEndTime")),
                    "BeforeInHourn", r.get("BeforeInHours"), "BeforeInMinute", r.get("BeforeInMinutes"),
                    "AfterOutHourn", r.get("AfterOutHours"), "AfterOutMinute", r.get("AfterOutMinutes"),
                    "ReprotTime", hm(r.get("ReportTime")), "ReportDay", r.get("ReportDays"));
            o.put("ShortLeaveInStartTime", hm(r.get("ShortLeaveInStartTime")));
            o.put("ShortLeaveInEndTime", hm(r.get("ShortLeaveInEndTime")));
            o.put("ShortLeaveOutStartTime", hm(r.get("ShortLeaveOutStartTime")));
            o.put("ShortLeaveOutEndTime", hm(r.get("ShortLeaveOutEndTime")));
            o.put("HaldDayInStartTime", hm(r.get("HalfDayInStartTime")));
            o.put("HaldDayInEndTime", hm(r.get("HalfDayInEndTime")));
            o.put("HaldDayOutStartTime", hm(r.get("HalfDayOutStartTime")));
            o.put("HaldDayOutEndTime", hm(r.get("HalfDayOutEndTime")));
            o.put("GraceOutStartTime", hm(r.get("GraceOutStartTime")));
            o.put("GraceOutEndTime", hm(r.get("GraceOutEndTime")));
            o.put("AbsentAfterInStartTime", hm(r.get("AbsentAfterInTime")));
            o.put("AbsentBeforeOutTime", hm(r.get("AbsentBeforeOutTime")));
            out.add(o);
        }
        return out;
    }

    /** grdShift_DoubleClick / Edit / SaveAs -> ReadById(Id) -> genShiftTiming.GetByID. */
    public Map<String, Object> shiftTiming(int id) {
        UserAccount u = hrm.user(SCREEN_SHIFT_TIMING);
        if (!owns(repo.shiftTimings(u), "ShiftTimingId", id)) throw invalid("Record not found.");
        Map<String, Object> r = one(repo.shiftTiming(id));
        Map<String, Object> o = map("ShiftTimingId", r.get("ShiftTimingId"), "TimingDescription", r.get("TimingDescription"),
                "ShiftId", toInt(r.get("ShiftId")), "ShiftLocationId", toInt(r.get("ShiftLocationId")),
                "StartDate", ymd(r.get("StartDate")), "EndDate", ymd(r.get("EndDate")),
                "StartTime", hm(r.get("StartTime")), "EndTime", hm(r.get("EndTime")),
                "StartBreakTime", hm(r.get("StartBreakTime")), "EndBreakTime", hm(r.get("EndBreakTime")),
                "GraceTime", hm(r.get("GraceTime")), "GraceInEndTime", hm(r.get("GraceInEndTime")),
                "BeforeInHours", toInt(r.get("BeforeInHours")), "AfterOutHours", toInt(r.get("AfterOutHours")),
                "GraceOutStartTime", hm(r.get("GraceOutStartTime")), "GraceOutEndTime", hm(r.get("GraceOutEndTime")));
        o.put("ShortLeaveInStartTime", hm(r.get("ShortLeaveInStartTime")));
        o.put("ShortLeaveInEndTime", hm(r.get("ShortLeaveInEndTime")));
        o.put("ShortLeaveOutStartTime", hm(r.get("ShortLeaveOutStartTime")));
        o.put("ShortLeaveOutEndTime", hm(r.get("ShortLeaveOutEndTime")));
        o.put("HalfDayInStartTime", hm(r.get("HalfDayInStartTime")));
        o.put("HalfDayOutStartTime", hm(r.get("HalfDayOutStartTime")));
        o.put("HalfDayOutEndTime", hm(r.get("HalfDayOutEndTime")));
        o.put("HalfDayInEndTime", hm(r.get("HalfDayInEndTime")));
        o.put("AbsentAfterInTime", hm(r.get("AbsentAfterInTime")));
        o.put("AbsentBeforeOutTime", hm(r.get("AbsentBeforeOutTime")));
        o.put("IsNextDayClosed", toBool(r.get("IsNextDayClosed")));
        return o;
    }

    /** Convert.ToDateTime(StartDate.ToString("dd/MMM/yyyy") + " " + picker.TimeOfDay). */
    private static LocalDateTime at(LocalDate day, String time) {
        String t = trim(time);
        try {
            if (t.length() == 5) t = t + ":00";
            return day.atTime(LocalTime.parse(t.length() > 8 ? t.substring(0, 8) : t));
        } catch (RuntimeException e) {
            throw invalid("String was not recognized as a valid DateTime.");
        }
    }

    /** Convert.ToDouble(text): FormatException for blank / non-numeric text. */
    private static double toDoubleStrict(String s) {
        String t = trim(s).replace(",", "");
        if (!t.matches("[+-]?(\\d+\\.?\\d*|\\.\\d+)([eE][+-]?\\d+)?")) throw invalid("Input string was not in a correct format.");
        return Double.parseDouble(t);
    }

    /**
     * Insert(): Validation() (Shift / Shift Location / Description), "Please Select Week Days", CheckShiftTiming
     * (runs on the still-empty model, so it never fails), Total Days Hours must be exactly 24, then the model
     * exactly as the form fills it - every time joined to the Start Date, ReportTime = now, WeekDayText "" -
     * one genShiftTimingWeekDay per ticked day, and genShiftTiming.Save (one transaction).
     * btnsave needs the Save right and btnupdate the Update right (frmGenShiftTiming_Load enables them so).
     */
    public Map<String, Object> saveShiftTiming(ShiftTimingSaveDto b) {
        UserAccount u = hrm.user(SCREEN_SHIFT_TIMING);
        int id = Math.max(0, b.id);
        hrm.require(u, SCREEN_SHIFT_TIMING, id > 0 ? "Update" : "Save");
        if (b.shiftId == 0 || !owns(repo.shifts(u), "ShiftId", b.shiftId)) throw invalid("Shift Required!");
        if (b.shiftLocationId == 0 || !owns(repo.shiftLocations(u, b.shiftId, true), "ShiftLocationId", b.shiftLocationId)) throw invalid("Shift Location Required!");
        if (str(b.description).isEmpty()) throw invalid("Description Required!");
        List<ShiftWeekDayDto> days = b.weekDays == null ? new ArrayList<>() : b.weekDays;
        if (days.isEmpty()) throw invalid("Please Select Week Days");
        if (toDoubleStrict(b.totalDaysHours) != 24.0) throw invalid("Total Days Hours not less or gratter than 24 hours");
        if (id > 0 && !owns(repo.shiftTimings(u), "ShiftTimingId", id)) throw invalid("Record not found.");
        List<Map<String, Object>> profiles = repo.profilesByType(u, PROFILE_TYPE_WEEK_DAYS);
        LocalDateTime start = toDay(b.startDate), end = toDay(b.endDate);
        if (start == null || end == null) throw invalid("String was not recognized as a valid DateTime.");
        LocalDate d = start.toLocalDate();
        LocalDateTime now = LocalDateTime.now();
        GenShiftTiming t = new GenShiftTiming();
        t.ShiftTimingId = id;
        t.OrganizationId = u.getOrganizationId();
        t.CompanyId = u.getCompanyId();
        t.TimingDescription = str(b.description);
        t.ShiftId = b.shiftId;
        t.ShiftLocationId = b.shiftLocationId;
        t.StartDate = start;
        t.EndDate = end;
        t.StartTime = at(d, b.startTime);
        t.StartBreakTime = at(d, b.startBreakTime);
        t.EndBreakTime = at(d, b.endBreakTime);
        t.EndTime = at(d, b.endTime);
        t.WeekDayText = "";
        t.GraceTime = at(d, b.lateStartTime);
        t.GraceInEndTime = at(d, b.lateEndTime);
        t.BeforeInHours = toInt(b.inHours);
        t.AfterOutHours = toInt(b.outHours);
        t.ReportTime = now;
        t.GraceOutStartTime = at(d, b.departureStartTime);
        t.GraceOutEndTime = at(d, b.departureEndTime);
        t.ShortLeaveInStartTime = at(d, b.shortLeaveInStartTime);
        t.ShortLeaveInEndTime = at(d, b.shortLeaveInEndTime);
        t.ShortLeaveOutStartTime = at(d, b.shortLeaveOutStartTime);
        t.ShortLeaveOutEndTime = at(d, b.shortLeaveOutEndTime);
        t.HalfDayInStartTime = at(d, b.halfDayInStartTime);
        t.HalfDayOutStartTime = at(d, b.halfDayOutStartTime);
        t.HalfDayOutEndTime = at(d, b.halfDayOutEndTime);
        t.HalfDayInEndTime = at(d, b.halfDayInEndTime);
        t.AbsentAfterInTime = at(d, b.absentAfterInTime);
        t.AbsentBeforeOutTime = at(d, b.absentBeforeOutTime);
        t.CreatedById = u.getId();
        t.CreatedOn = now;
        t.AlteredById = u.getId();
        t.AlteredOn = now;
        t.UserLogId = u.getId();
        t.IsNextDayClosed = b.isNextDayClosed;
        t.ActionTypeId = id == 0 ? 1 : 2;
        for (ShiftWeekDayDto w : days) {
            if (w == null || !owns(profiles, "ProfileId", w.profileId)) throw invalid("Record not found.");
            GenShiftTimingWeekDay o = new GenShiftTimingWeekDay();
            o.WeekDayProfileId = w.profileId;
            o.WeekDayName = str(w.name);
            o.CreatedById = u.getId();
            o.CreatedOn = now;
            o.AlteredById = u.getId();
            o.AlteredOn = now;
            o.ActionTypeId = 1;
            o.UserLogId = u.getId();
            t.GenShiftTimingWeekDaysDetailList.add(o);
        }
        int n = repo.saveShiftTiming(t);
        return saved(n, id == 0 ? "Record Save Successfully" : "Record Update Successfully");
    }

    // ================================================================== 655 frmHoliday

    /** frmHoliday_Load: MonthFill (page) + YearFill (CommonServices.GetYears: ActiveYr.Start_Period.Year .. this year). */
    public Map<String, Object> holidaySetup() {
        UserAccount u = hrm.user(SCREEN_HOLIDAY);
        return map("rights", hrm.rights(u, SCREEN_HOLIDAY), "years", years(u));
    }

    private List<Map<String, Object>> years(UserAccount u) {
        int yearId = hrm.financialYearId();
        int thisYear = LocalDate.now().getYear();
        int from = thisYear;
        List<Map<String, Object>> fy = repo.financialYears(u);
        for (Map<String, Object> r : fy) {
            if (toInt(r.get("Id")) == yearId) {
                LocalDateTime s = toDate(r.get("Start_Period"));
                if (s != null) from = s.getYear();
            }
        }
        List<Map<String, Object>> out = new ArrayList<>();
        for (int y = from; y <= thisYear; y++) out.add(map("Id", y, "Year", y));
        return out;
    }

    /**
     * GetByMonthandYearForUpdate: genHoliday.GetByMonthAndYear(Month, Year). The procedure does not filter by
     * company, so only the signed-in company's own rows are handed to the page (tenancy guard).
     */
    public List<Map<String, Object>> holidaysByMonth(int month, int year) {
        UserAccount u = hrm.user(SCREEN_HOLIDAY);
        List<Map<String, Object>> out = new ArrayList<>();
        if (month <= 0 || year <= 0) return out;
        for (Map<String, Object> r : repo.holidaysByMonth(month, year)) {
            if (toInt(r.get("OrginizationId")) != toInt(u.getOrganizationId()) || toInt(r.get("CompanyId")) != toInt(u.getCompanyId())) continue;
            out.add(map("HolidayId", toInt(r.get("HolidayId")), "HolidayDate", ymd(r.get("HolidayDate")), "Name", r.get("Name"),
                    "IsOffDay", toBool(r.get("IsOffDay")), "IsGazetted", toBool(r.get("IsGazetted")), "HolidayDetail", r.get("HolidayDetail")));
        }
        return out;
    }

    /**
     * Insert(): "On a Same Day you cannot check both IsOff and IsGazette", then one genHoliday per grid row
     * (Id, Date, Description, Day, IsOff, IsGazette + audit) and genHoliday.Save; "Save Successfully".
     */
    public Map<String, Object> saveHolidays(ShiftHolidaySaveDto b) {
        UserAccount u = hrm.user(SCREEN_HOLIDAY);
        List<ShiftHolidayRowDto> rows = b == null || b.rows == null ? new ArrayList<>() : b.rows;
        for (ShiftHolidayRowDto r : rows) {
            if (r != null && r.isOff && r.isGazette) throw invalid("On a Same Day you cannot check both IsOff and IsGazette");
        }
        List<Map<String, Object>> mine = null;
        LocalDateTime now = LocalDateTime.now();
        List<GenHoliday> list = new ArrayList<>();
        for (ShiftHolidayRowDto r : rows) {
            if (r == null) continue;
            int id = Math.max(0, r.id);
            if (id > 0) {
                if (mine == null) mine = repo.holidays(u);
                if (!owns(mine, "HolidayId", id)) throw invalid("Record not found.");
            }
            LocalDateTime date = toDay(r.date);
            if (date == null) throw invalid("String was not recognized as a valid DateTime.");
            GenHoliday h = new GenHoliday();
            h.HolidayId = id;
            h.HolidayDate = date;
            h.HolidayDetail = str(r.description);
            h.Name = str(r.day);
            h.IsOffDay = r.isOff;
            h.IsGazetted = r.isGazette;
            h.UserLogId = u.getId();
            h.OrginizationId = u.getOrganizationId();
            h.CompanyId = u.getCompanyId();
            h.AlteredById = u.getId();
            h.CreatedById = u.getId();
            h.CreatedOn = now;
            h.AlteredOn = now;
            h.ActionTypeId = 1;
            list.add(h);
        }
        int n = repo.saveHolidays(list, u);
        return saved(n, "Save Successfully");
    }
}
