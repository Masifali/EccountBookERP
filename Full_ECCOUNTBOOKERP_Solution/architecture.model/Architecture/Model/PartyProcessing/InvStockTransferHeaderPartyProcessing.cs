using System;
using System.Collections.Generic;

namespace Architecture.Model.PartyProcessing;

public class InvStockTransferHeaderPartyProcessing
{
	public bool IsApproved { get; set; }

	public DateTime? ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double OtherWeight { get; set; }

	public double WbNetWeight { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int FinancialYearId { get; set; }

	public int GatePassId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int NoOfBranches { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int WbTicketId { get; set; }

	public string RemarksHeader { get; set; }

	public string TransferType { get; set; }

	public virtual List<InvStockTransferDetailPartyProcessing> InvStockTransferDetailPartyProcessingList { get; set; }

	public virtual List<InventoryTransactionsPartyProcessingForFifo> InventoryTransactionsPartyProcessingForFIFOlist { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
