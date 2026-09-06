using System;

namespace Architecture.Model.Inventory;

public class InvAllocateBrandItemToDiscType
{
	public bool IsActive { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int BrandItemId { get; set; }

	public int CompanyId { get; set; }

	public int DiscountTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }
}
