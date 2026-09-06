using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.Model.pcc;

public class InvSaleInvoice
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DeliveryStartDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime DueDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ExpiryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public decimal BillAmount { get; set; }

	public decimal BillAmountWithoutDiscount { get; set; }

	public decimal SettlementDiscountHeader { get; set; }

	public decimal CommissionAmount { get; set; }

	public decimal CommissionRate { get; set; }

	public decimal CommissionUom { get; set; }

	public decimal DiscountAmountHeader { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal FrieghtAmountHeader { get; set; }

	public decimal InvoiceQty { get; set; }

	public decimal InvoiceWeight { get; set; }

	public decimal ItemAmountHeader { get; set; }

	public decimal ItemNetAmountHeader { get; set; }

	public decimal OtherWagesHeader { get; set; }

	public decimal PartyAddLessHeader { get; set; }

	public string Distance { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int SettlementDiscountAccountId { get; set; }

	public int CommissionDebitAcId { get; set; }

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

	public int OtherWagesAccountId { get; set; }

	public int PaymentTermsId { get; set; }

	public int PendingForView { get; set; }

	public int ProjectsId { get; set; }

	public int RefSalesManId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TransporterId { get; set; }

	public int VisitedById { get; set; }

	public int AutoUpdate { get; set; }

	public string CommissionRemarks { get; set; }

	public string CommissionType { get; set; }

	public string DeliveryRemarks { get; set; }

	public string DeliveryTerm { get; set; }

	public string ManualBillNo { get; set; }

	public string ReferencPartyAddress { get; set; }

	public string ReferencPartyCellNo { get; set; }

	public string ReferencPartyName { get; set; }

	public string RefrenenceNo { get; set; }

	public string RemarksHeader { get; set; }

	public int ReferencePartyId { get; set; }

	public int BuildingHeightId { get; set; }

	public int BuildingStoreyId { get; set; }

	public string BuildingArea { get; set; }

	public virtual List<InvSaleInvoiceJournal> InvSaleInvoiceJournalList { get; set; }

	public virtual List<InvSaleInvoiceExpense> InvSaleInvoiceExpenseList { get; set; }

	public virtual List<InvSaleInvoiceFreight> InvSaleInvoiceFreightList { get; set; }

	public virtual List<InvSaleInvoiceDetail> InvSaleInvoiceDetaillist { get; set; }

	public virtual List<InvSaleInvoiceWagesDetail> InvSaleInvoiceWagesDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }

	public virtual CommonIdsForFinancials CommonIdsForFinancials { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }
}
