namespace Architecture.Model.Export;

public class FcyBankReceiptBreakUpofPartyInvoices
{
	public int Id { get; set; }

	public int FcyBankReceiptId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocId { get; set; }

	public decimal RealizedAmount { get; set; }

	public int SortNo { get; set; }
}
