using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;

namespace Architecture.Model.Export;

public class ExImFCBankReceipts
{
	public bool IsApproved { get; set; }

	public bool PostState { get; set; }

	public DateTime DocumentDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime? PostDate { get; set; }

	public double BankDrAmount { get; set; }

	public double ExchangeRate { get; set; }

	public double FcFBCharges { get; set; }

	public double FcGrossAmount { get; set; }

	public double FcNetAmount { get; set; }

	public double NetAmountRs { get; set; }

	public double ThirdCurrencyReceiverExchangeRate { get; set; }

	public double ThirdCurrencyReceiverFcyAmount { get; set; }

	public int DebitAccountCurrencyId { get; set; }

	public double DebitAccountCurrencyAmount { get; set; }

	public double DebitAccountCurrencyExchangeRate { get; set; }

	public int BankGlDrAc { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int ExImInvoiceId { get; set; }

	public int DebitAccountTypeId { get; set; }

	public int ExImLcOrderId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int MultiCurrencyId { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int ProjectId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int VoucherHeadId { get; set; }

	public int ThirdCurrencyId { get; set; }

	public int FBCDebitAccountId { get; set; }

	public int TransTypeId { get; set; }

	public double ThirdCurrencyHcyExchangeRate { get; set; }

	public double ThirdCurrencyFcyExchangeRate { get; set; }

	public double ThirdCurrencyAmount { get; set; }

	public string BankFbpNo { get; set; }

	public string ExmLcPaymentTerm { get; set; }

	public int FcBankChargesTypeAcId { get; set; }

	public double FcBankChargesAmountRs { get; set; }

	public string Remarks { get; set; }

	public string ReferenceNo { get; set; }

	public virtual string CompanyName { get; set; }

	public virtual string InvoiceNo { get; set; }

	public virtual string LcOrderNo { get; set; }

	public virtual string CurrencyCode { get; set; }

	public virtual string ThirdCurrencyCode { get; set; }

	public virtual double VoucherExchangeRate { get; set; }

	public virtual List<ExImFcBankReceiptsDeductions> FcBankReceiptDetail { get; set; }

	public virtual List<FcyBankReceiptBreakUp> FcyBankReceiptBreakUpList { get; set; }

	public virtual List<FcyBankReceiptBreakUpofPartyInvoices> FcyBankReceiptBreakUpofPartyInvoiceslist { get; set; }

	public virtual List<FcyPartyPaymentBreackUpForFinancials> FcyPartyPaymentBreackUpForFinancialslist { get; set; }

	public virtual List<ExImShipmentPartyAgainstAdvancesPayment> ExImShipmentPartyAgainstAdvancesPaymentslist { get; set; }

	public virtual List<FcyBankReceiptPaymentTermDetail> FcyBankReceiptPaymentTermDetails { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual VoucherHead FcBankVoucherHead { get; set; }

	public virtual List<GainLossBreakup> gainLossBreakupslist { get; set; }
}
