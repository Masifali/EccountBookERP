namespace Architecture.Model.Accounts;

public class AccountsReconciliationDetail
{
	public double BalanceDiff { get; set; }

	public double ErpBalance { get; set; }

	public double ManualBalance { get; set; }

	public int AccountsReconiliationId { get; set; }

	public int ChartOfAccountId { get; set; }

	public int Id { get; set; }

	public string Remarks { get; set; }
}
