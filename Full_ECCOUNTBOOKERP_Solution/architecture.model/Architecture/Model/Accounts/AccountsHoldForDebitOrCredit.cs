using System;

namespace Architecture.Model.Accounts;

public class AccountsHoldForDebitOrCredit
{
	public bool CreditHoldStatus { get; set; }

	public bool DebitHoldStatus { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int BranchesId { get; set; }

	public int CoaAccountId { get; set; }

	public int CompanyId { get; set; }

	public int CreatedById { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public string CreditHoldReason { get; set; }

	public string DebitHoldReason { get; set; }
}
