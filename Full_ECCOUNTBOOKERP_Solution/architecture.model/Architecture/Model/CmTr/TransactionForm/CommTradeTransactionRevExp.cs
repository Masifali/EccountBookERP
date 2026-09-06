namespace Architecture.Model.CmTr.TransactionForm;

public class CommTradeTransactionRevExp
{
	public decimal CreditAmt { get; set; }

	public decimal DebitAmt { get; set; }

	public decimal Qty { get; set; }

	public decimal Rate { get; set; }

	public int AccountId { get; set; }

	public int CommTradeTransactionHeaderId { get; set; }

	public int Id { get; set; }

	public int LoadingDeliveryId { get; set; }

	public int TypeId { get; set; }

	public int LoadingDeliveryRevExpId { get; set; }

	public int SortNo { get; set; }

	public int SupplierCustomerId { get; set; }

	public string ReRemarks { get; set; }

	public virtual string SupplierName { get; set; }

	public virtual string AccountTitle { get; set; }
}
