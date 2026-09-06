using System;

namespace Architecture.Model.HRM.PolicyManagment;

public class genEmployeeLeaveDetail
{
	public byte ActionTypeId { get; set; }

	public bool IsApproved { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime LeaveDate { get; set; }

	public long AlteredById { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeLeaveDetailId { get; set; }

	public long EmployeeLeaveId { get; set; }

	public long UserLogId { get; set; }
}
