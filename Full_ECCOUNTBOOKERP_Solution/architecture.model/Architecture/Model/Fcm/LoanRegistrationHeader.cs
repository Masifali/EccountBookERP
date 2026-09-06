using System;
using System.Collections.Generic;

namespace Architecture.Model.Fcm;

public class LoanRegistrationHeader
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int LoanCodeNo { get; set; }

	public int loanProductId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public long DocNo { get; set; }

	public long Id { get; set; }

	public long LoanAccountId { get; set; }

	public long LoanAgreementHeaderId { get; set; }

	public long RowVersion { get; set; }

	public string LoanCaseNo { get; set; }

	public string Remarks { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public virtual string LoanAgreementDocNo { get; set; }

	public virtual List<LoanRegistrationDetail> FcmLoanRegistrationDetails { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
