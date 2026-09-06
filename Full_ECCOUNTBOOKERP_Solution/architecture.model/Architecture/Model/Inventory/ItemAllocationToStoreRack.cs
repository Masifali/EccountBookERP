using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class ItemAllocationToStoreRack
{
	public bool isActive { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int itemId { get; set; }

	public int ModifyUserId { get; set; }

	public int storeRackId { get; set; }

	public virtual string ItemName { get; set; }

	public virtual List<ItemAllocationToStoreRack> ItemAllocationToStoreRackList { get; set; }
}
