namespace Architecture.Model.Inventory;

public class PurchaseOrderSupplierDispatchDetail
{
	public double Amount { get; set; }

	public double ItemQty { get; set; }

	public double ItemRate { get; set; }

	public double NetWeight { get; set; }

	public int CropYearId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUOMId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int RateUOMId { get; set; }

	public int SupplierDispatchId { get; set; }

	public int LoadingDetailId { get; set; }

	public int PackingTypeId { get; set; }

	public virtual int SupplierDispatchNo { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string ItemUomCode { get; set; }

	public virtual string RateUomCode { get; set; }

	public virtual string PackingType { get; set; }

	public virtual double ItemUomEquivalent { get; set; }

	public virtual double RateUomEquivalent { get; set; }
}
