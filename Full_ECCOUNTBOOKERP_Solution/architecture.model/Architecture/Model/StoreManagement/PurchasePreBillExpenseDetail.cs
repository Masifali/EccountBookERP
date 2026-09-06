namespace Architecture.Model.StoreManagement;

public class PurchasePreBillExpenseDetail
{
	public double Amount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int PurchasePreBillExpenseDetailId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int PurchaseOrderExpId { get; set; }

	public int OtherItemId { get; set; }

	public int PurchasePreBillHeaderId { get; set; }

	public string Remarks { get; set; }

	public virtual string OtherItemName { get; set; }
}
