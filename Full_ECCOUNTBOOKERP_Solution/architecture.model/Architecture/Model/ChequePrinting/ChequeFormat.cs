using System;

namespace Architecture.Model.ChequePrinting;

public class ChequeFormat
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double ChequeHeight_InCm { get; set; }

	public double ChequeWdith_InCm { get; set; }

	public double Lef_Signature { get; set; }

	public double Left_AmountInFigures { get; set; }

	public double Left_AmountInWords_Line1 { get; set; }

	public double Left_Date { get; set; }

	public double Left_PartyName { get; set; }

	public double Top_AmountInFigures { get; set; }

	public double Top_AmountInWords_Line1 { get; set; }

	public double Top_Date { get; set; }

	public double Top_PartyName { get; set; }

	public double Top_Signature { get; set; }

	public double Top_AmountInWords_Line2 { get; set; }

	public double Left_AmountInWords_Line2 { get; set; }

	public int BankId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUser { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public string DateFormat { get; set; }

	public string FormatName { get; set; }

	public string Prefix_AmountInFigures { get; set; }

	public string Prefix_AmountInWords { get; set; }

	public string Prefix_PartyName { get; set; }

	public string Sufix_AmountInFigures { get; set; }

	public string Sufix_AmountInWords { get; set; }

	public string Sufix_PartyName { get; set; }
}
