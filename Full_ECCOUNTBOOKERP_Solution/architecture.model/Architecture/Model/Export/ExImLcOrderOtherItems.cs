namespace Architecture.Model.Export;

public class ExImLcOrderOtherItems
{
	public decimal oItemAmount { get; set; }

	public decimal oItemQty { get; set; }

	public decimal oItemWeightKgs { get; set; }

	public double oItemRate { get; set; }

	public int ExImLcOrderId { get; set; }

	public int Id { get; set; }

	public int otherItemId { get; set; }

	public string OtherItemRemarks { get; set; }

	public virtual string ItemName { get; set; }
}
