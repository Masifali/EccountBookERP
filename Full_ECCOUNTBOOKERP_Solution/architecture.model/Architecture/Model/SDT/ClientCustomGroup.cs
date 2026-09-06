using System;
using System.Collections.Generic;

namespace Architecture.Model.SDT;

public class ClientCustomGroup
{
	public bool isActive { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int companyId { get; set; }

	public int CustomGroupId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int organizationId { get; set; }

	public int supplierCustomerId { get; set; }

	public virtual List<ClientCustomGroup> ClientCustomGroupList { get; set; }
}
