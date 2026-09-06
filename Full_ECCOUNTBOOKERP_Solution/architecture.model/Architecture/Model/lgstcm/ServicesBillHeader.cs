using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.lgstcm;

public class ServicesBillHeader
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocumentDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double fcyExchangeRate { get; set; }

	public double transactionExchangeRate { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BilltoPartyId { get; set; }

	public int BranchesId { get; set; }

	public int BrokerOrServiceProviderId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int DueDays { get; set; }

	public int EntryUserId { get; set; }

	public int ExportInvoiceId { get; set; }

	public int fcyCurrencyId { get; set; }

	public int FinancialYearId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int paymentTermId { get; set; }

	public int ProjectsId { get; set; }

	public int ServicesBillHeaderId { get; set; }

	public int serviceTypeId { get; set; }

	public int transactionCurrencyId { get; set; }

	public string ApprovalRemarks { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public string ReferenceNo { get; set; }

	public string RemarksHeader { get; set; }

	public string FreightVoucherOutwardIds { get; set; }

	public int GlcyCurrencyId { get; set; }

	public double GlcyExchangeRate { get; set; }

	public double GlcyAmount { get; set; }

	public virtual string PaymentTerm { get; set; }

	public virtual string ServiceType { get; set; }

	public virtual string BilltoPartyName { get; set; }

	public virtual string ExportInvoiceNo { get; set; }

	public virtual string ExportInvoiceCustomerName { get; set; }

	public virtual string BrokerOrServiceProviderName { get; set; }

	public virtual string TransactionCurrencyCode { get; set; }

	public virtual string GlcyCurrencyCode { get; set; }

	public virtual int InvoiceAccountId { get; set; }

	public virtual List<ServicesBillDetail> ServicesBillDetailList { get; set; }

	public virtual List<ServicesBillFreightVoucherDetail> ServicesBillFreightVoucherDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }
}
