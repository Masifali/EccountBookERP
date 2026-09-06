using System;

namespace Architecture.Model;

public class SubsidiaryAccountsDefine
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int AccountId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public string SubsidiaryAccountTitle { get; set; }
}
