using System;
using System.Collections.Generic;

namespace Architecture.Model.Tax.Definitions;

public class RegularItemsAllocateToTaxItem
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int TaxItemId { get; set; }

	public bool IsActive { get; set; }

	public virtual string TaxItem { get; set; }

	public virtual List<RegularItemsAllocateToTaxItem> RegularItemAllocateToTaxItemList { get; set; }
}
