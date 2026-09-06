namespace Architecture.Model.Pcc;

public class BomOperationDetail
{
	public decimal CapacityQty { get; set; }

	public decimal HourlyCost { get; set; }

	public decimal OperationCost { get; set; }

	public int BomHeaderId { get; set; }

	public int Id { get; set; }

	public int OperationHours { get; set; }

	public int WorkStationOperationId { get; set; }

	public string OperationRemarks { get; set; }

	public virtual string WorkStationName { get; set; }

	public virtual string OperationName { get; set; }
}
