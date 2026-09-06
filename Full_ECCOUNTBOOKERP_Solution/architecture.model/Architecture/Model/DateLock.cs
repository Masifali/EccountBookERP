using System;

namespace Architecture.Model;

public class DateLock
{
	public int id { get; set; }

	public DateTime Date { get; set; }

	public bool Status { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }

	public int ProjectsId { get; set; }

	public int EntryUserId { get; set; }

	public DateTime EntryDate { get; set; }

	public int ModifyUserId { get; set; }

	public DateTime ModifyDate { get; set; }

	public bool IsApproved { get; set; }

	public int ApprovedUserId { get; set; }

	public DateTime? ApprovedDate { get; set; }
}
