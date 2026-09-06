using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class RevExpAcAllocationToProductAndDiscountType
{
	public DateTime EffetedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int DiscountTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProductTypeId { get; set; }

	public int ReveneuExpenseAccountId { get; set; }

	public virtual string ReveneuExpenseAccount { get; set; }

	public virtual string DiscountType { get; set; }

	public virtual string ProductType { get; set; }

	public virtual List<RevExpAcAllocationToProductAndDiscountType> RevExpAcAllocationToProductAndDiscountTypeList { get; set; }
}
