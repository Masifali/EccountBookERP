namespace Architecture.Model.Accounts;

public class VouchersAdvanceAdjustedInvoicesDetail
{
	public double BalanceAmount { get; set; }

	public double PaidAmount { get; set; }

	public int Id { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int VouchersAdvanceAdjustmentByInvoiceId { get; set; }

	public string SubRemarksInvoices { get; set; }
}
