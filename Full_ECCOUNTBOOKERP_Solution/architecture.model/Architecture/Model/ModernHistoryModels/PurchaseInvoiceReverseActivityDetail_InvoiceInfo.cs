using System;

namespace Architecture.Model.ModernHistoryModels;

public class PurchaseInvoiceReverseActivityDetail_InvoiceInfo
{
	public int Id { get; set; }

	public int DocNo { get; set; }

	public DateTime DocDate { get; set; }

	public double BillAmount { get; set; }

	public double InvoiceAmount { get; set; }

	public string DeliveryTerm { get; set; }

	public string PaymentTerm { get; set; }

	public DateTime DueDate { get; set; }

	public int DueDays { get; set; }

	public string ManualBillNo { get; set; }

	public string EntryUserName { get; set; }

	public int EntryUser { get; set; }

	public DateTime EntryDate { get; set; }

	public string ModifyUserName { get; set; }

	public int ModifyUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public string ApprovalUserName { get; set; }

	public bool IsApproved { get; set; }

	public int PostUser { get; set; }

	public DateTime PostDate { get; set; }

	public string EntryUserPrfilePicUrl { get; set; }

	public string ModifyUserPrfilePicUrl { get; set; }

	public string ApprovalUserPrfilePicUrl { get; set; }

	public int GpNo { get; set; }

	public string VehicleNo { get; set; }

	public string ItemName { get; set; }

	public string CropYear { get; set; }

	public double ItemQty { get; set; }

	public double GrossWeight { get; set; }

	public double EBTotalWt { get; set; }

	public double WeightCutTotal { get; set; }

	public double NetBillWeight { get; set; }

	public double ItemRate { get; set; }

	public double ItemAmount { get; set; }

	public string RateUom { get; set; }

	public double ExpenseAmount { get; set; }

	public double CommissionAmount { get; set; }

	public double ItemNetAmount { get; set; }

	public int SupplierCustomerId { get; set; }

	public int CommissionAgentId { get; set; }

	public int BrokerAgentId { get; set; }

	public string SupplierName { get; set; }

	public string ReferencePartyName { get; set; }

	public string CommissionAgent { get; set; }

	public string BrokerAgent { get; set; }

	public double TotalCommAmount { get; set; }

	public double BrokeryAmount { get; set; }

	public double TotalItemAmount { get; set; }

	public double OtherAddLess { get; set; }

	public double TotalItemNetAmount { get; set; }

	public int OrderNo { get; set; }

	public string PackSize { get; set; }

	public double RateCut { get; set; }

	public double NetRate { get; set; }

	public string PackingType { get; set; }
}
