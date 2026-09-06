using System;

namespace Architecture.Model.HRM.ProfileManagement;

public class hrmBenefit
{
	public bool IsDirect { get; set; }

	public bool PayrolEffect { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public int ActionTypeId { get; set; }

	public int AlteredById { get; set; }

	public int BenefitId { get; set; }

	public int BenefitTypeProfileId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int CreatedById { get; set; }

	public int OrganizationId { get; set; }

	public int UserLogId { get; set; }

	public string BenefitName { get; set; }

	public string BenefitPrefix { get; set; }

	public virtual string ProfileName { get; set; }
}
