using System;

namespace Architecture.Model.HRM.EmployeeManagement;

public class hrmEmployeeReference
{
	public bool IsReferPerson { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public int ActionTypeId { get; set; }

	public int AddressId { get; set; }

	public int AlteredById { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int CreatedById { get; set; }

	public int DesignationId { get; set; }

	public int EmployeeId { get; set; }

	public int EmployeeReferenceId { get; set; }

	public int EmployeeReferenceLineId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int UserLogId { get; set; }

	public string Description { get; set; }

	public string Name { get; set; }

	public string OrginizationName { get; set; }

	public string Mobile { get; set; }

	public string Email { get; set; }

	public virtual string EmployeeName { get; set; }

	public virtual string DesignationName { get; set; }
}
