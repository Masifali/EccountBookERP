using System;
using System.Collections.Generic;

namespace Architecture.Model.PartyProcessing;

public class InvStockOpeningBalancePartyProcessing
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public double Qty { get; set; }

	public double WeightKgs { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int FinancialYearId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ActionId { get; set; }

	public int ItemId { get; set; }

	public int ItemUomSch { get; set; }

	public int JobLotId { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PackingTypeId { get; set; }

	public int ProjectsId { get; set; }

	public int WarehouseId { get; set; }

	public int StockPartyId { get; set; }

	public int SupplierCustomerId { get; set; }

	public int CropYearId { get; set; }

	public string CropYear { get; set; }

	public string Remarks { get; set; }

	public string ScreenName { get; set; }

	public virtual List<DMSAttachments> AttachmentsList { get; set; }

	public virtual List<DMSAttachments> DeleteAttachmentsList { get; set; }
}
