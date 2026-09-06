using System;

namespace Architecture.Model.ModernHistoryModels;

public class SaleOrderActivity_InvoiceInfo
{
	public int SaleInvoiceId { get; set; }

	public int GdnId { get; set; }

	public int GpSrNo { get; set; }

	public int GdnNo { get; set; }

	public int DocNo { get; set; }

	public DateTime DocDate { get; set; }

	public double InvoiceAmount { get; set; }

	public double ItemAmount { get; set; }

	public double ChargeToProduct { get; set; }

	public double OtherExpenses { get; set; }

	public double CommissionAmount { get; set; }

	public double BillAmount { get; set; }

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
