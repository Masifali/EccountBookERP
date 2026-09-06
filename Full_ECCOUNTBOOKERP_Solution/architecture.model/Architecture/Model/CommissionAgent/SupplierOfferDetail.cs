namespace Architecture.Model.CommissionAgent;

public class SupplierOfferDetail
{
	public decimal buyerAmount { get; set; }

	public decimal buyerRate { get; set; }

	public decimal itemQty { get; set; }

	public decimal itemWeight { get; set; }

	public decimal SupplierAmount { get; set; }

	public decimal supplierRate { get; set; }

	public int actionTypeId { get; set; }

	public int cropYearId { get; set; }

	public int SupplierOfferDetailId { get; set; }

	public int SupplierOfferMasterId { get; set; }

	public int inventoryParentCategoryId { get; set; }

	public int itemId { get; set; }

	public int packingTypeId { get; set; }

	public int packUomId { get; set; }

	public int rateUomId { get; set; }

	public int sortNo { get; set; }

	public string cropYear { get; set; }

	public string remarks { get; set; }

	public string QualitySpecifications { get; set; }

	public virtual string PackingType { get; set; }

	public virtual string PackUomCode { get; set; }

	public virtual double PackUomEquivalent { get; set; }
}
