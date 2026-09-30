package com.mst.models.hrm.roster;

import com.mst.models.hrm.DesktopModel;

import java.time.LocalDateTime;

/**
 * Architecture.Model.HRM.AttendanceManagement.genDutyRoasterDetail (model 0730) - every non-virtual
 * property, as GenericProvider.SetProc sends it to Sp_genDutyRoasterDetail_Insert (36 parameters, all
 * declared by the procedure). DateTime? properties are null unless the form sets them - a null is
 * omitted from the EXEC exactly as AddWithValue(null) omits it on the desktop.
 * ShiftShortName and IsHoliday are virtual on the desktop (never sent): transient here.
 */
public class GenDutyRoasterDetail extends DesktopModel {
    public int ActionTypeId;
    public boolean IsOffDuty;
    public boolean IsOnDuty;
    public LocalDateTime AbsentAfterInDateTime;
    public LocalDateTime AbsentBeforeOutDateTime;
    public LocalDateTime AlteredOn;
    public LocalDateTime CreatedOn;
    public LocalDateTime DutyDate;
    public LocalDateTime EndTime;
    public LocalDateTime GraceInDateTime;
    public LocalDateTime GraceOutDateTime;
    public LocalDateTime GraceOutEndDateTime;
    public LocalDateTime GraceOutStartDateTime;
    public LocalDateTime HalfDayInEndDateTime;
    public LocalDateTime HalfDayInStartDateTime;
    public LocalDateTime HalfDayOutEndDateTime;
    public LocalDateTime HalfDayOutStartDateTime;
    public LocalDateTime RangeInDateTime;
    public LocalDateTime RangeOutDateTime;
    public LocalDateTime ShortLeaveInEndDateTime;
    public LocalDateTime ShortLeaveInStartDateTime;
    public LocalDateTime ShortLeaveOutEndDateTime;
    public LocalDateTime ShortLeaveOutStartDateTime;
    public LocalDateTime StartTime;
    public int ShiftId;
    public int ShiftTimingId;
    public long AlteredById;
    public long CreatedById;
    public long DutyRoasterDetailId;
    public long DutyRoasterDetailLineId;
    public long DutyRoasterId;
    public long EmployeeHistoryId;
    public long EmployeeId;
    public long UserLogId;
    public int EmployeeGroupId;
    public String WeekDayName;

    public transient String ShiftShortName;
    public transient boolean IsHoliday;
}
