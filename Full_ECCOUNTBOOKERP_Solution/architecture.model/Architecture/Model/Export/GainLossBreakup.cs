namespace Architecture.Model.Export;

public class GainLossBreakup
{
	public int Id { get; set; }

	public int FcyBankReceiptId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public double FcyAmount { get; set; }

	public double LocalAmount { get; set; }

	public double ExchangeRate { get; set; }

	public string Type { get; set; }

	public string Remarks { get; set; }
}
