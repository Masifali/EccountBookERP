using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.PolicyManagment;

public class hrmIncomeTaxPolicy
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

	public int IncomeTaxPolicyId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int SalaryFactorProfileId { get; set; }

	public int UserLogId { get; set; }

	public string PolicyDescription { get; set; }

	public virtual List<hrmIncomeTaxPolicySlab> IncomeTaxDetail { get; set; }

	public virtual List<hrmIncomeTaxPolicyEmployee> TaxPolicyEmployeesLists { get; set; }

	public virtual List<DMSAttachments> dmsAttachment { get; set; }
}
