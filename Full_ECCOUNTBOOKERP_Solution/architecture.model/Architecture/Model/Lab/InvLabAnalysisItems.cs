namespace Architecture.Model.Lab;

public class InvLabAnalysisItems
{
	public int CompanyId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public int ParentParameterId { get; set; }

	public int MasterParId { get; set; }

	public string AnalysisParameterCode { get; set; }

	public string AnalysisParameterDescription { get; set; }

	public bool IsSub { get; set; }

	public double MinValue { get; set; }

	public double MaxValue { get; set; }
}
