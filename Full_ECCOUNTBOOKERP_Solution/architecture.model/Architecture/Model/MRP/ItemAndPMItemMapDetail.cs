namespace Architecture.Model.MRP;

public class ItemAndPMItemMapDetail
{
	public double PmQty { get; set; }

	public double WeightCapacity { get; set; }

	public int ActionTypeId { get; set; }

	public int BaseUomId { get; set; }

	public int ItemAndPMItemMapDetailId { get; set; }

	public int ItemAndPMItemMapId { get; set; }

	public int PmItemId { get; set; }

	public int seqNo { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string PmItemName { get; set; }

	public virtual string PmItemCode { get; set; }

	public virtual string BaseUomCode { get; set; }

	public virtual double BaseUomEquivalent { get; set; }
}
