namespace Architecture.Model.Export;

public class FcyBankReceiptPaymentTermDetail
{
	public double ThisReceipt { get; set; }

	public int ExImInvoicePaymentTermsDetailId { get; set; }

	public int FcyBankReceiptId { get; set; }

	public int Id { get; set; }

	public int LineId { get; set; }

	public int PaymentTermId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int AdjustmentVoucherId { get; set; }

	public virtual string PaymentTerm { get; set; }
}
