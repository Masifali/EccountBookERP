using System.Collections.Generic;

namespace Architecture.Model.ModernHistoryModels;

public class PurchaseOrderActivityDetail
{
	public double OrderTotalWeight { get; set; }

	public double OrderReceivedWeight { get; set; }

	public double OrderBalanceWeight { get; set; }

	public double OrderQty { get; set; }

	public double OrderReceivedQty { get; set; }

	public double OrderBalanceQty { get; set; }

	public List<PuchaseOrderActivity_PoInfo> PurchaseOrderInfoList { get; set; }

	public List<PuchaseOrderActivity_GpInfo> GatePassInfoList { get; set; }

	public List<PurchaseOrderActivity_WagesInfo> WagesInfoList { get; set; }

	public List<PurchaseOrderActivity_LabInfo> LabInfoList { get; set; }

	public List<PurchaseOrderActivity_WbInfo> WeighBridgeInfoList { get; set; }

	public List<PurchaseOrderActivity_GrnInfo> GoodsReceiptNote { get; set; }

	public List<PurchaseOrderActivity_InvoiceInfo> PurchaseInvoiceInfo { get; set; }

	public List<PurchaseOrderActivity_OrderEmptyBagsInfo> OrderEmptyBagsInfoList { get; set; }
}
