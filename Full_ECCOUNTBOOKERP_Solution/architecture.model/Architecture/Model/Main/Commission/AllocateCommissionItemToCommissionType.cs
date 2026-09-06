using System;
using System.Collections.Generic;

namespace Architecture.Model.Main.Commission;

public class AllocateCommissionItemToCommissionType
{
	public bool IsActive { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CommissionItemId { get; set; }

	public int CommissionTypeId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public virtual List<AllocateCommissionItemToCommissionType> AllocateCommissionItemToCommissionTypeList { get; set; }
}
