namespace Architecture.Model.CommissionAgent;

public class InquiryBookingQualitySpecification
{
	public decimal rangeFrom { get; set; }

	public decimal rangeTo { get; set; }

	public int inquiryBookingMasterId { get; set; }

	public int inquiryBookingQualitySpecificationId { get; set; }

	public int qualityParameterId { get; set; }

	public int sortNo { get; set; }

	public string remarks { get; set; }

	public virtual string QualityParameter { get; set; }
}
