using System;

namespace Architecture.Model.ModernHistoryModels;

public class PurchaseOrderActivity_WbInfo
{
	public int WbId { get; set; }

	public int GatePassId { get; set; }

	public int GpSrNo { get; set; }

	public int TicketNo { get; set; }

	public DateTime TicketDate { get; set; }

	public double FirstWeight { get; set; }

	public double SecondWeight { get; set; }

	public double NetWbWeight { get; set; }

	public string WbRemarks { get; set; }

	public string EntryUserName { get; set; }

	public int EntryUser { get; set; }

	public DateTime EntryDate { get; set; }

	public string ModifyUserName { get; set; }

	public int ModifyUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public string EntryUserPrfilePicUrl { get; set; }

	public string ModifyUserPrfilePicUrl { get; set; }
}
