using System;
using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class AccountsPrematureReceipts
{
	public bool IsApproved { get; set; }

	public DateTime? ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EnteryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime? ChequeDate { get; set; }

	public decimal Amount { get; set; }

	public double SlipAmount { get; set; }

	public int ApprovalUserId { get; set; }

	public int BankId { get; set; }

	public int BranchId { get; set; }

	public int ChartOfAccountIdReceiver { get; set; }

	public int ChartOfAccountIdSender { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EnteryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int SupplierCustomerIdSalesMan { get; set; }

	public int VouchersId { get; set; }

	public int SenderPartyId { get; set; }

	public string EntryStatus { get; set; }

	public string RemarksHeader { get; set; }

	public string TrackingSlipRef { get; set; }

	public string ChequeNo { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public int FinancialYearId { get; set; }

	public int CostCenterId { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<AccountsPrematureReceipts> AccountsPrematureReceiptsList { get; set; }
}
