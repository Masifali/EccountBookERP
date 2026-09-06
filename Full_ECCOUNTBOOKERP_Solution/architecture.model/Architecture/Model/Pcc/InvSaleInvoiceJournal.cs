namespace Architecture.Model.pcc;

public class InvSaleInvoiceJournal
{
	public decimal JvCredit { get; set; }

	public decimal JvDebit { get; set; }

	public decimal JvPrcnt { get; set; }

	public decimal JvQty { get; set; }

	public decimal JvRate { get; set; }

	public int ChartofAccountId { get; set; }

	public int Id { get; set; }

	public int InvGdnId { get; set; }

	public int InvSaleInvoiceId { get; set; }

	public int TransporterSupCustId { get; set; }

	public string JvRemarks { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual string SupplierCustomer { get; set; }
}
