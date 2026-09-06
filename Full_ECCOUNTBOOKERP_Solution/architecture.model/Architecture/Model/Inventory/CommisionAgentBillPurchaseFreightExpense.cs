using System;

namespace Architecture.Model.Inventory;

public class CommisionAgentBillPurchaseFreightExpense
{
	public decimal Amount { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public int AccountId { get; set; }

	public int Id { get; set; }

	public int InvCommAgentBillTradeId { get; set; }

	public string Remarks { get; set; }

	public double DebitAmount { get; set; }

	public int grnSupplierLoadingMasterId { get; set; }

	public int purchaseOrderMasterId { get; set; }

	public int PmItemId { get; set; }

	public int PackingTypeId { get; set; }

	public int EBWeightDeductionTermId { get; set; }

	public virtual string PmItemName { get; set; }

	public virtual string PmItemCode { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string EBWeightDeductionTerm { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual int PurchaseOrderNo { get; set; }

	public virtual DateTime? PurchaseOrderDate { get; set; }

	public virtual int GrnNo { get; set; }

	public virtual DateTime? GrnDate { get; set; }
}
