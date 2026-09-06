using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.Model.Steel.Inventory;

public class InvSaleInvoice
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

	public int CommAgentId { get; set; }

	public int CommUOM { get; set; }

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

	public int SupplierReferenceNo { get; set; }

	public int TransporterId { get; set; }

	public int VoucherId { get; set; }

	public string CommRemarks { get; set; }

	public string CommType { get; set; }

	public string DeliveryTerm { get; set; }

	public string ManualBillNo { get; set; }

	public string RemarksHeader { get; set; }

	public virtual List<InvSaleInvoiceDetail> InvSaleInvoiceDetaillist { get; set; }

	public virtual List<InvSaleInvoiceExpense> InvSaleInvoiceExpenseList { get; set; }

	public virtual List<InvSaleInvoiceFreight> InvSaleInvoiceFreightList { get; set; }

	public virtual List<InvSaleInvoiceJournal> InvSaleInvoiceJournalList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }

	public virtual CommonIdsForFinancials CommonIdsForFinancials { get; set; }

	public InvSaleInvoice()
	{
		InvSaleInvoiceDetaillist = new List<InvSaleInvoiceDetail>();
		InvSaleInvoiceExpenseList = new List<InvSaleInvoiceExpense>();
		InvSaleInvoiceFreightList = new List<InvSaleInvoiceFreight>();
		InvSaleInvoiceJournalList = new List<InvSaleInvoiceJournal>();
	}
}
