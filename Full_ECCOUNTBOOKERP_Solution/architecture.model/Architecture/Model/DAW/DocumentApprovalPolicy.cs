using System;

namespace Architecture.Model.DAW;

public class DocumentApprovalPolicy
{
	public bool IsActive { get; set; }

	public bool IsFinalApproval { get; set; }

	public bool IsMandatory { get; set; }

	public DateTime EnteryDate { get; set; }

	public decimal LimitAmount { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int OrganizationId { get; set; }

	public int UserId { get; set; }

	public int ApprovalLevelId { get; set; }

	public int DocumentId { get; set; }

	public long Id { get; set; }
}
