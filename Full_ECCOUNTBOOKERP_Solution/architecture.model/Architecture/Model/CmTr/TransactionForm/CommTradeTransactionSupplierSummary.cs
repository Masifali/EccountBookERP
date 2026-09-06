namespace Architecture.Model.CmTr.TransactionForm;

public class CommTradeTransactionSupplierSummary
{
	public decimal BillAmount { get; set; }

	public decimal ItemAmount { get; set; }

	public decimal OtherExpenses { get; set; }

	public decimal PackingAmount { get; set; }

	public decimal Weight { get; set; }

	public int CommTradeTransactionHeaderId { get; set; }

	public int Id { get; set; }

	public int SortNo { get; set; }

	public int SupplierId { get; set; }

	public virtual string SupplierName { get; set; }
}
