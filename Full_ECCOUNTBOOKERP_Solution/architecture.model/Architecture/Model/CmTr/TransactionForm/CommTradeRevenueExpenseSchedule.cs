using System;
using System.Collections.Generic;

namespace Architecture.Model.CmTr.TransactionForm;

public class CommTradeRevenueExpenseSchedule
{
	public int SupplierCustomerId { get; set; }

	public string PolicyCode { get; set; }

	public string RemarksHeader { get; set; }

	public string ScreenName { get; set; }

	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public virtual string SupplierCustomerName { get; set; }

	public virtual List<CommTradeRevenueExpenseScheduleDetail> CommTradeRevenueExpenseScheduleList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }
}
