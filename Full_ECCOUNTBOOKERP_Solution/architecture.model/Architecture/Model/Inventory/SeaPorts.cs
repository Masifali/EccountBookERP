using System;

namespace Architecture.Model.Inventory;

public class SeaPorts
{
	public bool PostState { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public int CompanyId { get; set; }

	public int CountryId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int PortTypeId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public string PortName { get; set; }
}
