namespace Architecture.Model.Steel.Production;

public class InvProductionJobOrderPlant
{
	public int Id { get; set; }

	public int InvProductionJobOrderId { get; set; }

	public int PlantId { get; set; }

	public string RemarksDetail { get; set; }

	public int? BranchesId { get; set; }

	public int ActionTypeId { get; set; }

	public virtual string PlantName { get; set; }
}
