namespace Architecture.Model.TaxProject.Inventory;

public class InvSaleInvoiceExpense
{
	public double Amount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int Id { get; set; }

	public int InvRevExpItemId { get; set; }

	public int InvSaleInvoiceId { get; set; }

	public string Remarks { get; set; }

	public string CustomRemarks { get; set; }

	public virtual string OtherItemName { get; set; }
}
