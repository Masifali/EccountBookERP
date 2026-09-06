using System;

namespace Architecture.Model;

public class Department
{
	public int Id { get; set; }

	public string DepartmentName { get; set; }

	public DateTime EntryDate { get; set; }

	public int EntryUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ModifyUser { get; set; }

	public DateTime PostDate { get; set; }

	public int PostUser { get; set; }

	public bool PostState { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }
}
