using System;

namespace Architecture.Model;

public class UserAccountAllocation
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int IsActive { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int UserAccountId { get; set; }

	public virtual string CompName { get; set; }

	public virtual string ScreenName { get; set; }
}
