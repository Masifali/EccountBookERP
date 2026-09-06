namespace Architecture.Model.Main;

public class getGlobalAllSerivesItems
{
	public int Id { get; set; }

	public string ItemName { get; set; }

	public string ItemCode { get; set; }

	public int InventoryParentCategoriesId { get; set; }

	public string InvParentCateDescription { get; set; }

	public int ItemCategoryId { get; set; }

	public string ItemCategory { get; set; }

	public int ItemTypeId { get; set; }

	public int ItemTypeOfTypeId { get; set; }

	public string ItemTypeOfType { get; set; }

	public string ItemType { get; set; }

	public int ServicesMasterItemId { get; set; }

	public string ServicesMasterItem { get; set; }
}
