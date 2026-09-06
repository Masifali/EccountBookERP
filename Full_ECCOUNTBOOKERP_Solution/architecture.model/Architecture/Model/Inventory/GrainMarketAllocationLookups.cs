namespace Architecture.Model.Inventory;

public class GrainMarketAllocationLookups
{
	public int Id { get; set; }

	public int LookupTypeId { get; set; }

	public int GrainMarketId { get; set; }

	public int AllocatedObjectId { get; set; }

	public string AllocatedObjectName { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public bool Active { get; set; }
}
