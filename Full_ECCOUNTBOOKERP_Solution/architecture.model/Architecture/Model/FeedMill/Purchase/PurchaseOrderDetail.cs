namespace Architecture.Model.FeedMill.Purchase;

public class PurchaseOrderDetail
{
	public decimal BagPrice { get; set; }

	public decimal BagWeight { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal ItemQty { get; set; }

	public decimal ItemRate { get; set; }

	public decimal NetWeight { get; set; }

	public decimal WeightPerQty { get; set; }

	public decimal Amount { get; set; }

	public decimal TaxAmount { get; set; }

	public decimal TaxPercent { get; set; }

	public decimal TotalAmount { get; set; }

	public int ActionTypeId { get; set; }

	public int CityId { get; set; }

	public int CropYearId { get; set; }

	public int CurrencyId { get; set; }

	public int Id { get; set; }

	public int InvLabSampleAnalysisHeaderId { get; set; }

	public int ItemId { get; set; }

	public int ItemUOMId { get; set; }

	public int JobLotId { get; set; }

	public int LineId { get; set; }

	public int PurchaseOrderId { get; set; }

	public int RateUOMId { get; set; }

	public int TaxNameId { get; set; }

	public string LabSampleNo { get; set; }

	public string Moisture { get; set; }

	public string RemarksDetail { get; set; }

	public string ReviseRemarks { get; set; }

	public string TaxableStatus { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string TaxName { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double RateUomEquivalent { get; set; }

	public virtual string ItemUom { get; set; }

	public virtual double ItemUomEquivalent { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string CropYear { get; set; }

	public virtual string CityName { get; set; }

	public virtual string CurrencyCode { get; set; }
}
