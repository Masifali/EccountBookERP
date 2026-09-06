using System.Collections.Generic;

namespace Architecture.Model.Lab;

public class InvLabSampleAnalysisDetail
{
	public int Id { get; set; }

	public int InvLabAnalysisItemsId { get; set; }

	public int InvLabSampleAnalysisHeaderId { get; set; }

	public virtual string AnalysisGroupDescription { get; set; }

	public virtual string AnalysisParameterDescription { get; set; }

	public virtual double MinValue { get; set; }

	public virtual double MaxValue { get; set; }

	public double ResultValue { get; set; }

	public string RemarksDetail { get; set; }

	public virtual List<InvLabSampleAnalysiSubParamsDetail> InvLabSampleAnalysiSubParamsDetailsList { get; set; }
}
