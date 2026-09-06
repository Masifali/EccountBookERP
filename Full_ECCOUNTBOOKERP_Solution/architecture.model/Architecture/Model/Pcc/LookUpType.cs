using System;

namespace Architecture.Model.pcc;

public class LookUpType
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int SerialFrom { get; set; }

	public int SerialTo { get; set; }

	public string Prefix { get; set; }

	public string TypeName { get; set; }
}
