namespace Architecture.Model.HRM.AttendanceManagement;

public class DutyRoasterGroups
{
	public int Id { get; set; }

	public int EmployeeGroupId { get; set; }

	public int ShiftId { get; set; }

	public long DutyRoasterId { get; set; }

	public int WeekDayId { get; set; }
}
