using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class ItemAllocationToCustomGroup
{
	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int Id { get; set; }

	public int ItemCustomGroupId { get; set; }

	public int ItemId { get; set; }

	public int OrganizationId { get; set; }

	public virtual List<ItemAllocationToCustomGroup> ItemAllocationToCustomGroupsList { get; set; }
}
