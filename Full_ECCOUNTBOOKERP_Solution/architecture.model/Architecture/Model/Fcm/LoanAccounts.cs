using System;

namespace Architecture.Model.Fcm;

public class LoanAccounts
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int lenderRegistrationId { get; set; }

	public int LoanAccountsId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int refGLAccountId { get; set; }

	public int SortNo { get; set; }

	public string LoanAccountCode { get; set; }

	public string LoanAccountName { get; set; }
}
