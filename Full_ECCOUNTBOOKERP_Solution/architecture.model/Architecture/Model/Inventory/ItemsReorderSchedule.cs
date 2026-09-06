using System;

namespace Architecture.Model.Inventory;

public class ItemsReorderSchedule
{
	public DateTime EffectiveDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double MaxQty { get; set; }

	public double MinQty { get; set; }

	public double OptimalQty { get; set; }

	public double ReOrderPoint { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int SortNo { get; set; }

	public virtual string BranchName { get; set; }

	public virtual string itemName { get; set; }

	public virtual string ItemUOM { get; set; }
}
