namespace Architecture.Model.Inventory;

public class PurchaseOrderExpensesChargeToProduct
{
	public double Amount { get; set; }

	public double Percentage { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int AccountId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int Id { get; set; }

	public int PurchaseOrderId { get; set; }

	public string Remarks { get; set; }

	public virtual string AccountTitle { get; set; }
}
