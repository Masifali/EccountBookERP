using System;
using System.Collections.Generic;

namespace Architecture.Model.POS;

public class WsRmRequisitionPo
{
	public bool HoIsApproved { get; set; }

	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime HoApprovalDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int DestinationLocationId { get; set; }

	public int DocNo { get; set; }

	public int EntryUserId { get; set; }

	public int HoApprovedUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int SourceLocationId { get; set; }

	public string HoComments { get; set; }

	public string RemarksHeader { get; set; }

	public string ReqStatus { get; set; }

	public virtual List<WsRmRequisitionPoDetail> WsRmRequisitionPoDetailsList { get; set; }
}
