using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.Export;

public class ExportReturnInvoice
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal LcyAmount { get; set; }

	public decimal AddLessAmount { get; set; }

	public decimal FcyAmountWithOutAddLess { get; set; }

	public decimal NetWeight { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int ContractId { get; set; }

	public int CurrencyId { get; set; }

	public int DebitAccounId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PendingForView { get; set; }

	public int ProjectsId { get; set; }

	public int SupplierCustomerId { get; set; }

	public string RemarksHeader { get; set; }

	public virtual string InvoiceNo { get; set; }

	public virtual DateTime InvoiceDate { get; set; }

	public virtual string CustomerName { get; set; }

	public virtual string LcOrderNo { get; set; }

	public virtual string DebitAccountTitle { get; set; }

	public virtual string CurrencyName { get; set; }

	public virtual List<ExportReturnInvoiceDetail> ExportReturnInvoiceDetailList { get; set; }

	public virtual List<ExportReturnInvoicePaymentDetail> ExportReturnInvoicePaymentDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual VoucherHead VoucherHeadForInvoice { get; set; }
}
