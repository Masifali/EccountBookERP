using System;
using System.Collections.Generic;

namespace Architecture.Model.Steel.Production;

public class InvFoodProductionOverHeads
{
	public int Id { get; set; }

	public int DocumentTypeId { get; set; }

	public DateTime DocDate { get; set; }

	public int DocNo { get; set; }

	public int ProductionJobOrderId { get; set; }

	public int JobOrderOverHeadId { get; set; }

	public int PlantId { get; set; }

	public int CharOfAccountId { get; set; }

	public int ChargesTypeId { get; set; }

	public int BrandId { get; set; }

	public int BrandUomId { get; set; }

	public int RateUomId { get; set; }

	public decimal ItemQty { get; set; }

	public decimal ItemRate { get; set; }

	public decimal Amount { get; set; }

	public string RemarksOH { get; set; }

	public int ApprovedUserId { get; set; }

	public DateTime ApprovedDate { get; set; }

	public bool IsApproved { get; set; }

	public DateTime EntryDate { get; set; }

	public int EntryUserId { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }

	public int ProjectsId { get; set; }

	public int FinancialYearId { get; set; }

	public int ActionId { get; set; }

	public virtual List<InvFoodProductionOverHeads> InvFoodProductionOverHeadsList { get; set; }
}
