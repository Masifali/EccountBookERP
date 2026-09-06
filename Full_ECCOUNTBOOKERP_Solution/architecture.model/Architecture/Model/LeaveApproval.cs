using System.Collections.Generic;

namespace Architecture.Model;

public class LeaveApproval
{
	public int LeaveQuotaId { get; set; }

	public int LeaveTypeProfileId { get; set; }

	public int EmployeeId { get; set; }

	public int EntryUser { get; set; }

	public int EmployeeLeaveDetailId { get; set; }

	public int ActionId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public virtual List<LeaveApproval> LeaveApprovalslist { get; set; }
}
