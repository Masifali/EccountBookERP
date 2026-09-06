namespace Architecture.Model.PartyProcessing;

public class InvProductionProcessingBillOutPut
{
	public decimal Amount { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public decimal Weight { get; set; }

	public int Id { get; set; }

	public int InvProductionProcessingBillId { get; set; }

	public int ItemId { get; set; }

	public int DebitAccountId { get; set; }

	public int SortNo { get; set; }

	public int ItemUomId { get; set; }

	public int ActionTypeId { get; set; }

	public string Remarks { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemUomCode { get; set; }

	public virtual double ItemUomEquivalent { get; set; }

	public virtual string DebitAccount { get; set; }
}
