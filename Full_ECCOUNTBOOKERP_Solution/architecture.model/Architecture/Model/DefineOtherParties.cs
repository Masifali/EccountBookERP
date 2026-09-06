namespace Architecture.Model;

public class DefineOtherParties
{
	public bool IsActive { get; set; }

	public int Id { get; set; }

	public int DocumentTypeId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public string PartyName { get; set; }
}
