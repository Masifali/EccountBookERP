using System;

namespace Architecture.Model.SDT;

public class CustomGroup
{
	public bool isActive { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int customGroupId { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int sortNo { get; set; }

	public string customGroupName { get; set; }
}
