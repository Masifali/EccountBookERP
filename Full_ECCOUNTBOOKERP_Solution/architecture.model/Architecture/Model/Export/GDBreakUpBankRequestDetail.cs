namespace Architecture.Model.Export;

public class GDBreakUpBankRequestDetail
{
	public decimal BankReceivedAmount { get; set; }

	public decimal CommAmount { get; set; }

	public decimal RealizedAmount { get; set; }

	public int ActionTypeId { get; set; }

	public int GDBreakUpBankRequestHeaderId { get; set; }

	public int GDId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int Id { get; set; }

	public int SubCode { get; set; }

	public string RemarksDetail { get; set; }

	public string Status { get; set; }

	public virtual string GDNo { get; set; }

	public virtual decimal GdValue { get; set; }

	public virtual decimal FobValue { get; set; }

	public virtual decimal GdBalance { get; set; }
}
