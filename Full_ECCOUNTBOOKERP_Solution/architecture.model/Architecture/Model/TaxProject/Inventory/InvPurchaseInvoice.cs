using System;
using System.Collections.Generic;
using Architecture.Model.TaxProject.Accounts;

namespace Architecture.Model.TaxProject.Inventory;

public class InvPurchaseInvoice
{
	public bool IsApproved { get; set; }

	public bool CustomAccounts { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime DueDate { get; set; }

	public DateTime? PostDate { get; set; }

	public DateTime? DeliveryStartDate { get; set; }

	public DateTime SupplierInvoiceDate { get; set; }

	public double BillAmount { get; set; }

	public double CashReceived { get; set; }

	public double CommAmount { get; set; }

	public double CommRate { get; set; }

	public double FreightAmount { get; set; }

	public int TransportAccountId { get; set; }

	public int BranchesId { get; set; }

	public int BranchSrNo { get; set; }

	public int TransporterCreditPartyId { get; set; }

	public int TransporterDebitGLId { get; set; }

	public int TransporterDebitPartyId { get; set; }

	public int FinancialYearId { get; set; }

	public int CommissionAgentId { get; set; }

	public int CommissionCreditAccountId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int SalesTaxNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int DocumentTypeSrNo { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int ProjectsId { get; set; }

	public int TaxAccountId { get; set; }

	public int ReferencePartyId { get; set; }

	public int StockPartyId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int SupplierInvoiceNo { get; set; }

	public string SupplierReferenceNo { get; set; }

	public int CurrencyId { get; set; }

	public string ReferencePartyName { get; set; }

	public int DueDays { get; set; }

	public int ActionId { get; set; }

	public string CommissionRemarks { get; set; }

	public string CommissionType { get; set; }

	public string IsAttachments { get; set; }

	public bool IsTaxable { get; set; }

	public string ManualBillNo { get; set; }

	public string OtherRemarks { get; set; }

	public string RemarksHeader { get; set; }

	public string ScreenName { get; set; }

	public string UomScheduleIdCmRate { get; set; }

	public string DeliveryTerm { get; set; }

	public int PaymentTermsId { get; set; }

	public double DiscountAmount { get; set; }

	public int DiscountAccountId { get; set; }

	public int DeliveryDays { get; set; }

	public int BrokerAgentId { get; set; }

	public int InvoiceTypeId { get; set; }

	public int GhallaMandiId { get; set; }

	public int BillCalculateTypeId { get; set; }

	public string BrokeryType { get; set; }

	public double BrokeryRate { get; set; }

	public double BrokeryUom { get; set; }

	public double BrokeryAmount { get; set; }

	public double WagesAmount { get; set; }

	public decimal InvoiceQty { get; set; }

	public decimal InvoiceWeight { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public string FreightRemark { get; set; }

	public virtual int GpSrNo { get; set; }

	public virtual DateTime PreviousDate { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }

	public virtual CommonIdsForFinancials CommonIdsForFinancials { get; set; }

	public virtual List<InvPurchaseInvoiceJournal> InvPurchaseInvoicejournalList { get; set; }

	public virtual List<InvPurchaseInvoiceExpense> InvPurchaseInvoiceExpList { get; set; }

	public virtual List<InvPurchaseInvoiceFreight> InvPurchaseInvoiceFreightList { get; set; }

	public virtual List<InvPurchaseInvoiceDetail> invPurchaseInvoiceDetailList { get; set; }

	public virtual List<InvPurchaseInvoiceEmptyBags> InvPurchaseInvoiceEmptyBagslist { get; set; }

	public virtual List<InvPurchaseInvoicePaymentTermsDetail> InvPurchaseInvoicePaymentTermsDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<PaymentDueSchedule> PaymentDuesSchedules { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }

	public InvPurchaseInvoice()
	{
		invPurchaseInvoiceDetailList = new List<InvPurchaseInvoiceDetail>();
		InvPurchaseInvoicejournalList = new List<InvPurchaseInvoiceJournal>();
		InvPurchaseInvoiceExpList = new List<InvPurchaseInvoiceExpense>();
		InvPurchaseInvoiceFreightList = new List<InvPurchaseInvoiceFreight>();
		InvPurchaseInvoicePaymentTermsDetailList = new List<InvPurchaseInvoicePaymentTermsDetail>();
		InvPurchaseInvoiceEmptyBagslist = new List<InvPurchaseInvoiceEmptyBags>();
		PaymentDuesSchedules = new List<PaymentDueSchedule>();
	}
}
