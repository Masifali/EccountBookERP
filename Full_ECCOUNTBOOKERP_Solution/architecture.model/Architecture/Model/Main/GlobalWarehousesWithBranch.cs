namespace Architecture.Model.Main;

public class GlobalWarehousesWithBranch
{
	public int WarehouseId { get; set; }

	public int BranchId { get; set; }

	public string BranchName { get; set; }

	public string WarehouseName { get; set; }

	public int WareHouseTypeId { get; set; }

	public string WareHouseType { get; set; }

	public int PlantId { get; set; }

	public string PlantName { get; set; }

	public bool IsActive { get; set; }
}
