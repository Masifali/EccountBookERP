namespace Architecture.Model.Mfg;

public class WorkStationOperation
{
	public int BranchId { get; set; }

	public int CompanyId { get; set; }

	public int WorkStationOperationId { get; set; }

	public int OrganizationId { get; set; }

	public int ProductionOperationId { get; set; }

	public int ProductionWorkStationId { get; set; }

	public int SortNo { get; set; }

	public decimal OpHoursPerDay { get; set; }

	public virtual string ProductionOperation { get; set; }

	public virtual string ProductionWorkStation { get; set; }
}
