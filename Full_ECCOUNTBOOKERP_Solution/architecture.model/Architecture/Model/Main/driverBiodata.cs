using System;
using System.Collections.Generic;

namespace Architecture.Model.Main;

public class driverBiodata
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchId { get; set; }

	public int DocumentTypeId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int driverBiodataId { get; set; }

	public string alternateCellNo { get; set; }

	public string AttachmentsValues { get; set; }

	public string cellNo { get; set; }

	public string cnicNo { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public string driverFingerPrint { get; set; }

	public string driverName { get; set; }

	public string driverPicture { get; set; }

	public string fatherCnicNo { get; set; }

	public string fatherName { get; set; }

	public string whatsappNo { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
