namespace Architecture.Model.ModernHistoryModels;

public class VoucherHistory_Detail
{
	public int VoucherDetailId { get; set; }

	public int VoucherHeadId { get; set; }

	public int DetailAccountId { get; set; }

	public string DetailAccountTitle { get; set; }

	public int AgainstAccountId { get; set; }

	public string OffsetAccountTitle { get; set; }

	public double DebitAmount { get; set; }

	public double CreditAmount { get; set; }

	public string Comments { get; set; }

	public string Job_Lot { get; set; }

	public string IsTaxable { get; set; }

	public string TaxType { get; set; }

	public double TaxPrcnt { get; set; }

	public double TaxesTotalAmount { get; set; }
}
