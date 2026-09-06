namespace Architecture.Model.Steel.Production;

public class InvProductionJobOrderOverHeads
{
	public int Id { get; set; }

	public int ProductionJobOrderId { get; set; }

	public int CharOfAccountId { get; set; }

	public decimal Amount { get; set; }

	public string RemarksOH { get; set; }

	public decimal? ItemQty { get; set; }

	public decimal? ItemRate { get; set; }

	public int? BrandId { get; set; }

	public int? BrandUomId { get; set; }

	public int ActionTypeId { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual string BrandName { get; set; }

	public virtual string BrandUom { get; set; }

	public virtual double BrandEquivalent { get; set; }
}
