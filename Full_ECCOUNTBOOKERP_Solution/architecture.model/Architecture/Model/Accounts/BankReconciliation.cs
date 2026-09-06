using System;
using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class BankReconciliation
{
	public bool IsApproved { get; set; }

	public bool IsReconciled { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime TransactionDate { get; set; }

	public decimal Credit { get; set; }

	public decimal Debit { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BankAccountId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int VoucherHeadId { get; set; }

	public int VoucherDetailId { get; set; }

	public string ChequeNo { get; set; }

	public string Particulars { get; set; }

	public string Remarks { get; set; }

	public virtual string BankAccountTitle { get; set; }

	public virtual string BankAccountCode { get; set; }

	public virtual int VoucherCode { get; set; }

	public virtual DateTime? VoucherDate { get; set; }

	public virtual List<BankReconciliation> BankReconciliationList { get; set; }
}
