using System;

namespace Architecture.Model.Inventory;

public class StockMonthlyClosing
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime ClosingDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double Amount { get; set; }

	public double ClosingRate { get; set; }

	public double OpeningRate { get; set; }

	public double OpeningAmount { get; set; }

	public double Qty { get; set; }

	public double Weight { get; set; }

	public int ActionId { get; set; }

	public int HeaderId { get; set; }

	public int ApprovedUserId { get; set; }

	public int CompanyId { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public int WarehouseId { get; set; }

	public int JobLotId { get; set; }

	public int InvPackingTypeId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int AccountId { get; set; }

	public int RateUomId { get; set; }

	public int LineId { get; set; }

	public string CropYear { get; set; }

	public string Remarks { get; set; }
}
