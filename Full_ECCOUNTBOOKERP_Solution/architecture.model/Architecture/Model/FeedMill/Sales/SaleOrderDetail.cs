namespace Architecture.Model.FeedMill.Sales;

public class SaleOrderDetail
{
	public bool CommOnSale { get; set; }

	public bool IsApproved { get; set; }

	public double Amount { get; set; }

	public double BagPrice { get; set; }

	public double BagWeight { get; set; }

	public double ExchangeRate { get; set; }

	public double FcyAmount { get; set; }

	public double ItemDiscount { get; set; }

	public double ItemDiscountAmount { get; set; }

	public double ItemPrice { get; set; }

	public double NetWeight { get; set; }

	public double OrderItemQty { get; set; }

	public double OrderItemRate { get; set; }

	public double PackingAddLessOnRate { get; set; }

	public double RateDiscount { get; set; }

	public double RetailRate { get; set; }

	public double TaxAmount { get; set; }

	public double TaxPercent { get; set; }

	public double TotalAmount { get; set; }

	public int ActionTypeId { get; set; }

	public int CityId { get; set; }

	public int CropYearId { get; set; }

	public int CurrencyId { get; set; }

	public int DiscountTypeId { get; set; }

	public int Id { get; set; }

	public int JobLotId { get; set; }

	public int OrderItemId { get; set; }

	public int OrderItemRateUOMId { get; set; }

	public int OrderItemUOMId { get; set; }

	public int PackingTypeId { get; set; }

	public int PriceScheduleId { get; set; }

	public int ReferencePartyId { get; set; }

	public int SaleOrderId { get; set; }

	public int TaxNameId { get; set; }

	public int WarehouseId { get; set; }

	public string ItemDiscription { get; set; }

	public string LabSampleNo { get; set; }

	public string OrderRemarks { get; set; }

	public string TaxableStatus { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string CropYear { get; set; }

	public virtual double PUomEquivalent { get; set; }

	public virtual string PackUom { get; set; }

	public virtual double RUomEquivalent { get; set; }

	public virtual string RateUom { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackTypeDesc { get; set; }

	public virtual string CityName { get; set; }

	public virtual string WareHouseName { get; set; }

	public virtual string TaxName { get; set; }
}
