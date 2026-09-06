using System;

namespace Architecture.Model.ModernHistoryModels;

public class PurchaseOrderActivity_InvoiceInfo
{
	public int PurchaseInvoiceId { get; set; }

	public int GpSrNo { get; set; }

	public int GrnId { get; set; }

	public string GrnNo { get; set; }

	public int DocNo { get; set; }

	public int DueDays { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime DueDate { get; set; }

	public double BillWeight { get; set; }

	public double InvoiceAmount { get; set; }

	public double ItemAmount { get; set; }

	public double FreightAmount { get; set; }

	public double OtherExpenses { get; set; }

	public double CommissionAmount { get; set; }

	public double ChargeToProduct { get; set; }

	public double PartyAddLess { get; set; }

	public double BillAmount { get; set; }

	public double WagesAmount { get; set; }

	public double AvgRate { get; set; }

	public double AvgRateCut { get; set; }

	public double NetAvgRate { get; set; }

	public string EntryUserName { get; set; }

	public int EntryUser { get; set; }

	public DateTime EntryDate { get; set; }

	public string ModifyUserName { get; set; }

	public int ModifyUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public string ApprovalUserName { get; set; }

	public int PostUser { get; set; }

	public DateTime PostDate { get; set; }

	public string ApprovedStatus { get; set; }

	public string EntryUserPrfilePicUrl { get; set; }

	public string ModifyUserPrfilePicUrl { get; set; }

	public string ApprovalUserPrfilePicUrl { get; set; }
}
