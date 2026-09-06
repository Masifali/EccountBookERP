using System;

namespace Architecture.Model.ModernHistoryModels;

public class PurchaseInvoiceReverseActivityDetail_GpInfo
{
	public int GatePassId { get; set; }

	public int GrnNo { get; set; }

	public int GpSrNo { get; set; }

	public DateTime GpDate { get; set; }

	public string VehicleNo { get; set; }

	public string BiltyNo { get; set; }

	public double SupplierWeight { get; set; }

	public double FactoryWeight { get; set; }

	public double DifferenceWeight { get; set; }

	public DateTime VehicleInDate { get; set; }

	public DateTime VehicleOutDate { get; set; }

	public string EntryUserName { get; set; }

	public int EntryUser { get; set; }

	public DateTime EntryDate { get; set; }

	public string ModifyUserName { get; set; }

	public int ModifyUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public string ApprovalUserName { get; set; }

	public int PostUser { get; set; }

	public DateTime PostDate { get; set; }

	public string EntryUserPrfilePicUrl { get; set; }

	public string ModifyUserPrfilePicUrl { get; set; }

	public string ApprovalUserPrfilePicUrl { get; set; }
}
