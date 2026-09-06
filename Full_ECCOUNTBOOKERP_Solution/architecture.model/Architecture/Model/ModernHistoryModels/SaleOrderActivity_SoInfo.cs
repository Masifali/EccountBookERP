using System;

namespace Architecture.Model.ModernHistoryModels;

public class SaleOrderActivity_SoInfo
{
	public int SaleOrderId { get; set; }

	public int OrderNo { get; set; }

	public DateTime OrderDate { get; set; }

	public DateTime OrderExpiryDate { get; set; }

	public string PaymentTerm { get; set; }

	public string DeliveryTerm { get; set; }

	public string ExpiryStatus { get; set; }

	public string OrderStatus { get; set; }

	public int OrderSupCustId { get; set; }

	public string CustomerName { get; set; }

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

	public int PurchaseOrderDetailId { get; set; }

	public int StockAcId { get; set; }

	public int OrderItemId { get; set; }

	public string ItemName { get; set; }

	public string PackUom { get; set; }

	public double OrderItemQty { get; set; }

	public double NetWeight { get; set; }

	public double OrderItemRate { get; set; }

	public string RateUom { get; set; }

	public double Amount { get; set; }

	public string Crop { get; set; }

	public string LabSampleNo { get; set; }

	public string DetailRemarks { get; set; }
}
