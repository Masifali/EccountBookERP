namespace Architecture.Model;

public class DepartmentRequestDetail
{
	public double RequestedQty { get; set; }

	public int DeptReqId { get; set; }

	public int Id { get; set; }

	public int ItemId { get; set; }

	public int ItemUOMId { get; set; }

	public int AssetId { get; set; }

	public int ActionTypeId { get; set; }

	public int WorkOrderId { get; set; }

	public int WorkStationFromId { get; set; }

	public int WorkStationToId { get; set; }

	public int ItemConditionId { get; set; }

	public virtual double Equivalent { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string AssetName { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual string WorkOrderNo { get; set; }

	public virtual string WorkStationFromName { get; set; }

	public virtual string WorkStationToName { get; set; }

	public virtual string ItemCondition { get; set; }
}
