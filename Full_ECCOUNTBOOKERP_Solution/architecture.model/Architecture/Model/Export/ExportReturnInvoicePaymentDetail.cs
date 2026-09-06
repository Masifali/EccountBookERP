namespace Architecture.Model.Export;

public class ExportReturnInvoicePaymentDetail
{
	public double FcyAmount { get; set; }

	public double LcyAmount { get; set; }

	public double AdjustmentAmount { get; set; }

	public double PrcntOfTotal { get; set; }

	public int ExportReturnInvoiceId { get; set; }

	public int ExImInvoicePaymentDetailId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int Id { get; set; }

	public int SortNo { get; set; }

	public int PaymentTermId { get; set; }

	public string PaymentRemarks { get; set; }

	public int DueDays { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public virtual string PaymentTerm { get; set; }
}
