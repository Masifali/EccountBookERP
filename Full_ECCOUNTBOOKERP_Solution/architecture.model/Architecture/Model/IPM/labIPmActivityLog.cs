using System;
using System.Collections.Generic;

namespace Architecture.Model.IPM;

public class labIPmActivityLog
{
	public int labIPmActivityLogId { get; set; }

	public int labIPmActivityLogMasterId { get; set; }

	public int documentTypeId { get; set; }

	public int refDocumentTypeId { get; set; }

	public int refDocumentHeaderId { get; set; }

	public int refDocumentDetialId { get; set; }

	public string FumigationLotNo { get; set; }

	public int? jobLotIdIpm { get; set; }

	public int qcActivityStepId { get; set; }

	public DateTime? qcActivityDate { get; set; }

	public int activityForDays { get; set; }

	public DateTime? completionDueDate { get; set; }

	public int qcActivityStatusAfterCompletionId { get; set; }

	public DateTime? nextActivityPlanDate { get; set; }

	public string fumigatedBy { get; set; }

	public string checkBy { get; set; }

	public string verifiedBy { get; set; }

	public int qcActivityStatus { get; set; }

	public int stockReleaseHoldStatusId { get; set; }

	public string qcActivityDescription { get; set; }

	public string remarksHeader { get; set; }

	public string attachmentsValues { get; set; }

	public string customAttachmentsValues { get; set; }

	public int organizationId { get; set; }

	public int companyId { get; set; }

	public int financialYearId { get; set; }

	public int branchId { get; set; }

	public int? projectId { get; set; }

	public int entryUserId { get; set; }

	public DateTime entryDate { get; set; }

	public int? modifyUserId { get; set; }

	public DateTime? modifyDate { get; set; }

	public int? deletedUserId { get; set; }

	public DateTime? deletedOn { get; set; }

	public bool isApproved { get; set; }

	public int? approvedUserId { get; set; }

	public DateTime? approvedDate { get; set; }

	public string approvalRemarks { get; set; }

	public int actionId { get; set; }

	public virtual string RefDocumentTypeName { get; set; }

	public virtual string QcActivityStepName { get; set; }

	public virtual DateTime? RefDocDate { get; set; }

	public virtual int RefDocNo { get; set; }

	public virtual string VehicleNo { get; set; }

	public virtual int ItemId { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual double QtyBalance { get; set; }

	public virtual double WeightBalance { get; set; }

	public virtual int GrnNo { get; set; }

	public virtual int GpNo { get; set; }

	public virtual DateTime? GPDate { get; set; }

	public virtual int SupplierCustomerId { get; set; }

	public virtual string SupplierCustomerName { get; set; }

	public virtual int WarehouseId { get; set; }

	public virtual string WareHouseCode { get; set; }

	public virtual int JobLotId { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual int ManualNo { get; set; }

	public virtual string QcActivityStatusAfterCompletionName { get; set; }

	public virtual string StockReleaseHoldStatusName { get; set; }

	public virtual List<labIPmActivityLog> LabIPmActivityLogList { get; set; }

	public labIPmActivityLog()
	{
		LabIPmActivityLogList = new List<labIPmActivityLog>();
	}
}
