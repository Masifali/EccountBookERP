using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.Import;

public class ExImFCBankPaymensts
{
	public bool IsApproved { get; set; }

	public bool PostState { get; set; }

	public DateTime DocumentDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public double BankDrAmount { get; set; }

	public double ExchangeRate { get; set; }

	public double FcBankChargesAmountRs { get; set; }

	public double FcFBCharges { get; set; }

	public double FcGrossAmount { get; set; }

	public double FcNetAmount { get; set; }

	public double NetAmountRs { get; set; }

	public int BankGlDrAc { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int ExImInvoiceId { get; set; }

	public int ExImLcOrderId { get; set; }

	public int FcBankChargesTypeAcId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int MultiCurrencyId { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int ProjectId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int VoucherHeadId { get; set; }

	public string BankFbpNo { get; set; }

	public string ExmLcPaymentTerm { get; set; }

	public string Remarks { get; set; }

	public virtual double VoucherExchangeRate { get; set; }

	public virtual List<ExImFcBankPaymentsDeductions> ExImFcBankPaymentsDeductionslst { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual VoucherHead FcBankVoucherHead { get; set; }
}
