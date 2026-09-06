using System;
using System.Collections.Generic;

namespace Architecture.Model.FeedMill.Production;

public class ProductionElectricityUnits
{
	public DateTime EntryDate { get; set; }

	public DateTime ModifyDate { get; set; }

	public DateTime MonthDate { get; set; }

	public double CostPerBag { get; set; }

	public double CostPerUnit { get; set; }

	public double ElectricityUnits { get; set; }

	public double UnitPerBag { get; set; }

	public double ElectricityCost { get; set; }

	public int ActionId { get; set; }

	public int BranchesId { get; set; }

	public int CompanyId { get; set; }

	public int EntryUserId { get; set; }

	public int FinancialYearId { get; set; }

	public int Id { get; set; }

	public int ModifyUserId { get; set; }

	public string Month { get; set; }

	public int OrganizationId { get; set; }

	public double TotalBagProduced { get; set; }

	public string RemarksHeader { get; set; }

	public virtual List<ProductionElectricityUnits> ProductionElectricityUnitsList { get; set; }
}
