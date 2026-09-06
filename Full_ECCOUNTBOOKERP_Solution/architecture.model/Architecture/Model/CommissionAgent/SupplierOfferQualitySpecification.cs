namespace Architecture.Model.CommissionAgent;

public class SupplierOfferQualitySpecification
{
	public decimal rangeFrom { get; set; }

	public decimal rangeTo { get; set; }

	public int SupplierOfferMasterId { get; set; }

	public int SupplierOfferQualitySpecificationId { get; set; }

	public int qualityParameterId { get; set; }

	public int sortNo { get; set; }

	public string remarks { get; set; }

	public virtual string QualityParameter { get; set; }
}
