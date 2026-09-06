using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class GatePassInward
{
	public bool IsApproved { get; set; }

	public bool PostState { get; set; }

	public DateTime? EntryDate { get; set; }

	public DateTime GpDate { get; set; }

	public DateTime? InDateTimeStamp { get; set; }

	public DateTime? ModifyDate { get; set; }

	public DateTime? OutDateTimeStamp { get; set; }

	public DateTime? PostDate { get; set; }

	public DateTime? BiltyDate { get; set; }

	public double AdvanceByParty { get; set; }

	public double AdvanceByFactory { get; set; }

	public double DifferenceWeight { get; set; }

	public double FactoryWeight { get; set; }

	public double Freight { get; set; }

	public double NetPaid { get; set; }

	public double SupplierWeight { get; set; }

	public double PackUnit { get; set; }

	public double WeightComparedToPoWt { get; set; }

	public double AccessWeight { get; set; }

	public double SupplierFirstWeight { get; set; }

	public double SupplierSecondWeight { get; set; }

	public int CityId { get; set; }

	public int ItemId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int GpSrNo { get; set; }

	public int GpTypeSrNo { get; set; }

	public int PackingTypeId { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int NoOfPackages { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int PurchaseOrderId { get; set; }

	public int RefDocumentEntryNo { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int SupplierDispatchId { get; set; }

	public int WarehouseId { get; set; }

	public int WeighBridgeId { get; set; }

	public int FinancialYearId { get; set; }

	public int BranchesId { get; set; }

	public int FreightOn { get; set; }

	public int ActionIdForSpecialApproval { get; set; }

	public string BiltyNo { get; set; }

	public string DocAttachment { get; set; }

	public string GatepassType { get; set; }

	public string OtherRemarks { get; set; }

	public string OtherSupCust { get; set; }

	public string Status { get; set; }

	public string SupplierContractCode { get; set; }

	public string VarietyName { get; set; }

	public string VehicleNo { get; set; }

	public string VehicleType { get; set; }

	public string Container { get; set; }

	public string Container1 { get; set; }

	public byte[] GpQRCode { get; set; }

	public int? DestrictId { get; set; }

	public int? TehsilId { get; set; }

	public int? GhallaMandiId { get; set; }

	public int? LoadingContractorId { get; set; }

	public int? CarriageContractorId { get; set; }

	public int? WorkingReportNo { get; set; }

	public string SupplierCNICNO { get; set; }

	public string SupplierMobileNo { get; set; }

	public string TransporterName { get; set; }

	public string DriverName { get; set; }

	public string DriverCNICNO { get; set; }

	public string DriverMobileNo { get; set; }

	public string WeightDiffComments { get; set; }

	public int driverBioDataId { get; set; }

	public virtual string whatsappNo { get; set; }

	public virtual string AlternateCellNo { get; set; }

	public virtual string FatherName { get; set; }

	public virtual string fatherCnicNo { get; set; }

	public virtual int freightVoucherId { get; set; }

	public virtual string CityName { get; set; }

	public virtual string SupplierName { get; set; }

	public virtual bool RefferedInWbOrLab { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }

	public virtual List<GatePassInwardDetail> gatePassInwardDetails { get; set; }

	public virtual List<GatePassInwardPurchaseBreakUp> GatePassInwardPurchaseBreakUpList { get; set; }
}
