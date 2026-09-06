namespace Architecture.Model.Lab;

public class InvLabAnalysisStandardDeductionPolicyDetail
{
	public decimal DedValue { get; set; }

	public decimal RangeFrom { get; set; }

	public decimal RangeTo { get; set; }

	public double WeightKg { get; set; }

	public int AnalysisParameterId { get; set; }

	public int Id { get; set; }

	public int InvLabAnalysisStandardDeductionPolicyHeaderId { get; set; }

	public int SortNo { get; set; }

	public string DeductFrom { get; set; }

	public virtual string AnalysisParameterDescription { get; set; }
}
