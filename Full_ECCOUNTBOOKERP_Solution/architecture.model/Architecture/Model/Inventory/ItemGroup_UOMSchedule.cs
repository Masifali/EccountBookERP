namespace Architecture.Model.Inventory;

public class ItemGroup_UOMSchedule
{
	public int Id { get; set; }

	public int ItemGroupId { get; set; }

	public int UOMId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public string Equivalent { get; set; }
}
