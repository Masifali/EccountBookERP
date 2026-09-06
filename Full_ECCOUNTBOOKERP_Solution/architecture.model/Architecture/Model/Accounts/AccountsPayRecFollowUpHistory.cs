using System;

namespace Architecture.Model.Accounts;

public class AccountsPayRecFollowUpHistory
{
	public DateTime CommentsDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime FollowupDate { get; set; }

	public DateTime PromiseDate { get; set; }

	public int ChartOfAccountId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeIdRef { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int NextFollowupDays { get; set; }

	public int OrganizationId { get; set; }

	public string CommentsDetail { get; set; }
}
