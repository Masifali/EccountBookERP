using System;

namespace Architecture.Model.Inventory;

public class ItemModel
{
	public long itemModelId { get; set; }

	public string modalName { get; set; }

	public string SampleNo { get; set; }

	public int SeqNo { get; set; }

	public bool isActive { get; set; }

	public DateTime CreatedOn { get; set; }

	public long CreatedUserId { get; set; }

	public DateTime LastModifiedOn { get; set; }

	public long LastModifiedUserId { get; set; }

	public int ActionId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }
}
