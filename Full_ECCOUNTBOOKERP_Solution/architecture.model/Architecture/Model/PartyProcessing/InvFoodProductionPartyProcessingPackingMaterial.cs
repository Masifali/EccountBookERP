using System;

namespace Architecture.Model.PartyProcessing;

public class InvFoodProductionPartyProcessingPackingMaterial
{
	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime ApprovedDate { get; set; }

	public double Qty { get; set; }

	public bool IsApproved { get; set; }

	public int CompanyId { get; set; }

	public int FinancialYearId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int ApprovedUserId { get; set; }

	public int Id { get; set; }

	public int InvJobOrderId { get; set; }

	public int ItemId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int WarehouseId { get; set; }

	public string Remarks { get; set; }

	public string ScreenName { get; set; }

	public int ActionId { get; set; }
}
