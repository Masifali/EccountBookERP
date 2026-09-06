using System;
using System.Collections.Generic;

namespace Architecture.Model.StoreManagement;

public class PurchasePreBillHeader
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double BillAmount { get; set; }

	public double DiscountAmount { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BillToPartyId { get; set; }

	public int BranchesId { get; set; }

	public int CityId { get; set; }

	public int CompanyId { get; set; }

	public int DeliveryTermId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int PurchasePreBillHeaderId { get; set; }

	public string ApprovalRemarks { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public string ReferenceNo { get; set; }

	public string RemarksHeader { get; set; }

	public virtual string BilltoPartyName { get; set; }

	public virtual string DeliveryTerm { get; set; }

	public virtual string CityName { get; set; }

	public virtual List<PurchasePreBillDetail> PurchasePreBillDetailList { get; set; }

	public virtual List<PurchasePreBillExpenseDetail> PurchasePreBillExpenseDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public PurchasePreBillHeader()
	{
		PurchasePreBillDetailList = new List<PurchasePreBillDetail>();
		PurchasePreBillExpenseDetailList = new List<PurchasePreBillExpenseDetail>();
		AttachmentsList = new List<DMSAttachments>();
		DeleteAttachmentsList = new List<DMSAttachments>();
	}
}
