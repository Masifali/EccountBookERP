using System;

namespace Architecture.Model;

public class MultiCurrency
{
	public bool CurrencyStatus { get; set; }

	public bool PostState { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public double CurrencyRate { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public string CurrencyCode { get; set; }

	public string CurrencyName { get; set; }

	public string CurrencySymbol { get; set; }

	public string Url { get; set; }
}
