namespace Architecture.Model.Export;

public class ExImEFormRegistrationAdvancePaymentUtilizationDetail
{
	public double FcUtilizedAmount { get; set; }

	public int ExImEFormRegistrationId { get; set; }

	public int ExImFCBankReceiptsId { get; set; }

	public int FcCurrencyId { get; set; }

	public int Id { get; set; }

	public string Remarks { get; set; }

	public virtual string CurrencyCode { get; set; }

	public virtual string BankFbpNo { get; set; }
}
