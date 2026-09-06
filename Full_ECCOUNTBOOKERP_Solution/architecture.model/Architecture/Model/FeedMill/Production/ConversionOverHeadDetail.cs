namespace Architecture.Model.FeedMill.Production;

public class ConversionOverHeadDetail
{
	public decimal Amount { get; set; }

	public decimal PercentOfTotalInput { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public int ConversionHeaderId { get; set; }

	public int Id { get; set; }

	public int OverHeadAccountId { get; set; }

	public int OverHeadTypeId { get; set; }

	public string OhRemarks { get; set; }

	public virtual string OverHeadAccountTitle { get; set; }
}
