namespace Architecture.Model;

public class InvBoilerName
{
	public bool IsActive { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public string BoilerName { get; set; }
}
