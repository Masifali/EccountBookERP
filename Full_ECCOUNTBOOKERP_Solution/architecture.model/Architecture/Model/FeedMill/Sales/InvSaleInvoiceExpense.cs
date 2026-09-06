namespace Architecture.Model.FeedMill.Sales;

public class InvSaleInvoiceExpense
{
	public double Amount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int Id { get; set; }

	public int InvGdnId { get; set; }

	public int InvRevExpItemId { get; set; }

	public int InvSaleInvoiceId { get; set; }

	public string Remarks { get; set; }
}
