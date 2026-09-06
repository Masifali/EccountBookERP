using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class GatePassOutward
{
	public bool IsApproved { get; set; }

	public bool PostState { get; set; }

	public DateTime? BiltyDate { get; set; }

	public DateTime? EntryDate { get; set; }

	public DateTime GpDate { get; set; }

	public DateTime? InDateTimeStamp { get; set; }

	public DateTime? ModifyDate { get; set; }

	public DateTime? OutDateTimeStamp { get; set; }

	public DateTime? PostDate { get; set; }

	public double DifferenceWeight { get; set; }

	public double FactoryWeight { get; set; }

	public double Freight { get; set; }

	public double NetPaid { get; set; }

	public double SupplierWeight { get; set; }

	public double PackUnit { get; set; }

	public double WeightCommapredToSoWt { get; set; }

	public int CityId { get; set; }

	public int CompanyId { get; set; }

	public int FinancialYearId { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int GpSrNo { get; set; }

	public int GpTypeSrNo { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int NoOfPackages { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int RefDocumentEntryNo { get; set; }

	public int RefDocumentTypeId { get; set; }

	public int SaleOrderId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int WarehouseId { get; set; }

	public int WeighBridgeId { get; set; }

	public int BranchesId { get; set; }

	public int ItemId { get; set; }

	public string BiltyNo { get; set; }

	public string Container { get; set; }

	public string Container1 { get; set; }

	public string SealNo { get; set; }

	public string SealNo1 { get; set; }

	public string DocAttachment { get; set; }

	public string GatepassType { get; set; }

	public string OtherRemarks { get; set; }

	public string OtherSupCust { get; set; }

	public string Status { get; set; }

	public string SupplierContractCode { get; set; }

	public string VarietyName { get; set; }

	public string VehicleNo { get; set; }

	public string VehicleType { get; set; }

	public int ReferencePartyId { get; set; }

	public int TransporterId { get; set; }

	public string WeightDiffRemarks { get; set; }

	public virtual string ApprovedUserName { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
