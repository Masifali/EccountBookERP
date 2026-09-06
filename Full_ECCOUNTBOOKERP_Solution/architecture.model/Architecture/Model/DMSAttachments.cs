using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class DMSAttachments
{
	public int OrganizationId { get; set; }

	public int RefDocumentNo { get; set; }

	public int CompanyId { get; set; }

	public int ModifyUser { get; set; }

	public string Attachment { get; set; }

	public int BranchId { get; set; }

	public int Id { get; set; }

	public DateTime EntryDate { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int RefAccountId { get; set; }

	public int DMSFoldersLabelsId { get; set; }

	public DateTime ModifyDate { get; set; }

	public int EntryUser { get; set; }

	public string ScreenName { get; set; }

	public virtual List<DMSAttachments> AttachmentList { get; set; }

	public string UploadedFileCustomName { get; set; }

	public double UploadedFileSizeMb { get; set; }

	public bool DetailWiseAttachment { get; set; }

	public int LineId { get; set; }

	public virtual string EntryUserName { get; set; }

	public virtual string FileCustomName { get; set; }
}
