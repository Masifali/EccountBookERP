namespace Architecture.Model.Main;

public class getGlobalAllItems
{
	public int Id { get; set; }

	public string ItemName { get; set; }

	public string ItemCode { get; set; }

	public int InventoryParentCategoriesId { get; set; }

	public int ItemCategoryId { get; set; }

	public string ItemCategory { get; set; }

	public int ItemTypeId { get; set; }

	public int ItemTypeOfTypeId { get; set; }

	public string ItemTypeOfType { get; set; }

	public string ItemType { get; set; }

	public int ItemProductionStageId { get; set; }

	public string productionStageName { get; set; }

	public double FinishWeight { get; set; }

	public double DyeWeight { get; set; }

	public double SandWeight { get; set; }

	public string SampleNo { get; set; }

	public bool HasModel { get; set; }

	public int itemModalId { get; set; }

	public int ItemClassId { get; set; }

	public int LeadTimeDay { get; set; }

	public string WeightCapacity { get; set; }

	public string InvParentCateDescription { get; set; }

	public int MasterItemId { get; set; }

	public string MasterItem { get; set; }
}
