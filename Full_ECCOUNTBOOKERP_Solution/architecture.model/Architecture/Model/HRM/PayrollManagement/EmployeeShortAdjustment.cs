using System;

namespace Architecture.Model.HRM.PayrollManagement;

public class EmployeeShortAdjustment
{
	public bool IsDeduction { get; set; }

	public bool IsLeaveAdjust { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime ApprovedOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public decimal DeductionRate { get; set; }

	public decimal ShortDeductionAmount { get; set; }

	public int ActionTypeId { get; set; }

	public int DeductedShortHours { get; set; }

	public int EmployeeLeaveId { get; set; }

	public int SAMonth { get; set; }

	public int SAYear { get; set; }

	public int TotalShortHours { get; set; }

	public int TotalShortMinutes { get; set; }

	public long AlteredById { get; set; }

	public long ApprovedById { get; set; }

	public long CompanyId { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeId { get; set; }

	public long EmployeeShortAdjustmentId { get; set; }

	public long LocationId { get; set; }

	public long OrganizationId { get; set; }

	public long UserLogId { get; set; }

	public virtual string EmployeeName { get; set; }
}
