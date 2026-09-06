using System;
using System.Collections.Generic;

namespace Architecture.Model.Lab;

public class InvLabAnalysisStandardDeductionPolicyHeader
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EffectiveFrom { get; set; }

	public DateTime EfffectiveTo { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public string PolicyApplyOn { get; set; }

	public string PolicyName { get; set; }

	public string RemarksHeader { get; set; }

	public virtual string ItemName { get; set; }

	public virtual List<InvLabAnalysisStandardDeductionPolicyDetail> LabAnalysisStandardDetail { get; set; }
}
