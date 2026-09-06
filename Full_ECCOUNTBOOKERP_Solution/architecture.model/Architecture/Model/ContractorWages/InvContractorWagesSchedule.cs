using System;

namespace Architecture.Model.ContractorWages;

public class InvContractorWagesSchedule
{
	public DateTime EffectedDate { get; set; }

	public DateTime EffectedDateTo { get; set; }

	public double RateUom { get; set; }

	public double WageRate { get; set; }

	public double PackUomFrom { get; set; }

	public double PackUomTo { get; set; }

	public double CompanyRate { get; set; }

	public int CompanyId { get; set; }

	public int ContractorId { get; set; }

	public int Id { get; set; }

	public int InvConractorWagesAccountsId { get; set; }

	public int OrganizationId { get; set; }

	public int ActionId { get; set; }

	public int EntryUserId { get; set; }

	public DateTime EntryDate { get; set; }

	public int ModifyUserId { get; set; }

	public DateTime ModifyDate { get; set; }
}
