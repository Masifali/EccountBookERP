using System;

namespace Architecture.Model.HRM.EmployeeManagement;

public class genDepartment
{
	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public int ActionTypeId { get; set; }

	public int AlteredById { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int CreatedById { get; set; }

	public int DepartmentId { get; set; }

	public int DepartmentTypeProfileId { get; set; }

	public int OrganizationId { get; set; }

	public int UserLogId { get; set; }

	public string DepartmentCode { get; set; }

	public string DepartmentName { get; set; }

	public string ShortName { get; set; }
}
