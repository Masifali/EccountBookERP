namespace Architecture.Model.Production;

public class InvManufacturingBOMProcessFlow
{
	public double CapacityQty { get; set; }

	public double HourlyCost { get; set; }

	public double OperationCost { get; set; }

	public int Id { get; set; }

	public int InvManufacturingBOMHeaderId { get; set; }

	public int OperationsId { get; set; }

	public int OperationTime { get; set; }

	public int WorkStationId { get; set; }

	public virtual string OperationName { get; set; }

	public virtual string WorkstationName { get; set; }
}
