namespace Architecture.Model.ModernHistoryModels;

public class ContractorWagesBillDirect_Detail
{
	public int HeaderId { get; set; }

	public string ContractorName { get; set; }

	public string WagesItem { get; set; }

	public string ProductItem { get; set; }

	public string ItemDescription { get; set; }

	public string WarehouseFrom { get; set; }

	public string WarehouseTo { get; set; }

	public double PackSize { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public double Amount { get; set; }

	public string RefDocNo { get; set; }

	public string WagesActivityDescription { get; set; }

	public string RemarksDetail { get; set; }
}
