namespace Architecture.Model.Production;

public class ProdcutionFohAllocateToJobOrderDetail
{
	public double ConsumedFohAmount { get; set; }

	public double ConsumedHours { get; set; }

	public int ActionTypeId { get; set; }

	public int Id { get; set; }

	public int JobOrderId { get; set; }

	public int PlantId { get; set; }

	public int ProdcutionFohAllocateToJobOrderId { get; set; }

	public int SortNo { get; set; }

	public string Remarks { get; set; }

	public virtual string JobOrderNo { get; set; }

	public virtual string PlantName { get; set; }
}
