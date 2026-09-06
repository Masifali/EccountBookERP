using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class ItemAttributVariant
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int companyId { get; set; }

	public int EntryUserId { get; set; }

	public bool isActive { get; set; }

	public int itemId { get; set; }

	public int ModifyUserId { get; set; }

	public int organizationId { get; set; }

	public int seqNo { get; set; }

	public long attributeId { get; set; }

	public long itemAttributeVariantId { get; set; }

	public long variantId { get; set; }

	public virtual List<ItemAttributVariant> ItemAttributVariantList { get; set; }
}
