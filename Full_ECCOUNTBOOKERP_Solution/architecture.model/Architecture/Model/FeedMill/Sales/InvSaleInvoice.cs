using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.Model.FeedMill.Sales;

public class InvSaleInvoice
{
	public bool IsApproved { get; set; }

	public bool IsReserve { get; set; }

	public bool IsTaxable { get; set; }

	public DateTime CustomerApprovalDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime DueDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public DateTime SupplierInvoiceDate { get; set; }

	public decimal DiscountAmount { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal InvoiceQty { get; set; }

	public decimal InvoiceWeight { get; set; }

	public double BillAmount { get; set; }

	public double CommAmount { get; set; }

	public double CommRate { get; set; }

	public double FreightAmount { get; set; }

	public double UnloadingDisc { get; set; }

	public int UnloadingDiscAccountId { get; set; }

	public int ActionId { get; set; }

	public int BranchesId { get; set; }

	public int CommissionAgentId { get; set; }

	public int CommissionDebitAcGLId { get; set; }

	public int CompanyId { get; set; }

	public int CurrencyId { get; set; }

	public int CustomerApproval { get; set; }

	public int DiscountAccountId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int DueDays { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PaymentTermId { get; set; }

	public int PostUserId { get; set; }

	public int ProjectsId { get; set; }

	public int ReferencePartyId { get; set; }

	public int SalesTaxNo { get; set; }

	public int SupplierCustomerId { get; set; }

	public int SupplierInvoiceNo { get; set; }

	public int SupplierReferenceNo { get; set; }

	public int TaxAccountId { get; set; }

	public int TransporterCreditPartyId { get; set; }

	public int TransporterDebitGLId { get; set; }

	public int TransporterDebitPartyId { get; set; }

	public int TransporterId { get; set; }

	public string CommissionRemarks { get; set; }

	public string CommissionType { get; set; }

	public string CommRateUom { get; set; }

	public string CustomerApprovalRemarks { get; set; }

	public string DeliveryTerm { get; set; }

	public string FreightRemark { get; set; }

	public string ManualBillNo { get; set; }

	public string OtherRemarks { get; set; }

	public string RemarksHeader { get; set; }

	public string ScreenName { get; set; }

	public virtual List<InvSaleInvoiceJournal> InvSaleInvoiceJournalList { get; set; }

	public virtual List<InvSaleInvoiceExpense> InvSaleInvoiceExpenseList { get; set; }

	public virtual List<InvSaleInvoiceFreight> InvSaleInvoiceFreightList { get; set; }

	public virtual List<InvSaleInvoiceDetail> InvSaleInvoiceDetaillist { get; set; }

	public virtual List<InvSaleInvoiceCommission> InvSaleInvoiceCommission { get; set; }

	public virtual List<SaleInvoicePaymentTermsDetail> SaleInvoicePaymentTermsDetailslist { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }

	public virtual CommonIdsForFinancials CommonIdsForFinancials { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }
}
