namespace Architecture.Model.Production;

public class ManufacturingBOMPackingMaterialDetail
{
	public double Amount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ManufacturingBOMHeaderId { get; set; }

	public string pmRemarks { get; set; }

	public virtual string ItemName { get; set; }
}
