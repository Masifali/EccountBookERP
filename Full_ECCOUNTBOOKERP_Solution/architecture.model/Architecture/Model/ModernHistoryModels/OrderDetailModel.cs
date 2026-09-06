namespace Architecture.Model.ModernHistoryModels;

public class OrderDetailModel
{
	public int Id { get; set; }

	public int OrderId { get; set; }

	public string ItemName { get; set; }

	public string PackUom { get; set; }

	public double OrderItemQty { get; set; }

	public double NetWeight { get; set; }

	public double OrderItemRate { get; set; }

	public string RateUom { get; set; }

	public double Amount { get; set; }

	public string JobLotDescription { get; set; }

	public string CityArea { get; set; }

	public string Crop { get; set; }

	public double BagWeight { get; set; }

	public double BagPrice { get; set; }

	public string Moisture { get; set; }

	public int SampleAnalysisNo { get; set; }

	public int LabSampleId { get; set; }

	public string OrderRemarks { get; set; }
}
