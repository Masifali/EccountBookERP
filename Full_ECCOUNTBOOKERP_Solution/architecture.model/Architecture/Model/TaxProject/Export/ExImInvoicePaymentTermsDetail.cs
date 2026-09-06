namespace Architecture.Model.TaxProject.Export;

public class ExImInvoicePaymentTermsDetail
{
	public double FcyAmount { get; set; }

	public double PrcntOfTotal { get; set; }

	public int DueDays { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int ExImEFormRegistrationId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int Id { get; set; }

	public int PaymentTermId { get; set; }

	public int DocumentTypeId { get; set; }

	public int SortNo { get; set; }

	public string FinancialInstrumentNo { get; set; }

	public string PaymentRemarks { get; set; }

	public virtual string PaymentTerm { get; set; }
}
