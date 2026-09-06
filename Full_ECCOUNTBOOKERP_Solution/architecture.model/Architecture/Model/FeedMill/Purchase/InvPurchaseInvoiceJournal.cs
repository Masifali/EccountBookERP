namespace Architecture.Model.FeedMill.Purchase;

public class InvPurchaseInvoiceJournal
{
	public decimal Credit { get; set; }

	public decimal Debit { get; set; }

	public decimal Prcnt { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public int ChartofAccountId { get; set; }

	public int FreightId { get; set; }

	public int Id { get; set; }

	public int InvGrnId { get; set; }

	public int InvPurchaseInvoiceId { get; set; }

	public int SupplierCustomerId { get; set; }

	public string Remarks { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual string SupplierName { get; set; }
}
