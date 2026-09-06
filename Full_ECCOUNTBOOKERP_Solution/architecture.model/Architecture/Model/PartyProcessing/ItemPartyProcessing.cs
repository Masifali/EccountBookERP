using System;
using System.Collections.Generic;
using Architecture.Model.Inventory;

namespace Architecture.Model.PartyProcessing;

public class ItemPartyProcessing
{
	public bool IsActive { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ItemBaseUnitId { get; set; }

	public int ItemCategoryId { get; set; }

	public int ItemTypeId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int UomGroupId { get; set; }

	public string ItemCode { get; set; }

	public string ItemName { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<ItemAllocation> ItemAllocationlist { get; set; }
}
