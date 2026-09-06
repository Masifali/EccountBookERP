using System;

namespace Architecture.Model.Inventory;

public class InvWareHouse
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public string WareHouseCode { get; set; }

	public string WareHouseName { get; set; }

	public string WarehouseType { get; set; }

	public bool IsActive { get; set; }
}
