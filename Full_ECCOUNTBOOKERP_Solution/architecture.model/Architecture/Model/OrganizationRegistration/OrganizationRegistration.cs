namespace Architecture.Model.OrganizationRegistration;

public class OrganizationRegistration
{
	public int Id { get; set; }

	public int OrganizaionTemplateId { get; set; }

	public int NoOfLocations { get; set; }

	public string Code { get; set; }

	public string Name { get; set; }

	public string ReportingTitle { get; set; }

	public string Email { get; set; }

	public string PhoneNumber { get; set; }

	public string Country { get; set; }

	public string BaseCurrency { get; set; }

	public string State { get; set; }

	public string City { get; set; }

	public string Address { get; set; }

	public string ContactPerson { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string ContactPersonEmail { get; set; }

	public string ContactPersonPhone { get; set; }

	public byte[] CompLogoImage { get; set; }
}
