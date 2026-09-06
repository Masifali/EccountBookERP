using System;
using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class AccountsReconciliationHeader
{
	public bool IsApproved { get; set; }

	public DateTime? ApprovedDate { get; set; }

	public DateTime BalanceDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int AccountId { get; set; }

	public int ApprovedUser { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public string ReconciliationType { get; set; }

	public string RemarksHeader { get; set; }

	public virtual List<AccountsReconciliationDetail> AccountsReconiliationDetailList { get; set; }
}
