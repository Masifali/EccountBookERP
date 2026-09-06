using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class SupplierCustomerAllocateCostCenter
{
	public bool IsActive { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public long SupplierCustomerAllocateCostCenterId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int CostCenterId { get; set; }

	public virtual List<SupplierCustomerAllocateCostCenter> SupplierCustomerAllocateCostCenterList { get; set; }
}
