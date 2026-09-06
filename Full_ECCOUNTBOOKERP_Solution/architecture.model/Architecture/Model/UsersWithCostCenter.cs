using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class UsersWithCostCenter
{
	public bool IsActive { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public long UsersWithCostCenterId { get; set; }

	public int UserId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int CostCenterId { get; set; }

	public virtual List<UsersWithCostCenter> UsersWithCostCenterList { get; set; }
}
