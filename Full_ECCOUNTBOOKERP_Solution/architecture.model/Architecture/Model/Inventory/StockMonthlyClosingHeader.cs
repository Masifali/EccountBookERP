using System;
using System.Collections.Generic;

namespace Architecture.Model.Inventory;

public class StockMonthlyClosingHeader
{
	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int AccountId { get; set; }

	public int ActionId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int DocNo { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public string Remarks { get; set; }

	public virtual List<StockMonthlyClosing> StockMonthlyClosingList { get; set; }

	public virtual List<InventoryStockEvalautionDetail> InventoryStockEvalautionDetailslist { get; set; }
}
