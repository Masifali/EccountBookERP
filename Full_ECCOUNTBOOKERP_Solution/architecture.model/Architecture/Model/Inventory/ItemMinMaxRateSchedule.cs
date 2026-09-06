using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class ItemMinMaxRateSchedule
{
	public DateTime EffectedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double MaxRate { get; set; }

	public double MinRate { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int RateUomId { get; set; }

	public virtual List<ItemMinMaxRateSchedule> ItemMinMaxRateSchedulelist { get; set; }
}
