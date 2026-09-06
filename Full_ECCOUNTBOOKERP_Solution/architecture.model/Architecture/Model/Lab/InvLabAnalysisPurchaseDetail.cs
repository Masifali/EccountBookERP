using System.Collections.Generic;

namespace Architecture.Model.Lab;

public class InvLabAnalysisPurchaseDetail
{
	public int Id { get; set; }

	public int InvLabAnalysisPurchaseHeaderId { get; set; }

	public int InvLabGroupAnalysisStandardsId { get; set; }

	public int InvLabAnalysisItemsId { get; set; }

	public int AnalysisStutusId { get; set; }

	public virtual string AnalysisGroupDescription { get; set; }

	public virtual string AnalysisParameterDescription { get; set; }

	public virtual double MinValue { get; set; }

	public virtual double MaxValue { get; set; }

	public virtual bool IsEditableAfterApproval { get; set; }

	public double InAnalysisResult { get; set; }

	public string RemarksDetail { get; set; }

	public virtual double ParamsMinValue { get; set; }

	public virtual double ParamsMaxValue { get; set; }

	public virtual string PmLabStatus { get; set; }

	public virtual List<InvLabAnalysisPurchaseSubParamsDetail> InvLabAnalysisPurchaseSubParamsDetailslist { get; set; }
}
