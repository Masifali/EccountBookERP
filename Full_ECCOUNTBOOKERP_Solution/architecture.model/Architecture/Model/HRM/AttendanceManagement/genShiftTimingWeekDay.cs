using System;

namespace Architecture.Model.HRM.AttendanceManagement;

public class genShiftTimingWeekDay
{
	public int ActionTypeId { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public int ShiftTimingId { get; set; }

	public int WeekDayProfileId { get; set; }

	public long AlteredById { get; set; }

	public long CreatedById { get; set; }

	public long ShiftTimingWeekDayId { get; set; }

	public long UserLogId { get; set; }

	public string WeekDayName { get; set; }
}
