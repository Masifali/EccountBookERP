namespace Architecture.Model.StoreManagement;

public class DeliveryChallanExpenseDetail
{
	public double Amount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int DeliveryChallanExpenseDetailId { get; set; }

	public int DeliveryChallanHeaderId { get; set; }

	public int OtherItemId { get; set; }

	public int PurchasePreBillExpenseDetailId { get; set; }

	public int PurchasePreBillHeaderId { get; set; }

	public string Remarks { get; set; }

	public virtual string OtherItemName { get; set; }

	public virtual int PreBillNo { get; set; }
}
