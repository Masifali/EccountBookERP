using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.POS;

public class WsRmCreditNoteHeader
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PeriodFrom { get; set; }

	public DateTime PeriodTo { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public long DebitAccountId { get; set; }

	public long Id { get; set; }

	public string RemarksHeader { get; set; }

	public virtual List<WsRmCreditNoteDetail> WsRmCreditNoteDetailsList { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }
}
