namespace Architecture.Model.Main;

public class RacksWithWarehouseAndItem
{
	public int Id { get; set; }

	public string RackName { get; set; }

	public int WarehouseId { get; set; }

	public string WareHouseName { get; set; }

	public int ItemId { get; set; }

	public string ItemName { get; set; }

	public string ItemCode { get; set; }

	public int BranchId { get; set; }

	public string BranchName { get; set; }

	public int BaseRackId { get; set; }
}
