namespace Architecture.Model.Production;

public class PowerMeterForPowerConsumption
{
	public int Id { get; set; }

	public string PowerMeterName { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }
}
