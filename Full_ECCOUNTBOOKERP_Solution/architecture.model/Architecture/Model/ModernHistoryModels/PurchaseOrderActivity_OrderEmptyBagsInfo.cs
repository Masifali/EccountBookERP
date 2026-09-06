namespace Architecture.Model.ModernHistoryModels;

public class PurchaseOrderActivity_OrderEmptyBagsInfo
{
	public double Rate { get; set; }

	public double WeightCut { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int PackingTypeId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int Type { get; set; }

	public string ItemName { get; set; }

	public string PackTypeDesc { get; set; }

	public string EmptyBagsType { get; set; }
}
