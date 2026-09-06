namespace Architecture.Model.Steel.Inventory;

public class SaleOrderDetail
{
	public bool IsApproved { get; set; }

	public bool IsOrderTaxable { get; set; }

	public decimal ExchangeRate { get; set; }

	public decimal FcyAmount { get; set; }

	public decimal ItemAmount { get; set; }

	public decimal ItemNetAmount { get; set; }

	public decimal NetWeight { get; set; }

	public decimal OrderItemQty { get; set; }

	public decimal OrderItemRate { get; set; }

	public decimal TaxAmount { get; set; }

	public decimal TaxPercent { get; set; }

	public int ActionTypeId { get; set; }

	public int CityId { get; set; }

	public int CurrencyId { get; set; }

	public int Id { get; set; }

	public int InvLabSampleAnalysisHeaderId { get; set; }

	public int JobLotId { get; set; }

	public int OrderItemId { get; set; }

	public int OrderItemRateUOMId { get; set; }

	public int OrderItemUOMId { get; set; }

	public int PackingTypeId { get; set; }

	public int SaleOrderId { get; set; }

	public int TaxNameId { get; set; }

	public string RemarkDetail { get; set; }

	public string TaxRemarks { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual double PackUom { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double EquivalentRate { get; set; }

	public virtual string CityName { get; set; }

	public virtual string TaxName { get; set; }
}
