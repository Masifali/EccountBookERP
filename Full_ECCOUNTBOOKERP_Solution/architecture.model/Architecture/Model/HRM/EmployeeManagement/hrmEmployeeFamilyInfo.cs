using System;

namespace Architecture.Model.HRM.EmployeeManagement;

public class hrmEmployeeFamilyInfo
{
	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public int ActionTypeId { get; set; }

	public int AlteredById { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int CreatedById { get; set; }

	public int EmployeeFamilyInfoId { get; set; }

	public int EmployeeFamilyInfoLineId { get; set; }

	public int EmployeeId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int RelationshipProfileId { get; set; }

	public int UserLogId { get; set; }

	public string Description { get; set; }

	public string Name { get; set; }

	public virtual string NameEmployee { get; set; }

	public virtual string RelationshipName { get; set; }
}
