using System;
using System.Collections.Generic;

namespace Architecture.Model.CommissionAgent;

public class InquiryBookingMaster
{
	public bool isApproved { get; set; }

	public DateTime? approvedDate { get; set; }

	public DateTime deliveryStartDate { get; set; }

	public DateTime entryDate { get; set; }

	public DateTime expiryDate { get; set; }

	public DateTime inquiryBookingDate { get; set; }

	public DateTime modifyDate { get; set; }

	public DateTime validityDate { get; set; }

	public int actionId { get; set; }

	public int analysisGroupId { get; set; }

	public int approvedUserId { get; set; }

	public int branchId { get; set; }

	public int buyerId { get; set; }

	public int commissionAgentId { get; set; }

	public int inquiryStatusId { get; set; }

	public int companyId { get; set; }

	public int deliveryDays { get; set; }

	public int deliveryTermId { get; set; }

	public int documentTypeId { get; set; }

	public int entryUserId { get; set; }

	public int financialYearId { get; set; }

	public int inquiryBookingMasterId { get; set; }

	public int inquiryBookingNo { get; set; }

	public int modifyUserId { get; set; }

	public int organizationId { get; set; }

	public int projectId { get; set; }

	public int validityDays { get; set; }

	public string ShipToAddress { get; set; }

	public int shipToAddressId { get; set; }

	public int DeliveryToPartyId { get; set; }

	public string remarksHeader { get; set; }

	public string approvalRemarks { get; set; }

	public string attachmentsValues { get; set; }

	public string customAttachmentsValues { get; set; }

	public string QualitySpecifications { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<InquiryBookingDetail> InquiryBookingDetailList { get; set; }

	public virtual List<InquiryBookingPaymentSchedule> InquiryBookingPaymentScheduleList { get; set; }

	public virtual List<InquiryBookingQualitySpecification> InquiryBookingQualitySpecificationList { get; set; }

	public virtual List<InquiryBookingPartyDetail> InquiryBookingPartyDetailList { get; set; }

	public InquiryBookingMaster()
	{
		AttachmentsList = new List<DMSAttachments>();
		DeleteAttachmentsList = new List<DMSAttachments>();
		InquiryBookingDetailList = new List<InquiryBookingDetail>();
		InquiryBookingPaymentScheduleList = new List<InquiryBookingPaymentSchedule>();
		InquiryBookingQualitySpecificationList = new List<InquiryBookingQualitySpecification>();
	}
}
