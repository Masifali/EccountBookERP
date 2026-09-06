namespace Architecture.Model.Main;

public class GloablUomSchedule
{
	public int Id { get; set; }

	public string UOMCode { get; set; }

	public string UOMDescription { get; set; }

	public double Equivalent { get; set; }

	public bool BaseRateUom { get; set; }

	public bool BasePackUom { get; set; }

	public bool BaseSecondaryUom { get; set; }

	public int ItemId { get; set; }

	public bool Active { get; set; }

	public double QtyEquivalent { get; set; }

	public string ItemName { get; set; }

	public string ItemCode { get; set; }
}
