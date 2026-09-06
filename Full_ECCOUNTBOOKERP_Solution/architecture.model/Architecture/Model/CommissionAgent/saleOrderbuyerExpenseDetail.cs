namespace Architecture.Model.CommissionAgent;

public class saleOrderbuyerExpenseDetail
{
	public decimal Qty { get; set; }

	public double amount { get; set; }

	public double rate { get; set; }

	public int ItemId { get; set; }

	public int purchaseOrderMasterId { get; set; }

	public int purchaseOrderSupplierExpenseDetailId { get; set; }

	public int saleOrderbuyerExpenseDetailId { get; set; }

	public int saleOrderMasterId { get; set; }

	public int sortNo { get; set; }

	public string remarks { get; set; }

	public virtual string OtherItemName { get; set; }
}
