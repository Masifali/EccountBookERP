using System;
using System.Collections.Generic;

namespace Architecture.Model.Lab;

public class InvLabAnalysisPurchaseHeader
{
	public bool IsAccepted { get; set; }

	public bool IsApproved { get; set; }

	public bool IsAcceptedApproved { get; set; }

	public DateTime? ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime? EntryDate { get; set; }

	public DateTime? ModifyDate { get; set; }

	public double DeductionRate { get; set; }

	public double DeductionWeight { get; set; }

	public double QtyForWtCut { get; set; }

	public double ItemRate { get; set; }

	public double NoofBagsInspection { get; set; }

	public double Premium { get; set; }

	public double WeightCutUom { get; set; }

	public int WeightCutOnId { get; set; }

	public int ApprovedUserId { get; set; }

	public int DocumentTypeId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int EntryUser { get; set; }

	public int GatePassInwardId { get; set; }

	public int Id { get; set; }

	public int InvLabAnalysisGroup { get; set; }

	public int InvLabSampleLogRegisterId { get; set; }

	public int ItemId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int PoDetailId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int PricingScheduleId { get; set; }

	public int RateUomId { get; set; }

	public int PackingTypeId { get; set; }

	public int WarehouseId { get; set; }

	public int FinancialYearId { get; set; }

	public int JobLotId { get; set; }

	public string AnalystName { get; set; }

	public string CookingPic { get; set; }

	public string Crop { get; set; }

	public string Pic1Pathe { get; set; }

	public string RemarksHeader { get; set; }

	public string SupCustCode { get; set; }

	public string PartyLotRefNo { get; set; }

	public bool IsWeightCutCompulsory { get; set; }

	public bool IsRateCutCompulsory { get; set; }

	public bool RateApplied { get; set; }

	public int RateAppliedFromId { get; set; }

	public int RateAppliedUserId { get; set; }

	public bool WeightApplied { get; set; }

	public int WeightAppliedFromId { get; set; }

	public int WeightAppliedUserId { get; set; }

	public double PremiumLessPercentOnRate { get; set; }

	public double RateCutPercentOnOrderRate { get; set; }

	public int PestResultId { get; set; }

	public int PestStatusId { get; set; }

	public virtual List<InvLabAnalysisPurchaseDetail> LabSampleDetail { get; set; }

	public virtual List<DMSAttachments> dmsAttachment { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual string BiltyNo { get; set; }

	public virtual DateTime BiltyDate { get; set; }

	public virtual string VehicleNo { get; set; }

	public virtual int GpSrNo { get; set; }

	public virtual int RefDocumentTypeId { get; set; }

	public virtual int LabSampleNo { get; set; }

	public virtual int ParentcategoryId { get; set; }

	public virtual string Parentcategory { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string CityName { get; set; }

	public virtual string PurchaseType { get; set; }

	public virtual string RateUom { get; set; }

	public virtual string PartyName { get; set; }

	public virtual string UserNameAusr { get; set; }

	public virtual string ApprovalStatus { get; set; }

	public virtual double OrderRate { get; set; }

	public virtual DateTime InDateTimeStamp { get; set; }
}
