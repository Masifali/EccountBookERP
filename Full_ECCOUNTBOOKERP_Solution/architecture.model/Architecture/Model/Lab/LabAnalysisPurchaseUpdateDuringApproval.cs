using System;
using System.Collections.Generic;

namespace Architecture.Model.Lab;

public class LabAnalysisPurchaseUpdateDuringApproval
{
	public bool IsAccepted { get; set; }

	public bool IsApproved { get; set; }

	public bool IsAcceptedApproved { get; set; }

	public DateTime? ApprovedDate { get; set; }

	public DateTime? ModifyDate { get; set; }

	public double DeductionRate { get; set; }

	public double DeductionWeight { get; set; }

	public double QtyForWtCut { get; set; }

	public double ItemRate { get; set; }

	public double Premium { get; set; }

	public double WeightCutUom { get; set; }

	public int WeightCutOnId { get; set; }

	public int ApprovedUserId { get; set; }

	public int DocumentTypeId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int GatePassInwardId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int PricingScheduleId { get; set; }

	public int RateUomId { get; set; }

	public int PackingTypeId { get; set; }

	public int WarehouseId { get; set; }

	public int FinancialYearId { get; set; }

	public int JobLotId { get; set; }

	public string RemarksHeader { get; set; }

	public bool IsWeightCutCompulsory { get; set; }

	public bool IsRateCutCompulsory { get; set; }

	public int ActionId { get; set; }

	public bool RateApplied { get; set; }

	public int RateAppliedFromId { get; set; }

	public int RateAppliedUserId { get; set; }

	public bool WeightApplied { get; set; }

	public int WeightAppliedFromId { get; set; }

	public int WeightAppliedUserId { get; set; }

	public virtual int RefDocumentTypeId { get; set; }

	public virtual bool IsFromPendingWork { get; set; }

	public virtual List<LabAnalysisPurchaseUpdateDuringApproval> LabAnalysisPurchaseUpdateDuringApprovalList { get; set; }
}
