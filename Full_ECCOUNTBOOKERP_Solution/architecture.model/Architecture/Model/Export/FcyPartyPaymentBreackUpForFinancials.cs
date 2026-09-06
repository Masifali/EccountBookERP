namespace Architecture.Model.Export;

public class FcyPartyPaymentBreackUpForFinancials
{
	public decimal FBC { get; set; }

	public decimal RealizedAmount { get; set; }

	public decimal TotalAmount { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal LcyAmount { get; set; }

	public int FcyReceiptId { get; set; }

	public int Id { get; set; }
}
