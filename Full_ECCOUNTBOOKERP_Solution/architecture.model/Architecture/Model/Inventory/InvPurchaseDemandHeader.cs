using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class InvPurchaseDemandHeader
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int ParentCategoryId { get; set; }

	public int DocNo { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int FinancialYearId { get; set; }

	public int DocumentTypeId { get; set; }

	public string RemarksHeader { get; set; }

	public string Status { get; set; }

	public virtual string DetailIdsToDelete { get; set; }

	public virtual List<InvPurchaseDemandDetail> invPurchaseDemandDetails { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
