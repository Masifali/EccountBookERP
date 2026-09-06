namespace Architecture.Model.Accounts;

public class VouchersAdvanceAdjustedAccountsDetail
{
	public double AdjustedAmount { get; set; }

	public double AvailableBalance { get; set; }

	public int AccountId { get; set; }

	public int Id { get; set; }

	public int VouchersAdvanceAdjustmentByInvoiceId { get; set; }

	public string RemarksAdjustAccounts { get; set; }
}
