namespace Architecture.Model.PartyProcessing;

public class InvProductionProcessingBillOH
{
	public decimal Amount { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public int AccountId { get; set; }

	public int Id { get; set; }

	public int InvProductionProcessingBillId { get; set; }

	public string OHRemarks { get; set; }

	public int SortNo { get; set; }

	public virtual string AccountTitle { get; set; }
}
