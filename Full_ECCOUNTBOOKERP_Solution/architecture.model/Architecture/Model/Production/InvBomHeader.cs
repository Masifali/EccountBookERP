using System;
using System.Collections.Generic;

namespace Architecture.Model.Production;

public class InvBomHeader
{
	public bool IsActive { get; set; }

	public DateTime EffectedDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double BatchQty { get; set; }

	public int BatchQtyUomId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int ConsumptionItemId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectsId { get; set; }

	public string BomCode { get; set; }

	public string BomTypeDesc { get; set; }

	public virtual List<InvBomDetail> invBomDetails { get; set; }
}
