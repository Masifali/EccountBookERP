namespace Architecture.Model.POS;

public class WsRmIncentivePolicyDetail
{
	public double IncentiveRate { get; set; }

	public double TargetRangeFrom { get; set; }

	public double TargetRangeTo { get; set; }

	public int Id { get; set; }

	public int IncentiveTypeId { get; set; }

	public int ItemId { get; set; }

	public int TargetBaseTypeId { get; set; }

	public int WsRmIncentivePolicyHeaderId { get; set; }

	public int IncentiveCalcTypeId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string TargetBaseType { get; set; }

	public virtual string IncentiveType { get; set; }

	public virtual string IncentiveCalType { get; set; }
}
