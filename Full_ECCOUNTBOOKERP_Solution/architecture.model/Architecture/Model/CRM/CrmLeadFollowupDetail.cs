using System;

namespace Architecture.Model.CRM;

public class CrmLeadFollowupDetail
{
	public DateTime ContactDate { get; set; }

	public DateTime NextContatDate { get; set; }

	public int ContactedByEmpId { get; set; }

	public int ContactMediumId { get; set; }

	public int CrmLeadGenerationHeaderId { get; set; }

	public int LeadFollowupId { get; set; }

	public int ProcessStatusId { get; set; }

	public string ContactRemarks { get; set; }

	public string ContactNumbers { get; set; }

	public string MessageBody { get; set; }
}
