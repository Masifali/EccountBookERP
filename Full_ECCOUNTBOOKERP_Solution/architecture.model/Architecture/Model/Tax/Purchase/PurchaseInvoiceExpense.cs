namespace Architecture.Model.Tax.Purchase;

public class PurchaseInvoiceExpense
{
	public double Amount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int Id { get; set; }

	public int OtherItemId { get; set; }

	public int PurchaseInvoiceId { get; set; }

	public string Remarks { get; set; }

	public virtual string OtherItemName { get; set; }
}
