using System;
using System.Collections.Generic;

namespace Architecture.Model.MRP;

public class ItemAndPMItemMap
{
	public bool IsActive { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int BaseUomId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int ItemAndPMItemMapId { get; set; }

	public int ItemId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public long ItemUomCkId { get; set; }

	public string RemarksHeader { get; set; }

	public string ScreenName { get; set; }

	public virtual List<ItemAndPMItemMapDetail> ItemAndPMItemMapDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
