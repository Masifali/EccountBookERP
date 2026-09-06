namespace Architecture.Model.Lab;

public class InvLabAnalysisPurchaseSubParamsDetail
{
	public int Id { get; set; }

	public int InvLabAnalysisPurchaseDetailId { get; set; }

	public int InvLabAnalysisPurchaseHeaderId { get; set; }

	public int InvParentParameterId { get; set; }

	public int SubParameterId { get; set; }

	public virtual string SubParameterName { get; set; }

	public double ResultValue { get; set; }
}
