namespace Architecture.Model.ModernHistoryModels;

public class OtherExpensedDetailForInvoiceHistory
{
	public int InvoiceId { get; set; }

	public int ItemId { get; set; }

	public string ItemName { get; set; }

	public double Amount { get; set; }

	public string Remarks { get; set; }
}
