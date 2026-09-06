using System.Collections.Generic;

namespace Architecture.Model.pcc;

public class ItemAllocationToWagesGroup
{
	public int ContractorWagesGroupId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int CompanyId { get; set; }

	public int OrganizationId { get; set; }

	public int BranchesId { get; set; }

	public virtual string ContractorWagesGroup { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual List<ItemAllocationToWagesGroup> ItemAllocationToCustomGroupsList { get; set; }
}
