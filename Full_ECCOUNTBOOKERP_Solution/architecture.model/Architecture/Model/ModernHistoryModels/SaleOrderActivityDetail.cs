using System.Collections.Generic;

namespace Architecture.Model.ModernHistoryModels;

public class SaleOrderActivityDetail
{
	public double OrderTotalWeight { get; set; }

	public double OrderReceivedWeight { get; set; }

	public double OrderBalanceWeight { get; set; }

	public List<SaleOrderActivity_SoInfo> SaleOrderInfoList { get; set; }

	public List<SaleOrderActivity_DoInfo> DeliveryOrderInfoList { get; set; }

	public List<SaleOrderActivity_GpInfo> GatePassInfoList { get; set; }

	public List<SaleOrderActivity_WagesInfo> WagesInfoList { get; set; }

	public List<SaleOrderActivity_WbInfo> WeighBridgeInfoList { get; set; }

	public List<SaleOrderActivity_GdnInfo> GoodsDispatchtNotesInfoList { get; set; }

	public List<SaleOrderActivity_InvoiceInfo> InvoiceInfoList { get; set; }
}
