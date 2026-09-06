using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class ItemAllocationToSupplierCustomer
{
	public bool IsActive { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public long Id { get; set; }

	public int ItemId { get; set; }

	public int LeadTimeDay { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int SCrankLevel { get; set; }

	public int SupplierCustomerId { get; set; }

	public virtual List<ItemAllocationToSupplierCustomer> ItemAllocationToSupplierCustomerList { get; set; }
}
