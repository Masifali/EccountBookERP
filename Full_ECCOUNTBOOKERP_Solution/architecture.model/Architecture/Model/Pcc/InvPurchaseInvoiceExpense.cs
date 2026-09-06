namespace Architecture.Model.pcc;

public class InvPurchaseInvoiceExpense
{
	public decimal Amount { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public int Id { get; set; }

	public int InvPurchaseInvoiceId { get; set; }

	public int OtherItemId { get; set; }

	public string Remarks { get; set; }

	public virtual string OtherItemName { get; set; }
}
