namespace Architecture.Model.Steel.Inventory;

public class InvPurchaseInvoiceExpense
{
	public decimal Amount { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public int Id { get; set; }

	public int InvOtherItemId { get; set; }

	public int InvPurchaseInvoiceId { get; set; }

	public string Remarks { get; set; }

	public virtual string OtherItemName { get; set; }
}
