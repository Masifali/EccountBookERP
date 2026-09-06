using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class ExImVCITransaction
{
	public bool IsApproved { get; set; }

	public bool IsContainerAccepted { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int GatePassOutwardId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public string Container1No { get; set; }

	public string Container2No { get; set; }

	public string InspectedBy { get; set; }

	public string InspectorRemarks { get; set; }

	public string PictureTakenBy { get; set; }

	public string Seal1No { get; set; }

	public string Seal2No { get; set; }

	public string VerifiedBy { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<ExImVCITransactionDetail> TransactionDetail { get; set; }

	public virtual List<ExImVCITransaction> ExImVCITransactionList { get; set; }
}
