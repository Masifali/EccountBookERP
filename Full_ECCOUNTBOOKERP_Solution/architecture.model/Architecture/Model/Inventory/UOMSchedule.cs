using System;

namespace Architecture.Model.Inventory;

public class UOMSchedule
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double Equivalent { get; set; }

	public double QtyEquivalent { get; set; }

	public bool BaseRateUom { get; set; }

	public bool BasePackUom { get; set; }

	public bool BaseSecondaryUom { get; set; }

	public bool Active { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ScheduleUnitId { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual string ScheduleUnitName { get; set; }
}
