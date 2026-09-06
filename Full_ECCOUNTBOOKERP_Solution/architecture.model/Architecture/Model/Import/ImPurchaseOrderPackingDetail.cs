namespace Architecture.Model.Import;

public class ImPurchaseOrderPackingDetail
{
	public double GrossWeight { get; set; }

	public double ItemAmount { get; set; }

	public double ItemRate { get; set; }

	public double NetWeight { get; set; }

	public double NoOfPacks { get; set; }

	public double PackingWeight { get; set; }

	public double Qty { get; set; }

	public double HomeCurrencyAmount { get; set; }

	public int ExImLcOrderPurchaseOrderId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int PackingTypeId { get; set; }

	public int RateUomId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PackingType { get; set; }

	public virtual double RateUom { get; set; }

	public virtual double ItemUOM { get; set; }
}
