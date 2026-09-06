using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class PartyCustomGroups
{
	public int Id { get; set; }

	public int SupplierCustomerId { get; set; }

	public int CustomGroupId { get; set; }

	public int SortNo { get; set; }

	public int companyId { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int organizationId { get; set; }

	public virtual List<PartyCustomGroups> GetList { get; set; }
}
