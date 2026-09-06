using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class ItemAllocateToWareHouse
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int BrancheId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int WarehouseId { get; set; }

	public virtual List<ItemAllocateToWareHouse> ItemAllocateToWareHouseslist { get; set; }
}
