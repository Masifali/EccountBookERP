namespace Architecture.Model.Inventory;

public class PurchaseOrderSupplierExpense
{
	public int Id { get; set; }

	public int PurchaseOrderId { get; set; }

	public int InvRevExpItemId { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public double Amount { get; set; }

	public string Remarks { get; set; }

	public virtual string OtherItemName { get; set; }
}
