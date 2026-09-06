using System;

namespace Architecture.Model.SystemUtilities;

public class SeasonYearSchedule
{
	public bool IsActive { get; set; }

	public DateTime SeasonStartDate { get; set; }

	public DateTime SeasonEndDate { get; set; }

	public int CompanyId { get; set; }

	public int OrganizationId { get; set; }

	public int Id { get; set; }
}
