using System;

namespace Architecture.Model.Accounts;

public class PaymentByInvoiceDetail
{
	public DateTime DueDate { get; set; }

	public DateTime InvoiceDate { get; set; }

	public double AdvanceAmount { get; set; }

	public double Amount { get; set; }

	public double BalanceAmount { get; set; }

	public double BillAmount { get; set; }

	public double DAdjustmentAmount { get; set; }

	public double DWhtAmount { get; set; }

	public double TotalPaidAmount { get; set; }

	public int Id { get; set; }

	public int InvoiceNo { get; set; }

	public int PaymentByInvoiceHeaderId { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public string MannualBillNo { get; set; }

	public virtual string DocumentTypeCode { get; set; }
}
