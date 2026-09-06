namespace Architecture.Model.Production;

public class InvBomDetail
{
	public double ConsQty { get; set; }

	public double Prcnt { get; set; }

	public double PrdAmount { get; set; }

	public double PrdRate { get; set; }

	public double ProdQty { get; set; }

	public int Id { get; set; }

	public int InvBomHeaderId { get; set; }

	public int PrdRateUomId { get; set; }

	public int ProductionItemId { get; set; }

	public virtual string ItemName { get; set; }
}
