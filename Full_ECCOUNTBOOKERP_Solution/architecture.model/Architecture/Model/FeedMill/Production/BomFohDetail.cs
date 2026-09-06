namespace Architecture.Model.FeedMill.Production;

public class BomFohDetail
{
	public decimal Amount { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public decimal PercentOfTotalInput { get; set; }

	public int BomHeaderId { get; set; }

	public int Id { get; set; }

	public int OverHeadTypeId { get; set; }

	public int GLAccountId { get; set; }

	public string OhRemarks { get; set; }

	public virtual string OverHeadAccountTitle { get; set; }

	public virtual string BomHeaderNo { get; set; }
}
