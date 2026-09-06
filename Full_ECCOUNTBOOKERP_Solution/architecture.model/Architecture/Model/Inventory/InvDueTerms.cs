using System;

namespace Architecture.Model.Inventory;

public class InvDueTerms
{
	public int Id { get; set; }

	public string TermsCode { get; set; }

	public string TermsDescription { get; set; }

	public DateTime EntryDate { get; set; }

	public int EntryUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ModifyUser { get; set; }

	public DateTime PostDate { get; set; }

	public int PostUser { get; set; }

	public bool PostState { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }
}
