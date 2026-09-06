using System;
using System.Collections.Generic;

namespace Architecture.Model.Service;

public class ItemPricingSupplierCustomer
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EffectedFromH { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocTypeId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int USD { get; set; }

	public string PricingGroup { get; set; }

	public string RemarksHeader { get; set; }

	public virtual List<ItemPricingSupplierCustomerDetail> ItemPricingDetail { get; set; }
}
