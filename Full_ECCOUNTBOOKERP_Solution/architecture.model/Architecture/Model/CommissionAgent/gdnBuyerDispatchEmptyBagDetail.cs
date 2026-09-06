namespace Architecture.Model.CommissionAgent;

public class gdnBuyerDispatchEmptyBagDetail
{
	public decimal Rate { get; set; }

	public decimal weightCutKg { get; set; }

	public int gdnBuyerDispatchEmptyBagDetailId { get; set; }

	public int gdnBuyerDispatchMasterId { get; set; }

	public int PackingTypeId { get; set; }

	public int emptyBagPackingMaterialItemId { get; set; }

	public int saleOrderEmptyBagDetailId { get; set; }

	public int saleOrderMasterId { get; set; }

	public int sortNo { get; set; }

	public string remarks { get; set; }

	public virtual int SaleOrderNo { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string EmptyBagItem { get; set; }
}
