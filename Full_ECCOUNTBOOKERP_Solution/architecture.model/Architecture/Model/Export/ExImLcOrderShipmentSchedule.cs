using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class ExImLcOrderShipmentSchedule
{
	public DateTime AttentiveInspectionDate { get; set; }

	public DateTime AttentiveLoadingDate { get; set; }

	public DateTime AttentiveProductionDate { get; set; }

	public DateTime PackingMaterialDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal NetWeight { get; set; }

	public int ActionId { get; set; }

	public int BranchesId { get; set; }

	public int DocumentTypeId { get; set; }

	public int CompanyId { get; set; }

	public int DestinationPortId { get; set; }

	public int CustomGroupId { get; set; }

	public int EntryUserId { get; set; }

	public int ExImLcOrderId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int NoOfContainer { get; set; }

	public int OrganizationId { get; set; }

	public int FinancialYearId { get; set; }

	public int SortNo { get; set; }

	public int SupplierCustomerId { get; set; }

	public string ScheduleCode { get; set; }

	public string ScheduleRemarks { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public string CustomerContractNo { get; set; }

	public virtual string CustomerName { get; set; }

	public virtual string LcOrderNo { get; set; }

	public virtual string DestinationPort { get; set; }

	public virtual string EntryUser { get; set; }

	public virtual string ModifyUser { get; set; }

	public virtual string Status { get; set; }

	public virtual string RefferedStatus { get; set; }

	public virtual int NoOfAttachments { get; set; }

	public virtual bool ContractApproveStatus { get; set; }

	public virtual List<ExImLcOrderShipmentSchedule> ExImLcOrderShipmentScheduleList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
