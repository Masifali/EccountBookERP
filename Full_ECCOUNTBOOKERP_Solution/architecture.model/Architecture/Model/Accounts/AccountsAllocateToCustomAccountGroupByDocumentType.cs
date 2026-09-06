using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class AccountsAllocateToCustomAccountGroupByDocumentType
{
	public int Id { get; set; }

	public int AccountId { get; set; }

	public int CustomAccountGroupsByDocumentTypeId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public virtual List<AccountsAllocateToCustomAccountGroupByDocumentType> AccountsAllocateToCustomAccountGroupByDocumentTypeList { get; set; }
}
