namespace Architecture.Model.Inventory;

public class InvStockTransferExpense
{
	public double Amount { get; set; }

	public double Percentage { get; set; }

	public double Qty { get; set; }

	public double Rate { get; set; }

	public int AccountId { get; set; }

	public int Id { get; set; }

	public int InvStockTransferHeaderId { get; set; }

	public string Remarks { get; set; }
}
