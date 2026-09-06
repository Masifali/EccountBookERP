using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.EmployeeManagement;

public class hrmEmployeeBenefit
{
	public int EmployeeBenefitId { get; set; }

	public int EmployeeId { get; set; }

	public int EmployeeHistoryId { get; set; }

	public int BenefitId { get; set; }

	public int ApprovalEmployeeId { get; set; }

	public string Description { get; set; }

	public DateTime? CreatedOn { get; set; }

	public int CreatedById { get; set; }

	public int AlteredById { get; set; }

	public DateTime? AlteredOn { get; set; }

	public int ActionTypeId { get; set; }

	public int UserLogId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public virtual string Benefit { get; set; }

	public virtual string ProfileName { get; set; }

	public virtual List<hrmEmployeeBenefit> EmployeeBenefitsList { get; set; }
}
