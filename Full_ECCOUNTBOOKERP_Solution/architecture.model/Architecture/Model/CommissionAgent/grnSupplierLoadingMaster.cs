using System;
using System.Collections.Generic;

namespace Architecture.Model.CommissionAgent;

public class grnSupplierLoadingMaster
{
	public bool isApproved { get; set; }

	public bool isSupplierOtherChargesAllowed { get; set; }

	public DateTime? approvedDate { get; set; }

	public DateTime? biltyDate { get; set; }

	public DateTime? deliveryStartDate { get; set; }

	public DateTime docDate { get; set; }

	public DateTime? entryDate { get; set; }

	public DateTime? modifyDate { get; set; }

	public decimal biltyFreight { get; set; }

	public decimal biltyQty { get; set; }

	public decimal loadWeight { get; set; }

	public decimal otherAdLesCharges { get; set; }

	public decimal scaleNetWeight { get; set; }

	public decimal tareWeight { get; set; }

	public decimal totalFreight { get; set; }

	public int actionId { get; set; }

	public int approvedUserId { get; set; }

	public int branchId { get; set; }

	public int commissionAgentId { get; set; }

	public int companyId { get; set; }

	public int deliveryDays { get; set; }

	public int deliveryTermId { get; set; }

	public int docNo { get; set; }

	public int documentTypeId { get; set; }

	public int entryUserId { get; set; }

	public int financialYearId { get; set; }

	public int grnSupplierLoadingMasterId { get; set; }

	public int loadingCityId { get; set; }

	public int modifyUserId { get; set; }

	public int organizationId { get; set; }

	public int projectId { get; set; }

	public int supplierId { get; set; }

	public int transporterId { get; set; }

	public int unloadingCityId { get; set; }

	public int vehicleTypeId { get; set; }

	public string approvalRemarks { get; set; }

	public string attachmentsValues { get; set; }

	public string biltyNo { get; set; }

	public string customAttachmentsValues { get; set; }

	public string remarksHeader { get; set; }

	public string supplierRefDocNo { get; set; }

	public string transporterName { get; set; }

	public string vehicleNo { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<grnSupplierLoadingDetail> grnSupplierLoadingDetailList { get; set; }

	public virtual List<grnSupplierLoadingEmptyBagDetail> grnSupplierLoadingEmptyBagDetailList { get; set; }

	public virtual List<grnSupplierLoadingExpenseDetail> grnSupplierLoadingExpenseDetailList { get; set; }

	public grnSupplierLoadingMaster()
	{
		AttachmentsList = new List<DMSAttachments>();
		DeleteAttachmentsList = new List<DMSAttachments>();
		grnSupplierLoadingDetailList = new List<grnSupplierLoadingDetail>();
		grnSupplierLoadingEmptyBagDetailList = new List<grnSupplierLoadingEmptyBagDetail>();
		grnSupplierLoadingExpenseDetailList = new List<grnSupplierLoadingExpenseDetail>();
	}
}
