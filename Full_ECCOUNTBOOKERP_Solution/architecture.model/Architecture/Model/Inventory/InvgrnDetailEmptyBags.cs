namespace Architecture.Model.Inventory;

public class InvgrnDetailEmptyBags
{
	public double PurchaseQty { get; set; }

	public double ReceivedQty { get; set; }

	public int Id { get; set; }

	public int InvGrnId { get; set; }

	public int ItemId { get; set; }

	public int TypeId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int BagsCondition { get; set; }

	public string Remarks { get; set; }
}
