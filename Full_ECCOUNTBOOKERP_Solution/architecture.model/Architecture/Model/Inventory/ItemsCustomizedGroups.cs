namespace Architecture.Model.Inventory;

public class ItemsCustomizedGroups
{
	public double DefaultQty { get; set; }

	public int CustomGroupId { get; set; }

	public int InvLookUpsId { get; set; }

	public int ItemId { get; set; }

	public int SortNo { get; set; }

	public virtual string CustomerGroup { get; set; }

	public virtual string ItemName { get; set; }
}
