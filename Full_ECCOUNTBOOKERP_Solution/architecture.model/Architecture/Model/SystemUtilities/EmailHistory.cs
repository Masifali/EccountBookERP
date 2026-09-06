using System;
using System.Collections.Generic;

namespace Architecture.Model.SystemUtilities;

public class EmailHistory
{
	public int? Id { get; set; }

	public string EmailFrom { get; set; }

	public string EmailTo { get; set; }

	public string EmailSubject { get; set; }

	public string EmailBody { get; set; }

	public string FileUrl1 { get; set; }

	public string FileUrl2 { get; set; }

	public bool Status { get; set; }

	public int EntryUser { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime? PostedDate { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }

	public virtual List<EmailHistory> EmailHistoryList { get; set; }
}
