using System;
using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class ReceiptByInvoiceHeader
{
	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TaxTypeId { get; set; }

	public int TaxDebitAcId { get; set; }

	public int TaxCreditAcId { get; set; }

	public int FinancialYearId { get; set; }

	public double TaxPrct { get; set; }

	public double TaxAmount { get; set; }

	public virtual VoucherHead VoucherHeadPaymenet { get; set; }

	public virtual List<ReceiptByInvoiceDetail> ReceiptByInvoiceDetailList { get; set; }

	public virtual List<ReceiptByInvoiceOtherDetail> ReceiptByInvoiceOtherDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
