using System;
using System.Collections.Generic;

namespace Architecture.Model.Mfg;

public class WorkOrder
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DeadLine { get; set; }

	public DateTime DocDate { get; set; }

	public int DocNo { get; set; }

	public DateTime EndDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime StartDate { get; set; }

	public decimal ItemQty { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BomHeaderId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int WorkOrderId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public string BatchDesc { get; set; }

	public string ManualDocCode { get; set; }

	public string RemarksHeader { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string BomName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string PackUomCode { get; set; }

	public virtual string PackUomEquivelent { get; set; }

	public virtual List<WorkOrderCustomer> WorkOrderCustomerList { get; set; }

	public virtual List<WorkOrderOperation> WorkOrderOperationList { get; set; }

	public virtual List<workOrderMaterial> WorkOrderMaterialList { get; set; }

	public virtual List<workOrderItemDetail> workOrderItemDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
