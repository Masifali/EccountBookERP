using System;

namespace Architecture.Model.Accounts;

public class InvoiceWiseOpening
{
	public string InvoiceNo { get; set; }

	public decimal InvoiceAmount { get; set; }

	public decimal DebitAmount { get; set; }

	public int CompanyId { get; set; }

	public int BranchId { get; set; }

	public decimal ExchangeRate { get; set; }

	public int Id { get; set; }

	public short Status { get; set; }

	public DateTime InvoiceDate { get; set; }

	public int ModifyUserId { get; set; }

	public DateTime EntryDate { get; set; }

	public string Remarks { get; set; }

	public int PartyId { get; set; }

	public decimal CreditAmount { get; set; }

	public int EntryUserId { get; set; }

	public int CurrencyId { get; set; }

	public DateTime DueDate { get; set; }

	public decimal BalanceAmount { get; set; }

	public int FinancialYearId { get; set; }

	public int AccountId { get; set; }

	public DateTime ModifyDate { get; set; }

	public string InvoiceType { get; set; }

	public virtual string AccountTitle { get; set; }

	public virtual string PartyName { get; set; }

	public virtual string CurrencyName { get; set; }
}
