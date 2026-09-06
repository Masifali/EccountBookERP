using System;

namespace Architecture.Model.Production;

public class InvFoodProductionPowerConsumptionDetail
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double ReadingUnits { get; set; }

	public int ActionId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int InvProductionJobOrderId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PowerMeterId { get; set; }

	public DateTime ReadingDateAndTime { get; set; }

	public int ReadingPicPath { get; set; }

	public int SortNo { get; set; }

	public int PowerTypeId { get; set; }

	public int AutoManual { get; set; }

	public int PowerConsumptionRateScheduleId { get; set; }
}
