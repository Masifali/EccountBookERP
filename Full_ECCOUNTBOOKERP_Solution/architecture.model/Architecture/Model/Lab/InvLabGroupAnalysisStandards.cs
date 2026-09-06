namespace Architecture.Model.Lab;

public class InvLabGroupAnalysisStandards
{
	public double MaxValue { get; set; }

	public double MinValue { get; set; }

	public int CompanyId { get; set; }

	public int Id { get; set; }

	public int InvLabAnalysisGroup { get; set; }

	public int InvLabAnalysisItems { get; set; }

	public int OrganizationId { get; set; }

	public bool IsEditableAfterApproval { get; set; }
}
