namespace Architecture.Model.ModernHistoryModels;

public class EmtpyBagsDetailForInvoiceHistory
{
	public int InvoiceId { get; set; }

	public int ItemId { get; set; }

	public int TypeId { get; set; }

	public string ItemName { get; set; }

	public double Amount { get; set; }

	public string Remarks { get; set; }
}
