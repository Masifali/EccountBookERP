using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class StockSummaryByItem
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime DocDate { get; set; }

	public double Amount { get; set; }

	public double ItemQty { get; set; }

	public double ItemRate { get; set; }

	public double NetWeight { get; set; }

	public double RateUOM { get; set; }

	public int ActionId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUOMId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PackingTypeId { get; set; }

	public string Remarks { get; set; }

	public virtual List<StockSummaryByItem> StockSummaryByItemsList { get; set; }
}
