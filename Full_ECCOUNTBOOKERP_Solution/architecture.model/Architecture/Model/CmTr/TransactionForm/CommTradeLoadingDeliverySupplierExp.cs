namespace Architecture.Model.CmTr.TransactionForm;

public class CommTradeLoadingDeliverySupplierExp
{
	public decimal Qty { get; set; }

	public decimal ExpenseRate { get; set; }

	public int ExpenseAccountId { get; set; }

	public int CommTradeLoadingDeliveryId { get; set; }

	public int Id { get; set; }

	public int SortNo { get; set; }

	public int SupplierCustomerId { get; set; }

	public string Remarks { get; set; }

	public virtual string SupplierName { get; set; }

	public virtual string AccountTitle { get; set; }
}
