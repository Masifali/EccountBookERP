namespace Architecture.Model.Lab;

public class InvLabAnalysisSaleDetail
{
	public double InAnalysisResult { get; set; }

	public int Id { get; set; }

	public int InvLabAnalysisSaleHeaderId { get; set; }

	public int InvLabGroupAnalysisStandardsId { get; set; }

	public string RemarksDetail { get; set; }
}
