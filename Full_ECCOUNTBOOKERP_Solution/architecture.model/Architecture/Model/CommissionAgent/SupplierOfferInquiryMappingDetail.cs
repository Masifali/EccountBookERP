namespace Architecture.Model.CommissionAgent;

public class SupplierOfferInquiryMappingDetail
{
	public decimal mappedQty { get; set; }

	public decimal supplierRate { get; set; }

	public int actionTypeId { get; set; }

	public int inquiryDetailId { get; set; }

	public int inquiryMasterId { get; set; }

	public int mappingDetailId { get; set; }

	public int mappingMasterId { get; set; }

	public int supplierOfferDetailId { get; set; }

	public int supplierOfferMasterId { get; set; }

	public string remarks { get; set; }
}
