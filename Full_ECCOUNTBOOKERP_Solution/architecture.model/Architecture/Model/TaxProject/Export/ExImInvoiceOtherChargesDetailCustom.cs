namespace Architecture.Model.TaxProject.Export;

public class ExImInvoiceOtherChargesDetailCustom
{
	public double AddAmount { get; set; }

	public double LessAmount { get; set; }

	public int ChargesItemId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int Id { get; set; }

	public string Remarks { get; set; }

	public virtual string ChargesItemName { get; set; }
}
