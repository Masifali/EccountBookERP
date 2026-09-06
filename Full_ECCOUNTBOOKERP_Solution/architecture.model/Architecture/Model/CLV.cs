namespace Architecture.Model;

public class CLV
{
	public int CompanyId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public string CompanyName { get; set; }

	public string LicenseKey { get; set; }

	public virtual string ConvertedLicenseKey { get; set; }
}
