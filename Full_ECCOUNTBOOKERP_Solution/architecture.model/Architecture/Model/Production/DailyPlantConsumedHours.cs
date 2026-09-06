using System;

namespace Architecture.Model.Production;

public class DailyPlantConsumedHours
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime ReportDate { get; set; }

	public double ConsumedHours { get; set; }

	public decimal TotalWorkingHours { get; set; }

	public decimal Downtime { get; set; }

	public int ReasonId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int JobOrderId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PlantId { get; set; }
}
