using System;

namespace Architecture.Model.Inventory;

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

	public int SaleOrderId { get; set; }

	public bool SystemGeneratedRow { get; set; }

	public string PaymentRemarks { get; set; }

	public virtual int SaleOrderNo { get; set; }

	public virtual string TermsDescription { get; set; }
}
