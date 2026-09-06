using System;

namespace Architecture.Model.POS;

public class WsRmShiftDayCashInOut
{
	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double ReceivedAmount { get; set; }

	public double SkimmedAmount { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int WsRmShiftDayManagementId { get; set; }

	public string ReasonRemarks { get; set; }
}
