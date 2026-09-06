using System;
using System.Collections.Generic;

namespace Architecture.Model.CommissionAgent;

public class purchaseOrderMaster
{
	public bool isApproved { get; set; }

	public bool isSupplierOtherChargesAllowed { get; set; }

	public bool isWhtApplied { get; set; }

	public DateTime approvedDate { get; set; }

	public DateTime deliveryStartDate { get; set; }

	public DateTime docDate { get; set; }

	public DateTime entryDate { get; set; }

	public DateTime modifyDate { get; set; }

	public DateTime ValidityDate { get; set; }

	public decimal fcyAmount { get; set; }

	public decimal fcyFxRate { get; set; }

	public int actionId { get; set; }

	public int approvedUserId { get; set; }

	public int branchId { get; set; }

	public int commissionAgentId { get; set; }

	public int companyId { get; set; }

	public int deliveryDays { get; set; }

	public int deliveryTermId { get; set; }

	public int docNo { get; set; }

	public int documentTypeId { get; set; }

	public int EBWeightDeductionTermId { get; set; }

	public int entryUserId { get; set; }

	public int fcyId { get; set; }

	public int financialYearId { get; set; }

	public int modifyUserId { get; set; }

	public int organizationId { get; set; }

	public int projectId { get; set; }

	public int purchaseOrderMasterId { get; set; }

	public int statusId { get; set; }

	public int supplierId { get; set; }

	public string approvalRemarks { get; set; }

	public string attachmentsValues { get; set; }

	public string customAttachmentsValues { get; set; }

	public string remarksHeader { get; set; }

	public string paymentScheduleDescription { get; set; }

	public string purchaseOrderEmptyBagDetailDescription { get; set; }

	public string purchaseOrderEmptyBagDetailDescriptionII { get; set; }

	public string purchaseOrderSupplierExpenseDetailDescription { get; set; }

	public string purchaseOrderCommissionDetailDescription { get; set; }

	public virtual string CommissionAgentName { get; set; }

	public virtual string SupplierName { get; set; }

	public virtual string DeliveryTerm { get; set; }

	public virtual string status { get; set; }

	public string ShipToAddress { get; set; }

	public int shipToAddressId { get; set; }

	public int DeliveryToPartyId { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<purchaseOrderDetail> purchaseOrderDetailList { get; set; }

	public virtual List<purchaseOrderPaymentDetail> purchaseOrderPaymentDetailList { get; set; }

	public virtual List<purchaseOrderEmptyBagDetail> PurchaseOrderEmptyBagDetailList { get; set; }

	public virtual List<purchaseOrderSupplierExpenseDetail> purchaseOrderSupplierExpenseDetailList { get; set; }

	public virtual List<purchaseOrderCommissionDetail> PurchaseOrderCommissionDetailList { get; set; }

	public virtual List<purchaseOrderSaleOrderMapping> purchaseOrderSaleOrderMappingList { get; set; }

	public virtual List<SupplierOfferBuyerInquiryMappingDetail> SupplierOfferBuyerInquiryMappingDetailList { get; set; }

	public purchaseOrderMaster()
	{
		AttachmentsList = new List<DMSAttachments>();
		DeleteAttachmentsList = new List<DMSAttachments>();
		purchaseOrderDetailList = new List<purchaseOrderDetail>();
		purchaseOrderPaymentDetailList = new List<purchaseOrderPaymentDetail>();
		PurchaseOrderEmptyBagDetailList = new List<purchaseOrderEmptyBagDetail>();
		purchaseOrderSupplierExpenseDetailList = new List<purchaseOrderSupplierExpenseDetail>();
		purchaseOrderSaleOrderMappingList = new List<purchaseOrderSaleOrderMapping>();
	}
}
