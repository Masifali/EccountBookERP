using System;
using System.Collections.Generic;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;

namespace Architecture.Model.Tax.Purchase;

public class PurchaseInvoice
{
	public bool IsApproved { get; set; }

	public bool IsTaxable { get; set; }

	public DateTime DeliveryStartDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime DueDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime PostDate { get; set; }

	public DateTime SupplierInvoiceDate { get; set; }

	public DateTime UnApproveDate { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal InvoiceQty { get; set; }

	public decimal InvoiceWeight { get; set; }

	public double BillAmount { get; set; }

	public double BrokeryAmount { get; set; }

	public double BrokeryRate { get; set; }

	public double BrokeryUom { get; set; }

	public double CommAmount { get; set; }

	public double CommRate { get; set; }

	public double FreightAmount { get; set; }

	public double WagesAmount { get; set; }

	public int ActionId { get; set; }

	public int BillCalculateTypeId { get; set; }

	public int BranchesId { get; set; }

	public int BrokerAgentId { get; set; }

	public int CommissionAgentId { get; set; }

	public int CompanyId { get; set; }

	public int CurrencyId { get; set; }

	public int DeliveryDays { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int DueDays { get; set; }

	public int EntryUser { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int PaymentTermsId { get; set; }

	public int PostUser { get; set; }

	public int ProjectsId { get; set; }

	public int ReferencePartyId { get; set; }

	public int RevisionNo { get; set; }

	public int SupplierCustomerId { get; set; }

	public int SupplierInvoiceNo { get; set; }

	public int SupplierReferenceNo { get; set; }

	public int TaxAccountId { get; set; }

	public int TransportAccountId { get; set; }

	public int UnApproveUserId { get; set; }

	public string BrokeryType { get; set; }

	public string CommissionRemarks { get; set; }

	public string CommissionType { get; set; }

	public string DeliveryTerm { get; set; }

	public string ManualBillNo { get; set; }

	public string RemarksHeader { get; set; }

	public string ScreenName { get; set; }

	public string UomScheduleIdCmRate { get; set; }

	public virtual VoucherHead VoucherHeadInvoices { get; set; }

	public virtual CommonIdsForFinancials CommonIdsForFinancials { get; set; }

	public virtual List<PurchaseInvoiceJournal> PurchaseInvoicejournalList { get; set; }

	public virtual List<PurchaseInvoiceExpense> PurchaseInvoiceExpList { get; set; }

	public virtual List<PurchaseInvoiceFreight> PurchaseInvoiceFreightList { get; set; }

	public virtual List<PurchaseInvoiceDetail> PurchaseInvoiceDetailList { get; set; }

	public virtual List<PurchaseInvoiceEmptyBags> PurchaseInvoiceEmptyBagsList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<PaymentDueSchedule> PaymentDuesSchedules { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }

	public PurchaseInvoice()
	{
		PurchaseInvoiceDetailList = new List<PurchaseInvoiceDetail>();
		PurchaseInvoicejournalList = new List<PurchaseInvoiceJournal>();
		PurchaseInvoiceExpList = new List<PurchaseInvoiceExpense>();
		PurchaseInvoiceFreightList = new List<PurchaseInvoiceFreight>();
	}
}
