namespace Architecture.Model.FeedMill.Purchase;

public class InvPurchaseInvoiceFreight
{
	public decimal CreditAmount { get; set; }

	public decimal DebitAmount { get; set; }

	public decimal Percentage { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public decimal Weight { get; set; }

	public int FreightId { get; set; }

	public int Id { get; set; }

	public int InvGrnId { get; set; }

	public int InvPurchaseInvoiceId { get; set; }

	public int PurchaseOrderExpensesChargeToProductId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TansporterId { get; set; }

	public string Remarks { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual string SupplierName { get; set; }
}
