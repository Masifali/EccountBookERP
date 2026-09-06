using System;

namespace Architecture.Model.HRM.EmployeeManagement;

public class genDesignation
{
	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public int ActionTypeId { get; set; }

	public int AlteredById { get; set; }

	public int CompanyId { get; set; }

	public int CreatedById { get; set; }

	public int DesignationId { get; set; }

	public int OrganizationId { get; set; }

	public int UserLogId { get; set; }

	public string DesignationName { get; set; }
}
