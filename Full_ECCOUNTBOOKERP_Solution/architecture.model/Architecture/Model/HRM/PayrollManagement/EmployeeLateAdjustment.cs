using System;

namespace Architecture.Model.HRM.PayrollManagement;

public class EmployeeLateAdjustment
{
	public long ActionTypeId { get; set; }

	public bool IsDeduction { get; set; }

	public bool IsLeaveAdjust { get; set; }

	public long TotalLates { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime ApprovedOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public decimal DeductedDays { get; set; }

	public decimal DeductionRate { get; set; }

	public decimal LateDeductionAmount { get; set; }

	public decimal TotalLateDays { get; set; }

	public int DocMovementId { get; set; }

	public int LAMonth { get; set; }

	public int LAYear { get; set; }

	public long AlteredById { get; set; }

	public long ApprovedById { get; set; }

	public long CompanyId { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeId { get; set; }

	public long EmployeeLateAdjustmentId { get; set; }

	public long EmployeeLeaveId { get; set; }

	public long LocationId { get; set; }

	public long OrganizationId { get; set; }

	public long UserLogId { get; set; }

	public virtual string EmployeeName { get; set; }

	public virtual string LocationName { get; set; }
}
