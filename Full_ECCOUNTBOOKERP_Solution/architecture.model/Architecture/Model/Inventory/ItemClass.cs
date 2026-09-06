using System;

namespace Architecture.Model.Inventory;

public class ItemClass
{
	public bool PostState { get; set; }

	public DateTime EnteryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public int ClassId { get; set; }

	public int CompanyId { get; set; }

	public int EnteryUser { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public string ClassCode { get; set; }

	public string ClassDescription { get; set; }
}
