using System;
using System.Collections.Generic;

namespace Architecture.Model.PartyProcessing;

public class GatePassPartyProcessing
{
	public bool IsApproved { get; set; }

	public bool PostState { get; set; }

	public bool IsWeighable { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime GpDate { get; set; }

	public DateTime InDateTimeStamp { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime OutDateTimeStamp { get; set; }

	public DateTime PostDate { get; set; }

	public double DifferenceWeight { get; set; }

	public double FactoryWeight { get; set; }

	public double Freight { get; set; }

	public double ItemQty { get; set; }

	public double SupplierWeight { get; set; }

	public int CityId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int GpSrNo { get; set; }

	public int GpTypeSrNo { get; set; }

	public int Id { get; set; }

	public int ActionId { get; set; }

	public int ModifyUser { get; set; }

	public int FinancialYearId { get; set; }

	public int OrganizationId { get; set; }

	public int PostUser { get; set; }

	public int StockPartyId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int WeighBridgeId { get; set; }

	public int ItemId { get; set; }

	public int AdvanceDeliveryOrderId { get; set; }

	public string BiltyNo { get; set; }

	public string GatepassType { get; set; }

	public string OtherRemarks { get; set; }

	public string Status { get; set; }

	public string WeighBridgeStatus { get; set; }

	public string VarietyName { get; set; }

	public string VehicleNo { get; set; }

	public string VehicleType { get; set; }

	public string ScreenName { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
