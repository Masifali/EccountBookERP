namespace Architecture.Model.Tax.Purchase;

public class PurchaseInvoiceJournal
{
	public double JvCredit { get; set; }

	public double JvDebit { get; set; }

	public double JvPrcnt { get; set; }

	public double JvQty { get; set; }

	public double JvRate { get; set; }

	public int ChartofAccountId { get; set; }

	public int FreightId { get; set; }

	public int Id { get; set; }

	public int InvGrnId { get; set; }

	public int PurchaseInvoiceId { get; set; }

	public int SupplierCustomerId { get; set; }

	public string JvRemarks { get; set; }

	public virtual string AccountTitle { get; set; }
}
