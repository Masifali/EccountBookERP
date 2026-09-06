using System;

namespace Architecture.Model.Inventory;

public class invWarehouseRack
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public bool isActive { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int invWarehouseId { get; set; }

	public int ModifyUserId { get; set; }

	public int sortNo { get; set; }

	public string rackName { get; set; }
}
