namespace Architecture.Model.CmTr.TransactionForm;

public class CommTradeTransactionCommBrokery
{
	public decimal Amount { get; set; }

	public decimal Rate { get; set; }

	public int CalcParameterId { get; set; }

	public int ChargesIncentiveId { get; set; }

	public int CommissionAgentId { get; set; }

	public int CommissionAgentGLId { get; set; }

	public int CommTradeTransactionHeaderId { get; set; }

	public int Id { get; set; }

	public int RevenueExpenseId { get; set; }

	public int SortNo { get; set; }

	public int SupplierCustomerId { get; set; }

	public virtual string CommissionAgent { get; set; }

	public virtual string RevenueExpense { get; set; }

	public virtual string ChargesIncentive { get; set; }

	public virtual string CalculationParameter { get; set; }

	public virtual string SupplierName { get; set; }
}
