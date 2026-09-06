using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class MasterItemAllocations
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int MasterItemId { get; set; }

	public int ModifyUserId { get; set; }

	public string ManualCode { get; set; }

	public double EmptyBagWeight { get; set; }

	public virtual List<MasterItemAllocations> MasterItemAllocationslist { get; set; }
}
