using System;

namespace Architecture.Model.ChequePrinting;

public class ChequePrintList
{
	public int Id { get; set; }

	public int VoucherId { get; set; }

	public int VoucherDetailId { get; set; }

	public int AccountId { get; set; }

	public int BankId { get; set; }

	public int ChequeId { get; set; }

	public string ChequeNumber { get; set; }

	public int ChequeStatus { get; set; }

	public DateTime ChequeDate { get; set; }

	public bool PrintStatus { get; set; }

	public int EntryUser { get; set; }

	public DateTime EntryDate { get; set; }

	public int ModifyUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public int PrintUser { get; set; }

	public DateTime PrintDate { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int D1 { get; set; }

	public int D2 { get; set; }

	public int M1 { get; set; }

	public int M2 { get; set; }

	public int Y1 { get; set; }

	public int Y2 { get; set; }

	public int Y3 { get; set; }

	public int Y4 { get; set; }
}
