namespace Architecture.Model.FeedMill.Production;

public class ProductionOverHeadsDetail
{
	public decimal Amount { get; set; }

	public decimal PercentOfTotalInput { get; set; }

	public decimal BomQty { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public int BomOverHeadId { get; set; }

	public int ProductionHeaderId { get; set; }

	public int Id { get; set; }

	public int OverHeadAccountId { get; set; }

	public int OverHeadTypeId { get; set; }

	public string OhRemarks { get; set; }

	public int BomHeaderId { get; set; }

	public virtual string BomHeaderNo { get; set; }

	public virtual string OverHeadAccountTitle { get; set; }

	public virtual decimal BomAmount { get; set; }
}
