using System;
using System.Collections.Generic;

namespace Architecture.Model.CommissionAgent;

public class saleOrderMaster
{
	public bool isApproved { get; set; }

	public bool isbuyerOtherChargesAllowed { get; set; }

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

	public int buyerId { get; set; }

	public int DeliveryToPartyId { get; set; }

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

	public int saleOrderMasterId { get; set; }

	public int statusId { get; set; }

	public string approvalRemarks { get; set; }

	public string attachmentsValues { get; set; }

	public string buyerReferenceNo { get; set; }

	public string customAttachmentsValues { get; set; }

	public string paymentScheduleDescription { get; set; }

	public string remarksHeader { get; set; }

	public string saleOrderbuyerExpenseDetailDescription { get; set; }

	public string saleOrderCommissionDetailDescription { get; set; }

	public string saleOrderEmptyBagDetailDescription { get; set; }

	public string ShipToAddress { get; set; }

	public int shipToAddressId { get; set; }

	public virtual string CommissionAgentName { get; set; }

	public virtual string BuyerName { get; set; }

	public virtual string DeliveryTerm { get; set; }

	public virtual string status { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<saleOrderDetail> saleOrderDetailList { get; set; }

	public virtual List<saleOrderPaymentDetail> saleOrderPaymentDetailList { get; set; }

	public virtual List<saleOrderEmptyBagDetail> saleOrderEmptyBagDetailList { get; set; }

	public virtual List<saleOrderbuyerExpenseDetail> saleOrderBuyerExpenseDetailList { get; set; }

	public virtual List<saleOrderCommissionDetail> saleOrderCommissionDetailList { get; set; }

	public saleOrderMaster()
	{
		AttachmentsList = new List<DMSAttachments>();
		DeleteAttachmentsList = new List<DMSAttachments>();
		saleOrderDetailList = new List<saleOrderDetail>();
		saleOrderPaymentDetailList = new List<saleOrderPaymentDetail>();
		saleOrderEmptyBagDetailList = new List<saleOrderEmptyBagDetail>();
		saleOrderCommissionDetailList = new List<saleOrderCommissionDetail>();
		saleOrderBuyerExpenseDetailList = new List<saleOrderbuyerExpenseDetail>();
	}
}
