namespace Architecture.Model.Authentication;

public class ConfigrationsAllocation
{
	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int ConfigrationsDefinitionId { get; set; }

	public int Id { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public string ConfigKey { get; set; }

	public string ConfigValue { get; set; }

	public bool IsActive { get; set; }

	public string ConfigDescription { get; set; }

	public string ConfigModuleDescription { get; set; }
}
