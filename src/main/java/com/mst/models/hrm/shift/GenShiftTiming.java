package com.mst.models.hrm.shift;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;

/**
 * Architecture.Model.HRM.AttendanceManagement.genShiftTiming (model 0733) - SetProc to
 * Sp_genShiftTiming_Insert / Sp_genShiftTiming_Update (42 parameters; the Insert also declares the optional
 * @StartDateCheck / @EndDateCheck, which the desktop never sends). LocationName and
 * GenShiftTimingWeekDaysDetailList are virtual on the desktop and are transient here.
 */
public class GenShiftTiming extends DesktopModel {
    public boolean IsNextDayClosed;
    public LocalDateTime AbsentAfterInTime;
    public LocalDateTime AbsentBeforeOutTime;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public LocalDateTime EndBreakTime;
    public LocalDateTime EndDate;
    public LocalDateTime EndTime;
    public LocalDateTime GraceInEndTime;
    public LocalDateTime GraceOutEndTime;
    public LocalDateTime GraceOutStartTime;
    public LocalDateTime GraceTime;
    public LocalDateTime HalfDayInEndTime;
    public LocalDateTime HalfDayInStartTime;
    public LocalDateTime HalfDayOutEndTime;
    public LocalDateTime HalfDayOutStartTime;
    public LocalDateTime ReportTime;
    public LocalDateTime ShortLeaveInEndTime;
    public LocalDateTime ShortLeaveInStartTime;
    public LocalDateTime ShortLeaveOutEndTime;
    public LocalDateTime ShortLeaveOutStartTime;
    public LocalDateTime StartBreakTime;
    public LocalDateTime StartDate;
    public LocalDateTime StartTime;
    public int ActionTypeId;
    public int AfterOutHours;
    public int AfterOutMinutes;
    public int AlteredById;
    public int BeforeInHours;
    public int BeforeInMinutes;
    public int BranchId;
    public int CompanyId;
    public int CreatedById;
    public int OrganizationId;
    public int ProjectId;
    public int ReportDays;
    public int ShiftLocationId;
    public int ShiftTimingId;
    public int ShiftId;
    public int UserLogId;
    public String TimingDescription;
    public String WeekDayText;
    public transient String LocationName;
    public transient List<GenShiftTimingWeekDay> GenShiftTimingWeekDaysDetailList = new ArrayList<>();
}
