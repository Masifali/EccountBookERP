using System;
using System.Collections.Generic;

namespace Architecture.Model.pcc;

public class ContractorWagesRateSchedule
{
	public bool IsActive { get; set; }

	public DateTime EffectiveDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal MatureMTo { get; set; }

	public decimal MatureSTo { get; set; }

	public decimal MeatureMFrom { get; set; }

	public decimal MeatureSFrom { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public decimal CompanyRate { get; set; }

	public int ActionId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int ContractorWagesActivityId { get; set; }

	public int ContractorWagesGroupId { get; set; }

	public int ItemId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int RangeUomId { get; set; }

	public int ContractorId { get; set; }

	public virtual string WagesActivity { get; set; }

	public virtual string ItemWagesGroupName { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual List<ContractorWagesRateSchedule> ContractorWagesRateScheduleList { get; set; }
}
