using System;

namespace Architecture.Model.Production;

public class InvProductionCumPackMaterialConsumptionDetail
{
	public DateTime EntryDate { get; set; }

	public DateTime ModityDate { get; set; }

	public decimal Qty { get; set; }

	public double Weight { get; set; }

	public int ActionTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int InvProductionCumPackMaterialConsumptionHeaderId { get; set; }

	public int ModifyUserId { get; set; }

	public int PmItemId { get; set; }

	public int PmItemUomId { get; set; }

	public int SortNo { get; set; }

	public virtual string ItemName { get; set; }
}
