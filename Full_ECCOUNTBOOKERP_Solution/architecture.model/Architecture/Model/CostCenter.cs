using System;

namespace Architecture.Model;

public class CostCenter
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ParentCostCenterId { get; set; }

	public string CostCenterCode { get; set; }

	public string CostCenterName { get; set; }

	public virtual string ParentCostCenter { get; set; }
}
