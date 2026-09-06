using System;

namespace Architecture.Model.HRM.LeaveManagement;

public class CPLEmployeeLeaveDetail
{
	public long CPLEmployeeLeaveId { get; set; }

	public long CPLAttendanceId { get; set; }

	public long EmployeeLeaveId { get; set; }

	public int ActionTypeId { get; set; }

	public virtual DateTime CPLDate { get; set; }

	public virtual string WeekDayName { get; set; }

	public virtual double CPLQuota { get; set; }

	public virtual bool Active { get; set; }
}
