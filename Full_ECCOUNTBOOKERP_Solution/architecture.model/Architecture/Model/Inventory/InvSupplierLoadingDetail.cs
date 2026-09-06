namespace Architecture.Model.Inventory;

public class InvSupplierLoadingDetail
{
	public decimal Qty { get; set; }

	public decimal WeightKgs { get; set; }

	public int ActionId { get; set; }

	public int Id { get; set; }

	public int InvSupplierLoadingId { get; set; }

	public int ItemId { get; set; }

	public int OrderId { get; set; }

	public int PackingTypeId { get; set; }

	public int PackSizeId { get; set; }

	public int RateUomId { get; set; }

	public int SortNo { get; set; }

	public string CropYear { get; set; }

	public string Remarks { get; set; }

	public int SupplierId { get; set; }

	public int BrokerId { get; set; }

	public double Rate { get; set; }

	public double Amount { get; set; }

	public virtual string ItemName { get; set; }

	public virtual double PackSize { get; set; }

	public virtual string PackingType { get; set; }

	public virtual int OrderNo { get; set; }

	public virtual string SupplierName { get; set; }

	public virtual string BrokerName { get; set; }
}
