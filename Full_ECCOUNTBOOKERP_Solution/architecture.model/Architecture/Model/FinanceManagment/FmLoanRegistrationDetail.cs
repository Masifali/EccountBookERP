using System;

namespace Architecture.Model.FinanceManagment;

public class FmLoanRegistrationDetail
{
	public DateTime ApprovalDate { get; set; }

	public DateTime cExpiryDate { get; set; }

	public DateTime RegistrationDate { get; set; }

	public double cAmount { get; set; }

	public double cInterestRate { get; set; }

	public int cValidDays { get; set; }

	public int FmLoanRegistrationHeaderId { get; set; }

	public int Id { get; set; }

	public string caseDescription { get; set; }

	public string LoanColletralType { get; set; }

	public string RegistrationCode { get; set; }
}
