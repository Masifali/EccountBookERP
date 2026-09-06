using System;
using System.Collections.Generic;

namespace Architecture.Model.PartyProcessing;

public class InvFoodProductionPartyProcessing
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int BranchId { get; set; }

	public int FinancialYearId { get; set; }

	public string ScreenName { get; set; }

	public int ActionId { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int DocumentTypeId { get; set; }

	public int EntryUser { get; set; }

	public int Id { get; set; }

	public int InvJobOrderId { get; set; }

	public int ModifyUser { get; set; }

	public int OrganizationId { get; set; }

	public int ProjectId { get; set; }

	public int WIPItemId { get; set; }

	public string EntryType { get; set; }

	public string InvJobOrderNo { get; set; }

	public string MainRemarks { get; set; }

	public virtual List<InvFoodProductionPartyProcessingDetail> PartyProcessingDetailsList { get; set; }

	public virtual List<InvFoodProductionPartyProcessingPackingMaterial> PartyProcessingPackingMaterialsList { get; set; }
}
