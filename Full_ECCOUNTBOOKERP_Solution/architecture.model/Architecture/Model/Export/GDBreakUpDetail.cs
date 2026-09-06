namespace Architecture.Model.Export;

public class GDBreakUpDetail
{
	public int Id { get; set; }

	public int GDBreakUpHeaderId { get; set; }

	public int ExImInvoiceId { get; set; }

	public decimal InvoiceAmount { get; set; }

	public string Remarks { get; set; }

	public int ActionTypeId { get; set; }

	public virtual string InvoiceNo { get; set; }
}
