namespace Architecture.Model.Steel.Inventory;

public class InvPurchaseInvoiceFreight
{
	public decimal CreditAmount { get; set; }

	public decimal DebitAmount { get; set; }

	public decimal FrQty { get; set; }

	public decimal FrRate { get; set; }

	public decimal FrWeight { get; set; }

	public decimal Percentage { get; set; }

	public int FreightId { get; set; }

	public int Id { get; set; }

	public int InvGrnId { get; set; }

	public int InvPurchaseInvoiceId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int TansporterId { get; set; }

	public string Remarks { get; set; }
}
