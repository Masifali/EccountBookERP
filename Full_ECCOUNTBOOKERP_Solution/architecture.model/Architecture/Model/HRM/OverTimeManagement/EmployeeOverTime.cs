using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.OverTimeManagement;

public class EmployeeOverTime
{
	public byte ActionTypeId { get; set; }

	public bool IsApproved { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime ApprovedOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public decimal ApprovedAmount { get; set; }

	public decimal ApprovedHours { get; set; }

	public decimal NetOTHours { get; set; }

	public decimal OTHours { get; set; }

	public decimal OTRate { get; set; }

	public decimal ShortHours { get; set; }

	public decimal AddLessOTHours { get; set; }

	public int OverTimeMonth { get; set; }

	public int OverTimeYear { get; set; }

	public long AlteredById { get; set; }

	public long ApprovedById { get; set; }

	public long CompanyId { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeAttendanceId { get; set; }

	public long EmployeeId { get; set; }

	public long EmployeeOverTimeId { get; set; }

	public long LocationId { get; set; }

	public long OrganizationId { get; set; }

	public long OverTimeRequestDetailId { get; set; }

	public long UserLogId { get; set; }

	public virtual List<EmployeeOverTime> EmployeeOverTimeList { get; set; }
}
