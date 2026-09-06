using System;

namespace Architecture.Model.HRM.OverTimeManagement;

public class OverTimeRequestDetail
{
	public int ActionTypeId { get; set; }

	public bool IsAllowMeal { get; set; }

	public bool IsApproved { get; set; }

	public long OTHours { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime FromDateTime { get; set; }

	public DateTime ToDateTime { get; set; }

	public decimal OTRate { get; set; }

	public long AlteredById { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeId { get; set; }

	public long OverTimeRequestDetailId { get; set; }

	public long OverTimeRequestId { get; set; }

	public long UserLogId { get; set; }

	public bool RequestStatus { get; set; }

	public virtual string EmployeeName { get; set; }
}
