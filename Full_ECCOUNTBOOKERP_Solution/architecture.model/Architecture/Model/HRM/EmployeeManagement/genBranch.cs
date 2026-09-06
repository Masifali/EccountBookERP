using System;

namespace Architecture.Model.HRM.EmployeeManagement;

public class genBranch
{
	public int BranchId { get; set; }

	public int LocationId { get; set; }

	public int AddressId { get; set; }

	public int BranchTypeProfileId { get; set; }

	public string BranchName { get; set; }

	public int CreatedById { get; set; }

	public DateTime? CreatedOn { get; set; }

	public int AlteredById { get; set; }

	public DateTime? AlteredOn { get; set; }

	public int ActionTypeId { get; set; }

	public int UserLogId { get; set; }
}
