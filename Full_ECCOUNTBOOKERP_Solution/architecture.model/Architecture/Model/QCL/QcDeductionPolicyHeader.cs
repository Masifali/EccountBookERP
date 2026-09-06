using System;
using System.Collections.Generic;

namespace Architecture.Model.QCL;

public class QcDeductionPolicyHeader
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EffectiveFrom { get; set; }

	public DateTime EfffectiveTo { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int QcDeductionPolicyHeaderId { get; set; }

	public int InvLabGroupAnalysisStandardsId { get; set; }

	public string PolicyName { get; set; }

	public string RemarksHeader { get; set; }

	public virtual List<QcDeductionPolicyDetail> QcDeductionPolicyDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
