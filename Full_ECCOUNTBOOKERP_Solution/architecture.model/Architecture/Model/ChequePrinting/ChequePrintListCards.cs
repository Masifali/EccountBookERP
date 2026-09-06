using System;

namespace Architecture.Model.ChequePrinting;

public class ChequePrintListCards
{
	public int Id { get; set; }

	public string ChequeNumber { get; set; }

	public DateTime ChequeDate { get; set; }

	public double Amount { get; set; }

	public int D1 { get; set; }

	public int D3 { get; set; }

	public int M1 { get; set; }

	public int M2 { get; set; }

	public int Y1 { get; set; }

	public int Y2 { get; set; }

	public int Y3 { get; set; }

	public int Y4 { get; set; }

	public string BranchName { get; set; }

	public string BranchAddress { get; set; }

	public string BranchCode { get; set; }

	public string ChequeTemplete { get; set; }

	public int ChartOfAccountId { get; set; }

	public string CheqType { get; set; }

	public byte[] CheqTypeImage { get; set; }

	public byte[] ChequeImage { get; set; }

	public string AmountInWords { get; set; }

	public string PayeeTitle { get; set; }
}
