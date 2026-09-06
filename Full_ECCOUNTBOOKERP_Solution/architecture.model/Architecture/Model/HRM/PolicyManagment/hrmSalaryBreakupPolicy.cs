using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.PolicyManagment;

public class hrmSalaryBreakupPolicy
{
	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public double SalaryTypePercent { get; set; }

	public int ActionTypeId { get; set; }

	public int AlteredById { get; set; }

	public int CompanyId { get; set; }

	public int CreatedById { get; set; }

	public int LocationId { get; set; }

	public int OrganizationId { get; set; }

	public int SalaryBreakupPolicyId { get; set; }

	public int SalaryTypeProfileId { get; set; }

	public int UserLogId { get; set; }

	public virtual string SalaryType { get; set; }

	public virtual List<hrmSalaryBreakupPolicy> SalaryBreakupPolicieslist { get; set; }
}
