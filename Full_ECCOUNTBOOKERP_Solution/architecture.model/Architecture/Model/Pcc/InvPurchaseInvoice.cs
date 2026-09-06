using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.Model.pcc;

public class InvPurchaseInvoice
{
	public long RowVersion { get; set; }

	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DeliveryStartDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime DueDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ExpiryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal BillAmount { get; set; }

	public decimal CommissionAmount { get; set; }

	public decimal CommissionRate { get; set; }

	public decimal CommissionUom { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal FrieghtAmount { get; set; }

	public decimal InvoiceQty { get; set; }

	public decimal InvoiceWeight { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

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

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PaymentTermsId { get; set; }

	public int ProjectsId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TaxAccountId { get; set; }

	public int TransporterId { get; set; }

	public string CommissionRemarks { get; set; }

	public string CommissionType { get; set; }

	public string DeliveryRemarks { get; set; }

	public string DeliveryTerm { get; set; }

	public string ManualBillNo { get; set; }

	public string RemarksHeader { get; set; }

	public string TransporterRemarks { get; set; }

	public virtual List<InvPurchaseInvoiceDetail> InvPurchaseInvoiceDetailList { get; set; }

	public virtual List<InvPurchaseInvoiceJournal> InvPurchaseInvoiceJournalList { get; set; }

	public virtual List<InvPurchaseInvoiceExpense> InvPurchaseInvoiceExpenseList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }

	public virtual CommonIdsForFinancials CommonIdsForFinancials { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }
}
