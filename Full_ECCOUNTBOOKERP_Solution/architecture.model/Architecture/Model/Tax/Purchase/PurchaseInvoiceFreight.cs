namespace Architecture.Model.Tax.Purchase;

public class PurchaseInvoiceFreight
{
	public double FreightAmount { get; set; }

	public double FrQty { get; set; }

	public double FrRate { get; set; }

	public double FrWeight { get; set; }

	public double Percentage { get; set; }

	public int FreightId { get; set; }

	public int Id { get; set; }

	public int InvGrnId { get; set; }

	public int PurchaseInvoiceId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TansporterId { get; set; }

	public string Remarks { get; set; }
}
