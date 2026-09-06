using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.PartyProcessing;

public class InvProductionProcessingBill
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime BillDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal Rate { get; set; }

	public decimal Qty { get; set; }

	public decimal Weight { get; set; }

	public decimal Amount { get; set; }

	public decimal TotalBPPurchaseAmt { get; set; }

	public int FinancialYearId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BillNo { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int CreditAccountId { get; set; }

	public int Id { get; set; }

	public int InvProductionJobOrderId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int RateUomId { get; set; }

	public int StockPartyId { get; set; }

	public int ActionId { get; set; }

	public string ScreenName { get; set; }

	public string RemarksHeader { get; set; }

	public virtual List<InvProductionProcessingBillOH> InvProductionProcessingBillOHslist { get; set; }

	public virtual List<InvProductionProcessingBillPM> InvProductionProcessingBillPMslist { get; set; }

	public virtual List<InvProductionProcessingBillOutPut> InvProductionProcessingBillOutPutslist { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }
}
