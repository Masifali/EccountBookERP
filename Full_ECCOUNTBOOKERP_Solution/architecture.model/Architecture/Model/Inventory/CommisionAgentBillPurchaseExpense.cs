using System;

namespace Architecture.Model.Inventory;

public class CommisionAgentBillPurchaseExpense
{
	public double Amount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int Id { get; set; }

	public int InvCommAgentBillTradeId { get; set; }

	public int InvExpItemId { get; set; }

	public string Remarks { get; set; }

	public int grnSupplierLoadingMasterId { get; set; }

	public int purchaseOrderMasterId { get; set; }

	public virtual string OtherItemName { get; set; }

	public virtual int PurchaseOrderNo { get; set; }

	public virtual DateTime? PurchaseOrderDate { get; set; }

	public virtual int GrnNo { get; set; }

	public virtual DateTime? GrnDate { get; set; }
}
