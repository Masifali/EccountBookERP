namespace Architecture.Model.Mfg;

public class ProductionWorkStation
{
	public bool isActive { get; set; }

	public double electricityCost { get; set; }

	public double productionCapacity { get; set; }

	public double rentCost { get; set; }

	public double wages { get; set; }

	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int DepartmentId { get; set; }

	public int ProductionWorkStationId { get; set; }

	public int OrganizationId { get; set; }

	public int SortNo { get; set; }

	public string workStationCode { get; set; }

	public string WorkStationName { get; set; }

	public virtual string DepartmentName { get; set; }
}
