using System.Collections.Generic;

namespace Architecture.Model.Export;

public class FcyBankReceiptBreakUp
{
	public int Id { get; set; }

	public int FcyBankReceiptId { get; set; }

	public string SenderRefNo { get; set; }

	public string ReceiverRefNo { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefDocRecordId { get; set; }

	public decimal RealizedAmount { get; set; }

	public decimal AgencyComm { get; set; }

	public string StepStatus { get; set; }

	public int SortNo { get; set; }

	public int ActionTypeId { get; set; }

	public int GDId { get; set; }

	public int GdRefDocTypeId { get; set; }

	public virtual string InvoiceNo { get; set; }

	public virtual string GDNo { get; set; }

	public virtual double InvoiceAmount { get; set; }

	public virtual List<FcyBankReceiptBreakUp> FcyBankReceiptBreakUpList { get; set; }
}
