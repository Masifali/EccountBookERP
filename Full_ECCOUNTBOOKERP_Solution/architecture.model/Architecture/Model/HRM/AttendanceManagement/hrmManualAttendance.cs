using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.AttendanceManagement;

public class hrmManualAttendance
{
	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime DutyDate { get; set; }

	public DateTime? InDateTime { get; set; }

	public DateTime? OutDateTime { get; set; }

	public decimal ManualCPLQuota { get; set; }

	public int ActionTypeId { get; set; }

	public int AlteredById { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int CreatedById { get; set; }

	public int DepartmentId { get; set; }

	public int EmployeeId { get; set; }

	public int LocationId { get; set; }

	public int ManualAttendanceId { get; set; }

	public int OrganizationId { get; set; }

	public int ShiftId { get; set; }

	public int UserLogId { get; set; }

	public string AttendanceStatus { get; set; }

	public string Description { get; set; }

	public string IsPresent { get; set; }

	public virtual List<hrmManualAttendance> listHrmManualAttendances { get; set; }
}
