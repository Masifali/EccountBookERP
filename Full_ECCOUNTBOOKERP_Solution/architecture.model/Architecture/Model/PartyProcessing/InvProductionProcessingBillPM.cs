namespace Architecture.Model.PartyProcessing;

public class InvProductionProcessingBillPM
{
	public decimal Amount { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public int Id { get; set; }

	public int InvProductionProcessingBillId { get; set; }

	public int ItemId { get; set; }

	public int SortNo { get; set; }

	public string Remarks { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }
}
