using System;

namespace Architecture.Model.Steel.Production;

public class InvProductionJobOrderPackingMaterial
{
	public double Amount { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int Id { get; set; }

	public int InvProductionJobOrderId { get; set; }

	public int ItemId { get; set; }

	public int WarehouseId { get; set; }

	public int itemUomSchId { get; set; }

	public int OrganizationId { get; set; }

	public int CompanyId { get; set; }

	public int BranchesId { get; set; }

	public string pmRemarks { get; set; }

	public int EntryUser { get; set; }

	public int ModifyUser { get; set; }

	public int BrandId { get; set; }

	public int BrandUomId { get; set; }

	public int FinancialYearId { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public virtual string UOMDescription { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string BrandName { get; set; }

	public virtual string BrandUom { get; set; }

	public virtual double Equivalent { get; set; }
}
