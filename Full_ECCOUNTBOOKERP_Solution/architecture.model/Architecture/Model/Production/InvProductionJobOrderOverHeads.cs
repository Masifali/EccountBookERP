namespace Architecture.Model.Production;

public class InvProductionJobOrderOverHeads
{
	public double Amount { get; set; }

	public double ItemQty { get; set; }

	public double ItemRate { get; set; }

	public int CharOfAccountId { get; set; }

	public int Id { get; set; }

	public int InvProductionJobOrderId { get; set; }

	public string ohRemarks { get; set; }

	public int BrandId { get; set; }

	public int BrandUomId { get; set; }

	public int ActionTypeId { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual string BrandName { get; set; }

	public virtual string BrandUom { get; set; }

	public virtual double BrandEquivalent { get; set; }
}
