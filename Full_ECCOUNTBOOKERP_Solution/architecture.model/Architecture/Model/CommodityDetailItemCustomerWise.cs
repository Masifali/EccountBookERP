using System;

namespace Architecture.Model;

public class CommodityDetailItemCustomerWise
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int DetailTypeId { get; set; }

	public string Remarks { get; set; }

	public string HSCode { get; set; }

	public virtual string SupplierCustomer { get; set; }

	public virtual string Item { get; set; }
}
