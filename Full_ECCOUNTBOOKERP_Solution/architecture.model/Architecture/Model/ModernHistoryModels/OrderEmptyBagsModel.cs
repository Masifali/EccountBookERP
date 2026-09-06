namespace Architecture.Model.ModernHistoryModels;

public class OrderEmptyBagsModel
{
	public int Id { get; set; }

	public int OrderId { get; set; }

	public string EmptyBagType { get; set; }

	public string OtherItemName { get; set; }

	public string PackTypeDesc { get; set; }

	public double Rate { get; set; }

	public double WeightCut { get; set; }
}
