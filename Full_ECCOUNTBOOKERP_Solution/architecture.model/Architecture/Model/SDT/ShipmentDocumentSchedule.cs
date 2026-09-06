using System;
using System.Collections.Generic;

namespace Architecture.Model.SDT;

public class ShipmentDocumentSchedule
{
	public bool IsApproved { get; set; }

	public bool RefDocLockStatus { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime BaseCriteriaDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime DueDate { get; set; }

	public DateTime ReadyDate { get; set; }

	public int DocStatus { get; set; }

	public int afterDays { get; set; }

	public int ApprovedUserId { get; set; }

	public int beforeDays { get; set; }

	public int chartOfDocumentId { get; set; }

	public int CompanyId { get; set; }

	public int duplicate { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int original { get; set; }

	public int prioritySeqNo { get; set; }

	public int refDocumentId { get; set; }

	public int referenceDocumentTypeId { get; set; }

	public int shipmentDocumentScheduleId { get; set; }

	public string documentPath { get; set; }

	public string Remarks { get; set; }

	public virtual string ReferenceDocumentType { get; set; }

	public virtual int ReferenceDocumentNo { get; set; }

	public virtual int RefDocCustomGroupId { get; set; }

	public virtual string DocumentName { get; set; }

	public virtual List<ShipmentDocumentSchedule> ShipmentDocumentScheduleList { get; set; }

	public virtual List<DMSAttachments> DetailAttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
