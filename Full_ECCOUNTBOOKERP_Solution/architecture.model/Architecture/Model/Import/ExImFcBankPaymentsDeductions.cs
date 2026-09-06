namespace Architecture.Model.Import;

public class ExImFcBankPaymentsDeductions
{
	public double FcAmount { get; set; }

	public double RsAmount { get; set; }

	public int ExImFCBankPaymentsId { get; set; }

	public int ExImProceedsChargesTypeId { get; set; }

	public int Id { get; set; }

	public string ProceedsRemarks { get; set; }

	public string Type { get; set; }

	public virtual string ProceedsChargesTypeCode { get; set; }
}
