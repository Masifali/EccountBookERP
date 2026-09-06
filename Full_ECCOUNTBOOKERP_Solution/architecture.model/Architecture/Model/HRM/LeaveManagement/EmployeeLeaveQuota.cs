using System;

namespace Architecture.Model.HRM.LeaveManagement;

public class EmployeeLeaveQuota
{
	public byte ActionTypeId { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public decimal Assigned { get; set; }

	public decimal Avail { get; set; }

	public decimal Availed { get; set; }

	public decimal CPLQuota { get; set; }

	public int LeaveQuotaDetailId { get; set; }

	public long AlteredById { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeId { get; set; }

	public long EmployeeLeaveOpeningId { get; set; }

	public long EmployeeLeaveQuotaId { get; set; }

	public long UserLogId { get; set; }
}
