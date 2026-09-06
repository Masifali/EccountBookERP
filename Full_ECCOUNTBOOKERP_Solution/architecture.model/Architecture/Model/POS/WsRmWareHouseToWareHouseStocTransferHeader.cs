using System;
using System.Collections.Generic;

namespace Architecture.Model.POS;

public class WsRmWareHouseToWareHouseStocTransferHeader
{
	public int Id { get; set; }

	public int DocNo { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime ApprovedDate { get; set; }

	public int DocumentTypeId { get; set; }

	public string Remarks { get; set; }

	public int EntryUser { get; set; }

	public int ModifyUser { get; set; }

	public bool IsApproved { get; set; }

	public int ActionTypeId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int FinancialYearId { get; set; }

	public int ApprovedUser { get; set; }

	public virtual List<WsRmWareHouseToWareHouseStocTransferDetail> WsRmWareHouseToWareHouseStocTransferDetailList { get; set; }
}
