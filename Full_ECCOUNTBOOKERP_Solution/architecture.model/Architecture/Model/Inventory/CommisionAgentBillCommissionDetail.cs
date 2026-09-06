using System;

namespace Architecture.Model.Inventory;

public class CommisionAgentBillCommissionDetail
{
	public decimal commissionAmount { get; set; }

	public decimal commissionRate { get; set; }

	public int agentTypeId { get; set; }

	public int commissionAgentId { get; set; }

	public int commissionTypeId { get; set; }

	public int EntrySideId { get; set; }

	public int Id { get; set; }

	public int InvCommAgentBillTradeId { get; set; }

	public int purchaseOrderMasterId { get; set; }

	public int rateUomId { get; set; }

	public int debitAccountId { get; set; }

	public int saleOrderMasterId { get; set; }

	public int sortNo { get; set; }

	public string commissionRemarks { get; set; }

	public virtual string AgentType { get; set; }

	public virtual string CommissionAgentName { get; set; }

	public virtual string CommissionType { get; set; }

	public virtual string EntrySide { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual double RateUom { get; set; }

	public virtual int SaleOrderNo { get; set; }

	public virtual DateTime? SaleOrderDate { get; set; }

	public virtual int PurchaseOrderNo { get; set; }

	public virtual DateTime? PurchaseOrderDate { get; set; }
}
