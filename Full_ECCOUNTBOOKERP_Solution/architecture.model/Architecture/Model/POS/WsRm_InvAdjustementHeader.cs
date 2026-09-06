using System;
using System.Collections.Generic;

namespace Architecture.Model.POS;

public class WsRm_InvAdjustementHeader
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int WarehouseId { get; set; }

	public int DocumentTypeId { get; set; }

	public string RemarksHeader { get; set; }

	public virtual List<WsRmInvAdjustementDetail> WsRmInvAdjustementDetailList { get; set; }
}
