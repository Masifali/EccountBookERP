using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class CustomerAllocateToSalesMan
{
	public bool IsActive { get; set; }

	public DateTime EffectiveDate { get; set; }

	public double CommRate { get; set; }

	public int CompanyId { get; set; }

	public int CustomerId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public int SalesManId { get; set; }

	public string CommType { get; set; }

	public virtual List<CustomerAllocateToSalesMan> customerAllocateToSalesMenList { get; set; }
}
