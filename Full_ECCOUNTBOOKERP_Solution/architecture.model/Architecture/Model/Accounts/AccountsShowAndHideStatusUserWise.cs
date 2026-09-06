using System;
using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class AccountsShowAndHideStatusUserWise
{
	public bool IsActive { get; set; }

	public DateTime EntryDate { get; set; }

	public int AccountStatus { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int UserId { get; set; }

	public virtual List<AccountsShowAndHideStatusUserWise> AccountsShowAndHideStatusUserWiseslist { get; set; }
}
