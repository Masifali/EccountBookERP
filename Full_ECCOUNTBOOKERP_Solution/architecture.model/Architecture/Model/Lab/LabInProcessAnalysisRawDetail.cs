using System;

namespace Architecture.Model.Lab;

public class LabInProcessAnalysisRawDetail
{
	public DateTime Analysistime { get; set; }

	public decimal ResultValue { get; set; }

	public int Id { get; set; }

	public int LabInProcessAnalysisHeaderId { get; set; }

	public int ParameterId { get; set; }

	public int StepId { get; set; }

	public string RemarkDetail { get; set; }
}
