using System;

namespace Architecture.Model.Import;

public class invoicePaymentTerm
{
	public bool isApproved { get; set; }

	public DateTime approvedOn { get; set; }

	public DateTime createdOn { get; set; }

	public DateTime lastModifiedOn { get; set; }

	public double FcyAmount { get; set; }

	public double pctOfTotal { get; set; }

	public int appActionId { get; set; }

	public int approvedUserId { get; set; }

	public int createdUserId { get; set; }

	public int DueDays { get; set; }

	public long invoiceMasterId { get; set; }

	public long InvoicePaymentTermId { get; set; }

	public int locationBranchId { get; set; }

	public int paymentTermId { get; set; }

	public int DocumentTypeId { get; set; }

	public int SortNo { get; set; }

	public int FinancialInstrumentId { get; set; }

	public long lastModifiedUserId { get; set; }

	public long RowVersionLong { get; set; }

	public string Remarks { get; set; }

	public virtual string PaymentTerm { get; set; }

	public virtual string FinancialInstrumentNo { get; set; }
}
