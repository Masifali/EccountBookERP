using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.Model.Steel.Inventory;

public class InvPurchaseInvoice
{
	public bool IsApproved { get; set; }

	public bool IsTaxable { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DeliverystartDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime DueDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ExpiryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal BillAmount { get; set; }

	public decimal BrokeryAmount { get; set; }

	public decimal BrokeryRate { get; set; }

	public decimal BrokeryRateUom { get; set; }

	public decimal CommAmount { get; set; }

	public decimal CommRate { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal FreightAmount { get; set; }

	public decimal TotalQty { get; set; }

	public decimal TotalWeight { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int BrokerAgentId { get; set; }

	public int CommissionAgentId { get; set; }

	public int CompanyId { get; set; }

	public int CurrencyId { get; set; }

	public int DeliveryDays { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int DueDays { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int InvoiceTypeId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PaymentTermsId { get; set; }

	public int ProjectsId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TransportAccountId { get; set; }

	public int VoucherId { get; set; }

	public string BrokeryRemarks { get; set; }

	public string BrokeryType { get; set; }

	public string CommissionRemarks { get; set; }

	public string CommissionType { get; set; }

	public string CommRateUom { get; set; }

	public string DeliveryTerm { get; set; }

	public string ManualBillNo { get; set; }

	public string RemarksHeader { get; set; }

	public string SupplierReference { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }

	public virtual CommonIdsForFinancials CommonIdsForFinancials { get; set; }

	public virtual List<InvPurchaseInvoiceJournal> InvPurchaseInvoicejournalList { get; set; }

	public virtual List<InvPurchaseInvoiceExpense> InvPurchaseInvoiceExpList { get; set; }

	public virtual List<InvPurchaseInvoiceFreight> InvPurchaseInvoiceFreightList { get; set; }

	public virtual List<InvPurchaseInvoiceDetail> invPurchaseInvoiceDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public InvPurchaseInvoice()
	{
		invPurchaseInvoiceDetailList = new List<InvPurchaseInvoiceDetail>();
		InvPurchaseInvoicejournalList = new List<InvPurchaseInvoiceJournal>();
		InvPurchaseInvoiceExpList = new List<InvPurchaseInvoiceExpense>();
		InvPurchaseInvoiceFreightList = new List<InvPurchaseInvoiceFreight>();
	}
}
