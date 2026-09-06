using System;

namespace Architecture.Model.CmTr.TransactionForm;

public class CommTradeTransactionPaymentTermsDetail
{
	public DateTime DueDate { get; set; }

	public decimal Amount { get; set; }

	public decimal PrcntOfTotal { get; set; }

	public int CommTradeTransactionHeaderId { get; set; }

	public int DueDays { get; set; }

	public int Id { get; set; }

	public int PaymentTermId { get; set; }

	public int SortNo { get; set; }

	public int SupplierCustomerId { get; set; }

	public int CommTradeLoadingDeliveryId { get; set; }

	public int LoadingDeliveryPaymentDetailId { get; set; }

	public int SupplierCustomerTypeId { get; set; }

	public string PaymentRemarks { get; set; }

	public virtual string SupplierCustomerName { get; set; }

	public virtual string PaymentTerm { get; set; }
}
