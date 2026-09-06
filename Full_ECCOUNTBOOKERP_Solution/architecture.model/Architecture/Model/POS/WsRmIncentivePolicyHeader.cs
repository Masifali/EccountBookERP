using System;
using System.Collections.Generic;

namespace Architecture.Model.POS;

public class WsRmIncentivePolicyHeader
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime SchemeDateFrom { get; set; }

	public DateTime SchemeDateTo { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int CrAdvoiceTerms { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public string RemarksHeader { get; set; }

	public string SchemeDescription { get; set; }

	public virtual int GlAccountId { get; set; }

	public virtual List<WsRmIncentivePolicyDetail> WsRmIncentivePolicyDetailsList { get; set; }
}
