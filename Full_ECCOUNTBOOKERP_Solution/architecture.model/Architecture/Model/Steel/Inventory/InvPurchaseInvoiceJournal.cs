namespace Architecture.Model.Steel.Inventory;

public class InvPurchaseInvoiceJournal
{
	public decimal JvCredit { get; set; }

	public decimal JvDebit { get; set; }

	public decimal JvPrcnt { get; set; }

	public decimal JvQty { get; set; }

	public decimal JvRate { get; set; }

	public int ChartofAccountId { get; set; }

	public int FreightId { get; set; }

	public int Id { get; set; }

	public int InvGrnId { get; set; }

	public int InvPurchaseInvoiceId { get; set; }

	public string JvRemarks { get; set; }

	public virtual string AccountTitle { get; set; }
}
