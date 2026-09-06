namespace Architecture.Model.Mfg;

public class BomOperationDetail
{
	public decimal capacityQty { get; set; }

	public decimal hourlyCost { get; set; }

	public decimal operatingCost { get; set; }

	public int ActionTypeId { get; set; }

	public int bomHeaderId { get; set; }

	public int BomOperationDetailId { get; set; }

	public int sequenceNo { get; set; }

	public int WorkStationOperationId { get; set; }

	public string operationRemarks { get; set; }

	public string operationTime { get; set; }

	public virtual string WorkStationName { get; set; }

	public virtual string OperationName { get; set; }
}
