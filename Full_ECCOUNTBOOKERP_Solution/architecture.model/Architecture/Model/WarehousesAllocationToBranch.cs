using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class WarehousesAllocationToBranch
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int WarehouseId { get; set; }

	public int BranchId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public virtual string BranchName { get; set; }

	public virtual List<WarehousesAllocationToBranch> WarehousesAllocationToBranchList { get; set; }
}
