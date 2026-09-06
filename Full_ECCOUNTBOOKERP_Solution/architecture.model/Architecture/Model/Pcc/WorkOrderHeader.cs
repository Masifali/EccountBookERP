using System;
using System.Collections.Generic;

namespace Architecture.Model.pcc;

public class WorkOrderHeader
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DeadLine { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EndDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime StartDate { get; set; }

	public double LeadTime { get; set; }

	public decimal TopForce { get; set; }

	public decimal BottomForce { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProductionTypeId { get; set; }

	public int PlantId { get; set; }

	public int BomHeaderId { get; set; }

	public int ProjectsId { get; set; }

	public int WorkOrderHeaderId { get; set; }

	public string AttachmentsValues { get; set; }

	public string BatchDesc { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public string RemarksHeader { get; set; }

	public string Status { get; set; }

	public string WorkOrderNo { get; set; }

	public virtual string ProductionType { get; set; }

	public virtual List<WorkOrderCustomerDetail> WorkOrderCustomerDetailList { get; set; }

	public virtual List<WorkOrderInputDetail> WorkOrderInputDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
