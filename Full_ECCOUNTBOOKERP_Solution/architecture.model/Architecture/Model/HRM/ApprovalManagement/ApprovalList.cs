using System;
using System.Collections.Generic;

namespace Architecture.Model.HRM.ApprovalManagement;

public class ApprovalList
{
	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }

	public int FinancialYearId { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public long Id { get; set; }

	public long DetailId { get; set; }

	public bool ActionTypeId { get; set; }

	public string ReqType { get; set; }

	public DateTime PostDate { get; set; }

	public double Rate { get; set; }

	public double Weight { get; set; }

	public string Comments { get; set; }

	public string ScreenName { get; set; }

	public bool Status { get; set; }

	public virtual List<ApprovalList> AllApprovalLists { get; set; }
}
