using System;
using System.Collections.Generic;

namespace Architecture.Model.BankSchema;

public class pdcTransaction
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime cheqDate { get; set; }

	public DateTime docDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime voucherDate { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BankNameId { get; set; }

	public int BranchId { get; set; }

	public int CheqBookDetailId { get; set; }

	public int cheqTypeId { get; set; }

	public int CompanyId { get; set; }

	public int creditAccountId { get; set; }

	public int debitAccountId { get; set; }

	public int docNo { get; set; }

	public int documentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int payeeTitleId { get; set; }

	public int PendingForView { get; set; }

	public int ProjectId { get; set; }

	public int seqNo { get; set; }

	public int voucherCode { get; set; }

	public int voucherId { get; set; }

	public long pdcTransactionId { get; set; }

	public double cheqAmount { get; set; }

	public string cheqNo { get; set; }

	public string payeeTitle { get; set; }

	public string remarks { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
