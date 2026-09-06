using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class GatePassOutwardDriverInfo
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int BranchId { get; set; }

	public int EntryUser { get; set; }

	public int GatePassOutwardId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public string AlternateCellNo { get; set; }

	public string CnicNo { get; set; }

	public string DriverCellNo { get; set; }

	public string DriverName { get; set; }

	public string DriverPic { get; set; }

	public string FatherName { get; set; }

	public string ForwarderName { get; set; }

	public string RemarksHeader { get; set; }

	public string ThumbPic { get; set; }

	public string ScreenName { get; set; }

	public byte[] DriverImage { get; set; }

	public byte[] FingerPrintImage { get; set; }

	public int DocumentTypeId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public virtual DateTime GpDate { get; set; }

	public virtual List<DMSAttachments> dmsAttachment { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
