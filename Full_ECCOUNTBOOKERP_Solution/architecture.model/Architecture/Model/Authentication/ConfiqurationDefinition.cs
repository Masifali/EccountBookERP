namespace Architecture.Model.Authentication;

public class ConfiqurationDefinition
{
	public int? BaseCurrency { get; set; }

	public int? CashControlAccount { get; set; }

	public int? BankControlAccount { get; set; }

	public bool? CheqBookEnabled { get; set; }

	public bool? isMinusbalanceAllowed { get; set; }

	public string SendSMSPaymentVoucherValueFrom { get; set; }

	public string SendSMSReceiptsVoucherValueFrom { get; set; }

	public string ReportsFolderPath { get; set; }
}
