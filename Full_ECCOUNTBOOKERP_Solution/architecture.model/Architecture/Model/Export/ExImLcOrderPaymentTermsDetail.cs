namespace Architecture.Model.Export;

public class ExImLcOrderPaymentTermsDetail
{
	public double FcyAmount { get; set; }

	public double PrcntOfTotal { get; set; }

	public int DueDays { get; set; }

	public int ExportDueTypeId { get; set; }

	public int Id { get; set; }

	public int LcOrderId { get; set; }

	public int PaymentTermId { get; set; }

	public int SortNo { get; set; }

	public string Remarks { get; set; }

	public virtual string LcOrderTerm { get; set; }
}
