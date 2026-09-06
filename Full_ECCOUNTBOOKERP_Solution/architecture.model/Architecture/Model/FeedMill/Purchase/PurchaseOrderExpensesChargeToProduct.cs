namespace Architecture.Model.FeedMill.Purchase;

public class PurchaseOrderExpensesChargeToProduct
{
	public decimal Amount { get; set; }

	public decimal Percentage { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public int AccountId { get; set; }

	public int Id { get; set; }

	public int PurchaseOrderId { get; set; }

	public int SupplierCustomerId { get; set; }

	public string Remarks { get; set; }

	public virtual string AccountTitle { get; set; }
}
