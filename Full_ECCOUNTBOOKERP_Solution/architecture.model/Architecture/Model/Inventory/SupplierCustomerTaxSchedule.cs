using System;

namespace Architecture.Model.Inventory;

public class SupplierCustomerTaxSchedule
{
	public decimal TaxPercent { get; set; }

	public string AnyRemarks { get; set; }

	public int OrganizationId { get; set; }

	public bool IsExempted { get; set; }

	public DateTime ToDate { get; set; }

	public int CompanyId { get; set; }

	public string TaxAbleStatus { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int SupplierCustomerId { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime FromDate { get; set; }

	public int TaxImplementationTypeId { get; set; }

	public int EntryUserId { get; set; }

	public DateTime ModifyDate { get; set; }
}
