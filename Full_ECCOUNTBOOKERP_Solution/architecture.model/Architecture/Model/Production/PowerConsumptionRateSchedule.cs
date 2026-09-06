using System;

namespace Architecture.Model.Production;

public class PowerConsumptionRateSchedule
{
	public DateTime ApprovedDate { get; set; }

	public DateTime EffectiveDateFrom { get; set; }

	public DateTime EffectiveDateTo { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double PowerRate { get; set; }

	public double PriceUomequivalent { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public bool IsApproved { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PowerTypeId { get; set; }

	public int PriceUomId { get; set; }

	public int ProjectId { get; set; }

	public int SortNo { get; set; }
}
