namespace Architecture.Model.Production;

public class InvManufacturingBOMBiProductDetail
{
	public double bAmount { get; set; }

	public double bItemQty { get; set; }

	public double bRate { get; set; }

	public double bWeight { get; set; }

	public int bItemId { get; set; }

	public int bItemUomId { get; set; }

	public int bQtyUomId { get; set; }

	public int bRateUomId { get; set; }

	public int Id { get; set; }

	public int InvManufacturingBOMHeaderId { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string UomDesc { get; set; }

	public virtual double UOMEquivalent { get; set; }

	public virtual double RateEquivalent { get; set; }
}
