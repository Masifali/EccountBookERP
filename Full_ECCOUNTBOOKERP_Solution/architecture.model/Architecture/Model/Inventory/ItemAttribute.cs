using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class ItemAttribute
{
	public bool isActive { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int companyId { get; set; }

	public int EntryUserId { get; set; }

	public int itemId { get; set; }

	public int ModifyUserId { get; set; }

	public int organizationId { get; set; }

	public int seqNo { get; set; }

	public long attributeId { get; set; }

	public long itemAttributeId { get; set; }

	public virtual List<ItemAttribute> ItemAttributeList { get; set; }
}
