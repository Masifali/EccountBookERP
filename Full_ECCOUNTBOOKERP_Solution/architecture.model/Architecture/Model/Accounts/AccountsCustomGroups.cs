using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class AccountsCustomGroups
{
	public int AcLookUpsId { get; set; }

	public int ChartOfAccountId { get; set; }

	public int SortNo { get; set; }

	public int companyId { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int organizationId { get; set; }

	public virtual List<AccountsCustomGroups> GetList { get; set; }
}
