using System;
using System.Collections.Generic;

namespace Architecture.Model.Production;

public class ProductionOverHeadSchedule
{
	public DateTime EffectiveDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double OhRate { get; set; }

	public int ActionId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int CreditAccountId { get; set; }

	public int EntryUser { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OhAccountId { get; set; }

	public int OrganizationId { get; set; }

	public int PlantId { get; set; }

	public int RateUom { get; set; }

	public virtual List<ProductionOverHeadSchedule> ProductionOverHeadScheduleList { get; set; }
}
