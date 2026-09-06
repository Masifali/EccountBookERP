using System;

namespace Architecture.Model.Inventory;

public class InvLookupType
{
	public bool PostState { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime PostDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public string Prefix { get; set; }

	public string ProfileTypeName { get; set; }

	public int SerialFrom { get; set; }

	public int SerialTo { get; set; }
}
