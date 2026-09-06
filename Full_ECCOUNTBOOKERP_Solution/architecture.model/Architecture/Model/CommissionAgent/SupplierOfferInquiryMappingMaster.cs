using System;
using System.Collections.Generic;

namespace Architecture.Model.CommissionAgent;

public class SupplierOfferInquiryMappingMaster
{
	public DateTime entryDate { get; set; }

	public DateTime mappingDate { get; set; }

	public DateTime modifyDate { get; set; }

	public int actionId { get; set; }

	public int branchId { get; set; }

	public int companyId { get; set; }

	public int entryUserId { get; set; }

	public int financialYearId { get; set; }

	public int mappingMasterId { get; set; }

	public int mappingNo { get; set; }

	public int mappingStatusId { get; set; }

	public int modifyUserId { get; set; }

	public int organizationId { get; set; }

	public int purchaseOrderId { get; set; }

	public int revisionNo { get; set; }

	public int salesOrderId { get; set; }

	public long rowVersion { get; set; }

	public string remarks { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<SupplierOfferInquiryMappingDetail> SupplierOfferInquiryMappingDetailList { get; set; }

	public SupplierOfferInquiryMappingMaster()
	{
		AttachmentsList = new List<DMSAttachments>();
		DeleteAttachmentsList = new List<DMSAttachments>();
		SupplierOfferInquiryMappingDetailList = new List<SupplierOfferInquiryMappingDetail>();
	}
}
