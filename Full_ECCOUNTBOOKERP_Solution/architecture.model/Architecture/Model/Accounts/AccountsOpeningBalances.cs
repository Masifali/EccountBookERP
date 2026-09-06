using System;
using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class AccountsOpeningBalances
{
	public bool PostState { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public double YearObCredit { get; set; }

	public double YearObDebit { get; set; }

	public int ChartOfAccountId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }

	public int EntryUser { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public string ChartOfAccountTitle { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual string Start_Period { get; set; }

	public virtual string End_Period { get; set; }

	public virtual string FinancialYearCode { get; set; }

	public virtual List<AccountsOpeningBalances> OpeningBalanceList { get; set; }
}
