using System;

namespace Architecture.Model;

public class Branches
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int CompanyId { get; set; }

	public int Id { get; set; }

	public int OrganizationId { get; set; }

	public string BranchAddress { get; set; }

	public string BranchCode { get; set; }

	public string BranchContactPerson { get; set; }

	public string BranchEmailA { get; set; }

	public string BranchEmailB { get; set; }

	public string BranchLogo { get; set; }

	public string BranchMobileA { get; set; }

	public string BranchMobileB { get; set; }

	public string BranchMobileC { get; set; }

	public string BranchName { get; set; }

	public string BranchReportingTitle { get; set; }

	public string BranchTel { get; set; }
}
