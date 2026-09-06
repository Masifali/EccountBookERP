using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class ItemAllocation
{
	public bool IsActive { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int OrganizationId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public virtual string BranchName { get; set; }

	public virtual List<ItemAllocation> ItemAllocationlist { get; set; }
}
