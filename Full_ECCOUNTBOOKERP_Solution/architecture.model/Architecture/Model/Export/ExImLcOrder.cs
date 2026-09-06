using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class ExImLcOrder
{
	public bool IsApproved { get; set; }

	public bool PostState { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime? ExpiryDate { get; set; }

	public DateTime? LastShipmentDate { get; set; }

	public DateTime? LcOrderDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime? PostDate { get; set; }

	public DateTime? SalesContratDate { get; set; }

	public DateTime? ShipmentStartDate { get; set; }

	public DateTime? CustomerApprovalDate { get; set; }

	public DateTime? FactoryLoadingDate { get; set; }

	public double ConversionRate { get; set; }

	public double EquivalentAmount { get; set; }

	public double FCurrencyAmount { get; set; }

	public double GrossWeightKgs { get; set; }

	public double NetWeightKgs { get; set; }

	public double NoOfContainers { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DeliveryTermId { get; set; }

	public int DestinationPortId { get; set; }

	public int EntryUser { get; set; }

	public int ExImProformaInvoice { get; set; }

	public int ExporterBankId { get; set; }

	public int ExportCompanyId { get; set; }

	public int FcurrencyId { get; set; }

	public int CareierTypeId { get; set; }

	public int Id { get; set; }

	public int ImporterBankId { get; set; }

	public int LcOrderDocNo { get; set; }

	public int LcOrderStatusId { get; set; }

	public int LoadingPortId { get; set; }

	public int ModifyUser { get; set; }

	public int NoOfShipments { get; set; }

	public int NotifyPartyId { get; set; }

	public int NotifyParty2Id { get; set; }

	public int NotifyParty3Id { get; set; }

	public int OrganizationId { get; set; }

	public int PaymentTermId { get; set; }

	public int PostUser { get; set; }

	public int ProductionSchDaysBefore { get; set; }

	public int ProjectsId { get; set; }

	public int SalesPersonId { get; set; }

	public int SampleSchDaysBefore { get; set; }

	public int ShipmentIntervalDays { get; set; }

	public int SupCustId { get; set; }

	public int ConsigneeId { get; set; }

	public int ContractType { get; set; }

	public int CommissionAgentId { get; set; }

	public int DeliveryDays { get; set; }

	public int ActionId { get; set; }

	public double CommRate { get; set; }

	public double CommAmount { get; set; }

	public string CommodityDetial { get; set; }

	public string PaymentRemarks { get; set; }

	public string DeliveryRemarks { get; set; }

	public string CustomerCode { get; set; }

	public string ExpiryPlace { get; set; }

	public string FcurrencySymbol { get; set; }

	public string InquiryReference { get; set; }

	public string InsepctionDescription { get; set; }

	public string InsepctionRequired { get; set; }

	public string LcDocAttachment { get; set; }

	public string LcOrderNo { get; set; }

	public string CustomerOrderNo { get; set; }

	public string LegalizationDescription { get; set; }

	public string LegalizationRequired { get; set; }

	public string LotReference { get; set; }

	public string PartialShipment { get; set; }

	public string QuotReference { get; set; }

	public string RemarksHeader { get; set; }

	public string SalesContractRef { get; set; }

	public string ShippingMarks { get; set; }

	public string Status { get; set; }

	public string TransShipment { get; set; }

	public string InsuranceRemarks { get; set; }

	public string CustomerApprovalRemarks { get; set; }

	public string ContactPerson { get; set; }

	public int DocumentTypeId { get; set; }

	public int ShipedToId { get; set; }

	public int FinancialYearId { get; set; }

	public int CustomGroupId { get; set; }

	public string ProductSpecification { get; set; }

	public string AddLessComments { get; set; }

	public string AttachmentsValues { get; set; }

	public string CustomAttachmentsValues { get; set; }

	public double AddLessAmount { get; set; }

	public double TotalAmountWithAdls { get; set; }

	public virtual List<ExImLcOrderPackingDetail> LcOrderDetail { get; set; }

	public virtual List<ExImLcOrderOtherItems> ExImLcOrderOtherItemslist { get; set; }

	public virtual List<ExImLcOrderShipmentNInspectionNProductionSchedule> LcOrderShipmentNInspectioNProductionScheduleDetail { get; set; }

	public virtual List<ExImLcOrderDocumentList> LcOrderDocumentDetail { get; set; }

	public virtual List<ExImLcOrderAnalysisTestList> LcOrderAnalysisTestDetail { get; set; }

	public virtual List<ExImLcOrderPaymentTermsDetail> ExImLcOrderPaymentTermsDetailslist { get; set; }

	public virtual List<ExImLcContractPackingMaterialDetail> ExImLcContractPackingMaterialDetailslist { get; set; }

	public virtual List<ExImLcOrderOtherChargesDetail> ExImLcOrderOtherChargesDetailList { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public ExImLcOrder()
	{
		LcOrderDetail = new List<ExImLcOrderPackingDetail>();
		ExImLcOrderOtherItemslist = new List<ExImLcOrderOtherItems>();
		LcOrderShipmentNInspectioNProductionScheduleDetail = new List<ExImLcOrderShipmentNInspectionNProductionSchedule>();
		LcOrderDocumentDetail = new List<ExImLcOrderDocumentList>();
		LcOrderAnalysisTestDetail = new List<ExImLcOrderAnalysisTestList>();
		ExImLcOrderPaymentTermsDetailslist = new List<ExImLcOrderPaymentTermsDetail>();
		ExImLcContractPackingMaterialDetailslist = new List<ExImLcContractPackingMaterialDetail>();
		ExImLcOrderOtherChargesDetailList = new List<ExImLcOrderOtherChargesDetail>();
		AttachmentsList = new List<DMSAttachments>();
		DeleteAttachmentsList = new List<DMSAttachments>();
	}
}
