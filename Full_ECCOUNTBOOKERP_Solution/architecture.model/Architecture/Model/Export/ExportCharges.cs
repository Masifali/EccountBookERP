using System;

namespace Architecture.Model.Export;

public class ExportCharges
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int AccountId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int ExportChargesID { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public string ExportChargesCode { get; set; }

	public string ExportChargesName { get; set; }
}
