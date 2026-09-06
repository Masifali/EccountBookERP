namespace Architecture.Model.Lab;

public class InvLabSampleAnalysiSubParamsDetail
{
	public int Id { get; set; }

	public int InvLabSampleAnalysisDetailId { get; set; }

	public int InvLabSampleAnalysisHeaderId { get; set; }

	public int InvParentParameterId { get; set; }

	public int SubParameterId { get; set; }

	public virtual string ParentParameterName { get; set; }

	public virtual string SubParameterName { get; set; }

	public double ResultValue { get; set; }
}
