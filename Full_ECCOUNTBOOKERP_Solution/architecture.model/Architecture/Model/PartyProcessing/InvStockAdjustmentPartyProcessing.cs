using System;
using System.Collections.Generic;

namespace Architecture.Model.PartyProcessing;

public class InvStockAdjustmentPartyProcessing
{
	public bool IsApproved { get; set; }

	public DateTime DocDate { get; set; }

	public int ActionId { get; set; }

	public int AdjustmentTypeId { get; set; }

	public int ApprovalUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int StockPartyId { get; set; }

	public string RemarksHeader { get; set; }

	public virtual List<InvStockAdjustmentDetailPartyProcessing> InvStockAdjustmentDetailPartyProcessingList { get; set; }

	public virtual List<InventoryTransactionsPartyProcessingForFifo> InventoryTransactionsPartyProcessingForFIFOlist { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
