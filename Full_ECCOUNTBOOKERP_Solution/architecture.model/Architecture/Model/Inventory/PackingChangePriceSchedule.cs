using System;

namespace Architecture.Model.Inventory;

public class PackingChangePriceSchedule
{
	public DateTime EffectiveFrom { get; set; }

	public DateTime EffectiveTo { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double Equivalent { get; set; }

	public double PackingAddLess { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public int UomId { get; set; }

	public int EntryUser { get; set; }

	public int ModifyUser { get; set; }
}
