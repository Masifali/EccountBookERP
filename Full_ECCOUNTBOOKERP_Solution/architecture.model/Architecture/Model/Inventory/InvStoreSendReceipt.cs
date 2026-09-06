using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class InvStoreSendReceipt
{
	public DateTime ApprovedDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public int ModifyUserId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchId { get; set; }

	public int FinancialYearId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int InvStoreSendReceiptId { get; set; }

	public int OrganizationId { get; set; }

	public int ReceiverWarehouseId { get; set; }

	public int SenderWarehouseId { get; set; }

	public string ScreenName { get; set; }

	public virtual List<InvStoreSendReceiptDetail> invStoreSendReceiptDetailsList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
