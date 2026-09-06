namespace Architecture.Model;

public class ReferenceParties
{
	public bool IsActive { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int ReferencePartyTypeId { get; set; }

	public string ReferencePartyName { get; set; }
}
