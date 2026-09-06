namespace Architecture.Model.Mfg;

public class workOrderMaterial
{
	public decimal price { get; set; }

	public decimal Qty { get; set; }

	public double amount { get; set; }

	public int ActionTypeId { get; set; }

	public int ItemId { get; set; }

	public int sortNo { get; set; }

	public int uomId { get; set; }

	public int WorkOrderId { get; set; }

	public int WorkOrderMaterialId { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double PackEquivalent { get; set; }
}
