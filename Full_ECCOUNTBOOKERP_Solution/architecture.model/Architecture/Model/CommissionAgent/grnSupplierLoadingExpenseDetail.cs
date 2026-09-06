namespace Architecture.Model.CommissionAgent;

public class grnSupplierLoadingExpenseDetail
{
	public decimal Qty { get; set; }

	public double amount { get; set; }

	public double rate { get; set; }

	public int grnSupplierLoadingExpenseDetailId { get; set; }

	public int grnSupplierLoadingMasterId { get; set; }

	public int purchaseOrderMasterId { get; set; }

	public int purchaseOrderExpenseDetailId { get; set; }

	public int ItemId { get; set; }

	public int sortNo { get; set; }

	public string remarks { get; set; }

	public virtual int PurchaseOrderNo { get; set; }

	public virtual string OtherItemName { get; set; }
}
