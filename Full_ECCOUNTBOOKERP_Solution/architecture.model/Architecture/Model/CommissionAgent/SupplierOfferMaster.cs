using System;
using System.Collections.Generic;

namespace Architecture.Model.CommissionAgent;

public class SupplierOfferMaster
{
	public bool isApproved { get; set; }

	public DateTime? approvedDate { get; set; }

	public DateTime deliveryStartDate { get; set; }

	public DateTime entryDate { get; set; }

	public DateTime expiryDate { get; set; }

	public DateTime SupplierOfferDate { get; set; }

	public DateTime modifyDate { get; set; }

	public DateTime validityDate { get; set; }

	public int actionId { get; set; }

	public int analysisGroupId { get; set; }

	public int approvedUserId { get; set; }

	public int branchId { get; set; }

	public int SupplierId { get; set; }

	public int commissionAgentId { get; set; }

	public int inquiryStatusId { get; set; }

	public int companyId { get; set; }

	public int deliveryDays { get; set; }

	public int deliveryTermId { get; set; }

	public int documentTypeId { get; set; }

	public int entryUserId { get; set; }

	public int financialYearId { get; set; }

	public int SupplierOfferMasterId { get; set; }

	public int SupplierOfferNo { get; set; }

	public int modifyUserId { get; set; }

	public int organizationId { get; set; }

	public int projectId { get; set; }

	public int validityDays { get; set; }

	public string remarksHeader { get; set; }

	public string approvalRemarks { get; set; }

	public string attachmentsValues { get; set; }

	public string customAttachmentsValues { get; set; }

	public string QualitySpecifications { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<SupplierOfferDetail> SupplierOfferDetailList { get; set; }

	public virtual List<SupplierOfferPaymentSchedule> SupplierOfferPaymentScheduleList { get; set; }

	public virtual List<SupplierOfferQualitySpecification> SupplierOfferQualitySpecificationList { get; set; }

	public SupplierOfferMaster()
	{
		AttachmentsList = new List<DMSAttachments>();
		DeleteAttachmentsList = new List<DMSAttachments>();
		SupplierOfferDetailList = new List<SupplierOfferDetail>();
		SupplierOfferPaymentScheduleList = new List<SupplierOfferPaymentSchedule>();
		SupplierOfferQualitySpecificationList = new List<SupplierOfferQualitySpecification>();
	}
}
