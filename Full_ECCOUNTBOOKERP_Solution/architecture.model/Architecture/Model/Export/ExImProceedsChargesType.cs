using System;

namespace Architecture.Model.Export;

public class ExImProceedsChargesType
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProceedsChargesGlAccount { get; set; }

	public string ProceedsChargesTypeCode { get; set; }

	public string ProceedsChargesTypedescription { get; set; }
}
