namespace Architecture.Model.Mfg;

public class workOrderItemDetail
{
	public decimal Qty { get; set; }

	public int ActionTypeId { get; set; }

	public int CastingTypeId { get; set; }

	public int ItemId { get; set; }

	public int ItemVariantId { get; set; }

	public int OrderId { get; set; }

	public int sortNo { get; set; }

	public int UomId { get; set; }

	public int WorkOrderId { get; set; }

	public int workOrderItemDetailId { get; set; }

	public virtual int OrderNo { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual double SandWeight { get; set; }

	public virtual double DyeWeight { get; set; }

	public virtual string Uom { get; set; }

	public virtual double UomEquivalent { get; set; }

	public virtual string VariantDescription { get; set; }

	public virtual decimal VariantEquivalent { get; set; }

	public virtual string CastingType { get; set; }
}
