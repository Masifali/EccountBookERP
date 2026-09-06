namespace Architecture.Model.CommissionAgent;

public class gdnBuyerDispatchExpenseDetail
{
	public decimal Qty { get; set; }

	public double amount { get; set; }

	public double rate { get; set; }

	public int gdnBuyerDispatchExpenseDetailId { get; set; }

	public int gdnBuyerDispatchMasterId { get; set; }

	public int ItemId { get; set; }

	public int saleOrderExpenseDetailId { get; set; }

	public int saleOrderMasterId { get; set; }

	public int sortNo { get; set; }

	public string remarks { get; set; }

	public virtual int SaleOrderNo { get; set; }

	public virtual string OtherItemName { get; set; }
}
