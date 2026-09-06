using System;

namespace Architecture.Model.Inventory;

public class CommisionAgentBillPaymentDetail
{
	public DateTime? DueDate { get; set; }

	public decimal dueAmount { get; set; }

	public decimal pctOfTotal { get; set; }

	public int BaseDueDateTypeId { get; set; }

	public int DueDays { get; set; }

	public int EntrySideId { get; set; }

	public int Id { get; set; }

	public int InvCommAgentBillTradeId { get; set; }

	public int PaymentTermId { get; set; }

	public int purchaseOrderMasterId { get; set; }

	public int saleOrderMasterId { get; set; }

	public int sortNo { get; set; }

	public string remarks { get; set; }

	public virtual string PaymentTerm { get; set; }

	public virtual string BaseDateType { get; set; }

	public virtual int PurchaseOrderNo { get; set; }

	public virtual DateTime? PurchaseOrderDate { get; set; }

	public virtual int SaleOrderNo { get; set; }

	public virtual DateTime? SaleOrderDate { get; set; }
}
