using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.LeaveManagement;

public class EmployeeLeaveOpening
{
	public byte ActionTypeId { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public int LeaveQuotaId { get; set; }

	public int LeaveTypeProfileId { get; set; }

	public long AlteredById { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeLeaveOpeningId { get; set; }

	public long UserLogId { get; set; }

	public long OrganizationId { get; set; }

	public long CompanyId { get; set; }

	public virtual List<EmployeeLeaveQuota> EmployeeLeaveQuotasList { get; set; }
}
