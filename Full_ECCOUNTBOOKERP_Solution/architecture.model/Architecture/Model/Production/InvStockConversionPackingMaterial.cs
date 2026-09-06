namespace Architecture.Model.Production;

public class InvStockConversionPackingMaterial
{
	public double ItemAmount { get; set; }

	public double ItemQty { get; set; }

	public double ItemRate { get; set; }

	public int Id { get; set; }

	public int InvStockConversionId { get; set; }

	public int ItemId { get; set; }

	public int WarehouseId { get; set; }

	public int ItemSchUOM { get; set; }

	public int LineId { get; set; }

	public int BrandItemId { get; set; }

	public int ItemConditionId { get; set; }

	public int BrandItemUomId { get; set; }

	public string ChargeTo { get; set; }

	public int ContractScheduleId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int RackId { get; set; }

	public virtual string ContractScheduleNo { get; set; }

	public virtual string InvoiceNo { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string UOMDescription { get; set; }

	public virtual string WarehouseName { get; set; }

	public virtual string BrandName { get; set; }

	public virtual string BrandUom { get; set; }

	public virtual string ItemCondition { get; set; }

	public virtual string rackName { get; set; }
}
