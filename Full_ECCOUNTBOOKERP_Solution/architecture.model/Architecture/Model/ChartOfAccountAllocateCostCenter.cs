using System;
using System.Collections.Generic;

namespace Architecture.Model;

public class ChartOfAccountAllocateCostCenter
{
	public bool IsActive { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public long ChartOfAccountAllocateCostCenterId { get; set; }

	public int ChartOfAccountId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int CostCenterId { get; set; }

	public virtual List<ChartOfAccountAllocateCostCenter> ChartOfAccountAllocateCostCenterList { get; set; }
}
