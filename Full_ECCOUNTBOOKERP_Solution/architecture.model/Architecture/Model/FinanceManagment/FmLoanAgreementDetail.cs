namespace Architecture.Model.FinanceManagment;

public class FmLoanAgreementDetail
{
	public double fAmount { get; set; }

	public double fStockQty { get; set; }

	public double QtyRate { get; set; }

	public int FmLoanAgreementHeaderId { get; set; }

	public int Id { get; set; }

	public string ColletralSecurityDesc { get; set; }

	public string LoanColletralType { get; set; }
}
