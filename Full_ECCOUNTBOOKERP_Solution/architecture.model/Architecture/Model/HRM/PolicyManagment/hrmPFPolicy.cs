using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.PolicyManagment;

public class hrmPFPolicy
{
	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime FromDate { get; set; }

	public DateTime ToDate { get; set; }

	public int ActionTypeId { get; set; }

	public int AlteredById { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int CreatedById { get; set; }

	public int OrganizationId { get; set; }

	public int PFPolicyId { get; set; }

	public int ProjectId { get; set; }

	public int SalaryFactorProfileId { get; set; }

	public int UserLogId { get; set; }

	public string PolicyDescription { get; set; }

	public virtual string SalaryFactor { get; set; }

	public virtual List<hrmPFPolicySlab> PolicyDetail { get; set; }

	public virtual List<hrmPFPolicyEmployee> PFPolicyEmployeesList { get; set; }
}
