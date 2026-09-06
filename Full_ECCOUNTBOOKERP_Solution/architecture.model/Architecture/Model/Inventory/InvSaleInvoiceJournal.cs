namespace Architecture.Model.Inventory;

public class InvSaleInvoiceJournal
{
	public double JvCredit { get; set; }

	public double JvDebit { get; set; }

	public double JvPrcnt { get; set; }

	public double JvQty { get; set; }

	public double JvRate { get; set; }

	public int ChartofAccountId { get; set; }

	public int TransporterSupCustId { get; set; }

	public int Id { get; set; }

	public int InvSaleInvoiceId { get; set; }

	public string JvRemarks { get; set; }
}
