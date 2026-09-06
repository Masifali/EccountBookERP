using System;
using System.Collections.Generic;

namespace Architecture.Model.Mfg;

public class WorkOrderInProgress
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EndDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime StartDate { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int WorkOrderInProgressId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int WorkOrderId { get; set; }

	public string RemarksHeader { get; set; }

	public string Status { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string WorkOrderNo { get; set; }

	public virtual string BomName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string PackUomCode { get; set; }

	public virtual string PackUomEquivelent { get; set; }

	public virtual List<WorkOrderInProgressDetail> WorkOrderInProgressDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
