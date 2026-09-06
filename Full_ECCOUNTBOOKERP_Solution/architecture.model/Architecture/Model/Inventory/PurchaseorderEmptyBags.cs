namespace Architecture.Model.Inventory;

public class PurchaseorderEmptyBags
{
	public double Rate { get; set; }

	public double WeightCut { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int PackingTypeId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int Type { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string EmptyBagsType { get; set; }
}
