using System;

namespace Architecture.Model.CRM;

public class CrmLeadGenerationDetail
{
	public DateTime SatusChangeDate { get; set; }

	public int ProcessStatusId { get; set; }

	public long CrmLeadGenerationHeaderId { get; set; }

	public long LeadGenDetailId { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string ProcessStatusNameDetail { get; set; }
}
