using System;
using System.Collections.Generic;

namespace Architecture.Model.CmTr.TransactionForm;

public class CommTradeLoadingDelivery
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public string ManualRefNo { get; set; }

	public string RemarksHeader { get; set; }

	public string ScreenName { get; set; }

	public virtual List<CommTradeLoadingDeliveryDetail> CommTradeLoadingDeliveryDetailList { get; set; }

	public virtual List<CommTradeLoadingDeliveryRevExp> CommTradeLoadingDeliveryRevExpList { get; set; }

	public virtual List<CommTradeLoadingDeliverySupplierExp> CommTradeLoadingDeliverySupplierExpList { get; set; }

	public virtual List<CommTradeLoadingDeliveryPaymentTermsDetail> CommTradeLoadingDeliveryPaymentTermsDetailList { get; set; }

	public virtual List<CommTradeLoadingDeliveryPmOther> CommTradeLoadingDeliveryPmOtherList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
