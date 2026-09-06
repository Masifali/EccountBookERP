using System;
using System.Collections.Generic;

namespace Architecture.Model.FinanceManagment;

public class FmLoanRegistrationHeader
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int BankGLAccountId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int FmFundTypesId { get; set; }

	public int FmLoanAgreementHeaderId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public string LoanCodeNo { get; set; }

	public virtual List<FmLoanRegistrationDetail> FmLoanRegistrationDetails { get; set; }
}
