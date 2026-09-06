using System;

namespace Architecture.Model.Inventory;

public class CommisionAgentBillFreightDetail
{
	public int Id { get; set; }

	public int InvCommAgentBillTradeId { get; set; }

	public int EntrySideId { get; set; }

	public int AccountId { get; set; }

	public int grnSupplierLoadingMasterId { get; set; }

	public int gdnBuyerDispatchMasterId { get; set; }

	public decimal Amount { get; set; }

	public decimal AddLessAmount { get; set; }

	public decimal NetAmount { get; set; }

	public int sortNo { get; set; }

	public string Remarks { get; set; }

	public virtual string AccountBusinessName { get; set; }

	public virtual string AccountNickName { get; set; }

	public virtual string AccountPartyCode { get; set; }

	public virtual int GrnNo { get; set; }

	public virtual DateTime? GrnDate { get; set; }

	public virtual int GdnNo { get; set; }

	public virtual DateTime? GdnDate { get; set; }
}
