using System;
using System.Collections.Generic;

namespace Architecture.Model.Production;

public class InvProductionCumPackMaterialConsumptionHeader
{
	public bool IsApproved { get; set; }

	public DateTime ApprovedDate { get; set; }

	public DateTime DocDate { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public int ApprovedUserId { get; set; }

	public int DocumentTypeId { get; set; }

	public int FinancialYearId { get; set; }

	public int Branchesid { get; set; }

	public int CompanyId { get; set; }

	public int DocNo { get; set; }

	public int EntryUserId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public int OrganizationId { get; set; }

	public int PackSizeId { get; set; }

	public int PackTypeId { get; set; }

	public int ProjectsId { get; set; }

	public int VarietyItemId { get; set; }

	public string CropYear { get; set; }

	public string RemarksMain { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual double Equivalent { get; set; }

	public virtual List<InvProductionCumPackMaterialConsumptionDetail> InvProductionCumPackMaterialConsumptionDetailslist { get; set; }
}
