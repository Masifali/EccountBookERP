using System;

namespace Architecture.Model.Lab;

public class InvLabAnalysisInProcessDetail
{
	public DateTime Analysistime { get; set; }

	public double InAnalysisResult { get; set; }

	public int Id { get; set; }

	public int AnalysisParameterID { get; set; }

	public int InvLabAnalysisInProcessHeaderId { get; set; }

	public int InvLabGroupAnalysisStandardsId { get; set; }

	public string RemarksDetail { get; set; }
}
