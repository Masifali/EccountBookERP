using System;

namespace Architecture.Model.ModernHistoryModels;

public class SaleInvoiceReverseActivityDetail_GdnInfo
{
	public int Id { get; set; }

	public int DocNo { get; set; }

	public DateTime DocDate { get; set; }

	public int GatePassId { get; set; }

	public string GatePassType { get; set; }

	public int GatePassNumber { get; set; }

	public string DeliveryTerm { get; set; }

	public string VehicleNo { get; set; }

	public int SupplierCustomerId { get; set; }

	public string SupplierName { get; set; }

	public double PartyWeight { get; set; }

	public double FactoryWeight { get; set; }

	public double GrossWeight { get; set; }

	public double BagWeight { get; set; }

	public double WeightCut { get; set; }

	public double Ad_Less_Weight { get; set; }

	public double NetWeight { get; set; }

	public double StockWeight { get; set; }

	public string EntryUserName { get; set; }

	public int EntryUser { get; set; }

	public DateTime EntryDate { get; set; }

	public string ModifyUserName { get; set; }

	public int ModifyUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public string EntryUserPrfilePicUrl { get; set; }

	public string ModifyUserPrfilePicUrl { get; set; }
}
