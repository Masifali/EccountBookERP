namespace Architecture.Model.Production;

public class InvManufacturingBOMMaterialDetail
{
	public double ItemPrice { get; set; }

	public double PercentRate { get; set; }

	public double sAmount { get; set; }

	public double sItemQty { get; set; }

	public double sRate { get; set; }

	public double sWeight { get; set; }

	public int Id { get; set; }

	public int InvManufacturingBOMHeaderId { get; set; }

	public double Percentage { get; set; }

	public double PackingMaterialAmount { get; set; }

	public double OverHeadsAmount { get; set; }

	public int sItemId { get; set; }

	public int CropYearId { get; set; }

	public int sItemUomId { get; set; }

	public int sQtyUomId { get; set; }

	public int sRateUomId { get; set; }

	public string Remarks { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual double RateEquivalent { get; set; }
}
