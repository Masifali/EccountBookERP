using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.LeaveManagement;

public class genEmployeeLeave
{
	public byte ActionTypeId { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime ApprovedOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime FromDate { get; set; }

	public DateTime ToDate { get; set; }

	public decimal NoOfDays { get; set; }

	public int DocMovementId { get; set; }

	public int LeaveTypeProfileId { get; set; }

	public long AlteredById { get; set; }

	public long ApprovedById { get; set; }

	public long CompanyId { get; set; }

	public long CreatedById { get; set; }

	public long EmployeeId { get; set; }

	public long EmployeeLeaveId { get; set; }

	public long EmployeeLeaveQuotaId { get; set; }

	public long LocationId { get; set; }

	public long OrganizationId { get; set; }

	public long UserLogId { get; set; }

	public string Reason { get; set; }

	public string Remarks { get; set; }

	public virtual string LocationName { get; set; }

	public virtual string EmployeeName { get; set; }

	public virtual string LeaveType { get; set; }

	public virtual List<genEmployeeLeaveDetail> genEmployeeLeaveDetailList { get; set; }

	public virtual List<CPLEmployeeLeaveDetail> CPLEmployeeLeaveDetailslist { get; set; }
}
