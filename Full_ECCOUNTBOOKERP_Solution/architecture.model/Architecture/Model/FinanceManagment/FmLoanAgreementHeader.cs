using System;
using System.Collections.Generic;

namespace Architecture.Model.FinanceManagment;

public class FmLoanAgreementHeader
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ExpiryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double ApprovedAmount { get; set; }

	public double InterestRate { get; set; }

	public int ApprovedUserId { get; set; }

	public int BankGLAccountId { get; set; }

	public int BankId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int FmFundTypesId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int ValidDays { get; set; }

	public string FacilityCode { get; set; }

	public string InterestType { get; set; }

	public virtual List<FmLoanAgreementDetail> FmLoanAgreementDetails { get; set; }
}
