using System;

namespace Architecture.Model.HRM.PolicyManagment;

public class genEmployeeShortLeave
{
	public byte ActionTypeId { get; set; }

	public bool IsApproved { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime ApplyDate { get; set; }

	public DateTime ApprovedOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime FromTime { get; set; }

	public DateTime ToTime { get; set; }

	public decimal NoOfHours { get; set; }

	public long AlteredById { get; set; }

	public long ApprovedById { get; set; }

	public long CompanyId { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeId { get; set; }

	public long EmployeeLeaveQuotaId { get; set; }

	public long EmployeeShortLeaveId { get; set; }

	public long LocationId { get; set; }

	public long OrganizationId { get; set; }

	public long UserLogId { get; set; }

	public string IsAdjustStatus { get; set; }

	public string Reason { get; set; }

	public string Remarks { get; set; }

	public virtual string EmployeeName { get; set; }

	public virtual string LocationName { get; set; }
}
