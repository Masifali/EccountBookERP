using System;

namespace Architecture.Model.ModernHistoryModels;

public class SaleOrderActivity_WagesInfo
{
	public int Id { get; set; }

	public int DocNo { get; set; }

	public DateTime DocDate { get; set; }

	public int GdnId { get; set; }

	public int GdnNumber { get; set; }

	public int ContractorId { get; set; }

	public string ContractorName { get; set; }

	public string WagesAccountName { get; set; }

	public double Quantity { get; set; }

	public double Weight { get; set; }

	public double PackSize { get; set; }

	public double Rate { get; set; }

	public double Amount { get; set; }

	public string EntryUserName { get; set; }

	public int EntryUser { get; set; }

	public DateTime EntryDate { get; set; }

	public string ModifyUserName { get; set; }

	public int ModifyUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public string EntryUserPrfilePicUrl { get; set; }

	public string ModifyUserPrfilePicUrl { get; set; }
}
