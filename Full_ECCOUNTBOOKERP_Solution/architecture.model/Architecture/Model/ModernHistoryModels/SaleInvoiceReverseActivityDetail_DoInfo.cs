using System;

namespace Architecture.Model.ModernHistoryModels;

public class SaleInvoiceReverseActivityDetail_DoInfo
{
	public int DeliveryOrderId { get; set; }

	public int DocNo { get; set; }

	public DateTime DocDate { get; set; }

	public bool IsApproved { get; set; }

	public string DeliveryOrderType { get; set; }

	public int OrderSupCustId { get; set; }

	public int SaleOrderId { get; set; }

	public int OrderNo { get; set; }

	public string CustomerName { get; set; }

	public int GpSrNo { get; set; }

	public double PackingWeight { get; set; }

	public double GrossWeight { get; set; }

	public double LoadingQty { get; set; }

	public double LoadingWeight { get; set; }

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
