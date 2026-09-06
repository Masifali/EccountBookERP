namespace Architecture.Model.Inventory;

public class InvPurchaseInvoiceEmptyBags
{
	public double Amount { get; set; }

	public double PurchaseQty { get; set; }

	public double ReceivedQty { get; set; }

	public double Rate { get; set; }

	public int Id { get; set; }

	public int InvGrnId { get; set; }

	public int InvPurchaseInvoiceId { get; set; }

	public int ItemId { get; set; }

	public int TypeId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int CreditAccountId { get; set; }

	public int ItemConditionId { get; set; }

	public string Remarks { get; set; }

	public string CustomRemarks { get; set; }

	public virtual string ItemCondition { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual string EmptyBagsType { get; set; }
}
