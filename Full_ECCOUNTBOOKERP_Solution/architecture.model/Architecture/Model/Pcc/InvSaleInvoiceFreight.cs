namespace Architecture.Model.pcc;

public class InvSaleInvoiceFreight
{
	public decimal Debit { get; set; }

	public decimal FreightAmount { get; set; }

	public decimal FrQty { get; set; }

	public decimal FrRate { get; set; }

	public decimal FrWeight { get; set; }

	public int Id { get; set; }

	public int InvGdnId { get; set; }

	public int InvSaleInvoiceId { get; set; }

	public int TansporterId { get; set; }

	public int TransporterSupCustId { get; set; }

	public string Remarks { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual string SupplierCustomer { get; set; }
}
