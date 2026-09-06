using System;
using System.Collections.Generic;

namespace Architecture.Model.Steel.Inventory;

public class PurchaseOrder
{
	public bool CashFreight { get; set; }

	public bool CreditFreight { get; set; }

	public bool IsApproved { get; set; }

	public bool OrderTaxable { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DeliveryStartDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime OrderDueDate { get; set; }

	public DateTime OrderExpiryDate { get; set; }

	public decimal BrokeryAmount { get; set; }

	public decimal BrokeryRate { get; set; }

	public decimal CommAmount { get; set; }

	public decimal CommRate { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal OrderAmount { get; set; }

	public decimal OrderQty { get; set; }

	public decimal OrderWeight { get; set; }

	public int CurrencyId { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int brokeryRateUom { get; set; }

	public int CatagorySrNo { get; set; }

	public int CommAgentId { get; set; }

	public int CommRateUom { get; set; }

	public int CompanyId { get; set; }

	public int DeliveryDays { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrderCatagoryId { get; set; }

	public int OrderDueDays { get; set; }

	public int OrganizationId { get; set; }

	public int PaymentTermsId { get; set; }

	public int PendingForView { get; set; }

	public int ProjectsId { get; set; }

	public int BrokerAgentId { get; set; }

	public int DocNo { get; set; }

	public int Id { get; set; }

	public int OrderSupplierId { get; set; }

	public string BrokeryRemarks { get; set; }

	public string BrokeryType { get; set; }

	public string CommissionRemarks { get; set; }

	public string CommissionType { get; set; }

	public string DeliveryRemarks { get; set; }

	public string DeliveryTerm { get; set; }

	public string OrderStatus { get; set; }

	public string RemarksHeader { get; set; }

	public string SupplierRefNo { get; set; }

	public virtual List<PurchaseOrderDetail> purchaseOrderDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
