using System;

namespace Architecture.Model.TaxProject.Inventory;

public class SaleInvoicePaymentTermsDetail
{
	public decimal Amount { get; set; }

	public decimal PrcntOfTotal { get; set; }

	public DateTime DueDate { get; set; }

	public int DueDays { get; set; }

	public int Id { get; set; }

	public int InvSaleInvoiceId { get; set; }

	public int PaymentTermId { get; set; }

	public int SortNo { get; set; }

	public string PaymentRemarks { get; set; }
}
