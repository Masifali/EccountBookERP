using System;

namespace Architecture.Model.CommissionAgent;

public class saleOrderPaymentDetail
{
	public DateTime? DueDate { get; set; }

	public decimal dueAmount { get; set; }

	public decimal pctOfTotal { get; set; }

	public int BaseDueDateTypeId { get; set; }

	public int DueDays { get; set; }

	public int PaymentTermId { get; set; }

	public int saleOrderMasterId { get; set; }

	public int saleOrderPaymentDetailId { get; set; }

	public int sortNo { get; set; }

	public string remarks { get; set; }

	public virtual string PaymentTerm { get; set; }

	public virtual string BaseDateType { get; set; }
}
