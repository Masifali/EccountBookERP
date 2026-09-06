using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class SupplierCustomerAllocateToDiscounType
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int DiscounTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int SupplierCustomerId { get; set; }

	public virtual List<SupplierCustomerAllocateToDiscounType> SupplierCustomerAllocateToDiscounTypeList { get; set; }
}
