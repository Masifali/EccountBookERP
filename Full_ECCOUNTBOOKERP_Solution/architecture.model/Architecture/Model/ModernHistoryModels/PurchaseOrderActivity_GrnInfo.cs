using System;

namespace Architecture.Model.ModernHistoryModels;

public class PurchaseOrderActivity_GrnInfo
{
	public int GrnId { get; set; }

	public int GatePassId { get; set; }

	public int GpSrNo { get; set; }

	public int DocNo { get; set; }

	public DateTime DocDate { get; set; }

	public double AvgEbUnit { get; set; }

	public double TotalEbTotal { get; set; }

	public double AvgWtCut { get; set; }

	public double TotalWtCutTotal { get; set; }

	public double TotalAdLsWeight { get; set; }

	public double AvgStockEbUnit { get; set; }

	public double TotalStockEbTotal { get; set; }

	public double PartyWeight { get; set; }

	public double FactoryWeight { get; set; }

	public double FreightAmount { get; set; }

	public string PurchaseType { get; set; }

	public string DeliveryTerm { get; set; }

	public double TotalQuantity { get; set; }

	public double TotalGrossWeight { get; set; }

	public double TotalNetWeight { get; set; }

	public double TotalStockWeight { get; set; }

	public string AreaCity { get; set; }

	public string EntryUserName { get; set; }

	public int EntryUser { get; set; }

	public DateTime EntryDate { get; set; }

	public string ModifyUserName { get; set; }

	public int ModifyUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public string EntryUserPrfilePicUrl { get; set; }

	public string ModifyUserPrfilePicUrl { get; set; }
}
