using System;

namespace Architecture.Model.HRM.EmployeeManagement;

public class hrmEmployeeExperience
{
	public int EmployeeExperienceId { get; set; }

	public int EmployeeExperienceLineId { get; set; }

	public int EmployeeId { get; set; }

	public int CityId { get; set; }

	public int DesignationId { get; set; }

	public string OrginizationName { get; set; }

	public DateTime FromDate { get; set; }

	public DateTime ToDate { get; set; }

	public string Description { get; set; }

	public int CreatedById { get; set; }

	public DateTime CreatedOn { get; set; }

	public int AlteredById { get; set; }

	public DateTime AlteredOn { get; set; }

	public int ActionTypeId { get; set; }

	public int UserLogId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchId { get; set; }

	public int ProjectId { get; set; }

	public virtual string Designation { get; set; }

	public virtual string CityName { get; set; }

	public virtual string NameEmployee { get; set; }
}
