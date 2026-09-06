namespace Architecture.Model.ModernHistoryModels;

public class ChargeToProductDetailForInvoiceHistory
{
	public int InvoiceId { get; set; }

	public int AccountId { get; set; }

	public string AccountName { get; set; }

	public double Amount { get; set; }

	public string Remarks { get; set; }
}
