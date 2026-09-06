using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.POS;

public class WsRmStockTransferNotes
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int DestinationLocationId { get; set; }

	public int DocNo { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int SourceLocationId { get; set; }

	public int WsRmRequisitionPoId { get; set; }

	public string RemarksHeader { get; set; }

	public string ReqStatus { get; set; }

	public virtual List<WsRmStockTransferNotesDetail> WsRmStockTransferNotesDetailsList { get; set; }

	public virtual List<WsRmStockTransferNotesExpenses> WsRmStockTransferNotesExpensesList { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }
}
