using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class UserWarehouse
{
	public bool IsActive { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int InvWareHouseId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int UserAccountId { get; set; }

	public int UserWarehuseId { get; set; }

	public virtual List<UserWarehouse> UserWarehousesList { get; set; }
}
