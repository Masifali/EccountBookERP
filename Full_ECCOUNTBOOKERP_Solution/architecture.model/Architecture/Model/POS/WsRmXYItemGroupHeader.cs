using System;
using System.Collections.Generic;

namespace Architecture.Model.POS;

public class WsRmXYItemGroupHeader
{
	public bool IsActive { get; set; }

	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double XItemQty { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int GroupTypeId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public string GroupDescription { get; set; }

	public virtual List<WsRmXYItemGroupDetail> WsRmXYItemGroupDetailslist { get; set; }
}
