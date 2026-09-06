using System;

namespace Architecture.Model.CRM;

public class CRMLookupType
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int SerialFrom { get; set; }

	public int SerialTo { get; set; }

	public string CRMLookUpName { get; set; }

	public string Prefix { get; set; }
}
