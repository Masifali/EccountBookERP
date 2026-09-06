using System;

namespace Architecture.Model.Inventory;

public class InvDipReading
{
	public int Id { get; set; }

	public int TankId { get; set; }

	public int DocNo { get; set; }

	public DateTime DocDate { get; set; }

	public decimal BooksStock { get; set; }

	public decimal DipReadingStock { get; set; }

	public string StockTakenBy { get; set; }

	public int ItemId { get; set; }

	public int ItemUomId { get; set; }

	public string Remarks { get; set; }

	public DateTime EntryDate { get; set; }

	public int EntryUser { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ModifyUser { get; set; }

	public DateTime? ApprovedDate { get; set; }

	public int ApprovedUser { get; set; }

	public bool IsApproved { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }

	public int ProjectsId { get; set; }
}
