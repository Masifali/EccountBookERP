namespace Architecture.Model.Accounts;

public class VoucherDetailMultiCurrency
{
	public double ExchangeRate { get; set; }

	public int GlCurrencyId { get; set; }

	public int Id { get; set; }

	public int LocalCurrencyId { get; set; }

	public int VoucherHeadId { get; set; }

	public virtual string LcyCurrencySymbol { get; set; }

	public virtual string LcyCurrencyCode { get; set; }

	public virtual string GlFcyCurrencySymbol { get; set; }

	public virtual string GlFcyCurrencyCode { get; set; }
}
