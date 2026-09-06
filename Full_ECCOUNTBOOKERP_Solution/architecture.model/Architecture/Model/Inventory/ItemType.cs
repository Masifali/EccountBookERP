using System;

namespace Architecture.Model.Inventory;

public class ItemType
{
	public bool PostState { get; set; }

	public bool IsMother { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime? PostDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int Type { get; set; }

	public int ParentCategoryId { get; set; }

	public string TypeCode { get; set; }

	public string TypeDescription { get; set; }
}
