using System;

namespace Architecture.Model.Inventory;

public class CommisionAgentBillSaleExpense
{
	public double Amount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int Id { get; set; }

	public int InvCommAgentBillTradeId { get; set; }

	public int InvExpItemId { get; set; }

	public string Remarks { get; set; }

	public int gdnBuyerDispatchMasterId { get; set; }

	public int saleOrderMasterId { get; set; }

	public virtual string OtherItemName { get; set; }

	public virtual int SaleOrderNo { get; set; }

	public virtual DateTime? SaleOrderDate { get; set; }

	public virtual int GdnNo { get; set; }

	public virtual DateTime? GdnDate { get; set; }
}
