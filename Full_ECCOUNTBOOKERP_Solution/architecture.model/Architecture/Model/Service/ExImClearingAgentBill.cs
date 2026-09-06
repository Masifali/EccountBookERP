using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.Service;

public class ExImClearingAgentBill
{
	public bool IsApproved { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime DueDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double GrandTotal { get; set; }

	public double NetBillWeight { get; set; }

	public double OtherChargesTotal { get; set; }

	public double TaxAmountTotal { get; set; }

	public double TotalAmount { get; set; }

	public double USDRate { get; set; }

	public decimal BookingRate { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DestinationPortId { get; set; }

	public int DocNo { get; set; }

	public int DocTypeId { get; set; }

	public int DueDays { get; set; }

	public int EntryUser { get; set; }

	public int ExportInvoiceId { get; set; }

	public int ForwarderId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int LcContractId { get; set; }

	public int LoadingPortId { get; set; }

	public int ModifyUser { get; set; }

	public int NoOfContainer { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int ServiceOrderId { get; set; }

	public int SupCustId { get; set; }

	public string ContainerNo { get; set; }

	public string RefBillNo { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public string AttachmentsValues { get; set; }

	public virtual string BilltoPartyName { get; set; }

	public virtual string ExportInvoiceNo { get; set; }

	public virtual string ServiceType { get; set; }

	public virtual List<ExImClearingAgentBillDetail> BillDetail { get; set; }

	public virtual List<ExImClearingAgentBillOtherChargesDetail> OtherChargesDetail { get; set; }

	public virtual List<ExImClearingAgentBillTaxDetail> TaxDetail { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }
}
