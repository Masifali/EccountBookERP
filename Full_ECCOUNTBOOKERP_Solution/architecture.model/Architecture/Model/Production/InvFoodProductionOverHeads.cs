using System;
using System.Collections.Generic;

namespace Architecture.Model.Production;

public class InvFoodProductionOverHeads
{
	public DateTime DocDate { get; set; }

	public double Amount { get; set; }

	public double ItemQty { get; set; }

	public double ItemRate { get; set; }

	public int CharOfAccountId { get; set; }

	public int ChargesTypeId { get; set; }

	public int DocNo { get; set; }

	public int Id { get; set; }

	public int InvProductionJobOrderId { get; set; }

	public int JobOrderOverHeadId { get; set; }

	public int DocumentTypeId { get; set; }

	public int PlantId { get; set; }

	public string ohRemarks { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }

	public int EntryUser { get; set; }

	public int ModifyUser { get; set; }

	public int BrandId { get; set; }

	public int BrandUomId { get; set; }

	public int RateUomId { get; set; }

	public int DetailDocumentTypeId { get; set; }

	public int BaseDocumentTypeId { get; set; }

	public int FinancialYearId { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public virtual string OHDetailRowsRemoveIds { get; set; }

	public virtual List<InvFoodProductionOverHeads> InvFoodProductionOverHeadsList { get; set; }
}
