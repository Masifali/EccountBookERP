namespace Architecture.Model.TaxProject.Export;

public class ExImInvoiceOtherItems
{
	public decimal oItemAmount { get; set; }

	public decimal oItemQty { get; set; }

	public decimal oItemWeightKgs { get; set; }

	public double oItemRate { get; set; }

	public int ExImInvoiceId { get; set; }

	public int Id { get; set; }

	public int otherItemId { get; set; }

	public string OtherItemRemarks { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }
}
