using System;

namespace Architecture.Model.Inventory;

public class CommisionAgentBillSaleExpenseCreditToReleventAc
{
	public double Amount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int Id { get; set; }

	public int InvCommAgentBillTradeId { get; set; }

	public int AccountId { get; set; }

	public string Remarks { get; set; }

	public double DebitAmount { get; set; }

	public int gdnBuyerDispatchMasterId { get; set; }

	public int saleOrderMasterId { get; set; }

	public int PmItemId { get; set; }

	public int PackingTypeId { get; set; }

	public int EBWeightDeductionTermId { get; set; }

	public virtual string PmItemName { get; set; }

	public virtual string PmItemCode { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string EBWeightDeductionTerm { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual int SaleOrderNo { get; set; }

	public virtual DateTime? SaleOrderDate { get; set; }

	public virtual int GdnNo { get; set; }

	public virtual DateTime? GdnDate { get; set; }
}
