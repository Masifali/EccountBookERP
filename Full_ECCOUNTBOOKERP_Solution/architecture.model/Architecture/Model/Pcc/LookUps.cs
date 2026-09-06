using System;

namespace Architecture.Model.pcc;

public class LookUps
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int LookupTypeId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public string Description { get; set; }

	public string LookupName { get; set; }
}
