using System;
using System.Collections.Generic;
using Architecture.Model.TaxProject.Accounts;
using Architecture.Model.TaxProject.Inventory;

namespace Architecture.Model.TaxProject.Export;

public class ExportVoucher
{
	public bool IsApproved { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime? DueDate { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal LcyAmount { get; set; }

	public decimal NetWeight { get; set; }

	public int DueDays { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int CreditAccounId { get; set; }

	public int CommissionExpenseAccountId { get; set; }

	public int CurrencyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int ExImInvoiceId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int SupplierCustomerId { get; set; }

	public string Remarks { get; set; }

	public string InvoiceNo { get; set; }

	public double FcyAmountWithoutAddLess { get; set; }

	public double AddLessAmount { get; set; }

	public virtual List<ExportVoucherDetail> ExportVoucherDetailslist { get; set; }

	public virtual List<ExImInvoicePaymentTermsDetail> ExImInvoicePaymentTermsDetailList { get; set; }

	public virtual List<ExImInvoicePaymentTermsDetailCustom> ExImInvoicePaymentTermsDetailListCustom { get; set; }

	public virtual List<ExImInvoiceOtherChargesDetail> ExImInvoiceOtherChargesDetailList { get; set; }

	public virtual List<ExImInvoiceOtherChargesDetailCustom> ExImInvoiceOtherChargesDetailListCustom { get; set; }

	public virtual List<ExImInvoiceSaleManCommission> ExImInvoiceSaleManCommissionList { get; set; }

	public virtual VoucherHead VoucherHeadExImInvoices { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }

	public ExportVoucher()
	{
		ExImInvoiceOtherChargesDetailList = new List<ExImInvoiceOtherChargesDetail>();
		ExImInvoicePaymentTermsDetailList = new List<ExImInvoicePaymentTermsDetail>();
		ExImInvoiceSaleManCommissionList = new List<ExImInvoiceSaleManCommission>();
		DeleteAttachmentsList = new List<DMSAttachments>();
	}
}
