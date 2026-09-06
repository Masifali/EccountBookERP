using System;
using System.Collections.Generic;

namespace Architecture.Model.SystemUtilities;

public class AttachmentsHistory
{
	public int Id { get; set; }

	public int FolderId { get; set; }

	public string Tags { get; set; }

	public int ReferenceDocumentTypeId { get; set; }

	public int ReferenceDocumentId { get; set; }

	public string ScreenName { get; set; }

	public string UploadedFileName { get; set; }

	public string UploadedFileType { get; set; }

	public double UploadedFileSize_Mb { get; set; }

	public DateTime UploadedFileLastModifiedDate { get; set; }

	public string UploadedFileCustomName { get; set; }

	public bool IsSelected { get; set; }

	public bool IsCompressed { get; set; }

	public float CompressionPercent { get; set; }

	public double CompressedFileSize_Mb { get; set; }

	public int UploadUserId { get; set; }

	public DateTime UploadDate { get; set; }

	public int ModifyUserId { get; set; }

	public DateTime ModifyDate { get; set; }

	public bool IsRequested { get; set; }

	public int RequestedUserId { get; set; }

	public DateTime RequestedDate { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }

	public int ProjectId { get; set; }

	public int FinancialYearId { get; set; }

	public int OwnerId { get; set; }

	public int ActionTypeId { get; set; }

	public virtual List<AttachmentsHistory> AttachmentHistoryList { get; set; }
}
