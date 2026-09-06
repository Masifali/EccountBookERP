using System.Collections.Generic;

namespace Architecture.Model;

public class ItemAttributeVarientAllocationtoItem
{
	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int Id { get; set; }

	public int ItemAttributeVarientId { get; set; }

	public int ItemId { get; set; }

	public int OrganizationId { get; set; }

	public virtual string ItemAttributeVarient { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual List<ItemAttributeVarientAllocationtoItem> ItemAttributeVarientAllocationtoItemList { get; set; }
}
