namespace Architecture.Model.Mfg;

public class ProductionOverHeadsDetail
{
	public decimal Amount { get; set; }

	public decimal BomQty { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public decimal PercentOfTotalInput { get; set; }

	public int BomHeaderId { get; set; }

	public int BomOverHeadId { get; set; }

	public int OverHeadAccountId { get; set; }

	public int ProductionHeaderId { get; set; }

	public int ProductionOverHeadsDetailId { get; set; }

	public string Remarks { get; set; }

	public virtual string OverHeadAccountTitle { get; set; }
}
