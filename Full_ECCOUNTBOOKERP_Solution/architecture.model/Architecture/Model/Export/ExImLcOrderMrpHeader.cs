using System;
using System.Collections.Generic;

namespace Architecture.Model.Export;

public class ExImLcOrderMrpHeader
{
	public decimal ItemQty { get; set; }

	public int CompanyId { get; set; }

	public int ExImLcOrderId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int OrganizationId { get; set; }

	public int EntryUser { get; set; }

	public int ModifyUser { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public virtual List<ExImLcOrderMrpDetail> ExImLcOrderMrpDetailsList { get; set; }
}
