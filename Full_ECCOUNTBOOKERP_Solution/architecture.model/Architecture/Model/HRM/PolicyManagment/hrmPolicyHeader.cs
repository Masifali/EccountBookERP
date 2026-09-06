using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.PolicyManagment;

public class hrmPolicyHeader
{
	public DateTime AlteredOn { get; set; }

	public DateTime CreatedOn { get; set; }

	public DateTime DateFrom { get; set; }

	public DateTime DateTo { get; set; }

	public int ActionType { get; set; }

	public int AlteredBy { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int CreatedBy { get; set; }

	public int OrganizationId { get; set; }

	public int PolicyHeaderId { get; set; }

	public virtual List<hrmPolicyDetail> HrmPolicyDetailList { get; set; }
}
