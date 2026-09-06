namespace Architecture.Model.pcc;

public class InvSaleInvoiceExpense
{
	public decimal Amount { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public int Id { get; set; }

	public int InvOtherItemId { get; set; }

	public int InvSaleInvoiceId { get; set; }

	public string Remarks { get; set; }

	public virtual string OtherItemName { get; set; }
}
