namespace Architecture.Model.Lab;

public class InvLabAnalysisGroup
{
	public int CompanyId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public string AnalysisGroupCode { get; set; }

	public string AnalysisGroupDescription { get; set; }

	public int GroupType { get; set; }
}
