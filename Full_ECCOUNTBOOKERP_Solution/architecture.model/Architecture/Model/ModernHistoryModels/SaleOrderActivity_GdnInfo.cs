using System;

namespace Architecture.Model.ModernHistoryModels;

public class SaleOrderActivity_GdnInfo
{
	public int GdnId { get; set; }

	public int GatePassId { get; set; }

	public int GpSrNo { get; set; }

	public int DocNo { get; set; }

	public string CustomerName { get; set; }

	public DateTime DocDate { get; set; }

	public double FreightAmount { get; set; }

	public double ItemQty { get; set; }

	public double GrossWeight { get; set; }

	public double NetBillWeight { get; set; }

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
