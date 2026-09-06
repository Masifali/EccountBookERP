namespace Architecture.Model.FeedMill.Purchase;

public class InvPurchaseInvoiceEmptyBags
{
	public decimal Amount { get; set; }

	public decimal PurchaseQty { get; set; }

	public decimal Rate { get; set; }

	public decimal ReceivedQty { get; set; }

	public int CreditAccountId { get; set; }

	public int Id { get; set; }

	public int InvGrnEmptyBagId { get; set; }

	public int InvGrnId { get; set; }

	public int InvPurchaseInvoiceId { get; set; }

	public int ItemId { get; set; }

	public int PurchaseOrderEmptyBagId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int RateUomId { get; set; }

	public int TypeId { get; set; }

	public int WarehouseId { get; set; }

	public string Remarks { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual string EmptyBagsType { get; set; }
}
