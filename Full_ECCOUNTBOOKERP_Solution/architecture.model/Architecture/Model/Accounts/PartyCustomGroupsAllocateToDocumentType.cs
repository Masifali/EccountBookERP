using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class PartyCustomGroupsAllocateToDocumentType
{
	public int Id { get; set; }

	public int CompanyId { get; set; }

	public int OrganizationId { get; set; }

	public int ModifyUserId { get; set; }

	public int EntryUserId { get; set; }

	public int DocumentTypeId { get; set; }

	public int PartyCustomGroupsId { get; set; }

	public virtual List<PartyCustomGroupsAllocateToDocumentType> PartyCustomGroupsAllocateToDocumentTypeList { get; set; }
}
