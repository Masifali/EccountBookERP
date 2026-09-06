using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.Inventory;

public class InvStockTransferHeader
{
	public int Id { get; set; }

	public int DocNo { get; set; }

	public DateTime DocDate { get; set; }

	public int GatePassId { get; set; }

	public int WbTicketId { get; set; }

	public double WbNetWeight { get; set; }

	public double OtherWeight { get; set; }

	public string RemarksHeader { get; set; }

	public string TransferType { get; set; }

	public DateTime EntryDate { get; set; }

	public int EntryUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ModifyUser { get; set; }

	public bool IsApproved { get; set; }

	public int ApprovedUserId { get; set; }

	public DateTime? ApprovedDate { get; set; }

	public int TransitWarehouseId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }

	public int ProjectsId { get; set; }

	public int DocumentTypeId { get; set; }

	public int FinancialYearId { get; set; }

	public int NoOfBranches { get; set; }

	public int FromBranchId { get; set; }

	public int ToBranchId { get; set; }

	public virtual int GpSrNo { get; set; }

	public virtual int TicketNo { get; set; }

	public virtual int WorkingReportNo { get; set; }

	public virtual string BranchName { get; set; }

	public virtual string ProjectName { get; set; }

	public virtual DateTime PreviousDate { get; set; }

	public virtual string DetailRowsRemoveIds { get; set; }

	public virtual List<InvStockTransferDetail> InvStockTransferDetail { get; set; }

	public virtual List<InvStockTransferExpense> InvStockTransferExpense { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public InvStockTransferHeader()
	{
		AttachmentsList = new List<DMSAttachments>();
		DeleteAttachmentsList = new List<DMSAttachments>();
		InvStockTransferDetail = new List<InvStockTransferDetail>();
		InvStockTransferExpense = new List<InvStockTransferExpense>();
	}
}
