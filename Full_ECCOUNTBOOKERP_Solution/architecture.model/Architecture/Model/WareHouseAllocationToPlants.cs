using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class WareHouseAllocationToPlants
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int WarehouseId { get; set; }

	public int PlantId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public virtual string PlantName { get; set; }

	public virtual List<WareHouseAllocationToPlants> WareHouseAllocationToPlantsList { get; set; }
}
