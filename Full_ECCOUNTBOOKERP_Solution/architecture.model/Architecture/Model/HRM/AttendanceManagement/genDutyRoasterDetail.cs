using System;

namespace Architecture.Model.HRM.AttendanceManagement;

public class genDutyRoasterDetail
{
	public int ActionTypeId { get; set; }

	public bool IsOffDuty { get; set; }

	public bool IsOnDuty { get; set; }

	public DateTime? AbsentAfterInDateTime { get; set; }

	public DateTime? AbsentBeforeOutDateTime { get; set; }

	public DateTime? AlteredOn { get; set; }

	public DateTime? CreatedOn { get; set; }

	public DateTime? DutyDate { get; set; }

	public DateTime? EndTime { get; set; }

	public DateTime? GraceInDateTime { get; set; }

	public DateTime? GraceOutDateTime { get; set; }

	public DateTime? GraceOutEndDateTime { get; set; }

	public DateTime? GraceOutStartDateTime { get; set; }

	public DateTime? HalfDayInEndDateTime { get; set; }

	public DateTime? HalfDayInStartDateTime { get; set; }

	public DateTime? HalfDayOutEndDateTime { get; set; }

	public DateTime? HalfDayOutStartDateTime { get; set; }

	public DateTime? RangeInDateTime { get; set; }

	public DateTime? RangeOutDateTime { get; set; }

	public DateTime? ShortLeaveInEndDateTime { get; set; }

	public DateTime? ShortLeaveInStartDateTime { get; set; }

	public DateTime? ShortLeaveOutEndDateTime { get; set; }

	public DateTime? ShortLeaveOutStartDateTime { get; set; }

	public DateTime? StartTime { get; set; }

	public int ShiftId { get; set; }

	public int ShiftTimingId { get; set; }

	public long AlteredById { get; set; }

	public long CreatedById { get; set; }

	public long DutyRoasterDetailId { get; set; }

	public long DutyRoasterDetailLineId { get; set; }

	public long DutyRoasterId { get; set; }

	public long EmployeeHistoryId { get; set; }

	public long EmployeeId { get; set; }

	public long UserLogId { get; set; }

	public int EmployeeGroupId { get; set; }

	public string WeekDayName { get; set; }

	public virtual string ShiftShortName { get; set; }

	public virtual bool IsHoliday { get; set; }
}
