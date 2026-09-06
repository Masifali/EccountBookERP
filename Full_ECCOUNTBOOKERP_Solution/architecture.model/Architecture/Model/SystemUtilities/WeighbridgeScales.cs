namespace Architecture.Model.SystemUtilities;

public class WeighbridgeScales
{
	public bool IsActive { get; set; }

	public double AcceptedPlusMins { get; set; }

	public double WbsCapacityMax { get; set; }

	public double WbsCapacityMin { get; set; }

	public int CompanyId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public string ModelName { get; set; }

	public string WbsNameDescription { get; set; }
}
