using System;

namespace Architecture.Model.Inventory;

public class TaxSalesNotesGLMaping
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int GLAccountId { get; set; }

	public int Id { get; set; }

	public int LookUpsTaxInOutId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int SortNo { get; set; }

	public int TaxLookUpsId { get; set; }
}
