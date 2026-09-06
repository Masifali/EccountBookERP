using System;
using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class AccountsBudgetHeader
{
	public bool IsApproved { get; set; }

	public DateTime? ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal BudgetTotalAmount { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int ExpenseGLAcId { get; set; }

	public DateTime FnFromMonth { get; set; }

	public DateTime FnToMonth { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int DocumentTypeId { get; set; }

	public int FinancialYearId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public long DocNo { get; set; }

	public virtual List<AccountsBudgetDetail> accountBudgetDetails { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
