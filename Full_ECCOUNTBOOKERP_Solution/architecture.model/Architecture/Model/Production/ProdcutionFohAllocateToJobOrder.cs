using System;
using System.Collections.Generic;

namespace Architecture.Model.Production;

public class ProdcutionFohAllocateToJobOrder
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime FromDateMonth { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime ToDateMonth { get; set; }

	public double NetAmount { get; set; }

	public int AccountId { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int MethodTypeId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PendingForView { get; set; }

	public int ProjectId { get; set; }

	public int StatusId { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public string Month { get; set; }

	public string RemarksHeader { get; set; }

	public string Status { get; set; }

	public virtual List<ProdcutionFohAllocateToJobOrderDetail> ProdcutionFohAllocateToJobOrderDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
