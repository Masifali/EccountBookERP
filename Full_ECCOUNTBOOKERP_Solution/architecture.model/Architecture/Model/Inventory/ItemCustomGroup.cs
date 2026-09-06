using System;

namespace Architecture.Model.Inventory;

public class ItemCustomGroup
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public string GroupCode { get; set; }

	public string GroupName { get; set; }
}
