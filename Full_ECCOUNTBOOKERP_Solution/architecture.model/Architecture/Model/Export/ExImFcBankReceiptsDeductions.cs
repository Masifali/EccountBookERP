namespace Architecture.Model.Export;

public class ExImFcBankReceiptsDeductions
{
	public double FcAmount { get; set; }

	public double RsAmount { get; set; }

	public double ChargesPercent { get; set; }

	public int ExImFCBankReceiptsId { get; set; }

	public int ExImProceedsChargesTypeId { get; set; }

	public int Id { get; set; }

	public string FDBCNo { get; set; }

	public string ProceedsRemarks { get; set; }

	public string Type { get; set; }

	public virtual string ProceedsChargesTypeCode { get; set; }
}
