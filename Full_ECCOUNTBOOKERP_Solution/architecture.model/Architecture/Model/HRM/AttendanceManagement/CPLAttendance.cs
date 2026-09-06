using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.AttendanceManagement;

public class CPLAttendance
{
	public byte ActionTypeId { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime CPLDate { get; set; }

	public DateTime InTime { get; set; }

	public DateTime OutTime { get; set; }

	public int AlteredById { get; set; }

	public int CompanyId { get; set; }

	public int CreatedById { get; set; }

	public int OrganizationId { get; set; }

	public long CPLAttendanceId { get; set; }

	public long EmployeeId { get; set; }

	public int ReqTypeId { get; set; }

	public long EmployeeAttendanceId { get; set; }

	public decimal CPLQuota { get; set; }

	public virtual string EmployeeNo { get; set; }

	public virtual string EmployeeName { get; set; }

	public virtual List<CPLAttendance> CPLAttendancesList { get; set; }
}
