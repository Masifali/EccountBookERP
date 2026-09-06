using System;
using System.Collections.Generic;

namespace Architecture.Model.Accounts;

public class VoucherInvoicesAdjustment
{
	public DateTime DueDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime VoucherDate { get; set; }

	public decimal AdjustmentAmount { get; set; }

	public decimal InvoiceAmount { get; set; }

	public decimal AdvanceAmount { get; set; }

	public decimal VoucherAmount { get; set; }

	public decimal ExchangeRate { get; set; }

	public int ActionTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int PartyGlId { get; set; }

	public int RefDocNoId { get; set; }

	public int ExImInvoicePaymentTermsDetailId { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TransactionTypeId { get; set; }

	public int VoucherCode { get; set; }

	public int VoucherHeadId { get; set; }

	public int PaymentTypeId { get; set; }

	public string Remarks { get; set; }

	public string ScreenName { get; set; }

	public int PaymentTermId { get; set; }

	public int FcyId { get; set; }

	public virtual string RefDocumentType { get; set; }

	public virtual string PaymentType { get; set; }

	public virtual string PaymentTerm { get; set; }

	public virtual string PartyName { get; set; }

	public virtual string PartyCode { get; set; }

	public virtual string FcyCode { get; set; }

	public virtual string PartyGlAccountTitle { get; set; }

	public virtual string RefDocNo { get; set; }

	public virtual DateTime? RefDocDate { get; set; }

	public virtual string PartyGlAccountCode { get; set; }

	public virtual int ExImInvoiceId { get; set; }

	public virtual List<VoucherInvoicesAdjustment> VoucherInvoicesAdjustmentList { get; set; }
}
