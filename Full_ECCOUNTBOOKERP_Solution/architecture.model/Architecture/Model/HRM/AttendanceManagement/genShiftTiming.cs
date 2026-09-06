using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.AttendanceManagement;

public class genShiftTiming
{
	public bool IsNextDayClosed { get; set; }

	public DateTime AbsentAfterInTime { get; set; }

	public DateTime AbsentBeforeOutTime { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime EndBreakTime { get; set; }

	public DateTime EndDate { get; set; }

	public DateTime EndTime { get; set; }

	public DateTime GraceInEndTime { get; set; }

	public DateTime GraceOutEndTime { get; set; }

	public DateTime GraceOutStartTime { get; set; }

	public DateTime GraceTime { get; set; }

	public DateTime HalfDayInEndTime { get; set; }

	public DateTime HalfDayInStartTime { get; set; }

	public DateTime HalfDayOutEndTime { get; set; }

	public DateTime HalfDayOutStartTime { get; set; }

	public DateTime ReportTime { get; set; }

	public DateTime ShortLeaveInEndTime { get; set; }

	public DateTime ShortLeaveInStartTime { get; set; }

	public DateTime ShortLeaveOutEndTime { get; set; }

	public DateTime ShortLeaveOutStartTime { get; set; }

	public DateTime StartBreakTime { get; set; }

	public DateTime StartDate { get; set; }

	public DateTime StartTime { get; set; }

	public int ActionTypeId { get; set; }

	public int AfterOutHours { get; set; }

	public int AfterOutMinutes { get; set; }

	public int AlteredById { get; set; }

	public int BeforeInHours { get; set; }

	public int BeforeInMinutes { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int CreatedById { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int ReportDays { get; set; }

	public int ShiftLocationId { get; set; }

	public int ShiftTimingId { get; set; }

	public int ShiftId { get; set; }

	public int UserLogId { get; set; }

	public string TimingDescription { get; set; }

	public string WeekDayText { get; set; }

	public virtual string LocationName { get; set; }

	public virtual List<genShiftTimingWeekDay> GenShiftTimingWeekDaysDetailList { get; set; }
}
