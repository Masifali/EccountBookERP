namespace Architecture.Model.Inventory;

public class SupplierDispatchExpense
{
	public double Amount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int Id { get; set; }

	public int OrderId { get; set; }

	public int OrderExpId { get; set; }

	public int OtherItemId { get; set; }

	public int SupplierDispatchId { get; set; }

	public string Remarks { get; set; }

	public virtual string OtherItemName { get; set; }
}
