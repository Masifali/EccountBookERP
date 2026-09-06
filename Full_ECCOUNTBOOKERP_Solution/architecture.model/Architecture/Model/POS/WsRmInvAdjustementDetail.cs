namespace Architecture.Model.POS;

public class WsRmInvAdjustementDetail
{
	public decimal CostPrice { get; set; }

	public decimal CurrStock { get; set; }

	public decimal QtyInOut { get; set; }

	public int AdjustmentReason { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int SubLineNo { get; set; }

	public int WsRm_InvAdjustementHeaderId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PackUom { get; set; }
}
