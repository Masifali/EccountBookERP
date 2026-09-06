using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class DHLTracking
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime DateOfDispatched { get; set; }

	public DateTime FinalETADate { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int ApprovedUserId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public long DHLTrackingId { get; set; }

	public int ActionId { get; set; }

	public int CurrentStatusId { get; set; }

	public int DocumentTypeId { get; set; }

	public int PendingForView { get; set; }

	public string AWBBillNumber { get; set; }

	public string Detail { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
