using System;

namespace Architecture.Model.Authentication;

public class ActivityLog
{
	public DateTime LoginDate { get; set; }

	public int CompanyId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public int UserId { get; set; }

	public string Activity { get; set; }

	public string RefDocNo { get; set; }

	public string ScreenName { get; set; }
}
