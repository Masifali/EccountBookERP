using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class InvLabPreProductionExportLotInspectionHeader
{
	public bool IsApproved { get; set; }

	public DateTime? ApprovedDate { get; set; }

	public DateTime? ConfirmationDate { get; set; }

	public DateTime? DateOfInspection { get; set; }

	public DateTime? ResultDate { get; set; }

	public DateTime? EntryDate { get; set; }

	public DateTime? ModifyDate { get; set; }

	public DateTime? ReportDate { get; set; }

	public DateTime RequestDate { get; set; }

	public DateTime? SampleATADestinationDate { get; set; }

	public DateTime? SampleDispatchedDate { get; set; }

	public DateTime? SampleETADestinationDate { get; set; }

	public DateTime? SampleHandedOverDate { get; set; }

	public DateTime? SampleTakenDate { get; set; }

	public DateTime? StockReservedDate { get; set; }

	public DateTime? StockRSealedDate { get; set; }

	public double QtyKgs { get; set; }

	public int ActionId { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int CountryOfInspectionId { get; set; }

	public int PlaceOfInspectionId { get; set; }

	public int CountryOfOriginId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int InspectedByLabId { get; set; }

	public int InspectionAgencyId { get; set; }

	public int ItemId { get; set; }

	public int JobLotId { get; set; }

	public int LotCurrentStageId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public int ReportDocNo { get; set; }

	public int ReportResultRemarksId { get; set; }

	public int RequestedById { get; set; }

	public int RequestMediumId { get; set; }

	public int RequiredAnalysisId { get; set; }

	public int RevisionNo { get; set; }

	public int SampleHandedOverId { get; set; }

	public int SamplingResponsibilityId { get; set; }

	public int TransitDays { get; set; }

	public int ExImFarmingNTradeId { get; set; }

	public int ExImFarmingTypeId { get; set; }

	public int ExImTradeTypeId { get; set; }

	public string CourierTrackingNo { get; set; }

	public string ExporterLotRefNo { get; set; }

	public string InspectionRemarks { get; set; }

	public string InspectionReportNo { get; set; }

	public string LabRemarks { get; set; }

	public string LotRefNo { get; set; }

	public string PlaceOfInspection { get; set; }

	public string ProductSpecification { get; set; }

	public string ReportRefNo { get; set; }

	public string ReportStatus { get; set; }

	public string RequestedBy { get; set; }

	public string RequestMedium { get; set; }

	public string RequestRefNo { get; set; }

	public string RequiredAnalysis { get; set; }

	public string LotInstructionsOrRemarks { get; set; }

	public string ScreenName { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public virtual string LotCurrentStage { get; set; }

	public virtual List<InvLabPreProductionExportLotInspectionDetail> LotInspectionDetails { get; set; }

	public virtual List<InvLabPreProductionExportLotInspectionParametersDetail> InvLabPreProductionExportLotInspectionParametersDetailList { get; set; }

	public virtual List<ThirdPartyInspectionLotSamplingDetail> ThirdPartyInspectionLotSamplingDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
