using System;

namespace Architecture.Model.ChequePrinting;

public class Cheques
{
	public DateTime ChequeDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double Amount { get; set; }

	public int ChequeFormatId { get; set; }

	public int ChequeNumber { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public string PartyName { get; set; }

	public string ChequeType { get; set; }
}
