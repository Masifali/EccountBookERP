using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class BankBalanceManual
{
	public DateTime BalanceDate { get; set; }

	public DateTime BalanceTime { get; set; }

	public DateTime EntryDate { get; set; }

	public decimal Balance { get; set; }

	public int BySrource { get; set; }

	public int ChartOfAccountId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public string BySourceDescription { get; set; }

	public virtual List<BankBalanceManual> BankBalanceManualList { get; set; }
}
