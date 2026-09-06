namespace Architecture.Model.Inventory;

public class PreBookingOrderDetail
{
	public double Amount { get; set; }

	public double BagPrice { get; set; }

	public double BagWeight { get; set; }

	public double NetWeight { get; set; }

	public double OrderItemQty { get; set; }

	public double OrderItemRate { get; set; }

	public double RetailRate { get; set; }

	public double TaxAmount { get; set; }

	public double TaxPercent { get; set; }

	public double ItemPrice { get; set; }

	public double RateDiscount { get; set; }

	public double ItemDiscount { get; set; }

	public double ItemDiscountAmount { get; set; }

	public double PackingAddLessOnRate { get; set; }

	public double TotalAmount { get; set; }

	public double ExchangeRate { get; set; }

	public double FcyAmount { get; set; }

	public bool CommOnSale { get; set; }

	public int Id { get; set; }

	public int PriceScheduleId { get; set; }

	public int ActionTypeId { get; set; }

	public int JobLotId { get; set; }

	public int OrderItemId { get; set; }

	public int OrderItemRateUOMId { get; set; }

	public int DiscountTypeId { get; set; }

	public int OrderItemUOMId { get; set; }

	public int PreBookingOrderId { get; set; }

	public int TaxNameId { get; set; }

	public int CityId { get; set; }

	public int CostCenterId { get; set; }

	public int PackingTypeID { get; set; }

	public int CurrencyId { get; set; }

	public int ReferencePartyId { get; set; }

	public string AmountCalcType { get; set; }

	public int CropYearId { get; set; }

	public string CityArea { get; set; }

	public string Crop { get; set; }

	public string LabSampleNo { get; set; }

	public string OrderRemarks { get; set; }

	public string TaxableStatus { get; set; }

	public string ItemDiscription { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string DiscountType { get; set; }

	public virtual string ItemCodeNew { get; set; }

	public virtual string TaxName { get; set; }

	public virtual double UOMDescription { get; set; }

	public virtual string UOMCode { get; set; }

	public virtual string RateUom { get; set; }

	public virtual double EquivalentRate { get; set; }

	public virtual string JobLotDescription { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string ReferencePartyName { get; set; }

	public virtual string ImageOne { get; set; }

	public virtual string ImageTwo { get; set; }
}
