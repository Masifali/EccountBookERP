using System;

namespace Architecture.Model.PartyProcessing;

public class ItemTypePartyProcessing
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int LookUpId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public string TypeCode { get; set; }

	public string TypeDescription { get; set; }
}
