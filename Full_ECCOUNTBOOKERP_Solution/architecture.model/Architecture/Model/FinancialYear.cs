using System;

namespace Architecture.Model;

public class FinancialYear
{
	public bool PostState { get; set; }

	public bool LoginStatus { get; set; }

	public bool DefaultFinYear { get; set; }

	public bool Status { get; set; }

	public DateTime End_Period { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public DateTime Start_Period { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public string FinancialYearCode { get; set; }
}
