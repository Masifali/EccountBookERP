using System;

namespace Architecture.Model.Inventory;

public class InvPurchaseInvoicePaymentTermsDetail
{
	public decimal Amount { get; set; }

	public decimal PrcntOfTotal { get; set; }

	public DateTime DueDate { get; set; }

	public int DueDays { get; set; }

	public int Id { get; set; }

	public int InvPurchaseInvoiceId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int PaymentTermId { get; set; }

	public bool SystemGeneratedRow { get; set; }

	public int SortNo { get; set; }

	public string PaymentRemarks { get; set; }

	public virtual int PurchaseOrderNo { get; set; }
}
