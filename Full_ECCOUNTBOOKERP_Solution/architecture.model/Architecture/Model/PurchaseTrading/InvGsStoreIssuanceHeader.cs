using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.PurchaseTrading;

public class InvGsStoreIssuanceHeader
{
	public bool IsApproved { get; set; }

	public DateTime? ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int BranchSrNo { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int RefDocNoId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int DocumentTypeId { get; set; }

	public int FinancialYearId { get; set; }

	public int ExImInvoiceId { get; set; }

	public string Remarks { get; set; }

	public string ManualNo { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public int ActionId { get; set; }

	public int TypeId { get; set; }

	public int BaseDocumentTypeId { get; set; }

	public bool IsUploaded { get; set; }

	public virtual int ActionForVoucherId { get; set; }

	public virtual VoucherHead VoucherHead { get; set; }

	public virtual List<InvGsStoreIssuanceDetail> gsStoreIssuanceDetails { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
