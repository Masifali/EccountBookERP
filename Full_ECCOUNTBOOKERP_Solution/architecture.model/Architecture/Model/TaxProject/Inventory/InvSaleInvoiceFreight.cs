namespace Architecture.Model.TaxProject.Inventory;

public class InvSaleInvoiceFreight
{
	public double Debit { get; set; }

	public double FreightAmount { get; set; }

	public double FrQty { get; set; }

	public double FrRate { get; set; }

	public double FrWeight { get; set; }

	public int Id { get; set; }

	public int InvGdnId { get; set; }

	public int InvSaleInvoiceId { get; set; }

	public int TansporterId { get; set; }

	public int TransporterSupCustId { get; set; }

	public string Remarks { get; set; }
}
