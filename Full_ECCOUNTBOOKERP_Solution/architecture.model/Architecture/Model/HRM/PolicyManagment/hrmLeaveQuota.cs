using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.PolicyManagment;

public class hrmLeaveQuota
{
	public bool IsActive { get; set; }

	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime FromDate { get; set; }

	public DateTime ToDate { get; set; }

	public int ActionTypeId { get; set; }

	public int AlteredById { get; set; }

	public int CompanyId { get; set; }

	public int CreatedById { get; set; }

	public int LeaveQuotaId { get; set; }

	public int LocationId { get; set; }

	public int OrganizationId { get; set; }

	public long UserLogId { get; set; }

	public string Description { get; set; }

	public virtual List<hrmLeaveQuotaDetail> HrmLeaveQuotaDetailList { get; set; }
}
