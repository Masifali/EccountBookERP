using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class ExportContractSchedule
{
	public bool IsApproved { get; set; }

	public int RecordIdentityId { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime LoadingDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal ItemQty { get; set; }

	public decimal MTon { get; set; }

	public decimal NetWeight { get; set; }

	public decimal NoOfContainers { get; set; }

	public int ActionId { get; set; }

	public int DocumentTypeId { get; set; }

	public int ApprovedUserId { get; set; }

	public int CompanyId { get; set; }

	public int ContractDetailId { get; set; }

	public int ContractId { get; set; }

	public int CustomerId { get; set; }

	public int DestinationPortId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PackUomId { get; set; }

	public int PendingForView { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public string Remarks { get; set; }

	public string Status { get; set; }

	public virtual string CustomerName { get; set; }

	public virtual string ContractNo { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double PackEquivalent { get; set; }

	public virtual string PackingType { get; set; }

	public virtual double BalMTon { get; set; }

	public virtual double ContractMTon { get; set; }

	public virtual double ContractNoOfContainers { get; set; }

	public virtual List<ExportContractSchedule> ExportContractScheduleList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
