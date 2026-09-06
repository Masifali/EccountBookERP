using System;

namespace Architecture.Model.ContractorWages;

public class InvConractorWagesAccounts
{
	public bool IsActive { get; set; }

	public int CompanyId { get; set; }

	public int GlAccountId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public int WagesLookupId { get; set; }

	public string WagesAccountName { get; set; }

	public int WagesActivityId { get; set; }

	public int ModifyUserId { get; set; }

	public int EntryUserId { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }
}
