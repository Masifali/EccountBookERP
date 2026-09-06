using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.PolicyManagment;

public class hrmEOBIPolicy
{
	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime FromDate { get; set; }

	public DateTime ToDate { get; set; }

	public double CompanyShare { get; set; }

	public double EmployeeShare { get; set; }

	public int ActionTypeId { get; set; }

	public int AgeLimit { get; set; }

	public int AlteredById { get; set; }

	public int CompanyId { get; set; }

	public int CreatedById { get; set; }

	public int EOBIPolicyId { get; set; }

	public int OrganizationId { get; set; }

	public int UserLogId { get; set; }

	public string PolicyDescription { get; set; }

	public virtual List<hrmEOBIPolicyEmployee> EOBIPolicyEmployeesList { get; set; }
}
