namespace Architecture.Model.CommissionAgent;

public class grnSupplierLoadingEmptyBagDetail
{
	public decimal Rate { get; set; }

	public decimal weightCutKg { get; set; }

	public int grnSupplierLoadingEmptyBagDetailId { get; set; }

	public int grnSupplierLoadingMasterId { get; set; }

	public int purchaseOrderMasterId { get; set; }

	public int purchaseOrderEmptyBagDetailId { get; set; }

	public int PackingTypeId { get; set; }

	public int emptyBagPackingMaterialItemId { get; set; }

	public int sortNo { get; set; }

	public string remarks { get; set; }

	public virtual int PurchaseOrderNo { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string EmptyBagItem { get; set; }
}
