namespace Architecture.Model.ContractorWages;

public class InvWagesSceduleForAtuoCalculationDetail
{
	public int Id { get; set; }

	public int InvConractorWagesAccountsId { get; set; }

	public int InvWagesSceduleForAtuoCalculationHeaderId { get; set; }

	public int SortNo { get; set; }

	public int TransactionsTypeId { get; set; }

	public virtual string WagesAccountName { get; set; }
}
