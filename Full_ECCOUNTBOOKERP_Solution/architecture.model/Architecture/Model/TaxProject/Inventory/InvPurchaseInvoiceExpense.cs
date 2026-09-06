namespace Architecture.Model.TaxProject.Inventory;

public class InvPurchaseInvoiceExpense
{
	public int Id { get; set; }

	public int InvPurchaseInvoiceId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int PurchaseOrderSupplierExpId { get; set; }

	public int GdnId { get; set; }

	public int GdnExpId { get; set; }

	public int InvRevExpItemId { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public double Amount { get; set; }

	public string Remarks { get; set; }

	public string CustomRemarks { get; set; }

	public virtual string OtherItemName { get; set; }

	public virtual int GdnNo { get; set; }
}
