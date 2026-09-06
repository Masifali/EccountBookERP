namespace Architecture.Model.CommissionAgent;

public class InquiryBookingPartyDetail
{
	public int inquiryBookingPartyDetailId { get; set; }

	public int inquiryBookingMasterId { get; set; }

	public int SubPartyId { get; set; }

	public decimal itemQty { get; set; }

	public decimal Rate { get; set; }

	public int rateUomId { get; set; }

	public decimal Amount { get; set; }

	public int sortNo { get; set; }

	public string remarks { get; set; }

	public int actionTypeId { get; set; }

	public virtual string RateUomCode { get; set; }

	public virtual double RateUomEquivalent { get; set; }

	public virtual string SubPartyName { get; set; }
}
