using System;

namespace Architecture.Model.ModernHistoryModels;

public class PurchaseInvoiceReverseActivityDetail_LabInfo
{
	public int Id { get; set; }

	public DateTime DocDate { get; set; }

	public int DocNo { get; set; }

	public int GatePassId { get; set; }

	public int GatePassNumber { get; set; }

	public int StockAcId { get; set; }

	public int ItemId { get; set; }

	public string ItemName { get; set; }

	public double DeductionWeight { get; set; }

	public double DeductionRate { get; set; }

	public double ItemRate { get; set; }

	public string AnalystName { get; set; }

	public string RemarksHeader { get; set; }

	public string Crop { get; set; }

	public int PoDetailId { get; set; }

	public string LabStatus { get; set; }

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
