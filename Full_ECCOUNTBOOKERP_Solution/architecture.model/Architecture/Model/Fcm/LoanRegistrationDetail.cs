using System;

namespace Architecture.Model.Fcm;

public class LoanRegistrationDetail
{
	public DateTime ApprovalDate { get; set; }

	public DateTime ExpiryDate { get; set; }

	public DateTime LoanCaseDate { get; set; }

	public decimal Amount { get; set; }

	public decimal InterestRate { get; set; }

	public int ActionTypeId { get; set; }

	public int collateralSecurityTypeId { get; set; }

	public int SortNo { get; set; }

	public int ValidDays { get; set; }

	public long LoanRegistrationDetailId { get; set; }

	public long LoanRegistrationHeaderId { get; set; }

	public string caseDescription { get; set; }

	public string LoanCaseNo { get; set; }

	public virtual string LoanSecutiryType { get; set; }
}
