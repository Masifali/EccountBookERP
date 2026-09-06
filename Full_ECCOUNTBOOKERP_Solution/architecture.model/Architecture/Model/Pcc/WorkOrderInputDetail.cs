namespace Architecture.Model.pcc;

public class WorkOrderInputDetail
{
	public decimal ItemQty { get; set; }

	public decimal WeightKg { get; set; }

	public int BomHeaderId { get; set; }

	public int BomInputId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int SortNo { get; set; }

	public int WorkOrderHeaderId { get; set; }

	public string Remarks { get; set; }

	public virtual string BomNo { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemUomCode { get; set; }

	public virtual double ItemUomEquivalent { get; set; }
}
