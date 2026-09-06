using System;

namespace Architecture.Model.CommissionAgent;

public class SupplierOfferPaymentSchedule
{
	public DateTime dueBaseDate { get; set; }

	public decimal dueAmount { get; set; }

	public decimal pctOfTotal { get; set; }

	public int dueDays { get; set; }

	public int SupplierOfferMasterId { get; set; }

	public int SupplierOfferPaymentScheduleId { get; set; }

	public int paymentTermId { get; set; }

	public int sortNo { get; set; }

	public string remarks { get; set; }
}
