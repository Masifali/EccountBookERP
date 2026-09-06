namespace Architecture.Model.Production;

public class InvProductionJobOrderAndOrderAllocation
{
	public double MTon { get; set; }

	public int Id { get; set; }

	public int InvProductionJobOrderId { get; set; }

	public int ItemId { get; set; }

	public int OrderId { get; set; }

	public int OrderTypeId { get; set; }

	public int ScheduleId { get; set; }

	public string Remarks { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ContractNo { get; set; }

	public virtual string ScheduleNo { get; set; }
}
