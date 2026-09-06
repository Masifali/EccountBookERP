using System;
using Architecture.Model.Accounts;

namespace Architecture.Model.WeightBridge;

public class WbTransactions
{
	public bool IsApproved { get; set; }

	public DateTime? ApprovedDateTime { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime FirstDateTime { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime SecondDateTime { get; set; }

	public double FirstWeight { get; set; }

	public double ItemQty { get; set; }

	public double SecondWeight { get; set; }

	public double WbCharges { get; set; }

	public double NetWbWeight { get; set; }

	public double PackingUnitWeight { get; set; }

	public double PackingTotalWeight { get; set; }

	public double WeightAfterPackingWeight { get; set; }

	public double LoadWeight { get; set; }

	public double TearWeight { get; set; }

	public double SupplierWbNetWeight { get; set; }

	public bool MultiItem { get; set; }

	public int ApprovedUserId { get; set; }

	public int CompanyId { get; set; }

	public int DocTypeId { get; set; }

	public int EntryNo { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ActionId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ReferenceDocNoId { get; set; }

	public int ReferenceDocTypeId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int TicketNo { get; set; }

	public string BiltyNo { get; set; }

	public string ContainerNo { get; set; }

	public string FirstWtFrontPic { get; set; }

	public string FirstWtPicReading { get; set; }

	public string FirstWtUpperPic { get; set; }

	public string ItemDescription { get; set; }

	public string PartyName { get; set; }

	public string SecondPicReading { get; set; }

	public string SecondWtFrontPic { get; set; }

	public string SecondWtUpperPic { get; set; }

	public string VehicleNo { get; set; }

	public string VehicleType { get; set; }

	public string WbRemarks { get; set; }

	public int WorkingReportNo { get; set; }

	public int FinancialYearId { get; set; }

	public int ItemId { get; set; }

	public int BranchId { get; set; }

	public int ProjectId { get; set; }

	public int InvoiceId { get; set; }

	public string WeightDiffComments { get; set; }

	public string WeighBridgeType { get; set; }

	public string PackingType { get; set; }

	public int FirstWeighBridgeId { get; set; }

	public int SecondWeighBridgeId { get; set; }

	public virtual int RefDocumentTypeIdGpIn { get; set; }

	public virtual int RefDocumentTypeIdGpOut { get; set; }

	public virtual int RefDocumentTypeIdSt { get; set; }

	public virtual VoucherHead VoucherHeadWeighBridge { get; set; }
}
