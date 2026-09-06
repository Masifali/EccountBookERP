using System;

namespace Architecture.Model.Lab;

public class DeductionPolicyForGrn
{
	public bool IsActive { get; set; }

	public DateTime EffectiveDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal ValueFrom { get; set; }

	public decimal ValueTo { get; set; }

	public int ActionId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DeductionOnId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PolicyTypeId { get; set; }

	public int ProjectsId { get; set; }

	public int RevisionNo { get; set; }

	public string ConditionDescription { get; set; }

	public virtual string PolicyType { get; set; }

	public virtual string DeductionOn { get; set; }
}
