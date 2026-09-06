namespace Architecture.Model.ModernHistoryModels;

public class Customer_Supplier_AddLessDetailForInvoiceHistory
{
	public int InvoiceId { get; set; }

	public int AccountId { get; set; }

	public string AccountName { get; set; }

	public double DebitAmount { get; set; }

	public double CreditAmount { get; set; }

	public string Remarks { get; set; }
}
