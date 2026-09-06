using System;

namespace Architecture.Model.Inventory;

public class SupplierCustomerExemptionSchedule
{
	public DateTime FromDate { get; set; }

	public DateTime ToDate { get; set; }

	public int Id { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TaxPrcnt { get; set; }

	public int TaxTypeId { get; set; }

	public virtual string CompanyName { get; set; }

	public virtual string TaxName { get; set; }
}
