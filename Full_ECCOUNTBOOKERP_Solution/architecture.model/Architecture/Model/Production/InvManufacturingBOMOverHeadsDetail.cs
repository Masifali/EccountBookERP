namespace Architecture.Model.Production;

public class InvManufacturingBOMOverHeadsDetail
{
	public double ohAmount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int amountUomId { get; set; }

	public int Id { get; set; }

	public int InvManufacturingBOMHeaderId { get; set; }

	public int OverHeadTypeId { get; set; }

	public string OhRemarks { get; set; }

	public virtual string OverHeadAccountTitle { get; set; }
}
