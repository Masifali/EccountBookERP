using System;
using System.Collections.Generic;

namespace Architecture.Model.CRM;

public class CrmLeadGenerationHeader
{
	public bool IsActive { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime LeadDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int CompanyId { get; set; }

	public int EmpAssignToId { get; set; }

	public int EntryUserId { get; set; }

	public int LeadProcessStatus { get; set; }

	public int LeadsSourceId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public long CrmContactHeaderId { get; set; }

	public long LeadCode { get; set; }

	public long LeadGenHeaderId { get; set; }

	public string LeadStatus { get; set; }

	public string RamarksHeader { get; set; }

	public virtual List<CrmLeadGenerationDetail> crmLeadGenerationDetails { get; set; }
}
