using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class PurchaseOrder
{
	public bool IsAproved { get; set; }

	public bool PostState { get; set; }

	public DateTime DeliveryStartDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime OrderDueDate { get; set; }

	public DateTime OrderExpiryDate { get; set; }

	public DateTime? PostDate { get; set; }

	public double CommAmount { get; set; }

	public double CommRate { get; set; }

	public int BranchesId { get; set; }

	public int BrokerAgentSupCustId { get; set; }

	public int CatagorySrNo { get; set; }

	public int CompanyId { get; set; }

	public int DeliveryDays { get; set; }

	public int LocationTypeId { get; set; }

	public int DocNo { get; set; }

	public int BranchSrNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrderCatagoryId { get; set; }

	public int BaseDocumentTypeId { get; set; }

	public int OrderDueDays { get; set; }

	public int CityId { get; set; }

	public int OrderSupCustId { get; set; }

	public int OrganizationId { get; set; }

	public int PaymentTermsId { get; set; }

	public int PostUser { get; set; }

	public int ProjectsId { get; set; }

	public int RefrenenceParty { get; set; }

	public int SupplierCustomerIdStockParty { get; set; }

	public int UomScheduleIdCmRate { get; set; }

	public int BrokerAgentId { get; set; }

	public int BillCalculateTypeId { get; set; }

	public string BrokeryType { get; set; }

	public double BrokeryRate { get; set; }

	public double BrokeryUom { get; set; }

	public double BrokeryAmount { get; set; }

	public string CommissionRemarks { get; set; }

	public string CommissionType { get; set; }

	public string DeliveryRemarks { get; set; }

	public string DeliveryTerm { get; set; }

	public int DeliveryTermId { get; set; }

	public string OrderStatus { get; set; }

	public bool OrderTaxable { get; set; }

	public bool CashFreight { get; set; }

	public bool CreditFreight { get; set; }

	public string OrderType { get; set; }

	public string RemarksHeader { get; set; }

	public string SupplierRefNo { get; set; }

	public decimal OrderQty { get; set; }

	public decimal OrderWeight { get; set; }

	public decimal OrderAmount { get; set; }

	public int CurrencyId { get; set; }

	public int BookingPersonId { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public virtual string OrderDetailRemoveIds { get; set; }

	public virtual List<PurchaseOrderDetail> purchaseOrderDetailList { get; set; }

	public virtual List<PurchaseorderEmptyBags> PurchaseorderEmptyBagsList { get; set; }

	public virtual List<PurchaseOrderLabDeduction> PurchaseOrderLabDeductionList { get; set; }

	public virtual List<PurchaseOrderSupplierExpense> PurchaseOrderSupplierExpList { get; set; }

	public virtual List<PurchaseOrderSupplierDispatchDetail> SupplierDispatchDetailList { get; set; }

	public virtual List<PurchaseOrderPaymentTermsDetail> PurchaseOrderPaymentTermsDetailList { get; set; }

	public virtual List<PurchaseOrderExpensesChargeToProduct> PurchaseOrderExpensesChargeToProductsList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public PurchaseOrder()
	{
		AttachmentsList = new List<DMSAttachments>();
		DeleteAttachmentsList = new List<DMSAttachments>();
		purchaseOrderDetailList = new List<PurchaseOrderDetail>();
		PurchaseorderEmptyBagsList = new List<PurchaseorderEmptyBags>();
		PurchaseOrderLabDeductionList = new List<PurchaseOrderLabDeduction>();
		PurchaseOrderSupplierExpList = new List<PurchaseOrderSupplierExpense>();
		SupplierDispatchDetailList = new List<PurchaseOrderSupplierDispatchDetail>();
		PurchaseOrderExpensesChargeToProductsList = new List<PurchaseOrderExpensesChargeToProduct>();
	}
}
