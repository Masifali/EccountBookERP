namespace Architecture.Model.CommissionAgent;

public class saleOrderDetail
{
	public decimal fcyAmount { get; set; }

	public decimal ItemAmount { get; set; }

	public decimal itemQty { get; set; }

	public decimal itemRate { get; set; }

	public decimal itemWeight { get; set; }

	public decimal TaxAmount { get; set; }

	public decimal TaxPercent { get; set; }

	public decimal TotalAmount { get; set; }

	public int actionTypeId { get; set; }

	public int cropYearId { get; set; }

	public int inquiryBookingDetailId { get; set; }

	public int inquiryBookingMasterId { get; set; }

	public int inventoryParentCategoryId { get; set; }

	public int itemId { get; set; }

	public int packingTypeId { get; set; }

	public int packUomId { get; set; }

	public int purchaseOrderDetailId { get; set; }

	public int purchaseOrderMasterId { get; set; }

	public int rateUomId { get; set; }

	public int saleOrderDetailId { get; set; }

	public int saleOrderMasterId { get; set; }

	public int sortNo { get; set; }

	public int supplierOfferDetailId { get; set; }

	public int supplierOfferId { get; set; }

	public int TaxNameId { get; set; }

	public string cropYear { get; set; }

	public string qualitySpecification { get; set; }

	public string remarks { get; set; }

	public virtual string inventoryParentCategory { get; set; }

	public virtual string ItemName { get; set; }

	public virtual string ItemCode { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string PackUomCode { get; set; }

	public virtual double PackUomEquivalent { get; set; }

	public virtual string RateUomCode { get; set; }

	public virtual double RateUomEquivalent { get; set; }

	public virtual string TaxName { get; set; }
}
