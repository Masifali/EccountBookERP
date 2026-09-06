namespace Architecture.Model.Production;

public class InvProductionJobOrderPlant
{
	public int Id { get; set; }

	public int InvProductionJobOrderId { get; set; }

	public int PlantId { get; set; }

	public decimal AssumedPlantCapacity { get; set; }

	public string RemarksDetail { get; set; }

	public virtual string PlantName { get; set; }
}
