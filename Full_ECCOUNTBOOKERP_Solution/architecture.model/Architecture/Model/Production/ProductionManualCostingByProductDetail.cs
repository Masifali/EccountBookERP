using System;

namespace Architecture.Model.Production;

public class ProductionManualCostingByProductDetail
{
	public decimal Amount { get; set; }

	public decimal Rate { get; set; }

	public decimal Ratio { get; set; }

	public decimal PercentOfTotalInput { get; set; }

	public decimal ByProductAmount { get; set; }

	public int ActionTypeId { get; set; }

	public int Id { get; set; }

	public int InputDetailId { get; set; }

	public int InvFoodProductionId { get; set; }

	public int ItemId { get; set; }

	public int ProductionManualCostingId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual DateTime? ProductionDate { get; set; }

	public virtual int ProductionNo { get; set; }
}
